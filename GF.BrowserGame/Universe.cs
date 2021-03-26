using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OGame.Utility;

namespace OGame
{
    public class Universe
    {
        private string _name;
        private int _number;
        private int _speed;
        private bool _isRedesign;
        private bool _isACS;
        private bool _isResourcesInDF;

        public Universe(Dictionary<string, string> uniSettings)
        {
            foreach (var kvp in uniSettings)
            {
                switch (kvp.Key)
                {
                    case Constants.UniverseSettingsKeys.UNI_NAME:
                        _name = kvp.Value;
                        break;
                    case Constants.UniverseSettingsKeys.UNI_NUMBER:
                        _number = int.Parse(kvp.Value);
                        break;
                    case Constants.UniverseSettingsKeys.UNI_SPEED:
                        _speed = int.Parse(kvp.Value);
                        break;
                    case Constants.UniverseSettingsKeys.UNI_ISREDESIGN:
                        _isRedesign = Utilities.StringToBoolean(kvp.Value);
                        break;
                    case Constants.UniverseSettingsKeys.UNI_ISACS:
                        _isACS = Utilities.StringToBoolean(kvp.Value);
                        break;
                    case Constants.UniverseSettingsKeys.UNI_ISRESOURCESINDF:
                        _isResourcesInDF = Utilities.StringToBoolean(kvp.Value);
                        break;
                }
            }
        }

        public bool IsResourcesInDF
        {
            get { return _isResourcesInDF; }
            set { _isResourcesInDF = value; }
        }

        public bool IsACS
        {
            get { return _isACS; }
            set { _isACS = value; }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public bool IsRedesign
        {
            get { return _isRedesign; }
            set { _isRedesign = value; }
        }

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
