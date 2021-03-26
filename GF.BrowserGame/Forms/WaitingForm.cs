using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GF.BrowserGame.Forms
{
    public partial class WaitingForm : Form
    {
        private bool _formShowing = false;

        public WaitingForm()
        {
            InitializeComponent();
            Opacity = 0.0;
        }

        /// <summary>
        /// Override the OnPaint method to change the border color of form.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        public void SetTitle(string title)
        {
            labelTitle.Text = title;
        }

        public void SetShowHide(bool bformShow)
        {
            _formShowing = bformShow;
            if (!_formShowing)
            {
                ChangeStyle();
                Refresh();
            }
        }

        private void ChangeStyle()
        {
            double d = 1000.0 / 100000.0;

            while (true)
            {
                if (_formShowing)
                {
                    d = 1000.0 / 50000.0;//d = 1000.0 / 100000.0;
                    if (Opacity + d >= 1.0)
                    {
                        Opacity = 1.0;
                        return;
                    }
                    else
                    {
                        Opacity += d;
                        Refresh();
                    }
                }
                else
                {
                    d = 1000.0 / 25000.0; //d = 1000.0 / 50000.0;
                    if (Opacity - d <= 0)
                    {
                        Opacity = 0;
                        Close();
                        return;
                    }
                    else
                    {
                        Opacity -= d;
                        Refresh();
                    }
                }
            }
        }

        private void WaitingForm_Shown(object sender, EventArgs e)
        {
            ChangeStyle();
            Refresh();
        }

        private void WaitingForm_Activated(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
