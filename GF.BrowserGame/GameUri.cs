using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;

namespace GF.BrowserGame
{
    internal class GameUri
    {
        #region Game Server Configuration

        private Universe _universe;
        private string _serverPath = "http://{0}";
        private string _gameDirectory = "";
        private string _adminDirectory = "";
        private string _registrationDirectory = "";
        private string _absoluteGamePath = "{0}/{1}/";
        private string _absoluteAdminPath = "{0}/{1}/{2}/";
        private string _absoluteRegistrationPath = "{0}/{1}/{2}/";

        #endregion Game Server Configuration

        #region Login Uri Configuration

        private const string _loginViaGetMethodUri = "login2.php?login={0}&pass={1}&v=2&is_utf8=0";
        private const string _loginViaPostMethodUri = "login2.php";
        private const string _logoutUri = "index.php?page=logout&session={0}";

        #endregion Login Uri Configuration

        #region Admin Tool Uri Configuration

        private const string _adminIndexUri = "index.php?session={0}";
        private const string _adminHomeUri = "home.php?session={0}";
        private const string _adminReadNoteUri = "home.php?session={0}&details={1}";
        private const string _adminDeleteNoteUri = "home.php?session={0}&delit={1}";
        private const string _adminPrivateNotesUri = "notiz.php?session={0}";
        private const string _adminCreateAdminNoteUri = "note.php?session={0}&new=2";
        private const string _adminCreateGeneralNoteUri = "note.php?session={0}&new=3";
        private const string _adminInfosUri = "infos.php?session={0}";
        private const string _adminCheckUri = "kontrolle.php?session={0}";
        private const string _adminPilloryUri = "prangerliste.php?session={0}&side={1}";
        private const string _adminMatchingDataUri = "multiuser.php?session={0}";
        private const string _adminPlayerListUri = "uebersicht.php?session={0}&user";
        private const string _adminPlanetListUri = "uebersicht.php?session={0}&user";
        private const string _adminAllianceListUri = "uebersicht.php?session={0}&allianz";
        private const string _adminAccountTransferUri = "accounttausch.php?session={0}";
        private const string _adminCookiesUri = "cookie_login_log.php?session={0}";
        private const string _adminLoginsUri = "cookie_login_log.php?session={0}";
        private const string _adminTopListUri = "toplist.php?session={0}&side={1}";
        private const string _adminReportsUri = "meldung.php?session={0}";
        private const string _adminResumeReportUri = "meldung.php?session={0}&misstake={1}";
        private const string _adminForwardReportUri = "meldung.php?session={0}&forward={1}&detail={2}";
        private const string _adminBanReportUri = "meldung.php?session={0}&ban={1}";
        private const string _adminRejectReportUri = "meldung.php?session={0}&noban={1}";
        private const string _adminAllianceTextSearchUri = "allyinfosearch.php?session={0}";
        private const string _adminEnhancedSearchUri = "ext_search.php?session={0}";
        private const string _adminLogsUri = "logs.php?session={0}&side={1}";
        private const string _adminOperatorsSummaryUri = "op_overview.php?session={0}";

