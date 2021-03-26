using Celestos.Web.BLL;
using Celestos.Web.Static;
using OgameServiceV1.Serializable;
using LibCommonUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OgameServiceV1.DAL
{
    internal static class WebServiceDAL
    {
        private static string GetClientIpAddress()
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
            catch { }

            return ipAddress;
        }

        public static Guid? GetNewApplicationKey(Guid toolId, string computerName)
        {
            SqlDataConnector oDC = null;
            Guid? applicationKey = Guid.NewGuid();

            try
            {
                string ipAddress = GetClientIpAddress();

                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_Applications ([Key], ToolId, ComputerName, IsActive, StartTimeStamp, EndTimeStamp, AddUserId, ModUserId, AddDateTime, ModDateTime, AddIpAddress, ModIpAddress) " +
                                  "VALUES (@ApplicationKey, @ToolId, @ComputerName, 1, GETUTCDATE(), NULL, NULL, NULL, GETUTCDATE(), NULL, @IpAddress, NULL)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey.Value));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@ComputerName", SqlDbType.NVarChar, computerName));
                cmdParameters.Add(oDC.CreateInputParam("@IpAddress", SqlDbType.NVarChar, ipAddress));

                int result = oDC.ExecNonQuerybyQuery(sqlQuery, cmdParameters.ToArray());

                if (result != 1)
                {
                    applicationKey = null;
                }
            }
            catch (Exception ex)
            {
                // log it
                StoreException("Webservice", "GetNewApplicationKey", ex);
                applicationKey = null;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }

            return applicationKey;
        }

        public static Guid GetUserId(string userName)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT UserId FROM aspnet_Users " +
                                  "WHERE UserName = @UserName";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserName", SqlDbType.NVarChar, userName));

                DataSet ds = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    Guid userId = (Guid)row["UserId"];
                    return userId;
                }
                else
                    throw new Exception("invalid username");
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "GetUserId", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static void LinkUserToApplication(Guid applicationKey, Guid userId)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));

                oDC.ExecNonQuerybySP("LinkUserToApplication", cmdParameters.ToArray());
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "LinkUserToApplication", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static void UpgradeApplicationVersion(Guid toolId, Guid applicationKey, Guid userId, string prevToolVersion, string newToolVersion)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@PrevToolVersion", SqlDbType.NVarChar, prevToolVersion));
                cmdParameters.Add(oDC.CreateInputParam("@NewToolVersion", SqlDbType.NVarChar, newToolVersion));

                oDC.ExecNonQuerybySP("UpgradeApplicationVersion", cmdParameters.ToArray());
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "UpgradeApplicationVersion", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool CreateUserEncryptionKey(Guid userId, string password)
        {
            string serverKey = UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(RandomPassword.Generate(15, 20), password));
            string clientKey = UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(RandomPassword.Generate(15, 20), password));
            string userIdEncrypted = UtilitiesBLL.CreateDatabaseHash(userId.ToString());

            Int64 userEncryptionKeyId = InsertUserEncryptionKey(serverKey, clientKey);

            if (userEncryptionKeyId != 0)
                return LinkUserToEncryptionKey(userEncryptionKeyId, userIdEncrypted);
            else
                return false;
        }

        public static bool UpdateUserEncryptionKey(Guid userId, string password, string newPassword)
        {
            Int64 encryptionKeyId = GetUserEncryptionKeyId(userId);

            if (encryptionKeyId == 0)
            {
                return CreateUserEncryptionKey(userId, newPassword);
            }
            else
            {
                DataTable encryptionKeys = GetUserEncryptionKeys(encryptionKeyId);
                string serverKey = "";
                string clientKey = "";
                try
                {
                    serverKey = Encryption.DecryptString(UtilitiesBLL.DecryptEncryptionKeyHash(encryptionKeys.Rows[0]["ServerKey"].ToString()), password);
                    clientKey = Encryption.DecryptString(UtilitiesBLL.DecryptEncryptionKeyHash(encryptionKeys.Rows[0]["ClientKey"].ToString()), password);
                    serverKey = UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(serverKey, newPassword));
                    clientKey = UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(clientKey, newPassword));
                }
                catch (Exception ex)
                {
                    StoreException("Webservice", "UpdateUserEncryptionKey", ex);
                    // ServerKey or ClientKey is null/empty or something else went wrong when decrypting
                    // Therefore, create new keys
                    serverKey = UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(RandomPassword.Generate(15, 20), newPassword));
                    clientKey = UtilitiesBLL.CreateEncryptionKeyHash(Encryption.EncryptString(RandomPassword.Generate(15, 20), newPassword));
                    // Delete all saved passwords
                    DeleteUserUniverseCredentialsPassword(userId);
                }
                return UpdateEncryptionKey(encryptionKeyId, serverKey, clientKey);
            }
        }

        public static bool UpdateEncryptionKey(Int64 encryptionKeyId, string serverKey, string clientKey)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "UPDATE db_EncryptionKeys SET ServerKey = @ServerKey, ClientKey = @ClientKey WHERE Id = @EncryptionKeyId";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@EncryptionKeyId", SqlDbType.BigInt, encryptionKeyId));
                cmdParameters.Add(oDC.CreateInputParam("@ServerKey", SqlDbType.NVarChar, serverKey));
                cmdParameters.Add(oDC.CreateInputParam("@ClientKey", SqlDbType.NVarChar, clientKey));

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
                StoreException("Webservice", "UpdateEncryptionKey", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static Int64 InsertUserEncryptionKey(string serverKey, string clientKey)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_EncryptionKeys " +
                                  "(ServerKey, ClientKey, IsActive, StartTimeStamp, EndTimeStamp, AddDateTime, ModDateTime)" +
                                  "VALUES (@ServerKey, @ClientKey, 1, GETUTCDATE(), NULL, GETUTCDATE(), NULL); " +
                                  "SELECT SCOPE_IDENTITY() AS Id";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ServerKey", SqlDbType.NVarChar, serverKey));
                cmdParameters.Add(oDC.CreateInputParam("@ClientKey", SqlDbType.NVarChar, clientKey));

                object result = oDC.ExecScalarbyQuery(sqlQuery, cmdParameters.ToArray());

                if (result != null)
                {
                    return Int64.Parse(result.ToString());
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // log it
                StoreException("Webservice", "InsertUserEncryptionKey", ex);
                return 0;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool LinkUserToEncryptionKey(Int64 userEncryptionKeyId, string userIdEncrypted)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_EncryptionKeys_Users " +
                                  "(EncryptionKeyId, UserIdHash, IsActive, StartTimeStamp, EndTimeStamp, AddDateTime, ModDateTime)" +
                                  "VALUES (@EncryptionKeyId, @UserIdHash, 1, GETUTCDATE(), NULL, GETUTCDATE(), NULL)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@EncryptionKeyId", SqlDbType.BigInt, userEncryptionKeyId));
                cmdParameters.Add(oDC.CreateInputParam("@UserIdHash", SqlDbType.NVarChar, userIdEncrypted));

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
                StoreException("Webservice", "LinkUserToEncryptionKey", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static Int64 GetUserEncryptionKeyId(Guid userId)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT EncryptionKeyId FROM db_EncryptionKeys_Users WHERE UserIdHash = @UserIdHash";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserIdHash", SqlDbType.NVarChar, UtilitiesBLL.CreateDatabaseHash(userId.ToString())));

                object returnObj = oDC.ExecScalarbyQuery(sqlQuery, cmdParameters.ToArray());

                if (returnObj != null)
                    return (Int64)returnObj;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "GetUserEncryptionKeyId", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static DataTable GetUserEncryptionKeys(Int64 userEncryptionKeyId)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT * FROM db_EncryptionKeys WHERE Id = @UserEncryptionKeyId";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserEncryptionKeyId", SqlDbType.BigInt, userEncryptionKeyId));

                DataSet resultDs = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (resultDs != null && resultDs.Tables.Count == 1 && resultDs.Tables[0].Rows.Count == 1)
                    return resultDs.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "GetUserEncryptionKeys", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        internal static bool DeleteUserUniverseCredentialsPassword(Guid userId)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "UPDATE db_Users_UniversesAccounts SET [Password] = NULL, ModDateTime = GETUTCDATE(), ModUserId = @UserId WHERE UserId = @UserId";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));

                int result = oDC.ExecNonQuerybyQuery(sqlQuery, cmdParameters.ToArray());

                if (result != 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                //Log it
                StoreException("Webservice", "DeleteUserUniverseCredentialsPassword", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static List<Credentials> GetUserUniversesAccounts(Guid userId, string serverKey, string clientKey)
        {
            List<Credentials> credentialsList = new List<Credentials>();
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT UniversesAccounts.UniverseId, UniversesAccounts.PlayerId, UniversesAccounts.PlayerName, UsersUniversesAccounts.Password, UsersUniversesAccounts.AddDateTime, UsersUniversesAccounts.ModDateTime FROM db_UniversesAccounts UniversesAccounts " +
                                  "INNER JOIN db_Users_UniversesAccounts UsersUniversesAccounts on UsersUniversesAccounts.UniverseAccountId = UniversesAccounts.Id " +
                                  "WHERE UsersUniversesAccounts.UserId = @UserId";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));

                DataSet resultDs = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (resultDs != null && resultDs.Tables.Count == 1)
                {
                    foreach (DataRow row in resultDs.Tables[0].Rows)
                    {
                        Credentials credentials = new Credentials();
                        credentials.UniverseId = row["UniverseId"].ToString();
                        credentials.PlayerId = (Int64)row["PlayerId"];
                        credentials.UserName = (string)row["PlayerName"];

                        if (row["Password"] == DBNull.Value || row["Password"] == null || string.IsNullOrEmpty(row["Password"].ToString()))
                            credentials.Password = String.Empty;
                        else
                            credentials.Password = Encryption.EncryptString(Encryption.DecryptString((string)row["Password"], serverKey), clientKey);

                        credentials.AddDateTime = (DateTime)row["AddDateTime"];
                        if (row["ModDateTime"] == null || row["ModDateTime"] == DBNull.Value)
                            credentials.ModDateTime = null;
                        else
                            credentials.ModDateTime = (DateTime)row["ModDateTime"];
                        credentialsList.Add(credentials);
                    }
                }
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "GetUserUniversesAccounts", ex);
                //throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
            return credentialsList;
        }

        public static string GetLatestToolVersion(Guid toolId)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT TOP 1 [Version], ForceUpdate FROM db_ToolsVersion WHERE ToolId = @ToolId AND IsActive = 1 " +
                                  "AND StartTimeStamp <= GETUTCDATE() AND (EndTimeStamp > GETUTCDATE() OR EndTimeStamp IS NULL) " +
                                  "ORDER BY VersionNumber DESC";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));

                DataSet ds = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    string toolLatestVersion = row["Version"].ToString();
                    toolLatestVersion = toolLatestVersion + "|" + ((bool)row["ForceUpdate"] ? "1" : "0");
                    return toolLatestVersion;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "GetLatestToolVersion", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool IsApplicationUserValid(Guid applicationKey, Guid userId)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT COUNT(*) FROM db_Applications Applications " +
                                  "INNER JOIN db_Applications_Users ApplicationsUsers ON ApplicationsUsers.ApplicationKey = Applications.[Key] " +
                                  "WHERE Applications.[Key] = @ApplicationKey AND ApplicationsUsers.UserId = @UserId AND Applications.IsActive = 1 AND ApplicationsUsers.IsActive = 1 " +
                                  "AND Applications.StartTimeStamp <= GETUTCDATE() AND (Applications.EndTimeStamp > GETUTCDATE() OR Applications.EndTimeStamp IS NULL) " +
                                  "AND ApplicationsUsers.StartTimeStamp <= GETUTCDATE() AND (ApplicationsUsers.EndTimeStamp > GETUTCDATE() OR ApplicationsUsers.EndTimeStamp IS NULL)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));

                object retValue = oDC.ExecScalarbyQuery(sqlQuery, cmdParameters.ToArray());

                if (retValue != null && (int)retValue > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "IsApplicationUserValid", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool IsToolValid(Guid toolId, string toolVersion)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT COUNT(*) FROM db_Tools Tools " +
                                  "INNER JOIN db_ToolsVersion ToolsVersion ON ToolsVersion.ToolId = Tools.Id " +
                                  "WHERE Tools.Id = @ToolId AND Tools.IsActive = 1 AND ToolsVersion.IsActive = 1 AND ToolsVersion.[Version] = @ToolVersion " +
                                  "AND Tools.StartTimeStamp <= GETUTCDATE() AND (Tools.EndTimeStamp > GETUTCDATE() OR Tools.EndTimeStamp IS NULL) " +
                                  "AND ToolsVersion.StartTimeStamp <= GETUTCDATE() AND (ToolsVersion.EndTimeStamp > GETUTCDATE() OR ToolsVersion.EndTimeStamp IS NULL)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@ToolVersion", SqlDbType.NVarChar, toolVersion));

                object retValue = oDC.ExecScalarbyQuery(sqlQuery, cmdParameters.ToArray());

                if (retValue != null && (int)retValue > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "IsToolValid", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool IsUserAllowedToUseThisTool(Guid toolId, Guid userId)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT COUNT(*) FROM db_Users_Tools UsersTools " +
                                  "INNER JOIN db_Tools Tools ON Tools.Id = UsersTools.ToolId " +
                                  "INNER JOIN db_Tools_Communities ToolsCommunity ON ToolsCommunity.ToolId = UsersTools.ToolId " +
                                  "INNER JOIN db_Communities_Users CommunityUsers ON CommunityUsers.UserId = UsersTools.UserId " +
                                  "WHERE UsersTools.UserId = @UserId AND UsersTools.ToolId = @ToolId AND UsersTools.CommunityId = ToolsCommunity.CommunityId AND CommunityUsers.CommunityId = ToolsCommunity.CommunityId " +
                                  "AND UsersTools.IsActive = 1 AND Tools.IsActive = 1 AND ToolsCommunity.IsActive = 1 AND CommunityUsers.IsActive = 1 " +
                                  "AND UsersTools.StartTimeStamp <= GETUTCDATE() AND (UsersTools.EndTimeStamp > GETUTCDATE() OR UsersTools.EndTimeStamp IS NULL) " +
                                  "AND Tools.StartTimeStamp <= GETUTCDATE() AND (Tools.EndTimeStamp > GETUTCDATE() OR Tools.EndTimeStamp IS NULL) " +
                                  "AND ToolsCommunity.StartTimeStamp <= GETUTCDATE() AND (ToolsCommunity.EndTimeStamp > GETUTCDATE() OR ToolsCommunity.EndTimeStamp IS NULL) " +
                                  "AND CommunityUsers.StartTimeStamp <= GETUTCDATE() AND (CommunityUsers.EndTimeStamp > GETUTCDATE() OR CommunityUsers.EndTimeStamp IS NULL)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));

                object retValue = oDC.ExecScalarbyQuery(sqlQuery, cmdParameters.ToArray());

                if (retValue != null && (int)retValue > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "IsUserAllowedToUseThisTool", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static string GetUserCommunityData(Guid toolId, Guid userId)
        {
            List<Community> communityList = new List<Community>();
            List<Guid> userUniverseRoleIdList = new List<Guid>();

            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT Community.Name AS 'CommunityName', Community.Id AS 'CommunityId', Community.LanguageCode, Community.PortalUrl, Community.BoardUrl, Community.GameRulesUrl, Universe.*, UniverseRole.Id AS 'UniverseRoleId' " +
                                  "FROM db_Universes Universe " +
                                  "INNER JOIN db_Users_Universes UsersUniverse on UsersUniverse.UniverseId = Universe.Id " +
                                  "INNER JOIN db_Communities Community on Community.Id = Universe.CommunityId " +
                                  "INNER JOIN db_Communities_Users CommunityUsers on CommunityUsers.CommunityId = Community.Id " +
                                  "INNER JOIN db_UniversesRole UniverseRole on UniverseRole.Id = UsersUniverse.UniverseRoleId " +
                                  "INNER JOIN db_Tools_Communities ToolCommunities on ToolCommunities.ToolId = @ToolId AND ToolCommunities.CommunityId = Community.Id " +
                                  "INNER JOIN db_Users_Tools UserTools on UserTools.UserId = @UserId AND UserTools.ToolId = @ToolId AND UserTools.CommunityId = Community.Id " +
                                  "WHERE UsersUniverse.UserId = @UserId AND CommunityUsers.UserId = @UserId " +
                                  "AND Universe.IsActive = 1 AND (Universe.StartTimeStamp <= GETUTCDATE() AND (Universe.EndTimeStamp > GETUTCDATE() OR Universe.EndTimeStamp IS NULL)) " +
                                  "AND UsersUniverse.IsActive = 1 AND (UsersUniverse.StartTimeStamp <= GETUTCDATE() AND (UsersUniverse.EndTimeStamp > GETUTCDATE() OR UsersUniverse.EndTimeStamp IS NULL)) " +
                                  "AND Community.IsActive = 1 AND (Community.StartTimeStamp <= GETUTCDATE() AND (Community.EndTimeStamp > GETUTCDATE() OR Community.EndTimeStamp IS NULL)) " +
                                  "AND CommunityUsers.IsActive = 1 AND (CommunityUsers.StartTimeStamp <= GETUTCDATE() AND (CommunityUsers.EndTimeStamp > GETUTCDATE() OR CommunityUsers.EndTimeStamp IS NULL)) " +
                                  "AND UniverseRole.IsActive = 1 AND (UniverseRole.StartTimeStamp <= GETUTCDATE() AND (UniverseRole.EndTimeStamp > GETUTCDATE() OR UniverseRole.EndTimeStamp IS NULL)) " +
                                  "AND ToolCommunities.IsActive = 1 AND (ToolCommunities.StartTimeStamp <= GETUTCDATE() AND (ToolCommunities.EndTimeStamp > GETUTCDATE() OR ToolCommunities.EndTimeStamp IS NULL)) " +
                                  "AND UserTools.IsActive = 1 AND (UserTools.StartTimeStamp <= GETUTCDATE() AND (UserTools.EndTimeStamp > GETUTCDATE() OR UserTools.EndTimeStamp IS NULL)) " +
                                  "ORDER BY Community.Name, Universe.Number ASC";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));

                DataSet resultDs = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (resultDs != null && resultDs.Tables.Count == 1)
                {
                    foreach (DataRow row in resultDs.Tables[0].Rows)
                    {
                        if (!communityList.Exists(r => r.Id.Equals(row["CommunityId"].ToString())))
                        {
                            Community newCommunity = new Community();
                            newCommunity.Id = row["CommunityId"].ToString();
                            newCommunity.Name = row["CommunityName"].ToString();
                            newCommunity.IsActive = true;
                            newCommunity.Language = row["LanguageCode"].ToString();
                            newCommunity.PortalUrl = row["PortalUrl"].ToString();
                            newCommunity.BoardUrl = row["BoardUrl"].ToString();
                            newCommunity.GameRulesUrl = row["GameRulesUrl"].ToString();
                            communityList.Add(newCommunity);
                        }

                        Community communty = communityList.SingleOrDefault(r => r.Id.Equals(row["CommunityId"].ToString()));
                        Universe newUniverse = new Universe();
                        newUniverse.CommunityId = communty.Id;
                        newUniverse.Id = row["Id"].ToString();
                        newUniverse.IsActive = true;
                        newUniverse.Name = row["Name"].ToString();
                        newUniverse.Domain = row["Domain"].ToString();
                        newUniverse.Number = (int)row["Number"];
                        newUniverse.IsRedesign = (bool)row["IsRedesign"];
                        newUniverse.IsACS = (bool)row["IsACS"];
                        newUniverse.Speed = (int)row["Speed"];
                        newUniverse.MaxNumberOfAttackPerDay = (int)row["MaxNumberOfAttackPerDay"];
                        newUniverse.PercentOfFleetInDF = (int)row["PercentOfFleetInDF"];
                        newUniverse.PercentOfDefenseInDF = (int)row["PercentOfDefenseInDF"];
                        newUniverse.DomainGameDirectory = row["DomainGameDirectory"].ToString();
                        newUniverse.DomainAdminDirectory = row["DomainAdminDirectory"].ToString();
                        newUniverse.DomainRegDirectory = row["DomainRegDirectory"].ToString();
                        newUniverse.UniverseRoleId = row["UniverseRoleId"].ToString();
                        if (!userUniverseRoleIdList.Exists(r => r.Equals(new Guid(newUniverse.UniverseRoleId))))
                            userUniverseRoleIdList.Add(new Guid(row["UniverseRoleId"].ToString()));
                        communty.UniverseList.Add(newUniverse);
                    }
                }
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "GetUserCommunityData", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
                oDC = null;
            }

            if (communityList.Count == 0)
                return SerializeDeserializeObject.SerializeObject<List<Community>>(communityList);

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT Community.Id AS 'CommunityId', UniverseRole.Id AS 'UniverseRoleId', UniverseRole.Description AS 'UniverseRoleDescription', ToolSecureObject.Id AS 'SecureObjectId', ToolSecureObject.Name AS 'SecureObjectName', ToolSecureObject.Description AS 'SecureObjectDescription' " +
                                  "FROM db_ToolsSecureObject ToolSecureObject " +
                                  "INNER JOIN db_Tools Tool on Tool.Id = ToolSecureObject.ToolId " +
                                  "INNER JOIN db_ToolsSecureObject_Communities ToolSecureObjectCommunities on ToolSecureObjectCommunities.ToolSecureObjectId = ToolSecureObject.Id " +
                                  "INNER JOIN db_Communities Community on Community.Id = ToolSecureObjectCommunities.CommunityId " +
                                  "INNER JOIN db_Tools_Communities ToolCommunities on ToolCommunities.ToolId = Tool.Id " +
                                  "INNER JOIN db_UniversesRole_ToolsSecureObject UniverseRoleToolSecureObject on UniverseRoleToolSecureObject.ToolSecureObjectId = ToolSecureObject.Id " +
                                  "INNER JOIN db_UniversesRole UniverseRole on UniverseRole.Id = UniverseRoleToolSecureObject.UniverseRoleId " +
                                  "INNER JOIN db_Users_Tools UserTools on UserTools.UserId = @UserId AND UserTools.ToolId = @ToolId AND UserTools.CommunityId = Community.Id " +
                                  "INNER JOIN (SELECT DISTINCT UsersUniverse.UniverseRoleId, Universe.CommunityId FROM db_Universes Universe " +
                                              "INNER JOIN db_Users_Universes UsersUniverse on UsersUniverse.UniverseId = Universe.Id " +
                                              "WHERE UsersUniverse.UserId = @UserId " +
                                              "AND Universe.IsActive = 1 AND (Universe.StartTimeStamp <= GETUTCDATE() AND (Universe.EndTimeStamp > GETUTCDATE() OR Universe.EndTimeStamp IS NULL)) " +
                                              "AND UsersUniverse.IsActive = 1 AND (UsersUniverse.StartTimeStamp <= GETUTCDATE() AND (UsersUniverse.EndTimeStamp > GETUTCDATE() OR UsersUniverse.EndTimeStamp IS NULL))) UserUniverseRole on UserUniverseRole.UniverseRoleId = UniverseRole.Id " +
                                  "WHERE Community.Id = UniverseRoleToolSecureObject.CommunityId AND ToolSecureObjectCommunities.CommunityId = Community.Id AND Tool.Id = @ToolId " +
                                  "AND ToolCommunities.CommunityId = Community.Id AND UserUniverseRole.CommunityId = Community.Id " +
                                  "AND ToolSecureObject.IsActive = 1 AND (ToolSecureObject.StartTimeStamp <= GETUTCDATE() AND (ToolSecureObject.EndTimeStamp > GETUTCDATE() OR ToolSecureObject.EndTimeStamp IS NULL)) " +
                                  "AND Tool.IsActive = 1 AND (Tool.StartTimeStamp <= GETUTCDATE() AND (Tool.EndTimeStamp > GETUTCDATE() OR Tool.EndTimeStamp IS NULL)) " +
                                  "AND ToolSecureObjectCommunities.IsActive = 1 AND (ToolSecureObjectCommunities.StartTimeStamp <= GETUTCDATE() AND (ToolSecureObjectCommunities.EndTimeStamp > GETUTCDATE() OR ToolSecureObjectCommunities.EndTimeStamp IS NULL)) " +
                                  "AND Community.IsActive = 1 AND (Community.StartTimeStamp <= GETUTCDATE() AND (Community.EndTimeStamp > GETUTCDATE() OR Community.EndTimeStamp IS NULL)) " +
                                  "AND ToolCommunities.IsActive = 1 AND (ToolCommunities.StartTimeStamp <= GETUTCDATE() AND (ToolCommunities.EndTimeStamp > GETUTCDATE() OR ToolCommunities.EndTimeStamp IS NULL)) " +
                                  "AND UniverseRoleToolSecureObject.IsActive = 1 AND (UniverseRoleToolSecureObject.StartTimeStamp <= GETUTCDATE() AND (UniverseRoleToolSecureObject.EndTimeStamp > GETUTCDATE() OR UniverseRoleToolSecureObject.EndTimeStamp IS NULL)) " +
                                  "AND UniverseRole.IsActive = 1 AND (UniverseRole.StartTimeStamp <= GETUTCDATE() AND (UniverseRole.EndTimeStamp > GETUTCDATE() OR UniverseRole.EndTimeStamp IS NULL)) " +
                                  "AND UserTools.IsActive = 1 AND (UserTools.StartTimeStamp <= GETUTCDATE() AND (UserTools.EndTimeStamp > GETUTCDATE() OR UserTools.EndTimeStamp IS NULL))";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));

                DataSet resultDs = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (resultDs != null && resultDs.Tables.Count == 1)
                {
                    foreach (DataRow row in resultDs.Tables[0].Rows)
                    {
                        Community community = communityList.SingleOrDefault(r => r.Id.Equals(row["CommunityId"].ToString()));

                        if (community == null)
                            continue;

                        if (!community.UniverseRoleList.Exists(r => r.Id.Equals(row["UniverseRoleId"].ToString())))
                        {
                            UniverseRole newUniverseRole = new UniverseRole();
                            newUniverseRole.Id = row["UniverseRoleId"].ToString();
                            newUniverseRole.Name = row["UniverseRoleDescription"].ToString();
                            community.UniverseRoleList.Add(newUniverseRole);
                        }

                        UniverseRole universeRole = community.UniverseRoleList.SingleOrDefault(r => r.Id.Equals(row["UniverseRoleId"].ToString()));
                        SecureObject newSecureObject = new SecureObject();
                        newSecureObject.Id = row["SecureObjectId"].ToString();
                        newSecureObject.Name = row["SecureObjectName"].ToString();
                        newSecureObject.Description = row["SecureObjectDescription"].ToString();
                        universeRole.SecureObjectList.Add(newSecureObject);
                    }
                }
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "GetUserCommunityData", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
                oDC = null;
            }
            return SerializeDeserializeObject.SerializeObject<List<Community>>(communityList);
        }

        public static Int64 CreateApplicationsSession(Guid applicationKey, Guid toolId, string toolVersion, string computerName, DateTime startTime, DateTime lastActivity, DateTime? endTime)
        {
            SqlDataConnector oDC = null;

            try
            {
                string ipAddress = GetClientIpAddress();

                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_ApplicationsSession " +
                                  "(ApplicationKey, ToolId, ToolVersion, UserId, ComputerName, IpAddress, IsPublicComputer, StartTime, LastActivity, EndTime)" +
                                  "VALUES (@ApplicationKey, @ToolId, @ToolVersion, NULL, @ComputerName, @IpAddress, 0, @StartTime, @LastActivity, @EndTime); " +
                                  "SELECT SCOPE_IDENTITY() AS Id";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@ToolVersion", SqlDbType.NVarChar, toolVersion));
                cmdParameters.Add(oDC.CreateInputParam("@ComputerName", SqlDbType.NVarChar, computerName));
                cmdParameters.Add(oDC.CreateInputParam("@IpAddress", SqlDbType.NVarChar, ipAddress));
                cmdParameters.Add(oDC.CreateInputParam("@StartTime", SqlDbType.DateTime, startTime));
                cmdParameters.Add(oDC.CreateInputParam("@LastActivity", SqlDbType.DateTime, lastActivity));
                cmdParameters.Add(oDC.CreateInputParam("@EndTime", SqlDbType.DateTime, endTime));

                object result = oDC.ExecScalarbyQuery(sqlQuery, cmdParameters.ToArray());

                if (result != null)
                {
                    return Int64.Parse(result.ToString());
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // log it
                StoreException("Webservice", "CreateApplicationsSession", ex);
                return 0;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static Int64 ReCreateApplicationsSession(Guid applicationKey, Guid toolId, string toolVersion, Guid userId, string computerName, DateTime startTime, DateTime lastActivity, DateTime? endTime)
        {
            SqlDataConnector oDC = null;

            try
            {
                string ipAddress = GetClientIpAddress();

                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_ApplicationsSession " +
                                  "(ApplicationKey, ToolId, ToolVersion, UserId, ComputerName, IpAddress, IsPublicComputer, StartTime, LastActivity, EndTime)" +
                                  "VALUES (@ApplicationKey, @ToolId, @ToolVersion, @UserId, @ComputerName, @IpAddress, 0, @StartTime, @LastActivity, @EndTime); " +
                                  "SELECT SCOPE_IDENTITY() AS Id";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@ToolVersion", SqlDbType.NVarChar, toolVersion));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@ComputerName", SqlDbType.NVarChar, computerName));
                cmdParameters.Add(oDC.CreateInputParam("@IpAddress", SqlDbType.NVarChar, ipAddress));
                cmdParameters.Add(oDC.CreateInputParam("@StartTime", SqlDbType.DateTime, startTime));
                cmdParameters.Add(oDC.CreateInputParam("@LastActivity", SqlDbType.DateTime, lastActivity));
                cmdParameters.Add(oDC.CreateInputParam("@EndTime", SqlDbType.DateTime, endTime));

                object result = oDC.ExecScalarbyQuery(sqlQuery, cmdParameters.ToArray());

                if (result != null)
                {
                    return Int64.Parse(result.ToString());
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // log it
                StoreException("Webservice", "ReCreateApplicationsSession", ex);
                return 0;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool UpdateApplicationsSession(Guid applicationKey, Int64 applicationSessionId, Guid userId, bool isPublicComputer)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "UPDATE db_ApplicationsSession SET UserId = @UserId, IsPublicComputer = @IsPublicComputer WHERE Id = @ApplicationSessionId AND ApplicationKey = @ApplicationKey";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationSessionId", SqlDbType.BigInt, applicationSessionId));
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@IsPublicComputer", SqlDbType.Bit, isPublicComputer));

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
                StoreException("Webservice", "UpdateApplicationsSession", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool UpdateApplicationsSession(Guid applicationKey, Int64 applicationSessionId, DateTime lastActivity, DateTime? endTime)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "UPDATE db_ApplicationsSession SET LastActivity = @LastActivity, EndTime = @EndTime WHERE Id = @ApplicationSessionId AND ApplicationKey = @ApplicationKey";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationSessionId", SqlDbType.BigInt, applicationSessionId));
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@LastActivity", SqlDbType.DateTime, lastActivity));
                cmdParameters.Add(oDC.CreateInputParam("@EndTime", SqlDbType.DateTime, endTime));

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
                StoreException("Webservice", "UpdateApplicationsSession", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static void StoreException(string type, string page, Exception e)
        {
            SqlDataConnector oDC = null;

            try
            {
                string ipAddress = GetClientIpAddress();

                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_ExceptionLog " +
                                  "(DateTime, IpAddress, Type, Page, Message, Stack, InnerExceptionMessage)" +
                                  "VALUES (GETUTCDATE(), @IpAddress, @Type, @Page, @Message, @Stack, @InnerExceptionMessage)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@IpAddress", SqlDbType.NVarChar, ipAddress));
                cmdParameters.Add(oDC.CreateInputParam("@Type", SqlDbType.NVarChar, type));
                cmdParameters.Add(oDC.CreateInputParam("@Page", SqlDbType.NVarChar, page));
                cmdParameters.Add(oDC.CreateInputParam("@Message", SqlDbType.NVarChar, e.Message));
                cmdParameters.Add(oDC.CreateInputParam("@Stack", SqlDbType.NVarChar, e.StackTrace));
                cmdParameters.Add(oDC.CreateInputParam("@InnerExceptionMessage", SqlDbType.NVarChar, EssentialUtil.GetInnerExceptionMessage(e)));

                oDC.ExecNonQuerybyQuery(sqlQuery, cmdParameters.ToArray());

                emailException(e, type + " " + page);

            }
            catch (Exception ex)
            {
                // log it
                emailException(ex, "Webservice StoreException");
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool InsertApplicationsExceptionLog(Guid applicationKey, Guid toolId, Guid? userId, string type, string description, string message, string stack, string innerExceptionMessage)
        {
            SqlDataConnector oDC = null;

            try
            {
                string ipAddress = GetClientIpAddress();

                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_ApplicationsExceptionLog " +
                                  "(ApplicationKey, ToolId, UserId, IpAddress, DateTime, Type, Description, Message, Stack, InnerExceptionMessage)" +
                                  "VALUES (@ApplicationKey, @ToolId, @UserId, @IpAddress, GETUTCDATE(), @Type, @Description, @Message, @Stack, @InnerExceptionMessage); " +
                                  "SELECT SCOPE_IDENTITY() AS Id";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@IpAddress", SqlDbType.NVarChar, ipAddress));
                cmdParameters.Add(oDC.CreateInputParam("@Type", SqlDbType.NVarChar, type));
                cmdParameters.Add(oDC.CreateInputParam("@Description", SqlDbType.NVarChar, description));
                cmdParameters.Add(oDC.CreateInputParam("@Message", SqlDbType.NVarChar, message));
                cmdParameters.Add(oDC.CreateInputParam("@Stack", SqlDbType.NVarChar, stack));
                cmdParameters.Add(oDC.CreateInputParam("@InnerExceptionMessage", SqlDbType.NVarChar, innerExceptionMessage));

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
                StoreException("Webservice", "InsertApplicationsExceptionLog", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool InsertApplicationsWebClientExceptionLog(Guid applicationKey, Guid toolId, Guid? userId, string type, string url, string description, string message, string stack, string innerExceptionMessage)
        {
            SqlDataConnector oDC = null;

            try
            {
                string ipAddress = GetClientIpAddress();

                oDC = new SqlDataConnector();

                string sqlQuery = "INSERT INTO db_ApplicationsWebClientExceptionLog " +
                                  "(ApplicationKey, ToolId, UserId, IpAddress, DateTime, Type, Url, Description, Message, Stack, InnerExceptionMessage)" +
                                  "VALUES (@ApplicationKey, @ToolId, @UserId, @IpAddress, GETUTCDATE(), @Type, @Url, @Description, @Message, @Stack, @InnerExceptionMessage)";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@ApplicationKey", SqlDbType.UniqueIdentifier, applicationKey));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@IpAddress", SqlDbType.NVarChar, ipAddress));
                cmdParameters.Add(oDC.CreateInputParam("@Type", SqlDbType.NVarChar, type));
                cmdParameters.Add(oDC.CreateInputParam("@Url", SqlDbType.NVarChar, url));
                cmdParameters.Add(oDC.CreateInputParam("@Description", SqlDbType.NVarChar, description));
                cmdParameters.Add(oDC.CreateInputParam("@Message", SqlDbType.NVarChar, message));
                cmdParameters.Add(oDC.CreateInputParam("@Stack", SqlDbType.NVarChar, stack));
                cmdParameters.Add(oDC.CreateInputParam("@InnerExceptionMessage", SqlDbType.NVarChar, innerExceptionMessage));

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
                StoreException("Webservice", "InsertApplicationsWebClientExceptionLog", ex);
                return false;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
        }

        public static bool LinkUserToUniverseAccount(Guid userId, Guid universeId, Int64 playerId, string playerName, string password)
        {
            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UniverseId", SqlDbType.UniqueIdentifier, universeId));
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@PlayerId", SqlDbType.BigInt, playerId));
                cmdParameters.Add(oDC.CreateInputParam("@PlayerName", SqlDbType.NVarChar, playerName));
                cmdParameters.Add(oDC.CreateInputParam("@Password", SqlDbType.NVarChar, password));

                oDC.ExecNonQuerybySP("LinkUserToUniverseAccountLinkUserToUniverseAccount", cmdParameters.ToArray());
            }
            catch (Exception ex)
            {
                StoreException("Webservice", "LinkUserToUniverseAccount", ex);
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
            }
            return true;
        }

        // Query is obsolete but demontrates how to add different dataTable to 1 ds
        public static DataSet GetCommunityListAsDataSet(Guid toolId, Guid userId)
        {
            DataSet returnDataSet = new DataSet();

            SqlDataConnector oDC = null;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT Community.Name AS 'CommunityName', Community.Id AS 'CommunityId', Community.LanguageCode, Community.PortalUrl, Community.BoardUrl, Community.GameRulesUrl, Universe.*, UniverseRole.Id AS 'UniverseRoleId' " +
                                  "FROM db_Universes Universe " +
                                  "INNER JOIN db_Users_Universes UsersUniverse on UsersUniverse.UniverseId = Universe.Id " +
                                  "INNER JOIN db_Communities Community on Community.Id = Universe.CommunityId " +
                                  "INNER JOIN db_Communities_Users CommunityUsers on CommunityUsers.CommunityId = Community.Id " +
                                  "INNER JOIN db_UniversesRole UniverseRole on UniverseRole.Id = UsersUniverse.UniverseRoleId " +
                                  "WHERE UsersUniverse.UserId = @UserId AND CommunityUsers.UserId = @UserId " +
                                  "AND Universe.IsActive = 1 AND (Universe.StartTimeStamp <= GETUTCDATE() AND (Universe.EndTimeStamp > GETUTCDATE() OR Universe.EndTimeStamp IS NULL)) " +
                                  "AND UsersUniverse.IsActive = 1 AND (UsersUniverse.StartTimeStamp <= GETUTCDATE() AND (UsersUniverse.EndTimeStamp > GETUTCDATE() OR UsersUniverse.EndTimeStamp IS NULL)) " +
                                  "AND Community.IsActive = 1 AND (Community.StartTimeStamp <= GETUTCDATE() AND (Community.EndTimeStamp > GETUTCDATE() OR Community.EndTimeStamp IS NULL)) " +
                                  "AND CommunityUsers.IsActive = 1 AND (CommunityUsers.StartTimeStamp <= GETUTCDATE() AND (CommunityUsers.EndTimeStamp > GETUTCDATE() OR CommunityUsers.EndTimeStamp IS NULL)) " +
                                  "AND UniverseRole.IsActive = 1 AND (UniverseRole.StartTimeStamp <= GETUTCDATE() AND (UniverseRole.EndTimeStamp > GETUTCDATE() OR UniverseRole.EndTimeStamp IS NULL))";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));

                returnDataSet = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (returnDataSet != null && returnDataSet.Tables.Count == 1)
                    returnDataSet.Tables[0].TableName = "UniverseList";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
                oDC = null;
            }

            if (returnDataSet != null && returnDataSet.Tables.Count == 0)
                return returnDataSet;

            try
            {
                oDC = new SqlDataConnector();

                string sqlQuery = "SELECT Community.Id AS 'CommunityId', UniverseRole.Id AS 'UniverseRoleId', UniverseRole.Description AS 'UniverseRoleDescription', ToolSecureObject.Id AS 'SecureObjectId', ToolSecureObject.Name AS 'SecureObjectName', ToolSecureObject.Description AS 'SecureObjectDescription' " +
                                  "FROM db_ToolsSecureObject ToolSecureObject " +
                                  "INNER JOIN db_Tools Tool on Tool.Id = ToolSecureObject.ToolId " +
                                  "INNER JOIN db_ToolsSecureObject_Communities ToolSecureObjectCommunities on ToolSecureObjectCommunities.ToolSecureObjectId = ToolSecureObject.Id " +
                                  "INNER JOIN db_Communities Community on Community.Id = ToolSecureObjectCommunities.CommunityId " +
                                  "INNER JOIN db_Tools_Communities ToolCommunities on ToolCommunities.ToolId = Tool.Id " +
                                  "INNER JOIN db_UniversesRole_ToolsSecureObject UniverseRoleToolSecureObject on UniverseRoleToolSecureObject.ToolSecureObjectId = ToolSecureObject.Id " +
                                  "INNER JOIN db_UniversesRole UniverseRole on UniverseRole.Id = UniverseRoleToolSecureObject.UniverseRoleId " +
                                  "INNER JOIN (SELECT DISTINCT UsersUniverse.UniverseRoleId, Universe.CommunityId FROM db_Universes Universe " +
                                              "INNER JOIN db_Users_Universes UsersUniverse on UsersUniverse.UniverseId = Universe.Id " +
                                              "WHERE UsersUniverse.UserId = @UserId " +
                                              "AND Universe.IsActive = 1 AND (Universe.StartTimeStamp <= GETUTCDATE() AND (Universe.EndTimeStamp > GETUTCDATE() OR Universe.EndTimeStamp IS NULL)) " +
                                              "AND UsersUniverse.IsActive = 1 AND (UsersUniverse.StartTimeStamp <= GETUTCDATE() AND (UsersUniverse.EndTimeStamp > GETUTCDATE() OR UsersUniverse.EndTimeStamp IS NULL))) UserUniverseRole on UserUniverseRole.UniverseRoleId = UniverseRole.Id " +
                                  "WHERE Community.Id = UniverseRoleToolSecureObject.CommunityId AND ToolSecureObjectCommunities.CommunityId = Community.Id AND Tool.Id = @ToolId " +
                                  "AND ToolCommunities.CommunityId = Community.Id AND UserUniverseRole.CommunityId = Community.Id " +
                                  "AND ToolSecureObject.IsActive = 1 AND (ToolSecureObject.StartTimeStamp <= GETUTCDATE() AND (ToolSecureObject.EndTimeStamp > GETUTCDATE() OR ToolSecureObject.EndTimeStamp IS NULL)) " +
                                  "AND Tool.IsActive = 1 AND (Tool.StartTimeStamp <= GETUTCDATE() AND (Tool.EndTimeStamp > GETUTCDATE() OR Tool.EndTimeStamp IS NULL)) " +
                                  "AND ToolSecureObjectCommunities.IsActive = 1 AND (ToolSecureObjectCommunities.StartTimeStamp <= GETUTCDATE() AND (ToolSecureObjectCommunities.EndTimeStamp > GETUTCDATE() OR ToolSecureObjectCommunities.EndTimeStamp IS NULL)) " +
                                  "AND Community.IsActive = 1 AND (Community.StartTimeStamp <= GETUTCDATE() AND (Community.EndTimeStamp > GETUTCDATE() OR Community.EndTimeStamp IS NULL)) " +
                                  "AND ToolCommunities.IsActive = 1 AND (ToolCommunities.StartTimeStamp <= GETUTCDATE() AND (ToolCommunities.EndTimeStamp > GETUTCDATE() OR ToolCommunities.EndTimeStamp IS NULL)) " +
                                  "AND UniverseRoleToolSecureObject.IsActive = 1 AND (UniverseRoleToolSecureObject.StartTimeStamp <= GETUTCDATE() AND (UniverseRoleToolSecureObject.EndTimeStamp > GETUTCDATE() OR UniverseRoleToolSecureObject.EndTimeStamp IS NULL)) " +
                                  "AND UniverseRole.IsActive = 1 AND (UniverseRole.StartTimeStamp <= GETUTCDATE() AND (UniverseRole.EndTimeStamp > GETUTCDATE() OR UniverseRole.EndTimeStamp IS NULL)) ";

                List<SqlParameter> cmdParameters = new List<SqlParameter>();
                cmdParameters.Add(oDC.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                cmdParameters.Add(oDC.CreateInputParam("@ToolId", SqlDbType.UniqueIdentifier, toolId));

                DataSet tempDataSet = oDC.ExecDataSetbyQuery(sqlQuery, cmdParameters.ToArray());

                if (tempDataSet != null && tempDataSet.Tables.Count == 1)
                {
                    tempDataSet.Tables[0].TableName = "RoleList";
                    returnDataSet.Tables.Add(tempDataSet.Tables[0].Copy());
                    tempDataSet = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oDC != null)
                    oDC.Dispose();
                oDC = null;
            }
            return returnDataSet;
        }

        // Wrong location to do it but let's worry about this in the next version of this app
        // Added 13/05/2020
        private static void emailException(Exception ex, string page)
        {
            if (ex == null)
                return;
            try
            {
                string mailSubject = "Exception Message from " + Constants.WEBSITE_BASE_URL;
                string mailBody = "Hello there,\r\n\r\nAn Exception occured with the following details:\r\n\r\n" +
                                  "Type: " + ex.GetType().Name + "\r\n\r\n" +
                                  "Message: " + ex.Message + "\r\n\r\n" +
                                  "Page: " + page + "\r\n\r\n" +
                                  "Stack: " + ex.StackTrace;

                UtilitiesBLL.SendEmail(Constants.MAIL_EXCEPTION_TO, mailSubject, mailBody, Constants.SMTP_ACCOUNT);
            }
            catch { }
        }
    }
}
