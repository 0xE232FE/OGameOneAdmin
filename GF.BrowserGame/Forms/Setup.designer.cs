namespace GF.BrowserGame.Forms
{
    partial class Setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup));
            this.listViewUniverseList = new System.Windows.Forms.ListView();
            this.columnHeaderUniNumber = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderUniverseName = new System.Windows.Forms.ColumnHeader();
            this.tabControlSetup = new System.Windows.Forms.TabControl();
            this.tabPageMyPassword = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelMasterPassword = new System.Windows.Forms.Label();
            this.linkLabelEditPassword = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtboxRepeatPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtboxPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageMyUniverse = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxCommunityList = new System.Windows.Forms.ComboBox();
            this.linkLabelSelectNone = new System.Windows.Forms.LinkLabel();
            this.linkLabelSelectAll = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPageMyAccount = new System.Windows.Forms.TabPage();
            this.labelStatus = new System.Windows.Forms.Label();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CelestosPasswordtextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CelestosLogintextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.tabControlSetup.SuspendLayout();
            this.tabPageMyPassword.SuspendLayout();
            this.tabPageMyUniverse.SuspendLayout();
            this.tabPageMyAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewUniverseList
            // 
            this.listViewUniverseList.CheckBoxes = true;
            this.listViewUniverseList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUniNumber,
            this.columnHeaderUniverseName});
            this.listViewUniverseList.FullRowSelect = true;
            this.listViewUniverseList.GridLines = true;
            this.listViewUniverseList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewUniverseList.HideSelection = false;
            this.listViewUniverseList.Location = new System.Drawing.Point(10, 37);
            this.listViewUniverseList.MultiSelect = false;
            this.listViewUniverseList.Name = "listViewUniverseList";
            this.listViewUniverseList.Size = new System.Drawing.Size(492, 293);
            this.listViewUniverseList.TabIndex = 0;
            this.listViewUniverseList.UseCompatibleStateImageBehavior = false;
            this.listViewUniverseList.View = System.Windows.Forms.View.Details;
            this.listViewUniverseList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewUniverseList_ItemChecked);
            this.listViewUniverseList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewUniverseList_KeyDown);
            this.listViewUniverseList.Click += new System.EventHandler(this.listViewUniverseList_Click);
            // 
            // columnHeaderUniNumber
            // 
            this.columnHeaderUniNumber.Text = "Uni Number";
            this.columnHeaderUniNumber.Width = 96;
            // 
            // columnHeaderUniverseName
            // 
            this.columnHeaderUniverseName.Text = "Universe";
            this.columnHeaderUniverseName.Width = 301;
            // 
            // tabControlSetup
            // 
            this.tabControlSetup.Controls.Add(this.tabPageMyPassword);
            this.tabControlSetup.Controls.Add(this.tabPageMyUniverse);
            this.tabControlSetup.Controls.Add(this.tabPageMyAccount);
            this.tabControlSetup.Location = new System.Drawing.Point(14, 12);
            this.tabControlSetup.Name = "tabControlSetup";
            this.tabControlSetup.Padding = new System.Drawing.Point(10, 5);
            this.tabControlSetup.SelectedIndex = 0;
            this.tabControlSetup.Size = new System.Drawing.Size(524, 386);
            this.tabControlSetup.TabIndex = 1;
            this.tabControlSetup.SelectedIndexChanged += new System.EventHandler(this.tabControlSetup_SelectedIndexChanged);
            // 
            // tabPageMyPassword
            // 
            this.tabPageMyPassword.Controls.Add(this.label10);
            this.tabPageMyPassword.Controls.Add(this.label9);
            this.tabPageMyPassword.Controls.Add(this.labelMasterPassword);
            this.tabPageMyPassword.Controls.Add(this.linkLabelEditPassword);
            this.tabPageMyPassword.Controls.Add(this.label3);
            this.tabPageMyPassword.Controls.Add(this.txtboxRepeatPassword);
            this.tabPageMyPassword.Controls.Add(this.label2);
            this.tabPageMyPassword.Controls.Add(this.txtboxPassword);
            this.tabPageMyPassword.Controls.Add(this.label1);
            this.tabPageMyPassword.Location = new System.Drawing.Point(4, 26);
            this.tabPageMyPassword.Name = "tabPageMyPassword";
            this.tabPageMyPassword.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMyPassword.Size = new System.Drawing.Size(516, 356);
            this.tabPageMyPassword.TabIndex = 0;
            this.tabPageMyPassword.Text = "Master Password Setup";
            this.tabPageMyPassword.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(10, 290);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(492, 48);
            this.label10.TabIndex = 16;
            this.label10.Text = "Please make sure you remember the Master Password you have set. If you forget you" +
                "r Master Password, you will be unable to access this program and any information" +
                " protected by it.";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(10, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(492, 38);
            this.label9.TabIndex = 15;
            this.label9.Text = "In other words, any information saved locally such as credentials will be encrypt" +
                "ed using your master password. Without it, sensitive information cannot be decry" +
                "pted.";
            // 
            // labelMasterPassword
            // 
            this.labelMasterPassword.Location = new System.Drawing.Point(10, 18);
            this.labelMasterPassword.Name = "labelMasterPassword";
            this.labelMasterPassword.Size = new System.Drawing.Size(492, 48);
            this.labelMasterPassword.TabIndex = 14;
            this.labelMasterPassword.Text = resources.GetString("labelMasterPassword.Text");
            // 
            // linkLabelEditPassword
            // 
            this.linkLabelEditPassword.AutoSize = true;
            this.linkLabelEditPassword.Location = new System.Drawing.Point(330, 157);
            this.linkLabelEditPassword.Name = "linkLabelEditPassword";
            this.linkLabelEditPassword.Size = new System.Drawing.Size(28, 13);
            this.linkLabelEditPassword.TabIndex = 13;
            this.linkLabelEditPassword.TabStop = true;
            this.linkLabelEditPassword.Text = "Edit";
            this.linkLabelEditPassword.Visible = false;
            this.linkLabelEditPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEditPassword_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Choose a master password:";
            // 
            // txtboxRepeatPassword
            // 
            this.txtboxRepeatPassword.Location = new System.Drawing.Point(133, 187);
            this.txtboxRepeatPassword.Name = "txtboxRepeatPassword";
            this.txtboxRepeatPassword.PasswordChar = '*';
            this.txtboxRepeatPassword.Size = new System.Drawing.Size(179, 21);
            this.txtboxRepeatPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Repeat Password:";
            // 
            // txtboxPassword
            // 
            this.txtboxPassword.Location = new System.Drawing.Point(133, 154);
            this.txtboxPassword.Name = "txtboxPassword";
            this.txtboxPassword.PasswordChar = '*';
            this.txtboxPassword.Size = new System.Drawing.Size(179, 21);
            this.txtboxPassword.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password:";
            // 
            // tabPageMyUniverse
            // 
            this.tabPageMyUniverse.Controls.Add(this.label5);
            this.tabPageMyUniverse.Controls.Add(this.comboBoxCommunityList);
            this.tabPageMyUniverse.Controls.Add(this.linkLabelSelectNone);
            this.tabPageMyUniverse.Controls.Add(this.linkLabelSelectAll);
            this.tabPageMyUniverse.Controls.Add(this.label4);
            this.tabPageMyUniverse.Controls.Add(this.listViewUniverseList);
            this.tabPageMyUniverse.Location = new System.Drawing.Point(4, 26);
            this.tabPageMyUniverse.Name = "tabPageMyUniverse";
            this.tabPageMyUniverse.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMyUniverse.Size = new System.Drawing.Size(516, 356);
            this.tabPageMyUniverse.TabIndex = 1;
            this.tabPageMyUniverse.Text = "My Universe(s)";
            this.tabPageMyUniverse.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(331, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Show:";
            // 
            // comboBoxCommunityList
            // 
            this.comboBoxCommunityList.FormattingEnabled = true;
            this.comboBoxCommunityList.Location = new System.Drawing.Point(381, 10);
            this.comboBoxCommunityList.Name = "comboBoxCommunityList";
            this.comboBoxCommunityList.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCommunityList.TabIndex = 14;
            this.comboBoxCommunityList.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommunityList_SelectedIndexChanged);
            // 
            // linkLabelSelectNone
            // 
            this.linkLabelSelectNone.AutoSize = true;
            this.linkLabelSelectNone.Location = new System.Drawing.Point(73, 333);
            this.linkLabelSelectNone.Name = "linkLabelSelectNone";
            this.linkLabelSelectNone.Size = new System.Drawing.Size(94, 13);
            this.linkLabelSelectNone.TabIndex = 13;
            this.linkLabelSelectNone.TabStop = true;
            this.linkLabelSelectNone.Text = "Clear Selection";
            this.linkLabelSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSelectNone_LinkClicked);
            // 
            // linkLabelSelectAll
            // 
            this.linkLabelSelectAll.AutoSize = true;
            this.linkLabelSelectAll.Location = new System.Drawing.Point(7, 333);
            this.linkLabelSelectAll.Name = "linkLabelSelectAll";
            this.linkLabelSelectAll.Size = new System.Drawing.Size(60, 13);
            this.linkLabelSelectAll.TabIndex = 12;
            this.linkLabelSelectAll.TabStop = true;
            this.linkLabelSelectAll.Text = "Select All";
            this.linkLabelSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSelectAll_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Select your universe(s):";
            // 
            // tabPageMyAccount
            // 
            this.tabPageMyAccount.Controls.Add(this.labelStatus);
            this.tabPageMyAccount.Controls.Add(this.pictureBoxStatus);
            this.tabPageMyAccount.Controls.Add(this.label13);
            this.tabPageMyAccount.Controls.Add(this.label12);
            this.tabPageMyAccount.Controls.Add(this.label11);
            this.tabPageMyAccount.Controls.Add(this.label6);
            this.tabPageMyAccount.Controls.Add(this.CelestosPasswordtextBox);
            this.tabPageMyAccount.Controls.Add(this.label7);
            this.tabPageMyAccount.Controls.Add(this.CelestosLogintextBox);
            this.tabPageMyAccount.Controls.Add(this.label8);
            this.tabPageMyAccount.Location = new System.Drawing.Point(4, 26);
            this.tabPageMyAccount.Name = "tabPageMyAccount";
            this.tabPageMyAccount.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMyAccount.Size = new System.Drawing.Size(516, 356);
            this.tabPageMyAccount.TabIndex = 2;
            this.tabPageMyAccount.Text = "Account Details";
            this.tabPageMyAccount.UseVisualStyleBackColor = true;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(163, 234);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(53, 13);
            this.labelStatus.TabIndex = 19;
            this.labelStatus.Text = "(Status)";
            this.labelStatus.Visible = false;
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.Image = global::GF.BrowserGame.Properties.Resources.ajax_loader_small;
            this.pictureBoxStatus.Location = new System.Drawing.Point(129, 226);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(28, 28);
            this.pictureBoxStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxStatus.TabIndex = 18;
            this.pictureBoxStatus.TabStop = false;
            this.pictureBoxStatus.Visible = false;
            this.pictureBoxStatus.WaitOnLoad = true;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(504, 33);
            this.label13.TabIndex = 17;
            this.label13.Text = "Please provide below, your login name and password from celestos.net in order lin" +
                "k this application to your profile.";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(504, 24);
            this.label12.TabIndex = 16;
            this.label12.Text = "If you do not have a valid account, you will not be able to complete this setup.";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(504, 24);
            this.label11.TabIndex = 15;
            this.label11.Text = "In order to use this application, you must have obtained an account at celestos.n" +
                "et";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(339, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Input your credentials from your celestos account:";
            // 
            // CelestosPasswordtextBox
            // 
            this.CelestosPasswordtextBox.Location = new System.Drawing.Point(129, 189);
            this.CelestosPasswordtextBox.Name = "CelestosPasswordtextBox";
            this.CelestosPasswordtextBox.PasswordChar = '*';
            this.CelestosPasswordtextBox.Size = new System.Drawing.Size(179, 21);
            this.CelestosPasswordtextBox.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Password:";
            // 
            // CelestosLogintextBox
            // 
            this.CelestosLogintextBox.Location = new System.Drawing.Point(129, 156);
            this.CelestosLogintextBox.Name = "CelestosLogintextBox";
            this.CelestosLogintextBox.Size = new System.Drawing.Size(179, 21);
            this.CelestosLogintextBox.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Login Name:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(426, 404);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel Setup";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(307, 404);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(112, 23);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "Set Password";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 439);
            this.ControlBox = false;
            this.Controls.Add(this.tabControlSetup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Setup";
            this.Shown += new System.EventHandler(this.Setup_Shown);
            this.tabControlSetup.ResumeLayout(false);
            this.tabPageMyPassword.ResumeLayout(false);
            this.tabPageMyPassword.PerformLayout();
            this.tabPageMyUniverse.ResumeLayout(false);
            this.tabPageMyUniverse.PerformLayout();
            this.tabPageMyAccount.ResumeLayout(false);
            this.tabPageMyAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewUniverseList;
        private System.Windows.Forms.ColumnHeader columnHeaderUniNumber;
        private System.Windows.Forms.ColumnHeader columnHeaderUniverseName;
        private System.Windows.Forms.TabControl tabControlSetup;
        private System.Windows.Forms.TabPage tabPageMyPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtboxPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPageMyUniverse;
        private System.Windows.Forms.TextBox txtboxRepeatPassword;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabelSelectNone;
        private System.Windows.Forms.LinkLabel linkLabelSelectAll;
        private System.Windows.Forms.LinkLabel linkLabelEditPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxCommunityList;
        private System.Windows.Forms.TabPage tabPageMyAccount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox CelestosPasswordtextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox CelestosLogintextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelMasterPassword;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
        private System.Windows.Forms.Label labelStatus;
    }
}