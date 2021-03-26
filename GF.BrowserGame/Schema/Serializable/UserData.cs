using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class UserData
    {
        private ComaToolSession _comaToolSession = new ComaToolSession();
        public ComaToolSession ComaToolSession
        {
            get { return _comaToolSession; }
            set { _comaToolSession = value; }
        }

        private ComaToolCredentials _comaToolCredentials = new ComaToolCredentials();
        public ComaToolCredentials ComaToolCredentials
        {
            get { return _comaToolCredentials; }
            set { _comaToolCredentials = value; }
        }

        private List<GameSession> _gameSessionList = new List<GameSession>();
        public List<GameSession> GameSessionList
        {
            get { return _gameSessionList; }
            set { _gameSessionList = value; }
        }

        private List<Credentials> _credentialsList = new List<Credentials>();
        public List<Credentials> CredentialsList
        {
            get { return _credentialsList; }
            set { _credentialsList = value; }
        }

        private List<UniverseData> _universeData = new List<UniverseData>();
        public List<UniverseData> UniverseData
        {
            get { return _universeData; }
            set { _universeData = value; }
        }
    }
}
