using Celestos.Web.BLL;
using OgameServiceV1.Serializable;
using LibCommonUtil;
using OgameServiceV1.DAL;
using OgameServiceV1.WebServiceSecurity.Server;
using System;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace OgameServiceV1
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "https://ogameservicev1.azurewebsites.net")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        public AuthHeader Credentials;

        private Guid AuthenticateClient(string action)
        {
            Guid? userId = AuthHeaderValidation.Validate(action, Credentials);
            if (!userId.HasValue)
                throw new SoapException("Unauthorized", SoapException.ClientFaultCode);
            else
                return userId.Value;
        }

        private string GetClientIpAddress()
        {
            string ipAddress = null;
            try
            {
                if (HttpContext.Current != null)
                {
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
            catch (Exception ex) {

                WebServiceDAL.StoreException("Webservice", "GetClientIpAddress", ex);
            }

            return ipAddress;
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool Ping()
        {
            return true;
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public Guid IsUserAccountValid()
        {
            return AuthenticateClient("IsUserAccountValid");
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public Guid? RegisterApplication(Guid toolId, string computerName)
        {
            //AuthHeaderValidation.CreateSoapHeaderLog("RegisterApplication", toolId, null, null, null, 10, null);

            Guid? applicationKey = WebServiceDAL.GetNewApplicationKey(toolId, computerName);
            
            AuthHeaderValidation.CreateSoapHeaderLog("RegisterApplication", toolId, applicationKey, null, null, 10, null);

            return applicationKey;
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public Int64 StartApplicationSession(Guid applicationKey, Guid toolId, string toolVersion, string computerName)
        {
            try
            {
                if (Credentials != null && !string.IsNullOrEmpty(Credentials.Username) && !string.IsNullOrEmpty(Credentials.Password))
                {
                    Guid userId = AuthenticateClient("StartApplicationSession");
                    return WebServiceDAL.ReCreateApplicationsSession(applicationKey, toolId, toolVersion, userId, computerName, DateTime.UtcNow, DateTime.UtcNow, null);
                }
                else
                {
                    AuthHeaderValidation.CreateSoapHeaderLog("StartApplicationSession", toolId, applicationKey, null, null, 10, null);
                    return WebServiceDAL.CreateApplicationsSession(applicationKey, toolId, toolVersion, computerName, DateTime.UtcNow, DateTime.UtcNow, null);
                }
            }
            catch (Exception ex)
            {
                // Log it
                WebServiceDAL.StoreException("Webservice", "StartApplicationSession", ex);
                return 0;
            }
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool UpdateApplicationSessionUserId(Guid applicationKey, Int64 applicationSessionId, bool isPublicComputer)
        {
            try
            {
                Guid userId = AuthenticateClient("UpdateApplicationSessionUserId");
                WebServiceDAL.UpdateApplicationsSession(applicationKey, applicationSessionId, userId, isPublicComputer);
            }
            catch (Exception ex)
            {
                // Log it
                WebServiceDAL.StoreException("Webservice", "UpdateApplicationSessionUserId", ex);
                return false;
            }

            return true;
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool UpdateApplicationSession(Guid applicationKey, Int64 applicationSessionId, DateTime lastActivity)
        {
            AuthHeaderValidation.CreateSoapHeaderLog("UpdateApplicationSession", Credentials.ToolId, Credentials.ApplicationKey, null, null, 10, null);
            return WebServiceDAL.UpdateApplicationsSession(applicationKey, applicationSessionId, lastActivity, null);
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool EndApplicationSession(Guid applicationKey, Int64 applicationSessionId, DateTime lastActivity, DateTime endTime)
        {
            AuthHeaderValidation.CreateSoapHeaderLog("EndApplicationSession", Credentials.ToolId, Credentials.ApplicationKey, null, null, 10, null);

            WebServiceDAL.UpdateApplicationsSession(applicationKey, applicationSessionId, lastActivity, endTime);

            return true;
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public string SetupApplication(Guid toolId, Guid applicationKey, string prevToolVersion, string newToolVersion)
        {
            Guid userId = AuthenticateClient("SetupApplication");

            SetupAppObj returnObj = new SetupAppObj();
            try
            {
                // Link user to the application
                WebServiceDAL.LinkUserToApplication(applicationKey, userId);
                WebServiceDAL.UpgradeApplicationVersion(toolId, applicationKey, userId, prevToolVersion, newToolVersion);

                returnObj.IsApplicationUserValid = WebServiceDAL.IsApplicationUserValid(applicationKey, userId);
                returnObj.ToolLatestVersion = WebServiceDAL.GetLatestToolVersion(toolId);
                returnObj.IsToolValid = WebServiceDAL.IsToolValid(toolId, newToolVersion);

                if (returnObj.IsApplicationUserValid && returnObj.IsToolValid)
                    returnObj.IsUserAllowedToUseThisTool = WebServiceDAL.IsUserAllowedToUseThisTool(toolId, userId);

                if (returnObj.IsApplicationUserValid && returnObj.IsToolValid && returnObj.IsUserAllowedToUseThisTool)
                {
                    returnObj.CommunityData = WebServiceDAL.GetUserCommunityData(toolId, userId);

                    Int64 encryptionKeyId = WebServiceDAL.GetUserEncryptionKeyId(userId);

                    if (encryptionKeyId == 0)
                    {
                        returnObj.EncryptionKeysExists = WebServiceDAL.CreateUserEncryptionKey(userId, Credentials.Password);
                        encryptionKeyId = WebServiceDAL.GetUserEncryptionKeyId(userId);
                    }
                    else
                        returnObj.EncryptionKeysExists = true;

                    if (encryptionKeyId != 0)
                    {
                        DataTable encryptionKeys = WebServiceDAL.GetUserEncryptionKeys(encryptionKeyId);
                        string serverKey = "";
                        string clientKey = "";
                        try
                        {
                            serverKey = Encryption.DecryptString(UtilitiesBLL.DecryptEncryptionKeyHash(encryptionKeys.Rows[0]["ServerKey"].ToString()), Credentials.Password);
                            clientKey = Encryption.DecryptString(UtilitiesBLL.DecryptEncryptionKeyHash(encryptionKeys.Rows[0]["ClientKey"].ToString()), Credentials.Password);
                        }
                        catch (Exception ex)
                        {
                            WebServiceDAL.StoreException("Webservice", "SetupApplication - Keys encryption", ex);
                            // ServerKey or ClientKey is null/empty or something else went wrong when decrypting
                            // Therefore, create new keys
                            serverKey = UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(RandomPassword.Generate(15, 20), Credentials.Password));
                            clientKey = RandomPassword.Generate(15, 20);
                            returnObj.EncryptionKeysExists = WebServiceDAL.UpdateEncryptionKey(encryptionKeyId, serverKey, UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(clientKey, Credentials.Password)));
                            // Delete all saved passwords
                            WebServiceDAL.DeleteUserUniverseCredentialsPassword(userId);
                            serverKey = null;
                        }
                        if (!string.IsNullOrEmpty(clientKey))
                            returnObj.ClientEncryptionKey = clientKey;
                        else
                            throw new Exception("ClientKey cannot be empty or null");

                        if (!string.IsNullOrEmpty(serverKey))
                            returnObj.CredentialsList = WebServiceDAL.GetUserUniversesAccounts(userId, serverKey, clientKey);
                    }
                }
            }
            catch (Exception ex)
            {
                WebServiceDAL.StoreException("Webservice", "SetupApplication", ex);
                returnObj.Error = true;
                returnObj.ErrorMessage = ex.Message;
            }

            return SerializeDeserializeObject.SerializeObject<SetupAppObj>(returnObj); ;
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool UpgradeApplicationVersion(Guid toolId, Guid applicationKey, string prevToolVersion, string newToolVersion)
        {
            Guid userId = AuthenticateClient("UpgradeApplicationVersion");
            try
            {
                WebServiceDAL.UpgradeApplicationVersion(toolId, applicationKey, userId, prevToolVersion, newToolVersion);
            }
            catch (Exception ex)
            {
                // log it
                WebServiceDAL.StoreException("Webservice", "UpgradeApplicationVersion", ex);
                throw ex;
            }
            return true;
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public Guid GetUserId()
        {
            return AuthenticateClient("GetUserId");
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool IsToolValid(Guid toolId, string toolVersion)
        {
            AuthenticateClient("IsToolValid");
            return WebServiceDAL.IsToolValid(toolId, toolVersion);
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool IsUserAllowedToUseThisTool(Guid toolId)
        {
            Guid userId = AuthenticateClient("IsUserAllowedToUseThisTool");
            return WebServiceDAL.IsUserAllowedToUseThisTool(toolId, userId);
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public string GetLatestToolVersion(Guid toolId)
        {
            AuthenticateClient("GetLatestToolVersion");
            return WebServiceDAL.GetLatestToolVersion(toolId);
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public string GetUserCommunityData(Guid toolId)
        {
            Guid userId = AuthenticateClient("GetUserCommunityData");
            return WebServiceDAL.GetUserCommunityData(toolId, userId);
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool CreateApplicationExceptionLog(Guid applicationKey, Guid toolId, string type, string description, string message, string stack, string innerExceptionMessage)
        {
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(stack))
                        stack = Encryption.DecryptString(stack);
                }
                catch { }

                if (Credentials != null && !string.IsNullOrEmpty(Credentials.Username) && !string.IsNullOrEmpty(Credentials.Password))
                {
                    try
                    {
                        Guid userId = AuthenticateClient("CreateApplicationExceptionLog");
                        return WebServiceDAL.InsertApplicationsExceptionLog(applicationKey, toolId, userId, type, description, message, stack, innerExceptionMessage);
                    }
                    catch (Exception ex)
                    {
                        WebServiceDAL.StoreException("Webservice", "CreateApplicationExceptionLog Inside 1st Try/Catch", ex);
                        AuthHeaderValidation.CreateSoapHeaderLog("CreateApplicationExceptionLog", toolId, applicationKey, null, null, 10, null);
                        return WebServiceDAL.InsertApplicationsExceptionLog(applicationKey, toolId, null, type, description, message, stack, innerExceptionMessage);
                    }
                }
                else
                {
                    AuthHeaderValidation.CreateSoapHeaderLog("CreateApplicationExceptionLog", toolId, applicationKey, null, null, 10, null);
                    return WebServiceDAL.InsertApplicationsExceptionLog(applicationKey, toolId, null, type, description, message, stack, innerExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                // Log it
                WebServiceDAL.StoreException("Webservice", "CreateApplicationExceptionLog", ex);
                return false;
            }
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool CreateApplicationWebClientExceptionLog(Guid applicationKey, Guid toolId, string type, string url, string description, string message, string stack, string innerExceptionMessage)
        {
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(stack))
                        stack = Encryption.DecryptString(stack);
                }
                catch { }

                if (Credentials != null && !string.IsNullOrEmpty(Credentials.Username) && !string.IsNullOrEmpty(Credentials.Password))
                {
                    try
                    {
                        Guid userId = AuthenticateClient("CreateWebClientExceptionLog");
                        return WebServiceDAL.InsertApplicationsWebClientExceptionLog(applicationKey, toolId, userId, type, url, description, message, stack, innerExceptionMessage);
                    }
                    catch (Exception ex)
                    {
                        AuthHeaderValidation.CreateSoapHeaderLog("CreateWebClientExceptionLog", toolId, applicationKey, null, null, 10, null);
                        return WebServiceDAL.InsertApplicationsWebClientExceptionLog(applicationKey, toolId, null, type, url, description, message, stack, innerExceptionMessage);
                    }
                }
                else
                {
                    AuthHeaderValidation.CreateSoapHeaderLog("CreateWebClientExceptionLog", toolId, applicationKey, null, null, 10, null);
                    return WebServiceDAL.InsertApplicationsWebClientExceptionLog(applicationKey, toolId, null, type, url, description, message, stack, innerExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                // Log it
                WebServiceDAL.StoreException("Webservice", "CreateApplicationWebClientExceptionLog", ex);
                return false;
            }
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool LinkUserAccountToUniverseAccount(Guid universeId, Int64 playerId, string playerName)
        {
            Guid userId = AuthenticateClient("LinkUserAccountToUniverseAccount");
            try
            {
                return WebServiceDAL.LinkUserToUniverseAccount(userId, universeId, playerId, playerName, "DO_NOT_UPDATE_PASSWORD");
            }
            catch (Exception ex)
            {
                // Log it
                WebServiceDAL.StoreException("Webservice", "LinkUserAccountToUniverseAccount", ex);
                return false;
            }
        }

        [SoapHeader("Credentials")]
        [WebMethod(Description = "")]
        public bool SynchronizeCredentials(Guid universeId, Int64 playerId, string playerName, string password)
        {
            Guid userId = AuthenticateClient("SynchronizeCredentials");
            try
            {
                if (string.IsNullOrEmpty(password))
                    return WebServiceDAL.LinkUserToUniverseAccount(userId, universeId, playerId, playerName, password);
                else
                {
                    Int64 encryptionKeyId = WebServiceDAL.GetUserEncryptionKeyId(userId);
                    if (encryptionKeyId != 0)
                    {
                        DataTable encryptionKeys = WebServiceDAL.GetUserEncryptionKeys(encryptionKeyId);

                        string serverKey = Encryption.DecryptString(UtilitiesBLL.DecryptEncryptionKeyHash(encryptionKeys.Rows[0]["ServerKey"].ToString()), Credentials.Password);
                        string clientKey = Encryption.DecryptString(UtilitiesBLL.DecryptEncryptionKeyHash(encryptionKeys.Rows[0]["ClientKey"].ToString()), Credentials.Password);

                        password = Encryption.EncryptString(Encryption.DecryptString(password, clientKey), serverKey);
                        return WebServiceDAL.LinkUserToUniverseAccount(userId, universeId, playerId, playerName, password);
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Log it
                WebServiceDAL.StoreException("Webservice", "SynchronizeCredentials", ex);
                return false;
            }
        }
    }
}
