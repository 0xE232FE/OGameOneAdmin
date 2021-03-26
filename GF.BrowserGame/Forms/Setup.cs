using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Web.Services.Protocols;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Utility;
using System.ComponentModel;
using System.Collections;
using LibCommonUtil;

namespace GF.BrowserGame.Forms
{
    internal sealed partial class Setup : Form
    {
        private Guid _toolId;
        private string _prevToolVersion;
        private string _newToolVersion;
        private bool _setupMode = false;
        private List<Community> _communityList;
        private List<Universe> _universeList;
        private bool _myPasswordSet = false;
        private bool _myUniverseSet = true;
        private bool _multiUniverseMessageDisplayed = false;
        private string _userMasterPassword;
        private List<Community> _userCommunityList = new List<Community>();
        private List<Universe> _userUniverseList = new List<Universe>();
        private List<Universe> _tempUserUniverseList = new List<Universe>();
        private Guid _applicationId;
        private Guid _celestosUserId;
        private string _celestosUserName;
        private string _celestosPassword;

        public string WindowTitle
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public List<Community> UserCommunityList
        {
            get { return _userCommunityList; }
            set { _userCommunityList = value; }
        }

        public List<Universe> UserUniverseList
        {
            get { return _userUniverseList; }
            set { _userUniverseList = value; }
        }

        public string CelestosPassword
        {
            get { return _celestosPassword; }
            set { _celestosPassword = value; }
        }

        public string CelestosUserName
        {
            get { return _celestosUserName; }
            set { _celestosUserName = value; }
        }

        public Guid CelestosUserId
        {
            get { return _celestosUserId; }
            set { _celestosUserId = value; }
        }

        public string UserMasterPassword
        {
            get { return _userMasterPassword; }
            set { _userMasterPassword = value; }
        }

        /***********************************************************************************************************/


        #region ------ Delegates ------


        private delegate string GetListViewItemTextDelegate(ListView listView, int itemIndex);
        private delegate int GetListViewItemIndexByItemTextDelegate(ListView listView, string itemText);
        private delegate int GetListViewItemIndexBySubItemTextDelegate(ListView listView, int subItemIndex, string itemText);
        private delegate ListViewItem GetListViewItemDelegate(ListView listView, int subItemIndex, string itemText);


        #endregion ------ Delegates ------


        /***********************************************************************************************************/


        public Setup(bool setupMode, List<Community> communityList, List<Universe> universeList)
        {
            InitializeComponent();

            _setupMode = setupMode;
            _communityList = communityList;
            _universeList = universeList;
        }


        public Setup(bool setupMode, Guid toolId, Guid applicationId, string prevToolVersion, string newToolVersion)
        {
            InitializeComponent();

            _setupMode = setupMode;
            _toolId = toolId;
            _applicationId = applicationId;
            _prevToolVersion = prevToolVersion;
            _newToolVersion = newToolVersion;
        }


