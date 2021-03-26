using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GF.BrowserGame.Utility;

namespace GF.BrowserGame.Forms
{
    internal sealed partial class UnlockApplication : Form
    {
        private string _encryptedTestData;
        private bool _minimizeToTray = false;

        public UnlockApplication(string userName, string encryptedTestData, bool minimizeToTray)
        {
            InitializeComponent();
            txtboxUserName.Text = userName;
            _encryptedTestData = encryptedTestData;
            _minimizeToTray = minimizeToTray;
        }


        private void UnlockApplication_Load(object sender, EventArgs e)
        {

        }


        private void UnlockApplication_Shown(object sender, EventArgs e)
        {
            txtboxPassword.Focus();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (txtboxPassword.Text.Length == 0)
                MessageBox.Show("Please input a password.", "Password is empty!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (!string.IsNullOrEmpty(Utilities.DeSerializeObjectFromString<string>(_encryptedTestData, txtboxPassword.Text)))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Wrong password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            if (_minimizeToTray)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
                this.WindowState = FormWindowState.Minimized;
        }

        private void UnlockApplication_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void txtboxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                btnValidate_Click(null, null);
            }
        }
    }
}
