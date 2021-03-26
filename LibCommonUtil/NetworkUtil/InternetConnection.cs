using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using log4net;

namespace LibCommonUtil
{
    /// <summary>
    /// Event publisher class, other classes will observe this class.
    /// This class publishes one event: OnlineStatusChange.
    /// The observers will subscribe to this event.
    /// </summary>
    public class InternetConnection
    {
        /***********************************************************************************************************/


        #region ----- Constant Variables ------


        #endregion ----- Constant Variables ------


        /***********************************************************************************************************/


        #region ----- Private Variables ------


        private bool _Status = false;
        private bool _IsFirstTime = true;
        private bool _IsOnline = false;
        private bool _IsEstablishConnection = false;

        private int _TimeOutInMinutes = 10;
        private int _ConnectionStatus = -1;
        private int _CheckInternetInterval = 1000;

        private ILog _log;
        private System.Windows.Forms.Timer _timer;
        private DateTime _dtStart;


        #endregion ----- Private Variables ------


        /***********************************************************************************************************/


        #region ----- Public ENUM ------


        public enum CONNECTIONSTATE
        {
            OFFLINE = -1,
            TIMEDOUT = 0,
            ONLINE = 1
        };


        #endregion ----- Public ENUM ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        /// <summary>
        /// The delegate named OnlineStatusHandler, which will encapsulate
        /// any method that takes a connection object and an OnlineStatusEventArgs
        /// object as the parameter and returns no value.
        /// It's the delegate the subscribers must implement.
        /// </summary>
        /// <param name="onlineStatusEvent"></param>
        public delegate void OnlineStatusHandler(OnlineStatusEventArgs onlineStatusEvent);


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        /// <summary>
        /// The publish event
        /// </summary>
        public event OnlineStatusHandler OnlineStatusChange;


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor & Destructor ------


        /// <summary>
        /// Constructor.
        /// </summary>
        public InternetConnection(ILog log)
        {
            this._log = log;

            AssemblySettings settings = new AssemblySettings();

            string sInterval = settings["CheckInternetInterval"].Trim();

            if (string.IsNullOrEmpty(sInterval))
            {
                log.Warn("Key CheckInternetInterval in OGameLib.dll.config file is not defined.");
            }
            else
            {
                _CheckInternetInterval = Int32.Parse(sInterval) * 1000;
            }

            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = _CheckInternetInterval;
            _timer.Start();
        }


        /// <summary>
        /// Destructor.
        /// </summary>
        ~InternetConnection()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }


        #endregion ----- Constructor & Destructor ------


        /***********************************************************************************************************/


        #region ----- Protected Fire Event ------


        /// <summary>
        /// The method which fires the Event.
        /// </summary>
        /// <param name="onlineStatusEvent"></param>
        protected void OnOnlineStatusChange(OnlineStatusEventArgs onlineStatusEvent)
        {
            // Check if there are any Subscribers
            if (OnlineStatusChange != null)
            {
                // Call the Event
                OnlineStatusChange(onlineStatusEvent);
            }
        }


        protected void Timer_Tick(object sender, EventArgs eArgs)
        {
            CheckInternetConnection();

            //if (!_IsOnline && _IsEstablishConnection)
            //{
            //    ReleaseProcess();
            //}
        }


        #endregion ----- Protected Fire Event ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        /// <summary>
        /// This will raise an event when there is a change in online status
        /// </summary>
        /// <param name="state"></param>
        private void CheckInternetConnection()
        {
            bool status;

            status = NetworkUtilities.GetOnlineStatus();

            // If the online status has changed notify the subscribers
            if ((status != _Status) || _IsFirstTime)
            {
                if (status)
                {
                    _ConnectionStatus = (int)CONNECTIONSTATE.ONLINE;
                    _IsOnline = true;
                }
                else
                {
                    _ConnectionStatus = (int)CONNECTIONSTATE.OFFLINE;
                    _IsOnline = false;
                }

                // Create the OnlineStatusEventArgs object to pass to the subscribers
                OnlineStatusEventArgs onlineStatusEvent = new OnlineStatusEventArgs(_ConnectionStatus);

                // If anyone has subscribed, notify them
                OnOnlineStatusChange(onlineStatusEvent);

                _IsFirstTime = false;
            }

            // update online status state
            _Status = status;

        }


        #endregion ----- Private Methods ------


        /***********************************************************************************************************/


        #region ----- Public Method ------


        /// <summary>
        /// Method to Establish Internet Connection.
        /// Default timeout is 10 minutes.
        /// </summary>
        public void EstablishInternetConnection()
        {
        }


        /// <summary>
        /// Overload method to Establish Internet Connection.
        /// Timeout inminutes can be specified.
        /// </summary>
        /// <param name="timeOutInMinutes"></param>
        public void EstablishInternetConnection(int timeOutInMinutes)
        {
            _TimeOutInMinutes = timeOutInMinutes;
        }


        /// <summary>
        /// Method to Disconnect Internet Connection.
        /// </summary>
        public void DisconnectInternetConnection()
        {
        }


        #endregion ----- Public Method ------


        /***********************************************************************************************************/
    }


    /***********************************************************************************************************/


    /// <summary>
    /// The class to hold the information about the event.
    /// </summary>
    public class OnlineStatusEventArgs : EventArgs
    {
        /***********************************************************************************************************/


        #region ----- Public Readonly Variable ------


        public readonly int ConnectionState;


        #endregion ----- Public Readonly Variable ------


        /***********************************************************************************************************/


        #region ----- Public Event Args Method ------


        /// <summary>
        /// Online Status Event Args Method.
        /// </summary>
        /// <param name="connectionState"></param>
        public OnlineStatusEventArgs(int connectionState)
        {
            ConnectionState = connectionState;
        }


        #endregion ----- Public Event Args Method ------


        /***********************************************************************************************************/
    }


    /***********************************************************************************************************/
}