        private void Setup_Shown(object sender, EventArgs e)
        {
            //if ((_communityList != null && _universeList != null) && (_communityList.Count > 0 && _universeList.Count > 0))
            //{
            //    comboBoxCommunityList.Items.Add("-- All --");
            //    foreach (Community community in _communityList)
            //    {
            //        comboBoxCommunityList.Items.Add(community.Name);
            //    }

            //    comboBoxCommunityList.SelectedIndex = 0;
            //}
            //else
            //{
            //    MessageBox.Show("The application data is corrupted or has been deleted.\n\nPlease contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //    DialogResult = DialogResult.Cancel;
            //}
            tabControlSetup.TabPages.Remove(tabPageMyUniverse);
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            if (btnNext.Text.Equals("Set Password"))
            {
                if (txtboxPassword.Text.Length < 4)
                    MessageBox.Show("Your password must contain at least 4 characters.", "Password too short!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (!txtboxPassword.Text.Equals(txtboxRepeatPassword.Text))
                    MessageBox.Show("Please make sure both passwords are the same.", "Passwords don't match!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    _myPasswordSet = true;
                    _userMasterPassword = txtboxPassword.Text;
                    //tabControlSetup.SelectedTab = tabPageMyUniverse;
                    tabControlSetup.SelectedTab = tabPageMyAccount;
                    txtboxPassword.Enabled = txtboxRepeatPassword.Enabled = false;
                    linkLabelEditPassword.Visible = true;
                    return;
                }
                _myPasswordSet = false;
            }
            else if (btnNext.Text.Equals("Set Universe(s)"))
            {
                _userCommunityList.Clear();

                if (_tempUserUniverseList == null || _tempUserUniverseList.Count == 0)
                    MessageBox.Show("To complete the setup, you must select at least 1 universe.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    _userUniverseList = _tempUserUniverseList;

                    if (_userUniverseList.Count > 0)
                    {
                        foreach (Universe universe in _userUniverseList)
                        {
                            if (!_userCommunityList.Exists(r => r.Id == universe.CommunityId))
                            {
                                Community community = GetCommunity(universe.CommunityId);
                                if (community != null)
                                    _userCommunityList.Add(community);
                            }
                        }
                        _myUniverseSet = true;
                        tabControlSetup.SelectedTab = tabPageMyAccount;
                        return;
                    }
                    else
                        MessageBox.Show("To complete the setup, you must select at least 1 universe.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _myUniverseSet = false;
                }
            }
            else if (btnNext.Text.Equals("Complete Setup"))
            {
                if (CelestosLogintextBox.Text.Trim().Length < 1 || CelestosPasswordtextBox.Text.Trim().Length < 1)
                    MessageBox.Show("You must provide a login name and password.", "Fields required!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    BackgroundWorker bgw = new BackgroundWorker();
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);

                    btnCancel.Enabled = false;
                    btnNext.Enabled = false;
                    pictureBoxStatus.Visible = true;
                    labelStatus.Text = "Please wait...";
                    labelStatus.Visible = true;
                    bgw.RunWorkerAsync();
                }
            }
        }


        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBoxStatus.Visible = false;
            labelStatus.Visible = false;

            btnCancel.Enabled = true;
            btnNext.Enabled = true;

            if (e.Error != null)
            {
                if (e.Error.Message.Equals("Network"))
                {
                    MessageBox.Show("Unfortunately, celestos's server is temporarilly unavailable.\r\n\r\nPlease try again in a few minutes.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(e.Error.Message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.Cancel;
                    return;
                }
            }
            else
            {
                if (e.Result.ToString().Equals("Valid credentials"))
                {
                    MessageBox.Show("You have been authenticated successfully!\r\n\r\nClick Ok to complete the setup.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    if (e.Result.ToString().Equals("Wrong credentials"))
                        MessageBox.Show("Your credentials are not valid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(e.Result.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            bool knownException = false;
            _celestosUserName = CelestosLogintextBox.Text.Trim();
            _celestosPassword = CelestosPasswordtextBox.Text;
            OgameServiceV1Call webServiceCall = new OgameServiceV1Call(_toolId, "", _applicationId, _celestosUserName, _celestosPassword, null);

            try
            {
                SetupAppObj returnObj = webServiceCall.SetupApplication(_prevToolVersion, _newToolVersion, true);

                if (!returnObj.Error)
                {
                   // _celestosUserId = returnObj.UserId;

                    if (returnObj.IsToolValid && returnObj.IsUserAllowedToUseThisTool)
                    {
                        if (!string.IsNullOrEmpty(returnObj.CommunityData))
                        {
                            try
                            {
                                _userCommunityList = SerializeDeserializeObject.DeserializeObject<List<GF.BrowserGame.Schema.Serializable.Community>>(returnObj.CommunityData);
                            }
                            catch { }

                            if (_userCommunityList != null && _userCommunityList.Count > 0)
                            {
                                e.Result = "Valid credentials";
                                return;
                            }
                        }
                        knownException = true;
                        throw new Exception("Your credentials are valid but unfortunately you have not been allocated to any universes!\r\n\r\nContact your game administrator for more information.");
                    }
                    else if (!returnObj.IsToolValid)
                    {
                        knownException = true;
                        throw new Exception("Your credentials are valid but unfortunately this tool is not currently active or your version is expired!\r\n\r\nDownload a new version or contact an administrator for more information.");
                    }
                    else if (!returnObj.IsUserAllowedToUseThisTool)
                    {
                        knownException = true;
                        throw new Exception("Your credentials are valid but unfortunately you are not allowed to use this tool!\r\n\r\nContact an administrator for more information.");
                    }
                }
                else
                {
                    knownException = true;
                    throw new Exception("Your credentials are valid but unfortunately something went wrong while downloading your account details!\r\n\r\nContact an administrator for more information.");
                }
            }
            catch (SoapException soapEx)
            {
                if (soapEx.Message.Contains("account is not approved"))
                    e.Result = "Authentication has failed because your account is not approved.\r\n\r\nContact an administrator for more information";
                else if (soapEx.Message.Contains("account is locked"))
                    e.Result = "Authentication has failed because your account is locked.\r\n\r\nContact an administrator for more information";
                else if (soapEx.Message.Contains("database problem"))
                    throw new Exception("Network");
                else
                    e.Result = "Wrong credentials";
            }
            catch (Exception ex)
            {
                if (!knownException || ex.Message.Equals("all webservices are down"))
                    throw new Exception("Network");
                else
                    throw ex;
            }
        }


        private void linkLabelEditPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _myPasswordSet = false;
            txtboxPassword.Enabled = txtboxRepeatPassword.Enabled = true;
            linkLabelEditPassword.Visible = false;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancelling the setup will close the application.\n\nAre you sure?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                DialogResult = DialogResult.Cancel;
        }


        private void tabControlSetup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btnNext.Enabled)
            {
                tabControlSetup.SelectedTab = tabPageMyAccount;
                return;
            }

            if (tabControlSetup.SelectedTab == tabPageMyUniverse && !_myPasswordSet)
            {
                MessageBox.Show("You must set your Master Password first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlSetup.SelectedTab = tabPageMyPassword;
                return;
            }
            else if (tabControlSetup.SelectedTab == tabPageMyAccount && !_myPasswordSet)
            {
                MessageBox.Show("You must set your Master Password first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlSetup.SelectedTab = tabPageMyPassword;
                return;
            }
            else if (tabControlSetup.SelectedTab == tabPageMyAccount && !_myUniverseSet)
            {
                MessageBox.Show("You must set your universe(s) first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlSetup.SelectedTab = tabPageMyUniverse;
                return;
            }
            else if (tabControlSetup.SelectedTab == tabPageMyUniverse && (!txtboxPassword.Text.Trim().Equals(_userMasterPassword) || !txtboxRepeatPassword.Text.Trim().Equals(_userMasterPassword)))
            {
                MessageBox.Show("It seems that you have modified your Master Password but not clicked the button \"Set Password\".\n\nPlease verify your password.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlSetup.SelectedTab = tabPageMyPassword;
                return;
            }
            else if (tabControlSetup.SelectedTab == tabPageMyUniverse)
            {
                listViewUniverseList.Focus();
                btnNext.Text = "Set Universe(s)";
            }
            else if (tabControlSetup.SelectedTab == tabPageMyPassword)
            {
                btnNext.Text = "Set Password";
            }
            else if (tabControlSetup.SelectedTab == tabPageMyAccount)
            {
                listViewUniverseList.Focus();
                btnNext.Text = "Complete Setup";
            }

            if (tabControlSetup.SelectedTab == tabPageMyUniverse && !_multiUniverseMessageDisplayed)
            {
                _multiUniverseMessageDisplayed = true;
                MessageBox.Show("The purpose of this tool is to manage all your universes at the same time and within the same tool.\n\nIf you have access to 2 universes in OGame.us and 1 universe in OGame.org, you can select all three from the list.\nIf you have access to more unis, then select more!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listViewUniverseList.Focus();
            }
        }


        private void linkLabelSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem item in listViewUniverseList.Items)
            {
                item.Checked = true;
            }
        }


        private void linkLabelSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem item in listViewUniverseList.Items)
            {
                item.Checked = false;
            }
        }


        private void listViewUniverseList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in listViewUniverseList.Items)
                {
                    item.Checked = true;
                }
            }
            else if (e.KeyCode == Keys.Z && e.Control)
            {
                listViewUniverseList.MultiSelect = true;
                foreach (ListViewItem item in listViewUniverseList.Items)
                {
                    item.Checked = false;
                }
            }
        }


