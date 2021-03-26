using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgameServiceV1.Serializable
{
    [Serializable]
    public class SetupAppObj
    {
        private bool _encryptionKeysExists = false;
        private string _toolLatestVersion;
        private bool _isApplicationUserValid = false;
        private bool _isToolValid = false;
        private bool _isUserAllowedToUseThisTool = false;
        private string _communityData;
        private bool _error = false;
        private string _errorMessage;
        private string _clientEncryptionKey = "";
        private List<Credentials> _credentialsList;

        public List<Credentials> CredentialsList
        {
            get { return _credentialsList; }
            set { _credentialsList = value; }
        }

        public string ClientEncryptionKey
        {
            get { return _clientEncryptionKey; }
            set { _clientEncryptionKey = value; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public bool Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public string CommunityData
        {
            get { return _communityData; }
            set { _communityData = value; }
        }

        public bool IsUserAllowedToUseThisTool
        {
            get { return _isUserAllowedToUseThisTool; }
            set { _isUserAllowedToUseThisTool = value; }
        }

        public bool IsToolValid
        {
            get { return _isToolValid; }
            set { _isToolValid = value; }
        }

        public bool IsApplicationUserValid
        {
            get { return _isApplicationUserValid; }
            set { _isApplicationUserValid = value; }
        }

        public string ToolLatestVersion
        {
            get { return _toolLatestVersion; }
            set { _toolLatestVersion = value; }
        }

        public bool EncryptionKeysExists
        {
            get { return _encryptionKeysExists; }
            set { _encryptionKeysExists = value; }
        }
    }
}
