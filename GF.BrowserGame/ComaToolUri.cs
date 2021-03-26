using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;

namespace GF.BrowserGame
{
    internal class ComaToolUri
    {
        private const string _serverPath = "https://coma.gameforge.com";
        private const string _loginServerPath = "https://login.gameforge.de";

        public string GetDomain()
        {
            return _serverPath;
        }

        private const string _indexUri = "/index.php";
        private const string _profileUri = "/index.php?page=profile";
        private const string _loginUri = "/index.php?page=login";
        private const string _logoutUri = "/index.php?my=logout";
        private const string _postLoginUri = "/?project=coma_live_environment&return=https%3a%2f%2fcoma.gameforge.com%2f";
        private const string _ticketIndexUri = "/ticket/index.php";
        private const string _ticketIndex2Uri = "/ticket/index.php?active_comm={0}";
        private const string _myTicketUri = "/ticket/index.php?active_comm={0}&page=tickets&action=gom&offset={1}";
        private const string _openTicketUri = "/ticket/index.php?active_comm={0}&page=tickets&action=go&offset={1}";
        private const string _closedTicketUri = "/ticket/index.php?active_comm={0}&page=tickets&action=gc&offset={1}";
        private const string _viewTicketUri = "/ticket/index.php?active_comm={0}&page=answer&action=view&id={1}&value={2}";
        private const string _viewTicketAnswerUri = "/ticket/index.php?active_comm={0}&page=answer&action=answer&id={1}&value={2}";
        private const string _viewTicketAnswerTemplateUri = "/ticket/index.php?active_comm={0}&page=answer&action=answer&id={1}&value={2}&link={3}";
        private const string _viewTicketSubmitAnswerUri = "/ticket/index.php?active_comm={0}&page=answer&action=submit&id={1}&value={2}";


        public Uri GetIndexUri()
        {
            return new Uri(_serverPath + _indexUri, UriKind.Absolute);
        }

        public Uri GetProfileUri()
        {
            return new Uri(_serverPath + _profileUri, UriKind.Absolute);
        }

        public Uri GetLoginUri()
        {
            return new Uri(_loginServerPath, UriKind.Absolute);
        }

        public Uri GetLogoutUri()
        {
            return new Uri(_serverPath + _logoutUri, UriKind.Absolute);
        }

        public Uri GetPostLoginUri()
        {
            return new Uri(_loginServerPath + _postLoginUri, UriKind.Absolute);
        }

        public Uri GetTicketIndexUri()
        {
            return new Uri(_serverPath + _ticketIndexUri, UriKind.Absolute);
        }

        public Uri GetTicketIndex2Uri(int communityId)
        {
            return new Uri(_serverPath + String.Format(_ticketIndex2Uri, communityId), UriKind.Absolute);
        }

        public Uri GetMyTicketUri(int communityId, int offset)
        {
            return new Uri(_serverPath + String.Format(_myTicketUri, communityId, offset), UriKind.Absolute);
        }

        public Uri GetOpenTicketUri(int communityId, int offset)
        {
            return new Uri(_serverPath + String.Format(_openTicketUri, communityId, offset), UriKind.Absolute);
        }

        public Uri GetClosedTicketUri(int communityId, int offset)
        {
            return new Uri(_serverPath + String.Format(_closedTicketUri, communityId, offset), UriKind.Absolute);
        }

        public Uri GetViewTicketUri(int communityId, int ticketId, string ticketValue)
        {
            return new Uri(_serverPath + String.Format(_viewTicketUri, communityId, ticketId, ticketValue), UriKind.Absolute);
        }

        public Uri GetViewTicketAnswerUri(int communityId, int ticketId, string ticketValue)
        {
            return new Uri(_serverPath + String.Format(_viewTicketAnswerUri, communityId, ticketId, ticketValue), UriKind.Absolute);
        }

        public Uri GetViewTicketAnswerTemplateUri(int communityId, int ticketId, string ticketValue, string link)
        {
            return new Uri(_serverPath + String.Format(_viewTicketAnswerTemplateUri, communityId, ticketId, ticketValue, link), UriKind.Absolute);
        }

        public Uri GetViewTicketSubmitAnswerUri(int communityId, int ticketId, string ticketValue)
        {
            return new Uri(_serverPath + String.Format(_viewTicketSubmitAnswerUri, communityId, ticketId, ticketValue), UriKind.Absolute);
        }
    }
}
