using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class ComaToolSession
    {
        private int _status = (int)Enums.SESSION_STATUS.INVALID;
        private List<WebClientCookie> _cookies = new List<WebClientCookie>();

        public void SetSession(List<WebClientCookie> cookies)
        {
            _cookies = cookies;
        }

        public void SetSession(List<WebClientCookie> cookies, int status)
        {
            _cookies = cookies;
            _status = status;
        }

        public void Reset(int status)
        {
            _cookies = new List<WebClientCookie>();
            _status = status;
        }

        public bool isNullOrEmpty()
        {
            if (_cookies == null || _cookies.Count == 0)
            {
                _cookies = new List<WebClientCookie>();
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
    }
}
