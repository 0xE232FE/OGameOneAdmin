using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using GF.BrowserGame.Schema.Serializable;
using GF.BrowserGame;
using GF.BrowserGame.Forms;
using Microsoft.Win32;
using GF.BrowserGame.Static;

namespace OGameOneAdmin.Controls
{
    public partial class ComaToolWebBrowserControl : UserControl
    {
        private object _locker = new object();
        private GameManager _gameManager;
        private TicketManager _ticketManager;
        private ComaToolControl _comaToolControl;
        private Uri _siteUri;
        private string _retUrl = "";
        private WaitingForm _waitingForm;
        private bool _waitingFormShowed = false;
        private bool _firstNavigationCompleted = false;
        private Uri _urlNavigated = null;
        private bool _urlNavigatedVerified = false;
        private int _backgroundWorkerRunning = 0;
        private bool _isInProgressBarVisible = false;

        public ComaToolWebBrowserControl(GameManager gameManager, ComaToolControl comaToolControl)
        {
            InitializeComponent();
            _gameManager = gameManager;
            _ticketManager = _gameManager.GetTicketManager();
            _comaToolControl = comaToolControl;
        }

        public void LoadControl(Uri siteUri)
        {
            UseIE8DocMode();
            webBrowser.ScriptErrorsSuppressed = true;
            _siteUri = siteUri;
            //webBrowser.Navigate("about:blank");
            txtBoxUrl.Text = siteUri.AbsoluteUri;
            webBrowser.Navigate(siteUri);
        }

        public void UseIE8DocMode()
        {
            //[(HKEY_CURRENT_USER or HKEY_LOCAL_MACHINE)\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION]
            //"MyApplication.exe" = dword 8000 (Hex: 0x1F40)
            //7000: Pages containing standards-based <!DOCTYPE> directives are displayed in IE7 mode.
            //8000: Pages containing standards-based <!DOCTYPE> directives are displayed in IE8 mode
            //8888: Pages are always displayed in IE8 mode, regardless of the <!DOCTYPE> directive. (This bypasses the exceptions listed earlier.)
            //9000: Use IE9 settings!
            //9999: Force IE9 
            //11000 (0x2AF8) Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 mode.
            //11001(0x2AF9) Internet Explorer 11.Webpages are displayed in IE11 Standards mode, regardless of the!DOCTYPE directive.
            RegistryKey key = null;
            try
            {
                key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                if (key == null)
                    key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION");
                key.SetValue(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName, 11001, RegistryValueKind.DWord);
                key.Close();
            }
            catch (Exception)
            {
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (_firstNavigationCompleted)
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
            _urlNavigated = e.Url;
            _urlNavigatedVerified = false;

            if (_urlNavigated.AbsoluteUri.Contains("coma.gameforge.com") || _urlNavigated.AbsoluteUri.Contains("ogame") || _urlNavigated.AbsoluteUri.Contains("about:blank"))
            {
                _urlNavigatedVerified = true;
                txtBoxUrl.Text = _urlNavigated.AbsoluteUri;
            }
            else
            {
                webBrowser.Stop();
                MessageBox.Show("You are not allowed to visit this website.", "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                webBrowser.Navigate(_siteUri);
            }

            if (webBrowser.DocumentText.Contains("index.php?page=login"))
                btnLogout_Click(null, null);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!_firstNavigationCompleted)
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                _firstNavigationCompleted = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            webBrowser.Stop();
            //webBrowser.Navigate(_siteUri);
            webBrowser.Navigate(txtBoxUrl.Text);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            bool success = false;

            var worker = new BackgroundWorker();
            worker.DoWork += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.START_WORK);
                success = _ticketManager.Logout();
            };
            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE.COMPLETE_WORK);
                _comaToolControl.OnNotifyLoggedOut();
            };
            worker.RunWorkerAsync();
        }

        private void ShowHideInProgressBar()
        {
            lock (_locker)
            {
                if (_backgroundWorkerRunning == 0 && _isInProgressBarVisible)
                {
                    Invoke(new MethodInvoker(() => btnHome.Enabled = true));
                    Invoke(new MethodInvoker(() => pictureBoxInProgress.Visible = _isInProgressBarVisible = false));
                }
                else if (_backgroundWorkerRunning > 0 && !_isInProgressBarVisible)
                {
                    Invoke(new MethodInvoker(() => btnHome.Enabled = false));
                    Invoke(new MethodInvoker(() => pictureBoxInProgress.Visible = _isInProgressBarVisible = true));
                }
            }
        }

        void OnBackgroundWorkerNotification(Enums.BACKGROUNDWORKER_STATE bgwState)
        {
            lock (_locker)
            {
                if (_backgroundWorkerRunning < 0)
                    _backgroundWorkerRunning = 0;

                if (bgwState.Equals(Enums.BACKGROUNDWORKER_STATE.START_WORK))
                    _backgroundWorkerRunning++;
                else
                    _backgroundWorkerRunning--;

                ShowHideInProgressBar();
            }
        }
    }
}
