using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OGameOneAdmin.Object;
using GF.BrowserGame;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;
using GF.BrowserGame.Forms;
using GF.BrowserGame.Schema.Internal;
using LibCommonUtil;
using System.IO;

namespace OGameOneAdmin.Controls
{
    public partial class ATLogsViewerControl : UserControl
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private object _locker = new object();
        private GameManager _gameManager;
        private List<string> _universeIdList = new List<string>();
        private List<ATUser> _userList = new List<ATUser>();
        private int _backgroundWorkerRunning = 0;
        private bool _isInProgressBarVisible = false;
        private bool _quitProcess = false;

        private string _outPut = AppDomain.CurrentDomain.BaseDirectory;
        private string _fileName;
        private string _ticketStatsFileName = AppDomain.CurrentDomain.BaseDirectory + "ticket-stats.txt";
        private string _atStats = "";
        private string _ticketStats = "";
        private string statusTemplate = "Retrieving page #{0} / Logs found #{1} / Logs parsed #{2}";


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public ATLogsViewerControl(GameManager gameManager)
        {
            InitializeComponent();
            _gameManager = gameManager;
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Events ------


        private void comboBoxCommunityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCommunityList.SelectedIndex == -1)
                {
                    comboBoxCommunityList.SelectedIndex = 0;
                    return;
                }