        private const string _adminPlayerOverviewUri = "kontrolle.php?session={0}&uid={1}&showallnotes=1";
        private const string _adminPlayerDeleteShortNoteUri = "kontrolle.php?session={0}&uid={1}&delshortnotenow={2}";
        private const string _adminPlayerLogoutUri = "kontrolle.php?session={0}&uid={1}&ausloggen";
        private const string _adminPlayerDeactivateIPCheckUri = "kontrolle.php?session={0}&uid={1}&stipcheck=0";
        private const string _adminPlayerActivateIPCheckUri = "kontrolle.php?session={0}&uid={1}&stipcheck=1";
        private const string _adminPlayerResetPasswordUri = "kontrolle.php?session={0}&uid={1}&resetpass=1";
        private const string _adminPlayerResendValidationEmailUri = "kontrolle.php?session={0}&uid={1}&sendrevert";
        private const string _adminPlayerPlanetOverviewUri = "kontrolle.php?session={0}&uid={1}&ressourcen={2}";
        private const string _adminPlayerRenamePlanetUri = "renameplanet.php?session={0}&uid={1}&pid={2}";
        private const string _adminPlayerNotesUri = "seenotes.php?session={0}&uid={1}&notetyp=1";
        private const string _adminPlayerNoteDetailsUri = "seenotes.php?session={0}&uid={1}&details={2}";
        private const string _adminPlayerDeleteNoteUri = "seenotes.php?session={0}&uid={1}&notetyp=1&delit={2}";
        private const string _adminPlayerChangeNoteStatusUri = "note.php?session={0}&uid={1}&changestat={2}";
        private const string _adminPlayerReportedMessageDetailsUri = "meldung.php?session={0}&uid={1}&detail={2}";
        private const string _adminPlayerAddNewNoteUri = "note.php?session={0}&uid={1}&new=1";
        private const string _adminPlayerAddLongNoteUri = "note.php?session={0}&uid={1}&addnew=1";
        private const string _adminPlayerBanUri = "sperren.php?session={0}&uid={1}";
        private const string _adminPlayerUnbanUri = "entsperren.php?session={0}&uid={1}";
        private const string _adminPlayerRenameUri = "umbenennen.php?session={0}&uid={1}";
        private const string _adminPlayerChangeEmailUri = "emailaender.php?session={0}&uid={1}";
        private const string _adminPlayerLogsUri = "kontrolle.php?session={0}&uid={1}&logs";
        private const string _adminPlayerSendMessageUri = "sendmsg.php?session={0}&uid={1}";

        private const string _adminPlayerFleetLogOverviewUri = "flottenlog.php?session={0}&uid={1}";
        private const string _adminPlayerFleetLogByMissionUri = "flottenlog.php?session={0}&uid={1}&list={2}";
 
        private const string _adminPlayersFleetLogByMissionUri = "flottenlog.php?session={0}&uid={1}&list={2}&touser={3}";

        private const string _adminPlayerFleetLogByMissionByPlanetUri = "flottenlog.php?session={0}&uid={1}&list={2}&showplanet={3}";
        private const string _adminPlayerFleetLogByMissionFromPlanetUri = "flottenlog.php?session={0}&uid={1}&list={2}&fromplanet={3}";
        private const string _adminPlayerFleetLogByMissionToPlanetUri = "flottenlog.php?session={0}&uid={1}&list={2}&toplanet={3}";

        private const string _adminPlayersFleetLogByMissionByPlanetUri = "flottenlog.php?session={0}&uid={1}&list={2}&showplanet={3}&touser={4}";
        private const string _adminPlayersFleetLogByMissionFromPlanetUri = "flottenlog.php?session={0}&uid={1}&list={2}&fromplanet={3}&touser={4}";
        private const string _adminPlayersFleetLogByMissionToPlanetUri = "flottenlog.php?session={0}&uid={1}&list={2}&toplanet={3}&touser={4}";

        private const string _adminQuickNickSearchUri = "direktsuche.php?session={0}&nick={1}";
        private const string _adminQuickAllianceSearchUri = "direktsuche.php?session={0}&allianz={1}";
        private const string _adminQuickPlayerInfoSearchUri = "direktsuche.php?session={0}&nickinfos={1}";

        // *NOT WORKING * POST Method
        private const string _adminLogsDetailsUri = "logs.php?session={0}"; //"logs.php?session={0}&stats"

        #endregion Admin Tool Uri Configuration

        public GameUri(Universe universe)
        {
            if (universe != null)
            {
                _universe = universe;
                SetupGameUri();
            }
            else
                throw new Exception("GameUri could not be initialized");
        }

        public void SetupGameUri()
        {
            if (_universe != null)
            {
                if (_universe.Domain.StartsWith("http"))
                    _serverPath = _universe.Domain;
                else
                    _serverPath = string.Format(_serverPath, _universe.Domain);

                _gameDirectory = _universe.DomainGameDirectory;
                _adminDirectory = _universe.DomainAdminDirectory;
                _registrationDirectory = _universe.DomainRegDirectory;
                _absoluteGamePath = string.Format(_absoluteGamePath, _serverPath, _gameDirectory);
                _absoluteAdminPath = string.Format(_absoluteAdminPath, _serverPath, _gameDirectory, _adminDirectory);
                _absoluteRegistrationPath = string.Format(_absoluteRegistrationPath, _serverPath, _gameDirectory, _registrationDirectory);
            }
        }

