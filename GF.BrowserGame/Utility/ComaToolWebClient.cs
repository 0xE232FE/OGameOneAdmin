using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;
using System.Windows.Forms;
using GF.BrowserGame.Forms;
using System.Threading;

namespace GF.BrowserGame.Utility
{
    sealed internal class ComaToolWebClient
    {        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        public delegate void WebClientCredentialsEventHandler(string userName, string password);


        public delegate void LogWebClientExceptionEventHandler(Uri requestUri, Exception ex);


        public delegate void ErrorOccurredEventHandler(string errorMessage);


        public delegate void SessionStatusHandler(SessionStatusEventArgs onlineStatusEvent);


        public delegate void BytesDownloadedHandler(long bytesDownloaded);


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        public event WebClientCredentialsEventHandler WebClientCredentialsModified;


        public event LogWebClientExceptionEventHandler LogWebClientException;


        public event ErrorOccurredEventHandler ErrorOccurred;


        public event SessionStatusHandler SessionStatusChange;


        public event BytesDownloadedHandler BytesDownloaded;


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        private string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729)";
        private string _acceptType = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private string _contentType = "application/x-www-form-urlencoded";
        private string _acceptLanguage = "en-us,en;q=0.5";
        private string _acceptCharset = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
        private string _defaultUri = "https://lobby.ogame.gameforge.com/en_GB/";
        private DateTime _lastWebRequest = DateTime.Now;
        private string _userName = null;
        private string _password = null;
        private CookieContainer _cookiesContainer;
        private int _sessionStatus = (int)Enums.SESSION_STATUS.UNKNOWN;
        private object _locker = new object();

        public ComaToolWebClient(string userName, string password)
        {
            _userName = userName;
            _password = password;
            _cookiesContainer = new CookieContainer();
        }

        public ComaToolWebClient(string userName, string password, CookieContainer cookiesContainer)
        {
            _userName = userName;
            _password = password;
            if (_cookiesContainer != null)
                _cookiesContainer = null;
            _cookiesContainer = cookiesContainer;
        }

        public ComaToolWebClient(string userName, string password, List<WebClientCookie> cookies)
        {
            _userName = userName;
            _password = password;

            if (_cookiesContainer != null)
                _cookiesContainer = null;

            _cookiesContainer = new CookieContainer();

            if (cookies != null)
            {
                foreach (WebClientCookie gameCookie in cookies)
                {
                    AddCookie(gameCookie);
                }
            }
        }


        /***********************************************************************************************************/


