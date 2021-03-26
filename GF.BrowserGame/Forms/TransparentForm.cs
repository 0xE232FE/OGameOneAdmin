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
    public partial class TransparentForm : Form
    {
        private bool _formShowed = false;

        public TransparentForm()
        {
            InitializeComponent();
            Opacity = 0.0;
        }

        private void TransparentForm_Shown(object sender, EventArgs e)
        {
            _formShowed = true;
        }

        public void CloseForm()
        {
            try
            {
                if (!_formShowed)
                    timer1.Enabled = true;
                else
                    this.Close();
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!_formShowed)
                    this.Close();
            }
            catch { }
        }
    }
}
