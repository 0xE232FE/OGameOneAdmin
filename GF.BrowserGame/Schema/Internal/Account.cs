using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class Account
    {
        private Int64 uid = 0;
        private string name = "";
        private int rank;
        private int points;
        private bool ipCheckActivated = true;
        private bool banned = false;
        private bool vmode = false;
        private bool smallInactive = false;
        private bool bigInactive = false;
        private bool deletionMode = false;
        private List<IpLog> ipLogList = new List<IpLog>();
        private string permaEmail;
        private string dynaEmail;

        public string DynaEmail
        {
            get { return dynaEmail; }
            set { dynaEmail = value; }
        }

        public List<IpLog> IpLogList
        {
            get { return ipLogList; }
            set { ipLogList = value; }
        }

        public string PermaEmail
        {
            get { return permaEmail; }
            set { permaEmail = value; }
        }

        public bool DeletionMode
        {
            get { return deletionMode; }
            set { deletionMode = value; }
        }

        public bool BigInactive
        {
            get { return bigInactive; }
            set { bigInactive = value; }
        }

        public bool SmallInactive
        {
            get { return smallInactive; }
            set { smallInactive = value; }
        }

        public bool Vmode
        {
            get { return vmode; }
            set { vmode = value; }
        }

        public bool Banned
        {
            get { return banned; }
            set { banned = value; }
        }

        public bool IpCheckActivated
        {
            get { return ipCheckActivated; }
            set { ipCheckActivated = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Int64 Uid
        {
            get { return uid; }
            set { uid = value; }
        }
    }
}
