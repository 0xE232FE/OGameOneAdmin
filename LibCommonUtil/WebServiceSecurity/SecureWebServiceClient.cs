using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO.Compression;

namespace LibCommonUtil.WebServiceSecurity.Client
{
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
            return null;
        }


        /// <summary>
        /// The SOAP extension was configured to run using a configuration file
        /// instead of an attribute applied to a specific Web service method.
        /// </summary>
        public override object GetInitializer(Type WebServiceType)
        {
            return GetType();
        }


        /// <summary>
        /// Receive the file name stored by GetInitializer and
        /// store it in a member variable for this specific instance.
        /// </summary> 
        public override void Initialize(object initializer)
        {
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
    }


    /***********************************************************************************************************/
}
