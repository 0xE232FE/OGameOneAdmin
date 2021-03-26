using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgameServiceV1.Serializable
{
    [Serializable]
    public class SecureObject
    {
        private string _id;
        private string _name;
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
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
