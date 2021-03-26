using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Static
{
    public static class Constants
    {
        public static class ApplicationData
        {
            public const string APPDATA_DIRECTORY = "Application\\";
            public const string USERDATA_DIRECTORY = "User Data\\{0}\\";
            public const string APPCONFIG_FILENAME = "app.config.ctx";
            public const string USERDATA_FILENAME = "user.ctx";
            public const string USERAPPCONFIGDATA_FILENAME = "user.app.config.ctx";
            public const string COMMUNITYDATA_FILENAME = "community.ctx";
            public const string UNIVERSE_FILENAME = "universe.ctx";
        }

        public static class Message
        {
            public const string WEBSERVICE_SETTINGS_NOT_FOUND = "Critical error, webservices settings cannot be found.";
            public const string TRY_NEXT_URL = "try next url";
            public const string WEBSERVICES_ARE_DOWN = "All celestos's webservices are temporarilly unvailable, please try again later.";
        }

        // Universe Settings Dictionary Keys
        public static class UniverseSettingsKeys
        {
            public const string UNI_NAME = "uni_name";
            public const string UNI_NUMBER = "uni_number";
            public const string UNI_SPEED = "uni_speed";
            public const string UNI_ISREDESIGN = "uni_isredesign";
            public const string UNI_ISACS = "uni_isacs";
            public const string UNI_ISRESOURCESINDF = "uni_isresourcesindf";
        }

        public static class UrlParameters
        {
            public const string SESSION = "session=";
            public const string PHPSESSID = "PHPSESSID=";
        }

        public static class MonthToShort
        {
            public const string JANUARY = "jan";
            public const string FEBRUARY = "feb";
            public const string MARCH = "mar";
            public const string APRIL = "apr";
            public const string MAY = "may";
            public const string JUNE = "jun";
            public const string JULY = "jul";
            public const string AUGUST = "aug";
            public const string SEPTEMBER = "sep";
            public const string OCTOBER = "oct";
            public const string NOVEMBER = "nov";
            public const string DECEMBER = "dec";
        }

        public static class DayToShort
        {
            public const string MONDAY = "mon";
            public const string TUESDAY = "tue";
            public const string WEDNESDAY = "wed";
            public const string THURSDAY = "thu";
            public const string FRIDAY = "fri";
            public const string SATURDAY = "sat";
            public const string SUNDAY = "sun";
        }

        public static class SecureObject
        {
            public const string VIEW_TAB_DASHBOARD = "view.tab.dashboard";
            public const string VIEW_TAB_NOTES = "view.tab.notes";
            public const string VIEW_TAB_TICKETS = "view.tab.tickets";
            public const string VIEW_TAB_MULTI = "view.tab.multi";
            public const string VIEW_TAB_STATS = "view.tab.stats";
            public const string USE_IE_LOGIN = "use.ie.login";
            public const string USE_URL_LOGIN = "use.url.login";
            public const string USE_QUICK_LOGIN = "use.quick.login";
            public const string USE_OPEN_BROWSER = "use.open.browser";
        }

        public const int SESSION_LENGTH = 12;
        public const string SESSION_INVALID = "<script>document.location.href=";
        public const string SESSION_EMPTY = "<script>document.location.href='index.php?page=overview&session=&displayError=1'</script>";
        public const string PAGE_ERROR = "displayError=1";
        public const string LOGIN_FAILED = "login failed";
        public const string ERROR = "error";
        public const string TRUE_VALUE = "true";
        public const string FALSE_VALUE = "false";
        public const string NEWLINE = "\r\n";
        public const string PREFIX = "--";
    }
}
