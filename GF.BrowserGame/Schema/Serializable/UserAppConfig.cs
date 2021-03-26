using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class UserAppConfig
    {
        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        private bool _showAppInTaskBar = true;
        public bool ShowAppInTaskBar
        {
            get { return _showAppInTaskBar; }
            set { _showAppInTaskBar = value; }
        }

        private bool _minimizeToTray = false;
        public bool MinimizeToTray
        {
            get { return _minimizeToTray; }
            set { _minimizeToTray = value; }
        }

        private bool _doNotShowMinimizeToolTip = false;
        public bool DoNotShowMinimizeToolTip
        {
            get { return _doNotShowMinimizeToolTip; }
            set { _doNotShowMinimizeToolTip = value; }
        }

        private bool _minimizeWhenAppLocked = false;
        public bool MinimizeWhenAppLocked
        {
            get { return _minimizeWhenAppLocked; }
            set { _minimizeWhenAppLocked = value; }
        }

        private bool _synchronizeOGameCredentials = true;
        public bool SynchronizeOGameCredentials
        {
            get { return _synchronizeOGameCredentials; }
            set { _synchronizeOGameCredentials = value; }
        }

        private bool _showDashboardTips = true;
        public bool ShowDashboardTips
        {
            get { return _showDashboardTips; }
            set { _showDashboardTips = value; }
        }

        private bool _showOptions = true;
        public bool ShowOptions
        {
            get { return _showOptions; }
            set { _showOptions = value; }
        }

        private string _externalWebBrowser = "Internet Explorer";
        public string ExternalWebBrowser
        {
            get { return _externalWebBrowser; }
            set { _externalWebBrowser = value; }
        }
    }
}