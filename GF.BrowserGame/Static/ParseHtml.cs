using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Utility;
using GF.BrowserGame.Schema.Internal;
using System.Net;

namespace GF.BrowserGame.Static
{
    internal static class ParseHtml
    {
        public static void Foo(string htmlContent, out bool isContentValid)
        {
            isContentValid = IsContentValid(htmlContent);
        }

        public static Int64 GetPlayerIdByPlayerName(string htmlContent, string playerName)
        {
            htmlContent = htmlContent.Replace("&nbsp;", " ");
            Regex objRegEx = new Regex(@"<a\shref=\""kontrolle\.php\?session=.*?&uid=(?<UserId>\d+)\"">(<font\scolor=\""#6A6A77\"">(?<UserName>.*?)</font>.*?|(?<UserName>.*?))<a.*?</a></a><br>");
            MatchCollection matches = objRegEx.Matches(htmlContent);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;

                if (groups["UserName"].Value.ToLower().Equals(playerName.ToLower()))
                    return Int64.Parse(groups["UserId"].Value);
            }

            return 0;
        }

        public static string GetPlayerEmail(string htmlContent)
        {
            int pilloryIndex = htmlContent.IndexOf("<th colspan=\"5\">");
            htmlContent = htmlContent.Substring(0, pilloryIndex);
            Regex objRegEx = new Regex(@"<tr><td>(.)*?</td><td>(?<Email>.*?@.*?)</td></tr>");
            MatchCollection matches = objRegEx.Matches(htmlContent);
            string email = String.Empty;
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                email = groups["Email"].Value.Trim();
            }
            return email;
        }

        public static List<Note> GetPersonalNotes(string htmlContent, string universeId)
        {
            int personalNotesIndex = htmlContent.IndexOf("<table width=\"500\">");
            int generalNotesIndex = htmlContent.LastIndexOf("<table width=\"500\">");
            string personalNotesContent = htmlContent.Substring(personalNotesIndex, generalNotesIndex - personalNotesIndex);

            List<Note> personalNotesList = new List<Note>();

            Regex objRegEx = new Regex(@"<tr><td><a\shref=\""home\.php\?session=.*?&details=(?<NoteId>\d*)\"">(?<Owner>(.|\s)*?)>>\s(?<NotePreview>(.|\s)*?)</a></td></tr>");
            MatchCollection matches = objRegEx.Matches(personalNotesContent);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Note personalNote = new Note();
                personalNote.Id = int.Parse(groups["NoteId"].Value);
                personalNote.UniverseId = universeId;
                personalNote.Author = groups["Owner"].Value;
                personalNote.Preview = groups["NotePreview"].Value;
                personalNotesList.Add(personalNote);
            }

            return personalNotesList;
        }

        public static List<Note> GetGeneralNotes(string htmlContent, string universeId)
        {
            int generalNotesIndex = htmlContent.LastIndexOf("<table width=\"500\">");
            string generalNotesContent = htmlContent.Substring(generalNotesIndex);

            List<Note> generalNotesList = new List<Note>();

            Regex objRegEx = new Regex(@"<tr><td><a\shref=\""home\.php\?session=.*?&details=(?<NoteId>\d*)\"">(?<Owner>(.|\s)*?)>>\s(?<NotePreview>(.|\s)*?)</a></td></tr>");
            MatchCollection matches = objRegEx.Matches(generalNotesContent);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Note generalNote = new Note();
                generalNote.Id = int.Parse(groups["NoteId"].Value);
                generalNote.UniverseId = universeId;
                generalNote.Author = groups["Owner"].Value;
                generalNote.Preview = groups["NotePreview"].Value;
                generalNotesList.Add(generalNote);
            }

            return generalNotesList;
        }

