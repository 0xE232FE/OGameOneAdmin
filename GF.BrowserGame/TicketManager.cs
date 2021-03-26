using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using GF.BrowserGame.Utility;
using GF.BrowserGame.Static;
using GF.BrowserGame.Schema.Serializable;
using LibCommonUtil;
using System.Diagnostics;
using System.ComponentModel;
using GF.BrowserGame.Schema.Internal;

namespace GF.BrowserGame
{
    public class TicketManager
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------

        private ComaToolCredentials _credentials;
        private ComaToolSession _session;
        private ComaToolUri _ticketUri;
        private ComaToolWebClient _httpClient;
        private List<string> _comaToolCommunityList = new List<string>();
        private int _dateFormatId;
        private long _bytesDownloaded = 0;
        private object _locker = new object();


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        public delegate void LogWebClientExceptionEventHandler(Uri requestUri, Exception ex);


        public delegate void ErrorOccurredEventHandler(string errorMessage);


        public delegate void SessionInvalidEventHandler();


        public delegate void LoggedInEventHandler();


        public delegate void LoginFailedEventHandler();


        public delegate void LoggedOutEventHandler();


        public delegate void NotifyLoggedOutEventHandler();


        public delegate void BytesDownloadedEventHandler(long bytesDownloaded);


        public delegate void AsyncWorkerCompletedEventHandler();


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        public event LogWebClientExceptionEventHandler LogWebClientException;


        public event ErrorOccurredEventHandler ErrorOccurred;


        public event SessionInvalidEventHandler SessionInvalid;


        public event LoggedInEventHandler LoggedIn;


        public event LoginFailedEventHandler LoginFailed;


        public event LoggedOutEventHandler LoggedOut;


        public event NotifyLoggedOutEventHandler NotifyLoggedOut;


        public event BytesDownloadedEventHandler BytesDownloaded;


