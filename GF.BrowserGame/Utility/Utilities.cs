using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using GF.BrowserGame.Static;
using LibCommonUtil;
using System.Diagnostics;
using GF.BrowserGame.Schema.Serializable;

namespace GF.BrowserGame.Utility
{
    internal static class Utilities
    {
        public static void OpenDefaultWebBrowser(string url)
        {
            EssentialUtil.OpenDefaultWebBrowser(url);
        }

        public static void OpenWebBrowser(string webBrowserType, string url)
        {
            EssentialUtil.OpenWebBrowser(webBrowserType, url);
        }

        public static string AppendCookiesToUrl(string url, string cookies)
        {
            string newUrl = "";
            string lastUrlChar = url.Substring(url.Length - 1);

            if (lastUrlChar.Equals('/'))
            {
                newUrl = url + "load/cookies/v1/" + cookies + "/redir/";
            }
            else if (url.LastIndexOf('/') != -1)
            {
                newUrl = url.Substring(0, url.LastIndexOf('/') + 1);
                newUrl = newUrl + "load/cookies/v1/" + cookies + "/redir/";

                string redir = url.Substring(url.LastIndexOf('/') + 1);

                if (redir.Length > 0)
                    newUrl = newUrl + redir;
                else
                    newUrl = newUrl + "/";
            }
            return newUrl;
        }

        public static string GetCookiesString(List<WebClientCookie> cookies)
        {
            string cookieString = "";
            foreach (WebClientCookie cookie in cookies)
            {
                cookieString += cookie.Key + "=" + cookie.Value + "|";
            }
            return cookieString;
        }

        public static bool StringToBoolean(string value)
        {
            if (value.Equals(Constants.TRUE_VALUE))
                return true;
            else
                return false;
        }

        public static string StringToMD5Hash(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value);

            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string StringTo64(string value)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(value);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string GetPostStringFromUri(Uri uri)
        {
            if (uri != null && !string.IsNullOrEmpty(uri.Query))
                if (uri.Query.StartsWith("?"))
                    return uri.Query.Substring(1);
                else
                    return uri.Query;
            else
                return string.Empty;
        }

        internal static string SerializeObjectToString<T>(T obj, string password)
        {
            try
            {
                //return Encryption.EncryptString(SerializeDeserializeObject.SerializeObject<T>(obj), password);
                return SerializeDeserializeObject.SerializeObject<T>(obj);
            }
            catch (Exception ex)
            {
                return Constants.ERROR;
            }
        }

        internal static T DeSerializeObjectFromString<T>(string encryptedString, string password)
        {
            try
            {
                //return SerializeDeserializeObject.DeserializeObject<T>(Encryption.DecryptString(encryptedString, password));
                return SerializeDeserializeObject.DeserializeObject<T>(encryptedString);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        internal static bool SerializeToFile<T>(T obj, string fileName, string password)
        {
            try
            {
                //string encryptedData = Encryption.EncryptString(SerializeDeserializeObject.SerializeObject<T>(obj), password);
                //SerializeDeserializeObject.SerializeObjectToFile<String>(fileName, true, encryptedData);
                SerializeDeserializeObject.SerializeObjectToFile<T>(fileName, true, obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static T DeSerializeFromFile<T>(string fileName, string password)
        {
            try
            {
                //string encryptedData = SerializeDeserializeObject.DeserializeObjectFromFile<String>(fileName);
                //return SerializeDeserializeObject.DeserializeObject<T>(Encryption.DecryptString(encryptedData, password));
                return SerializeDeserializeObject.DeserializeObjectFromFile<T>(fileName);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        internal static int MonthToInt(string month)
        {
            switch (month.ToLower())
            {
                case Constants.MonthToShort.JANUARY:
                    return 1;
                case Constants.MonthToShort.FEBRUARY:
                    return 2;
                case Constants.MonthToShort.MARCH:
                    return 3;
                case Constants.MonthToShort.APRIL:
                    return 4;
                case Constants.MonthToShort.MAY:
                    return 5;
                case Constants.MonthToShort.JUNE:
                    return 6;
                case Constants.MonthToShort.JULY:
                    return 7;
                case Constants.MonthToShort.AUGUST:
                    return 8;
                case Constants.MonthToShort.SEPTEMBER:
                    return 9;
                case Constants.MonthToShort.OCTOBER:
                    return 10;
                case Constants.MonthToShort.NOVEMBER:
                    return 11;
                case Constants.MonthToShort.DECEMBER:
                    return 12;
                default:
                    return -1;
            }

        }

        internal static int DayToInt(string day)
        {
            switch (day.ToLower())
            {
                case Constants.DayToShort.MONDAY:
                    return 1;
                case Constants.DayToShort.TUESDAY:
                    return 2;
                case Constants.DayToShort.WEDNESDAY:
                    return 3;
                case Constants.DayToShort.THURSDAY:
                    return 4;
                case Constants.DayToShort.FRIDAY:
                    return 5;
                case Constants.DayToShort.SATURDAY:
                    return 6;
                case Constants.DayToShort.SUNDAY:
                    return 7;
                default:
                    return -1;
            }

        }
    }
}