        public static Note GetAdminNoteDetails(string htmlContent, Note note)
        {
            try
            {
                int noteDetailsIndex = htmlContent.IndexOf("<table width=\"500\">");
                htmlContent = htmlContent.Substring(noteDetailsIndex + "<table width=\"500\">".Length);
                int personalNotesIndex = htmlContent.IndexOf("<table width=\"500\">");
                string noteDetailsContent = htmlContent.Substring(0, personalNotesIndex);
                //Liam</td><td style="background-color:#114161; color:#FFFFFF;">Date: Tue Aug 10 15:26:28 2010
                Regex objRegEx = new Regex(@"</td><td style=\""background-color:#114161; color:#FFFFFF;\"">(.)*?: (?<NoteDate>(.|\s)*?)</td></tr><tr><td colspan=2 style=\""background-color:#114161; color:#FFFFFF;\"">(?<NoteDetails>(.|\s)*?)</td></tr>");
                MatchCollection matches = objRegEx.Matches(noteDetailsContent);

                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    note.CreationDateTime = Converter.GetNoteDateTime(groups["NoteDate"].Value);
                    note.Content = groups["NoteDetails"].Value.Replace("<br />", "");
                    return note;
                }
            }
            catch { }
            return note;
        }

        public static List<ATUser> GetUserListFromOperatorSummary(string htmlContent)
        {
            List<ATUser> userList = new List<ATUser>();
            try
            {
                Regex objRegEx = new Regex(@"<tr><th>(?<Nick>(.|\s)*?)<a\shref=\""sendmsg\.php");

                MatchCollection matches = objRegEx.Matches(htmlContent);

                foreach (Match match in matches)
                {
                    ATUser user = new ATUser();
                    GroupCollection groups = match.Groups;
                    user.Nick = groups["Nick"].Value.Replace("&nbsp;", " ");

                    if (!string.IsNullOrEmpty(user.Nick) && !user.Nick.Equals("Legor"))
                        userList.Add(user);
                }
            }
            catch { }
            return userList;
        }

        public static List<ATLog> GetLogs(string htmlContent)
        {
            Regex objRegEx = new Regex(@"<tr><td style=\""background-color:#114161; color:#FFFFFF;\"">(?<Year>\d{4})-(?<Month>\d{2})-(?<Day>\d{2})\s(?<Hour>\d{2}):(?<Min>\d{2}):(?<Sec>\d{2})</td><td style=\""background-color:#114161; color:#FFFFFF;\"">(?<LogMessage>(.|\s)*?)</td></tr>");

            MatchCollection matches = objRegEx.Matches(htmlContent);

            List<ATLog> logList = new List<ATLog>();

            foreach (Match match in matches)
            {
                ATLog log = new ATLog();
                GroupCollection groups = match.Groups;
                //log.sDate = groups["Date"].Value;
                log.Date = new DateTime(int.Parse(groups["Year"].Value), int.Parse(groups["Month"].Value), int.Parse(groups["Day"].Value), int.Parse(groups["Hour"].Value), int.Parse(groups["Min"].Value), int.Parse(groups["Sec"].Value));
                log.Log = groups["LogMessage"].Value.ToLower();
                logList.Add(log);
            }

            return logList;
        }

        public static string GetATClicks(string htmlContent, string nick)
        {
            htmlContent = htmlContent.Replace("&nbsp;", " ");
            Regex objRegEx = new Regex(@"<tr><td>" + nick + "</td><td>(?<ThreeDays>\\d+)</td><td>(?<SevenDays>\\d+)</td><td>(?<ForteenDays>\\d+)</td><td>(?<ThirtyDays>\\d+)</td><td>(?<AllDays>\\d+)</td>");

            MatchCollection matches = objRegEx.Matches(htmlContent);

            string ret = "";

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                ret = groups["ThreeDays"].Value + "|" + groups["SevenDays"].Value + "|" + groups["ForteenDays"].Value + "|" + groups["ThirtyDays"].Value + "|" + groups["AllDays"].Value;
            }

            return ret;
        }

        public static bool IsNoteSuccessfull(string htmlContent)
        {
            Regex objRegEx = new Regex(@"window\.close");
            if (objRegEx.IsMatch(htmlContent))
                return true;
            else
                return false;
        }

        public static string[] GetRawMultiLogs(string htmlContent)
        {
            Regex objRegEx = new Regex(@"(?<Total>\d+)\s:\s(.|\s)*?<br><br>");
            htmlContent = htmlContent.Replace("&nbsp;", " ");
            MatchCollection matches = objRegEx.Matches(htmlContent);

            string[] multiLogs = new string[matches.Count];
            int i = 0;
            foreach (Match match in matches)
            {
                multiLogs[i] = match.Value;
                i++;
            }
            return multiLogs;
        }

        public static List<List<Account>> GetMultiLogsFromRaw(string[] rawMultiLogs)
        {
            List<List<Account>> multiList = new List<List<Account>>();
            for (int i = 0; i < rawMultiLogs.Length; i++)
            {
                Regex objRegEx = new Regex(@"<a\shref=\""kontrolle\.php\?session=.*?&uid=(?<UID>\d+)\"">(<font\scolor=\""#\w{6}\"">)?(?<PlayerName>(.|\s)*?)(</font>)?(<a\shref=\""sendmsg\.php\?session=.*?&uid=\d+\"">\*</a>)?(\s\((?<AccountStatus>(<font\scolor=\""#\w{6}\"">\w</font>)*)\))?(<a\shref=\""#\""\starget=\""test\""\sonClick=\""window\.owndirect\('(.|\s)*?','(.|\s)*?'\);\""></a>)?</a>");

                MatchCollection matches = objRegEx.Matches(rawMultiLogs[i]);
                List<Account> accountList = new List<Account>();
                int countBan = 0;
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    Account account = new Account();

                    account.Uid = int.Parse(groups["UID"].Value);
                    account.Name = groups["PlayerName"].Value;
                    account = GetAccountStatus(groups["AccountStatus"].Value, account);

                    if (account.Banned)
                        countBan++;

                    accountList.Add(account);
                }
                if (countBan != matches.Count)
                    multiList.Add(accountList);
            }
            return multiList;
        }

        public static Account GetAccountStatus(string status, Account account)
        {
            Regex objRegEx = new Regex(@"<font\scolor=\""#\w{6}\"">(?<Status>\w)</font>");
            MatchCollection matches = objRegEx.Matches(status);

            //if (matches.Count == 0)
            //return null;

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;

                switch (groups["Status"].Value)
                {
                    case "i":
                        account.SmallInactive = true;
                        break;
                    case "I":
                        account.BigInactive = true;
                        break;
                    case "u":
                        account.Vmode = true;
                        break;
                    case "g":
                        account.Banned = true;
                        break;
                    case "L":
                        account.DeletionMode = true;
                        break;
                    default:
                        break;
                }
            }
            return account;
        }

        public static List<IpLog> GetLoginsIPListForMulti(string htmlContent, string nick)
        {
            List<IpLog> ipLogList = new List<IpLog>();

            htmlContent = htmlContent.Replace("&nbsp;", " ");
            Regex objRegEx = new Regex(@"(?<Date>\d{2}-\d{2}\s*\d{2}:\d{2}:\d{2}):\s*(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)(?<UserName>.*?)<br\s>");
            MatchCollection matches = objRegEx.Matches(htmlContent);

            //Regex Error = new Regex("<tr><td>input\\serror</td></tr>");

            int count = 1;
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;

                IpLog ipLog = new IpLog();
                ipLog.Id = count;
                ipLog.IpString = groups["First"].Value + "." + groups["Second"].Value + "." + groups["Third"].Value + "." + groups["Fourth"].Value;
                ipLog.IpAddress = IPAddress.Parse(ipLog.IpString);
                ipLog.Date = groups["Date"].Value.Trim();
                if (!nick.Equals(groups["UserName"].Value.Trim()))
                {
                    ipLog.Nick = groups["UserName"].Value.Trim() + " (" + nick + ")";//UserName
                }
                else
                    ipLog.Nick = groups["UserName"].Value.Trim();//UserName

                ipLog.DateTime = ConvertToDatetimeIPLog(ipLog.Date);

                ipLogList.Add(ipLog);
                count++;
            }
            return ipLogList;
        }

        public static bool IsLoggedInComaTool(string htmlContent)
        {
            if (htmlContent.Contains("index.php?my=logout") && !htmlContent.Contains("index.php?page=login"))
                return true;
            else
                return false;
        }

        public static List<string> GetComaToolOGameCommunityList(string htmlContent)
        {
            List<string> communityList = new List<string>();

            Regex objRegEx = new Regex(@"<option value='(?<CommunityId>\d+)'( selected)?>(?<CommunityName>.*?)</option>");
            MatchCollection matches = objRegEx.Matches(htmlContent);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;

                if (groups["CommunityName"].Value.ToLower().Contains("ogame"))
                {
                    communityList.Add(groups["CommunityId"].Value + "|" + groups["CommunityName"].Value);
                }
            }

            return communityList;
        }

