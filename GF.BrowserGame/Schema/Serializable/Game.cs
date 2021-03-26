using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class Game
    {
        private List<Community> _communityList = new List<Community>();
        public List<Community> CommunityList
        {
            get { return _communityList; }
            set { _communityList = value; }
        }
    }
}
