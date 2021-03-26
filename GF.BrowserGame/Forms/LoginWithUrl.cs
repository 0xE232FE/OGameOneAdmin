using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GF.BrowserGame.Schema.Serializable;

namespace GF.BrowserGame.Forms
{
    internal sealed partial class LoginWithUrl : Form
    {
        private string _retUrl = "";

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

        public LoginWithUrl()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtboxAdminToolUrl.Text.Trim().Length == 0)
                MessageBox.Show("Please input a URL.", "URL is empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                _retUrl = txtboxAdminToolUrl.Text.Trim();
                DialogResult = DialogResult.OK;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void LoginWithUrl_Shown(object sender, EventArgs e)
        {
            txtboxAdminToolUrl.Focus();
            if (Clipboard.ContainsText() && Clipboard.GetText().StartsWith("http"))
            {
                txtboxAdminToolUrl.Text = Clipboard.GetText();
                btnLogin.Focus();
            }
        }
    }
}
