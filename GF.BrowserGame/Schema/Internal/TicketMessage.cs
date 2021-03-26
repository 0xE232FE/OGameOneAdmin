using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Schema.Internal
{
    public class TicketMessage
    {
        private string nickName;
        private DateTime messageDateTime;
        private string message;
        private string ipAddress;

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public DateTime MessageDateTime
        {
            get { return messageDateTime; }
            set { messageDateTime = value; }
        }

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
    }
}
