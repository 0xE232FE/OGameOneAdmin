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

namespace OGameOneAdmin.Controls
{
    public partial class TemplateControl : UserControl
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private object _locker = new object();
        private GameManager _gameManager;
        private List<string> _universeIdList = new List<string>();
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


        public TemplateControl(GameManager gameManager)
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

                    comboBoxUniverseList.Items.Add("-- All --");

                    foreach (Universe universe in _gameManager.UniverseList)
                    {
                        if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, universe.Id))
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
                    comboBoxUniverseList.Items.Add("-- All --");

                    int count = 0;
                    foreach (Universe universe in _gameManager.UniverseList)
                    {
                        if (universe.CommunityId.Equals(communityId) && _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, universe.Id))
                        {
                            comboBoxUniverseList.Items.Add(new ItemObject(universe.Domain, universe.Id));
                            count++;
                        }
                    }

                    if (count == 1)
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
                    if (comboBoxCommunityList.SelectedIndex == 0)
                    {
                        _universeIdList = _gameManager.UniverseList.Where(p => _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, p.Id)).Select(r => r.Id).ToList();
                    }
                    else
                    {
                        ItemObject communityObj = comboBoxCommunityList.SelectedItem as ItemObject;
                        string communityName = communityObj.Key;
                        string communityId = communityObj.ValueOfKey.ToString();
                        _universeIdList = _gameManager.UniverseList.Where(r => r.CommunityId.Equals(communityId) && _gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, r.Id)).Select(r => r.Id).ToList();
                    }
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
                if (_gameManager.HasCommunitySecureObject(Constants.SecureObject.VIEW_TAB_NOTES, community.Id))
                    comboBoxCommunityList.Items.Add(new ItemObject(community.Name, community.Id));
            }

            comboBoxUniverseList.Items.Add("-- All --");

            foreach (Universe universe in _gameManager.UniverseList)
            {
                if (_gameManager.HasUniverseSecureObject(Constants.SecureObject.VIEW_TAB_NOTES, universe.Id))
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
