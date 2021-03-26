using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace GF.BrowserGame.Schema.Internal
{
    public class IpLog
    {
        private int id;
        private IPAddress ipAddress;
        private string ipString;
        private string date;
        private string host = "";
        private string country;
        private string region;
        private string city;
        private double latitude;
        private double longitude;
        private double distance;
        private string nick;
        private DateTime dateTime;

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public string IpString
        {
            get { return ipString; }
            set { ipString = value; }
        }

        public IPAddress IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
    }
}