        public static int GetDateFormatId(string htmlContent)
        {
            Regex objRegEx = new Regex(@"<select name='my_date'>(?<DateFormat>(\s|.)*?)</select>");

            MatchCollection matches = objRegEx.Matches(htmlContent);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string dateFormat = groups["DateFormat"].Value;

                Regex objRegEx2 = new Regex(@"<option value='(?<DateFormatId>\d)' selected>");
                MatchCollection matches2 = objRegEx2.Matches(dateFormat);
                foreach (Match match2 in matches2)
                {
                    GroupCollection groups2 = match2.Groups;
                    return int.Parse(groups2["DateFormatId"].Value.Trim());
                }
            }

            return 0;
        }

        public static int GetTotalMyTicket(string htmlContent)
        {
            //index.php?page=tickets&action=gom'><img src='style/images/ticket_icon.gif'>My Tickets (4)</a></li>
            Regex objRegEx = new Regex(@"page=tickets&action=gom'><img src='style/images/ticket_icon\.gif'>.*?\((?<Total>\d+)\)</a></li>");

            if (objRegEx.IsMatch(htmlContent))
            {
                Match match = objRegEx.Match(htmlContent);
                GroupCollection groups = match.Groups;
                return int.Parse(groups["Total"].Value.Trim());
            }
            else
            {
                return 0;
            }
        }

