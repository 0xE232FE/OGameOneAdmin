using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class CommunityTicket
    {
        private int _communityId;
        private string _communityName;
        private List<TicketGUI> _myTicketList = new List<TicketGUI>();
        private List<TicketGUI> _openTicketList = new List<TicketGUI>();
        private List<TicketGUI> _closedTicketList = new List<TicketGUI>();
        private int _totalMyTicket;
        private int _totalOpenTicket;
        private int myTicketTotalPageNr = 1;
        public int MyTicketTotalPageNr
        {
            get { return myTicketTotalPageNr; }
            set { myTicketTotalPageNr = value; }
        }
        private int openTicketTotalPageNr = 1;
        public int OpenTicketTotalPageNr
        {
            get { return openTicketTotalPageNr; }
            set { openTicketTotalPageNr = value; }
        }
        private int closedTicketTotalPageNr = 1;
        public int ClosedTicketTotalPageNr
        {
            get { return closedTicketTotalPageNr; }
            set { closedTicketTotalPageNr = value; }
        }
        private int myTicketCurrentPageNr = 1;
        public int MyTicketCurrentPageNr
        {
            get { return myTicketCurrentPageNr; }
            set { myTicketCurrentPageNr = value; }
        }
        private int openTicketCurrentPageNr = 1;
        public int OpenTicketCurrentPageNr
        {
            get { return openTicketCurrentPageNr; }
            set { openTicketCurrentPageNr = value; }
        }
        private int closedTicketCurrentPageNr = 1;
        public int ClosedTicketCurrentPageNr
        {
            get { return closedTicketCurrentPageNr; }
            set { closedTicketCurrentPageNr = value; }
        }

        public List<TicketGUI> OpenTicketList
        {
            get { return _openTicketList; }
            set { _openTicketList = value; }
        }

        public int TotalOpenTicket
        {
            get { return _totalOpenTicket; }
            set { _totalOpenTicket = value; }
        }

        public int TotalMyTicket
        {
            get { return _totalMyTicket; }
            set { _totalMyTicket = value; }
        }

        public List<TicketGUI> ClosedTicketList
        {
            get { return _closedTicketList; }
            set { _closedTicketList = value; }
        }

        public List<TicketGUI> MyTicketList
        {
            get { return _myTicketList; }
            set { _myTicketList = value; }
        }

        public string CommunityName
        {
            get { return _communityName; }
            set { _communityName = value; }
        }

        public int CommunityId
        {
            get { return _communityId; }
            set { _communityId = value; }
        }
    }
}
