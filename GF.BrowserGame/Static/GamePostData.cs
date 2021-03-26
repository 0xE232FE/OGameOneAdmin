using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GF.BrowserGame.Static
{
    internal static class GamePostData
    {
        public static Dictionary<string, string> GetLogin(string userName, string password)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("login", userName);
            postData.Add("pass", password);
            postData.Add("v", "2");
            postData.Add("is_utf8", "0");
            return postData;
        }

        public static Dictionary<string, string> GetAdminSetLanguage(string language)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("langselect", language);
            return postData;
        }

        public static Dictionary<string, string> GetAdminPrivateNote(string note)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("notiz", note);
            return postData;
        }

        public static Dictionary<string, string> GetAdminCreateAdminNote(int adminId, string note)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("go", adminId.ToString());
            postData.Add("notiz", note);
            postData.Add("senden", "   Send   ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminCreateGeneralNote(string note)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("notiz", note);
            postData.Add("senden", "   Send   ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminSearch(string search)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("element", search);
            postData.Add("senden", " Send ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPillorySearch(string search)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("such", search);
            return postData;
        }

        //public static Dictionary<string, string> GetAdminMatchingData(bool password, bool ip, bool email, bool alliance)
        //{
        //    Dictionary<string, string> postData = new Dictionary<string, string>();

        //    if (password)
        //        postData.Add("x[]", "0");

        //    if (ip)
        //        postData.Add("x[]", "1");

        //    if (email)
        //        postData.Add("x[]", "2");

        //    if (alliance)
        //        postData.Add("x[]", "3");

        //    postData.Add("ok", "send");
        //    return postData;
        //}

        public static string GetAdminMatchingData(bool password, bool ip, bool email, bool alliance)
        {
            string postData = "";

            if (ip)
                postData += HttpUtility.UrlEncode("x[]") + "=" + HttpUtility.UrlEncode("0");

            if (email)
                postData += "&" + HttpUtility.UrlEncode("x[]") + "=" + HttpUtility.UrlEncode("1");

            if (alliance)
                postData += "&" + HttpUtility.UrlEncode("x[]") + "=" + HttpUtility.UrlEncode("2");

            if (password)
                postData += "&" + HttpUtility.UrlEncode("x[]") + "=" + HttpUtility.UrlEncode("3");

            postData += "&" + HttpUtility.UrlEncode("ok") + "=" + HttpUtility.UrlEncode("send");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerList(int pageNumber, int maxPerPage)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("max_per_side", maxPerPage.ToString());
            postData.Add("side", pageNumber.ToString());
            postData.Add("checktype", "1");
            postData.Add("senden", "ok");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlanetList(int pageNumber, int maxPerPage)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("max_per_side", maxPerPage.ToString());
            postData.Add("side", pageNumber.ToString());
            postData.Add("checktype", "2");
            postData.Add("senden", "ok");
            return postData;
        }

        public static Dictionary<string, string> GetAdminAllianceList(int pageNumber, int maxPerPage, bool displayPicture, int pictureSize)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("max_per_side", maxPerPage.ToString());
            postData.Add("side", pageNumber.ToString());
            postData.Add("width", pictureSize.ToString());
            postData.Add("bild", displayPicture ? "1" : "0");
            postData.Add("senden", "ok");
            return postData;
        }

        public static Dictionary<string, string> GetAdminAccountTransfer(string playerName1, string playerName2, bool exchangePassword)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("spieler1", playerName1);
            postData.Add("spieler2", playerName2);
            if (exchangePassword)
                postData.Add("pw", "1");
            postData.Add("senden", "Exchange");
            return postData;
        }

        public static Dictionary<string, string> GetAdminLogins(string search, DateTime startDate, DateTime endDate)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("login", "on");
            postData.Add("user", search);
            postData.Add("login_start_day", startDate.Day.ToString());
            postData.Add("login_start_month", startDate.Month.ToString());
            postData.Add("login_start_year", startDate.Year.ToString());
            postData.Add("login_end_day", endDate.Day.ToString());
            postData.Add("login_end_month", endDate.Month.ToString());
            postData.Add("login_end_year", endDate.Year.ToString());
            return postData;
        }

        public static Dictionary<string, string> GetAdminAlliancesTextSearch(string search)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("text", search);
            return postData;
        }

        public static Dictionary<string, string> GetAdminLogs(string search, int pageNumber)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("such", search);
            postData.Add("side", pageNumber.ToString());
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerAddNewNote(string note)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("notiz", note);
            postData.Add("add", "   Send   ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerChangeNoteStatus(string noteStatus)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("stat", "1");
            postData.Add("own", noteStatus);
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerBan(string playerName, string banReason, int banDuration, bool activateVacationMode)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("spielername", playerName);
            postData.Add("sperrgrund", banReason);
            postData.Add("sperrdauer", banDuration.ToString());
            if (activateVacationMode)
                postData.Add("umod", "1");
            postData.Add("senden", "     Ban     ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerUnBan(string playerName, string unBanReason, bool removeVacationMode)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("spielername", playerName);
            postData.Add("sperrgrund", unBanReason);
            if (removeVacationMode)
                postData.Add("enfumod", "1");
            postData.Add("senden", "   unban   ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerRename(string newPlayerName)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("nameneu", newPlayerName);
            postData.Add("senden", "  rename  ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerChangeEmail(string playerName, string currentEmail, string newEmail)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("spielername", playerName);
            postData.Add("emailalt", currentEmail);
            postData.Add("emailneu", newEmail);
            postData.Add("senden", "     change     ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminPlayerSendMessage(string playerName, string subject, string message, bool addMessageToNote)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("uname", playerName);
            postData.Add("betreff", subject);
            postData.Add("nachricht", message);
            if (addMessageToNote)
                postData.Add("addnote", "addnote");
            postData.Add("senden", "     send     ");
            return postData;
        }

        public static Dictionary<string, string> GetAdminLogsDetails()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("stats", "");
            return postData;
        }

        public static Dictionary<string, string> GetComaToolLogin(string email, string password)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("username", email);
            postData.Add("password", password);
            //postData.Add("save", "on");
            postData.Add("page", "login");
            return postData;
        }

        public static Dictionary<string, string> GetComaAnswerPostData(string answer, string folder)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("answer", answer);
            postData.Add("folder", folder);
            postData.Add("submit", "Submit");
            return postData;
        }
    }
}
