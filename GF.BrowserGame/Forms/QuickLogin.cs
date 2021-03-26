using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GF.BrowserGame.Forms
{
    internal sealed partial class QuickLogin : Form
    {
        private string _userName;

        private string _password;

        private bool _rememberCredentials;

        public bool RememberCredentials
        {
            get { return _rememberCredentials; }
            set { _rememberCredentials = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string WindowTitle
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public QuickLogin(string userName, bool enableRememberMe)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(userName))
                txtboxUserName.Text = userName;

            checkBoxRememberMe.Enabled = enableRememberMe;
        }

        private void QuickLogin_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtboxUserName.Text))
                txtboxUserName.Focus();
            else
                txtboxPassword.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtboxUserName.Text.Trim().Length == 0 || txtboxPassword.Text.Trim().Length == 0)
                MessageBox.Show("Please input your username/password", "Form incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                _userName = txtboxUserName.Text.Trim();
                _password = txtboxPassword.Text.Trim();
                _rememberCredentials = checkBoxRememberMe.Checked;
                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void txtboxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                btnLogin_Click(null, null);
            }
        }
    }
}
