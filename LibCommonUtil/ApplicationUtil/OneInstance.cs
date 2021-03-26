using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace LibCommonUtil
{
    sealed public class OneInstance
    {
        /***********************************************************************************************************/


        #region ------ Declare External Functions ------


        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWinCallBack callBackFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern void GetWindowText(int hWnd, StringBuilder str, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);


        #endregion ------ Declare External Functions ------


        /***********************************************************************************************************/


        #region ------ Private Constant ------


        private const int SW_RESTORE = 9;


        #endregion  ------ Private Constant ------


        /***********************************************************************************************************/


        #region ------ Private Static Variables ------


        private static Mutex mutex;
        private static IntPtr windowHandle;
        private static string sTitle;


        #endregion ------ Private Static Variables ------


        /***********************************************************************************************************/


        #region ----- Delegate ------


        delegate bool EnumWinCallBack(int hwnd, int lParam);


        #endregion ----- Delegate ------


        /***********************************************************************************************************/


        #region ------ Private Methods ------


        /// <summary>
        /// EnumWindowCallBack
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static bool EnumWindowCallBack(int hwnd, int lParam)
        {
            windowHandle = (IntPtr)hwnd;
            StringBuilder sbuilder = new StringBuilder(256);
            GetWindowText((int)windowHandle, sbuilder, sbuilder.Capacity);
            string strTitle = sbuilder.ToString();

            if (strTitle == sTitle)
            {
                ShowWindow(windowHandle, SW_RESTORE);
                SetForegroundWindow(windowHandle);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Check if given exe alread running or not
        /// </summary>
        /// <returns>returns true if already running</returns>
        private static bool IsAlreadyRunning()
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            mutex = new Mutex(true, sExeName);

            if (mutex.WaitOne(0, false))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Check if given exe alread running or not
        /// </summary>
        /// <param name="exeName"></param>
        /// <returns>returns true if already running</returns>
        private static bool IsAlreadyRunning(string exeName)
        {
            mutex = new Mutex(true, exeName);

            if (mutex.WaitOne(0, false))
            {
                return false;
            }

            return true;
        }


        #endregion ------ Private Methods ------


        /***********************************************************************************************************/


        #region ------ Public Static Methods ------


        /// <summary>
        /// Execute a form base application if another instance already running on
        /// the system activate previous one
        /// </summary>
        /// <param name="frmMain">main form</param>
        /// <returns>true if no previous instance is running</returns>
        public static bool Run(Form frmMain)
        {
            if (IsAlreadyRunning())
            {
                sTitle = frmMain.Text;
                //set focus on previously running app
                int result = EnumWindows(new EnumWinCallBack(EnumWindowCallBack), 0);
                if (result == 1)
                {
                    MessageBox.Show("You may only start one instance of this application.", "Application is already running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }

            Application.Run(frmMain);

            return true;
        }


        /// <summary>
        /// Execute a form base application if another instance already running on
        /// the system activate previous one
        /// </summary>
        /// <param name="frmMain">main form</param>
        /// <param name="moduleName">name of the executable</param>
        /// <returns>true if no previous instance is running</returns>
        public static bool Run(Form frmMain, string exeName)
        {
            if (IsAlreadyRunning(exeName))
            {
                sTitle = frmMain.Text;
                //set focus on previously running app
                int result = EnumWindows(new EnumWinCallBack(EnumWindowCallBack), 0);
                if (result == 1)
                {
                    MessageBox.Show("You may only start one instance of this application.", "Application is already running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }

            Application.Run(frmMain);

            return true;
        }


        /// <summary>
        /// For console base application
        /// </summary>
        /// <returns></returns>
        public static bool Run()
        {
            if (IsAlreadyRunning())
            {
                return false;
            }

            return true;
        }


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/
    }
}
