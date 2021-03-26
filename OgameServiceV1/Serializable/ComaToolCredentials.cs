using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgameServiceV1.Serializable
{
    [Serializable]
    public class ComaToolCredentials
    {
        private string _userName;
        private string _password;
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
    }
}
