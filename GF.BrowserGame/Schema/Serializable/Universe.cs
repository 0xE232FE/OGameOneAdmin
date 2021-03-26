using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Serializable
{
    [Serializable]
    public class Universe
    {
        // Community
        private string _communityId;

        // Uni Role
        private string _universeRoleId;

        // Uni General settings
        private bool _isActive;
        private string _id;
        private string _name;
        private string _domain;
        private int _number;
        private int _speed;
        private bool _isRedesign;
        private bool _isACS;
        private int _maxNumberOfAttackPerDay;
        private int _percentOfFleetInDF;
        private int _percentOfDefenseInDF;

        // Uni Url settings
        private string _domainGameDirectory = "game";
        private string _domainAdminDirectory = "admin2";
        private string _domainRegDirectory = "reg";

        public string UniverseRoleId
        {
            get { return _universeRoleId; }
            set { _universeRoleId = value; }
        }

        public string DomainRegDirectory
        {
            get { return _domainRegDirectory; }
            set { _domainRegDirectory = value; }
        }

        public string DomainAdminDirectory
        {
            get { return _domainAdminDirectory; }
            set { _domainAdminDirectory = value; }
        }

        public string DomainGameDirectory
        {
            get { return _domainGameDirectory; }
            set { _domainGameDirectory = value; }
        }

        public int PercentOfDefenseInDF
        {
            get { return _percentOfDefenseInDF; }
            set { _percentOfDefenseInDF = value; }
        }

        public int PercentOfFleetInDF
        {
            get { return _percentOfFleetInDF; }
            set { _percentOfFleetInDF = value; }
        }

        public int MaxNumberOfAttackPerDay
        {
            get { return _maxNumberOfAttackPerDay; }
            set { _maxNumberOfAttackPerDay = value; }
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

        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
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

        public string CommunityId
        {
            get { return _communityId; }
            set { _communityId = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
    }
}