        public static int GetTotalOpenTicket(string htmlContent)
        {
            //index.php?page=tickets&action=gom'><img src='style/images/ticket_icon.gif'>My Tickets (4)</a></li>
            Regex objRegEx = new Regex(@"page=tickets&action=go'><img src='style/images/ticket\.gif'>.*?\((?<Total>\d+)\)</a></li>");

            if (objRegEx.IsMatch(htmlContent))
            {
                Match match = objRegEx.Match(htmlContent);
                GroupCollection groups = match.Groups;
                return int.Parse(groups["Total"].Value.Trim());
            }
            else
            {
                return 0;
            }
        }

        public static List<TicketGUI> GetTicketList(string htmlContent)
        {
            //Regex objRegEx = new Regex(@"<tr><td\sclass='text'(.|\s)*?<a\shref='index\.php\?page=answer&action=view&id=(?<TicketId>\d+)&value=(?<TicketValue>.*?)'>(?<TicketSubject>(.|\s)*?)</a></td><td(.|\s)*?>(?<Server>\d+)</td><td(.|\s)*?>(?<NickName>(.|\s)*?)</td>(.|\s)*?<td(.|\s)*?>(?<StaffNickName>(.|\s)*?)</td>(.|\s)*?<td(.|\s)*?>(?<ReplyNumber>\d+)</td>(.|\s)*?<td(.|\s)*?>(?<TicketDate>(.|\s)*?)</td>(.|\s)*?</tr>");
            Regex objRegEx = new Regex(@"<tr><td class='text' (bgcolor='#E9E9E9'|bgcolor='cyan')? width='\d*'>((&nbsp;)*|<img src='style/images/attachments\.gif' width='\d*' height='\d*'>)</td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'><a\shref='index\.php\?page=answer&action=view&id=(?<TicketId>\d+)&value=(?<TicketValue>.*?)'.*?>(?<TicketSubject>(.|\s)*?)</a>(.|\s)*?</td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(?<Server>\d+)</td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(.|\s)*?</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(?<NickName>(.|\s)*?)</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(?<StaffNickName>(.|\s)*?)</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(.|\s)*?</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text' align='center'>(?<ReplyNumber>\d+)</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text' align='center'><font color='grey'>(?<TicketDate>(.|\s)*?)</font></td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text' align='center'>(?<TicketDate2>[^<>]*?)</td>(.|\s)*?</tr>");
            //Regex objRegEx = new Regex(@"<tr><td class='text' (bgcolor='#E9E9E9'|bgcolor='cyan')? width='\d*'>((&nbsp;)*|<img src='style/images/attachments\.gif' width='\d*' height='\d*'>)</td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'><a\shref='index\.php\?page=answer&action=view&id=(?<TicketId>\d+)&value=(?<TicketValue>.*?)'.*?>(?<TicketSubject>(.|\s)*?)</a>(.|\s)*?</td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(?<Server>\d+)</td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(.|\s)*?</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(?<NickName>(.|\s)*?)</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(?<StaffNickName>(.|\s)*?)</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text'>(.|\s)*?</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text' align='center'>(?<ReplyNumber>\d+)</td>(.|\s)*?<td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text' align='center'><font color='grey'>(?<TicketDate>(.|\s)*?)</font></td><td (bgcolor='#E9E9E9'|bgcolor='cyan')? class='text' align='center'>(?<TicketDate2>(.|\s)*?)</td>(.|\s)*?</tr>");

            Regex objRegEx2 = new Regex(@"index\.php\?page=answer&action=view&id=");

            List<TicketGUI> ticketList = new List<TicketGUI>();

            if (objRegEx2.IsMatch(htmlContent))
            {
                MatchCollection matches = objRegEx.Matches(htmlContent);

                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    TicketGUI ticket = new TicketGUI();
                    ticket.TicketId = int.Parse(groups["TicketId"].Value);
                    ticket.TicketValue = groups["TicketValue"].Value;
                    ticket.Subject = groups["TicketSubject"].Value;
                    if (groups["Server"].Value.Equals("00"))
                    { ticket.Server = 0; }
                    else
                    {
                        ticket.Server = int.Parse(groups["Server"].Value);
                    }
                    ticket.NickName = groups["NickName"].Value;
                    ticket.StaffNickName = groups["StaffNickName"].Value;
                    ticket.OpenedTickets = int.Parse(groups["ReplyNumber"].Value);
                    ticket.DateString = groups["TicketDate"].Value;
                    ticket.DateString2 = groups["TicketDate2"].Value;
                    ticketList.Add(ticket);
                }
                return ticketList;
            }
            else
            {
                return ticketList;
            }
        }

        public static int GetCurrentTicketPageNr(string htmlContent)
        {
            Regex objRegEx = new Regex(@"<font color='white'>(?<CurrentPage>\d+)</font>");
            if (objRegEx.IsMatch(htmlContent))
            {
                Match match = objRegEx.Match(htmlContent);
                GroupCollection groups = match.Groups;
                int nr = int.Parse(groups["CurrentPage"].Value.Trim());
                if (nr == 0)
                    nr = 1;
                return nr;
            }
            else
            {
                return 1;
            }
        }

        public static int GetTotalTicketPageNr(string htmlContent)
        {
            //<a href='index.php?page=tickets&order=datum&action=gc&offset=19' style='text-decoration:none;'>&raquo;</a>
            Regex objRegEx = new Regex(@"&offset=(?<TotalPage>\d+)'\sstyle='text-decoration:none;'>&raquo;</a>");
            if (objRegEx.IsMatch(htmlContent))
            {
                Match match = objRegEx.Match(htmlContent);
                GroupCollection groups = match.Groups;
                int nr = int.Parse(groups["TotalPage"].Value.Trim());
                if (nr == 0)
                    nr = 1;
                return nr;
            }
            else
            {
                return 1;
            }
        }

        public static Ticket GetTicketDetails(string htmlContent, Ticket ticket)
        {
            MatchCollection matches;

            Regex emailRegEx;
            if (!htmlContent.Contains("onclick=\"showUser"))
                emailRegEx = new Regex(@"<td\sclass='text'><b>.*?</b></td>\r\n\s*<td class='text'>(?<Email>.*?@.*?)</td>");
            else
                emailRegEx = new Regex(@"<font color='\w+'><b>(?<Email>.*?@.*?)</b></font>");
                //emailRegEx = new Regex("onclick=\"showUser('(?<Email>.*?@.*?)','result',)");

            //Regex createdOnRegEx = new Regex(@"<td\sclass='text'><b>Created\son</b></td>\r\n\s*<td class='text'>(?<CreatedOn>.*?)</td>");
            //Regex lastReplyRegEx = new Regex(@"<td\sclass='text'><b>Last\sreply</b></td>\r\n\s*<td class='text'>(?<LastReply>.*?)</td>");
            Regex ipAddressRegEx = new Regex(@"<td\sclass='text'><b>.*?</b></td>\r\n\s*<td class='text'>(?<IpAddress>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})</td>");

            matches = emailRegEx.Matches(htmlContent);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                ticket.Email = groups["Email"].Value.Trim();
            }

            //matches = createdOnRegEx.Matches(html);
            //foreach (Match match in matches)
            //{
            //    GroupCollection groups = match.Groups;
            //    ticket.CreatedOn = ConvertToDatetimeTicket(groups["CreatedOn"].Value.Trim());
            //}

            //matches = lastReplyRegEx.Matches(html);
            //foreach (Match match in matches)
            //{
            //    GroupCollection groups = match.Groups;
            //    ticket.LastReply = ConvertToDatetimeTicket(groups["LastReply"].Value.Trim());
            //}

            matches = ipAddressRegEx.Matches(htmlContent);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                ticket.IpAddress = groups["IpAddress"].Value.Trim();
            }

            return ticket;
        }

        public static List<TicketMessage> GetTicketMessages(string htmlContent, int dateFormatId)
        {
            MatchCollection matches;
            htmlContent = htmlContent.Replace("&nbsp;", "");
            Regex obRegEx = new Regex(@"<tr>\r\n\s*<td\sclass='head'\swidth='15'\salign='center'><img\ssrc='style/images/ip\.gif'\swidth='12'\sheight='11'\stitle='(?<IpAddress>.*?)'></td>\r\n\s*<td\sclass='head'\scolspan='2'><b>(?<Nick>.*?)</b></td>\r\n\s*<td\sclass='head'\salign='right'\swidth='150'>(?<DateTime>.*?)</td>\r\n\s*</tr>\r\n\s*(<tr>\r\n\s*<td\sclass='text'></td>\r\n\s*<td\sclass='text'><b>.*?</b></td>\r\n\s*<td\sclass='text'\scolspan='2'>.*?</td>\r\n\s*</tr>\r\n\s*<tr><td\sclass='text'\scolspan='4'></td></tr>\r\n\s*)?<tr>\r\n\s*<td\sclass='text'></td>\r\n\s*<td\sclass='text'\svalign='top'\swidth='150'><b>.*?</b></td>\r\n\s*<td\sclass='text'\scolspan='2'><div\sstyle='width:640px;'>(?<Message>.*?)</div></td>\r\n\s*</tr>");
            //Regex obRegEx = new Regex(@"<tr>\r\n\s*<td\sclass='head'\swidth='15'\salign='center'><img\ssrc='style/images/ip\.gif'\swidth='12'\sheight='11'\stitle='(?<IpAddress>.*?)'></td>\r\n\s*<td\sclass='head'\scolspan='2'><b>(?<Nick>.*?)</b></td>\r\n\s*<td\sclass='head'\salign='right'\swidth='150'>(?<DateTime>\d{4}-\d{2}-\d{2},?\s*\d{2}:\d{2})</td>\r\n\s*</tr>\r\n\s*(<tr>\r\n\s*<td\sclass='text'></td>\r\n\s*<td\sclass='text'><b>Subject</b></td>\r\n\s*<td\sclass='text'\scolspan='2'>.*?</td>\r\n\s*</tr>\r\n\s*<tr><td\sclass='text'\scolspan='4'></td></tr>\r\n\s*)?<tr>\r\n\s*<td\sclass='text'></td>\r\n\s*<td\sclass='text'\svalign='top'\swidth='150'><b>Message</b></td>\r\n\s*<td\sclass='text'\scolspan='2'><div\sstyle='width:640px;'>(?<Message>.*?)</div></td>\r\n\s*</tr>");
            matches = obRegEx.Matches(htmlContent);
            List<TicketMessage> ticketMessageList = new List<TicketMessage>();
            foreach (Match match in matches)
            {
                TicketMessage message = new TicketMessage();
                GroupCollection groups = match.Groups;
                message.IpAddress = groups["IpAddress"].Value.Trim();
                message.NickName = groups["Nick"].Value.Trim();
                message.Message = ReplaceHTMLEntities(groups["Message"].Value.Trim());
                message.MessageDateTime = ConvertToDatetimeTicket(dateFormatId, groups["DateTime"].Value.Trim());
                ticketMessageList.Add(message);
            }

            return ticketMessageList;
        }

        public static List<AnswerTemplate> GetAnswerTemplateDetails(string htmlContent)
        {
            MatchCollection matches;
            Regex obRegEx = new Regex(@"<a\shref='index\.php\?page=answer&action=answer&id=\d+?&link=(?<Link>\d+?)&value=.{33}'>(?<AnswerTitle>.*?)</a>");
            matches = obRegEx.Matches(htmlContent);
            List<AnswerTemplate> answerTemplateList = new List<AnswerTemplate>();
            foreach (Match match in matches)
            {
                AnswerTemplate template = new AnswerTemplate();
                GroupCollection groups = match.Groups;
                template.Link = groups["Link"].Value.Trim();
                template.Title = groups["AnswerTitle"].Value.Trim();
                answerTemplateList.Add(template);
            }

            return answerTemplateList;
        }

        public static string GetTicketAnswerTemplate(string htmlContent)
        {
            MatchCollection matches;
            htmlContent = htmlContent.Replace("&nbsp;", " ");
            Regex obRegEx = new Regex(@"id='answer'\sname='answer'\srows='8'\scols='138'>(?<Answer>(.|\s|\r|\n)*?)</textarea>");
            matches = obRegEx.Matches(htmlContent);
            string result = String.Empty;
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                result = ReplaceHTMLEntities(groups["Answer"].Value.Trim());
            }

            return result;
        }

        public static bool IsTicketAnswerSuccessfull(string htmlContent)
        {
            Regex objRegEx = new Regex(@"<td class='text'><a href='index\.php\?page=tickets&action=");
            if (objRegEx.IsMatch(htmlContent))
                return true;
            else
                return false;
        }

        public static string ReplaceHTMLEntities(string text)
        {
            text = text.Replace("&quot;", "\"");
            text = text.Replace("&amp;", "&");
            text = text.Replace("&lt;", "<");
            text = text.Replace("&gt;", ">");
            text = text.Replace("&circ;", "^");
            text = text.Replace("&tilde;", "~");
            text = text.Replace("&ndash;", "–");
            text = text.Replace("&mdash;", "—");
            text = text.Replace("&lsquo;", "‘");
            text = text.Replace("&rsquo;", "’");
            text = text.Replace("&#039;", "'");
            return text;
        }

        public static DateTime ConvertToDatetimeTicket(int dateFormatId, string date)
        {
            try
            {
                Regex objRegEx = new Regex(GetDateFormatRegex(dateFormatId));
                if (objRegEx.IsMatch(date))
                {
                    Match match = objRegEx.Match(date);
                    GroupCollection groups = match.Groups;
                    string amPm = groups["AmPm"].Value;

                    if (String.IsNullOrEmpty(amPm))
                    {
                        return new DateTime(int.Parse(groups["Year"].Value), int.Parse(groups["Month"].Value), int.Parse(groups["Day"].Value), int.Parse(groups["Hour"].Value), int.Parse(groups["Min"].Value), 0);
                    }
                    else
                    {
                        if (amPm.Equals("am"))
                        {
                            return new DateTime(int.Parse(groups["Year"].Value), int.Parse(groups["Month"].Value), int.Parse(groups["Day"].Value), int.Parse(groups["Hour"].Value), int.Parse(groups["Min"].Value), 0);
                        }
                        else
                        {
                            int hour = int.Parse(groups["Hour"].Value);
                            switch (hour)
                            {
                                case 1:
                                    hour = 13;
                                    break;
                                case 2:
                                    hour = 14;
                                    break;
                                case 3:
                                    hour = 15;
                                    break;
                                case 4:
                                    hour = 16;
                                    break;
                                case 5:
                                    hour = 17;
                                    break;
                                case 6:
                                    hour = 18;
                                    break;
                                case 7:
                                    hour = 19;
                                    break;
                                case 8:
                                    hour = 20;
                                    break;
                                case 9:
                                    hour = 21;
                                    break;
                                case 10:
                                    hour = 22;
                                    break;
                                case 11:
                                    hour = 23;
                                    break;
                                case 12:
                                    hour = 00;
                                    break;
                            }
                            return new DateTime(int.Parse(groups["Year"].Value), int.Parse(groups["Month"].Value), int.Parse(groups["Day"].Value), hour, int.Parse(groups["Min"].Value), 0);
                        }
                    }
                }
                else
                    return DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ConvertToDatetimeIPLog(string date)
        {
            try
            {
                //01-17 02:41:52
                Regex objRegEx = new Regex(@"(?<Month>\d{2})-(?<Day>\d{2})\s*(?<Hour>\d{2}):(?<Min>\d{2}):(?<Sec>\d{2})");
                DateTime now = DateTime.Today;
                if (objRegEx.IsMatch(date))
                {
                    Match match = objRegEx.Match(date);
                    GroupCollection groups = match.Groups;
                    int year = 2000;
                    if (int.Parse(groups["Month"].Value) > now.Month)
                    {
                        year = now.Year - 1;
                    }
                    else
                        year = now.Year;

                    return new DateTime(year, int.Parse(groups["Month"].Value), int.Parse(groups["Day"].Value), int.Parse(groups["Hour"].Value), int.Parse(groups["Min"].Value), int.Parse(groups["Sec"].Value));
                }
                else
                    return now;
            }
            catch
            {
                return DateTime.Today;
            }
        }

        public static string GetDateFormatRegex(int dateFormatId)
        {
            switch (dateFormatId)
            {
                case 1:
                    // 24/3/2012, 1:07 am
                    return @"(?<Day>\d{1,2})/(?<Month>\d{1,2})/(?<Year>\d{4}), (?<Hour>\d{1,2}):(?<Min>\d{2}) (?<AmPm>(am|pm))";
                case 2:
                    // 24/3/2012, 01:07
                    return @"(?<Day>\d{1,2})/(?<Month>\d{1,2})/(?<Year>\d{4}), (?<Hour>\d{1,2}):(?<Min>\d{2})(\s)?(?<AmPm>(am|pm)?)";
                case 3:
                    // 3/24/2012, 1:07 am
                    return @"(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>\d{4}), (?<Hour>\d{1,2}):(?<Min>\d{2}) (?<AmPm>(am|pm))";
                case 4:
                    // 3/24/2012, 01:07
                    return @"(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>\d{4}), (?<Hour>\d{1,2}):(?<Min>\d{2})(\s)?(?<AmPm>(am|pm)?)";
                case 5:
                    // 2012-03-24, 1:07 am
                    return @"(?<Year>\d{4})-(?<Month>\d{1,2})-(?<Day>\d{1,2}), (?<Hour>\d{1,2}):(?<Min>\d{2}) (?<AmPm>(am|pm))";
                case 6:
                    // 2012-03-24, 01:07
                    return @"(?<Year>\d{4})-(?<Month>\d{1,2})-(?<Day>\d{1,2}), (?<Hour>\d{1,2}):(?<Min>\d{2})(\s)?(?<AmPm>(am|pm)?)";
                case 7:
                    // 24.03.2012, 1:07 am
                    return @"(?<Day>\d{1,2})\.(?<Month>\d{1,2})\.(?<Year>\d{4}), (?<Hour>\d{1,2}):(?<Min>\d{2}) (?<AmPm>(am|pm))";
                case 8:
                    // 24.03.2012, 01:07
                    return @"(?<Day>\d{1,2})\.(?<Month>\d{1,2})\.(?<Year>\d{4}), (?<Hour>\d{1,2}):(?<Min>\d{2})(\s)?(?<AmPm>(am|pm)?)";
                default:
                    return @"do-not-match";
            }
        }

        public static bool IsContentValid(string htmlContent)
        {
            bool bReturn = false;

            if (htmlContent.StartsWith(Constants.SESSION_INVALID))
                bReturn = false;
            else
                bReturn = true;

            return bReturn;
        }
    }
}
