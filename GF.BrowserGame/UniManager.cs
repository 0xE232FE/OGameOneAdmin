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

namespace GF.BrowserGame
{
    public class UniManager
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private Universe _universe;
        private Credentials _credentials;
        private GameSession _session;
        private GameUri _gameUri;
        private OGameWebClient _httpClient;
        private long _bytesDownloaded = 0;
        private object _locker = new object();


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        public delegate void LogWebClientExceptionEventHandler(Uri requestUri, Exception ex);


        public delegate void ErrorOccurredEventHandler(Universe universe, string errorMessage);


        public delegate void SessionInvalidEventHandler(Universe universe);


        public delegate void LoggedInEventHandler(Universe universe);


        public delegate void LoginFailedEventHandler(Universe universe);


        public delegate void LoggedOutEventHandler(Universe universe);


        public delegate void NotifyLoggedOutEventHandler(Universe universe);


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


        public UniManager(Universe universe, Credentials credentials, GameSession session)
        {
            if (universe == null || credentials == null || session == null)
                throw new Exception("Could not initialize UniManager");

            _universe = universe;
            _gameUri = new GameUri(universe);
            _credentials = credentials;
            _session = session;

            if (!session.isNullOrEmpty())
            {
                _session.Status = (int)Enums.SESSION_STATUS.UNKNOWN;
                _httpClient = new OGameWebClient(_credentials.WebClientUsername, _credentials.WebClientPassword, _session.Cookies);
            }
            else
                _httpClient = new OGameWebClient(_credentials.WebClientUsername, _credentials.WebClientPassword);

            _httpClient.WebClientCredentialsModified += new OGameWebClient.WebClientCredentialsEventHandler(_httpClient_WebClientCredentialsModified);
            _httpClient.LogWebClientException += new OGameWebClient.LogWebClientExceptionEventHandler(_httpClient_LogWebClientException);
            _httpClient.ErrorOccurred += new OGameWebClient.ErrorOccurredEventHandler(httpClient_ErrorOccurred);
            _httpClient.SessionStatusChange += new OGameWebClient.SessionStatusHandler(OnSessionStatusChange);
            _httpClient.BytesDownloaded += new OGameWebClient.BytesDownloadedHandler(_httpClient_BytesDownloaded);
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Public Universe Functions ------


        public void UpdateGameUri()
        {
            _gameUri.SetupGameUri();
        }


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


        public string GetCommunityId()
        {
            if (_universe.CommunityId != null)
                return _universe.CommunityId;
            else
            {
                OnErrorOccurred("Error, community Id cannot be null or empty");
                return string.Empty;
            }
        }


        public string GetUniverseId()
        {
            if (_universe.Id != null)
                return _universe.Id;
            else
            {
                OnErrorOccurred("Error, universe Id cannot be null or empty");
                return string.Empty;
            }
        }


        public string GetUniverseName()
        {
            return _universe.Name;
        }


        public int GetUniverseNumber()
        {
            return _universe.Number;
        }


        public string GetUniverseDomain()
        {
            return _universe.Domain;
        }


        public bool IsSessionExist()
        {
            if (_session == null)
                return false;
            else
                return true;
        }


        /// <summary>
        /// Checks whether the session has been set to a value.
        /// It does not check whether the session is valid or invalid.
        /// </summary>
        /// <param name="fireEvent"></param>
        /// <returns></returns>
        public bool IsSessionSet(bool fireEvent)
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


        public Universe GetUniverse()
        {
            return _universe;
        }


        #endregion ----- Public Universe Functions ------


        #region Session/Login/Logout Functions


        internal Credentials GetCredentials()
        {
            return _credentials;
        }


        internal GameSession GetGameSession()
        {
            return _session;
        }


        internal bool SaveCredentials(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                OnErrorOccurred("Credentials could not be saved. Username or password is empty.");
                return false;
            }

            if (_credentials == null)
                _credentials = new Credentials();

            _credentials.UniverseId = _universe.Id;
            _credentials.UserName = userName;
            _credentials.Password = password;
            _credentials.AddDateTime = DateTime.UtcNow;
            _credentials.ModDateTime = DateTime.UtcNow;
            return true;
        }


