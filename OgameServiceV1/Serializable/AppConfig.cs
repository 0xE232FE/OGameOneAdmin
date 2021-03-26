using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgameServiceV1.Serializable
{
    [Serializable]
    public class AppConfig
    {
        private Guid? _toolId;
        public Guid? ToolId
        {
            get { return _toolId; }
            set { _toolId = value; }
        }

        private string _toolVersion;
        public string ToolVersion
        {
            get { return _toolVersion; }
            set { _toolVersion = value; }
        }

        private string _applicationSecret;
        public string ApplicationSecret
        {
            get { return _applicationSecret; }
            set { _applicationSecret = value; }
        }

        private Guid? _applicationKey;
        public Guid? ApplicationKey
        {
            get { return _applicationKey; }
            set { _applicationKey = value; }
        }

        private WebServiceSettings _webServiceSettings;
        public WebServiceSettings WebServiceSettings
        {
            get { return _webServiceSettings; }
            set { _webServiceSettings = value; }
        }

        private string _lastLoginName;
        public string LastLoginName
        {
            get { return _lastLoginName; }
            set { _lastLoginName = value; }
        }

        private bool _IsPublicComputer = false;
        public bool IsPublicComputer
        {
            get { return _IsPublicComputer; }
            set { _IsPublicComputer = value; }
        }
    }
}
