using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class TicketGUI
    {
        private int ticketId;
        private string ticketValue;
        private string subject;
        private int server;
        private string nickName;
        private string staffNickName;
        private int openedTickets;
        // Original ticket date
        private string dateString;
        // Last communication date
        private string dateString2;

        public string DateString2
        {
            get { return dateString2; }
            set { dateString2 = value; }
        }

        public string DateString
        {
            get { return dateString; }
            set { dateString = value; }
        }

        public int OpenedTickets
        {
            get { return openedTickets; }
            set { openedTickets = value; }
        }

        public string StaffNickName
        {
            get { return staffNickName; }
            set { staffNickName = value; }
        }

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        public int Server
        {
            get { return server; }
            set { server = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string TicketValue
        {
            get { return ticketValue; }
            set { ticketValue = value; }
        }

        public int TicketId
        {
            get { return ticketId; }
            set { ticketId = value; }
        }
    }
}
