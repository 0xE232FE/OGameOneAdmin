using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using LibCommonUtil;
using GF.BrowserGame.Static;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.net.celestos.ogameServiceV1;

namespace GF.BrowserGame.Utility
{
    sealed internal class OgameServiceV1Call
    {
        private Guid _toolId;
        private string _toolVersion;
        private object _lock = new object();
        private DateTime _lastPingDateTime = DateTime.MinValue;


        private AuthHeader _soapHeader;
        internal AuthHeader SoapHeader
        {
            get { return _soapHeader; }
        }

        private List<String> _webServiceSettingsUrlList = new List<string>();
        private int _currentWebServiceSettingsUrlIndex = 0;

        public WebServiceSettings _webServiceSettings = new WebServiceSettings();
        private WebServiceUrl _currentWebServiceUrl;
        private int _currentWebServiceUrlIndex = 0;
        private bool _currentWebserviceUrlUp = false;

        public OgameServiceV1Call(Guid toolId, string toolVersion, Guid? applicationKey, WebServiceSettings webServiceSettings)
        {
           _toolId = toolId;
            _toolVersion = toolVersion;
            _webServiceSettings = webServiceSettings;

            SetWebServiceSettingsUrl();

            _soapHeader = new AuthHeader();
            _soapHeader.ToolId = toolId;
            _soapHeader.ApplicationKey = applicationKey;
            _soapHeader.Username = null;
            _soapHeader.Password = null;
            RegisterSoapExtension();
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
        }

        public OgameServiceV1Call(Guid toolId, string toolVersion, Guid? applicationKey, string userName, string password, WebServiceSettings webServiceSettings)
        {
            _toolId = toolId;
            _toolVersion = toolVersion;
            _webServiceSettings = webServiceSettings;

            SetWebServiceSettingsUrl();

            _soapHeader = new AuthHeader();
            _soapHeader.ToolId = toolId;
            _soapHeader.ApplicationKey = applicationKey;
            _soapHeader.Username = userName;
            _soapHeader.Password = password;
            RegisterSoapExtension();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
        }

        internal void UpdateWebServiceSettings(WebServiceSettings webServiceSettings)
        {
            _webServiceSettings = webServiceSettings;
        }

        internal void UpdateSoapHeader(string password)
        {
            _soapHeader.Password = password;
        }

        internal void UpdateSoapHeader(Guid? applicationKey)
        {
            _soapHeader.ApplicationKey = applicationKey;
        }

        internal void UpdateSoapHeader(string userName, string password)
        {
            _soapHeader.Username = userName;
            _soapHeader.Password = password;
        }

        // WebServiceSettings Url

