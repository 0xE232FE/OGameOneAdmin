using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class WebClientCookie
    {
        private string key;
        private string value;
        private string domain;

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}
