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
    public partial class About : Form
    {
        public About(string appName, string appVersion, string buildDate)
        {
            InitializeComponent();
            this.Text = "About " + appName;
            labelAppName.Text = appName;
            labelAppVersion.Text = "Version: " + appVersion;
            labelBuildDate.Text = "Build date: " + buildDate;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utility.Utilities.OpenDefaultWebBrowser("http://corporate.gameforge.com/en/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utility.Utilities.OpenDefaultWebBrowser("https://board.en.ogame.gameforge.com/wcf/index.php?user/134194-vodler/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utility.Utilities.OpenDefaultWebBrowser("http://ogame.celestos.net");
        }
    }
}