        private void SetWebServiceSettingsUrl()
        {
            if (_webServiceSettingsUrlList == null)
                _webServiceSettingsUrlList = new List<string>();

            try
            {
                /* Live */
                _webServiceSettingsUrlList.Add(string.Format(@"https://celestos.s3-ap-southeast-2.amazonaws.com/webservice-ogamelive-{0}-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));
                //_webServiceSettingsUrlList.Add(string.Format(@"https://celestos.s3-ap-southeast-2.amazonaws.com/webservice-ogamelive-http-{0}-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));

                //https://celestos.s3-ap-southeast-2.amazonaws.com/webservice-ogamelive-http-8483201b3210-v1.4.xml
                //_webServiceSettingsUrlList.Add(string.Format(@"http://www.celestos.net/static/gameforge/ogame/webservice-{0}-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));
                //_webServiceSettingsUrlList.Add(string.Format(@"http://celestos.somee.com/static/gameforge/ogame/webservice-{0}-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));


                /* Dev
                _webServiceSettingsUrlList.Add(string.Format(@"https://www.celestos.net/static/gameforge/ogame/webservice-{0}-dev-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));
                //_webServiceSettingsUrlList.Add(string.Format(@"http://www.celestos.net/static/gameforge/ogame/webservice-{0}-dev-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));
                //_webServiceSettingsUrlList.Add(string.Format(@"http://celestos.somee.com/static/gameforge/ogame/webservice-{0}-dev-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));
                _webServiceSettingsUrlList.Add(string.Format(@"http://staging.celestos.net/static/gameforge/ogame/webservice-{0}-dev-v{1}.xml", _toolId.ToString().Split('-')[4], _toolVersion));
                */
            }
            catch (Exception ex)
            {
                // Log it
            }
        }

        private int GetNumberOfWebServiceSettingsUrl()
        {
            if (_webServiceSettingsUrlList != null)
                return _webServiceSettingsUrlList.Count;
            else
                return 0;
        }

        private string GetCurrenWebServiceSettingsUrl()
        {
            if (_currentWebServiceSettingsUrlIndex >= GetNumberOfWebServiceSettingsUrl())
                _currentWebServiceSettingsUrlIndex = 0;

            if (GetNumberOfWebServiceSettingsUrl() > 0)
                return _webServiceSettingsUrlList.ElementAt(_currentWebServiceSettingsUrlIndex);
            else
                return null;
        }

        private string GetNextWebServiceSettingsUrl()
        {
            if (_currentWebServiceSettingsUrlIndex >= GetNumberOfWebServiceSettingsUrl() - 1)
                _currentWebServiceSettingsUrlIndex = 0;
            else
                _currentWebServiceSettingsUrlIndex++;

            return GetCurrenWebServiceSettingsUrl();
        }

        internal WebServiceSettings GetLatestWebServiceSettings(bool throwException)
        {
            WebServiceSettings webServiceSettings = null;
            WebBrowserClient webClient = new WebBrowserClient();
            string webServiceSettingsUrl = GetCurrenWebServiceSettingsUrl();

            int retries = 1;
        TryAgain:
            try
            {
                string webServiceSettingsString = webClient.GetPage(new Uri(webServiceSettingsUrl), "", true);

                if (!string.IsNullOrEmpty(webServiceSettingsString))
                {
                    try
                    {
                        webServiceSettingsString = SerializeDeserializeObject.DeserializeObject<String>(webServiceSettingsString);
                        webServiceSettingsString = Encryption.DecryptString(webServiceSettingsString);
                        webServiceSettings = SerializeDeserializeObject.DeserializeObject<WebServiceSettings>(webServiceSettingsString);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(Constants.Message.TRY_NEXT_URL);
                    }
                }
                else
                    throw new Exception(Constants.Message.TRY_NEXT_URL);
            }
            catch
            {
                if (retries < GetNumberOfWebServiceSettingsUrl())
                {
                    retries++;
                    webServiceSettingsUrl = GetNextWebServiceSettingsUrl();
                    goto TryAgain;
                }
                else if (throwException)
                    throw new Exception(Constants.Message.WEBSERVICE_SETTINGS_NOT_FOUND);
            }

            return webServiceSettings;
        }

        // WebService Url

        private int GetNumberOfWebServiceUrl()
        {
            if (_webServiceSettings != null && _webServiceSettings.WebServiceUrlList != null)
                return _webServiceSettings.WebServiceUrlList.Count;
            else
                return 0;
        }

        private WebServiceUrl GetCurrentWebServiceUrl()
        {
            if (_currentWebServiceUrlIndex >= GetNumberOfWebServiceUrl())
                _currentWebServiceUrlIndex = 0;

            if (GetNumberOfWebServiceUrl() > 0)
                return _webServiceSettings.WebServiceUrlList.ElementAt(_currentWebServiceUrlIndex);
            else
                return null;
        }

        private WebServiceUrl GetNextWebServiceUrl()
        {
            if (_currentWebServiceUrlIndex >= GetNumberOfWebServiceUrl() - 1)
                _currentWebServiceUrlIndex = 0;
            else
                _currentWebServiceUrlIndex++;

            return GetCurrentWebServiceUrl();
        }

        private void GetValidWebServiceUrl(bool throwException)
        {
            lock (_lock)
            {
                bool updateWebServiceSettingsWhenAllServicesAreDown = true;
                WebBrowserClient webClient = new WebBrowserClient();
                WebServiceUrl webServiceUrl = GetCurrentWebServiceUrl();

                if (_currentWebServiceUrl != null && (DateTime.Now - _lastPingDateTime).TotalSeconds <= 15)
                    return;

                int retries = 1;
            TryAgain:
                try
                {
                    if (webServiceUrl != null && PingWebService(webServiceUrl.Url, false))
                    {
                        _lastPingDateTime = DateTime.Now;
                        _currentWebServiceUrl = webServiceUrl;
                    }
                    else
                    {
                        throw new Exception(Constants.Message.TRY_NEXT_URL);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Equals(Constants.Message.TRY_NEXT_URL))
                    {
                        if (retries < GetNumberOfWebServiceUrl())
                        {
                            retries++;
                            webServiceUrl = GetNextWebServiceUrl();
                            goto TryAgain;
                        }
                        else if (updateWebServiceSettingsWhenAllServicesAreDown)
                        {
                            updateWebServiceSettingsWhenAllServicesAreDown = false;
                            WebServiceSettings tempWebServiceSettings = GetLatestWebServiceSettings(false);
                            if (tempWebServiceSettings != null && _webServiceSettings.SettingId != tempWebServiceSettings.SettingId)
                            {
                                _webServiceSettings.SettingId = tempWebServiceSettings.SettingId;
                                _webServiceSettings.WebServiceUrlList = tempWebServiceSettings.WebServiceUrlList;
                                _currentWebServiceUrlIndex = 0;
                                webServiceUrl = GetCurrentWebServiceUrl();
                                retries = 1;
                                goto TryAgain;
                            }
                        }
                        if (throwException)
                            throw new Exception(Constants.Message.WEBSERVICES_ARE_DOWN);
                    }
                    else if (throwException)
                        throw ex;
                }
            }
        }

        private bool PingWebService(string webServiceUrl, bool throwException)
        {
            bool returnValue = false;

            Service myService = null;

            try
            {
                myService = new Service();
                
                myService.Url = webServiceUrl;
                //myService.Url = "http://localhost:1618/Service.asmx";
                myService.AuthHeaderValue = _soapHeader;
                myService.Timeout = 5000;
                returnValue = myService.Ping();
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return returnValue;
        }

        private void RegisterSoapExtension()
        {
            LibCommonUtil.WebServiceSecurity.Client.AuthenticationSoapExtension customSoapExtension = new LibCommonUtil.WebServiceSecurity.Client.AuthenticationSoapExtension();
            RegisterExtension(customSoapExtension.GetType(), 1, System.Web.Services.Configuration.PriorityGroup.High);
        }

        private void RegisterExtension(Type extClass, int priority, System.Web.Services.Configuration.PriorityGroup group)
        {
            if (!extClass.IsSubclassOf(typeof(System.Web.Services.Protocols.SoapExtension)))
            {
                throw new ArgumentException("Type must be derived from SoapException.", "extClass");
            }

            if (priority < 1)
            {
                throw new ArgumentOutOfRangeException("priority", priority, "Priority must be greater or equal to 1.");
            }

            System.Web.Services.Configuration.WebServicesSection wsSection = System.Web.Services.Configuration.WebServicesSection.Current;

            foreach (System.Web.Services.Configuration.SoapExtensionTypeElement it in wsSection.SoapExtensionTypes)
            {
                if (it.Type == extClass)
                {
                    return; // already registered
                }
            }
            // make collection writable
            Type type = typeof(System.Configuration.ConfigurationElementCollection);
            System.Reflection.FieldInfo readOnly = type.GetField("bReadOnly", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            readOnly.SetValue(wsSection.SoapExtensionTypes, false);

            // add extension
            wsSection.SoapExtensionTypes.Add(new System.Web.Services.Configuration.SoapExtensionTypeElement(extClass, priority, group));
            // restore original collection state
            type = typeof(System.Configuration.ConfigurationElement);
            System.Reflection.MethodInfo method = type.GetMethod("ResetModified", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(wsSection.SoapExtensionTypes, null);
            method = type.GetMethod("SetReadOnly", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(wsSection.SoapExtensionTypes, null);
        }

        public Guid? RegisterApplication(Guid toolId, string computerName, bool throwException)
        {
            Guid? retValue = null;
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                retValue = myService.RegisterApplication(toolId, computerName);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return retValue;
        }

        public SetupAppObj SetupApplication(string prevToolVersion, string newToolVersion, bool throwException)
        {
            SetupAppObj returnObj = new SetupAppObj();
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                string retValue = myService.SetupApplication((Guid)_soapHeader.ToolId, (Guid)_soapHeader.ApplicationKey, prevToolVersion, newToolVersion);
                returnObj = SerializeDeserializeObject.DeserializeObject<SetupAppObj>(retValue);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return returnObj;
        }

        public void UpgradeApplicationVersion(string prevToolVersion, string newToolVersion, bool throwException)
        {
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                myService.UpgradeApplicationVersion((Guid)_soapHeader.ToolId, (Guid)_soapHeader.ApplicationKey, prevToolVersion, newToolVersion);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public Guid? IsUserAccountValid(bool throwException)
        {
            Guid? retValue = null;
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                retValue = (Guid)myService.IsUserAccountValid();
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return retValue;
        }

        public Guid? GetUserId(bool throwException)
        {
            Guid? retValue = null;
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                retValue = myService.GetUserId();
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return retValue;
        }

        public bool IsToolValid(Guid toolId, string toolVersion, bool throwException)
        {
            bool retValue = false;
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                retValue = myService.IsToolValid(toolId, toolVersion);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return retValue;
        }

        public bool IsUserAllowedToUseThisTool(Guid toolId, bool throwException)
        {
            bool retValue = false;
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                retValue = myService.IsUserAllowedToUseThisTool(toolId);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return retValue;
        }

        public string GetToolLatestVersion(Guid toolId, bool throwException)
        {
            string retValue = null;
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                retValue = myService.GetLatestToolVersion(toolId);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return retValue;
        }

        public Int64 StartApplicationSession(Guid toolId, string toolVersion, string computerName, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                return myService.StartApplicationSession((Guid)_soapHeader.ApplicationKey, toolId, toolVersion, computerName);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
                else
                    return 0;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public void UpdateApplicationSessionUserId(Int64 applicationSessionId, bool isPublicComputer, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                myService.UpdateApplicationSessionUserId((Guid)_soapHeader.ApplicationKey, applicationSessionId, isPublicComputer);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public void UpdateApplicationSession(Int64 applicationSessionId, DateTime lastActivity, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                myService.UpdateApplicationSession((Guid)_soapHeader.ApplicationKey, applicationSessionId, lastActivity);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public void EndApplicationSession(Int64 applicationSessionId, DateTime lastActivity, DateTime endTime, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                myService.EndApplicationSession((Guid)_soapHeader.ApplicationKey, applicationSessionId, lastActivity, endTime);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public List<GF.BrowserGame.Schema.Serializable.Community> GetUserCommunityList(Guid toolId, bool throwException)
        {
            List<GF.BrowserGame.Schema.Serializable.Community> communityList = new List<GF.BrowserGame.Schema.Serializable.Community>();
            Service myService = null;
            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                string retValue = myService.GetUserCommunityData(toolId);
                communityList = SerializeDeserializeObject.DeserializeObject<List<GF.BrowserGame.Schema.Serializable.Community>>(retValue);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
            return communityList;
        }

        public bool CreateApplicationExceptionLog(Guid toolId, string description, Exception e, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                return myService.CreateApplicationExceptionLog((Guid)_soapHeader.ApplicationKey, toolId, e.GetType().Name, description, e.Message, (string.IsNullOrEmpty(e.StackTrace) ? "" : Encryption.EncryptString(e.StackTrace)), EssentialUtil.GetInnerExceptionMessage(e));
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
                else
                    return false;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public bool CreateWebClientExceptionLog(Guid toolId, string url, Exception e, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                string statusCode = null;
                if (e.GetType() == typeof(WebException) && ((WebException)e).Response != null)
                    statusCode = ((HttpWebResponse)((WebException)e).Response).StatusDescription;
                return myService.CreateApplicationWebClientExceptionLog((Guid)_soapHeader.ApplicationKey, toolId, e.GetType().Name, url, statusCode, e.Message, (string.IsNullOrEmpty(e.StackTrace) ? "" : Encryption.EncryptString(e.StackTrace)), EssentialUtil.GetInnerExceptionMessage(e));
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
                else
                    return false;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public bool SynchronizeCredentials(Guid universeId, Int64 playerId, string playerName, string password, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                return myService.SynchronizeCredentials(universeId, playerId, playerName, password);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
                else
                    return false;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        public bool LinkUserToUniverseAccount(Guid universeId, Int64 playerId, string playerName, bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
                return myService.LinkUserAccountToUniverseAccount(universeId, playerId, playerName);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
                else
                    return false;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }

        private void WebServiceMethodTemplate(bool throwException)
        {
            Service myService = null;

            try
            {
                myService = new Service();
                GetValidWebServiceUrl(true);
                myService.Url = _currentWebServiceUrl.Url;
                myService.AuthHeaderValue = _soapHeader;
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
            finally
            {
                if (myService != null)
                    myService.Dispose();
                myService = null;
            }
        }
    }
}
