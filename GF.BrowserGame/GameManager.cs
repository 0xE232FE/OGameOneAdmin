using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using GF.BrowserGame.Static;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Utility;
using GF.BrowserGame.Forms;
using LibCommonUtil;
using System.Web.Services.Protocols;
using Microsoft.Win32;
using System.Threading;
using GF.BrowserGame.Schema.Internal;

namespace GF.BrowserGame
{
    public class GameManager
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private Guid _toolId;
        private string _toolVersion;
        private string _buildDate = "14/05/2020";
        private Guid? _applicationKey = null;
        private Guid _userId;
        internal Guid UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        private Int64 _applicationSessionId;
        private RegistryKey _baseRegistryKey = Registry.CurrentUser;
        private string _registrySubKey = "SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName;

        private List<Credentials> _credentialsListFromServer = new List<Credentials>();
        private string _clientEncryptionKey;

        private string _appDirectory;
        private string _appDataDirectory;
        private string _appConfigDataFile;

        private string _userDataDirectory;
        private string _universeDataFile;
        private string _userDataFile;
        private string _userAppConfigDataFile;

        private bool _isInitialized = false;
        private bool _applicationExit = false;
        private bool _useRegistry = false;
        private bool _isDataDirectorySet = false;
        private bool _creatingSession = false;
        private bool _updatingSession = false;
        private bool _createdSession = false;
        private bool _sendingExceptionLog = false;

        private UserAppConfig _userAppConfig;
        private UserData _userData;
        private List<UniverseData> _universeDataList;

        private AppConfig _appConfig;
        internal AppConfig AppConfig
        {
            get { return _appConfig; }
            set { _appConfig = value; }
        }

        private List<Community> _communityList = new List<Community>();
        private List<UniManager> _uniManagerList;
        private List<Universe> _universeList;
        private TicketManager _ticketManager;

        private OgameServiceV1Call _webServiceCall;
        internal OgameServiceV1Call WebServiceCall
        {
            get { return _webServiceCall; }
            set { _webServiceCall = value; }
        }

        private System.Windows.Forms.Timer _applicationSessionTimer;


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/

        public delegate void NotifyLoggedOutEventHandler(Universe universe);
        public event NotifyLoggedOutEventHandler NotifyLoggedOut;


        #region ----- Constructor ------


