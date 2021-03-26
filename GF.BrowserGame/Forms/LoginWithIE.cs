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

namespace GF.BrowserGame.Forms
{
    internal sealed partial class LoginWithIE : Form
    {
        private GameManager _gameManager;
        private Uri _loginUri;
        private Universe _universe;
        private string _retUrl = "";
        private WaitingForm _waitingForm;
        private bool _waitingFormShowed = false;
        private bool _firstNavigationCompleted = false;
        private Uri _urlNavigated = null;
        private bool _urlNavigatedVerified = false;

        public string RetUrl
        {
            get { return _retUrl; }
            set { _retUrl = value; }
        }

        public string WindowTitle
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public LoginWithIE(GameManager gameManager, Uri loginUri, Universe universe)
        {
            InitializeComponent();
            _gameManager = gameManager;
            _loginUri = loginUri;
            _universe = universe;
            _waitingForm = new WaitingForm();
        }

        private void LoginWithIE_Shown(object sender, EventArgs e)
        {
            //webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Navigate(_loginUri);
            timer1.Enabled = true;
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            _urlNavigated = e.Url;
            _urlNavigatedVerified = false;

            if (_urlNavigated.PathAndQuery.StartsWith("/game/index.php?page=overview"))
            {
                _urlNavigatedVerified = true;
                bool isUniverseValid = false;

                if (_urlNavigated.AbsoluteUri.ToLower().StartsWith("http://" + _universe.Domain.ToLower()))
                    isUniverseValid = true;

                if (!isUniverseValid)
                {
                    webBrowser.Stop();
                    MessageBox.Show("You have not logged into the correct universe.\n\nPlease login again to " + _universe.Name, "Wrong Universe", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    webBrowser.Navigate(_loginUri);
                }
                else
                {
                    _retUrl = _urlNavigated.AbsoluteUri;
                    DialogResult = DialogResult.OK;
                }
            }
            else if (!_urlNavigated.AbsoluteUri.ToLower().Contains(_loginUri.AbsoluteUri.ToLower()))
            {
                try
                {
                    if (_urlNavigated.AbsoluteUri.ToLower().Contains(_universe.Domain.Substring(_universe.Domain.IndexOf(".") + 1)))
                        return;
                }
                catch { }

                if (_urlNavigated.AbsoluteUri.StartsWith("http"))
                {
                    _urlNavigatedVerified = true;
                    webBrowser.Stop();
                    _gameManager.CreateApplicationExceptionLogAsync("Navigation failed in LoginWithIE Form", new Exception("Login url = " + _loginUri + " - Url Navigated to = " + _urlNavigated.AbsoluteUri));

                    if (MessageBox.Show("You are only allowed to go to this page: " + _urlNavigated.AbsoluteUri + "\n\nYou can only navigate to the community corresponding to the universe you are trying to login to: " + _universe.Domain + " or close the window.", "Info", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        DialogResult = DialogResult.Cancel;
                    else
                        webBrowser.Navigate(_loginUri);
                }
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _firstNavigationCompleted = true;
            _waitingForm.SetShowHide(false);
            _urlNavigated = e.Url;

            if (_urlNavigated != null && !_urlNavigatedVerified)
            {
                if (!_urlNavigated.AbsoluteUri.ToLower().Contains(_loginUri.AbsoluteUri.ToLower()))
                {
                    try
                    {
                        if (_urlNavigated.AbsoluteUri.ToLower().Contains(_universe.Domain.Substring(_universe.Domain.IndexOf(".") + 1)))
                            return;
                    }
                    catch { }

                    _gameManager.CreateApplicationExceptionLogAsync("Navigation failed in LoginWithIE Form", new Exception("Login url = " + _loginUri + " - Url Navigated to = " + _urlNavigated.AbsoluteUri));

                    if (!_urlNavigated.AbsoluteUri.StartsWith("http"))
                    {
                        if (MessageBox.Show("Internet Explorer could not load " + _loginUri.AbsoluteUri + " properly.\n\nIf you keep getting this error, make sure you have the latest version of Internet Explorer installed.", "Info", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            DialogResult = DialogResult.Cancel;
                        else
                            webBrowser.Navigate(_loginUri);
                    }
                    else
                    {
                        if (MessageBox.Show("You are only allowed to go to this page: " + _urlNavigated.AbsoluteUri + "\n\nYou can only navigate to the community corresponding to the universe you are trying to login to: " + _universe.Domain + " or close the window.", "Info", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            DialogResult = DialogResult.Cancel;
                        else
                            webBrowser.Navigate(_loginUri);
                    }
                }
            }
        }

        private void GameLoginBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (!_waitingFormShowed && !_firstNavigationCompleted)
            {
                _waitingFormShowed = true;
                _waitingForm.SetShowHide(true);
                DialogResult result = _waitingForm.ShowDialog();
            }
        }
    }
}