        public event AsyncWorkerCompletedEventHandler AsyncWorkerCompleted;


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public TicketManager(ComaToolCredentials credentials, ComaToolSession session)
        {
            if (credentials == null || session == null)
                throw new Exception("Could not initialize TicketManager");

            _ticketUri = new ComaToolUri();
            _credentials = credentials;
            _session = session;

            if (!_session.isNullOrEmpty())
            {
                _session.Status = (int)Enums.SESSION_STATUS.UNKNOWN;
                _httpClient = new ComaToolWebClient(_credentials.WebClientUsername, _credentials.WebClientPassword, _session.Cookies);
            }
            else
                _httpClient = new ComaToolWebClient(_credentials.WebClientUsername, _credentials.WebClientPassword);

            _httpClient.WebClientCredentialsModified += new ComaToolWebClient.WebClientCredentialsEventHandler(_httpClient_WebClientCredentialsModified);
            _httpClient.LogWebClientException += new ComaToolWebClient.LogWebClientExceptionEventHandler(_httpClient_LogWebClientException);
            _httpClient.ErrorOccurred += new ComaToolWebClient.ErrorOccurredEventHandler(httpClient_ErrorOccurred);
            _httpClient.SessionStatusChange += new ComaToolWebClient.SessionStatusHandler(OnSessionStatusChange);
            _httpClient.BytesDownloaded += new ComaToolWebClient.BytesDownloadedHandler(_httpClient_BytesDownloaded);
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Public Ticket Functions ------


        public bool UseCredentials()
        {
            if (_credentials != null && !string.IsNullOrEmpty(_credentials.UserName) && !string.IsNullOrEmpty(_credentials.Password))
                return true;
            else
                return false;
        }


        public bool UseUserNameCredential()
        {
            if (_credentials != null && !string.IsNullOrEmpty(_credentials.UserName))
                return true;
            else
                return false;
        }


        public bool IsSessionExist()
        {
            if (_session == null)
                return false;
            else
                return true;
        }


        public int GetSessionStatus()
        {
            if (IsSessionExist())
                return _session.Status;
            else
                return (int)Enums.SESSION_STATUS.INVALID;
        }


        public bool IsSessionStatusValid()
        {
            if (_session == null)
                return false;
            else
                return _session.isStatusValid();
        }


        public bool IsSessionStatusInvalid()
        {
            if (_session == null)
                return false;
            else
                return _session.isStatusInvalid();
        }


        public bool IsSessionStatusUnknown()
        {
            if (_session == null)
                return false;
            else
                return _session.isStatusUnknown();
        }


        public bool IsCredentialsExist()
        {
            return UseCredentials();
        }


        public ComaToolCredentials GetCredentials()
        {
            return _credentials;
        }

        public void RetrieveDateFormatId()
        {
            string htmlContent = _httpClient.GetPage(_ticketUri.GetProfileUri(), _ticketUri.GetIndexUri().AbsoluteUri, true);
            _dateFormatId = ParseHtml.GetDateFormatId(htmlContent);
        }


        public void RetrieveComaToolCommunityList()
        {
            string htmlContent = _httpClient.GetPage(_ticketUri.GetTicketIndexUri(), _ticketUri.GetIndexUri().AbsoluteUri, true);
            _comaToolCommunityList = ParseHtml.GetComaToolOGameCommunityList(htmlContent);
        }


        public List<string> GetComaToolCommunityList()
        {
            return _comaToolCommunityList;
        }


        public void GetMyTicket(CommunityTicket comTicket, int pageNumber)
        {
            if (!IsSessionSet(true))
            {
                return;
            }

            string htmlContent = _httpClient.GetPage(_ticketUri.GetMyTicketUri(comTicket.CommunityId, pageNumber), _ticketUri.GetTicketIndexUri().AbsoluteUri, true);

            if (!IsSessionStatusValid())
            {
                OnNotifyLoggedOut();
                return;
            }
            else
            {
                comTicket.TotalMyTicket = ParseHtml.GetTotalMyTicket(htmlContent);
                comTicket.TotalOpenTicket = ParseHtml.GetTotalOpenTicket(htmlContent);
                comTicket.MyTicketList = ParseHtml.GetTicketList(htmlContent);
                comTicket.MyTicketCurrentPageNr = ParseHtml.GetCurrentTicketPageNr(htmlContent);
                comTicket.MyTicketTotalPageNr = ParseHtml.GetTotalTicketPageNr(htmlContent);
            }
        }


        public void GetOpenTicket(CommunityTicket comTicket, int pageNumber)
        {
            if (!IsSessionSet(true))
            {
                return;
            }

            string htmlContent = _httpClient.GetPage(_ticketUri.GetOpenTicketUri(comTicket.CommunityId, pageNumber), _ticketUri.GetTicketIndexUri().AbsoluteUri, true);

            if (!IsSessionStatusValid())
            {
                OnNotifyLoggedOut();
                return;
            }
            else
            {
                comTicket.TotalMyTicket = ParseHtml.GetTotalMyTicket(htmlContent);
                comTicket.TotalOpenTicket = ParseHtml.GetTotalOpenTicket(htmlContent);
                comTicket.OpenTicketList = ParseHtml.GetTicketList(htmlContent);
                comTicket.OpenTicketCurrentPageNr = ParseHtml.GetCurrentTicketPageNr(htmlContent);
                comTicket.OpenTicketTotalPageNr = ParseHtml.GetTotalTicketPageNr(htmlContent);
            }
        }


        public void GetClosedTicket(CommunityTicket comTicket, int pageNumber)
        {
            if (!IsSessionSet(true))
            {
                return;
            }

            string htmlContent = _httpClient.GetPage(_ticketUri.GetClosedTicketUri(comTicket.CommunityId, pageNumber), _ticketUri.GetTicketIndexUri().AbsoluteUri, true);

            if (!IsSessionStatusValid())
            {
                OnNotifyLoggedOut();
                return;
            }
            else
            {
                comTicket.TotalMyTicket = ParseHtml.GetTotalMyTicket(htmlContent);
                comTicket.TotalOpenTicket = ParseHtml.GetTotalOpenTicket(htmlContent);
                comTicket.ClosedTicketList = ParseHtml.GetTicketList(htmlContent);
                comTicket.ClosedTicketCurrentPageNr = ParseHtml.GetCurrentTicketPageNr(htmlContent);
                comTicket.ClosedTicketTotalPageNr = ParseHtml.GetTotalTicketPageNr(htmlContent);
            }
        }


        public Ticket ReadTicket(CommunityTicket comTicket, Ticket ticket, out List<AnswerTemplate> answerTemplateList)
        {
            answerTemplateList = new List<AnswerTemplate>();

            if (!IsSessionSet(true))
            {
                return null;
            }

            string htmlContent = _httpClient.GetPage(_ticketUri.GetViewTicketUri(comTicket.CommunityId, ticket.TicketId, ticket.TicketValue), _ticketUri.GetOpenTicketUri(comTicket.CommunityId, comTicket.OpenTicketCurrentPageNr).AbsoluteUri, true);

            if (!IsSessionStatusValid())
            {
                OnNotifyLoggedOut();
                return null;
            }
            else
            {
                ticket = ParseHtml.GetTicketDetails(htmlContent, ticket);
                ticket.TicketMessageList = ParseHtml.GetTicketMessages(htmlContent, _dateFormatId);
                answerTemplateList = ParseHtml.GetAnswerTemplateDetails(htmlContent);
                return ticket;
            }
        }


        public string GetTicketAnswerTemplate(CommunityTicket comTicket, Ticket ticket, string link)
        {
            if (!IsSessionSet(true))
            {
                return "";
            }

            string htmlContent = _httpClient.GetPage(_ticketUri.GetViewTicketAnswerTemplateUri(comTicket.CommunityId, ticket.TicketId, ticket.TicketValue, link), _ticketUri.GetViewTicketUri(comTicket.CommunityId, ticket.TicketId, ticket.TicketValue).AbsoluteUri, true);

            if (!IsSessionStatusValid())
            {
                OnNotifyLoggedOut();
                return "";
            }
            else
            {
                return ParseHtml.GetTicketAnswerTemplate(htmlContent);
            }
        }


        public bool PostTicketAnswer(CommunityTicket comTicket, Ticket ticket, string answer)
        {
            if (!IsSessionSet(true))
            {
                return false;
            }

            string htmlContent = _httpClient.PostSynchronous(_ticketUri.GetViewTicketSubmitAnswerUri(comTicket.CommunityId, ticket.TicketId, ticket.TicketValue), GamePostData.GetComaAnswerPostData(answer, "0"), _ticketUri.GetViewTicketAnswerTemplateUri(comTicket.CommunityId, ticket.TicketId, ticket.TicketValue, "0").AbsoluteUri, true);

            if (!IsSessionStatusValid())
            {
                OnNotifyLoggedOut();
                return false;
            }
            else
            {
                return ParseHtml.IsTicketAnswerSuccessfull(htmlContent);
            }
        }


        public void SelectActiveCommunity(int communityId)
        {
            if (!IsSessionSet(true))
            {
                return;
            }

            _httpClient.SubmitActiveCommunity(_ticketUri.GetTicketIndex2Uri(communityId), _ticketUri.GetTicketIndexUri().AbsoluteUri);

            if (!IsSessionStatusValid())
            {
                OnNotifyLoggedOut();
                return;
            }
        }


        #endregion ----- Public Universe Functions ------


        #region Session/Login/Logout Functions


        internal ComaToolSession GetComaToolSession()
        {
            return _session;
        }


        public void SetInternetExplorerCookies()
        {
            if (_session != null)
            {
                foreach (WebClientCookie cookie in _session.Cookies)
                {
                    EssentialUtil.SetWinINETCookieString(_ticketUri.GetDomain(), null, cookie.Key + "=" + cookie.Value);
                }
            }
        }


        public bool SaveCredentials(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                OnErrorOccurred("Credentials could not be saved. Username or password is empty.");
                return false;
            }

            if (_credentials == null)
                _credentials = new ComaToolCredentials();

            _credentials.UserName = userName;
            _credentials.Password = password;
            return true;
        }