        public GameManager(Guid toolId, string toolVersion)
        {
            _toolId = toolId;
            _toolVersion = toolVersion;

            _appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + Application.CompanyName + "\\" + Application.ProductName + "\\";
            CheckAppDirectory(); // Critical check, throw exception if failed
            _appDataDirectory = _appDirectory + Constants.ApplicationData.APPDATA_DIRECTORY;
            CheckAppDataDirectory(); // Critical check, throw exception if failed

            _appConfigDataFile = _appDataDirectory + Constants.ApplicationData.APPCONFIG_FILENAME; // App config

            if (!LoadApplicationConfig()) // This will load the app config in memory
                throw new Exception("Critical error, application config could not be loaded\r\n\r\nPlease contact an administrator.");

            //Verify if app key exist
            if (!DoesApplicationKeyExist())
            {
                GetNewApplicationKey(); // If this application cannot obtain a key, an exception will be thrown and the application will close itself.
            }

            if (_appConfig.WebServiceSettings == null || (_appConfig.WebServiceSettings != null && _appConfig.WebServiceSettings.WebServiceUrlList.Count == 0) || !_toolVersion.Equals(_appConfig.ToolVersion))
            {
                // Get Latest WebServiceSettings
                _appConfig.WebServiceSettings = null;
                GetLatestWebServiceSettingsOnApplicationStart(); // Throw an exception if no webservice url is found.
            }

            _applicationSessionTimer = new System.Windows.Forms.Timer();
            _applicationSessionTimer.Interval = 900000;
            _applicationSessionTimer.Tick += new System.EventHandler(this.applicationSessionTimer_Tick);
            _applicationSessionTimer.Enabled = true;
            _webServiceCall = new OgameServiceV1Call(_toolId, _toolVersion, _applicationKey, _appConfig.WebServiceSettings);

            try
            {
                CreateApplicationSessionAsync();
                if (AuthenticateUser())
                {
                    UpdateApplicationSessionUserIdAsync();
                    if (SetupApplication())
                    {
                        //if (!_appConfig.IsPublicComputer)
                        //{
                        SetDataDirectory(_userId.ToString().Split('-')[4]);
                        CheckDataDirectory(); // Critical check, throw exception if failed
                        _universeDataFile = _userDataDirectory + Constants.ApplicationData.UNIVERSE_FILENAME; // General notes etc..
                        _userDataFile = _userDataDirectory + Constants.ApplicationData.USERDATA_FILENAME; // Credentials + session
                        _userAppConfigDataFile = _userDataDirectory + Constants.ApplicationData.USERAPPCONFIGDATA_FILENAME; // App Config per user
                        //}
                        LoadApplicationData();
                    }
                }
            }
            catch (Exception ex)
            {
                EndApplicationSession();
                throw ex;
            }
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private void CheckAppDirectory()
        {
            try
            {
                if (!Directory.Exists(_appDirectory))
                    Directory.CreateDirectory(_appDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Critical error, could not create app directory.", ex);
            }
        }


        private void CheckAppDataDirectory()
        {
            try
            {
                if (!Directory.Exists(_appDataDirectory))
                    Directory.CreateDirectory(_appDataDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Critical error, could not create app data directory.", ex);
            }
        }


        private void SetDataDirectory(string directoryName)
        {
            if (!string.IsNullOrEmpty(directoryName))
            {
                _userDataDirectory = string.Format(_appDirectory + Constants.ApplicationData.USERDATA_DIRECTORY, directoryName);
                _isDataDirectorySet = true;
            }
            else
                throw new Exception("Critical error, the application could not create your data directory.");
        }


        private void CheckDataDirectory()
        {
            try
            {
                if (!Directory.Exists(_userDataDirectory))
                    Directory.CreateDirectory(_userDataDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Critical error, could not create data directory.", ex);
            }
        }


        private bool LoadApplicationConfig()
        {
            _appConfig = GetDataFromFile<AppConfig>(_appConfigDataFile, "");

            if (_appConfig != null)
            {
                if (_appConfig.ToolId.HasValue && _toolId == _appConfig.ToolId.Value && _appConfig.ApplicationKey.HasValue &&
                    !string.IsNullOrEmpty(_appConfig.ApplicationSecret) && !string.IsNullOrEmpty(_appConfig.ToolVersion))
                    return true;
            }

            return ResetApplicationConfig();
        }


        private bool ResetApplicationConfig()
        {
            _appConfig = new AppConfig();

            _appConfig.ToolId = _toolId;
            _appConfig.ToolVersion = _toolVersion;
            _appConfig.ApplicationSecret = RandomPassword.Generate(10);
            _appConfig.WebServiceSettings = new WebServiceSettings();

            return SaveApplicationConfig();
        }


        private bool DoesApplicationKeyExist()
        {
            bool returnVal = false;
            try
            {
                string tempApplicationKey = RegistryManagement.Read(_baseRegistryKey, _registrySubKey, "ApplicationKey");

                if (string.IsNullOrEmpty(tempApplicationKey))
                {
                    if (RegistryManagement.Write(_baseRegistryKey, _registrySubKey, "CanUseRegistry", "Yes"))
                    {
                        if (!string.IsNullOrEmpty(RegistryManagement.Read(_baseRegistryKey, _registrySubKey, "CanUseRegistry")))
                        {
                            _useRegistry = true;
                            if (_appConfig.ApplicationKey.HasValue)
                                if (!ResetApplicationConfig())
                                    throw new Exception("Critical error, application config could not be loaded\r\n\r\nPlease contact an administrator.");
                        }

                        RegistryManagement.DeleteKey(_baseRegistryKey, _registrySubKey, "CanUseRegistry");
                    }
                    else
                    {
                        _useRegistry = false;
                        if (_appConfig.ApplicationKey.HasValue)
                        {
                            _applicationKey = _appConfig.ApplicationKey.Value;
                            returnVal = true;
                        }
                    }
                }
                else
                {
                    _useRegistry = true;
                    try
                    {
                        tempApplicationKey = Encryption.DecryptString(tempApplicationKey, _appConfig.ApplicationSecret);
                        _applicationKey = new Guid(tempApplicationKey);

                        if (_applicationKey == _appConfig.ApplicationKey)
                            returnVal = true;
                        else
                        {
                            // Config file has been altered or transfered from one computer to another or registry key has been altered
                            RegistryManagement.DeleteKey(_baseRegistryKey, _registrySubKey, "ApplicationKey");
                            if (!ResetApplicationConfig())
                                throw new Exception("Critical error, application config could not be loaded.\r\n\r\nPlease contact an administrator.");
                            _applicationKey = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Critical error, application config could not be loaded"))
                            throw ex;

                        // Config file has been altered or transfered from one computer to another or registry key has been altered
                        RegistryManagement.DeleteKey(_baseRegistryKey, _registrySubKey, "ApplicationKey");
                        if (!ResetApplicationConfig())
                            throw new Exception("Critical error, application config could not be loaded.\r\n\r\nPlease contact an administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Critical error, application config could not be loaded"))
                    throw ex;

                _useRegistry = false;
                // Log it
            }
            return returnVal;
        }


        private void GetNewApplicationKey()
        {
            string exceptionMessage = "";

            using (var waitingForm = new SplashScreen())
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (sender, e) =>
                {
                    OgameServiceV1Call webserviceCall = new OgameServiceV1Call(_toolId, _toolVersion, null, null);
                    _appConfig.WebServiceSettings = webserviceCall.GetLatestWebServiceSettings(true);
                    webserviceCall.UpdateWebServiceSettings(_appConfig.WebServiceSettings);
                    _applicationKey = webserviceCall.RegisterApplication(_toolId, Environment.MachineName, true);
                };

                worker.RunWorkerCompleted += (sender, e) =>
                {
                    if (_applicationKey.HasValue)
                    {
                        _appConfig.ApplicationKey = _applicationKey;

                        if (!SaveApplicationConfig())
                            exceptionMessage = "Critical error, application config could not be loaded.\r\n\r\nPlease contact an administrator.";

                        if (_useRegistry)
                        {
                            if (!SaveApplicationKeyToRegistry())
                                exceptionMessage = "Critical error, application config could not be loaded.\r\n\r\nPlease contact an administrator.";
                        }
                    }
                    else
                        exceptionMessage = "The application could not authenticate with celestos's server.\r\n\r\nPlease try again later or contact an administrator.";

                    waitingForm.SetShowHide(false);
                };

                worker.RunWorkerAsync();

                waitingForm.SetShowHide(true);

                DialogResult result = waitingForm.ShowDialog();

                if (!string.IsNullOrEmpty(exceptionMessage))
                    throw new Exception(exceptionMessage);
            }
        }


        private bool SaveApplicationKeyToRegistry()
        {
            return RegistryManagement.Write(_baseRegistryKey, _registrySubKey, "ApplicationKey", Encryption.EncryptString(_applicationKey.Value.ToString(), _appConfig.ApplicationSecret));
        }


        private void GetLatestWebServiceSettingsOnApplicationStart()
        {
            string exceptionMessage = "";

            using (var waitingForm = new SplashScreen())
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (sender, e) =>
                {
                    OgameServiceV1Call webserviceCall = new OgameServiceV1Call(_toolId, _toolVersion, _applicationKey, null);
                    _appConfig.WebServiceSettings = webserviceCall.GetLatestWebServiceSettings(true);
                };

                worker.RunWorkerCompleted += (sender, e) =>
                {
                    if (_appConfig.WebServiceSettings != null && _appConfig.WebServiceSettings.WebServiceUrlList.Count > 0)
                    {
                        if (!SaveApplicationConfig())
                            exceptionMessage = "Critical error, application config could not be loaded.\r\n\r\nPlease contact an administrator.";
                    }
                    else
                        exceptionMessage = "The application could not authenticate with celestos's server.\r\n\r\nPlease try again later or contact an administrator.";

                    waitingForm.SetShowHide(false);
                };

                worker.RunWorkerAsync();

                waitingForm.SetShowHide(true);
                DialogResult result = waitingForm.ShowDialog();

                if (!string.IsNullOrEmpty(exceptionMessage))
                    throw new Exception(exceptionMessage);
            }
        }


        internal void ParseWebServiceResponseException(Exception e, bool sendExceptionLog, string exceptionDescription, bool requestNewCredentialsOnFaillure, out bool getNewCredentials, out bool closeApplication, out string errorMessage)
        {
            errorMessage = "";
            closeApplication = false;
            getNewCredentials = false;

            if (e == null || string.IsNullOrEmpty(e.Message))
                return;

            if (e.Message.Contains("account is not approved"))
            {
                closeApplication = true;
                errorMessage = "Authentication has failed because your account is not approved.\r\n\r\nContact an administrator for more information";
            }
            else if (e.Message.Contains("account is locked"))
            {
                closeApplication = true;
                errorMessage = "Authentication has failed because your account is locked.\r\n\r\nContact an administrator for more information";
            }
            else if (e.Message.Contains("invalid username"))
            {
                if (requestNewCredentialsOnFaillure)
                    getNewCredentials = true;
                else
                    errorMessage = "Your login name is incorrect.";
            }
            else if (e.Message.Contains("wrong password"))
            {
                if (requestNewCredentialsOnFaillure)
                    getNewCredentials = true;
                else
                    errorMessage = "Your password is incorrect.";
            }
            else if (e.Message.Contains("database problem"))
                errorMessage = "Celestos's database is temporarilly unvailable, please try again later.";
            else if (e.Message.Contains(Constants.Message.WEBSERVICES_ARE_DOWN))
                errorMessage = Constants.Message.WEBSERVICES_ARE_DOWN;
            else if (e.Message.Contains("No soap header was specified."))
            {
                closeApplication = true;
                errorMessage = "A technical error occurred, please contact an administrator and quote the error code #0001.";
            }
            else if (e.Message.Contains("Username was not supplied for authentication in SoapHeader."))
            {
                closeApplication = true;
                errorMessage = "A technical error occurred, please contact an administrator and quote the error code #0002.";
            }
            else if (e.Message.Contains("Password was not supplied for authentication in SoapHeader."))
            {
                closeApplication = true;
                errorMessage = "A technical error occurred, please contact an administrator and quote the error code #0003.";
            }
            else
            {
                if (sendExceptionLog)
                    CreateApplicationExceptionLog(exceptionDescription, e);
                errorMessage = "A technical error occurred, please try again.";
            }
        }


        private bool AuthenticateUser()
        {
            bool retValue = false;
            try
            {
                using (var authenticateUserForm = new AuthenticateUser(this))
                {
                    DialogResult result = authenticateUserForm.ShowDialog();
                    if (result == DialogResult.OK)
                        return true;
                }
            }
            catch (Exception ex)
            {
                CreateApplicationExceptionLog("Error occurred in AuthenticateUser()", ex);
                throw new Exception("Error occurred in AuthenticateUser()", ex);
            }
            Application.DoEvents();
            return retValue;
        }


        private bool SetupApplication()
        {
            SetupAppObj returnObj = null;
            bool retValue = false;
            bool closeApplication = false;
            bool getNewCredentials = false;
            bool warnUpdateAvailable = false;
            bool forceDownloadUpdate = false;
            string errorMessage = "";

            using (var waitingForm = new SplashScreen())
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (sender, e) =>
                {
                    returnObj = _webServiceCall.SetupApplication(_appConfig.ToolVersion, _toolVersion, true);
                };

                worker.RunWorkerCompleted += (sender, e) =>
                {
                    try
                    {
                        if (e.Error != null)
                        {
                            ParseWebServiceResponseException(e.Error, true, "GameManager.SetupApplication() has failed", true, out getNewCredentials, out closeApplication, out errorMessage);
                        }
                        else if (returnObj != null)
                        {
                            if (!returnObj.Error)
                            {
                                if (returnObj.IsApplicationUserValid && returnObj.IsToolValid && returnObj.IsUserAllowedToUseThisTool)
                                {
                                    try
                                    {
                                        _communityList = SerializeDeserializeObject.DeserializeObject<List<GF.BrowserGame.Schema.Serializable.Community>>(returnObj.CommunityData);

                                        if (!_appConfig.ToolVersion.Equals(_toolVersion))
                                        {
                                            _appConfig.ToolVersion = _toolVersion;
                                            if (!SaveApplicationConfig())
                                                errorMessage = "Critical error, application config could not be loaded.\r\n\r\nPlease contact an administrator.";
                                        }

                                        if (_communityList != null && _communityList.Count > 0)
                                        {
                                            _clientEncryptionKey = returnObj.ClientEncryptionKey;

                                            if (returnObj.CredentialsList != null)
                                                _credentialsListFromServer = returnObj.CredentialsList;
                                            else
                                                _credentialsListFromServer = new List<Credentials>();

                                            string toolLatestVersion = returnObj.ToolLatestVersion;

                                            if (!string.IsNullOrEmpty(toolLatestVersion))
                                            {
                                                int compResult = EssentialUtil.CompareVersion(_toolVersion, toolLatestVersion.Split('|')[0]);

                                                if (compResult < 0) // _toolVersion is < toolLatestVersion
                                                {
                                                    if (toolLatestVersion.Split('|')[1].Equals("0")) // just warn new update available
                                                    {
                                                        warnUpdateAvailable = true;
                                                    }
                                                    else // force client to upgrade
                                                    {
                                                        forceDownloadUpdate = true;
                                                    }
                                                }
                                                else if (compResult > 0) // _toolVersion is > toolLatestVersion
                                                {

                                                }
                                            }
                                            retValue = true;
                                        }
                                        else
                                        {
                                            errorMessage = "The application could not be loaded.\n\nYou have not been allocated to any universes!\r\rContact your game administrator for more information.";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Log it
                                        CreateApplicationExceptionLog("GameManager.SetupApplication() has failed", ex);
                                        _communityList = null;
                                        errorMessage = "A technical error occurred, please try again.";
                                    }
                                }
                                else if (!returnObj.IsApplicationUserValid)
                                {
                                    errorMessage = "Your application seems to have been deactivated.\r\n\r\nContact an administrator for more information.";
                                }
                                else if (!returnObj.IsToolValid)
                                {
                                    string toolLatestVersion = returnObj.ToolLatestVersion;

                                    if (!string.IsNullOrEmpty(toolLatestVersion))
                                    {
                                        int compResult = EssentialUtil.CompareVersion(_toolVersion, toolLatestVersion.Split('|')[0]);

                                        if (compResult < 0) // _toolVersion is < toolLatestVersion
                                            forceDownloadUpdate = true;
                                    }
                                    if (!forceDownloadUpdate)
                                        errorMessage = "This tool is not currently active.\r\n\r\nContact an administrator for more information.";
                                }
                                else if (!returnObj.IsUserAllowedToUseThisTool)
                                {
                                    errorMessage = "You do not have the correct permission to use this tool.\r\n\r\nContact an administrator for more information.";
                                }
                            }
                            else
                            {
                                CreateApplicationExceptionLog("GameManager.SetupApplication() has failed, returnObj returned an error from server", new Exception(returnObj.ErrorMessage));
                                errorMessage = "Your credentials are valid but unfortunately something went wrong while downloading your account details!\r\n\r\nTry again or contact an administrator for more information.";
                            }
                        }
                        else
                        {
                            CreateApplicationExceptionLog("GameManager.SetupApplication() has failed, returnObj is null", e.Error);
                            errorMessage = "A technical error occurred, please try again.";
                        }
                    }
                    catch (Exception ex)
                    {
                        CreateApplicationExceptionLog("GameManager.SetupApplication() has failed, unknow error", ex);
                        errorMessage = "A technical error occurred, please try again.";
                    }
                    waitingForm.Close();
                };


                worker.RunWorkerAsync();
                waitingForm.SetShowHide(true);
                DialogResult result = waitingForm.ShowDialog();

                if (!string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show(errorMessage, "Information", MessageBoxButtons.OK);

                if (getNewCredentials)
                {
                    if (MessageBox.Show("It seems that you have changed your password on celestos.net since you have last used this application.\r\n\r\nYou must update your credentials to continue using this application.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        if (UpdateCelestosCredentials())
                        {
                            return SetupApplication();
                        }
                    }
                }

                if (warnUpdateAvailable)
                {
                    if (MessageBox.Show("A new version of this application is available!\r\n\r\nWould you like to download it now?", "Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Utilities.OpenDefaultWebBrowser("https://celestos.azurewebsites.net");
                        retValue = false;
                    }
                }
                else if (forceDownloadUpdate)
                {
                    if (MessageBox.Show("A new version of this application is available!\r\n\r\nYour version is no longer active, you must upgrade to the new version in order to continue using it.\r\n\r\nWould you like to download it now?", "Update Available!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Utilities.OpenDefaultWebBrowser("https://celestos.azurewebsites.net");
                    }
                    retValue = false;
                }
            }
            return retValue;
        }


        private void LoadApplicationData()
        {
            try
            {
                _isInitialized = false;
                _uniManagerList = null;
                _uniManagerList = new List<UniManager>();
                _universeList = new List<Universe>();
                _ticketManager = null;

                if (_communityList == null || _communityList.Count == 0)
                    throw new Exception("The application could not be loaded.\n\nYou have not been allocated to any universes!\r\rContact your game administrator for more information.");

                if (!_appConfig.IsPublicComputer)
                {
                    _userData = GetDataFromFile<UserData>(_userDataFile, _clientEncryptionKey);
                    _universeDataList = GetDataFromFile<List<UniverseData>>(_universeDataFile, _clientEncryptionKey);
                }

                _userAppConfig = GetDataFromFile<UserAppConfig>(_userAppConfigDataFile, _clientEncryptionKey);

                if (_userAppConfig == null)
                    _userAppConfig = new UserAppConfig();

                if (string.IsNullOrEmpty(_userAppConfig.AppName))
                    _userAppConfig.AppName = Application.ProductName;

                if (_userData == null)
                    _userData = new UserData();

                if (_universeDataList == null)
                    _universeDataList = new List<UniverseData>();

                foreach (Community community in _communityList)
                {
                    if (community.UniverseList != null && community.UniverseList.Count > 0)
                    {
                        foreach (Universe universe in community.UniverseList)
                        {
                            GameSession uniSession = _userData.GameSessionList.SingleOrDefault(r => r.UniverseId.Equals(universe.Id));
                            if (uniSession == null)
                            {
                                uniSession = new GameSession();
                                uniSession.UniverseId = universe.Id;
                                _userData.GameSessionList.Add(uniSession);
                            }

                            Credentials uniCredentials = _userData.CredentialsList.SingleOrDefault(r => r.UniverseId.Equals(universe.Id));
                            Credentials uniCredentialsFromServer = _credentialsListFromServer.SingleOrDefault(r => r.UniverseId.Equals(universe.Id));

                            if (uniCredentials == null)
                            {
                                uniCredentials = new Credentials();
                                uniCredentials.UniverseId = universe.Id;
                                uniCredentials.AddDateTime = DateTime.MinValue;
                                uniCredentials.ModDateTime = DateTime.MinValue;
                                if (uniCredentialsFromServer != null)
                                {
                                    if (_userAppConfig.SynchronizeOGameCredentials)
                                    {
                                        uniCredentials.PlayerId = uniCredentialsFromServer.PlayerId;
                                        uniCredentials.UserName = uniCredentialsFromServer.UserName;
                                        uniCredentials.Password = uniCredentialsFromServer.Password;
                                        uniCredentials.AddDateTime = DateTime.UtcNow;
                                        uniCredentials.ModDateTime = DateTime.UtcNow;
                                        uniCredentials.AreCredentialsSyncToServer = true;
                                    }
                                    uniCredentials.IsUserToUniverseAccountLinkCreated = true;
                                }
                                _userData.CredentialsList.Add(uniCredentials);
                            }
                            else
                            {
                                if (_userAppConfig.SynchronizeOGameCredentials && uniCredentialsFromServer != null && uniCredentials.AreCredentialsSyncToServer && (string.IsNullOrEmpty(uniCredentialsFromServer.Password) && !string.IsNullOrEmpty(uniCredentials.Password)))
                                {
                                    uniCredentials.AreCredentialsSyncToServer = false;
                                }

                                if (_userAppConfig.SynchronizeOGameCredentials && uniCredentialsFromServer != null && (uniCredentials.ModDateTime < uniCredentialsFromServer.AddDateTime || (uniCredentialsFromServer.ModDateTime.HasValue && uniCredentials.ModDateTime < uniCredentialsFromServer.ModDateTime.Value)))
                                {
                                    uniCredentials.PlayerId = uniCredentialsFromServer.PlayerId;
                                    uniCredentials.UserName = uniCredentialsFromServer.UserName;
                                    uniCredentials.Password = uniCredentialsFromServer.Password;
                                    uniCredentials.AddDateTime = DateTime.UtcNow;
                                    uniCredentials.ModDateTime = DateTime.UtcNow;
                                    uniCredentials.AreCredentialsSyncToServer = true;
                                }

                                if (uniCredentialsFromServer != null && (uniCredentialsFromServer.PlayerId != uniCredentials.PlayerId || !uniCredentialsFromServer.UserName.ToLower().Equals(uniCredentials.UserName.ToLower())))
                                    uniCredentials.IsUserToUniverseAccountLinkCreated = false;
                                else if (uniCredentialsFromServer != null)
                                    uniCredentials.IsUserToUniverseAccountLinkCreated = true;
                                else
                                    uniCredentials.IsUserToUniverseAccountLinkCreated = false;
                            }

                            UniverseData universeData = _universeDataList.SingleOrDefault(r => r.UniverseId.Equals(universe.Id));

                            if (universeData == null)
                            {
                                universeData = new UniverseData();
                                universeData.UniverseId = universe.Id;
                                _universeDataList.Add(universeData);
                            }

                            UniManager uniManager = new UniManager(universe, uniCredentials, uniSession);
                            uniManager.LogWebClientException += new UniManager.LogWebClientExceptionEventHandler(uniManager_LogWebClientException);
                            _uniManagerList.Add(uniManager);
                            _universeList.Add(universe);
                        }
                    }
                }

                _ticketManager = new TicketManager(_userData.ComaToolCredentials, _userData.ComaToolSession);
                _ticketManager.LogWebClientException += new TicketManager.LogWebClientExceptionEventHandler(_ticketManager_LogWebClientException);

                if (!_appConfig.IsPublicComputer)
                {
                    if (!SaveUserData())
                        throw new Exception("Critical error, your data could not be saved locally.\r\n\r\nPlease contact an administrator for more information.");
                    if (!SaveUniverseDataList())
                        throw new Exception("Critical error, your data could not be saved locally.\r\n\r\nPlease contact an administrator for more information.");
                }

                if (!SaveUserAppConfig())
                    throw new Exception("Critical error, your app data could not be saved locally.\r\n\r\nPlease contact an administrator for more information.");

                if (_uniManagerList == null || _uniManagerList.Count == 0)
                    throw new Exception("The application could not be loaded.\n\nInternal error, please contact your game administrator!");
                else
                    _isInitialized = true;
            }
            catch (Exception ex)
            {
                CreateApplicationExceptionLog("GameManager.LoadApplicationData() has failed", ex);
                throw ex;
            }
        }

        #region Application Exception & Session Methods


        internal void CreateApplicationExceptionLog(string description, Exception exception)
        {
            try
            {
                if (_webServiceCall != null)
                {
                    _webServiceCall.CreateApplicationExceptionLog(_toolId, description, exception, false);
                }
            }
            catch (Exception ex)
            {
                // Log it
            }
        }


        internal void CreateApplicationExceptionLogAsync(string description, Exception exception)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                try
                {
                    if (_webServiceCall != null)
                    {
                        _sendingExceptionLog = true;
                        _webServiceCall.CreateApplicationExceptionLog(_toolId, description, exception, false);
                    }
                }
                catch (Exception ex)
                {
                    // Log it
                }
                _sendingExceptionLog = false;
            };
            worker.RunWorkerAsync();
        }


        internal void CreateWebClientExceptionLogAsync(Uri requestUri, Exception exception)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                try
                {
                    if (_webServiceCall != null)
                    {
                        _webServiceCall.CreateWebClientExceptionLog(_toolId, requestUri.AbsoluteUri, exception, false);
                    }
                }
                catch (Exception ex)
                {
                    // Log it
                }
            };
            worker.RunWorkerAsync();
        }


        private void CreateApplicationSessionAsync()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                if (!_createdSession && !_creatingSession)
                {
                    _creatingSession = true;
                    try
                    {
                        _applicationSessionId = _webServiceCall.StartApplicationSession(_toolId, _toolVersion, Environment.MachineName, true);
                        if (_applicationSessionId > 0)
                            _createdSession = true;
                    }
                    catch (Exception ex)
                    {
                        // Log it
                        CreateApplicationExceptionLog("CreateApplicationSessionAsync() has failed", ex);
                    }
                    _creatingSession = false;
                }
            };
            worker.RunWorkerAsync();
        }


        private void UpdateApplicationSessionUserIdAsync()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                if (_createdSession && !_updatingSession)
                {
                    _updatingSession = true;
                    try
                    {
                        if (_applicationKey.HasValue)
                        {
                            _webServiceCall.UpdateApplicationSessionUserId(_applicationSessionId, _appConfig.IsPublicComputer, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log it
                        CreateApplicationExceptionLog("UpdateApplicationSessionUserIdAsync() has failed", ex);
                    }
                    _updatingSession = false;
                }
            };
            worker.RunWorkerAsync();
        }


        private void UpdateApplicationSession(bool endSession)
        {
            if (_createdSession && !_updatingSession)
            {
                _updatingSession = true;
                try
                {
                    if (_applicationKey.HasValue)
                    {
                        if (endSession)
                            _webServiceCall.EndApplicationSession(_applicationSessionId, DateTime.UtcNow, DateTime.UtcNow, true);
                        else
                            _webServiceCall.UpdateApplicationSession(_applicationSessionId, DateTime.UtcNow, true);
                    }
                }
                catch (Exception ex)
                {
                    // Log it
                    CreateApplicationExceptionLog("UpdateApplicationSession() has failed", ex);
                }
                _updatingSession = false;
            }
        }


        private void UpdateApplicationSessionAsync(bool endSession)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                if (_createdSession && !_updatingSession)
                {
                    _updatingSession = true;
                    try
                    {
                        if (_applicationKey.HasValue)
                        {
                            if (endSession)
                                _webServiceCall.EndApplicationSession(_applicationSessionId, DateTime.UtcNow, DateTime.UtcNow, true);
                            else
                                _webServiceCall.UpdateApplicationSession(_applicationSessionId, DateTime.UtcNow, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log it
                        CreateApplicationExceptionLog("UpdateApplicationSessionAsync() has failed", ex);
                    }
                    _updatingSession = false;
                }
            };
            worker.RunWorkerAsync();
        }


        #endregion Application Exception & Session Methods


        private bool SaveUserAppConfig()
        {
            return SaveDataToFile<UserAppConfig>(_userAppConfig, _userAppConfigDataFile, _clientEncryptionKey);
        }


        private bool SaveUserData()
        {
            return SaveDataToFile<UserData>(_userData, _userDataFile, _clientEncryptionKey);
        }


        private bool SaveUniverseDataList()
        {
            if (_universeDataList != null)
                return SaveDataToFile<List<UniverseData>>(_universeDataList, _universeDataFile, _clientEncryptionKey);
            else
                return false;
        }


        private bool UnlockApplication()
        {
            bool retValue = false;
            try
            {
                using (var unlockAppForm = new UnlockApplication(_webServiceCall.SoapHeader.Username, Utilities.SerializeObjectToString("Can you decrypt me?", _webServiceCall.SoapHeader.Password), _userAppConfig.MinimizeWhenAppLocked))
                {
                    DialogResult result = unlockAppForm.ShowDialog();
                    if (result == DialogResult.OK)
                        retValue = true;
                    else
                        retValue = false;
                }
            }
            catch (Exception ex)
            {
                CreateApplicationExceptionLog("Error occurred in UnlockApplication()", ex);
                throw new Exception("Error occurred in UnlockApplication()", ex);
            }
            Application.DoEvents();
            return retValue;
        }


        private bool SetApplicationOptions()
        {
            bool retValue = false;
            try
            {
                using (var optionsForm = new Options(_userAppConfig))
                {
                    DialogResult result = optionsForm.ShowDialog();
                    if (result == DialogResult.OK)
                        retValue = true;
                    else
                        retValue = false;
                }
            }
            catch (Exception ex)
            {
                CreateApplicationExceptionLog("Error occurred in SetApplicationOptions()", ex);
                throw new Exception("Error occurred in SetApplicationOptions()", ex);
            }
            Application.DoEvents();
            return retValue;
        }


        private bool UpdateCommunityList()
        {
            bool retValue = false;
            bool forceCloseApp = false;
            bool getNewCredentials = false;
            string showMessage = null;

            using (var waitingForm = new WaitingForm())
            {
                var worker = new BackgroundWorker();

                #region worker.DoWork
                worker.DoWork += (sender, e) =>
                {
                    bool knownException = false;

                    try
                    {
                        _communityList = _webServiceCall.GetUserCommunityList(_toolId, true);
                        e.Result = "Valid credentials";
                        //SaveDataToFile<List<Community>>(_communityList, _communityDataFile, _masterPassword);
                    }
                    catch (SoapException soapEx)
                    {
                        if (soapEx.Message.Contains("account is not approved"))
                            e.Result = "Authentication has failed because your account is not approved.\r\n\r\nContact an administrator for more information";
                        else if (soapEx.Message.Contains("account is locked"))
                            e.Result = "Authentication has failed because your account is locked.\r\n\r\nContact an administrator for more information";
                        else if (soapEx.Message.Contains("database problem"))
                            throw new Exception("Network");
                        else
                            e.Result = "Wrong credentials";
                    }
                    catch (Exception ex)
                    {
                        if (!knownException || ex.Message.Equals("all webservices are down"))
                            throw new Exception("Network");
                        else
                            throw ex;
                    }
                };
                #endregion worker.DoWork

                #region worker.RunWorkerCompleted
                worker.RunWorkerCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (e.Error.Message.Equals("Network"))
                        {
                            retValue = false;
                            showMessage = "Celestos's server is unavailable, please try again later.";
                        }
                        else
                        {
                            retValue = false;
                            showMessage = e.Error.Message;
                        }
                    }
                    else
                    {
                        if (e.Result.ToString().Equals("Valid credentials"))
                        {
                            retValue = true;
                        }
                        else
                        {
                            if (e.Result.ToString().Equals("Wrong credentials"))
                            {
                                // Trigger renew credentials
                                getNewCredentials = true;
                                retValue = false;
                            }
                            else
                            {
                                // account locked or not approved -> should force app to close
                                retValue = false;
                                forceCloseApp = true;
                                showMessage = e.Result.ToString();
                            }
                        }
                    }
                    waitingForm.SetShowHide(false);
                };
                #endregion worker.RunWorkerCompleted

                worker.RunWorkerAsync();

                waitingForm.SetShowHide(true);
                DialogResult result = waitingForm.ShowDialog();

                if (!string.IsNullOrEmpty(showMessage))
                    MessageBox.Show(showMessage, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (getNewCredentials)
                {
                    if (MessageBox.Show("It seems that you have changed your password on celestos.net since you have last used this application.\r\n\r\nYou must update your credentials to continue using this application.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        if (UpdateCelestosCredentials())
                            retValue = UpdateCommunityList();
                        else
                            forceCloseApp = true;
                    }
                    else
                        forceCloseApp = true;
                }
            }
            if (forceCloseApp)
                throw new Exception("Close application");
            return retValue;
        }


        private bool UpdateCelestosCredentials()
        {
            bool retValue = false;
            try
            {
                using (var updateCelestosCredentialsAppForm = new UpdateCelestosCredentials(this))
                {
                    DialogResult result = updateCelestosCredentialsAppForm.ShowDialog();
                    if (result == DialogResult.OK)
                        retValue = true;
                }
            }
            catch (Exception ex)
            {
                CreateApplicationExceptionLog("Error occurred in UpdateCelestosCredentials()", ex);
                //throw new Exception("Error occurred in UpdateCelestosCredentials()", ex);
            }
            Application.DoEvents();
            return retValue;
        }


        private string GetToolLatestVersion()
        {
            string retValue = null;
            bool closeApplication = false;
            bool getNewCredentials = false;
            string errorMessage = "";

            using (var waitingForm = new WaitingForm())
            {
                var worker = new BackgroundWorker();

                #region worker.DoWork
                worker.DoWork += (sender, e) =>
                {
                    retValue = _webServiceCall.GetToolLatestVersion(_toolId, true);
                };
                #endregion worker.DoWork

                #region worker.RunWorkerCompleted
                worker.RunWorkerCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        ParseWebServiceResponseException(e.Error, true, "GameManager.GetToolLatestVersion() has failed", true, out getNewCredentials, out closeApplication, out errorMessage);
                    }
                    waitingForm.SetShowHide(false);
                };
                #endregion worker.RunWorkerCompleted

                worker.RunWorkerAsync();

                waitingForm.SetShowHide(true);
                DialogResult result = waitingForm.ShowDialog();

                if (!string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show(errorMessage, "Information", MessageBoxButtons.OK);

                if (getNewCredentials)
                {
                    if (MessageBox.Show("It seems that you have changed your password on celestos.net since you have last used this application.\r\n\r\nYou must update your credentials to continue using this application.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        if (UpdateCelestosCredentials())
                            return GetToolLatestVersion();
                        else
                            closeApplication = true;
                    }
                    else
                        closeApplication = true;
                }
            }
            if (closeApplication)
                throw new Exception("Close application");
            return retValue;
        }


        private void SynchronizeCredentialsAsync(UniManager uniManager)
        {
            bool closeApplication = false;
            bool getNewCredentials = false;
            string errorMessage = "";

            var worker = new BackgroundWorker();

            #region worker.DoWork
            worker.DoWork += (sender, e) =>
            {
                Credentials credentials = uniManager.GetCredentials();
                credentials.IsUserToUniverseAccountLinkCreated = credentials.AreCredentialsSyncToServer = _webServiceCall.SynchronizeCredentials(new Guid(uniManager.GetUniverseId()), uniManager.GetCredentials().PlayerId, uniManager.GetCredentials().UserName, uniManager.GetCredentials().Password, true);
            };
            #endregion worker.DoWork

            #region worker.RunWorkerCompleted
            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    ParseWebServiceResponseException(e.Error, true, "GameManager.SynchronizeCredentialsAsync() has failed", true, out getNewCredentials, out closeApplication, out errorMessage);
                }

                if (!string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show(errorMessage, "Information", MessageBoxButtons.OK);

                if (getNewCredentials)
                {
                    if (MessageBox.Show("It seems that you have changed your password on celestos.net since you have last used this application.\r\n\r\nYou must update your credentials to continue using this application.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        if (UpdateCelestosCredentials())
                            SynchronizeCredentialsAsync(uniManager);
                        else
                            closeApplication = true;
                    }
                    else
                        closeApplication = true;
                }

                //if (closeApplication)
                //    throw new Exception("Close application");
            };
            #endregion worker.RunWorkerCompleted

            worker.RunWorkerAsync();
        }


        private void LinkUserToUniverseAccountAsync(UniManager uniManager)
        {
            bool closeApplication = false;
            bool getNewCredentials = false;
            string errorMessage = "";

            var worker = new BackgroundWorker();

            #region worker.DoWork
            worker.DoWork += (sender, e) =>
            {
                Credentials credentials = uniManager.GetCredentials();
                credentials.IsUserToUniverseAccountLinkCreated = _webServiceCall.LinkUserToUniverseAccount(new Guid(uniManager.GetUniverseId()), uniManager.GetCredentials().PlayerId, uniManager.GetCredentials().UserName, true);
            };
            #endregion worker.DoWork

            #region worker.RunWorkerCompleted
            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    ParseWebServiceResponseException(e.Error, true, "GameManager.LinkUserToUniverseAccountAsync() has failed", true, out getNewCredentials, out closeApplication, out errorMessage);
                }

                if (!string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show(errorMessage, "Information", MessageBoxButtons.OK);

                if (getNewCredentials)
                {
                    if (MessageBox.Show("It seems that you have changed your password on celestos.net since you have last used this application.\r\n\r\nYou must update your credentials to continue using this application.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        if (UpdateCelestosCredentials())
                            LinkUserToUniverseAccountAsync(uniManager);
                        else
                            closeApplication = true;
                    }
                    else
                        closeApplication = true;
                }

                //if (closeApplication)
                //    throw new Exception("Close application");
            };
            #endregion worker.RunWorkerCompleted

            worker.RunWorkerAsync();
        }


        private void RunQuickLogin(UniManager uniManager)
        {
            if (!uniManager.IsCredentialsExist())
            {
                using (var quickLoginForm = new QuickLogin(uniManager.GetCredentials().UserName, !_userAppConfig.SynchronizeOGameCredentials))
                {
                    quickLoginForm.WindowTitle = "Login to " + uniManager.GetUniverseDomain();
                    DialogResult result = quickLoginForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        using (var waitingForm = new WaitingForm())
                        {
                            var worker = new BackgroundWorker();

                            #region worker.DoWork
                            worker.DoWork += (sender, e) =>
                            {
                                string errorMessage = "";
                                if (uniManager.OnQuickLogin(quickLoginForm.UserName, quickLoginForm.Password, out errorMessage))
                                {
                                    string previousSavedUserName = uniManager.GetCredentials().UserName;
                                    string previousSavedPassword = uniManager.GetCredentials().Password;

                                    if (string.IsNullOrEmpty(previousSavedUserName) || (!string.IsNullOrEmpty(previousSavedUserName) && !previousSavedUserName.ToLower().Equals(quickLoginForm.UserName.ToLower())))
                                    {
                                        uniManager.SaveCredentialsPlayerId(0);
                                        uniManager.SetCredentialsIsUserToUniverseAccountLinkCreated(false);
                                    }

                                    try
                                    {
                                        if (uniManager.GetCredentials().PlayerId == 0)
                                        {
                                            Int64 playerId = SearchPlayerId(uniManager.GetUniverseId(), quickLoginForm.UserName);
                                            if (playerId != -1 && playerId != 0)
                                                uniManager.SaveCredentialsPlayerId(playerId);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        CreateApplicationExceptionLog("GameManager.SearchPlayerId() has failed in GameManager.RunQuickLogin()", ex);
                                    }

                                    if (quickLoginForm.RememberCredentials)
                                    {
                                        uniManager.SaveCredentials(quickLoginForm.UserName, Encryption.EncryptString(quickLoginForm.Password, _clientEncryptionKey));
                                        if (_userAppConfig.SynchronizeOGameCredentials)
                                            SynchronizeCredentialsAsync(uniManager);
                                        else
                                            LinkUserToUniverseAccountAsync(uniManager);
                                    }
                                    else
                                    {
                                        uniManager.SaveCredentialsUserName(quickLoginForm.UserName);
                                        if (!uniManager.GetCredentials().IsUserToUniverseAccountLinkCreated)
                                            LinkUserToUniverseAccountAsync(uniManager);
                                    }
                                }
                            };
                            #endregion worker.DoWork

                            #region worker.RunWorkerCompleted
                            worker.RunWorkerCompleted += (sender, e) =>
                            {
                                waitingForm.SetShowHide(false);
                            };
                            #endregion worker.RunWorkerCompleted

                            worker.RunWorkerAsync();

                            waitingForm.SetShowHide(true);
                            waitingForm.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                string errorMessage = "";
                if (uniManager.OnQuickLogin(uniManager.GetCredentials().UserName, Encryption.DecryptString(uniManager.GetCredentials().Password, _clientEncryptionKey), out errorMessage))
                {
                    try
                    {
                        if (uniManager.GetCredentials().PlayerId == 0)
                        {
                            uniManager.SetCredentialsIsUserToUniverseAccountLinkCreated(false);
                            Int64 playerId = SearchPlayerId(uniManager.GetUniverseId(), uniManager.GetCredentials().UserName);
                            if (playerId != -1 && playerId != 0)
                                uniManager.SaveCredentialsPlayerId(playerId);
                        }
                    }
                    catch (Exception ex)
                    {
                        CreateApplicationExceptionLog("GameManager.SearchPlayerId() has failed in GameManager.RunQuickLogin()", ex);
                    }

                    if (_userAppConfig.SynchronizeOGameCredentials && !uniManager.GetCredentials().AreCredentialsSyncToServer)
                        SynchronizeCredentialsAsync(uniManager);
                    else if (!uniManager.GetCredentials().IsUserToUniverseAccountLinkCreated)
                        LinkUserToUniverseAccountAsync(uniManager);
                }
                else if (errorMessage.Equals(Constants.LOGIN_FAILED))
                {
                    uniManager.SaveCredentialsPassword(null);

                    if (_userAppConfig.SynchronizeOGameCredentials)
                        SynchronizeCredentialsAsync(uniManager);
                    else if (!uniManager.GetCredentials().IsUserToUniverseAccountLinkCreated)
                        LinkUserToUniverseAccountAsync(uniManager);
                }
            }
        }


        private bool RunLoginWithInternetExplorer(UniManager uniManager)
        {
            using (var loginWithIEForm = new LoginWithIE(this, GetCommunityPortalUrl(uniManager.GetCommunityId()), uniManager.GetUniverse()))
            {
                loginWithIEForm.WindowTitle = "Login to " + uniManager.GetUniverseDomain();
                DialogResult result = loginWithIEForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return uniManager.OnLoggedInViaGameLoginBrowser(loginWithIEForm.RetUrl);
                }
                else
                {
                    return false;
                }
            }
        }


        private bool RunLoginWithUrl(UniManager uniManager)
        {
            using (var loginWindowForm = new LoginWithUrl())
            {
                loginWindowForm.WindowTitle = "Login to " + uniManager.GetUniverseDomain();
                DialogResult result = loginWindowForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return uniManager.OnLoggedInWithUrlLogin(loginWindowForm.RetUrl);
                }
                else
                {
                    return false;
                }
            }
        }


        private Universe GetUniverseData(List<Universe> universeList, string universeId)
        {
            if (universeList == null || universeList.Count == 0)
                return null;
            else
            {
                foreach (Universe universe in universeList)
                {
                    if (universe.Id.ToLower().Equals(universeId.ToLower()))
                        return universe;
                }
                return null;
            }
        }


        private void applicationSessionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_createdSession)
                    UpdateApplicationSessionAsync(false);
                else if (!_creatingSession)
                    CreateApplicationSessionAsync();
            }
            catch (Exception ex)
            {
                // Log it
            }
        }


