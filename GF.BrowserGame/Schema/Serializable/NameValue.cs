using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class NameValue
    {
        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        private string _itemValue;
        public string ItemValue
        {
            get { return _itemValue; }
            set { _itemValue = value; }
        }

        public void SetNameValue(string name, string value)
        {
            _itemName = name;
            _itemValue = value;
        }
    }
}
