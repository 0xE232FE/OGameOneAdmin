using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Collections.Generic;
using LibCommonUtil;
using OgameServiceV1.DAL;


namespace OgameServiceV1.WebServiceSecurity.Server
{
    /***********************************************************************************************************/


    /// <summary>
    /// Extends from SoapHeader and provides a username / password for web method that need authentication.
    /// </summary>
    sealed public class AuthHeader : SoapHeader
    {
        public Guid? ToolId;
        public Guid? ApplicationKey;
        public string Username;
        public string Password;
    }


    /***********************************************************************************************************/


    /// <summary>
    /// Create a static method to process SoapHeader for authentication.
    /// First check that some type of credentials were passed in the SoapHeader
    /// and that the properties are not null.
    /// Finally, to validate each property to see if it contains the desired information.
    /// </summary
    sealed public class AuthHeaderValidation
    {
        /// <summary>
        /// Validates the credentials of the soap header.
        /// If credentials are valid, return the userId or throw an exception.
        /// </summary>
        /// <returns></returns>
        public static Guid? Validate(string action, AuthHeader soapHeader)
        {
            Guid? userId = null;

            if (soapHeader == null)
            {
                CreateSoapHeaderLog(action, null, null, null, null, 1, null);
                throw new NullReferenceException("No soap header was specified.");
            }
            else if (soapHeader.Username == null)
            {
                CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, null, 2, null);
                throw new NullReferenceException("Username was not supplied for authentication in SoapHeader.");
            }
            else if (soapHeader.Password == null)
            {
                CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, soapHeader.Username, 3, null);
                throw new NullReferenceException("Password was not supplied for authentication in SoapHeader.");
            }
            else // Verify Credentials
            {
                bool isValid = false;

                try
                {
                    isValid = AreCredentialsValid(out userId, soapHeader.Username, soapHeader.Password);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Equals("invalid username"))
                        CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, soapHeader.Username, 4, null);
                    else if (ex.Message.Equals("account is not approved"))
                        CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, soapHeader.Username, 5, null);
                    else if (ex.Message.Equals("account is locked"))
                        CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, soapHeader.Username, 6, null);
                    else if (ex.Message.Equals("wrong password"))
                        CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, soapHeader.Username, 7, null);
                    else
                    {
                        // Log it
                        CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, soapHeader.Username, 8, ex.Message);
                    }

                    throw ex;
                }

                if (isValid)
                {
                    CreateSoapHeaderLog(action, soapHeader.ToolId, soapHeader.ApplicationKey, userId, soapHeader.Username, 0, null);
                    return userId;
                }
                else
                    throw new SoapException("Unauthorized", SoapException.ClientFaultCode);
            }
        }

        public static bool AreCredentialsValid(out Guid? userId, string userName, string password)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT B.UserId, Password, PasswordSalt, IsApproved, IsLockedOut FROM aspnet_Membership A " +
                                  "INNER JOIN aspnet_Users B on A.UserId = B.UserId WHERE B.UserName = @UserName";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserName", SqlDbType.NVarChar, userName));

                DataSet ds = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    userId = new Guid(row["UserId"].ToString());
                    string encryptedPassword = row["Password"].ToString();
                    string passwordSalt = row["PasswordSalt"].ToString();
                    bool isApproved = (bool)row["IsApproved"];
                    bool isLocked = (bool)row["IsLockedOut"];

                    if (EncodePassword(password, passwordSalt).Equals(encryptedPassword))
                    {
                        if (!isApproved)
                            throw new Exception("account is not approved");
                        else if (isLocked)
                            throw new Exception("account is locked");
                        else
                            return true;
                    }
                    else
                        throw new Exception("wrong password");
                }
                else
                {
                    throw new Exception("invalid username");
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("database problem");
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        private static string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            byte[] inArray = null;
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create(Membership.HashAlgorithmType);
            if (algorithm != null)
            {
                inArray = algorithm.ComputeHash(dst);
                return Convert.ToBase64String(inArray);
            }
            else
                throw new Exception("Encode Password has failed");
        }

        public static void CreateSoapHeaderLog(string action, Guid? toolId, Guid? applicationKey, Guid? userId, string userName, int status, string error)
        {
            // status = 0 -> credentials valid
            // status = 1 -> soapHeader == null
            // status = 2 -> soapHeader.Username == null
            // status = 3 -> soapHeader.Password == null
            // status = 4 -> invalid username
            // status = 5 -> account is not approved
            // status = 6 -> account is locked
            // status = 7 -> wrong password
            // status = 8 -> ValidateCredentials sql exception
            // status = 9 -> Could not find soap header
            // status = 10 -> soapheader not required

            SqlDataConnector oDC = null;

            try
            {
                bool https = false;
                string server = null;
                string url = null;
                string ipAddress = null;
                try
                {
                    if (HttpContext.Current != null)
                    {
                        try
                        {
                            https = HttpContext.Current.Request.Url.Scheme.ToLower().Equals("https") ? true : false;
                            server = HttpContext.Current.Request.Url.Authority;
                            url = HttpContext.Current.Request.Url.AbsoluteUri;
                        }
                        catch { }

                        try
                        {
                            ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                        }
                        catch
                        {
                        }

                        try
                        {
                            if (string.IsNullOrEmpty(ipAddress))
                                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        }
                        catch { }

                        if (string.IsNullOrEmpty(ipAddress))
                            ipAddress = HttpContext.Current.Request.UserHostAddress;
                    }
                }
                catch { }

                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_SoapHeaderLog " +
                                  "(DateTime, Https, Server, Url, Action, IpAddress, ToolId, ApplicationKey, UserId, UserName, Status, Error)" +
                                  "VALUES (GETUTCDATE(), @Https, @Server, @Url, @Action, @IpAddress, @ToolId, @ApplicationKey, @UserId, @UserName, @Status, @Error)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@Https", SqlDbType.Bit, https));
                cmdParameters.Add(oDC.CreateInputParam("@Server", SqlDbType.NVarChar, server));
                cmdParameters.Add(oDC.CreateInputParam("@Url", SqlDbType.NVarChar, url));
                cmdParameters.Add(oDC.CreateInputParam("@Action", SqlDbType.NVarChar, action));
                cmdParameters.Add(oDC.CreateInputParam("@IpAddress", SqlDbType.NVarChar, ipAddress));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@UserName", SqlDbType.NVarChar, userName));
                cmdParameters.Add(oDC.CreateInputParam("@Status", SqlDbType.Int, status));
                cmdParameters.Add(oDC.CreateInputParam("@Error", SqlDbType.NVarChar, error));

                int result = oDC.ExecNonQuerybyQuery(sqlQuery, cmdParameters.ToArray());

                if (result != 1)
                {
                    // log it
                }

            }
            catch (Exception ex)
            {
                // log it
                WebServiceDAL.StoreException("Webservice", "CreateSoapHeaderLog", ex);
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool UpdateLastSoapHeaderLog(Guid toolId, Guid applicationKey, Guid userId, string userName, string action)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "UPDATE db_SoapHeaderLog SET ApplicationKey = @ApplicationKey, UserId = @UserId WHERE Id IN (SELECT TOP 1 Id FROM db_SoapHeaderLog WHERE Action = @Action AND UserName = @UserName AND ToolId = @ToolId ORDER BY Id DESC)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@Action", SqlDbType.NVarChar, action));
                cmdParameters.Add(oDC.CreateInputParam("@UserName", SqlDbType.NVarChar, userName));

                int result = oDC.ExecNonQuerybyQuery(sqlQuery, cmdParameters.ToArray());

                if (result != 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                // log it
                WebServiceDAL.StoreException("Webservice", "UpdateLastSoapHeaderLog", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }
    }


    /***********************************************************************************************************/


    /// <summary>
    /// Custom SoapExtension that authenticates the method being called.
    /// </summary>
    sealed public class AuthenticationSoapExtension : SoapExtension
    {
        /***********************************************************************************************************/


        #region ------ Define Private Variables ------


        private bool encryptMessage = true;
        private bool compressMessage = true;

        private string sPassPhrase = "$Celestos - RIjnda3l Symm3trIc CiphEr!";    // can be any string
        private string sSaltValue = "$OGame - Best Online Browser Game!";         // can be any string
        private string sHashAlgorithm = "SHA1";                                   // can be "MD5"
        private int iPasswordIterations = 5;                                      // can be any number
        private string sInitVector = "$Yel6yeZar_?Bds!";                          // must be 16 bytes
        private int iKeySize = 256;                                               // can be 192 or 128

        private Stream oldStream;
        private Stream newStream;

        private string filename;


        #endregion ------ Define Private Variables ------


        /***********************************************************************************************************/


        #region ------ Private Methods ------


        private void Compress(SoapMessage message)
        {
            if (compressMessage)
            {
                MemoryStream ms = new MemoryStream();
                newStream.Position = 0;
                CopyBinaryStream(newStream, ms);
                ms.Position = 0;
                ms = ZipUtil.CompressStream(ms);
                ms.Position = 0;
                newStream = new MemoryStream();
                CopyBinaryStream(ms, newStream);
            }
        }


        private void Encrypt(SoapMessage message)
        {
            if (encryptMessage)
            {
                MemoryStream ms = new MemoryStream();
                newStream.Position = 0;
                CopyBinaryStream(newStream, ms);
                ms.Position = 0;
                byte[] compressedBytes = ms.ToArray();
                RijndaelEnhanced rj = new RijndaelEnhanced(sPassPhrase, sInitVector);
                byte[] encryptedBytes = rj.EncryptToBytes(compressedBytes);
                newStream.Position = 0;
                BinaryWriter binaryWriter = new BinaryWriter(newStream);
                binaryWriter.Write(encryptedBytes);
                binaryWriter.Flush();

                newStream.Position = 0;
                CopyBinaryStream(newStream, oldStream);
            }
            else
            {
                newStream.Position = 0;
                CopyBinaryStream(newStream, oldStream);
            }
        }


        /// <summary>
        /// Decrypt the SOAP Stream BeforeSerialize.
        /// </summary>
        /// <param name="message"></param>
        private void Decrypt(SoapMessage message)
        {
            if (encryptMessage)
            {
                CopyBinaryStream(oldStream, newStream);
                MemoryStream ms = new MemoryStream();
                newStream.Position = 0;
                CopyBinaryStream(newStream, ms);
                ms.Position = 0;
                byte[] encryptedBytes = ms.ToArray();
                RijndaelEnhanced rj = new RijndaelEnhanced(sPassPhrase, sInitVector);

                if (compressMessage)
                {
                    byte[] compressedBytes = rj.DecryptToBytes(encryptedBytes);
                    newStream.Position = 0;
                    BinaryWriter binaryWriter = new BinaryWriter(newStream);
                    binaryWriter.Write(compressedBytes);
                    binaryWriter.Flush();
                }
                else
                {
                    string soapMessage = rj.Decrypt(encryptedBytes); ;
                    newStream.Position = 0;
                    StreamWriter streamWriter = new StreamWriter(newStream);

                    // Clear all newStream contents with WhiteSpace before overwrite
                    for (int i = 0; i < newStream.Length; i++)
                    {
                        streamWriter.Write(" ");
                        streamWriter.Flush();
                    }

                    newStream.Position = 0;
                    streamWriter.Write(soapMessage);
                    streamWriter.Flush();
                }

                newStream.Position = 0;
            }
        }


        private void DeCompress(SoapMessage message)
        {
            if (!encryptMessage)
            {
                CopyBinaryStream(oldStream, newStream);
                newStream.Position = 0;
            }

            if (compressMessage)
            {
                newStream.Position = 0;
                Stream ms = new MemoryStream();
                CopyBinaryStream(newStream, ms);
                ms.Position = 0;
                ms = ZipUtil.DeCompressStream(ms);
                ms.Position = 0;
                newStream.Position = 0;
                CopyBinaryStream(ms, newStream);
                newStream.Position = 0;
            }
        }


        /// <summary>
        /// Write output encrypted message AfterSerialize to file.
        /// </summary>
        /// <param name="message"></param>
        private void WriteOutput(SoapMessage message)
        {
            newStream.Position = 0;

            FileStream fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            string soapString = (message is SoapServerMessage) ? "SoapResponse" : "SoapRequest";

            streamWriter.WriteLine();
            streamWriter.WriteLine("------ " + soapString + " at " + DateTime.Now.ToString() + " ------");
            streamWriter.Flush();

            CopyBinaryStream(newStream, fileStream);

            streamWriter.Close();
        }


        /// <summary>
        /// Write output decrypted message BeforeSerialize to file.
        /// </summary>
        /// <param name="message"></param>
        private void WriteInput(SoapMessage message)
        {
            newStream.Position = 0;

            FileStream fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            string soapString = (message is SoapClientMessage) ? "SoapResponse" : "SoapRequest";

            streamWriter.WriteLine();
            streamWriter.WriteLine("------ " + soapString + " at " + DateTime.Now.ToString() + " ------");
            streamWriter.Flush();

            newStream.Position = 0;

            CopyBinaryStream(newStream, fileStream);

            streamWriter.Close();
        }


        /// <summary>
        /// Copy from Stream to Stream using Binary Reader/Writer
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void CopyBinaryStream(Stream from, Stream to)
        {
            int bytesRead;
            byte[] buffer = new byte[2];
            BinaryReader reader = new BinaryReader(from);
            BinaryWriter writer = new BinaryWriter(to);

            do
            {
                bytesRead = reader.Read(buffer, 0, buffer.Length);
                writer.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            writer.Flush();
        }


        /// <summary>
        /// Copy from Stream to Stream using Text Reader/Writer
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void CopyTextStream(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }


        #endregion ------ Private Methods ------


        /***********************************************************************************************************/


        #region ------ Public Overide Methods ------


        /// <summary>
        /// Chaining the Message Stream.
        /// Save the Stream representing the SOAP request or SOAP response into a local memory buffer.
        /// Overriding the ChainStream method to hook into the message stream.
        /// When an extension is installed, ASP.NET calls ChainStream,
        /// passing in a reference to the stream containing the SOAP message.
        /// The main goal is to save the stream containing the SOAP message.
        /// This implementation also creates a new stream for holding a working copy of the message.
        /// The Stream passed into ChainStream contains the serialized SOAP request.
        /// SOAP extensions work by writing into a stream.
        /// Notice that ChainStream creates a memory stream and passes it back to the caller.
        /// The stream returned by ChainStream contains the serialized SOAP response.
        /// </summary>
        public override Stream ChainStream(Stream stream)
        {
            // Save the message stream in a member variable.
            oldStream = stream;

            // Create a new stream and save it as a member variable.
            newStream = new MemoryStream();
            return newStream;
        }


        /// <summary>
        /// When the SOAP extension is accessed for the first time,
        /// the XML Web service method store the file name passed in using the corresponding SoapExtensionAttribute. 
        /// </summary>  
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return ((AuthenticationSoapExtensionAttribute)attribute).Filename;
        }


        /// <summary>
        /// The SOAP extension was configured to run using a configuration file
        /// instead of an attribute applied to a specific Web service method.
        /// </summary>
        public override object GetInitializer(Type WebServiceType)
        {
            // Return a file name to log the trace information to, based on the type.
            return "D:\\Stephane\\My Dropbox\\Ogame\\Tools\\Visual Studio 2008\\New Tools\\" + WebServiceType.FullName + "_SoapLog.txt";
        }


        /// <summary>
        /// Receive the file name stored by GetInitializer and
        /// store it in a member variable for this specific instance.
        /// </summary> 
        public override void Initialize(object initializer)
        {
            filename = (string)initializer;
        }


        /// <summary>
        /// Process the soap (encrypt / decrypt).
        /// If the SoapMessageStage is such that the SoapRequest or SoapResponse
        /// is still in the SOAP format to be sent or received, save it out to a file.
        /// </summary>
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    Compress(message);
                    Encrypt(message);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    Decrypt(message);
                    DeCompress(message);
                    break;
                case SoapMessageStage.AfterDeserialize:
                    //Check for an AuthHeader containing valid credentials
                    //foreach (SoapHeader header in message.Headers)
                    //{
                    //    if (header is AuthHeader)
                    //    {
                    //        AuthHeader credentials = (AuthHeader)header;

                    //        if (AuthHeaderValidation.Validate(message.Action, credentials))
                    //            return;
                    //        break;
                    //    }
                    //}
                    //// Fail the call if we get to here. Either the header
                    //// isn't there or it contains invalid credentials.
                    //AuthHeaderValidation.AddSoapHeaderLog(message.Action, null, null, null, null, 9, null);
                    //throw new SoapException("Unauthorized", SoapException.ClientFaultCode);
                    break;
                default:
                    throw new Exception("Invalid Soap Message Stage.");
            }
        }


        #endregion ------ Public Overide Methods ------


    }


    /***********************************************************************************************************/


    /// <summary>
    /// Custom SoapExtensionAttribute.
    /// In order to have the method call to custom SoapExtension, an object need to be created
    /// that extends SoapExtensionAttribute. It is a farily simple class with two overridden properties.
    /// </summary
    [AttributeUsage(AttributeTargets.Method)]
    sealed public class AuthenticationSoapExtensionAttribute : SoapExtensionAttribute
    {
        private string filename = "";
        private int priority;

        public override Type ExtensionType
        {
            get { return typeof(AuthenticationSoapExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
            }
        }
    }


    /***********************************************************************************************************/
}