        private void listViewUniverseList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                if (e.Item.Checked)
                {
                    Universe universe = GetUniverse(e.Item.Tag.ToString());
                    if (universe != null && !_tempUserUniverseList.Exists(r => r.Id == universe.Id))
                        _tempUserUniverseList.Add(universe);
                }
                else
                {
                    Universe universe = GetUniverse(e.Item.Tag.ToString());
                    if (universe != null)
                        _tempUserUniverseList.Remove(universe);
                }
            }
            catch { }
        }


        private void listViewUniverseList_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewUniverseList.SelectedItems.Count == 1)
                {
                    foreach (ListViewItem item in listViewUniverseList.SelectedItems)
                    {
                        item.Checked = !item.Checked;
                        item.Selected = false;
                    }
                }
            }
            catch { }
        }


        private void comboBoxCommunityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCommunityList.SelectedIndex == -1)
                {
                    comboBoxCommunityList.SelectedIndex = 0;
                    return;
                }

                listViewUniverseList.Items.Clear();

                if (comboBoxCommunityList.SelectedIndex == 0)
                {
                    foreach (Universe universe in _universeList)
                    {
                        if (_tempUserUniverseList.Exists(r => r.Id == universe.Id))
                            AddListViewItem(listViewUniverseList, universe.Number.ToString(), universe.Id, universe.Id, true, true);
                        else
                            AddListViewItem(listViewUniverseList, universe.Number.ToString(), universe.Id, universe.Id, true, false);
                    }
                }
                else
                {
                    string communityName = comboBoxCommunityList.SelectedItem.ToString().ToLower();
                    string communityId = _communityList.SingleOrDefault(r => r.Name.ToLower().Equals(communityName)).Id;

                    foreach (Universe universe in _universeList)
                    {
                        if (universe.CommunityId.Equals(communityId))
                        {
                            if (_tempUserUniverseList.Exists(r => r.Id == universe.Id))
                                AddListViewItem(listViewUniverseList, universe.Number.ToString(), universe.Id, universe.Id, true, true);
                            else
                                AddListViewItem(listViewUniverseList, universe.Number.ToString(), universe.Id, universe.Id, true, false);
                        }
                    }
                }
                listViewUniverseList.Focus();
            }
            catch (Exception ex)
            {
            }
        }


        private Community GetCommunity(string communityId)
        {
            foreach (Community community in _communityList)
            {
                if (community.Id.Equals(communityId))
                    return community;
            }
            return null;
        }


        private Universe GetUniverse(string universeId)
        {
            foreach (Universe universe in _universeList)
            {
                if (universe.Id.Equals(universeId))
                    return universe;
            }
            return null;
        }


        /***********************************************************************************************************/


        #region ------ ListView Access Methods (thread safe) ------


        private string GetListViewItemText(ListView listView, int itemIndex)
        {
            if (listView.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (string)this.Invoke(new GetListViewItemTextDelegate(GetListViewItemText), new object[] { listView, itemIndex });
            }
            else
            {
                return listView.Items[itemIndex].Text;
            }
        }


        /// <summary>
        /// Get an itemIndex using an item.Text value.
        /// Returns -1 if the item does not exist
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="itemText"></param>
        /// <returns></returns>
        private int GetListViewItemIndex(ListView listView, string itemText)
        {
            if (listView.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (int)this.Invoke(new GetListViewItemIndexByItemTextDelegate(GetListViewItemIndex), new object[] { listView, itemText });
            }
            else
            {
                foreach (ListViewItem item in listView.Items)
                {
                    if (item.Text.ToUpper().Equals(itemText.ToUpper()))
                        return item.Index;
                }
                return -1;
            }
        }


        /// <summary>
        /// Get an itemIndex using a subItemIndex and the corresponding itemText.
        /// Returns -1 if the item does not exist
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="subItemIndex"></param>
        /// <param name="itemText"></param>
        /// <returns></returns>
        private int GetListViewItemIndex(ListView listView, int subItemIndex, string itemText)
        {
            if (listView.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (int)this.Invoke(new GetListViewItemIndexBySubItemTextDelegate(GetListViewItemIndex), new object[] { listView, subItemIndex, itemText });
            }
            else
            {
                foreach (ListViewItem item in listView.Items)
                {
                    if (item.SubItems[subItemIndex].Text.ToUpper().Equals(itemText.ToUpper()))
                        return item.Index;
                }
                return -1;
            }
        }


        /// <summary>
        /// Get a ListViewItem using a subItemIndex and the corresponding itemText.
        /// Returns null if the item does not exist
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="subItemIndex"></param>
        /// <param name="itemText"></param>
        /// <returns></returns>
        private ListViewItem GetListViewItem(ListView listView, int subItemIndex, string itemText)
        {
            if (listView.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                return (ListViewItem)this.Invoke(new GetListViewItemDelegate(GetListViewItem), new object[] { listView, subItemIndex, itemText });
            }
            else
            {
                foreach (ListViewItem item in listView.Items)
                {
                    if (item.SubItems[subItemIndex].Text.ToUpper().Equals(itemText.ToUpper()))
                        return item;
                }
                return null;
            }
        }


        #endregion ------ ListView Access Methods (thread safe) ------


        /***********************************************************************************************************/


        #region ------ General ListView Methods (not thread safe) ------


        private void AddListViewItem(ListView listView, string text, string tag)
        {
            ListViewItem item = new ListViewItem();
            item.Text = text;
            item.Tag = tag;
            listView.Items.Add(item);
        }


        private void AddListViewItem(ListView listView, string text, string tag, string subItemText, bool useItemStyleForSubItems)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            AddListViewSubItem(item, subItemText);
            listView.Items.Add(item);
        }


        private void AddListViewItem(ListView listView, string text, string tag, string subItemText, bool useItemStyleForSubItems, bool isChecked)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            item.Checked = isChecked;
            AddListViewSubItem(item, subItemText);
            listView.Items.Add(item);
        }


        private void AddListViewItem(ListView listView, string text, string tag, string[] subItemsTextArray, bool useItemStyleForSubItems)
        {
            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = useItemStyleForSubItems;
            item.Text = text;
            item.Tag = tag;
            foreach (string subItemText in subItemsTextArray)
            {
                AddListViewSubItem(item, subItemText);
            }
            listView.Items.Add(item);
        }


        private void AddListViewSubItem(ListView listView, int itemIndex, string subItemText)
        {
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            subItem.Text = subItemText;
            listView.Items[itemIndex].SubItems.Add(subItem);
        }


        private void AddListViewSubItem(ListView listView, string itemText, string subItemText)
        {
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            subItem.Text = subItemText;
            int itemIndex = GetListViewItemIndex(listView, itemText);
            if (itemIndex != -1)
                listView.Items[itemIndex].SubItems.Add(subItem);
        }

        // Modified for PerformanceCounterMonitor
        private void AddListViewSubItem(ListViewItem item, string subItemText)
        {
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            subItem.Text = subItemText;
            // Modification starts
            //if (!string.IsNullOrEmpty(subItemText) && subItemText.Contains('/'))
            //{
            //    subItem.Tag = subItemText.Split('/')[0].Trim().Replace(",", "");
            //}
            // Modification ends
            item.SubItems.Add(subItem);
        }


        private void UpdateListViewItem(ListView listView, int itemIndex, string itemText)
        {
            if (!listView.Items[itemIndex].Text.Equals(itemText))
                listView.Items[itemIndex].Text = itemText;
        }


        private void UpdateListViewItem(ListView listView, string itemText, string newItemText)
        {
            int itemIndex = GetListViewItemIndex(listView, itemText);
            if (itemIndex != -1 && !listView.Items[itemIndex].Text.Equals(newItemText))
                listView.Items[itemIndex].Text = newItemText;
        }

        // Modified for PerformanceCounterMonitor
        private void UpdateListViewSubItem(ListView listView, int itemIndex, int subItemIndex, string subItemText)
        {
            if (!listView.Items[itemIndex].SubItems[subItemIndex].Text.Equals(subItemText))
            {
                listView.Items[itemIndex].SubItems[subItemIndex].Text = subItemText;
                // Modification starts
                //if (!string.IsNullOrEmpty(subItemText) && subItemText.Contains('/'))
                //{
                //    listView.Items[itemIndex].Tag = subItemText.Split('/')[0].Trim().Replace(",", "");
                //}
                // Modification ends
            }
        }


        private void UpdateListViewSubItem(ListView listView, string itemText, int subItemIndex, string subItemText)
        {
            int itemIndex = GetListViewItemIndex(listView, itemText);
            if (itemIndex != -1 && !listView.Items[itemIndex].SubItems[subItemIndex].Text.Equals(subItemText))
                listView.Items[itemIndex].SubItems[subItemIndex].Text = subItemText;
        }


        private void UpdateListViewSubItem(ListViewItem item, int subItemIndex, string subItemText)
        {
            if (!item.SubItems[subItemIndex].Text.Equals(subItemText))
                item.SubItems[subItemIndex].Text = subItemText;
        }


        #endregion ------ General ListView Methods (not thread safe) ------


        /***********************************************************************************************************/

    }
}
