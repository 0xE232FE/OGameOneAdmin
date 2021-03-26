using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace LibCommonUtil
{
    sealed public class NetworkUtilities
    {
        /***********************************************************************************************************/


        #region ------ Declare External Function ------


        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState",
                   ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern bool InternetGetConnectedState(ref Int32 lpdwFlags, Int32 dwReserved);


        #endregion ------ Declare External Function ------


        /***********************************************************************************************************/


        #region ------ Public Enum ------


        /// <summary>
        /// Enum to hold the possible connection states
        /// </summary>
        [Flags]
        public enum ConnectionStatusEnum : int
        {
            INTERNET_CONNECTION_MODEM = 0x01,
            INTERNET_CONNECTION_LAN = 0x02,
            INTERNET_CONNECTION_PROXY = 0x04,
            INTERNET_CONNECTION_MODEM_BUSY = 0x08,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40,
            INTERNET_RAS_INSTALLED = 0x10
        }


        #endregion ------ Public Enum ------


        /***********************************************************************************************************/


        #region ------ Public Static Functions ------


        /// <summary>
        /// Method for retrieving the network online status
        /// </summary>
        /// <param name="host"></param>
        /// <returns>True / False</returns>
        public static bool GetOnlineStatus()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }


        /// <summary>
        /// Method for retrieving the network connection type
        /// </summary>
        /// <param name="host"></param>
        /// <returns>Type (int)</returns>
        public static int GetConnectionType()
        {
            Int32 lngFlags = 0;

            if (InternetGetConnectedState(ref lngFlags, 0))
            {
                // Connected.
                if ((lngFlags & (int)ConnectionStatusEnum.INTERNET_CONNECTION_LAN) != 0)
                {
                    // LAN connection.
                    return (int)ConnectionStatusEnum.INTERNET_CONNECTION_LAN;
                }
                else if ((lngFlags & (int)ConnectionStatusEnum.INTERNET_CONNECTION_MODEM) != 0)
                {
                    // Modem connection.
                    return (int)ConnectionStatusEnum.INTERNET_CONNECTION_MODEM;
                }
                else if ((lngFlags & (int)ConnectionStatusEnum.INTERNET_CONNECTION_PROXY) != 0)
                {
                    // Proxy connection.
                    return (int)ConnectionStatusEnum.INTERNET_CONNECTION_PROXY;
                }
                else
                {
                    return (int)ConnectionStatusEnum.INTERNET_CONNECTION_OFFLINE;
                }
            }
            else
            {
                // Not connected.
                return (int)ConnectionStatusEnum.INTERNET_CONNECTION_OFFLINE;
            }
        }


        /// <summary>
        /// Method for retrieving the IP address from the host provided
        /// </summary>
        /// <param name="host">the host we need the address for</param>
        /// <returns></returns>
        public static IPAddress GetIpFromHost(ref string host)
        {
            string returnMessage = string.Empty;

            //IPAddress instance for holding the returned host
            IPAddress address = null;

            try
            {
                //Get the host IP from the name provided
                address = Dns.GetHostEntry(host).AddressList[0];
            }
            catch (SocketException ex)
            {
                //Some DNS error happened, return the message
                returnMessage = string.Format("DNS Error: {0}", ex.Message);
            }

            return address;
        }


        /// <summary>
        /// Method to check the ping status of a provided host
        /// </summary>
        /// <param name="addr">The host we need to ping</param>
        /// <returns></returns>
        public static bool CheckSiteStatus(string host, int nrPing, ref string returnMessage)
        {
            bool online = false;

            //String to hold our return messge
            returnMessage = string.Empty;

            //IPAddress instance for holding the returned host
            IPAddress address = GetIpFromHost(ref host);

            if (address != null)
            {
                //Set the ping options, TTL 128
                PingOptions options = new PingOptions(128, true);

                //Create a new ping instance
                Ping ping = new Ping();

                //32 byte buffer
                byte[] data = new byte[32];

                //Ping the host
                for (int i = 0; i < nrPing; i++)
                {
                    try
                    {
                        //send the ping 4 times to the host and record the returned data
                        PingReply reply = ping.Send(address, 1000, data, options);

                        //make sure we dont have a null reply
                        if (!(reply == null))
                        {
                            switch (reply.Status)
                            {
                                case IPStatus.Success:
                                    returnMessage = string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}",
                                        reply.Address, reply.Buffer.Length, reply.RoundtripTime, reply.Options.Ttl);
                                    online = true;
                                    break;

                                case IPStatus.TimedOut:
                                    returnMessage = "Connection has timed out.";
                                    online = false;
                                    break;

                                default:
                                    returnMessage = string.Format("Ping failed: {0}", reply.Status.ToString());
                                    online = false;
                                    break;
                            }
                        }
                        else
                        {
                            returnMessage = "Connection failed for an unknown reason.";
                            online = false;
                        }
                    }
                    catch (PingException ex)
                    {
                        returnMessage = string.Format("Connection Error: {0}", ex.Message);
                        online = false;
                    }
                    catch (SocketException ex)
                    {
                        returnMessage = string.Format("Connection Error: {0}", ex.Message);
                        online = false;
                    }
                }
            }
            else
            {
                online = false;
            }

            return online;
        }


        #endregion ------ Public Static Functions ------


        /***********************************************************************************************************/
    }
}