        #region ----- Protected Fire Event ------


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnWebClientCredentialsModified(string userName, string password)
        {
            // Check if there are any Subscribers
            if (WebClientCredentialsModified != null)
            {
                // Call the Event
                WebClientCredentialsModified(userName, password);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnLogWebClientException(Uri requestUri, Exception ex)
        {
            // Check if there are any Subscribers
            if (LogWebClientException != null)
            {
                // Call the Event
                LogWebClientException(requestUri, ex);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnErrorOccurred(string errorMessage)
        {
            // Check if there are any Subscribers
            if (ErrorOccurred != null)
            {
                // Call the Event
                ErrorOccurred(errorMessage);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnSessionStatusChange(SessionStatusEventArgs sessionStatusEvent)
        {
            // Check if there are any Subscribers
            if (SessionStatusChange != null)
            {
                // Call the Event
                SessionStatusChange(sessionStatusEvent);
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnBytesDownloaded(long bytesDownloaded)
        {
            // Check if there are any Subscribers
            if (BytesDownloaded != null)
            {
                // Call the Event
                BytesDownloaded(bytesDownloaded);
            }
        }


        #endregion ----- Protected Fire Event ------


        /***********************************************************************************************************/


        public void ClearCookies()
        {
            if (_cookiesContainer != null)
                _cookiesContainer = null;
            _cookiesContainer = new CookieContainer();
            _sessionStatus = (int)Enums.SESSION_STATUS.INVALID;
        }

        public void AddCookie(string domain, string key, string value)
        {
            if (_cookiesContainer == null)
                _cookiesContainer = new CookieContainer();

            if (domain.StartsWith("http://"))
                domain = domain.Substring("http://".Length);

            Cookie cookie = new Cookie(key, value, "/", domain);
            _cookiesContainer.Add(cookie);
        }

        public void AddCookie(WebClientCookie gameCookie)
        {
            if (_cookiesContainer == null)
                _cookiesContainer = new CookieContainer();

            Cookie cookie = new Cookie(gameCookie.Key, gameCookie.Value, "/", gameCookie.Domain.StartsWith("http://") ? gameCookie.Domain.Substring("http://".Length) : gameCookie.Domain);
            _cookiesContainer.Add(cookie);
        }

        public void AddCookies(List<WebClientCookie> cookies)
        {
            if (cookies != null)
            {
                foreach (WebClientCookie gameCookie in cookies)
                {
                    AddCookie(gameCookie);
                }
            }
        }

        public void AddCookies(string cookieData)
        {
            if (cookieData == null || cookieData.Length == 0)
                return;

            foreach (string cookie in cookieData.Split(';'))
            {

            }
        }

        public List<WebClientCookie> GetCookies(string domain)
        {
            if (_cookiesContainer == null || _cookiesContainer.Count == 0)
                return new List<WebClientCookie>();

            List<WebClientCookie> cookies = new List<WebClientCookie>();

            foreach (Cookie cookie in _cookiesContainer.GetCookies(new Uri(domain)))
            {
                WebClientCookie httpCookie = new WebClientCookie();
                httpCookie.Key = cookie.Name;
                httpCookie.Value = cookie.Value;
                httpCookie.Domain = cookie.Domain;
                cookies.Add(httpCookie);
            }

            return cookies;
        }

        public void SetCookies(List<WebClientCookie> cookies)
        {
            if (_cookiesContainer != null)
                _cookiesContainer = null;

            _cookiesContainer = new CookieContainer();

            if (cookies != null)
            {
                foreach (WebClientCookie gameCookie in cookies)
                {
                    AddCookie(gameCookie);
                }
            }
        }

        public int GetCookiesCount()
        {
            if (_cookiesContainer == null)
                return 0;
            else
                return _cookiesContainer.Count;
        }

        public bool UseNetworkCredentials()
        {
            if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password))
                return true;
            else
                return false;
        }

        public bool IsSessionValid(string htmlContent)
        {
            int sessionStatus = CheckSessionStatus(htmlContent);

            if (sessionStatus == (int)Enums.SESSION_STATUS.VALID)
                return true;
            else
                return false;
        }

        public bool IsSessionValid(Uri requestUri, string referer)
        {
            string htmlContent = GetPage(requestUri, referer, false);

            int sessionStatus = CheckSessionStatus(htmlContent);

            if (sessionStatus == (int)Enums.SESSION_STATUS.VALID)
                return true;
            else
                return false;
        }

        public void SubmitActiveCommunity(Uri requestUri, string referer)
        {
            try
            {
                RequestDefaultUri();
                WebResponse response = GetWebResponseViaGetMethod(requestUri, referer, false);
                if (response != null)
                    response.Close();
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
            }
        }

        public void Logout(Uri requestUri, string referer)
        {
            try
            {
                WebResponse response = GetWebResponseViaGetMethod(requestUri, referer, false);
                if (response != null)
                    response.Close();
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
            }
        }

        #region GetPage

        private string LoadDefaultUri()
        {
            try
            {
                Uri requestUri = new Uri(_defaultUri);
                HttpWebRequest httpRequest;
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    httpRequest = PrepareGetRequest(requestUri, false, null);
                    response = SendGetRequest(httpRequest, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                if (response != null && response.Headers != null && response.Headers["Location"] != null && response.Headers["Location"].Length > 0)
                {
                    do
                    {
                        response.Close();
                        Uri location = GetRedirectLocation(requestUri, response.Headers["Location"].ToString());
                        response = null;
                        httpRequest = PrepareGetRequest(location, false, "");
                        response = SendGetRequest(httpRequest, out exception);
                        ParseWebException(response, exception, requestUri, out submitCredentials);
                    } while (submitCredentials);
                }

                return ReadResponseContent(response, false);
            }
            catch (Exception ex)
            {
                return Constants.ERROR;
            }
            finally
            {
            }
        }

        public string GetPage(Uri requestUri, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                HttpWebRequest httpRequest;
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    httpRequest = PrepareGetRequest(requestUri, false, null);
                    response = SendGetRequest(httpRequest, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                if (response != null && response.Headers != null && response.Headers["Location"] != null && response.Headers["Location"].Length > 0)
                {
                    do
                    {
                        response.Close();
                        Uri location = GetRedirectLocation(requestUri, response.Headers["Location"].ToString());
                        response = null;
                        httpRequest = PrepareGetRequest(location, false, "");
                        response = SendGetRequest(httpRequest, out exception);
                        ParseWebException(response, exception, requestUri, out submitCredentials);
                    } while (submitCredentials);
                }

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        public string GetPage(Uri requestUri, string referer, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                HttpWebRequest httpRequest;
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    httpRequest = PrepareGetRequest(requestUri, false, referer);
                    response = SendGetRequest(httpRequest, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                if (response != null && response.Headers != null && response.Headers["Location"] != null && response.Headers["Location"].Length > 0)
                {
                    do
                    {
                        response.Close();
                        Uri location = GetRedirectLocation(requestUri, response.Headers["Location"].ToString());
                        response = null;
                        httpRequest = PrepareGetRequest(location, false, referer);
                        response = SendGetRequest(httpRequest, out exception);
                        ParseWebException(response, exception, requestUri, out submitCredentials);
                    } while (submitCredentials);
                }

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        public string GetPage(Uri requestUri, string referer, bool allowRedirect, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                HttpWebRequest httpRequest;
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password))
                    allowRedirect = false;

                do
                {
                    httpRequest = PrepareGetRequest(requestUri, allowRedirect, referer);
                    response = SendGetRequest(httpRequest, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                if (response != null && response.Headers != null && response.Headers["Location"] != null && response.Headers["Location"].Length > 0)
                {
                    do
                    {
                        response.Close();
                        Uri location = GetRedirectLocation(requestUri, response.Headers["Location"].ToString());
                        response = null;
                        httpRequest = PrepareGetRequest(location, allowRedirect, referer);
                        response = SendGetRequest(httpRequest, out exception);
                        ParseWebException(response, exception, requestUri, out submitCredentials);
                    } while (submitCredentials);
                }

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        #endregion GetPage

        #region PostSynchronous

        public string PostSynchronous(Uri requestUri, Dictionary<string, string> postVariables, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, null);
                    string postString = dictionaryToPostString(postVariables);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        public string PostSynchronous(Uri requestUri, Dictionary<string, string> postVariables, string referer, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, referer);
                    string postString = dictionaryToPostString(postVariables);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        public string PostSynchronous(Uri requestUri, Dictionary<string, string> postVariables, string referer, bool allowRedirect, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, referer);
                    string postString = dictionaryToPostString(postVariables);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        public string PostSynchronous(Uri requestUri, string postString, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, null);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        public string PostSynchronous(Uri requestUri, string postString, string referer, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, referer);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        public string PostSynchronous(Uri requestUri, string postString, string referer, bool allowRedirect, bool checkSessionStatus)
        {
            try
            {
                RequestDefaultUri();
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, referer);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return ReadResponseContent(response, checkSessionStatus);
            }
            catch (Exception ex)
            {
                OnLogWebClientException(requestUri, ex);
                return Constants.ERROR;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        #endregion PostSynchronous

        ///// <summary>
        ///// Post data must be included in the requestUri.
        ///// E.g. http://www.domain.com/index.php?page=home&lang=en
        ///// Post data will be extracted and send via POST.
        ///// </summary>
        ///// <param name="requestUri"></param>
        ///// <param name="referer"></param>
        ///// <param name="allowRedirect"></param>
        ///// <param name="checkSessionStatus"></param>
        ///// <returns></returns>
        //public string PostSynchronous(Uri requestUri, string referer, bool allowRedirect, bool checkSessionStatus)
        //{
        //    HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, referer);
        //    byte[] postData = Encoding.ASCII.GetBytes(Utilities.GetPostStringFromUri(requestUri));
        //    return ReadResponseContent(SendPostRequest(httpRequest, postData), checkSessionStatus);
        //}

        private WebResponse GetWebResponseViaGetMethod(Uri requestUri, string referer, bool allowRedirect)
        {
            try
            {
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PrepareGetRequest(requestUri, allowRedirect, referer);
                    response = SendGetRequest(httpRequest, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return response;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        private WebResponse GetWebResponseViaPostMethod(Uri requestUri, string postString, string referer, bool allowRedirect)
        {
            try
            {
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, referer);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return response;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        private WebResponse GetWebResponseViaPostMethod(Uri requestUri, Dictionary<string, string> postVariables, string referer, bool allowRedirect)
        {
            try
            {
                WebClientRequestManager.WaitWhileMaxSimultaneousRequestIsReached();
                WebResponse response;
                Exception exception = null;
                bool submitCredentials = false;

                do
                {
                    submitCredentials = false;
                    HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, referer);
                    string postString = dictionaryToPostString(postVariables);
                    byte[] postData = Encoding.ASCII.GetBytes(postString);
                    response = SendPostRequest(httpRequest, postData, out exception);
                    ParseWebException(response, exception, requestUri, out submitCredentials);
                } while (submitCredentials);

                return response;
            }
            finally
            {
                WebClientRequestManager.ReleaseMaxRequestLockWait();
            }
        }

        private HttpWebRequest PrepareGetRequest(Uri requestUri, bool allowRedirect, string requestReferer)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            httpRequest.Timeout = 15000;
            httpRequest.UserAgent = _userAgent;
            httpRequest.Accept = _acceptType;
            httpRequest.Headers.Add("Accept-Language", _acceptLanguage);
            httpRequest.Headers.Add("Accept-Charset", _acceptCharset);
            httpRequest.AllowAutoRedirect = allowRedirect;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip;
            httpRequest.Method = "GET";

            if (UseNetworkCredentials())
            {
                //ICredentials credentials = new NetworkCredential(_userName, _password);
                //httpRequest.Credentials = credentials;

                byte[] authBytes = Encoding.UTF8.GetBytes((_userName + ":" + _password).ToCharArray());
                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(authBytes);
            }

            if (_cookiesContainer == null)
                _cookiesContainer = new CookieContainer();

            httpRequest.CookieContainer = _cookiesContainer;

            if (!string.IsNullOrEmpty(requestReferer))
                httpRequest.Referer = requestReferer;

            return httpRequest;
        }

        private HttpWebRequest PreparePostRequest(Uri requestUri, bool allowRedirect, string requestReferer)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            httpRequest.Timeout = 15000;
            httpRequest.UserAgent = _userAgent;
            httpRequest.Accept = _acceptType;
            httpRequest.ContentType = _contentType;
            httpRequest.Headers.Add("Accept-Language", _acceptLanguage);
            httpRequest.Headers.Add("Accept-Charset", _acceptCharset);
            httpRequest.AllowAutoRedirect = allowRedirect;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip;
            httpRequest.Method = "POST";

            if (UseNetworkCredentials())
            {
                //ICredentials credentials = new NetworkCredential(_userName, _password);
                //httpRequest.Credentials = credentials;

                byte[] authBytes = Encoding.UTF8.GetBytes((_userName + ":" + _password).ToCharArray());
                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(authBytes);
            }

            if (_cookiesContainer == null)
                _cookiesContainer = new CookieContainer();

            httpRequest.CookieContainer = _cookiesContainer;

            if (!string.IsNullOrEmpty(requestReferer))
                httpRequest.Referer = requestReferer;

            return httpRequest;
        }

        private WebResponse SendGetRequest(HttpWebRequest httpRequest, out Exception exception)
        {
            exception = null;
            try
            {
                return httpRequest.GetResponse();
            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }

        private WebResponse SendPostRequest(HttpWebRequest httpRequest, byte[] postData, out Exception exception)
        {
            exception = null;
            try
            {
                httpRequest.ContentLength = postData.Length;

                using (Stream postStream = httpRequest.GetRequestStream())
                {
                    postStream.Write(postData, 0, postData.Length);
                    postStream.Close();
                }
                return httpRequest.GetResponse();
            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }

        /// <summary>
        /// Read Response and return "error" if response is null
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string ReadResponseContent(WebResponse response, bool checkSessionStatus)
        {
            try
            {
                string responseContent = "";

                if (response != null)
                {
                    try
                    {
                        long bytesDownloaded = long.Parse(response.Headers["Content-Length"].ToString());
                        if (bytesDownloaded > 0)
                            OnBytesDownloaded(bytesDownloaded);
                    }
                    catch { }

                    HttpStatusCode statusCode = GetResponseStatusCode(response);

                    if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Found)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader responseStreamReader = new StreamReader(responseStream))
                            {
                                responseContent = responseStreamReader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        // should log the status code etc...
                        responseContent = Constants.ERROR;
                    }

                    response.Close();
                }
                else
                    responseContent = Constants.ERROR;

                if (checkSessionStatus)
                {
                    CheckSessionStatus(responseContent);
                    if (!string.IsNullOrEmpty(responseContent) && responseContent.StartsWith(Constants.SESSION_INVALID))
                        responseContent = "";
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int CheckSessionStatus(string htmlContent)
        {
            lock (_locker)
            {
                int status;

                if (htmlContent.Equals(Constants.ERROR))
                {
                    status = (int)Enums.SESSION_STATUS.UNKNOWN;
                    //OnErrorOccurred("Error occurred.");
                }
                else if (htmlContent.Contains("ticket/index.php") || htmlContent.Contains("index.php?page=ticket"))
                    status = (int)Enums.SESSION_STATUS.VALID;
                else
                    status = (int)Enums.SESSION_STATUS.INVALID;

                if (_sessionStatus != status)
                {
                    _sessionStatus = status;

                    SessionStatusEventArgs sessionStatusEvent = new SessionStatusEventArgs(_sessionStatus);

                    OnSessionStatusChange(sessionStatusEvent);
                }
                return _sessionStatus;
            }
        }

        private Uri GetRedirectLocation(Uri requestUri, string location)
        {
            if (!location.StartsWith("http://") && !location.StartsWith("https://"))
            {
                if (location.Contains(requestUri.Host))
                    location = "http://" + location;
                else if (location.StartsWith("/"))
                    location = "http://" + requestUri.Host + location;
                else
                    location = "http://" + requestUri.Host + "/" + location;
            }
            return new Uri(location);
        }

        private HttpStatusCode GetResponseStatusCode(WebResponse response)
        {
            if (response == null)
                return HttpStatusCode.RequestTimeout;

            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)response;
                return webResponse.StatusCode;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private void RequestDefaultUri()
        {
            lock (_locker)
            {
                TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan lastRequest = new TimeSpan(_lastWebRequest.Ticks);
                if (now.TotalSeconds - lastRequest.TotalSeconds > (60 * 10))
                {
                    LoadDefaultUri();
                }

                _lastWebRequest = DateTime.Now;
            }
        }

        private void ParseWebException(WebResponse response, Exception exception, Uri requestUri, out bool submitCredentials)
        {
            submitCredentials = false;
            if (response == null)
            {
                if (exception.GetType() == typeof(WebException) && ((WebException)exception).Response != null)
                {
                    response = ((WebException)exception).Response;
                    HttpStatusCode statusCode = GetResponseStatusCode(response);
                    RequestWindowsAuthentication(statusCode, requestUri, out submitCredentials);
                    if (submitCredentials)
                    {
                        response.Close();
                        response = null;
                    }
                    else
                        throw exception;
                }
                else
                {
                    throw exception;
                }
            }
        }

        private void RequestWindowsAuthentication(HttpStatusCode statusCode, Uri requestUri, out bool submitCredentials)
        {
            submitCredentials = false;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    using (var windowsAuthenticationForm = new WindowsAuthentication(requestUri.Host))
                    {
                        DialogResult result = windowsAuthenticationForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            _userName = windowsAuthenticationForm.userName;
                            _password = windowsAuthenticationForm.password;
                            OnWebClientCredentialsModified(_userName, _password);
                            submitCredentials = true;
                        }
                        else
                            OnWebClientCredentialsModified(null, null);
                    }
                }
                catch (Exception ex)
                {
                    submitCredentials = false;
                }
            }
        }

        private string dictionaryToPostString(Dictionary<string, string> postVariables)
        {
            string postString = "";
            foreach (KeyValuePair<string, string> pair in postVariables)
            {
                postString += HttpUtility.UrlEncode(pair.Key) + (!string.IsNullOrEmpty(pair.Value) ? "=" +
                    HttpUtility.UrlEncode(pair.Value) : "") + "&";
            }
            postString = postString.Remove(postString.Length - 1, 1);
            return postString;
        }

        private Dictionary<string, string> postStringToDictionary(string postString)
        {
            char[] delimiters = { '&' };
            string[] postPairs = postString.Split(delimiters);

            Dictionary<string, string> postVariables = new Dictionary<string, string>();
            foreach (string pair in postPairs)
            {
                char[] keyDelimiters = { '=' };
                string[] keyAndValue = pair.Split(keyDelimiters);
                if (keyAndValue.Length > 1)
                {
                    postVariables.Add(HttpUtility.UrlDecode(keyAndValue[0]),
                        HttpUtility.UrlDecode(keyAndValue[1]));
                }
            }

            return postVariables;
        }
    }
}
