using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class ATLog
    {
        private string _sDate;
        private DateTime date;
        private string log;
        private bool found;

        public bool Found
        {
            get { return found; }
            set { found = value; }
        }

        public string Log
        {
            get { return log; }
            set { log = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string sDate
        {
            get { return _sDate; }
            set { _sDate = value; }
        }
    }
}
