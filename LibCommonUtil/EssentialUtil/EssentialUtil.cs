using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net;

namespace LibCommonUtil
{
    sealed public class EssentialUtil
    {

        /***********************************************************************************************************/


        #region ------ Extern Static Functions ------


        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string url, string name, string data);

        [DllImport("ieframe.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IESetProtectedModeCookie(string url, string name, string data, int flags);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(
          string url, string cookieName, StringBuilder cookieData, ref int size);


        #endregion ------ Extern Static Functions ------


        /***********************************************************************************************************/


        #region ------ Private Constant ------


        private const int MF_BYPOSITION = 0x400;
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;


        #endregion ------ Private Constant ------


        /***********************************************************************************************************/


        #region ------ Private Static Variable ------


        #endregion ------ Private Static Variable ------


        /***********************************************************************************************************/


        #region ------ Public Static Methods ------


        /// <summary>
        /// Disable the close button on Windows Form.
        /// </summary>
        /// <param name="controlHandle"></param>
        public static void DisableCloseButtonOnWinForm(IntPtr controlHandle)
        {
            IntPtr hMenu = GetSystemMenu(controlHandle, false);

            int menuItemCount = GetMenuItemCount(hMenu);

            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }


        /// <summary>
        /// Extract executable filepath and executable arguments.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="filepath"></param>
        /// <param name="arguments"></param>
        public static void ExtractExecutablePathAndArgument(string command, ref string filepath, ref string arguments)
        {
            int iIndex = -1;
            iIndex = command.ToUpper().IndexOf(".EXE");
            if (iIndex == -1)
                iIndex = command.ToUpper().IndexOf(".BAT");

            if (iIndex > 0)
            {
                filepath = command.Substring(0, iIndex + 4);
                arguments = command.Substring(iIndex + 4);
            }
        }


        /// <summary>
        /// Format message with current date time stamp.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string FormatMessage(string message)
        {
            return DateTime.Now.ToString() + " : " + message;
        }


        /// <summary>
        /// Format elasped time by count seconds.
        /// </summary>
        /// <param name="elaspedTime"></param>
        /// <returns></returns>
        public static string FormatElaspedTime(int elaspedSeconds)
        {
            string sReturn = "00:00:00";  // Return value format.

            int iHour = (elaspedSeconds / 3600);
            int iMinute = ((elaspedSeconds / 60) - (iHour * 60));
            int iSecond = elaspedSeconds - ((iHour * 3600) + (iMinute * 60));

            if (iHour < 0)
                iHour = iHour * -1;

            if (iMinute < 0)
                iMinute = iMinute * -1;

            if (iSecond < 0)
                iSecond = iSecond * -1;

            if (iHour >= 10)
                sReturn = iHour.ToString();
            else
                sReturn = "0" + iHour.ToString();

            if (iMinute >= 10)
                sReturn = sReturn + ":" + iMinute.ToString();
            else
                sReturn = sReturn + ":0" + iMinute.ToString();

            if (iSecond >= 10)
                sReturn = sReturn + ":" + iSecond.ToString();
            else
                sReturn = sReturn + ":0" + iSecond.ToString();

            return sReturn;
        }


        /// <summary>
        /// Format a number by separating thousands with a symbol such as . or ,
        /// </summary>
        /// <param name="number"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormatNumber(int number, string separator)
        {
            string sReturn = "";  // Return value format 1,000,000.

            if (number > 0)
            {
                string s = number.ToString();
                char[] c = s.ToCharArray();
                int count = 1;
                for (int i = c.Length - 1; i >= 0; i--)
                {
                    if (count > 3)
                    {
                        count = 1;
                        sReturn = c[i] + separator + sReturn;
                        count++;
                    }
                    else
                    {
                        sReturn = c[i] + sReturn;
                        count++;
                    }
                }
                return sReturn;
            }
            else return number.ToString();
        }


        /// <summary>
        /// Format a number by separating thousands with a symbol such as . or ,
        /// </summary>
        /// <param name="number"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormatNumber(long number, string separator)
        {
            string sReturn = "";  // Return value format 1,000,000.

            if (number > 0)
            {
                string s = number.ToString();
                char[] c = s.ToCharArray();
                int count = 1;
                for (int i = c.Length - 1; i >= 0; i--)
                {
                    if (count > 3)
                    {
                        count = 1;
                        sReturn = c[i] + separator + sReturn;
                        count++;
                    }
                    else
                    {
                        sReturn = c[i] + sReturn;
                        count++;
                    }
                }
                return sReturn;
            }
            else return number.ToString();
        }


        /// <summary>
        /// Format a number by separating thousands with a symbol such as . or ,
        /// </summary>
        /// <param name="number"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormatNumber(double number, string separator)
        {
            string sReturn = "";  // Return value format 1,000,000.

            if (number > 0)
            {
                string s = number.ToString();
                if (s.Contains("."))
                {
                    separator = ",";
                    string[] values = s.Split('.');
                    s = values[0];
                    sReturn = "." + values[1];
                }
                char[] c = s.ToCharArray();
                int count = 1;
                for (int i = c.Length - 1; i >= 0; i--)
                {
                    if (count > 3)
                    {
                        count = 1;
                        sReturn = c[i] + separator + sReturn;
                        count++;
                    }
                    else
                    {
                        sReturn = c[i] + sReturn;
                        count++;
                    }
                }
                return sReturn;
            }
            else return number.ToString();
        }


        /// <summary>
        /// Compare two version with the format a.b.c.d.
        /// </summary>
        /// <param name="s1">The s1.</param>
        /// <param name="s2">The s2.</param>
        /// <returns>1 = great than; -1 = less than; 0 = equal</returns>
        public static int CompareVersion(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1))
                throw new NullReferenceException("Parameters of function CompareVersion is empty!");
            if (string.IsNullOrEmpty(s2))
                return 1;
            string[] sa1 = s1.Split('.');
            string[] sa2 = s2.Split('.');
            for (int i = 0; i < sa1.Length; i++)
            {
                if (int.Parse(sa1[i]) > int.Parse(sa2[i]))
                    return 1;
                else if (int.Parse(sa1[i]) < int.Parse(sa2[i]))
                    return -1;

            }
            return 0;
        }


        /// <summary>
        /// Get a list of all countries stored in Windows registry
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCountryList()
        {
            //Create a new generic list to hold the country names returned
            List<string> countriesList = new List<string>();

            RegistryKey countries =
                Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Telephony\\Country List");

            if (countries != null)
            {
                // Get the list of subkeys, which is actually the country code
                string[] subkeys = countries.GetSubKeyNames();

                foreach (string ccode in subkeys)
                {
                    // Open the subkey, to get the country name
                    RegistryKey country = countries.OpenSubKey(ccode);

                    countriesList.Add(country.GetValue("Name").ToString());

                    // We're done using the key, we should close it
                    country.Close();
                }

                // It's now safe to close the country list key
                countries.Close();
            }

            countriesList.Sort();

            return countriesList;
        }


        public static void OpenDefaultWebBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void OpenWebBrowser(string webBrowserType, string url)
        {
            try
            {
                if (webBrowserType.Equals("Internet Explorer"))
                    Process.Start("IExplore.exe", url);
                else if (webBrowserType.Equals("Firefox"))
                    Process.Start("firefox.exe", url);
                else if (webBrowserType.Equals("Chrome"))
                    Process.Start("chrome.exe", url);
                else
                    OpenDefaultWebBrowser(url);
            }
            catch (Exception ex)
            {
                OpenDefaultWebBrowser(url);
            }
        }


        public static string GetInnerExceptionMessage(Exception ex)
        {
            if (ex != null && ex.InnerException != null)
                return FindInnerExceptionMessage(ex.InnerException).Message;
            else
                return null;
        }


        public static Exception FindInnerExceptionMessage(Exception innerException)
        {
            if (innerException != null && innerException.InnerException != null)
                return FindInnerExceptionMessage(innerException.InnerException);
            else
                return innerException;
        }


        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;

            // Determine the size of the cookie
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookie(uri.ToString(), null, cookieData,
              ref datasize))
            {
                if (datasize < 0)
                    return null;

                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), null, cookieData,
                  ref datasize))
                    return null;
            }

            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }


        public static string GetUriCookieString(Uri uri)
        {
            CookieContainer cookies = null;

            // Determine the size of the cookie
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookie(uri.ToString(), null, cookieData,
              ref datasize))
            {
                if (datasize < 0)
                    return null;

                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), null, cookieData,
                  ref datasize))
                    return null;
            }
            return cookieData.ToString();
        }


        public static bool SetWinINETCookieString(string sURL, string sName, string sData)
        {
            //IESetProtectedModeCookie(sURL, sName, sData + "; expires= " + String.Format("{0:r}", DateTime.Now.AddHours(1)) + "; path=/", 0x10);
            IESetProtectedModeCookie(sURL, sName, sData + "; expires= " + String.Format("{0:r}", DateTime.Now.AddHours(1)) + "; path=/", 0);
            return InternetSetCookie(sURL, sName, sData + "; expires= " + String.Format("{0:r}", DateTime.Now.AddHours(1)) + "; path=/");
        }


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/
    }
}
