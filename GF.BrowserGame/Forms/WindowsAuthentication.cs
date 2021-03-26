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
    public partial class WindowsAuthentication : Form
    {
        public string userName;
        public string password;

        public WindowsAuthentication(string serverName)
        {
            InitializeComponent();
            this.Text += serverName;
            label1.Text = string.Format(label1.Text, serverName);
        }

        private void WindowsAuthentication_Shown(object sender, EventArgs e)
        {
            txtboxUserName.Focus();
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
            userName = txtboxUserName.Text.Trim();
            password = txtboxPassword.Text.Trim();

            if (userName.Length < 1 || password.Length < 1)
                MessageBox.Show("You must provide a user name and password.", "Fields required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