        internal bool SaveCredentialsUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                OnErrorOccurred("Credentials could not be saved. Username is empty.");
                return false;
            }

            if (_credentials == null)
                _credentials = new Credentials();

            _credentials.UniverseId = _universe.Id;
            _credentials.UserName = userName;
            return true;
        }


        internal bool SaveCredentialsPassword(string password)
        {
            if (_credentials == null)
                _credentials = new Credentials();

            _credentials.UniverseId = _universe.Id;
            _credentials.Password = password;
            return true;
        }


        internal bool SaveCredentialsPlayerId(Int64 playerId)
        {
            if (playerId == -1)
            {
                OnErrorOccurred("PlayerId could not be saved. PlayerId is invalid.");
                return false;
            }

            if (_credentials == null)
                _credentials = new Credentials();

            _credentials.UniverseId = _universe.Id;
            _credentials.PlayerId = playerId;
            return true;
        }


        internal bool SetCredentialsIsUserToUniverseAccountLinkCreated(bool value)
        {
            if (_credentials == null)
                _credentials = new Credentials();

            _credentials.UniverseId = _universe.Id;
            _credentials.IsUserToUniverseAccountLinkCreated = value;
            return true;
        }


        internal bool OnQuickLogin(string userName, string password, out string errorMessage)
        {
            return Login(userName, password, out errorMessage);
        }


        internal bool OnLoggedInWithUrlLogin(string loginUrl)
        {
            ClearSession();
            Uri loginUri;

            try
            {
                loginUri = new Uri(loginUrl);
            }
            catch
            {
                loginUri = null;
            }

            if (loginUri != null && loginUri.Query.IndexOf("session=") != -1)
            {
                string session = "";

                try
                {
                    session = loginUri.Query.Substring(loginUri.Query.IndexOf("session=") + Constants.UrlParameters.SESSION.Length, Constants.SESSION_LENGTH);
                }
                catch
                {
                    OnErrorOccurred("The link you have provided is invalid.");
                    return false;
                }

                bool isUniverseValid = false;

                if (loginUri.AbsoluteUri.ToLower().StartsWith("http://" + _universe.Domain.ToLower()))
                    isUniverseValid = true;

                if (string.IsNullOrEmpty(session) || session.Length != Constants.SESSION_LENGTH)
                {
                    OnLoginFailed();
                    return false;
                }
                else if (!isUniverseValid)
                {
                    OnErrorOccurred("The link you have provided is invalid for this universe.");
                    return false;
                }
                else
                {
                    if (_session == null)
                    {
                        _session = new GameSession();
                        _session.UniverseId = _universe.Id;
                    }
                    _session.Session = session;

                    if (IsSessionValid())
                    {
                        SetSession(session, _httpClient.GetCookies(_gameUri.GetDomain()), (int)Enums.SESSION_STATUS.VALID);
                        OnLoggedIn();
                        return true;
                    }
                    else
                    {
                        OnLoginFailed();
                        return false;
                    }
                }
            }
            else
            {
                OnErrorOccurred("The link you have provided is invalid.");
                return false;
            }
        }


        internal bool OnLoggedInViaGameLoginBrowser(string loginUrl)
        {
            ClearSession();
            Uri loginUri;

            try
            {
                loginUri = new Uri(loginUrl);
            }
            catch
            {
                loginUri = null;
            }

            if (loginUri != null && (loginUri.Query.IndexOf("session=") != -1 || loginUri.Query.IndexOf(Constants.UrlParameters.PHPSESSID) != -1))
            {
                string session = "";

                try
                {
                    if (loginUri.Query.IndexOf(Constants.UrlParameters.SESSION) != -1)
                    {
                        session = loginUri.Query.Substring(loginUri.Query.IndexOf(Constants.UrlParameters.SESSION) + Constants.UrlParameters.SESSION.Length, Constants.SESSION_LENGTH);
                    }
                    else
                    {
                        session = loginUri.Query.Substring(loginUri.Query.IndexOf(Constants.UrlParameters.PHPSESSID) + Constants.UrlParameters.PHPSESSID.Length, Constants.SESSION_LENGTH);
                    }

                    List<WebClientCookie> cookies = new List<WebClientCookie>();

                    foreach (System.Net.Cookie cookie in EssentialUtil.GetUriCookieContainer(loginUri).GetCookies(loginUri))
                    {
                        if (cookie.Name.Contains("PHPSESSID") || cookie.Name.Contains("prsess") || cookie.Name.Contains("login"))
                        {
                            WebClientCookie httpCookie = new WebClientCookie();
                            httpCookie.Key = cookie.Name;
                            httpCookie.Value = cookie.Value;
                            httpCookie.Domain = cookie.Domain;
                            cookies.Add(httpCookie);
                        }
                    }
                    _httpClient.SetCookies(cookies);
                }
                catch
                {
                    session = string.Empty;
                }

                if (string.IsNullOrEmpty(session) || session.Length != Constants.SESSION_LENGTH)
                {
                    OnLoginFailed();
                    return false;
                }
                else
                {
                    if (_session == null)
                    {
                        _session = new GameSession();
                        _session.UniverseId = _universe.Id;
                    }
                    _session.Session = session;

                    if (IsSessionValid())
                    {
                        SetSession(session, _httpClient.GetCookies(_gameUri.GetDomain()), (int)Enums.SESSION_STATUS.VALID);
                        OnLoggedIn();
                        return true;
                    }
                    else
                    {
                        OnLoginFailed();
                        return false;
                    }
                }
            }
            else
            {
                OnErrorOccurred("Error occurred after login, game url is invalid.");
                return false;
            }
        }


        public bool Logout()
        {
            if (!IsSessionSet(true))
                return false;

            _httpClient.Logout(_gameUri.GetLogoutUri(_session.Session), "");

            return !IsSessionValid();
        }


        /// <summary>
        /// Fire an event if the session has not been initialised.
        /// Return True if session is valid, False if invalid
        /// </summary>
        /// <returns></returns>
        public bool IsSessionValid()
        {
            if (!IsSessionSet(true))
                return false;

            return _httpClient.IsSessionValid(_gameUri.GetAdminIndexUri(_session.Session), "");
        }


        public void IsSessionValidAsync()
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWorkIsSessionValid);
            bgw.RunWorkerAsync();
        }


        #endregion Session/Login/Logout Functions


        #region ----- Language Functions ------


        public void SetAdminToolLanguage(string language)
        {
            if (!IsSessionSet(true))
                return;

            _httpClient.PostSynchronous(_gameUri.GetAdminSetLanguageUri(_session.Session), GamePostData.GetAdminSetLanguage(language), "", true, true);
        }


        #endregion ----- Functions ------


        #region ----- Get General AT Page Functions ------


        internal string GetAdminIndexPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminIndexUri(_session.Session), "", true, true);
        }


        internal string GetAdminHomePage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminHomeUri(_session.Session), "", true, true);
        }


        internal string GetAdminReadNotePage(int noteId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminReadNoteUri(_session.Session, noteId), "", true, true);
        }


        internal string GetAdminDeleteNotePage(int noteId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminDeleteNoteUri(_session.Session, noteId), "", true, true);
        }


        internal string GetAdminInfosPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminInfosUri(_session.Session), "", true, true);
        }


        internal string GetAdminCheckPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminCheckUri(_session.Session), "", true, true);
        }


        internal string GetAdminCheckPage(string search)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminCheckUri(_session.Session), GamePostData.GetAdminSearch(search), "", true, true);
        }


        /// <summary>
        /// PageNumber starts from 1.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        internal string GetAdminPilloryPage(int pageNumber)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPilloryUri(_session.Session, pageNumber), "", true, true);
        }


        internal string GetAdminPilloryPage(string search)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPilloryUri(_session.Session, 1), GamePostData.GetAdminPillorySearch(search), "", true, true);
        }


        internal string GetAdminMatchingDataPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminMachingDataUri(_session.Session), "", true, true);
        }


        internal string GetAdminMatchingDataPage(bool password, bool ip, bool email, bool alliance)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminMachingDataUri(_session.Session), GamePostData.GetAdminMatchingData(password, ip, email, alliance), "", true, true);
        }


        internal string GetAdminPlayerListPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerListUri(_session.Session), "", true, true);
        }


        internal string GetAdminPlayerListPage(int pageNumber, int maxPerPage)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerListUri(_session.Session), GamePostData.GetAdminPlayerList(pageNumber, maxPerPage), "", true, true);
        }


        internal string GetAdminPlanetListPage(int pageNumber, int maxPerPage)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlanetListUri(_session.Session), GamePostData.GetAdminPlanetList(pageNumber, maxPerPage), "", true, true);
        }


        internal string GetAdminAllianceListPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminAllianceListUri(_session.Session), "", true, true);
        }


        internal string GetAdminAllianceListPage(int pageNumber, int maxPerPage, bool displayPicture, int pictureSize)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminAllianceListUri(_session.Session), GamePostData.GetAdminAllianceList(pageNumber, maxPerPage, displayPicture, pictureSize), "", true, true);
        }


        internal string GetAdminAccountTransferPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminAccountTransferUri(_session.Session), "", true, true);
        }


        internal string GetAdminAccountTransferPage(string playerName1, string playerName2, bool exchangePassword)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminAccountTransferUri(_session.Session), GamePostData.GetAdminAccountTransfer(playerName1, playerName2, exchangePassword), "", true, true);
        }


        internal string GetAdminCookiesPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminCookiesUri(_session.Session), "", true, true);
        }


        internal string GetAdminLoginsPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminLoginsUri(_session.Session), "", true, true);
        }


        internal string GetAdminLoginsPage(string search, DateTime startDate, DateTime endDate)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminLoginsUri(_session.Session), GamePostData.GetAdminLogins(search, startDate, endDate), "", true, true);
        }


        /// <summary>
        /// PageNumber starts from 0.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        internal string GetAdminTopListPage(int pageNumber)
        {
            return _httpClient.GetPage(_gameUri.GetAdminTopListUri(_session.Session, pageNumber), "", true, true);
        }


        internal string GetAdminReportsPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminReportsUri(_session.Session), "", true, true);
        }


        internal string GetAdminResumeReportPage(int reportedMessageId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminResumeReportUri(_session.Session, reportedMessageId), "", true, true);
        }


        internal string GetAdminForwardReportPage(int reportedMessageId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminForwardReportUri(_session.Session, reportedMessageId), "", true, true);
        }


        internal string GetAdminBanReportPage(int reportedMessageId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminBanReportUri(_session.Session, reportedMessageId), "", true, true);
        }


        internal string GetAdminRejectReportPage(int reportedMessageId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminRejectReportUri(_session.Session, reportedMessageId), "", true, true);
        }


        internal string GetAdminAlliancesTextSearchPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminAllianceListUri(_session.Session), "", true, true);
        }


        internal string GetAdminAlliancesTextSearchPage(string search)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminAllianceTextSearchUri(_session.Session), GamePostData.GetAdminAlliancesTextSearch(search), "", true, true);
        }


        internal string GetAdminEnhancedSearchPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminEnhancedSearchUri(_session.Session), "", true, true);
        }


        /// <summary>
        /// PageNumber starts from 1.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        internal string GetAdminLogsPage(int pageNumber)
        {
            return _httpClient.GetPage(_gameUri.GetAdminLogsUri(_session.Session, pageNumber), "", true, true);
        }


        /// <summary>
        /// PageNumber starts from 1.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        internal string GetAdminLogsPage(string search, int pageNumber)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminLogsUri(_session.Session, pageNumber), GamePostData.GetAdminLogs(search, pageNumber), "", true, true);
        }


        /// <summary>
        /// Not working
        /// </summary>
        /// <returns></returns>
        internal string GetAdminLogsDetailsPage()
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminLogsDetailsUri(_session.Session), GamePostData.GetAdminLogsDetails(), "", true, true);
        }


        internal string GetAdminOperatorsSummaryPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminOperatorsSummaryUri(_session.Session), "", true, true);
        }


        internal string GetAdminPrivateNotesPage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminPrivateNotesUri(_session.Session), "", true, true);
        }


        /// <summary>
        /// Adding a note will override previous notes.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        internal string GetAdminPrivateNotePage(string note)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPrivateNotesUri(_session.Session), GamePostData.GetAdminPrivateNote(note), "", true, true);
        }


        internal string GetAdminCreateAdminNotePage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminCreateAdminNoteUri(_session.Session), "", true, true);
        }


        internal string GetAdminCreateAdminNotePage(int adminId, string note)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminCreateAdminNoteUri(_session.Session), GamePostData.GetAdminCreateAdminNote(adminId, note), "", true, true);
        }


        internal string GetAdminCreateGeneralNotePage()
        {
            return _httpClient.GetPage(_gameUri.GetAdminCreateGeneralNoteUri(_session.Session), "", true, true);
        }


        internal string GetAdminCreateGeneralNotePage(string note)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminCreateGeneralNoteUri(_session.Session), GamePostData.GetAdminCreateGeneralNote(note), "", true, true);
        }


        internal string GetAdminQuickNickSearchPage(string search)
        {
            return _httpClient.GetPage(_gameUri.GetAdminQuickNickSearchUri(_session.Session, search), "", true, true);
        }


        internal string GetAdminQuickAllianceSearchPage(string search)
        {
            return _httpClient.GetPage(_gameUri.GetAdminQuickAllianceSearchUri(_session.Session, search), "", true, true);
        }


        internal string GetAdminQuickPlayerInfoPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminQuickPlayerInfoSearchUri(_session.Session, playerId), "", true, true);
        }


        #endregion ----- Get General AT Page Functions ------


        #region ----- Get AT Player Functions ------


        internal string GetAdminPlayerOverviewPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerOverviewUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerDeleteShortNotePage(int playerId, int shortNoteId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerDeleteShortNoteUri(_session.Session, playerId, shortNoteId), "", true, true);
        }


        internal string GetAdminPlayerLogoutPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerLogoutUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerDeactivateIPCheckPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerDeactivateIPCheckUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerActivateIPCheckPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerActivateIPCheckUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerResetPasswordPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerResetPasswordUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerResendValidationEmailPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerResendValidationEmailUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerPlanetOverviewPage(int playerId, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerPlanetOverviewUri(_session.Session, playerId, planetId), "", true, true);
        }


        internal string GetAdminPlayerRenamePlanetPage(int playerId, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerRenamePlanetUri(_session.Session, playerId, planetId), "", true, true);
        }


        internal string GetAdminPlayerNotesPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerNotesUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerNoteDetailsPage(int playerId, int noteId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerNoteDetailsUri(_session.Session, playerId, noteId), "", true, true);
        }


        internal string GetAdminPlayerDeleteNotePage(int playerId, int noteId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerDeleteNoteUri(_session.Session, playerId, noteId), "", true, true);
        }


        internal string GetAdminPlayerChangeNoteStatusPage(int playerId, int noteId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerChangeNoteStatusUri(_session.Session, playerId, noteId), "", true, true);
        }


        internal string GetAdminPlayerChangeNoteStatusPage(int playerId, int noteId, string noteStatus)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerChangeNoteStatusUri(_session.Session, playerId, noteId), GamePostData.GetAdminPlayerChangeNoteStatus(noteStatus), "", true, true);
        }


        internal string GetAdminPlayerReportedMessageDetailsPage(int playerId, int reportedMessageId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerReportedMessageDetailsUri(_session.Session, playerId, reportedMessageId), "", true, true);
        }


        internal string GetAdminPlayerAddNewNotePage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerAddNewNoteUri(_session.Session, playerId), "", true, true);
        }


        internal string PostAdminPlayerLongNote(int playerId, string note)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerAddLongNoteUri(_session.Session, playerId), GamePostData.GetAdminPlayerAddNewNote(note), "", true, true);
        }


        internal string GetAdminPlayerBanPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerBanUri(_session.Session, playerId), "", true, true);
        }


        /// <summary>
        /// Ban duration in days.
        /// Permanent ban: 9999 days.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerName"></param>
        /// <param name="banReason"></param>
        /// <param name="banDuration"></param>
        /// <param name="activateVacationMode"></param>
        /// <returns></returns>
        internal string GetAdminPlayerBanPage(int playerId, string playerName, string banReason, int banDuration, bool activateVacationMode)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerBanUri(_session.Session, playerId), GamePostData.GetAdminPlayerBan(playerName, banReason, banDuration, activateVacationMode), "", true, true);
        }


        internal string GetAdminPlayerUnbanPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerUnbanUri(_session.Session, playerId), "", true, true);
        }


        /// <summary>
        /// Never remove vacation mode when unbanning an account.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerName"></param>
        /// <param name="unBanReason"></param>
        /// <param name="removeVacationMode"></param>
        /// <returns></returns>
        internal string GetAdminPlayerUnbanPage(int playerId, string playerName, string unBanReason, bool removeVacationMode)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerUnbanUri(_session.Session, playerId), GamePostData.GetAdminPlayerUnBan(playerName, unBanReason, removeVacationMode), "", true, true);
        }


        internal string GetAdminPlayerRenamePage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerRenameUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerRenamePage(int playerId, string newPlayerName)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerRenameUri(_session.Session, playerId), GamePostData.GetAdminPlayerRename(newPlayerName), "", true, true);
        }


        internal string GetAdminPlayerChangeEmailPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerChangeEmailUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerChangeEmailPage(int playerId, string playerName, string currentEmail, string newEmail)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerChangeEmailUri(_session.Session, playerId), GamePostData.GetAdminPlayerChangeEmail(playerName, currentEmail, newEmail), "", true, true);
        }


        internal string GetAdminPlayerLogsPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerLogsUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerSendMessagePage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerSendMessageUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerSendMessagePage(int playerId, string playerName, string subject, string message, bool addMessageToNote)
        {
            return _httpClient.PostSynchronous(_gameUri.GetAdminPlayerSendMessageUri(_session.Session, playerId), GamePostData.GetAdminPlayerSendMessage(playerName, subject, message, addMessageToNote), "", true, true);
        }


        internal string GetAdminPlayerFleetLogOverviewPage(int playerId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerFleetLogOverviewUri(_session.Session, playerId), "", true, true);
        }


        internal string GetAdminPlayerFleetLogByMissionPage(int playerId, Enums.FLEETLOG_MISSION mission)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerFleetLogByMissionUri(_session.Session, playerId, mission), "", true, true);
        }


        internal string GetAdminPlayerFleetLogByMissionByPlanetPage(int playerId, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerFleetLogByMissionByPlanetUri(_session.Session, playerId, mission, planetId), "", true, true);
        }


        internal string GetAdminPlayerFleetLogByMissionToPlanetPage(int playerId, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerFleetLogByMissionToPlanetUri(_session.Session, playerId, mission, planetId), "", true, true);
        }


        internal string GetAdminPlayerFleetLogByMissionFromPlanetPage(int playerId, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayerFleetLogByMissionFromPlanetUri(_session.Session, playerId, mission, planetId), "", true, true);
        }


        internal string GetAdminPlayersFleetLogByMission(int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayersFleetLogByMissionUri(_session.Session, player1Id, player2Id, mission), "", true, true);
        }


        internal string GetAdminPlayersFleetLogByMissionByPlanet(int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayersFleetLogByMissionByPlanetUri(_session.Session, player1Id, player2Id, mission, planetId), "", true, true);
        }


        internal string GetAdminPlayersFleetLogByMissionToPlanet(int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayersFleetLogByMissionToPlanetUri(_session.Session, player1Id, player2Id, mission, planetId), "", true, true);
        }


        internal string GetAdminPlayersFleetLogByMissionFromPlanet(int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return _httpClient.GetPage(_gameUri.GetAdminPlayersFleetLogByMissionFromPlanetUri(_session.Session, player1Id, player2Id, mission, planetId), "", true, true);
        }


        #endregion ----- Get AT Player Functions ------


        #region ----- Functions ------


        public void OpenWebBrowser(string webBrowserType)
        {
            if (!IsSessionSet(true))
                return;

            try
            {
                string url = _gameUri.GetAdminIndexUri(_session.Session).AbsoluteUri;

                if (!webBrowserType.Equals("Internet Explorer"))
                {
                    string cookies = Utilities.GetCookiesString(_session.Cookies);
                    url = Utilities.AppendCookiesToUrl(url, Utilities.StringTo64(cookies));
                }

                Utilities.OpenWebBrowser(webBrowserType, url);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex.Message);
            }
        }


        public void OpenWebBrowserUserAccount(string webBrowserType, int playerId)
        {
            if (!IsSessionSet(true))
                return;

            try
            {
                string url = _gameUri.GetAdminPlayerOverviewUri(_session.Session, playerId).AbsoluteUri;

                if (!webBrowserType.Equals("Internet Explorer"))
                {
                    string cookies = Utilities.GetCookiesString(_session.Cookies);
                    url = Utilities.AppendCookiesToUrl(url, Utilities.StringTo64(cookies));
                }

                Utilities.OpenWebBrowser(webBrowserType, url);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex.Message);
            }
        }


        #endregion ----- Functions ------


        #region ----- Blank ------

        #endregion ----- Blank ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private void ClearSession()
        {
            if (_session == null)
            {
                _session = new GameSession();
                _session.UniverseId = _universe.Id;
            }

            _session.Reset((int)Enums.SESSION_STATUS.INVALID);
            _httpClient.ClearCookies();
            OnLoggedOut();
        }


        private void SetSession(string session, List<WebClientCookie> cookies, int status)
        {
            if (_session == null)
                _session = new GameSession();
            _session.UniverseId = _universe.Id;
            _session.SetSession(session, cookies, status);
        }


        private bool Login(string userName, string password, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                ClearSession();
                string session = _httpClient.CreateSession(_gameUri.GetLoginViaPostMethodUri(), GamePostData.GetLogin(userName, password), "");

                if (!string.IsNullOrEmpty(session) && session.Equals(Constants.LOGIN_FAILED))
                {
                    errorMessage = Constants.LOGIN_FAILED;
                    OnLoginFailed();
                    return false;
                }
                else if (!string.IsNullOrEmpty(session) && !session.Equals(Constants.ERROR))
                {
                    SetSession(session, _httpClient.GetCookies(_gameUri.GetDomain()), (int)Enums.SESSION_STATUS.VALID);
                    foreach (WebClientCookie cookie in _session.Cookies)
                    {
                        EssentialUtil.SetWinINETCookieString(_gameUri.GetDomain(), null, cookie.Key + "=" + cookie.Value);
                    }
                    OnLoggedIn();
                    return true;
                }
                errorMessage = Constants.ERROR;
                OnErrorOccurred("Error occurred during Login.");
                return false;
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


        #region ----- BackgroundWorker DoWork Methods ------


        void bgw_DoWorkIsSessionValid(object sender, DoWorkEventArgs e)
        {
            IsSessionValid();
        }


        #endregion ----- BackgroundWorker DoWork Methods ------


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
                OnSessionInvalid();
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
        protected void OnSessionInvalid()
        {
            ClearSession();

            // Check if there are any Subscribers
            if (SessionInvalid != null)
            {
                // Call the Event
                SessionInvalid(_universe);
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
                ErrorOccurred(_universe, errorMessage);
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
                LoggedIn(_universe);
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
                LoginFailed(_universe);
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
                LoggedOut(_universe);
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
