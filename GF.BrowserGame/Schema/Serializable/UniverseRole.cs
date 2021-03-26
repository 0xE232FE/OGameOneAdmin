using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class UniverseRole
    {
        private string _id;
        private string _name;
        private List<SecureObject> _secureObjectList = new List<SecureObject>();

        public List<SecureObject> SecureObjectList
        {
            get { return _secureObjectList; }
            set { _secureObjectList = value; }
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
    }
}
