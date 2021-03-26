using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GF.BrowserGame.Static;
using GF.BrowserGame;
using GF.BrowserGame.Schema.Serializable;

namespace OGameOneAdmin.Controls
{
    public partial class ComaToolLogin : UserControl
    {
        private GameManager _gameManager;
        private TicketManager _ticketManager;
        private ComaToolControl _comaToolControl;

        public ComaToolLogin(GameManager gameManager, ComaToolControl comaToolControl)
        {
            InitializeComponent();
            _gameManager = gameManager;
            _ticketManager = _gameManager.GetTicketManager();
            _comaToolControl = comaToolControl;
        }

        public void LoadControl(bool firstLoad)
        {
            // Load text box if credentials
            if(_ticketManager.IsCredentialsExist())
            {
                ComaToolCredentials credentials = _ticketManager.GetCredentials();
                txtboxUserName.Text = credentials.UserName;
                txtboxPassword.Text = credentials.Password;
            }

            if (firstLoad)
                CheckSessionValidityOnFirstLoad();
        }

        private void txtboxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                if (txtboxPassword.Text.Length > 0)
                    btnLogin_Click(null, null);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            txtboxUserName.Text = txtboxUserName.Text.Trim();
            txtboxPassword.Text = txtboxPassword.Text.Trim();

            if (txtboxUserName.Text.Trim().Length < 1 || txtboxPassword.Text.Trim().Length < 1)
                MessageBox.Show("You must provide a email address and password.", "Fields required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                Login();
            }
        }

        private void Login()
        {
            bool success = false;
            string errorMessage = "";
            labelStatus.Text = "Authenticating...";

            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                success = _ticketManager.OnLogin(txtboxUserName.Text.Trim(), txtboxPassword.Text, out errorMessage);
            };

            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                btnLogin.Visible = true;
                pictureBoxStatus.Visible = false;
                labelStatus.Visible = false;

                if (success)
                {
                    //Save credentials
                    if (checkBoxRememberMe.Checked)
                        _ticketManager.SaveCredentials(txtboxUserName.Text.Trim(), txtboxPassword.Text);
                    else
                    {
                        _ticketManager.SaveCredentialsUserName("");
                        _ticketManager.SaveCredentialsPassword("");
                    }
                    //CT Control
                    _comaToolControl.OnNotifyLoggedIn();
                }
                else
                {
                    MessageBox.Show("Invalid email address or password", "Error");
                    _ticketManager.SaveCredentialsUserName("");
                    _ticketManager.SaveCredentialsPassword("");
                }
            };

            btnLogin.Visible = false;
            pictureBoxStatus.Visible = true;
            labelStatus.Visible = true;
            worker.RunWorkerAsync();
        }

        private void CheckSessionValidityOnFirstLoad()
        {
            bool success = false;
            labelStatus.Text = "Checking for existing session...";

            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                success = _ticketManager.IsSessionValid(false);
                if (success)
                {
                    _ticketManager.RetrieveDateFormatId();
                    _ticketManager.RetrieveComaToolCommunityList();
                    _ticketManager.SetInternetExplorerCookies();
                }
            };

            worker.RunWorkerCompleted += (workerSender, workerEvent) =>
            {
                btnLogin.Visible = true;
                pictureBoxStatus.Visible = false;
                labelStatus.Visible = false;

                if (success)
                {
                    _comaToolControl.OnNotifyLoggedIn();
                }
            };

            btnLogin.Visible = false;
            pictureBoxStatus.Visible = true;
            labelStatus.Visible = true;
            worker.RunWorkerAsync();
        }
    }
}
