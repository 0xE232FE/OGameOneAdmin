using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;

namespace LibCommonUtil
{
    sealed public class TimeConverUtil
    {
        /***********************************************************************************************************/


        #region ------ Public Static Methods ------


        #region TimeZoneInfo


        public static NameValueCollection GetSystemTimeZones()
        {
            NameValueCollection timeZoneCollection = new NameValueCollection();
            foreach (TimeZoneInfo timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
            {
                timeZoneCollection.Add(timeZoneInfo.Id, timeZoneInfo.DisplayName);
            }
            return timeZoneCollection;
        }


        public static TimeZoneInfo GetTimeZoneInfo(string timeZoneId)
        {
            ReadOnlyCollection<TimeZoneInfo> timeZoneCollection = TimeZoneInfo.GetSystemTimeZones();

            var results = from timeZone in timeZoneCollection
                         where timeZone.Id.Equals(timeZoneId)
                         select timeZone;

            if (results != null && results.Count() == 1)
                return results.FirstOrDefault();
            else
                return null;
        }


        public static bool IsTimeZoneExisting(string timeZoneId)
        {
            TimeZoneInfo timeZoneInfo = GetTimeZoneInfo(timeZoneId);
            if (timeZoneInfo == null)
                return false;
            else
                return true;
        }


        public static int GetUtcOffsetInMinutes(string timeZoneId)
        {
            int returnValue = 0;
            TimeZoneInfo timeZone = GetTimeZoneInfo(timeZoneId);
            if (timeZone != null)
            {
                double timeOffset = timeZone.BaseUtcOffset.TotalMinutes;
                if (timeZone.IsDaylightSavingTime(DateTime.Now))
                    timeOffset += 60;
                returnValue = Convert.ToInt32(timeOffset);
            }
            return returnValue;
        }


        public static int GetUtcOffsetInMinutes(TimeZoneInfo timeZoneInfo)
        {
            int returnValue = 0;
            if (timeZoneInfo != null)
            {
                double timeOffset = timeZoneInfo.BaseUtcOffset.TotalMinutes;
                if (timeZoneInfo.IsDaylightSavingTime(DateTime.Now))
                    timeOffset += 60;
                returnValue = Convert.ToInt32(timeOffset);
            }
            return returnValue;
        }


        #endregion


        #region Convert Time


        private static DateTime ConvertTime(DateTime dtInput, TimeZoneInfo timeZone, bool bFromUTC)
        {
            dtInput = DateTime.SpecifyKind(dtInput, DateTimeKind.Unspecified);
            if (bFromUTC)
                return TimeZoneInfo.ConvertTimeFromUtc(dtInput, timeZone);
            else
                return TimeZoneInfo.ConvertTimeToUtc(dtInput, timeZone);
        }


        public static DateTime ConvertFromUTC(DateTime dtInput, string timeZoneId)
        {
            try
            {
                return ConvertTime(dtInput, GetTimeZoneInfo(timeZoneId), true);
            }
            catch
            {
                return dtInput;
            }
        }


        public static DateTime ConvertToUTC(DateTime dtInput, string timeZoneId)
        {
            try
            {
                return ConvertTime(dtInput, GetTimeZoneInfo(timeZoneId), false);
            }
            catch
            {
                return dtInput;
            }
        }


        public static DateTime ConvertFromUTC(DateTime dtInput, TimeZoneInfo timeZoneInfo)
        {
            try
            {
                return ConvertTime(dtInput, timeZoneInfo, true);
            }
            catch
            {
                return dtInput;
            }
        }


        public static DateTime ConvertToUTC(DateTime dtInput, TimeZoneInfo timeZoneInfo)
        {
            try
            {
                return ConvertTime(dtInput, timeZoneInfo, false);
            }
            catch
            {
                return dtInput;
            }
        }


        public static DateTime QuickConvertFromUTC(DateTime dtInput, TimeZoneInfo timeZoneInfo)
        {
            return dtInput.AddMinutes(GetUtcOffsetInMinutes(timeZoneInfo));
        }


        public static DateTime QuickConvertToUTC(DateTime dtInput, TimeZoneInfo timeZoneInfo)
        {
            return dtInput.AddMinutes(-GetUtcOffsetInMinutes(timeZoneInfo));
        }


        #endregion


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/
    }
}
