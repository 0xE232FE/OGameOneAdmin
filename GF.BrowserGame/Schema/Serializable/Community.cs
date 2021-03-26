using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class Community
    {
        private bool _isActive;
        private string _id;
        private string _name;
        private string _language;
        private string _portalUrl;
        private string _boardUrl;
        private string _gameRulesUrl;

        private List<UniverseRole> _universeRoleList = new List<UniverseRole>();
        private List<Universe> _universeList = new List<Universe>();

        public List<Universe> UniverseList
        {
            get { return _universeList; }
            set { _universeList = value; }
        }

        public List<UniverseRole> UniverseRoleList
        {
            get { return _universeRoleList; }
            set { _universeRoleList = value; }
        }

        public string GameRulesUrl
        {
            get { return _gameRulesUrl; }
            set { _gameRulesUrl = value; }
        }

        public string BoardUrl
        {
            get { return _boardUrl; }
            set { _boardUrl = value; }
        }

        public string PortalUrl
        {
            get { return _portalUrl; }
            set { _portalUrl = value; }
        }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
    }
}
