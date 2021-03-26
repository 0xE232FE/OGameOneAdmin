using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class ATUser
    {
        private string nick;
        private List<ATLog> logList = new List<ATLog>();
        private string atStats = "";
        private string ticketStats = "";

        public string AtStats
        {
            get { return atStats; }
            set { atStats = value; }
        }

        public string TicketStats
        {
            get { return ticketStats; }
            set { ticketStats = value; }
        }

        internal List<ATLog> LogList
        {
            get { return logList; }
            set { logList = value; }
        }

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }
    }
}
