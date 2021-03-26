using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class WebServiceSettings
    {
        private int _settingId;
        public int SettingId
        {
            get { return _settingId; }
            set { _settingId = value; }
        }

        private List<WebServiceUrl> _webServiceUrlList = new List<WebServiceUrl>();
        public List<WebServiceUrl> WebServiceUrlList
        {
            get { return _webServiceUrlList; }
            set { _webServiceUrlList = value; }
        }
    }
}