        private void ShowWaitingForm()
        {
            using (var waitingForm = new WaitingForm())
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (sender, e) =>
                {
                    System.Threading.Thread.Sleep(5000);
                };

                worker.RunWorkerCompleted += (sender, e) =>
                {
                    waitingForm.SetShowHide(false);
                };

                worker.RunWorkerAsync();

                waitingForm.SetShowHide(true);
                DialogResult result = waitingForm.ShowDialog();
            }
        }


        #region SecureObject


        private bool HasUniverseRoleListSecureObject(string secureObjectName, List<UniverseRole> universeRoleList)
        {
            foreach (UniverseRole universeRole in universeRoleList)
            {
                foreach (SecureObject secureObject in universeRole.SecureObjectList)
                {
                    if (secureObject.Name.Equals(secureObjectName))
                        return true;
                }
            }
            return false;
        }

        private List<UniverseRole> GetCommunityUniverseRoleList(string communityId)
        {
            return GetCommunityList().FirstOrDefault(p => p.Id.Equals(communityId)).UniverseRoleList;
        }

        private UniverseRole GetCommunityUniverseRole(string communityId, string universeRoleId)
        {
            return GetCommunityUniverseRoleList(communityId).FirstOrDefault(p => p.Id.Equals(universeRoleId));
        }


        #endregion SecureObject


        #region Utilities Calls


        private T GetDataFromFile<T>(string fileName, string password)
        {
            CheckAppDirectory();
            if (_isDataDirectorySet)
                CheckDataDirectory();

            if (File.Exists(fileName))
            {
                try
                {
                    return Utilities.DeSerializeFromFile<T>(fileName, password);
                }
                catch { }
            }
            return default(T);
        }


        private bool SaveDataToFile<T>(T obj, string fileName, string password)
        {
            CheckAppDirectory();
            if (_isDataDirectorySet)
                CheckDataDirectory();

            try
            {
                return Utilities.SerializeToFile<T>(obj, fileName, password);
            }
            catch
            {
                return false;
            }

        }


        #endregion Utilities Calls


        #endregion ----- Private Methods ------


        /***********************************************************************************************************/


        #region ----- Internal Methods ------


        internal List<Universe> GetApplicationUniverseList()
        {
            return GetDataFromFile<List<Universe>>(_universeDataFile, "");
        }


        #endregion ----- Internal Methods ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        #region SecureObject


        public bool HasAnyCommunitySecureObject(string secureObjectName)
        {
            foreach (Community community in GetCommunityList())
            {
                if (HasCommunitySecureObject(secureObjectName, community.Id))
                    return true;
            }
            return false;
        }

        public bool HasCommunitySecureObject(string secureObjectName, string communityId)
        {
            return HasUniverseRoleListSecureObject(secureObjectName, GetCommunityUniverseRoleList(communityId));
        }

        public bool HasUniverseSecureObject(string secureObjectName, string universeId)
        {
            Universe universe = _universeList.FirstOrDefault(p => p.Id.Equals(universeId));
            if (universe == null)
                return false;

            UniverseRole universeRole = GetCommunityUniverseRole(universe.CommunityId, universe.UniverseRoleId);

            if (universeRole == null)
                return false;

            foreach (SecureObject secureObject in universeRole.SecureObjectList)
            {
                if (secureObject.Name.Equals(secureObjectName))
                    return true;
            }
            return false;
        }


        #endregion SecureObject


        public Int64 SearchPlayerId(string universeId, string playerName)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return -1;

            string htmlContent = uniManager.GetAdminQuickNickSearchPage(playerName);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return -1;
            }
            else
            {
                return ParseHtml.GetPlayerIdByPlayerName(htmlContent, playerName);
            }
        }


        public string RetrievePlayerEmail(string universeId, int playerId)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return null;

            string htmlContent = uniManager.GetAdminPlayerOverviewPage(playerId);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
            {
                return ParseHtml.GetPlayerEmail(htmlContent);
            }
        }


        public List<List<Account>> RetrieveMatchingData(string universeId, bool email, bool ip, bool alliance)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return null;

            string htmlContent = uniManager.GetAdminMatchingDataPage(false, ip, email, alliance);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
            {
                string[] rawMultiLogs = ParseHtml.GetRawMultiLogs(htmlContent);
                return ParseHtml.GetMultiLogsFromRaw(rawMultiLogs);
            }
        }


        public List<IpLog> RetrieveLoginsIP(string universeId, string nick, int playerId, DateTime from, DateTime to)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return null;

            string htmlContent = uniManager.GetAdminLoginsPage(playerId.ToString(), from, to);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
            {
                return ParseHtml.GetLoginsIPListForMulti(htmlContent, nick);
            }
        }


        public int AddLongNoteToPlayer(string universeId, int playerId, string note)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return -1;

            string htmlContent = uniManager.PostAdminPlayerLongNote(playerId, note);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return -1;
            }
            else
            {
                if (ParseHtml.IsNoteSuccessfull(htmlContent))
                    return 1;
                else
                    return 0;
            }
        }


        #region Logs / Stats


        public List<ATUser> GetUserListFromOperatorSummary(string universeId)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return null;

            string htmlContent = uniManager.GetAdminOperatorsSummaryPage();

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
            {
                return ParseHtml.GetUserListFromOperatorSummary(htmlContent);
            }
        }


        public List<ATLog> GetLogs(string universeId, string search, int pageNumber, out string atClickStats)
        {
            UniManager uniManager = GetUniManager(universeId);
            atClickStats = "";
            if (!uniManager.IsSessionSet(true))
                return null;

            string htmlContent = uniManager.GetAdminLogsPage(search, pageNumber);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
            {
                List<ATLog> logList = ParseHtml.GetLogs(htmlContent);
                if (string.IsNullOrEmpty(atClickStats))
                {
                    atClickStats = ParseHtml.GetATClicks(htmlContent, search);
                }
                return logList;
            }
        }


        #endregion Logs / Stats


        #region Notes Methods


        public List<Note> UniverseGeneralNotesList(string universeId)
        {
            return _universeDataList.SingleOrDefault(r => r.UniverseId.Equals(universeId)).GeneralNotesList;
        }


        public List<Note> UniversePersonalNotesList(string universeId)
        {
            return _universeDataList.SingleOrDefault(r => r.UniverseId.Equals(universeId)).PersonalNotesList;
        }


        public void GetUniverseGeneralAndPersonalNotes(string universeId, out List<Note> generalNoteList, out List<Note> personalNoteList)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
            {
                generalNoteList = null;
                personalNoteList = null;
                return;
            }

            string htmlContent = uniManager.GetAdminHomePage();

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                generalNoteList = null;
                personalNoteList = null;
                return;
            }
            else
            {
                generalNoteList = ParseHtml.GetGeneralNotes(htmlContent, universeId);
                personalNoteList = ParseHtml.GetPersonalNotes(htmlContent, universeId);
            }
        }


        public List<Note> GetUniverseGeneralNotes(string universeId)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return null;

            List<Note> generalNoteList = ParseHtml.GetGeneralNotes(uniManager.GetAdminHomePage(), universeId);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
            {
                return generalNoteList;
            }
        }


        public List<Note> GetUniversePersonalNotes(string universeId)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return null;

            List<Note> personalNoteList = ParseHtml.GetPersonalNotes(uniManager.GetAdminHomePage(), universeId);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
                return personalNoteList;
        }


        public Note GetUniverseNoteDetails(Enums.NOTE_TYPE noteType, string universeId, int noteId)
        {
            UniManager uniManager = GetUniManager(universeId);

            Note note;

            if (noteType.Equals(Enums.NOTE_TYPE.PERSONAL))
                note = UniversePersonalNotesList(universeId).SingleOrDefault(r => r.Id == noteId);
            else
                note = UniverseGeneralNotesList(universeId).SingleOrDefault(r => r.Id == noteId);

            if (note == null || !string.IsNullOrEmpty(note.Content))
                return note;

            if (!uniManager.IsSessionSet(true))
                return null;

            note = ParseHtml.GetAdminNoteDetails(uniManager.GetAdminReadNotePage(noteId), note);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                return null;
            }
            else
                return note;
        }


        public void DeleteAdminNote(string universeId, int noteId, out List<Note> generalNoteList, out List<Note> personalNoteList)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
            {
                generalNoteList = null;
                personalNoteList = null;
                return;
            }

            string htmlContent = uniManager.GetAdminDeleteNotePage(noteId);

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
                generalNoteList = null;
                personalNoteList = null;
                return;
            }
            else
            {
                generalNoteList = ParseHtml.GetGeneralNotes(htmlContent, universeId);
                personalNoteList = ParseHtml.GetPersonalNotes(htmlContent, universeId);
            }
        }


        /*
        public bool DeleteAdminNote_original(string universeId, int noteId)
        {
            UniManager uniManager = GetUniManager(universeId);
            if (!uniManager.IsSessionSet(true))
                return false;

            List<Note> generalNotesList = ParseHtml.GetGeneralNotes(uniManager.GetAdminDeleteNotePage(noteId), universeId);
            bool noteIsDeleted = false;

            if (!uniManager.IsSessionStatusValid())
            {
                OnNotifyLoggedOut(uniManager);
            }
            else
            {
                noteIsDeleted = true;
                foreach (Note note in generalNotesList)
                {
                    if (note.Id == noteId)
                        noteIsDeleted = false;

                    if (!uniManager.GetUniverse().GeneralNotesList.Exists(r => r.Id == note.Id))
                        uniManager.GetUniverse().GeneralNotesList.Add(note);
                }
                List<Note> removedNoteList = new List<Note>();
                foreach (Note note in uniManager.GetUniverse().GeneralNotesList)
                {
                    if (!generalNotesList.Exists(r => r.Id == note.Id))
                        removedNoteList.Add(note);
                }
                foreach (Note note in removedNoteList)
                {
                    uniManager.GetUniverse().GeneralNotesList.Remove(note);
                }
            }
            return noteIsDeleted;
        }
        */


        #endregion Notes Methods


        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        public bool LockUnLockApplication()
        {
            return UnlockApplication();
        }


        public bool SaveApplicationConfig()
        {
            if (_appConfig != null)
                return SaveDataToFile<AppConfig>(_appConfig, _appConfigDataFile, "");
            else
                return false;
        }


        public void SaveApplicationData()
        {
            if (!_appConfig.IsPublicComputer)
            {
                SaveDataToFile<UserData>(_userData, _userDataFile, _clientEncryptionKey);
                SaveDataToFile<List<UniverseData>>(_universeDataList, _universeDataFile, _clientEncryptionKey);
            }
            SaveDataToFile<UserAppConfig>(_userAppConfig, _userAppConfigDataFile, _clientEncryptionKey);
        }


        public void CheckApplicationExceptionLog()
        {
            try
            {
                while (_sendingExceptionLog)
                {
                    Thread.Sleep(200);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                // Log it
            }
        }


        public void EndApplicationSession()
        {
            try
            {
                _applicationSessionTimer.Enabled = false;

                while ((_creatingSession && !_createdSession) || _updatingSession)
                {
                    Thread.Sleep(200);
                    Application.DoEvents();
                }

                if (_createdSession)
                    UpdateApplicationSession(true);
            }
            catch (Exception ex)
            {
                // Log it
            }
        }


        public void ReloadApplicationData()
        {
            _isInitialized = false;

            SaveApplicationData();

            UpdateCommunityList();

            if (_communityList != null && _communityList.Count == 0)
                throw new Exception("It seems that you have been unallocated from all your universes.\r\n\r\nPlease contact your game administrator for more information.");
            else
                LoadApplicationData();
        }


        public void CheckForUpdates()
        {
            string toolLatestVersion = GetToolLatestVersion();

            if (!string.IsNullOrEmpty(toolLatestVersion))
            {
                int result = EssentialUtil.CompareVersion(_toolVersion, toolLatestVersion.Split('|')[0]);
                if (result == 0) // same
                {
                    MessageBox.Show("You are already using the latest version.", "No Update Available!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result < 0) // _toolVersion is < toolLatestVersion
                {
                    if (MessageBox.Show("A new version of this application is available!\r\n\r\nWould you like to download it now?", "Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Utilities.OpenDefaultWebBrowser("https://celestos.azurewebsites.net/Login.aspx");
                    }

                    // Force update, then close the app
                    //if (MessageBox.Show("A new version of this application is available.\r\n\r\nPlease go to our website and download the update now as your current version is no longer active.\r\n\r\nThank you!", "Update Available!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    //{
                    //    Utilities.OpenWebBrowser("https://celestos.azurewebsites.net/Login.aspx");
                    //}
                }
                else if (result > 0) // _toolVersion is > toolLatestVersion
                {

                }
            }
        }


        public void ShowDashboardTips(bool forceShow)
        {
            if (_userAppConfig.ShowDashboardTips || forceShow)
            {
                using (var showHowToForm = new HowTo())
                {
                    DialogResult result = showHowToForm.ShowDialog();
                }
                if (_userAppConfig.ShowDashboardTips)
                    _userAppConfig.ShowDashboardTips = false;
            }
        }


        public void ShowOptions(bool forceShow)
        {
            if (_userAppConfig.ShowOptions || forceShow)
            {
                SetApplicationOptions();
                if (_userAppConfig.ShowOptions)
                    _userAppConfig.ShowOptions = false;
            }
        }


        public void ShowAbout()
        {
            using (var aboutForm = new About(Application.ProductName, _toolVersion, _buildDate))
            {
                DialogResult result = aboutForm.ShowDialog();
            }
        }


        public bool IsQuickLoginAllowed(string universeId)
        {
            return GetUniManager(universeId).IsCredentialsExist();
        }


        public void QuickLogin(string universeId)
        {
            RunQuickLogin(GetUniManager(universeId));
        }


        public bool LoginWithInternetExplorer(string universeId)
        {
            return RunLoginWithInternetExplorer(GetUniManager(universeId));
        }


        public bool LoginWithURL(string universeId)
        {
            return RunLoginWithUrl(GetUniManager(universeId));
        }


        public TicketManager GetTicketManager()
        {
            return _ticketManager;
        }


        public UserAppConfig GetUserApplicationConfig()
        {
            return _userAppConfig;
        }


        public string GetCommunityName(string communityId)
        {
            return _communityList.SingleOrDefault(r => r.Id.Equals(communityId)).Name;
        }


        public string GetCommunityId(string communityName)
        {
            return _communityList.SingleOrDefault(r => r.Name.ToLower().Equals(communityName.ToLower())).Id;
        }


        public Uri GetCommunityPortalUrl(string communityId)
        {
            return new Uri(_communityList.SingleOrDefault(r => r.Id.Equals(communityId)).PortalUrl, UriKind.Absolute);
        }


        public string GetSessionStatus(string universeId, out Color color, out FontStyle fontStyle)
        {
            switch (GetUniverseSessionStatus(universeId))
            {
                case (int)Enums.SESSION_STATUS.VALID:
                    color = Color.Green;
                    fontStyle = FontStyle.Bold;
                    return "Online";
                case (int)Enums.SESSION_STATUS.UNKNOWN:
                    color = Color.DarkOrange;
                    fontStyle = FontStyle.Italic;
                    return "Unknown";
                case (int)Enums.SESSION_STATUS.INVALID:
                    color = Color.Gray;
                    fontStyle = FontStyle.Regular;
                    return "Offline";
                default:
                    color = Color.Black;
                    fontStyle = FontStyle.Regular;
                    return string.Empty;
            }
        }


        public bool UniManagerExist(string uniId)
        {
            if (_uniManagerList == null || _uniManagerList.Count == 0)
                return false;
            else if (_uniManagerList.Exists(r => r.GetUniverseId().Equals(uniId)))
                return true;
            else
                return false;
        }


        public UniManager GetUniManager(string uniId)
        {
            try
            {
                return _uniManagerList.SingleOrDefault(r => r.GetUniverseId().Equals(uniId));
            }
            catch {
                return null;
            }
        }


        public string GetUniId(string communityId, int uniNumber)
        {
            try
            {
                return _universeList.FirstOrDefault(r => r.CommunityId.Equals(communityId) && r.Number == uniNumber).Id;
            }
            catch {
                return "";
            }
        }


        public string GetUniverseDomain(string uniId)
        {
            return _uniManagerList.SingleOrDefault(r => r.GetUniverseId().Equals(uniId)).GetUniverseDomain();
        }


        public bool IsUniverseSessionExist(string universeId)
        {
            try
            {
                return _uniManagerList.SingleOrDefault(r => r.GetUniverseId().Equals(universeId)).IsSessionExist();
            }
            catch {
                return false;
            }
        }


        public int GetUniverseSessionStatus(string universeId)
        {
            return _uniManagerList.SingleOrDefault(r => r.GetUniverseId().Equals(universeId)).GetSessionStatus();
        }


        public List<Community> GetCommunityList()
        {
            return _communityList;
        }


        public List<UniManager> UniManagerList
        {
            get { return _uniManagerList; }
            set
            {
                if (value == null)
                {
                    _uniManagerList.Clear();
                }
                else
                    _uniManagerList = value;
            }
        }


        public List<Universe> UniverseList
        {
            get { return _universeList; }
            set
            {
                if (value == null)
                {
                    _universeList.Clear();
                }
                else
                    _universeList = value;
            }
        }


        #endregion ----- Public Methods ------


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnNotifyLoggedOut(UniManager uniManager)
        {
            // Check if there are any Subscribers
            if (NotifyLoggedOut != null)
            {
                // Call the Event
                NotifyLoggedOut(uniManager.GetUniverse());
            }
        }


        //private bool CheckSessionStatus(string htmlContent)
        //{
        //        int status;

        //        if (htmlContent.StartsWith(Constants.SESSION_INVALID))
        //        {
        //            if (htmlContent.StartsWith(Constants.SESSION_EMPTY))
        //            {
        //                status = (int)Enums.SESSION_STATUS.INVALID;
        //                //OnErrorOccurred("Your session has expired.\n\nPlease login again to this universe.");
        //            }
        //            else if (htmlContent.Contains(Constants.PAGE_ERROR))
        //            {
        //                status = (int)Enums.SESSION_STATUS.VALID;
        //                //OnErrorOccurred("Error occurred.");
        //            }
        //            else
        //                status = (int)Enums.SESSION_STATUS.INVALID;
        //        }
        //        else
        //            status = (int)Enums.SESSION_STATUS.VALID;

        //        return _sessionStatus;
        //}


        /***********************************************************************************************************/


        #region ----- BackgroundWorker DoWork Methods ------


        // Obsolete
        void bgw_DoWorkQuickLogin(object sender, DoWorkEventArgs e)
        {
            //if (e.Argument != null)
            //{
            //    object[] arguments = (object[])e.Argument;
            //    UniManager uniManager = (UniManager)arguments[0];
            //    string errorMessage = "";
            //    if (uniManager.OnQuickLogin((string)arguments[1], (string)arguments[2], false, out errorMessage))
            //    {
            //        if (arguments[3].ToString().Equals("true"))
            //        {
            //            uniManager.SaveCredentials((string)arguments[1], Utilities.StringToMD5Hash((string)arguments[2]));
            //        }
            //    }
            //}
        }


        #endregion ----- BackgroundWorker DoWork Methods ------


        /***********************************************************************************************************/


        #region ----- Events Callback ------


        void uniManager_LogWebClientException(Uri requestUri, Exception ex)
        {
            CreateWebClientExceptionLogAsync(requestUri, ex);
        }


        void _ticketManager_LogWebClientException(Uri requestUri, Exception ex)
        {
            CreateWebClientExceptionLogAsync(requestUri, ex);
        }


        #endregion ----- Events Callback ------


        /***********************************************************************************************************/


        #region ----- Protected Fire Events ------



        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/

    }
}
