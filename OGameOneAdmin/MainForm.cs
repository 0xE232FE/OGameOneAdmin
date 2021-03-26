using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using OGameOneAdmin.Controls;
using GF.BrowserGame;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame.Static;
using LibCommonUtil;
using System.Configuration;
using System.IO;
using LibCommonUtil.WebBrowser;
using System.Net;

namespace OGameOneAdmin
{
    public partial class MainForm : Form
    {
        /***********************************************************************************************************/


        #region ------ Privates Variables ------


        private Guid _toolId = new Guid("F4FC05F5-FD75-469C-92E3-8483201B3210");
        private string _toolVersion = "1.4";
        private bool _applicationExit = false;
        private bool _lockApplication = false;
        private UserAppConfig _userAppConfig;
        private GameManager _gameManager;
        private TicketManager _ticketManager;
        private List<UniManager> _uniManagerList = new List<UniManager>();
        private List<Universe> _universeList = new List<Universe>();
        private long _bytesDownloaded = 0;
        private object _locker = new object();
        private int _lastUniverseSelected = -1;
        private FormWindowState _currentFormWindowState = FormWindowState.Normal;

        // Tab Pages
        private TabPage _tabPageAdminNotes = new TabPage("Notes");
        private TabPage _tabPageATLogsViewer = new TabPage("Stats");
        private TabPage _tabPageComaTool = new TabPage("Tickets");
        private TabPage _tabPageMultiChecker = new TabPage("Multi");

        // Controls
        private AdminNotesControl _adminNotesControl;
        private ATLogsViewerControl _atLogsViewerControl;
        private ComaToolControl _comaToolControl;
        private MultiCheckerControl _multiCheckerControl;


        #endregion ------ Privates Variables ------


        /***********************************************************************************************************/


        #region ------ Delegates ------


        private delegate void AddProcessListToListViewDelegate(List<Universe> universeList);
        private delegate void UpdateSingleItemListViewDelegate(string universeId, bool beginEndUpdate);


        #endregion ------ Delegates ------


        /***********************************************************************************************************/


        #region ----- Constructor ------


        public MainForm()
        {
            InitializeComponent();
            this.Text = Application.ProductName;
            this.Opacity = 0;
            //CustomCertificatePolicy ccp = new CustomCertificatePolicy();
            //ServicePointManager.CertificatePolicy = ccp;
        }


        #endregion ----- Constructor ------


        /***********************************************************************************************************/


        #region ----- Form Events ------


        #region ----- Main Events ------


        private void MainForm_Load(object sender, EventArgs e)
        {
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!LoadGameManager())
                this.Close();
            else
            {
                tabControlMain_Selected(null, null);
                this.Opacity = 100;

                BeginInvoke(new MethodInvoker(() =>
                {
                    try
                    {
                        _gameManager.ShowOptions(false);
                        ResetAppSettings();
                        _gameManager.ShowDashboardTips(false);
                    }
                    catch { }
                }));
            }
        }


        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (notifyIcon1.Visible)
                notifyIcon1_Click(null, null);
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (!_applicationExit)
                listViewUniverse.ResizeColumns();

            if (this.WindowState != FormWindowState.Minimized)
                _currentFormWindowState = this.WindowState;