        public string GetDomain()
        {
            return _serverPath;
        }

        #region General Admin Uri

        public Uri GetAdminLoadCookiesUrl(string cookies)
        {
            return new Uri(_absoluteAdminPath + "load/cookies/" + cookies, UriKind.Absolute);
        }

        public Uri GetLoginViaGetMethodUri(string userName, string password)
        {
            return new Uri(_absoluteRegistrationPath + string.Format(_loginViaGetMethodUri, userName, password), UriKind.Absolute);
        }

        public Uri GetLoginViaPostMethodUri()
        {
            return new Uri(_absoluteRegistrationPath + _loginViaPostMethodUri, UriKind.Absolute);
        }

        public Uri GetLogoutUri(string session)
        {
            return new Uri(_absoluteGamePath + string.Format(_logoutUri, session), UriKind.Absolute);
        }

        public Uri GetAdminIndexUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminIndexUri, session), UriKind.Absolute);
        }

        public Uri GetAdminSetLanguageUri(string session)
        {
            return GetAdminIndexUri(session);
        }

        public Uri GetAdminHomeUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminHomeUri, session), UriKind.Absolute);
        }

        public Uri GetAdminReadNoteUri(string session, int noteId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminReadNoteUri, session, noteId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminDeleteNoteUri(string session, int noteId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminDeleteNoteUri, session, noteId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminInfosUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminInfosUri, session), UriKind.Absolute);
        }

        public Uri GetAdminCheckUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminCheckUri, session), UriKind.Absolute);
        }

        /// <summary>
        /// PageNumber starts from 1.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public Uri GetAdminPilloryUri(string session, int pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            return new Uri(_absoluteAdminPath + string.Format(_adminPilloryUri, session, pageNumber.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminMachingDataUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminMatchingDataUri, session), UriKind.Absolute);
        }

        public Uri GetAdminPlayerListUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerListUri, session), UriKind.Absolute);
        }

        public Uri GetAdminPlanetListUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlanetListUri, session), UriKind.Absolute);
        }

        public Uri GetAdminAllianceListUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminAllianceListUri, session), UriKind.Absolute);
        }

        public Uri GetAdminAccountTransferUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminAccountTransferUri, session), UriKind.Absolute);
        }

        public Uri GetAdminCookiesUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminCookiesUri, session), UriKind.Absolute);
        }

        public Uri GetAdminLoginsUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminLoginsUri, session), UriKind.Absolute);
        }

        /// <summary>
        /// PageNumber starts from 0.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public Uri GetAdminTopListUri(string session, int pageNumber)
        {
            if (pageNumber < 0)
                pageNumber = 0;
            return new Uri(_absoluteAdminPath + string.Format(_adminTopListUri, session, pageNumber.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminReportsUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminReportsUri, session), UriKind.Absolute);
        }

        public Uri GetAdminResumeReportUri(string session, int reportedMessageId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminResumeReportUri, session, reportedMessageId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminForwardReportUri(string session, int reportedMessageId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminForwardReportUri, session, reportedMessageId.ToString(), reportedMessageId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminBanReportUri(string session, int reportedMessageId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminBanReportUri, session, reportedMessageId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminRejectReportUri(string session, int reportedMessageId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminRejectReportUri, session, reportedMessageId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminAllianceTextSearchUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminAllianceTextSearchUri, session), UriKind.Absolute);
        }

        public Uri GetAdminEnhancedSearchUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminEnhancedSearchUri, session), UriKind.Absolute);
        }

        /// <summary>
        /// PageNumber starts from 1.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public Uri GetAdminLogsUri(string session, int pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            return new Uri(_absoluteAdminPath + string.Format(_adminLogsUri, session, pageNumber), UriKind.Absolute);
        }

        public Uri GetAdminLogsDetailsUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminLogsDetailsUri, session), UriKind.Absolute);
        }

        public Uri GetAdminOperatorsSummaryUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminOperatorsSummaryUri, session), UriKind.Absolute);
        }

        public Uri GetAdminPrivateNotesUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPrivateNotesUri, session), UriKind.Absolute);
        }

        public Uri GetAdminCreateAdminNoteUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminCreateAdminNoteUri, session), UriKind.Absolute);
        }

        public Uri GetAdminCreateGeneralNoteUri(string session)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminCreateGeneralNoteUri, session), UriKind.Absolute);
        }

        public Uri GetAdminQuickNickSearchUri(string session, string search)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminQuickNickSearchUri, session, search), UriKind.Absolute);
        }

        public Uri GetAdminQuickAllianceSearchUri(string session, string search)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminQuickAllianceSearchUri, session, search), UriKind.Absolute);
        }

        public Uri GetAdminQuickPlayerInfoSearchUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminQuickPlayerInfoSearchUri, session, playerId), UriKind.Absolute);
        }

        #endregion General Admin Uri

        #region General AdminPlayer Based Uri

        public Uri GetAdminPlayerOverviewUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerOverviewUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerDeleteShortNoteUri(string session, int playerId, int shortNoteId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerDeleteShortNoteUri, session, playerId.ToString(), shortNoteId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerLogoutUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerLogoutUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerDeactivateIPCheckUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerDeactivateIPCheckUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerActivateIPCheckUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerActivateIPCheckUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerResetPasswordUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerResetPasswordUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerResendValidationEmailUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerResendValidationEmailUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerPlanetOverviewUri(string session, int playerId, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerPlanetOverviewUri, session, playerId.ToString(), planetId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerRenamePlanetUri(string session, int playerId, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerRenamePlanetUri, session, playerId.ToString(), planetId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerNotesUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerNotesUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerNoteDetailsUri(string session, int playerId, int noteId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerNoteDetailsUri, session, playerId.ToString(), noteId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerDeleteNoteUri(string session, int playerId, int noteId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerDeleteNoteUri, session, playerId.ToString(), noteId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerChangeNoteStatusUri(string session, int playerId, int noteId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerChangeNoteStatusUri, session, playerId.ToString(), noteId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerReportedMessageDetailsUri(string session, int playerId, int reportedMessageId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerReportedMessageDetailsUri, session, playerId.ToString(), reportedMessageId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerAddNewNoteUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerAddNewNoteUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerAddLongNoteUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerAddLongNoteUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerBanUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerBanUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerUnbanUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerUnbanUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerRenameUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerRenameUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerChangeEmailUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerChangeEmailUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerLogsUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerLogsUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerSendMessageUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerSendMessageUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerFleetLogOverviewUri(string session, int playerId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerFleetLogOverviewUri, session, playerId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerFleetLogByMissionUri(string session, int playerId, Enums.FLEETLOG_MISSION mission)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerFleetLogByMissionUri, session, playerId.ToString(), ((int)mission).ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerFleetLogByMissionByPlanetUri(string session, int playerId, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerFleetLogByMissionByPlanetUri, session, playerId.ToString(), ((int)mission).ToString(), planetId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerFleetLogByMissionToPlanetUri(string session, int playerId, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerFleetLogByMissionToPlanetUri, session, playerId.ToString(), ((int)mission).ToString(), planetId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayerFleetLogByMissionFromPlanetUri(string session, int playerId, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayerFleetLogByMissionFromPlanetUri, session, playerId.ToString(), ((int)mission).ToString(), planetId.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayersFleetLogByMissionUri(string session, int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayersFleetLogByMissionUri, session, player1Id.ToString(), ((int)mission).ToString(), player2Id.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayersFleetLogByMissionByPlanetUri(string session, int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayersFleetLogByMissionByPlanetUri, session, player1Id.ToString(), ((int)mission).ToString(), planetId.ToString(), player2Id.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayersFleetLogByMissionToPlanetUri(string session, int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayersFleetLogByMissionToPlanetUri, session, player1Id.ToString(), ((int)mission).ToString(), planetId.ToString(), player2Id.ToString()), UriKind.Absolute);
        }

        public Uri GetAdminPlayersFleetLogByMissionFromPlanetUri(string session, int player1Id, int player2Id, Enums.FLEETLOG_MISSION mission, int planetId)
        {
            return new Uri(_absoluteAdminPath + string.Format(_adminPlayersFleetLogByMissionFromPlanetUri, session, player1Id.ToString(), ((int)mission).ToString(), planetId.ToString(), player2Id.ToString()), UriKind.Absolute);
        }

        #endregion General AdminPlayer Based Uri
    }
}
