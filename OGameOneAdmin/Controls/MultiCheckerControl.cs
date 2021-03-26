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
using GF.BrowserGame.Schema.Internal;

namespace OGameOneAdmin.Controls
{
    public partial class MultiCheckerControl : UserControl
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private object _locker = new object();
        private GameManager _gameManager;
        private List<string> _universeIdList = new List<string>();
        private List<List<Account>> _matchingDataList;
        private List<Account> _accountList = new List<Account>();
        private DateTime multiDateFrom;
        private DateTime multiDateTo;
        private int _backgroundWorkerRunning = 0;
        private bool _isInProgressBarVisible = false;


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public MultiCheckerControl(GameManager gameManager)
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
                        if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_MULTI, universe.Id))
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
                        if (universe.CommunityId.Equals(communityId) && _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_MULTI, universe.Id))
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


        private void btnSearchMatchingData_Click(object sender, EventArgs e)
        {
            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }

            if (!checkBoxMultiAlliance.Checked && !checkBoxMultiIP.Checked && !checkBoxMultiEmail.Checked)
                return;

            EnableDisableAll(false);

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                SearchMatchingData();
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);
                listBoxMatchingData.Items.Clear();
                if (_matchingDataList != null)
                {
                    foreach (List<Account> accountList in _matchingDataList)
                    {
                        string key = accountList.Count + " :";
                        string valueOfKey = "";
                        foreach (Account account in accountList)
                        {
                            valueOfKey += account.Uid;
                            key += " " + account.Name;
                        }
                        listBoxMatchingData.Items.Add(new ItemObject(key, valueOfKey));
                    }
                }
            };
            worker.RunWorkerAsync();
        }


        private void listBoxMatchingData_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemObject accountListObj = listBoxMatchingData.SelectedItem as ItemObject;

            if (_matchingDataList != null)
            {
                foreach (List<Account> accountList in _matchingDataList)
                {
                    string valueOfKey = "";
                    foreach (Account account in accountList)
                    {
                        valueOfKey += account.Uid;
                    }
                    if (valueOfKey.Equals(accountListObj.ValueOfKey.ToString()))
                    {
                        _accountList.Clear();
                        listBoxAccountList.Items.Clear();
                        foreach (Account account in accountList)
                        {
                            listBoxAccountList.Items.Add(account.Name);
                            _accountList.Add(account);
                        }
                    }
                }
            }
        }


        private void btnAddToList_Click(object sender, EventArgs e)
        {
            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }

            Int64 playerId = -1;
            string searchPlayerName = txtBoxAddAccount.Text.Trim();

            if (string.IsNullOrEmpty(searchPlayerName))
            {
                MessageBox.Show("Please enter a player name that you wish to search and add to the list", "Field is empty");
                return;
            }

            if (PlayerNameExistInAccountInvolved(searchPlayerName))
            {
                MessageBox.Show(searchPlayerName + " has already been added to the list.", "Player already in the list");
                txtBoxAddAccount.Text = "";
                return;
            }

            EnableDisableAll(false);

            var worker = new BackgroundWorker();

            worker.DoWork += (workerSender2, workerEvent2) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                playerId = _gameManager.SearchPlayerId(_universeIdList.FirstOrDefault(), searchPlayerName);
            };

            worker.RunWorkerCompleted += (workerSender2, workerEvent2) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);
                if (playerId != -1)
                {
                    if (playerId != 0)
                    {
                        if (PlayerIdExistInAccountInvolved(playerId))
                        {
                            MessageBox.Show(searchPlayerName + " has already been added to the list", "Player already in the list");
                            txtBoxAddAccount.Text = "";
                        }
                        else
                        {
                            Account account = new Account();
                            account.Uid = playerId;
                            account.Name = searchPlayerName;

                            listBoxAccountList.Items.Add(account.Name);
                            _accountList.Add(account);
                            txtBoxAddAccount.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show(searchPlayerName + " could not be found on this server.", "Player not found");
                    }
                }
            };
            worker.RunWorkerAsync();
        }


        private void btnRemoveAccountFromList_Click(object sender, EventArgs e)
        {
            if (listBoxAccountList.SelectedItems.Count == 0)
                return;

            string playerName = listBoxAccountList.SelectedItem.ToString();

            if (PlayerNameExistInAccountInvolved(playerName))
            {
                List<Account> accList = new List<Account>();
                foreach (Account acc in _accountList)
                {
                    if (!acc.Name.Equals(playerName))
                    {
                        accList.Add(acc);
                    }
                }
                _accountList = accList;
                listBoxAccountList.Items.Remove(playerName);
            }
        }


        private void btnOpenAcc_Click(object sender, EventArgs e)
        {
            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }

            try
            {
                if (listBoxAccountList.SelectedItems.Count > 0)
                {
                    string nickname = listBoxAccountList.SelectedItem.ToString();
                    int playerId = 0;

                    foreach (Account acc in _accountList)
                    {
                        if (acc.Name.ToLower().Equals(nickname.ToLower()))
                            playerId = int.Parse(acc.Uid.ToString());
                    }

                    _gameManager.GetUniManager(_universeIdList.FirstOrDefault()).OpenWebBrowserUserAccount(_gameManager.GetUserApplicationConfig().ExternalWebBrowser, playerId);
                }
            }
            catch
            {

                MessageBox.Show("Error occurred");
            }
        }


        private void btnOpenAllAcc_Click(object sender, EventArgs e)
        {
            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }

            try
            {
                if (_accountList.Count > 0)
                {
                    if (_accountList.Count > 5)
                    {
                        if (MessageBox.Show("You are about to open in your web browser " + _accountList.Count + " players overview.\n\nAre you sure?", "Info", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            return;
                    }
                    foreach (Account acc in _accountList)
                    {
                        int playerId = int.Parse(acc.Uid.ToString());
                        _gameManager.GetUniManager(_universeIdList.FirstOrDefault()).OpenWebBrowserUserAccount(_gameManager.GetUserApplicationConfig().ExternalWebBrowser, playerId);
                    }
                }
            }
            catch
            {

                MessageBox.Show("Error occurred");
            }
        }


        private void btnCheckMulti_Click(object sender, EventArgs e)
        {
            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }

            if (_accountList.Count == 0)
            {
                DisplayError("Error, you have not selected any players.");
                return;
            }

            multiDateFrom = DateTime.Now.AddMonths(-3).AddDays(5);
            multiDateTo = DateTime.Now.AddDays(1);

            EnableDisableAll(false);

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                string progress = "";
                foreach (Account acc in _accountList)
                {
                    progress += "Retrieving login logs for " + acc.Name + "</br>";
                    BeginInvoke(new MethodInvoker(() => webBrowser1.DocumentText = progress));
                    int playerId = int.Parse(acc.Uid.ToString());
                    acc.IpLogList = _gameManager.RetrieveLoginsIP(_universeIdList.FirstOrDefault(), acc.Name, playerId, multiDateFrom, multiDateTo);
                }
            };

            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                EnableDisableAll(true);
                webBrowser1.DocumentText = "Processing logs...";
                GenerateMultiReport();
            };
            worker.RunWorkerAsync();
        }


        #endregion ----- Events ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        public void LoadControl()
        {
            SetupComboBox();
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
                if (_gameManager.HasCommunitySecureObject(Constants.SecureObject.VIEW_TAB_MULTI, community.Id))
                    comboBoxCommunityList.Items.Add(new ItemObject(community.Name, community.Id));
            }

            comboBoxUniverseList.Items.Add("-- All --");

            foreach (Universe universe in _gameManager.UniverseList)
            {
                if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_MULTI, universe.Id))
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


        private void SearchMatchingData()
        {
            if (_universeIdList.Count == 0)
            {
                DisplayError("Error, you must select a universe.");
                return;
            }

            _matchingDataList = _gameManager.RetrieveMatchingData(_universeIdList.FirstOrDefault(), checkBoxMultiEmail.Checked, checkBoxMultiIP.Checked, checkBoxMultiAlliance.Checked);
        }


        private bool PlayerIdExistInAccountInvolved(Int64 playerId)
        {
            if (_accountList != null)
            {
                if (_accountList.FirstOrDefault(r => r.Uid == playerId) != null)
                    return true;
            }
            return false;
        }


        private bool PlayerNameExistInAccountInvolved(string playerName)
        {
            if (_accountList != null)
            {
                if (_accountList.FirstOrDefault(r => r.Name.ToLower().Equals(playerName.ToLower())) != null)
                    return true;
            }
            return false;
        }


        private void GenerateMultiReport()
        {
            if (_accountList == null || _accountList.Count == 0)
                return;

            List<IpLog> ipLogList = new List<IpLog>();
            foreach (Account account in _accountList)
            {
                foreach (IpLog ipLog in account.IpLogList)
                {
                    ipLogList.Add(ipLog);
                }
            }

            if (ipLogList.Count == 0)
            {
                webBrowser1.DocumentText = "No ip log was found!";
                return;
            }

            string body = "";
            int countLess60 = 0;
            int countLess300 = 0;
            int countLess1800 = 0;

            string header = "";
            //header += "<font color=\"#ff0000\">Log in under 60 seconds apart</font><br />";
            //header += "<font color=\"#ff8000\">Log in under 300 seconds apart</font><br />";
            //header += "<font color=\"#0000ff\">Log in under 1800 seconds apart</font><br /><br />";
            body += "<table>";

            IpLog prevLog = null;
            bool notPrinted = true;
            bool newDay = false;

            //multiIpLogList.OrderBy(d => d.DateTime);
            var multiLoginsList = from log in ipLogList
                                  orderby log.DateTime ascending
                                  select log;

            foreach (IpLog log in multiLoginsList)
            {
                if (prevLog == null)
                    prevLog = log;
                else
                {
                    TimeSpan diff = log.DateTime - prevLog.DateTime;
                    //if (diff.TotalHours < 24 && log.DateTime.Month > prevLog.DateTime.Month && log.DateTime.Day > prevLog.DateTime.Day)
                    //{
                    //    newDay = true;
                    //}

                    if (prevLog.IpString.Equals(log.IpString) && !prevLog.Nick.Equals(log.Nick))
                    {
                        if (diff.TotalSeconds < 60)
                        {
                            countLess60++;
                            if (notPrinted)
                                body += "<tr><td><font color=\"#ff0000\">" + prevLog.Date + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff0000\">" + prevLog.IpString + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff0000\">" + prevLog.Nick + "</font></td></tr>";
                            if (newDay)
                                body += "<tr><td colspan=\"3\"><hr /></td></tr>";
                            body += "<tr><td><font color=\"#ff0000\">" + log.Date + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff0000\">" + log.IpString + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff0000\">" + log.Nick + "</font></td></tr>";
                            notPrinted = false;
                        }
                        else if (diff.TotalSeconds < 300)
                        {
                            countLess300++;
                            if (notPrinted)
                                body += "<tr><td><font color=\"#ff8000\">" + prevLog.Date + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff8000\">" + prevLog.IpString + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff8000\">" + prevLog.Nick + "</font></td></tr>";
                            if (newDay)
                                body += "<tr><td colspan=\"3\"><hr /></td></tr>";
                            body += "<tr><td><font color=\"#ff8000\">" + log.Date + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff8000\">" + log.IpString + "</font>&nbsp;&nbsp;</td><td><font color=\"#ff8000\">" + log.Nick + "</font></td></tr>";
                            notPrinted = false;
                        }
                        else if (diff.TotalSeconds < 1800)
                        {
                            countLess1800++;
                            if (notPrinted)
                                body += "<tr><td><font color=\"#0000ff\">" + prevLog.Date + "</font>&nbsp;&nbsp;</td><td><font color=\"#0000ff\">" + prevLog.IpString + "</font>&nbsp;&nbsp;</td><td><font color=\"#0000ff\">" + prevLog.Nick + "</font></td></tr>";
                            if (newDay)
                                body += "<tr><td colspan=\"3\"><hr /></td></tr>";
                            body += "<tr><td><font color=\"#0000ff\">" + log.Date + "</font>&nbsp;&nbsp;</td><td><font color=\"#0000ff\">" + log.IpString + "</font>&nbsp;&nbsp;</td><td><font color=\"#0000ff\">" + log.Nick + "</font></td></tr>";
                            notPrinted = false;
                        }
                        else
                        {
                            if (notPrinted)
                                body += "<tr><td>" + prevLog.Date + "&nbsp;&nbsp;</td><td>" + prevLog.IpString + "&nbsp;&nbsp;</td><td>" + prevLog.Nick + "</td></tr>";
                            if (newDay)
                                body += "<tr><td colspan=\"3\"><hr /></td></tr>";
                            notPrinted = true;
                            //temp += "<tr><td>" + log.Date + "&nbsp;&nbsp;</td><td>" + log.IpString + "&nbsp;&nbsp;</td><td>" + log.Nick + "</td></tr>";
                        }
                    }
                    else
                    {
                        if (notPrinted)
                            body += "<tr><td>" + prevLog.Date + "&nbsp;&nbsp;</td><td>" + prevLog.IpString + "&nbsp;&nbsp;</td><td>" + prevLog.Nick + "</td></tr>";
                        if (newDay)
                            body += "<tr><td colspan=\"3\"><hr /></td></tr>";
                        notPrinted = true;
                    }
                    //temp += "<tr><td>" + log.Date + "&nbsp;&nbsp;</td><td>" + log.IpString + "&nbsp;&nbsp;</td><td>" + log.Nick + "</td></tr>";
                    prevLog = log;
                    newDay = false;
                }
            }

            body += "</table>";

            string report = "******** Results ********<br /><font color=\"#ff0000\"># of logins under 60 sec = " + countLess60 + "</font><br />";
            report += "<font color=\"#ff8000\"># of logins under 300 sec = " + countLess300 + "</font><br />";
            report += "<font color=\"#0000ff\"># of logins under 1800 sec = " + countLess1800 + "</font><br /><br />";

            webBrowser1.DocumentText = header + report + body;
        }


        private void EnableDisableAll(bool state)
        {
            comboBoxCommunityList.Enabled = state;
            comboBoxUniverseList.Enabled = state;
            groupBox1.Enabled = state;
            groupBox2.Enabled = state;
            groupBox3.Enabled = state;
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
