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
    public partial class ComaToolControl : UserControl
    {
        /***********************************************************************************************************/


        #region ----- Privates Variables ------


        private object _locker = new object();
        private GameManager _gameManager;
        private TicketManager _ticketManager;
        private List<string> _universeIdList = new List<string>();
        private int _backgroundWorkerRunning = 0;
        private bool _isInProgressBarVisible = false;
        private bool _isComaToolSessionValid = false;

        private TabControl _tabControl;
        private ComaToolLogin _comaToolLoginControl;
        private ComaToolSubControl _comaToolSubControl;
        private ComaToolWebBrowserControl _comaToolWebBrowserControl;

        private List<TabPage> _tabPageList = new List<TabPage>();
        private TabPage _tabPageWebBrowser;


        #endregion ----- Privates Variables ------


        /***********************************************************************************************************/


        #region ----- Public Delegate ------


        #endregion ----- Public Delegate ------


        /***********************************************************************************************************/


        #region ----- Public Publish Event ------


        #endregion ----- Public Publish Event ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public ComaToolControl(GameManager gameManager)
        {
            InitializeComponent();
            _gameManager = gameManager;
            _ticketManager = _gameManager.GetTicketManager();
            _ticketManager.NotifyLoggedOut += new TicketManager.NotifyLoggedOutEventHandler(_ticketManager_NotifyLoggedOut);
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Events ------


        private void btnMenu_Click(object sender, EventArgs e)
        {
            //contextMenuStrip1.Show(btnMenu, Point.Empty);
            contextMenuStrip1.Show(btnMenu, btnMenu.Bounds.Left - 7, btnMenu.Bounds.Bottom - 8);
        }


        private void contextMenuStrip1_MouseLeave(object sender, EventArgs e)
        {
            contextMenuStrip1.Hide();
        }


        void _tabControl_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                ComaToolSubControl subControl = e?.TabPage?.Controls[0] as ComaToolSubControl ?? null;
                subControl?.SelectActiveCommunity();
            }
            catch { }
        }


        #endregion ----- Events ------


        /***********************************************************************************************************/


        #region ----- Public Methods ------


        public void LoadControl()
        {
            AddComaToolLoginControl(true);
            _comaToolWebBrowserControl = new ComaToolWebBrowserControl(_gameManager, this);
            _comaToolWebBrowserControl.Dock = DockStyle.Fill;
        }


        public void ApplyFocus()
        {
        }


        public void OnNotifyLoggedIn()
        {
            // Remove login control
            RemoveComaToolLoginControl();

            List<string> communityList = _ticketManager.GetComaToolCommunityList();

            AddTabControl();

            _tabControl.Selected += new TabControlEventHandler(_tabControl_Selected);

            AddComaToolWebBrowserTab();
            _tabPageWebBrowser.Controls.Add(_comaToolWebBrowserControl);
            _comaToolWebBrowserControl.LoadControl(new Uri("https://lobby.ogame.gameforge.com/en_GB/"));

            int count = 1;
            foreach (string community in communityList)
            {
                int communityId = Int32.Parse(community.Split('|')[0]);
                string communityName = community.Split('|')[1];
                TabPage tab = new TabPage(communityName);
                tab.BackColor = System.Drawing.Color.Transparent;
                tab.UseVisualStyleBackColor = true;

                ComaToolSubControl subControl = new ComaToolSubControl(_gameManager, this, communityId, communityName);
                subControl.Dock = DockStyle.Fill;
                tab.Controls.Add(subControl);

                _tabControl.TabPages.Add(tab);

                if (count == 1)
                {
                    _tabControl.SelectedTab = tab;
                    subControl.SelectActiveCommunity();
                }

                if (communityList.Count < 10)
                {
                    //subControl.LoadControl();
                }
                else if (count == 1)
                {
                    //subControl.LoadControl();
                }
                count++;
            }

            //if (communityList.Count == 1)
            //{
            //    int communityId = Int32.Parse(communityList[0].Split('|')[0]);
            //    string communityName = communityList[0].Split('|')[1];
            //    AddComaToolSubControl(communityId, communityName);
            //}
            //else
            //{
            //    MessageBox.Show("Sorry, it seems you do not currently have access to the ticket tool.\r\rIf this is an error, please contact vodler", "Could not load tickets");
            //}
        }


        public void OnNotifyLoggedOut()
        {
            // If 1 community
            RemoveComaToolSubControl();

            // If more than 1 community
            // Loop and remove tab + subControl
            RemoveTabControl();

            // Add login control
            AddComaToolLoginControl(false);
        }


        #endregion ----- Public Methods ------


        /***********************************************************************************************************/


        #region ----- Internal Methods ------


        #endregion ----- Internal Methods ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private void RemoveComaToolLoginControl()
        {
            if (this.Controls.Contains(_comaToolLoginControl))
                this.Controls.Remove(_comaToolLoginControl);
            _comaToolLoginControl = null;
        }


        private void AddComaToolLoginControl(bool firstLoad)
        {
            RemoveComaToolLoginControl();
            _comaToolLoginControl = new ComaToolLogin(_gameManager, this);
            _comaToolLoginControl.Dock = DockStyle.Fill;
            this.Controls.Add(_comaToolLoginControl);
            _comaToolLoginControl.LoadControl(firstLoad);
        }


        private void RemoveComaToolSubControl()
        {
            if (this.Controls.Contains(_comaToolSubControl))
                this.Controls.Remove(_comaToolSubControl);
            _comaToolSubControl = null;
        }


        private void AddComaToolSubControl(int communityId, string communityName)
        {
            RemoveComaToolSubControl();
            _comaToolSubControl = new ComaToolSubControl(_gameManager, this, communityId, communityName);
            _comaToolSubControl.Dock = DockStyle.Fill;
            this.Controls.Add(_comaToolSubControl);
        }


        private void RemoveTabControl()
        {
            if (this.Controls.Contains(_tabControl))
            {
                foreach (TabPage tab in _tabControl.TabPages)
                {
                    tab.Controls.Clear();
                }
                _tabControl.TabPages.Clear();
                this.Controls.Remove(_tabControl);
            }
            _tabControl = null;
        }


        private void AddTabControl()
        {
            RemoveTabControl();
            _tabControl = new TabControl();
            _tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(_tabControl);
        }


        private void AddComaToolWebBrowserTab()
        {
            _tabPageWebBrowser = new TabPage("WebView");
            _tabPageWebBrowser.BackColor = System.Drawing.Color.Transparent;
            _tabPageWebBrowser.UseVisualStyleBackColor = true;
            _tabControl.TabPages.Add(_tabPageWebBrowser);
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


        void _ticketManager_NotifyLoggedOut()
        {
            BeginInvoke(new MethodInvoker(() => MessageBox.Show("Your ComaTool session has expired.\n\nPlease login again to access the ticket system.", "Session invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)));
            BeginInvoke(new MethodInvoker(() => OnNotifyLoggedOut()));
        }


        #endregion ----- Events Callback ------



        /***********************************************************************************************************/


        #region ----- Protected Fire Events ------


        #endregion ----- Protected Fire Events ------


        /***********************************************************************************************************/

    }
}
