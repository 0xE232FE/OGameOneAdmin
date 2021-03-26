using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgameServiceV1.Serializable
{
    [Serializable]
    public class Credentials
    {
        private string _universeId;
        private Int64 _playerId;
        private string _userName;
        private string _password;
        private DateTime _addDateTime;
        private DateTime? _modDateTime;
        private bool _isUserToUniverseAccountLinkCreated = false;
        private bool _areCredentialsSyncToServer = false;
        private string _webClientUsername;
        private string _webClientPassword;

        public string WebClientPassword
        {
            get { return _webClientPassword; }
            set { _webClientPassword = value; }
        }

        public string WebClientUsername
        {
            get { return _webClientUsername; }
            set { _webClientUsername = value; }
        }

        public bool AreCredentialsSyncToServer
        {
            get { return _areCredentialsSyncToServer; }
            set { _areCredentialsSyncToServer = value; }
        }

        public bool IsUserToUniverseAccountLinkCreated
        {
            get { return _isUserToUniverseAccountLinkCreated; }
            set { _isUserToUniverseAccountLinkCreated = value; }
        }

        public DateTime? ModDateTime
        {
            get { return _modDateTime; }
            set { _modDateTime = value; }
        }

        public DateTime AddDateTime
        {
            get { return _addDateTime; }
            set { _addDateTime = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public Int64 PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        public string UniverseId
        {
            get { return _universeId; }
            set { _universeId = value; }
        }
    }
}
