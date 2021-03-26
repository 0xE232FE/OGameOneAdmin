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
    public partial class Options : Form
    {
        private UserAppConfig _userAppConfig;
        private bool _settingsLoaded = false;

        public Options(UserAppConfig userAppConfig)
        {
            InitializeComponent();
            _userAppConfig = userAppConfig;
            txtBoxAppName.Text = userAppConfig.AppName;
            checkBoxShowAppInTaskBar.Checked = userAppConfig.ShowAppInTaskBar;
            checkBoxMinimizeWhenAppLocked.Checked = userAppConfig.MinimizeWhenAppLocked;
            checkBoxMinimizeToTray.Checked = userAppConfig.MinimizeToTray;
            checkBoxSyncOGameCredentials.Checked = userAppConfig.SynchronizeOGameCredentials;
            checkBoxDoNotShowTrayToolTip.Checked = userAppConfig.DoNotShowMinimizeToolTip;
            try
            {
                comboBoxWebBrowser.SelectedItem = userAppConfig.ExternalWebBrowser;
                if (comboBoxWebBrowser.SelectedItem == null)
                    throw new Exception("Invalid browser selection");
            }
            catch
            {
                comboBoxWebBrowser.SelectedItem = "Internet Explorer";
                userAppConfig.ExternalWebBrowser = "Internet Explorer";
            }
            _settingsLoaded = true;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBoxAppName.Text.Trim()) && txtBoxAppName.Text.Trim().Length < 25)
                _userAppConfig.AppName = txtBoxAppName.Text;
            else
                _userAppConfig.AppName = Application.ProductName;
            _userAppConfig.MinimizeWhenAppLocked = checkBoxMinimizeWhenAppLocked.Checked;
            _userAppConfig.MinimizeToTray = checkBoxMinimizeToTray.Checked;
            _userAppConfig.ShowAppInTaskBar = checkBoxShowAppInTaskBar.Checked;
            _userAppConfig.SynchronizeOGameCredentials = checkBoxSyncOGameCredentials.Checked;
            _userAppConfig.DoNotShowMinimizeToolTip = checkBoxDoNotShowTrayToolTip.Checked;
            _userAppConfig.ExternalWebBrowser = comboBoxWebBrowser.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void checkBoxShowAppInTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowAppInTaskBar.Checked)
            {
                checkBoxMinimizeToTray.Enabled = true;
                checkBoxMinimizeToTray.Checked = false;
            }
            else
            {
                checkBoxMinimizeToTray.Enabled = false;
                checkBoxMinimizeToTray.Checked = true;
            }
        }

        private void btnDeleteSavedPasswords_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxSyncOGameCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (_settingsLoaded && checkBoxSyncOGameCredentials.Checked)
            {
                MessageBox.Show("Be aware that your credentials will be synchronized next time you login into your universe(s).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
