using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GF.BrowserGame.Schema.Serializable;

namespace OGameOneAdmin
{
    public partial class Setup : Form
    {
        private bool _setupMode = false;
        private List<Community> _communityList;
        private List<Universe> _universeList;
        private bool _myPasswordSet = false;
        private bool _multiUniverseMessageDisplayed = false;
        private string _userPassword;
        private List<Community> _userCommunityList = new List<Community>();
        private List<Universe> _userUniverseList = new List<Universe>();
        private List<Universe> _tempUserUniverseList = new List<Universe>();

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

        public string UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }
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


        private void Setup_Shown(object sender, EventArgs e)
        {
            if ((_communityList != null || _communityList.Count > 0) && (_universeList != null || _universeList.Count > 0))
            {
                comboBoxCommunityList.Items.Add("-- All --");
                foreach (Community community in _communityList)
                {
                    comboBoxCommunityList.Items.Add(community.Name);
                }

                comboBoxCommunityList.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("The application data is corrupted or has been deleted.\n\nPlease contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                DialogResult = DialogResult.Cancel;
            }
        }


        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            if (txtboxPassword.Text.Trim().Length < 4)
                MessageBox.Show("Your password must contain at least 4 characters.", "Password too short!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (!txtboxPassword.Text.Trim().Equals(txtboxRepeatPassword.Text.Trim()))
                MessageBox.Show("Please make sure both passwords are the same.", "Passwords don't match!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                _myPasswordSet = true;
                _userPassword = txtboxPassword.Text.Trim();
                tabControlSetup.SelectedTab = tabPageMyUniverse;
                txtboxPassword.Enabled = txtboxRepeatPassword.Enabled = false;
                linkLabelEditPassword.Visible = true;
                return;
            }
            _myPasswordSet = false;
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
            if (tabControlSetup.SelectedTab == tabPageMyUniverse && !_myPasswordSet)
            {
                MessageBox.Show("You must set your password first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlSetup.SelectedTab = tabPageMyPassword;
            }
            else if (tabControlSetup.SelectedTab == tabPageMyUniverse && (!txtboxPassword.Text.Trim().Equals(_userPassword) || !txtboxRepeatPassword.Text.Trim().Equals(_userPassword)))
            {
                MessageBox.Show("It seems that you have modified your password but not clicked the button \"Set Password\".\n\nPlease verify your password.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlSetup.SelectedTab = tabPageMyPassword;
            }
            else if (tabControlSetup.SelectedTab == tabPageMyUniverse && !_multiUniverseMessageDisplayed)
            {
                _multiUniverseMessageDisplayed = true;
                MessageBox.Show("If you have access to more than 1 universe and/or more than 1 community, you may add all of them during this setup.\n\nThe purpose of this tool is to manage all your universes at the same time and within the same tool.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private void btnCompleteSetup_Click(object sender, EventArgs e)
        {
            _userUniverseList.Clear();

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
                    DialogResult = DialogResult.OK;
                }
                else
                    MessageBox.Show("To complete the setup, you must select at least 1 universe.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void comboBoxCommunityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCommunityList.SelectedIndex == -1)
                    comboBoxCommunityList.SelectedIndex = 0;

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

                    foreach (Universe universe in _universeList)
                    {
                        if (universe.Id.Contains(communityName))
                        {
                            if (_tempUserUniverseList.Exists(r => r.Id == universe.Id))
                                AddListViewItem(listViewUniverseList, universe.Number.ToString(), universe.Id, universe.Id, true, true);
                            else
                                AddListViewItem(listViewUniverseList, universe.Number.ToString(), universe.Id, universe.Id, true, false);
                        }
                    }
                }
            }
            catch { }
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
