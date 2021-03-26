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
    public partial class SplashScreen : Form
    {
        private bool _formShowing = false;
        private int _step = 0;

        public SplashScreen()
        {
            InitializeComponent();
            Opacity = 0.0;
            label1.Parent = pictureBox1;
            label1.Visible = true;
        }

        /// <summary>
        /// Override the OnPaint method to change the border color of form.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
                    d = 1000.0 / 30000.0;//d = 1000.0 / 100000.0;
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
                    d = 1000.0 / 20000.0; //d = 1000.0 / 50000.0;
                    if (Opacity - d <= 0)
                    {
                        Opacity = 0;
                        timer1.Stop();
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

        private void SplashScreen_Shown(object sender, EventArgs e)
        {
            ChangeStyle();
            Refresh();
        }

        private void SplashScreen_Activated(object sender, EventArgs e)
        {
            Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string dots = "";
                if (_step == 4)
                    _step = 0;

                _step++;

                for (int i = 0; i < _step; i++)
                {
                    dots += ".";
                }

                label1.Text = "Loading" + dots;
            }
            catch { }
        }
    }
}