        public bool SaveCredentialsUserName(string userName)
        {
            if (_credentials == null)
                _credentials = new ComaToolCredentials();

            _credentials.UserName = userName;
            return true;
        }


        public bool SaveCredentialsPassword(string password)
        {
            if (_credentials == null)
                _credentials = new ComaToolCredentials();

            _credentials.Password = password;
            return true;
        }


        public bool OnLogin(string userName, string password, out string errorMessage)
        {
            return Login(userName, password, out errorMessage);
        }


        public bool Logout()
        {
            if (!IsSessionSet(false))
                return false;

            _httpClient.Logout(_ticketUri.GetLogoutUri(), _ticketUri.GetIndexUri().AbsoluteUri);

            return !IsSessionValid(false);
        }


        /// <summary>
        /// Fire an event if the session has not been initialised.
        /// Return True if session is valid, False if invalid
        /// </summary>
        /// <returns></returns>
        public bool IsSessionValid(bool fireEvent)
        {
            if (!IsSessionSet(fireEvent))
                return false;

            return _httpClient.IsSessionValid(_ticketUri.GetIndexUri(), "");
        }


        public void IsSessionValidAsync()
        {
            //BackgroundWorker bgw = new BackgroundWorker();
            //bgw.DoWork += new DoWorkEventHandler(bgw_DoWorkIsSessionValid);
            //bgw.RunWorkerAsync();
        }


        #endregion Session/Login/Logout Functions


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        /// <summary>
        /// Checks whether the session has been set to a value.
        /// It does not check whether the session is valid or invalid.
        /// </summary>
        /// <param name="fireEvent"></param>
        /// <returns></returns>
        internal bool IsSessionSet(bool fireEvent)
        {
            if (_session == null || _session.isNullOrEmpty())
            {
                if (fireEvent)
                    OnSessionInvalid();
                return false;
            }
            else
                return true;
        }


