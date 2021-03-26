using Celestos.Web.Static;
using LibCommonUtil;
using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Mail;

namespace Celestos.Web.BLL
{
    /// <summary>
    /// Summary description for UtilitiesBLL
    /// </summary>
    public class UtilitiesBLL
    {
        public UtilitiesBLL()
        {
        }

        public static string GetConnectionString()
        {
            string defaultDatabase = ConfigurationManager.AppSettings["defaultDatabase"];
            if (string.IsNullOrEmpty(defaultDatabase)) throw new Exception("Section 'appSetting' does not contain 'defaultDatabase' key");

            string sConnectionString = ConfigurationManager.ConnectionStrings[defaultDatabase].ConnectionString;
            if (string.IsNullOrEmpty(sConnectionString))
                throw new Exception("Config file does not contain connectionStrings section");

            return Encryption.DecryptString(sConnectionString);
        }

        public static Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetInnerException(ex.InnerException);
            else
                return ex;
        }

        public static string GetWebsiteUrl()
        {
            string host = HttpContext.Current.Request.Url.Authority;
            if (host.Contains("www"))
                return Constants.WEBSITE_BASE_URL;
            else
                return "https://" + HttpContext.Current.Request.Url.Authority + "/";
        }

        /// <summary>
        /// Extract the directory from the path requested and verify if it 
        /// corresponds to the profile directory.
        /// </summary>
        /// <param name="path">Path requested</param>
        /// <returns>Boolean</returns>
        public static bool IsProfileDirectory(string path)
        {
            string directory;
            if (IsDirectoryValid(path, out directory) && directory.Equals(Constants.PROFIL_DIRECTORY))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Extract the directory from the path requested and verify if it exists.
        /// </summary>
        /// <param name="path">Path requested</param>
        /// <param name="directory">directory extracted from the path requested</param>
        /// <returns>Boolean and the directory extracted from the path requested</returns>
        public static bool IsDirectoryValid(string path, out string directory)
        {
            char[] splitter = { '/' };
            directory = path.Split(splitter)[Constants.DIRECTORY_INDEX] + "/";

            switch (directory)
            {
                case Constants.ADMIN_DIRECTORY:
                    return true;
                case Constants.PROFIL_DIRECTORY:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Get the UI translation of a given RoleName
        /// </summary>
        public static string RoleNameToUI(string roleName)
        {
            switch (roleName)
            {
                case Constants.ROLE_ADMINISTRATOR:
                    return "Administrator";
                default:
                    return String.Empty;
            }
        }

        public static string EmptyIfNull(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            else
                return value;
        }

        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsGuidNull(Guid guid)
        {
            if (guid == null || guid == new Guid(Constants.GUID_IS_NULL))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Try parsing the datetime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(string dateTime)
        {
            IFormatProvider culture = CultureInfo.CreateSpecificCulture("en-AU");
            DateTimeStyles styles = DateTimeStyles.None;
            DateTime dateResult;

            DateTime.TryParse(dateTime, culture, styles, out dateResult);

            return dateResult;
        }

        /// <summary>
        /// Get start date and end date of the last X days
        /// </summary>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        public static void GetDates(int days, ref DateTime startDate, ref DateTime endDate)
        {
            startDate = DateTime.Today.AddDays(-days); ;
            endDate = DateTime.Today;
        }

        /// <summary>
        /// Get This week start date and end date (mon-sun)
        /// </summary>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        public static void GetThisWeekDates(ref DateTime startDate, ref DateTime endDate)
        {
            DayOfWeek day = DateTime.Now.DayOfWeek;
            int days = day - DayOfWeek.Monday;
            startDate = DateTime.Today.AddDays(-days); ;
            endDate = startDate.AddDays(6);
        }

        /// <summary>
        /// Get last week start date and end date (mon-sun)
        /// </summary>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        public static void GetLastWeekDates(ref DateTime startDate, ref DateTime endDate)
        {
            DayOfWeek day = DateTime.Now.DayOfWeek;
            int days = day - DayOfWeek.Monday;
            startDate = DateTime.Today.AddDays(-days - 7);
            endDate = startDate.AddDays(6);
        }

        /// <summary>
        /// Get this month start date and end date
        /// </summary>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        public static void GetThisMonthDates(ref DateTime startDate, ref DateTime endDate)
        {
            startDate = new DateTime(DateTime.Today.Year, DateTime.Now.Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Get the last month start date and end date
        /// </summary>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        public static void GetLastMonthDates(ref DateTime startDate, ref DateTime endDate)
        {
            DateTime today = DateTime.Today;
            startDate = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM
        /// </summary>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        public static bool IsDateValid(DateTime date)
        {
            if (date >= new DateTime(1753, 1, 1) && date <= new DateTime(9999, 12, 31))
                return true;
            else
                return false;
        }

        public static DateTime GetDateTimeMinValue()
        {
            return new DateTime(1753, 1, 1);
        }

        public static DateTime GetDateTimeMaxValue()
        {
            return new DateTime(9999, 12, 31);
        }

        #region Encryption Methods

        public static string CreateHTMLHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.EncryptString(text, Encryption.DecryptString(Constants.HTML_ENCRYPTION_PASSWORD));
        }

        public static string DecryptHTMLHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.DecryptString(text, Encryption.DecryptString(Constants.HTML_ENCRYPTION_PASSWORD));
        }

        public static bool ValidateHTMLHash(string text, string hash)
        {
            if (text.Equals(Encryption.DecryptString(hash, Encryption.DecryptString(Constants.HTML_ENCRYPTION_PASSWORD))))
                return true;
            else
                return false;
        }

        public static string CreateDatabaseHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.EncryptString(text, Encryption.DecryptString(Constants.DATABASE_ENCRYPTION_PASSWORD));
        }

        public static string DecryptDatabaseHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.DecryptString(text, Encryption.DecryptString(Constants.DATABASE_ENCRYPTION_PASSWORD));
        }

        public static string CreateInCodeHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.EncryptString(text, Encryption.DecryptString(Constants.INCODE_ENCRYPTION_PASSWORD));
        }

        public static string DecryptInCodeHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.DecryptString(text, Encryption.DecryptString(Constants.INCODE_ENCRYPTION_PASSWORD));
        }

        public static string CreateEncryptionKeyHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.EncryptString(text, Encryption.DecryptString(Constants.ENCRYPTION_KEY_PASSWORD));
        }

