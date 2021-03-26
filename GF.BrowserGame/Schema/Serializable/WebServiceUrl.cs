using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class WebServiceUrl
    {
        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private bool _isHttps = false;
        public bool IsHttps
        {
            get { return _isHttps; }
            set { _isHttps = value; }
        }

        private bool _canSwitchToHttp = false;
        public bool CanSwitchToHttp
        {
            get { return _canSwitchToHttp; }
            set { _canSwitchToHttp = value; }
        }

        public WebServiceUrl()
        {
        }

        public void SetWebServiceUrl(string url, bool isHttps, bool canSwitchToHttp)
        {
            _url = url;
            _isHttps = isHttps;
            _canSwitchToHttp = canSwitchToHttp;
        }
    }
}