        private void ClearSession()
        {
            if (_session == null)
            {
                _session = new ComaToolSession();
            }

            _session.Reset((int)Enums.SESSION_STATUS.INVALID);
            _httpClient.ClearCookies();
            OnLoggedOut();
        }


        private void SetSession(List<WebClientCookie> cookies, int status)
        {
            if (_session == null)
                _session = new ComaToolSession();
            _session.SetSession(cookies, status);
        }


        private bool Login(string userName, string password, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                ClearSession();
                string htmlContent = _httpClient.PostSynchronous(_ticketUri.GetPostLoginUri(), GamePostData.GetComaToolLogin(userName, password), _ticketUri.GetLoginUri().AbsoluteUri, true, false);

                if (htmlContent.Contains("'index.php?page=login&action=check"))
                {
                    errorMessage = Constants.LOGIN_FAILED;
                    OnLoginFailed();
                    return false;
                }
                else if (!htmlContent.Equals(Constants.ERROR))
                {
                    SetSession(_httpClient.GetCookies(_ticketUri.GetDomain() + "/index.php"), (int)Enums.SESSION_STATUS.VALID);
                    
                    if (!IsSessionValid(false))
                    {
                        errorMessage = Constants.LOGIN_FAILED;
                        OnLoginFailed();
                        return false;
                    }

                    SetSession(_httpClient.GetCookies(_ticketUri.GetDomain() + "/index.php"), (int)Enums.SESSION_STATUS.VALID);

                    RetrieveDateFormatId();

                    RetrieveComaToolCommunityList();

                    SetInternetExplorerCookies();

                    OnLoggedIn();
                    return true;
                }
                else
                {
                    errorMessage = Constants.ERROR;
                    OnLoginFailed();
                    return false;
                }
            }
            catch (Exception ex)
            {
                ClearSession();
                errorMessage = Constants.ERROR;
                OnErrorOccurred(ex.Message);
                return false;
            }
        }


        #endregion ----- Private Methods ------


        /***********************************************************************************************************/


        #region ----- Events Callback ------


        void _httpClient_WebClientCredentialsModified(string userName, string password)
        {
            if (_credentials != null)
            {
                _credentials.WebClientUsername = userName;
                _credentials.WebClientPassword = password;
            }
        }


        void _httpClient_LogWebClientException(Uri requestUri, Exception ex)
        {
            OnLogWebClientException(requestUri, ex);
        }


        void httpClient_ErrorOccurred(string errorMessage)
        {
            OnErrorOccurred(errorMessage);
        }


        void OnSessionStatusChange(SessionStatusEventArgs onlineStatusEvent)
        {
            _session.Status = onlineStatusEvent.SessionState;
            if (onlineStatusEvent.SessionState == (int)Enums.SESSION_STATUS.VALID)
                OnLoggedIn();
            else if (onlineStatusEvent.SessionState == (int)Enums.SESSION_STATUS.INVALID)
                ClearSession(); // This method will trigger the OnLoggedOut() event
            else if (onlineStatusEvent.SessionState == (int)Enums.SESSION_STATUS.UNKNOWN)
            { 
                //OnSessionInvalid(); 
            }
        }


        void _httpClient_BytesDownloaded(long bytesDownloaded)
        {
            lock (_locker)
            {
                _bytesDownloaded += bytesDownloaded;
            }
            OnBytesDownloaded(bytesDownloaded);
        }


        #endregion ----- Events Callback ------


        /***********************************************************************************************************/


        #region ----- Protected Fire Events ------


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnNotifyLoggedOut()
        {
            // Check if there are any Subscribers
            if (NotifyLoggedOut != null)
            {
                // Call the Event
                NotifyLoggedOut();
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnSessionInvalid()
        {
            ClearSession();

            // Check if there are any Subscribers
            if (SessionInvalid != null)
            {
                // Call the Event
                SessionInvalid();
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
        protected void OnLoggedIn()
        {
            // Check if there are any Subscribers
            if (LoggedIn != null)
            {
                // Call the Event
                LoggedIn();
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnLoginFailed()
        {
            ClearSession();

            // Check if there are any Subscribers
            if (LoginFailed != null)
            {
                // Call the Event
                LoginFailed();
            }
        }


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnLoggedOut()
        {
            // Check if there are any Subscribers
            if (LoggedOut != null)
            {
                // Call the Event
                LoggedOut();
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


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="sessionStatusEvent"></param>
        protected void OnAsyncWorkerCompleted()
        {
            // Check if there are any Subscribers
            if (AsyncWorkerCompleted != null)
            {
                // Call the Event
                AsyncWorkerCompleted();
            }
        }


        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/
    }
}