        public static string DecryptEncryptionKeyHash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return Encryption.DecryptString(text, Encryption.DecryptString(Constants.ENCRYPTION_KEY_PASSWORD));
        }

        #endregion

        public static void SendEmail(string mailTo, string mailSubject, string mailBody, int smtpAccount)
        {
            MailMessage mail = new MailMessage();

            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);

            if (smtpAccount == 1)
            {
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "admin@celestos.net");
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "");
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", false);
                mail.From = "admin@celestos.net";
                SmtpMail.SmtpServer = "mail.celestos.net";
            }
            else if (smtpAccount == 2)
            {
                //mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "no-reply@celestos.net");
                //mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "");
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", false);
                mail.From = "no-reply@celestos.net";
                SmtpMail.SmtpServer = "relay-hosting.secureserver.net";
            }
            else
            {
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "celestos.net@gmail.com");
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "");
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", true);
                mail.From = "celestos.net@gmail.com";
                SmtpMail.SmtpServer = "smtp.gmail.com";
            }

            mail.To = mailTo;

            mail.Subject = mailSubject;

            mail.Body = mailBody;

            //mail.BodyFormat = MailFormat.Html;

            SmtpMail.Send(mail);
        }

        public static void SendInvitationEmail(string mailTo, string communityName, Guid communityId, Int64 invitationId)
        {
            string mailSubject = "Invitation to join " + communityName + " staff tool website";
            string mailBody = "Hello there,\r\n\r\nPlease click this link to join your community:\r\n\r\n" +
                              GetWebsiteUrl() + "/register.aspx?invite=" +
                              HttpUtility.UrlEncode(CreateHTMLHash(invitationId.ToString() + "|" + communityName + "|" + communityId.ToString() + "|" + DateTime.UtcNow.Ticks.ToString())) +
                              "\r\n\r\nThis link will expire in 7 days.\r\n\r\nThank you.";

            UtilitiesBLL.SendEmail(mailTo, mailSubject, mailBody, Constants.SMTP_ACCOUNT);
        }

        public static string UrlEncodePostString(string postString)
        {
            char[] delimiters = { '&' };
            string[] postPairs = postString.Split(delimiters);
            string returnPostString = "";

            foreach (string pair in postPairs)
            {
                char[] keyDelimiters = { '=' };
                string[] keyAndValue = pair.Split(keyDelimiters);
                if (keyAndValue.Length > 1)
                {
                    returnPostString += keyAndValue[0] + "=" + HttpUtility.UrlEncode(keyAndValue[1]);
                }
            }

            return returnPostString;
        }

        public static string UrlEncodeBase64PostString(string postString)
        {
            try
            {
                string returnPostString = "";

                int delimiterIndex = postString.IndexOf('=');

                string key = postString.Substring(0, delimiterIndex + 1);
                string base64Value = postString.Substring(delimiterIndex + 1);

                return returnPostString = key + HttpUtility.UrlEncode(base64Value);
            }
            catch (Exception e)
            {
                return HttpUtility.UrlEncode(postString);
            }
        }

        public static void CheckIfDateFromIsPriorDateTo(DateTime dateFrom, Nullable<DateTime> dateTo)
        {
            if (dateFrom == null)
                throw new Exception("Date from cannot be null or empty");
            else if (dateFrom != null && dateTo == null)
                return;
            else if (dateTo != null && (dateFrom.Ticks > dateTo.Value.Ticks))
                throw new Exception("Date from must be prior to Date to");
        }

        public static bool GetBooleanValue(object boolValue, object valueIfNull)
        {
            if (boolValue == null || boolValue == DBNull.Value)
                return valueIfNull.ToString().ToLower().Equals("true") ? true : false;
            else
                return (bool)boolValue;
        }

        public static int GetIntegerValue(object intValue, object valueIfNull)
        {
            if (intValue == null || intValue == DBNull.Value)
                return int.Parse(valueIfNull.ToString());
            else
                return (int)intValue;
        }
    }
}