            if (this.WindowState == FormWindowState.Minimized && _userAppConfig != null && (_userAppConfig.MinimizeToTray || !_userAppConfig.ShowAppInTaskBar))
                minimizeToTrayToolStripMenuItem_Click(null, null);
        }


        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (_userAppConfig != null && _userAppConfig.ShowAppInTaskBar)
                notifyIcon1.Visible = false;

            this.Show();
            this.WindowState = _currentFormWindowState;

            if (_lockApplication)
                lockApplicationToolStripMenuItem_Click(null, null);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _applicationExit = true;

                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = false;

                Application.DoEvents();

                if (_gameManager != null)
                {
                    if (_gameManager.IsInitialized)
                    {
                        UnregisterEvents();

                        Application.DoEvents();

                        for (int i = 1; i <= 5; i++)
                        {
                            Thread.Sleep(200);
                            Application.DoEvents();
                        }
                    }
                    _gameManager.CheckApplicationExceptionLog();
                    _gameManager.EndApplicationSession();
                }
            }
            catch (Exception ex)
            {
                // Log it
            }
        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_gameManager != null)
                    _gameManager.SaveApplicationConfig();

                if (_gameManager != null && _gameManager.IsInitialized)
                    _gameManager.SaveApplicationData();
            }
            catch (Exception ex)
            {
                // Log it
            }
        }


        private void tabControlMain_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (tabControlMain.SelectedTab == _tabPageAdminNotes)
                    _adminNotesControl.ApplyFocus();
                else if (tabControlMain.SelectedTab == tabPageDashboard)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        listViewUniverse.Focus();
                        if (_lastUniverseSelected != -1)
                        {
                            try
                            {
                                //listViewUniverse.Items[_lastUniverseSelected].Selected = true;
                            }
                            catch { }
                        }
                    }));
                }
            }
            catch { }
        }


        #endregion ----- Main Events ------


        #region ----- Main Menu Events ------


        private void loginToAllUniversesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (UniManager uniManager in _uniManagerList)
            {
                if (uniManager.IsSessionStatusInvalid() && uniManager.IsCredentialsExist())
                {
                    BackgroundWorker bgw = new BackgroundWorker();
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWorkQuickLogin);
                    bgw.RunWorkerAsync(uniManager.GetUniverseId());
                }
            }
        }


        private void logoutFromAllUniversesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (UniManager uniManager in _uniManagerList)
            {
                if (uniManager.IsSessionStatusValid() || uniManager.IsSessionStatusUnknown())
                {
                    BackgroundWorker bgw = new BackgroundWorker();
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWorkLogout);
                    bgw.RunWorkerAsync(uniManager.GetUniverseId());
                }
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void lockApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _lockApplication = true;
                if (sender != null && _userAppConfig != null && _userAppConfig.MinimizeWhenAppLocked)
                    minimizeToTrayToolStripMenuItem_Click(null, null);
                else if (_gameManager.LockUnLockApplication())
                    _lockApplication = false;
                else
                    minimizeToTrayToolStripMenuItem_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The application will exit due to an internal error.", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameManager.ShowOptions(true);
            ResetAppSettings();
        }


        private void synchronizeDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadApp();
        }


        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _gameManager.CheckForUpdates();
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Close application"))
                    this.Close();
                else
                    MessageBox.Show("An error occurred: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void minimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;

            if (this.WindowState != FormWindowState.Minimized)
                _currentFormWindowState = this.WindowState;

            this.Hide();

            if (_userAppConfig != null && !_userAppConfig.DoNotShowMinimizeToolTip)
            {
                notifyIcon1.BalloonTipTitle = Application.ProductName;
                if (_lockApplication)
                    notifyIcon1.BalloonTipText = "Your application has been locked and minimized to the system tray.";
                else
                    notifyIcon1.BalloonTipText = "Your application has been minimized to the system tray.";
                notifyIcon1.ShowBalloonTip(3000);
            }
        }


        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameManager.ShowDashboardTips(true);
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameManager.ShowAbout();
        }


        #endregion ----- Main Menu Events ------


        #region ----- UniverseList Events ------


        private void listViewUniverse_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            FlickerFreeListView myListView = (FlickerFreeListView)sender;

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == myListView.ListViewColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (myListView.ListViewColumnSorter.Order == SortOrder.Ascending)
                {
                    myListView.ListViewColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    myListView.ListViewColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                myListView.ListViewColumnSorter.SortColumn = e.Column;
                myListView.ListViewColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            myListView.ListViewItemSorter = myListView.ListViewColumnSorter;
            myListView.Sort();
        }


        private void listViewUniverse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewUniverse.SelectedItems.Count == 1)
                _lastUniverseSelected = listViewUniverse.SelectedIndices[0];
            else if (_lastUniverseSelected != -1)
            {
                try
                {
                    listViewUniverse.Items[_lastUniverseSelected].Focused = false;
                }
                catch { }
            }
            UpdateUniverseListMenu();
        }


        #endregion ----- UniverseList Events ------


        #region ----- UniverseList Menu Events ------


        private void universeListContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (listViewUniverse.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
        }


        private void quickLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string universeId = GetSelectedUniveseId();
            if (!_gameManager.IsQuickLoginAllowed(universeId))
                _gameManager.QuickLogin(universeId);
            else
            {
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.DoWork += new DoWorkEventHandler(bgw_DoWorkQuickLogin);
                bgw.RunWorkerAsync(universeId);
            }
        }


        private void loginWithInternetExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string universeId = GetSelectedUniveseId();
            _gameManager.LoginWithInternetExplorer(universeId);
        }


        private void loginWithAUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string universeId = GetSelectedUniveseId();
            _gameManager.LoginWithURL(universeId);
        }


        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWorkLogout);
            string universeId = GetSelectedUniveseId();
            bgw.RunWorkerAsync(universeId);
        }


        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string universeId = GetSelectedUniveseId();
            _gameManager.GetUniManager(universeId).OpenWebBrowser(_userAppConfig.ExternalWebBrowser);
        }


        private void checkSessionStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckUnknownSessionStatus(GetSelectedUniveseId());
        }


        #endregion ----- UniverseList Menu Events ------


        #endregion ----- Form Events ------


        /***********************************************************************************************************/


        #region ----- BackgroundWorker DoWork Methods ------


        void bgw_DoWorkQuickLogin(object sender, DoWorkEventArgs e)
        {
            _gameManager.QuickLogin(e.Argument.ToString());
        }


        void bgw_DoWorkLogout(object sender, DoWorkEventArgs e)
        {
            _gameManager.GetUniManager(e.Argument.ToString()).Logout();
        }


        void bgw_DoWorkCheckUnknownSessionStatus(object sender, DoWorkEventArgs e)
        {
            if (e.Argument == null)
            {
                foreach (UniManager uniManager in _uniManagerList.Where(r => r.IsSessionStatusUnknown()))
                {
                    uniManager.IsSessionValidAsync();
                }
            }
            else
            {
                string universeId = e.Argument.ToString();
                _gameManager.GetUniManager(universeId).IsSessionValidAsync();
            }
        }


        #endregion ----- BackgroundWorker DoWork Methods ------


        /***********************************************************************************************************/


        #region ----- Private Methods ------


        private void TimeDuration()
        {
            double totalDuration = 0;
            DateTime start = DateTime.Now;

            DateTime end = DateTime.Now;
            totalDuration = new TimeSpan(end.Ticks - start.Ticks).TotalMilliseconds;
            BeginInvoke(new MethodInvoker(() => MessageBox.Show("Login duration = " + totalDuration.ToString())));
        }


        private void ReloadApp()
        {
            try
            {
                this.Enabled = false;
                UnregisterEvents();

                if (_gameManager.HasAnyCommunitySecureObject(Constants.SecureObject.VIEW_TAB_NOTES))
                {
                    _tabPageAdminNotes.Controls.Remove(_adminNotesControl);
                    _adminNotesControl = null;
                    tabControlMain.TabPages.Remove(_tabPageAdminNotes);
                }

                _gameManager.ReloadApplicationData();

                _uniManagerList = _gameManager.UniManagerList;
                _universeList = _gameManager.UniverseList;

                foreach (UniManager uniManager in _uniManagerList)
                {
                    uniManager.LoggedIn += new UniManager.LoggedInEventHandler(uniManager_LoggedIn);
                    uniManager.LoginFailed += new UniManager.LoginFailedEventHandler(uniManager_LoginFailed);
                    uniManager.LoggedOut += new UniManager.LoggedOutEventHandler(uniManager_LoggedOut);
                    uniManager.SessionInvalid += new UniManager.SessionInvalidEventHandler(uniManager_SessionInvalid);
                    uniManager.ErrorOccurred += new UniManager.ErrorOccurredEventHandler(uniManager_ErrorOccurred);
                    uniManager.BytesDownloaded += new UniManager.BytesDownloadedEventHandler(uniManager_BytesDownloaded);
                }

                SetListViewUniverse(_universeList);
                listViewUniverse.ResizeColumns();
                CheckUnknownSessionStatus(null);

                if (_gameManager.HasAnyCommunitySecureObject(Constants.SecureObject.VIEW_TAB_NOTES))
                {
                    _adminNotesControl = new AdminNotesControl(_gameManager);
                    _adminNotesControl.Dock = DockStyle.Fill;
                    _tabPageAdminNotes.Controls.Add(_adminNotesControl);
                    _tabPageAdminNotes.BackColor = System.Drawing.Color.Transparent;
                    _tabPageAdminNotes.UseVisualStyleBackColor = true;
                    tabControlMain.TabPages.Add(_tabPageAdminNotes);
                    _adminNotesControl.LoadControl();
                }

                this.Enabled = true;
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("Close application"))
                    MessageBox.Show(ex.Message, "Error while re-loading the application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private bool LoadGameManager()
        {
            try
            {
                _gameManager = new GameManager(_toolId, _toolVersion);

                if (_gameManager.IsInitialized)
                {
                    ResetAppSettings();

                    _gameManager.NotifyLoggedOut += new GameManager.NotifyLoggedOutEventHandler(_gameManager_NotifyLoggedOut);
                    _uniManagerList = _gameManager.UniManagerList;
                    _universeList = _gameManager.UniverseList;

                    _ticketManager = _gameManager.GetTicketManager();
                    _ticketManager.LoggedIn += new TicketManager.LoggedInEventHandler(_ticketManager_LoggedIn);
                    _ticketManager.LoginFailed += new TicketManager.LoginFailedEventHandler(_ticketManager_LoginFailed);
                    _ticketManager.LoggedOut += new TicketManager.LoggedOutEventHandler(_ticketManager_LoggedOut);
                    _ticketManager.SessionInvalid += new TicketManager.SessionInvalidEventHandler(_ticketManager_SessionInvalid);
                    _ticketManager.ErrorOccurred += new TicketManager.ErrorOccurredEventHandler(_ticketManager_ErrorOccurred);
                    _ticketManager.BytesDownloaded += new TicketManager.BytesDownloadedEventHandler(_ticketManager_BytesDownloaded);

                    foreach (UniManager uniManager in _uniManagerList)
                    {
                        uniManager.LoggedIn += new UniManager.LoggedInEventHandler(uniManager_LoggedIn);
                        uniManager.LoginFailed += new UniManager.LoginFailedEventHandler(uniManager_LoginFailed);
                        uniManager.LoggedOut += new UniManager.LoggedOutEventHandler(uniManager_LoggedOut);
                        uniManager.SessionInvalid += new UniManager.SessionInvalidEventHandler(uniManager_SessionInvalid);
                        uniManager.ErrorOccurred += new UniManager.ErrorOccurredEventHandler(uniManager_ErrorOccurred);
                        uniManager.BytesDownloaded += new UniManager.BytesDownloadedEventHandler(uniManager_BytesDownloaded);
                    }

                    SetListViewUniverse(_universeList);
                    listViewUniverse.ResizeColumns();

                    CheckUnknownSessionStatus(null);

                    if (_gameManager.HasAnyCommunitySecureObject(Constants.SecureObject.VIEW_TAB_NOTES))
                    {
                        _adminNotesControl = new AdminNotesControl(_gameManager);
                        _adminNotesControl.Dock = DockStyle.Fill;
                        _tabPageAdminNotes.Controls.Add(_adminNotesControl);
                        _tabPageAdminNotes.BackColor = System.Drawing.Color.Transparent;
                        _tabPageAdminNotes.UseVisualStyleBackColor = true;
                        tabControlMain.TabPages.Add(_tabPageAdminNotes);
                        _adminNotesControl.LoadControl();
                    }

                    if (_gameManager.HasAnyCommunitySecureObject(Constants.SecureObject.VIEW_TAB_TICKETS))
                    {
                        _comaToolControl = new ComaToolControl(_gameManager);
                        _comaToolControl.Dock = DockStyle.Fill;
                        _tabPageComaTool.Controls.Add(_comaToolControl);
                        _tabPageComaTool.BackColor = System.Drawing.Color.Transparent;
                        _tabPageComaTool.UseVisualStyleBackColor = true;
                        tabControlMain.TabPages.Add(_tabPageComaTool);
                        _comaToolControl.LoadControl();
                    }

                    if (_gameManager.HasAnyCommunitySecureObject(Constants.SecureObject.VIEW_TAB_MULTI))
                    {
                        _multiCheckerControl = new MultiCheckerControl(_gameManager);
                        _multiCheckerControl.Dock = DockStyle.Fill;
                        _tabPageMultiChecker.Controls.Add(_multiCheckerControl);
                        _tabPageMultiChecker.BackColor = System.Drawing.Color.Transparent;
                        _tabPageMultiChecker.UseVisualStyleBackColor = true;
                        tabControlMain.TabPages.Add(_tabPageMultiChecker);
                        _multiCheckerControl.LoadControl();
                    }

                    if (_gameManager.HasAnyCommunitySecureObject(Constants.SecureObject.VIEW_TAB_STATS))
                    {
                        _atLogsViewerControl = new ATLogsViewerControl(_gameManager);
                        _atLogsViewerControl.Dock = DockStyle.Fill;
                        _tabPageATLogsViewer.Controls.Add(_atLogsViewerControl);
                        _tabPageATLogsViewer.BackColor = System.Drawing.Color.Transparent;
                        _tabPageATLogsViewer.UseVisualStyleBackColor = true;
                        tabControlMain.TabPages.Add(_tabPageATLogsViewer);
                        _atLogsViewerControl.LoadControl();
                    }
                    this.Enabled = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while loading the application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void ResetAppSettings()
        {
            if (_gameManager != null)
            {
                _userAppConfig = _gameManager.GetUserApplicationConfig();
                if (_userAppConfig == null)
                    return;
                this.Text = _userAppConfig.AppName;
                this.ShowInTaskbar = _userAppConfig.ShowAppInTaskBar;
            }
        }


        private void UnregisterEvents()
        {
            if (_ticketManager != null)
            {
                _ticketManager.LoggedIn -= new TicketManager.LoggedInEventHandler(_ticketManager_LoggedIn);
                _ticketManager.LoginFailed -= new TicketManager.LoginFailedEventHandler(_ticketManager_LoginFailed);
                _ticketManager.LoggedOut -= new TicketManager.LoggedOutEventHandler(_ticketManager_LoggedOut);
                _ticketManager.SessionInvalid -= new TicketManager.SessionInvalidEventHandler(_ticketManager_SessionInvalid);
                _ticketManager.ErrorOccurred -= new TicketManager.ErrorOccurredEventHandler(_ticketManager_ErrorOccurred);
                _ticketManager.BytesDownloaded -= new TicketManager.BytesDownloadedEventHandler(_ticketManager_BytesDownloaded);

            }
            if (_uniManagerList != null)
            {
                foreach (UniManager uniManager in _uniManagerList)
                {
                    uniManager.LoggedIn -= new UniManager.LoggedInEventHandler(uniManager_LoggedIn);
                    uniManager.LoginFailed -= new UniManager.LoginFailedEventHandler(uniManager_LoginFailed);
                    uniManager.LoggedOut -= new UniManager.LoggedOutEventHandler(uniManager_LoggedOut);
                    uniManager.SessionInvalid -= new UniManager.SessionInvalidEventHandler(uniManager_SessionInvalid);
                    uniManager.ErrorOccurred -= new UniManager.ErrorOccurredEventHandler(uniManager_ErrorOccurred);
                    uniManager.BytesDownloaded -= new UniManager.BytesDownloadedEventHandler(uniManager_BytesDownloaded);
                }
            }
        }


        private void CheckUnknownSessionStatus(string universeId)
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWorkCheckUnknownSessionStatus);
            if (string.IsNullOrEmpty(universeId))
                bgw.RunWorkerAsync();
            else
                bgw.RunWorkerAsync(universeId);
        }


        private int GetSelectedUniveseIndex()
        {
            if (listViewUniverse.SelectedIndices.Count > 0)
                return listViewUniverse.SelectedIndices[0];
            else
                return -1;
        }


        private string GetSelectedUniveseId()
        {
            if (listViewUniverse.SelectedIndices.Count > 0)
            {
                int selectedIndex = listViewUniverse.SelectedIndices[0];
                return listViewUniverse.Items[selectedIndex].Tag.ToString();
            }
            else
                return string.Empty;
        }


        private void UpdateUniverseListMenu()
        {
            if (listViewUniverse.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                this.Invoke(new MethodInvoker(() => UpdateUniverseListMenu()));
            }
            else
            {
                if (listViewUniverse.SelectedItems.Count == 0)
                {
                    // Enabled
                    quickLoginToolStripMenuItem.Enabled = false;
                    loginWithToolStripMenuItem.Enabled = false;
                    loginWithInternetExplorerToolStripMenuItem.Enabled = false;
                    loginWithAUrlToolStripMenuItem.Enabled = false;

                    logoutToolStripMenuItem.Enabled = false;
                    openInBrowserToolStripMenuItem.Enabled = false;

                    // Visible
                    checkSessionStatusToolStripMenuItem.Visible = false;
                }
                else
                {
                    int selectedIndex = GetSelectedUniveseIndex();
                    if (selectedIndex != -1)
                    {
                        string universeId = GetSelectedUniveseId();
                        universeIdToolStripMenuItem.Text = _gameManager.GetUniverseDomain(universeId);
                        universeIdToolStripMenuItem.Tag = universeId;
                        string sessionStatus = listViewUniverse.Items[selectedIndex].SubItems[4].Text;

                        bool hasQuickLogin = _gameManager.HasUniverseSecureObject(Constants.SecureObject.USE_QUICK_LOGIN, universeId);
                        bool hasIELogin = _gameManager.HasUniverseSecureObject(Constants.SecureObject.USE_IE_LOGIN, universeId);
                        bool hasUrlLogin = _gameManager.HasUniverseSecureObject(Constants.SecureObject.USE_URL_LOGIN, universeId);
                        bool hasOpenBrowser = _gameManager.HasUniverseSecureObject(Constants.SecureObject.USE_OPEN_BROWSER, universeId);

                        if (sessionStatus.Equals("Online"))
                        {
                            // Enabled
                            quickLoginToolStripMenuItem.Enabled = false;
                            loginWithToolStripMenuItem.Enabled = false;
                            loginWithInternetExplorerToolStripMenuItem.Enabled = false;
                            loginWithAUrlToolStripMenuItem.Enabled = false;

                            logoutToolStripMenuItem.Enabled = true;
                            openInBrowserToolStripMenuItem.Enabled = hasOpenBrowser;

                            // Visible
                            checkSessionStatusToolStripMenuItem.Visible = false;

                            quickLoginToolStripMenuItem.Visible = hasQuickLogin;
                            loginWithToolStripMenuItem.Visible = ((hasIELogin || hasUrlLogin) || (hasIELogin && hasUrlLogin)) ? true : false;
                            loginWithInternetExplorerToolStripMenuItem.Visible = hasIELogin;
                            loginWithAUrlToolStripMenuItem.Visible = hasUrlLogin;

                            logoutToolStripMenuItem.Visible = true;
                            openInBrowserToolStripMenuItem.Visible = hasOpenBrowser;
                        }
                        else if (sessionStatus.Equals("Offline"))
                        {
                            // Enabled
                            quickLoginToolStripMenuItem.Enabled = hasQuickLogin;
                            loginWithToolStripMenuItem.Enabled = ((hasIELogin || hasUrlLogin) || (hasIELogin && hasUrlLogin)) ? true : false;
                            loginWithInternetExplorerToolStripMenuItem.Enabled = hasIELogin;
                            loginWithAUrlToolStripMenuItem.Enabled = hasUrlLogin;

                            logoutToolStripMenuItem.Enabled = false;
                            openInBrowserToolStripMenuItem.Enabled = false;

                            // Visible
                            checkSessionStatusToolStripMenuItem.Visible = false;

                            quickLoginToolStripMenuItem.Visible = hasQuickLogin;
                            loginWithToolStripMenuItem.Visible = ((hasIELogin || hasUrlLogin) || (hasIELogin && hasUrlLogin)) ? true : false; ;
                            loginWithInternetExplorerToolStripMenuItem.Visible = hasIELogin;
                            loginWithAUrlToolStripMenuItem.Visible = hasUrlLogin;

                            logoutToolStripMenuItem.Visible = true;
                            openInBrowserToolStripMenuItem.Visible = hasOpenBrowser;
                        }
                        else if (sessionStatus.Equals("Unknown"))
                        {
                            // Enabled
                            quickLoginToolStripMenuItem.Enabled = false;
                            loginWithToolStripMenuItem.Enabled = false;
                            loginWithInternetExplorerToolStripMenuItem.Enabled = false;
                            loginWithAUrlToolStripMenuItem.Enabled = false;

                            logoutToolStripMenuItem.Enabled = false;
                            openInBrowserToolStripMenuItem.Enabled = false;

                            // Visible
                            checkSessionStatusToolStripMenuItem.Visible = true;

                            quickLoginToolStripMenuItem.Visible = false;
                            loginWithToolStripMenuItem.Visible = false;
                            loginWithInternetExplorerToolStripMenuItem.Visible = false;
                            loginWithAUrlToolStripMenuItem.Visible = false;

                            logoutToolStripMenuItem.Visible = false;
                            openInBrowserToolStripMenuItem.Visible = false;
                        }
                    }
                }
            }
        }


        private void SetListViewUniverse(List<Universe> universeList)
        {
            AddUniverseListToListViewUniverse(universeList);
        }


        private void AddUniverseListToListViewUniverse(List<Universe> universeList)
        {
            if (listViewUniverse.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                this.Invoke(new AddProcessListToListViewDelegate(AddUniverseListToListViewUniverse), new object[] { universeList });
            }
            else
            {
                listViewUniverse.BeginUpdate();
                listViewUniverse.Items.Clear();
                foreach (Universe universe in universeList)
                {
                    string[] subItemsTextArray = new string[4];
                    Color[] subItemsForeColorArray = new Color[4];
                    FontStyle[] subItemsFontStyleArray = new FontStyle[4];
                    subItemsTextArray[0] = universe.Number.ToString();
                    subItemsForeColorArray[0] = Color.Black;
                    subItemsFontStyleArray[0] = FontStyle.Regular;
                    subItemsTextArray[1] = universe.Name;
                    subItemsForeColorArray[1] = Color.Black;
                    subItemsFontStyleArray[1] = FontStyle.Regular;
                    subItemsTextArray[2] = universe.Domain;
                    subItemsForeColorArray[2] = Color.Black;
                    subItemsFontStyleArray[2] = FontStyle.Regular;
                    subItemsTextArray[3] = _gameManager.GetSessionStatus(universe.Id, out subItemsForeColorArray[3], out subItemsFontStyleArray[3]);

                    listViewUniverse.AddListViewItem(_gameManager.GetCommunityName(universe.CommunityId), universe.Id, subItemsTextArray, subItemsForeColorArray, subItemsFontStyleArray, false);
                }
                listViewUniverse.EndUpdate();

                if (listViewUniverse.Items.Count > 0)
                    listViewUniverse.Items[0].Selected = true;
            }
        }


        private void UpdateListViewUniverseStatus(string universeId, bool beginEndUpdate)
        {
            if (listViewUniverse.InvokeRequired)
            {
                //Marshal this call back to the UI thread (via the form instance)...
                this.Invoke(new UpdateSingleItemListViewDelegate(UpdateListViewUniverseStatus), new object[] { universeId, beginEndUpdate });
            }
            else
            {
                int itemIndex = listViewUniverse.GetListViewItemIndexByTag(universeId);

                if (itemIndex == -1) return;

                if (beginEndUpdate)
                    listViewUniverse.BeginUpdate();
                Color color;
                FontStyle fontStyle;
                listViewUniverse.UpdateListViewSubItem(itemIndex, 4, _gameManager.GetSessionStatus(universeId, out color, out fontStyle), color, fontStyle);
                listViewUniverse.Sort();
                if (beginEndUpdate)
                    listViewUniverse.EndUpdate();
            }
        }


        private string BytesToKiloBytes(long bytes, int decimals)
        {
            return EssentialUtil.FormatNumber(Math.Round(double.Parse((bytes / 1024).ToString()), decimals), ",");
        }


        private string BytesToMegaBytes(long bytes, int decimals)
        {
            return EssentialUtil.FormatNumber(Math.Round(double.Parse(((bytes / 1024) / 1024).ToString()), decimals), ",");
        }


        #endregion ----- Private Methods ------


        /***********************************************************************************************************/


        #region ----- Events Callback ------


        void uniManager_SessionInvalid(Universe universe)
        {
            if (!_applicationExit)
                BeginInvoke(new MethodInvoker(() => MessageBox.Show("You must have a valid session to use this feature.\n\nPlease login to this universe.", "Session invalid - " + universe.Domain, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)));
        }


        void uniManager_LoggedOut(Universe universe)
        {
            if (!_applicationExit)
            {
                UpdateListViewUniverseStatus(universe.Id, true);
                UpdateUniverseListMenu();
            }
        }


        void uniManager_LoggedIn(Universe universe)
        {
            if (!_applicationExit)
            {
                UpdateListViewUniverseStatus(universe.Id, true);
                UpdateUniverseListMenu();
            }
        }


        void uniManager_LoginFailed(Universe universe)
        {
            if (!_applicationExit)
                BeginInvoke(new MethodInvoker(() => MessageBox.Show("Please verify your credentials.", "Login failed - " + universe.Domain, MessageBoxButtons.OK, MessageBoxIcon.Error)));
        }


        void uniManager_ErrorOccurred(Universe universe, string errorMessage)
        {
            if (!_applicationExit)
                BeginInvoke(new MethodInvoker(() => MessageBox.Show(errorMessage, "Error occurred - " + universe.Domain, MessageBoxButtons.OK, MessageBoxIcon.Error)));
        }


        void uniManager_BytesDownloaded(long bytesDownloaded)
        {
            if (!_applicationExit)
            {
                lock (_locker)
                {
                    _bytesDownloaded += bytesDownloaded;
                }
                if (_bytesDownloaded < 1024)
                    BeginInvoke(new MethodInvoker(() => dataDownloadedtoolStripStatusLabelValue.Text = _bytesDownloaded.ToString() + " bytes"));
                else
                    BeginInvoke(new MethodInvoker(() => dataDownloadedtoolStripStatusLabelValue.Text = BytesToKiloBytes(_bytesDownloaded, 0) + " kb"));
            }
        }


        void _ticketManager_ErrorOccurred(string errorMessage)
        {
            if (!_applicationExit)
                BeginInvoke(new MethodInvoker(() => MessageBox.Show(errorMessage, "Error occurred - ComaTool", MessageBoxButtons.OK, MessageBoxIcon.Error)));
        }


        void _ticketManager_SessionInvalid()
        {
            if (!_applicationExit)
                BeginInvoke(new MethodInvoker(() => MessageBox.Show("You must have a valid session to use this feature.\n\nPlease login again to the coma tool.\r\rPlease press the Logout button to access the login page.", "Invalid ComaTool Session", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)));
        }


        void _ticketManager_LoggedOut()
        {
            
        }


        void _ticketManager_LoginFailed()
        {
            
        }


        void _ticketManager_LoggedIn()
        {
           
        }


        void _ticketManager_BytesDownloaded(long bytesDownloaded)
        {
            uniManager_BytesDownloaded(bytesDownloaded);
        }


        void _gameManager_NotifyLoggedOut(Universe universe)
        {
            if (!_applicationExit)
                BeginInvoke(new MethodInvoker(() => MessageBox.Show("Your session has expired.\n\nPlease login again to this universe.", "Session invalid - " + universe.Domain, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)));

        }


        #endregion ----- Events Callback ------


        /***********************************************************************************************************/
    }
}
