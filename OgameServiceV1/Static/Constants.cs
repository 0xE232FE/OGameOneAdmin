using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Celestos.Web.Static
{
    /// <summary>
    /// Summary description for Constants
    /// </summary>
    public class Constants
    {
        public Constants()
        {
        }

        // The application name as it appears in the database
        public const string APPLICATION_NAME = "/";
        public const string WINDOW_TITLE = "OGame International Staff Tool";
        public const string WEBSITE_TITLE = "OGame International Staff Tool";

        //Settings
        public const string DROPDOWNLIST_SELECT = "Select";
        public static string[] LANGUAGE_LIST = { "en-GB", "en-US", "en-AU", "fr", "de" };
        public const string DEFAULT_DATE_TIME_FORMAT = "default";
        public static string[] DATE_FORMAT_LIST = { "dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yyyy", "MM.dd.yyyy", "MM/dd/yyyy", "MM-dd-yyyy", "yyyy.MM.dd", "yyyy/MM/dd", "yyyy-MM-dd" };
        public static string[] TIME_FORMAT_LIST = { "hh:mm:ss tt", "HH:mm:ss" };
        public const string DEFAULT_CULTURE_NAME = "en";
        public const string GUID_IS_NULL = "00000000-0000-0000-0000-000000000000";
        public const string DEFAULT_TIME_ZONE = "UTC";
        public const int SMTP_ACCOUNT = 3; // 1 -> Celestos.net | 2 -> Godaddy | 3 -> Gmail
        public const string MAIL_EXCEPTION_TO = "stephane.chevassus@gmail.com";


        //Directories config
        public const string RELATIVE_PATH = "~/";
        public const string WEB_ROOT_PATH = "/";
        public const string WEBSITE_BASE_URL = "https://celestos.azurewebsites.net/";
        public const string WEBSERVICE_BASE_URL = "https://ogameservicev1.azurewebsites.net/";
        public const string WEBSERVICE_URL = @"http://";
        public const int DIRECTORY_INDEX = 2;
        public const string ADMIN_DIRECTORY = "administrator/";
        public const string PROFIL_DIRECTORY = "profil/";

        //Encryption Password
        public const string INCODE_ENCRYPTION_PASSWORD = "Gtyh5EtUdTqgRJQgLoLkzR4HBmSHRih8qdgn1/9l+tA=";//"kd*s-+D354h*)@NDT@Hdu3_!@#HHD";
        public const string HTML_ENCRYPTION_PASSWORD = "bL33a8VHPjEaZMHk7NIXRojzfDRI1zy2Ft75yhmN7AQ=";//"h-d8=!7_s+E9*#@Sd_uUo%$";
        public const string DATABASE_ENCRYPTION_PASSWORD = "G3JQgK64N5jIntVXZ6YBgXNb1rJBY9FPn9E660rTF4M=";//"j@s3&sY^gs_-+xS=!Zo14";
        public const string ENCRYPTION_KEY_PASSWORD = "qVDExzVQXev08rDHEVHs/CVhoDxkTMHGIKmSowAAy5E=";//"j5@jUD83g2_D*(p.dj)z]";

        //Pages config
        public const string DEFAULT_PAGE = "default.aspx";
        public const string HOME_PAGE = "home.aspx";
        public const string LOGIN_PAGE = "login.aspx";
        public const string REGISTER_PAGE = "register.aspx";
        public const string LOGOUT_PAGE = "logout.aspx";
        public const string ACCOUNT_PAGE = "account.aspx";
        public const string MESSAGES_PAGE = "messages.aspx";
        public const string DASHBOARD_PAGE = "dashboard.aspx";
        public const string CHANGEYOURPASSWORD_PAGE = "changeYourPassword.aspx";
        public const string UNHAUTHORIZED_PAGE = "unauthorizedAccess.aspx";
        public const string FORGOTPASSWORD_PAGE = "forgotPassword.aspx";

        //Roles Name
        public const string ROLE_ADMINISTRATOR = "Administrator";
        public const string ROLE_COMMUNITY_MANAGER = "Community Manager";
        public const string ROLE_GAME_ADMINISTRATOR = "Game Administrator";
        public const string ROLE_SUPER_GAME_OPERATOR = "Super Game Operator";
        public const string ROLE_GAME_OPERATOR = "Game Operator";

        //Gender
        public const string GENDER_MALE = "Gender_Male";
        public const string GENDER_FEMALE = "Gender_Female";

        //Title
        public const string TITLE_MR = "TITLE_MR";
        public const string TITLE_MRS = "TITLE_MRS";
        public const string TITLE_MS = "TITLE_MS";
        public const string TITLE_MISS = "TITLE_MISS";

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