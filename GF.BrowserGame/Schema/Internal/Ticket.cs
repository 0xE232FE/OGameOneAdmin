using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class Ticket
    {
        private int ticketId;
        private string ticketValue;
        private string subject;
        private int server;
        private string nickName;
        private string staffNickName;
        private int openedTickets;
        private string dateString;
        private DateTime createdOn;
        private DateTime lastReply;
        private string ipAddress;
        private string community;
        private string email;
        private Account account = new Account();
        private bool isNickMatch = false;
        private bool isPermaEmailMatch = false;
        private bool isDynaEmailMatch = false;
        private List<TicketMessage> ticketMessageList = new List<TicketMessage>();
        private List<Account> accountInvolvedList = new List<Account>();

        public List<Account> AccountInvolvedList
        {
            get { return accountInvolvedList; }
            set { accountInvolvedList = value; }
        }

        public List<TicketMessage> TicketMessageList
        {
            get { return ticketMessageList; }
            set { ticketMessageList = value; }
        }

        public bool IsDynaEmailMatch
        {
            get { return isDynaEmailMatch; }
            set { isDynaEmailMatch = value; }
        }

        public bool IsPermaEmailMatch
        {
            get { return isPermaEmailMatch; }
            set { isPermaEmailMatch = value; }
        }

        public bool IsNickMatch
        {
            get { return isNickMatch; }
            set { isNickMatch = value; }
        }

        public Account Account
        {
            get { return account; }
            set { account = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Community
        {
            get { return community; }
            set { community = value; }
        }

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public DateTime LastReply
        {
            get { return lastReply; }
            set { lastReply = value; }
        }

        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
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
