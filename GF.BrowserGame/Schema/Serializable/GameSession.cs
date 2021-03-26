using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class GameSession
    {
        private string _universeId;
        private string _session = string.Empty;
        private int _status = (int)Enums.SESSION_STATUS.INVALID;
        private List<WebClientCookie> _cookies = new List<WebClientCookie>();

        public void SetSession(string session, List<WebClientCookie> cookies)
        {
            _session = session;
            _cookies = cookies;
        }

        public void SetSession(string session, List<WebClientCookie> cookies, int status)
        {
            _session = session;
            _cookies = cookies;
            _status = status;
        }

        public void Reset(int status)
        {
            _session = string.Empty;
            _cookies = new List<WebClientCookie>();
            _status = status;
        }

        public bool isNullOrEmpty()
        {
            if (string.IsNullOrEmpty(_session) || _session.Length != Constants.SESSION_LENGTH)
            {
                _session = string.Empty;
                return true;
            }
            else
                return false;
        }

        public bool isStatusUnknown()
        {
            if (_status == (int)Enums.SESSION_STATUS.UNKNOWN)
                return true;
            else
                return false;
        }

        public bool isStatusValid()
        {
            if (_status == (int)Enums.SESSION_STATUS.VALID)
                return true;
            else
                return false;
        }

        public bool isStatusInvalid()
        {
            if (_status == (int)Enums.SESSION_STATUS.INVALID)
                return true;
            else
                return false;
        }

        public List<WebClientCookie> Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Session
        {
            get { return _session; }
            set { _session = value; }
        }

        public string UniverseId
        {
            get { return _universeId; }
            set { _universeId = value; }
        }
    }
}
