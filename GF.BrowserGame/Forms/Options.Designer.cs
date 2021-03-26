namespace GF.BrowserGame.Forms
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.checkBoxMinimizeWhenAppLocked = new System.Windows.Forms.CheckBox();
            this.checkBoxSyncOGameCredentials = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.checkBoxShowAppInTaskBar = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxAppName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonApply = new System.Windows.Forms.Button();
            this.checkBoxDoNotShowTrayToolTip = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.btnDeleteSavedPasswords = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxWebBrowser = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxMinimizeWhenAppLocked
            // 
            this.checkBoxMinimizeWhenAppLocked.AutoSize = true;
            this.checkBoxMinimizeWhenAppLocked.Location = new System.Drawing.Point(14, 148);
            this.checkBoxMinimizeWhenAppLocked.Name = "checkBoxMinimizeWhenAppLocked";
            this.checkBoxMinimizeWhenAppLocked.Size = new System.Drawing.Size(337, 17);
            this.checkBoxMinimizeWhenAppLocked.TabIndex = 0;
            this.checkBoxMinimizeWhenAppLocked.Text = "Minimize to system tray when the application is locked";
            this.checkBoxMinimizeWhenAppLocked.UseVisualStyleBackColor = true;
            // 
            // checkBoxSyncOGameCredentials
            // 
            this.checkBoxSyncOGameCredentials.AutoSize = true;
            this.checkBoxSyncOGameCredentials.Checked = true;
            this.checkBoxSyncOGameCredentials.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSyncOGameCredentials.Location = new System.Drawing.Point(14, 285);
            this.checkBoxSyncOGameCredentials.Name = "checkBoxSyncOGameCredentials";
            this.checkBoxSyncOGameCredentials.Size = new System.Drawing.Size(340, 17);
            this.checkBoxSyncOGameCredentials.TabIndex = 1;
            this.checkBoxSyncOGameCredentials.Text = "Synchronize my OGame credentials to Celestos server";
            this.checkBoxSyncOGameCredentials.UseVisualStyleBackColor = true;
            this.checkBoxSyncOGameCredentials.CheckedChanged += new System.EventHandler(this.checkBoxSyncOGameCredentials_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "If unchecked, the application will appear in your system tray.";
            // 
            // checkBoxMinimizeToTray
            // 
            this.checkBoxMinimizeToTray.AutoSize = true;
            this.checkBoxMinimizeToTray.Location = new System.Drawing.Point(14, 125);
            this.checkBoxMinimizeToTray.Name = "checkBoxMinimizeToTray";
            this.checkBoxMinimizeToTray.Size = new System.Drawing.Size(295, 17);
            this.checkBoxMinimizeToTray.TabIndex = 3;
            this.checkBoxMinimizeToTray.Text = "Minimize to system tray instead of the taskbar ";
            this.checkBoxMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowAppInTaskBar
            // 
            this.checkBoxShowAppInTaskBar.AutoSize = true;
            this.checkBoxShowAppInTaskBar.Checked = true;
            this.checkBoxShowAppInTaskBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowAppInTaskBar.Location = new System.Drawing.Point(14, 88);
            this.checkBoxShowAppInTaskBar.Name = "checkBoxShowAppInTaskBar";
            this.checkBoxShowAppInTaskBar.Size = new System.Drawing.Size(205, 17);
            this.checkBoxShowAppInTaskBar.TabIndex = 4;
            this.checkBoxShowAppInTaskBar.Text = "Show application in the taskbar";
            this.checkBoxShowAppInTaskBar.UseVisualStyleBackColor = true;
            this.checkBoxShowAppInTaskBar.CheckedChanged += new System.EventHandler(this.checkBoxShowAppInTaskBar_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 305);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(317, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "We use the same secure methods as banks to keep your data safe.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(369, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "You data is always encrypted no matter if it is stored locally or on our server.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(504, 39);
            this.label4.TabIndex = 7;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Window title:";
            // 
            // txtBoxAppName
            // 
            this.txtBoxAppName.Location = new System.Drawing.Point(98, 55);
            this.txtBoxAppName.Name = "txtBoxAppName";
            this.txtBoxAppName.Size = new System.Drawing.Size(178, 21);
            this.txtBoxAppName.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 41);
            this.panel1.TabIndex = 36;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(11, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(190, 14);
            this.label6.TabIndex = 21;
            this.label6.Text = "Personalize the application:";
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(309, 382);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(100, 23);
            this.buttonApply.TabIndex = 37;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // checkBoxDoNotShowTrayToolTip
            // 
            this.checkBoxDoNotShowTrayToolTip.AutoSize = true;
            this.checkBoxDoNotShowTrayToolTip.Location = new System.Drawing.Point(14, 171);
            this.checkBoxDoNotShowTrayToolTip.Name = "checkBoxDoNotShowTrayToolTip";
            this.checkBoxDoNotShowTrayToolTip.Size = new System.Drawing.Size(423, 17);
            this.checkBoxDoNotShowTrayToolTip.TabIndex = 38;
            this.checkBoxDoNotShowTrayToolTip.Text = "Do not show tool tip when the application is minimized to system tray";
            this.checkBoxDoNotShowTrayToolTip.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(415, 382);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonCancel.TabIndex = 39;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // btnDeleteSavedPasswords
            // 
            this.btnDeleteSavedPasswords.Enabled = false;
            this.btnDeleteSavedPasswords.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSavedPasswords.Location = new System.Drawing.Point(34, 334);
            this.btnDeleteSavedPasswords.Name = "btnDeleteSavedPasswords";
            this.btnDeleteSavedPasswords.Size = new System.Drawing.Size(240, 25);
            this.btnDeleteSavedPasswords.TabIndex = 40;
            this.btnDeleteSavedPasswords.Text = "Delete all saved passwords from our server";
            this.btnDeleteSavedPasswords.UseVisualStyleBackColor = true;
            this.btnDeleteSavedPasswords.Click += new System.EventHandler(this.btnDeleteSavedPasswords_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(139, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "External Web Browser:";
            // 
            // comboBoxWebBrowser
            // 
            this.comboBoxWebBrowser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWebBrowser.FormattingEnabled = true;
            this.comboBoxWebBrowser.Items.AddRange(new object[] {
            "Internet Explorer",
            "Firefox",
            "Chrome"});
            this.comboBoxWebBrowser.Location = new System.Drawing.Point(155, 199);
            this.comboBoxWebBrowser.Name = "comboBoxWebBrowser";
            this.comboBoxWebBrowser.Size = new System.Drawing.Size(144, 21);
            this.comboBoxWebBrowser.TabIndex = 42;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 417);
            this.ControlBox = false;
            this.Controls.Add(this.comboBoxWebBrowser);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnDeleteSavedPasswords);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkBoxDoNotShowTrayToolTip);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtBoxAppName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxShowAppInTaskBar);
            this.Controls.Add(this.checkBoxMinimizeToTray);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxSyncOGameCredentials);
            this.Controls.Add(this.checkBoxMinimizeWhenAppLocked);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Options";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxMinimizeWhenAppLocked;
        private System.Windows.Forms.CheckBox checkBoxSyncOGameCredentials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxMinimizeToTray;
        private System.Windows.Forms.CheckBox checkBoxShowAppInTaskBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxAppName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.CheckBox checkBoxDoNotShowTrayToolTip;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button btnDeleteSavedPasswords;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxWebBrowser;
    }
}