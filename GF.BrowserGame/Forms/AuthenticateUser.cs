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

namespace GF.BrowserGame.Forms
{
    internal sealed partial class AuthenticateUser : Form
    {
        private GameManager _gameManager;

        public AuthenticateUser(GameManager gameManager)
        {
            InitializeComponent();
            _gameManager = gameManager;
        }

        private void AuthenticateUser_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_gameManager.AppConfig.LastLoginName))
            {
                txtboxUserName.Text = _gameManager.AppConfig.LastLoginName;
                txtboxPassword.Focus();
            }
            else
                txtboxUserName.Focus();
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("We recommend you to tick this box if this is not your personal computer or if you are temporarilly using it.\r\n\r\nIf checked, the state of the application as well as any data downloaded from celestos's server will not be stored locally.\r\n\r\nIf unchecked, the state of the application will be encrypted and securely stored locally so it can be restored next time you load the application.", "Information", MessageBoxButtons.OK);
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
                MessageBox.Show("You must provide a login name and password.", "Fields required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                ValidateCredentials();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ValidateCredentials()
        {
            Guid? userId = null;
            string userName = "";
            string password = "";
            string errorMessage = "";
            bool closeApplication = false;
            bool getNewCredentials = false;

            using (var transparentForm = new TransparentForm())
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (sender, e) =>
                {
                    userName = txtboxUserName.Text.Trim();
                    password = txtboxPassword.Text.Trim();
                    _gameManager.WebServiceCall.UpdateSoapHeader(userName, password);
                    userId = _gameManager.WebServiceCall.IsUserAccountValid(true);
                };

                worker.RunWorkerCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        userId = null;
                        _gameManager.ParseWebServiceResponseException(e.Error, true, "AuthenticateUser.ValidateCredentials() has failed", false, out getNewCredentials, out closeApplication, out errorMessage);
                    }
                    else if (!userId.HasValue)
                        errorMessage = "A technical error occurred, please contact an administrator and quote the error code #0004.";

                    transparentForm.Close();
                };

                btnLogin.Visible = false;
                btnCancel.Visible = false;
                pictureBoxStatus.Visible = true;
                labelStatus.Visible = true;

                worker.RunWorkerAsync();
                DialogResult result = transparentForm.ShowDialog();

                pictureBoxStatus.Visible = false;
                labelStatus.Visible = false;
                btnLogin.Visible = true;
                btnCancel.Visible = true;

                if (!userId.HasValue && !string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show(errorMessage);
                else if (userId.HasValue)
                {
                    _gameManager.AppConfig.IsPublicComputer = checkBoxPublicComputer.Checked;

                    if (!_gameManager.AppConfig.IsPublicComputer)
                        _gameManager.AppConfig.LastLoginName = userName;
                    else
                        _gameManager.AppConfig.LastLoginName = "";

                    _gameManager.UserId = userId.Value;
                    DialogResult = DialogResult.OK;
                }
                else
                    MessageBox.Show("A technical error occurred, please contact an administrator and quote the error code #0006.");
            }
        }
    }
}