                if (comboBoxCommunityList.SelectedIndex == 0)
                {
                    comboBoxUniverseList.Items.Clear();

                    comboBoxUniverseList.Items.Add("-- Select --");

                    foreach (Universe universe in _gameManager.UniverseList)
                    {
                        if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_STATS, universe.Id))
                            comboBoxUniverseList.Items.Add(new ItemObject(universe.Domain, universe.Id));
                    }

                    if (comboBoxUniverseList.Items.Count == 2)
                    {
                        comboBoxUniverseList.Enabled = false;
                        comboBoxUniverseList.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBoxUniverseList.Enabled = true;
                        comboBoxUniverseList.SelectedIndex = 0;
                    }
                }
                else
                {
                    ItemObject communityObj = comboBoxCommunityList.SelectedItem as ItemObject;
                    string communityName = communityObj.Key;
                    string communityId = communityObj.ValueOfKey.ToString();

                    comboBoxUniverseList.Items.Clear();
                    comboBoxUniverseList.Items.Add("-- Select --");

                    int count = 0;
                    foreach (Universe universe in _gameManager.UniverseList)
                    {
                        if (universe.CommunityId.Equals(communityId) && _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_STATS, universe.Id))
                        {
                            comboBoxUniverseList.Items.Add(new ItemObject(universe.Domain, universe.Id));
                            count++;
                        }
                    }

                    if (count == 2)
                    {
                        comboBoxUniverseList.Enabled = false;
                        comboBoxUniverseList.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBoxUniverseList.Enabled = true;
                        comboBoxUniverseList.SelectedIndex = 0;
                    }
                }
            }
            catch { }
        }


        private void comboBoxUniverseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxUniverseList.SelectedIndex == -1)
                {
                    comboBoxUniverseList.SelectedIndex = 0;
                    return;
                }

                if (comboBoxUniverseList.SelectedIndex == 0)
                {

                }
                else
                {
                    _universeIdList.Clear();
                    ItemObject universeObj = comboBoxUniverseList.SelectedItem as ItemObject;
                    string universeId = universeObj.ValueOfKey.ToString();
                    _universeIdList.Add(universeId);
                    RetrieveGOList(universeId);
                }
            }
            catch { }
        }


        private void btnMenu_Click(object sender, EventArgs e)
        {
            //contextMenuStrip1.Show(btnMenu, Point.Empty);
            contextMenuStrip1.Show(btnMenu, btnMenu.Bounds.Left - 7, btnMenu.Bounds.Bottom - 8);
        }


        private void contextMenuStrip1_MouseLeave(object sender, EventArgs e)
        {
            contextMenuStrip1.Hide();
        }


        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.CompareTo(dateTimePickerTo.Value) > 0)
            {
                DisplayError("Error, date from must be prior or equal date to.");
                return;
            }

            if (textBox1.Text.Trim().Length == 0)
            {
                DisplayError("Error, you must input a nick name.");
                return;
            }

            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }

            btnGenerate.Enabled = false;
            btnCancel.Enabled = true;
            _quitProcess = false;
            contextMenuStrip1.Enabled = false;
            comboBoxCommunityList.Enabled = false;
            comboBoxUniverseList.Enabled = false;
            listBoxGO.Enabled = false;
            txbResult2.Text = "";
            

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                GenerateStats();
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                contextMenuStrip1.Enabled = true;
                comboBoxCommunityList.Enabled = true;
                comboBoxUniverseList.Enabled = true;
                listBoxGO.Enabled = true;
            };
            worker.RunWorkerAsync();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            _quitProcess = true;
        }


        private void listBoxGO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGO.SelectedItem != null)
                textBox1.Text = listBoxGO.SelectedItem.ToString();
        }


        private void retrieveOperatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }
            RetrieveGOList(_universeIdList.FirstOrDefault());
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool found = false;
                string name = "";
                foreach (string nick in listBoxGO.Items)
                {
                    if (textBox1.Text.Trim().ToLower().Equals(nick.Trim().ToLower()))
                    {
                        found = true;
                        name = nick;
                    }
                }
                if (found)
                    listBoxGO.SelectedItem = name;
                else
                    listBoxGO.SelectedItem = null;
            }
            catch { }
        }


        #endregion ----- Events ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        public void LoadControl()
        {
            SetupComboBox();

            DateTime today = DateTime.Now;
            if (today.Day < 20)
            {
                if (today.Month == 1)
                {
                    dateTimePickerFrom.Value = new DateTime(today.Year - 1, 12, 1);
                    dateTimePickerTo.Value = new DateTime(today.Year, today.Month, 1).AddDays(-1);
                }
                else
                {
                    dateTimePickerFrom.Value = new DateTime(today.Year, today.Month - 1, 1);
                    dateTimePickerTo.Value = new DateTime(today.Year, today.Month, 1).AddDays(-1);
                }
            }
            else
            {
                dateTimePickerFrom.Value = new DateTime(today.Year, today.Month, 1);
                dateTimePickerTo.Value = DateTime.Now;
            }
        }


        public void ApplyFocus()
        {
        }


        #endregion ----- Public Methods ------


        /***********************************************************************************************************/


        #region ----- Internal Methods ------


        #endregion ----- Internal Methods ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private void SetupComboBox()
        {
            comboBoxCommunityList.Items.Add("-- All --");

            foreach (Community community in _gameManager.GetCommunityList())
            {
                if (_gameManager.HasCommunitySecureObject(Constants.SecureObject.VIEW_TAB_STATS, community.Id))
                    comboBoxCommunityList.Items.Add(new ItemObject(community.Name, community.Id));
            }

            comboBoxUniverseList.Items.Add("-- Select --");

            foreach (Universe universe in _gameManager.UniverseList)
            {
                if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_STATS, universe.Id))
                    comboBoxUniverseList.Items.Add(new ItemObject(universe.Domain, universe.Id));
            }

            if (comboBoxUniverseList.Items.Count == 2)
            {
                comboBoxUniverseList.Enabled = false;
            }


            if (comboBoxCommunityList.Items.Count > 2)
            {
                comboBoxCommunityList.Enabled = true;
                comboBoxCommunityList.SelectedIndex = 0;
            }
            else
            {
                comboBoxCommunityList.Enabled = false;
                comboBoxCommunityList.SelectedIndex = 1;
            }
        }


        private void ShowHideInProgressBar()
        {
            lock (_locker)
            {
                if (_backgroundWorkerRunning == 0 && _isInProgressBarVisible)
                {
                    Invoke(new MethodInvoker(() => pictureBoxInProgress.Visible = _isInProgressBarVisible = false));
                }
                else if (_backgroundWorkerRunning > 0 && !_isInProgressBarVisible)
                {
                    Invoke(new MethodInvoker(() => pictureBoxInProgress.Visible = _isInProgressBarVisible = true));
                }
            }
        }


        private static void DisplayError(String Error)
        {
            MessageBox.Show(Error, "Error", MessageBoxButtons.OK);
        }


        private void RetrieveGOList(string universeId)
        {
            listBoxGO.Items.Clear();
            listBoxGO.Items.Add("Retrieving...");

            string errorMessage = "";

            using (var waitingForm = new WaitingForm())
            {
                var worker = new BackgroundWorker();

                #region worker.DoWork
                worker.DoWork += (sender, e) =>
                {
                    _userList = _gameManager.GetUserListFromOperatorSummary(universeId);
                };
                #endregion worker.DoWork

                #region worker.RunWorkerCompleted
                worker.RunWorkerCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        // Do something
                    }
                    waitingForm.SetShowHide(false);
                };
                #endregion worker.RunWorkerCompleted

                worker.RunWorkerAsync();

                waitingForm.SetShowHide(true);
                DialogResult result = waitingForm.ShowDialog();

                if (!string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show(errorMessage, "Information", MessageBoxButtons.OK);

                if (_userList == null)
                {
                    _userList = new List<ATUser>();
                    listBoxGO.Items.Clear();
                    return;
                }

                if (_userList.Count == 0)
                {
                    MessageBox.Show("No operator was found however you may try to input a nickname manually!", "Information", MessageBoxButtons.OK);
                }

                listBoxGO.Items.Clear();

                foreach (ATUser user in _userList)
                {
                    listBoxGO.Items.Add(user.Nick);
                }

                if (_userList.Count > 0)
                {
                    textBox1.Text = _userList.First().Nick;
                    listBoxGO.SelectedIndex = 0;
                }

                listBoxGO.SelectionMode = SelectionMode.One;
            }
        }


        private void GenerateStats()
        {
            DateTime checkFrom = dateTimePickerFrom.Value;
            DateTime checkTo = dateTimePickerTo.Value;
            _atStats = "";
            _ticketStats = "";

            string such = textBox1.Text.Trim();
            int pageNrb = 1;

            int countTotal = 0;
            int countOverview = 0;
            int countFleetLog = 0;
            int countLoginLog = 0;
            int countCookieLog = 0;
            int countAlliance = 0;
            int countBan = 0;
            int countUnban = 0;
            int countReportedMessage = 0;
            int countIpCheck = 0;
            int countLogOff = 0;
            int countEmailChanged = 0;
            int countPasswordReset = 0;

            List<ATLog> logList = GetLogsFilterByDateAndSuch(checkFrom, checkTo, such, pageNrb);

            //ORG
            countOverview += logList.Count(r => r.Log.Contains("checked |") && !r.Log.Contains("(+"));
            countFleetLog += logList.Count(r => r.Log.Contains("checks the fleet log"));
            countLoginLog += logList.Count(r => r.Log.Contains("checked the login logs"));
            countAlliance += logList.Count(r => r.Log.Contains("checked |") && r.Log.Contains("(+"));
            countReportedMessage += logList.Count(r => r.Log.Contains("takes over ticket"));
            countBan += logList.Count(r => r.Log.Contains(": banned |"));
            countUnban += logList.Count(r => r.Log.Contains(": unbanned |"));
            countEmailChanged += logList.Count(r => r.Log.Contains("mail for"));
            countIpCheck += logList.Count(r => r.Log.Contains("ip check changed"));
            countLogOff += logList.Count(r => r.Log.Contains("logs off"));
            countPasswordReset += logList.Count(r => r.Log.Contains("password reset"));

            //US
            countOverview += logList.Count(r => r.Log.Contains("looks into:") && !r.Log.Contains("(+"));
            //countFleetLog += logList.Count(r => r.Log.Contains("checks the fleet log"));
            //countLoginLog += logList.Count(r => r.Log.Contains("checked the login logs"));
            countAlliance += logList.Count(r => r.Log.Contains("looks into:") && r.Log.Contains("(+"));
            //countReportedMessage += logList.Count(r => r.Log.Contains("takes over ticket"));
            //countBan += logList.Count(r => r.Log.Contains(": banned |"));
            //countUnban += logList.Count(r => r.Log.Contains(": unbanned |"));
            //countEmailChanged += logList.Count(r => r.Log.Contains("mail for"));
            //countIpCheck += logList.Count(r => r.Log.Contains("ip check changed"));
            //countLogOff += logList.Count(r => r.Log.Contains("logs off"));
            //countPasswordReset += logList.Count(r => r.Log.Contains("password reset"));

            //COM.PT
            countOverview += logList.Count(r => r.Log.Contains("verifica (") && !r.Log.Contains("(+"));
            countFleetLog += logList.Count(r => r.Log.Contains("verifica os registos de frota"));
            countLoginLog += logList.Count(r => r.Log.Contains("consulta os registos de login de"));
            countAlliance += logList.Count(r => r.Log.Contains("verifica (") && r.Log.Contains("(+"));
            countReportedMessage += logList.Count(r => r.Log.Contains("denúncia"));
            countBan += logList.Count(r => r.Log.Contains(": banido"));
            countUnban += logList.Count(r => r.Log.Contains(": desbanido"));
            countEmailChanged += logList.Count(r => r.Log.Contains("email alterado"));
            countIpCheck += logList.Count(r => r.Log.Contains("altera verificação de ip"));
            countLogOff += logList.Count(r => r.Log.Contains("desconexão"));
            countPasswordReset += logList.Count(r => r.Log.Contains("senha restaurada"));

            //FR
            countOverview += logList.Count(r => r.Log.Contains("consulte |") && !r.Log.Contains("(+"));
            countFleetLog += logList.Count(r => r.Log.Contains("consulte le log de flottes de"));
            countLoginLog += logList.Count(r => r.Log.Contains("consulte les logs de logins de"));
            countAlliance += logList.Count(r => r.Log.Contains("consulte |") && r.Log.Contains("(+"));
            if (countReportedMessage == 0)
            countReportedMessage += logList.Count(r => r.Log.Contains("takes over ticket"));
            if (countBan == 0)
            countBan += logList.Count(r => r.Log.Contains(": banned |"));
            if (countUnban == 0)
            countUnban += logList.Count(r => r.Log.Contains(": unbanned |"));
            countEmailChanged += logList.Count(r => r.Log.Contains("modif adresse mail"));
            countIpCheck += logList.Count(r => r.Log.Contains("modif statut verif ip"));
            countLogOff += logList.Count(r => r.Log.Contains("a déloggé le joueur"));
            countPasswordReset += logList.Count(r => r.Log.Contains("reset mot de passe"));

            //DE
            countOverview += logList.Count(r => r.Log.Contains("nimmt einsicht |") && !r.Log.Contains("(+"));
            countFleetLog += logList.Count(r => r.Log.Contains("überprüft den flottenlog"));
            countLoginLog += logList.Count(r => r.Log.Contains("betrachtet die loginlogs von"));
            countAlliance += logList.Count(r => r.Log.Contains("nimmt einsicht |") && r.Log.Contains("(+"));
            if (countReportedMessage == 0)
                countReportedMessage += logList.Count(r => r.Log.Contains("takes over ticket"));
            if (countBan == 0)
                countBan += logList.Count(r => r.Log.Contains(": banned |"));
            if (countUnban == 0)
                countUnban += logList.Count(r => r.Log.Contains(": unbanned |"));
            countEmailChanged += logList.Count(r => r.Log.Contains("email geändert"));
            countIpCheck += logList.Count(r => r.Log.Contains("ip check geändert"));
            countLogOff += logList.Count(r => r.Log.Contains("loggt einen spieler aus"));
            countPasswordReset += logList.Count(r => r.Log.Contains("passwort reset"));


            string space = "";
            string stats = countOverview + " overview checks.\n";
            stats += countFleetLog + " fleetlog checks.\n";
            stats += countLoginLog + " cookie / logins checks.\n";
            stats += countAlliance + " alliance checks.\n";
            stats += countReportedMessage + " reported messages handled.\n";
            stats += countUnban + " unbans made.\n";
            stats += countBan + " bans made.\n";
            stats += (countEmailChanged / 2) + " emails changed.\n";
            stats += countLogOff + " logoffs made.\n";
            stats += countIpCheck + " ip check changed.\n";
            stats += countPasswordReset + " password changed.\n\n";

            if (File.Exists(_ticketStatsFileName))
            {
                // create reader & open file
                TextReader tr = new StreamReader(_ticketStatsFileName);

                // read a line of text
                string contentOfFile = tr.ReadToEnd();

                foreach (string line in contentOfFile.Split('\n'))
                {
                    string[] userStats = line.Split('|');
                    if (userStats.Count() > 4 && userStats[0].ToLower().Equals(such.ToLower()))
                    {
                        _ticketStats = line;
                    }
                }
                tr.Close();
                tr.Dispose();
            }

            string[] atClick = _atStats.Split('|');
            string[] ticketStats = _ticketStats.Split('|');

            stats += "AT Stats:\n";
            stats += "3 days | 7 days | 14 days | 30 days | All\n";
            try
            {
                stats += " " + atClick[0] + " | " + atClick[1] + " | " + atClick[2] + " | " + atClick[3] + " | " + atClick[4] + " \n\n";
            }
            catch
            {
                stats += "not available\n\n";
            }

            //stats += "Ticket Stats:\n";
            //stats += "1 day | 3 days | 7 days | All\n";
            //if (!string.IsNullOrEmpty(_ticketStats))
            //    stats += " " + ticketStats[1] + " | " + ticketStats[2] + " | " + ticketStats[3] + " | " + ticketStats[4] + " \n\n";
            //else
            //    stats += "\n";

            string bbcode = "[b][color=#009900][size=18]********************** " + _gameManager.GetUniverseDomain(_universeIdList.FirstOrDefault()) + " **********************[/size][/color][/b]\n";
            bbcode += "**************************\n";
            bbcode += "[size=12][color=#ff0000][b]" + such + "[/b][/color][/size]\n";
            bbcode += "**************************\n\n";
            bbcode += "[b][u]Admin Tool[/u][/b]\n";
            try
            {
                bbcode += "[table][tr][td]3 days[/td][td]7 days[/td][td]14 days[/td][td]30 days[/td][td]All[/td][/tr][tr][td][align=center][b]" + atClick[0] + "[/b][/align][/td][td][align=center][b]" + atClick[1] + "[/b][/align][/td][td][align=center][b]" + atClick[2] + "[/b][/align][/td][td][align=center][b]" + atClick[3] + "[/b][/align][/td][td][align=center][b]" + atClick[4] + "[/b][/align][/td][/tr][/table]\n";
            }
            catch
            {
                bbcode += "[table][tr][td]3 days[/td][td]7 days[/td][td]14 days[/td][td]30 days[/td][td]All[/td][/tr][tr][td][align=center][b] - [/b][/align][/td][td][align=center][b] - [/b][/align][/td][td][align=center][b] - [/b][/align][/td][td][align=center][b] - [/b][/align][/td][td][align=center][b] - [/b][/align][/td][/tr][/table]\n";

            }


            //bbcode += "[b][u]Ticket[/u][/b]\n";
            //bbcode += "[table][tr][td]1 day[/td][td]3 days[/td][td]7 days[/td][td]All[/td][/tr]\n";

            //if (!string.IsNullOrEmpty(_ticketStats))
            //    bbcode += "[tr][td][align=center][b]" + ticketStats[1] + "[/b][/align][/td][td][align=center][b]" + ticketStats[2] + "[/b][/align][/td][td][align=center][b]" + ticketStats[3] + "[/b][/align][/td][td][align=center][b]" + ticketStats[4] + "[/b][/align][/td][/tr][/table]\n";
            //else
            //    bbcode += "[tr][td][align=center][b]{1_day}[/b][/align][/td][td][align=center][b]{3_days}[/b][/align][/td][td][align=center][b]{7_days}[/b][/align][/td][td][align=center][b]{All}[/b][/align][/td][/tr][/table]\n";


            bbcode += "[b][size=12][color=#ff6600]" + such + " made:[/color][/size][/b]\n";
            bbcode += "[table][tr][td][b]" + countOverview + "[/b][/td][td] overview checks.[/td][/tr][tr][td][b]" + countFleetLog + "[/b][/td][td] fleetlog checks.[/td][/tr][tr][td][b]" + countLoginLog + "[/b][/td][td] cookie / logins checks.[/td][/tr][tr][td][b]" + countAlliance + "[/b][/td][td] alliance checks.[/td][/tr][tr][td][b]" + countReportedMessage + "[/b][/td][td] reported messages handled.[/td][/tr][tr][td][b]" + countUnban + "[/b][/td][td] unbans made.[/td][/tr][tr][td][b]" + countBan + "[/b][/td][td] bans made.[/td][/tr][tr][td][b]" + countEmailChanged + "[/b][/td][td] emails changed.[/td][/tr][tr][td][b]" + countLogOff + "[/b][/td][td] logoffs made.[/td][/tr][tr][td][b]" + countIpCheck + "[/b][/td][td] ip check changed.[/td][/tr][tr][td][b]" + countPasswordReset + "[/b][/td][td] password changed.[/td][/tr][/table]\n\n";

            bbcode += "[color=#ffff99]# are GO tasks being fulfilled and to what extent?[/color]\n";
            bbcode += "[color=#99ff99]# are user notes and general notes added on a regular basis?[/color]\n";
            bbcode += "[color=#ffff99]# are tickets being replied to within 24/48 hrs?[/color]\n";
            bbcode += "[color=#99ff99]# please add some descriptive evaluation:[/color]\n\n";

            if (logList.Count > 0)
            {
                try
                {
                    //_fileName = g_UserData.Universe + "_" + such + "_" + checkFrom.ToString("dd'-'MM'-'yyyy") + "-to-" + checkTo.ToString("dd'-'MM'-'yyyy") + ".txt";
                    //// create a writer and open the file
                    //TextWriter tw = new StreamWriter(_outPut + _fileName);

                    //string fileContent = "Statistics for " + such + ":\n\n" + stats;

                    //fileContent += "\n\nSource Code for the board:\n\n" + bbcode;

                    //// write a line of text to the file
                    //tw.WriteLine(fileContent);

                    //// close the stream
                    //tw.Close();
                    //tw.Dispose();
                    //CrossThreadManagement.SetControlValue(btnOpenFile, "Enabled", true);
                }
                catch (Exception ex)
                {
                    //_fileName = "";
                    //DisplayError("Could not save the output to file!");
                    //CrossThreadManagement.SetControlValue(btnOpenFile, "Enabled", false);
                }
                if (string.IsNullOrEmpty(_fileName))
                {
                    space += "Statistics for " + such + ":\n\n";
                }
                else
                {
                    space += "The output was saved to the following text file:" + "\n\n";
                    space += "Directory: " + _outPut + "\n";
                    space += "File name: " + _fileName + "\n\n";
                }
                CrossThreadManagement.SetControlValue(txbResult2, "text", space + stats + "\n\nSource Code for the board:\n\n" + bbcode);
            }
            else
            {
                CrossThreadManagement.SetControlValue(txbResult2, "text", space + "No log found for " + such);
            }

            CrossThreadManagement.SetControlValue(btnGenerate, "Enabled", true);
            CrossThreadManagement.SetControlValue(btnCancel, "Enabled", false);
        }


        private List<ATLog> GetLogsFilterByDateAndSuch(DateTime dateFrom, DateTime dateTo, string such, int page)
        {
            CrossThreadManagement.SetControlValue(labeStatus, "Text", string.Format(statusTemplate, page, 0, 0));

            bool continueProcess = true;
            int side = page;
            int totalFound = 0;

            List<ATLog> finalList = new List<ATLog>();

            while (continueProcess && !_quitProcess)
            {
                CrossThreadManagement.SetControlValue(labeStatus, "Text", string.Format(statusTemplate, side, totalFound, finalList.Count));

                continueProcess = false;
                DateTime lastEntry;
                List<ATLog> tempList = new List<ATLog>();
                tempList = _gameManager.GetLogs(_universeIdList.FirstOrDefault(), such, side, out _atStats);
                if (tempList == null)
                    break;
                totalFound += tempList.Count;
                CrossThreadManagement.SetControlValue(labeStatus, "Text", string.Format(statusTemplate, side, totalFound, finalList.Count));
                if (tempList.Count > 0)
                {
                    lastEntry = tempList.Min(i => i.Date);
                    if (((DateTime.Compare(lastEntry, dateFrom.Date) > 0) || (DateTime.Compare(lastEntry, dateFrom.Date) == 0)))
                    {
                        continueProcess = true;
                    }
                }

                int found = 0;
                foreach (ATLog log in tempList)
                {
                    if (((DateTime.Compare(dateFrom.Date, log.Date.Date) < 0) || (DateTime.Compare(dateFrom.Date, log.Date.Date) == 0)) && ((DateTime.Compare(dateTo.Date, log.Date.Date) > 0) || (DateTime.Compare(dateTo.Date, log.Date.Date) == 0)))
                    {
                        found++;
                        finalList.Add(log);
                        CrossThreadManagement.SetControlValue(labeStatus, "Text", string.Format(statusTemplate, side, totalFound, finalList.Count));
                    }
                }
                side++;
            }
            return finalList;
        }


        #endregion ----- Private Methods ------


        /***********************************************************************************************************/


        #region ----- BackgroundWorker DoWork Methods ------



        #endregion ----- BackgroundWorker DoWork Methods ------


        /***********************************************************************************************************/


        #region ----- Events Callback ------


        void OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE bgwState)
        {
            lock (_locker)
            {
                if (bgwState.Equals(Enums.BACKGROUNDWORKER_STATE.START_WORK))
                    _backgroundWorkerRunning++;
                else
                    _backgroundWorkerRunning--;

                ShowHideInProgressBar();
            }
        }


        #endregion ----- Events Callback ------


        /***********************************************************************************************************/


        #region ----- Protected Fire Events ------


        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/

    }
}
