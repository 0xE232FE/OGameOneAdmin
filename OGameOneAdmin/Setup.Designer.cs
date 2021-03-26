namespace OGameOneAdmin
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
            this.linkLabelEditPassword = new System.Windows.Forms.LinkLabel();
            this.btnCancel1 = new System.Windows.Forms.Button();
            this.btnSetPassword = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
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
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.btnCompleteSetup = new System.Windows.Forms.Button();
            this.tabControlSetup.SuspendLayout();
            this.tabPageMyPassword.SuspendLayout();
            this.tabPageMyUniverse.SuspendLayout();
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
            this.listViewUniverseList.Location = new System.Drawing.Point(9, 37);
            this.listViewUniverseList.MultiSelect = false;
            this.listViewUniverseList.Name = "listViewUniverseList";
            this.listViewUniverseList.Size = new System.Drawing.Size(422, 275);
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
            this.columnHeaderUniverseName.Text = "Universe Name";
            this.columnHeaderUniverseName.Width = 301;
            // 
            // tabControlSetup
            // 
            this.tabControlSetup.Controls.Add(this.tabPageMyPassword);
            this.tabControlSetup.Controls.Add(this.tabPageMyUniverse);
            this.tabControlSetup.Location = new System.Drawing.Point(12, 12);
            this.tabControlSetup.Name = "tabControlSetup";
            this.tabControlSetup.Padding = new System.Drawing.Point(10, 5);
            this.tabControlSetup.SelectedIndex = 0;
            this.tabControlSetup.Size = new System.Drawing.Size(449, 375);
            this.tabControlSetup.TabIndex = 1;
            this.tabControlSetup.SelectedIndexChanged += new System.EventHandler(this.tabControlSetup_SelectedIndexChanged);
            // 
            // tabPageMyPassword
            // 
            this.tabPageMyPassword.Controls.Add(this.linkLabelEditPassword);
            this.tabPageMyPassword.Controls.Add(this.btnCancel1);
            this.tabPageMyPassword.Controls.Add(this.btnSetPassword);
            this.tabPageMyPassword.Controls.Add(this.label3);
            this.tabPageMyPassword.Controls.Add(this.richTextBox1);
            this.tabPageMyPassword.Controls.Add(this.txtboxRepeatPassword);
            this.tabPageMyPassword.Controls.Add(this.label2);
            this.tabPageMyPassword.Controls.Add(this.txtboxPassword);
            this.tabPageMyPassword.Controls.Add(this.label1);
            this.tabPageMyPassword.Location = new System.Drawing.Point(4, 26);
            this.tabPageMyPassword.Name = "tabPageMyPassword";
            this.tabPageMyPassword.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMyPassword.Size = new System.Drawing.Size(441, 345);
            this.tabPageMyPassword.TabIndex = 0;
            this.tabPageMyPassword.Text = "Password Setup";
            this.tabPageMyPassword.UseVisualStyleBackColor = true;
            // 
            // linkLabelEditPassword
            // 
            this.linkLabelEditPassword.AutoSize = true;
            this.linkLabelEditPassword.Location = new System.Drawing.Point(281, 240);
            this.linkLabelEditPassword.Name = "linkLabelEditPassword";
            this.linkLabelEditPassword.Size = new System.Drawing.Size(25, 13);
            this.linkLabelEditPassword.TabIndex = 13;
            this.linkLabelEditPassword.TabStop = true;
            this.linkLabelEditPassword.Text = "Edit";
            this.linkLabelEditPassword.Visible = false;
            this.linkLabelEditPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEditPassword_LinkClicked);
            // 
            // btnCancel1
            // 
            this.btnCancel1.Location = new System.Drawing.Point(339, 306);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(92, 23);
            this.btnCancel1.TabIndex = 8;
            this.btnCancel1.Text = "Cancel Setup";
            this.btnCancel1.UseVisualStyleBackColor = true;
            this.btnCancel1.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSetPassword
            // 
            this.btnSetPassword.Location = new System.Drawing.Point(237, 306);
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Size = new System.Drawing.Size(96, 23);
            this.btnSetPassword.TabIndex = 5;
            this.btnSetPassword.Text = "Set Password";
            this.btnSetPassword.UseVisualStyleBackColor = true;
            this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Choose a secret password:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Location = new System.Drawing.Point(9, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(422, 187);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // txtboxRepeatPassword
            // 
            this.txtboxRepeatPassword.Location = new System.Drawing.Point(112, 270);
            this.txtboxRepeatPassword.Name = "txtboxRepeatPassword";
            this.txtboxRepeatPassword.PasswordChar = '*';
            this.txtboxRepeatPassword.Size = new System.Drawing.Size(154, 20);
            this.txtboxRepeatPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 273);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Repeat Password:";
            // 
            // txtboxPassword
            // 
            this.txtboxPassword.Location = new System.Drawing.Point(112, 237);
            this.txtboxPassword.Name = "txtboxPassword";
            this.txtboxPassword.PasswordChar = '*';
            this.txtboxPassword.Size = new System.Drawing.Size(154, 20);
            this.txtboxPassword.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
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
            this.tabPageMyUniverse.Controls.Add(this.btnCancel2);
            this.tabPageMyUniverse.Controls.Add(this.btnCompleteSetup);
            this.tabPageMyUniverse.Controls.Add(this.listViewUniverseList);
            this.tabPageMyUniverse.Location = new System.Drawing.Point(4, 26);
            this.tabPageMyUniverse.Name = "tabPageMyUniverse";
            this.tabPageMyUniverse.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMyUniverse.Size = new System.Drawing.Size(441, 345);
            this.tabPageMyUniverse.TabIndex = 1;
            this.tabPageMyUniverse.Text = "My Universe(s)";
            this.tabPageMyUniverse.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(284, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Show:";
            // 
            // comboBoxCommunityList
            // 
            this.comboBoxCommunityList.FormattingEnabled = true;
            this.comboBoxCommunityList.Location = new System.Drawing.Point(327, 10);
            this.comboBoxCommunityList.Name = "comboBoxCommunityList";
            this.comboBoxCommunityList.Size = new System.Drawing.Size(104, 21);
            this.comboBoxCommunityList.TabIndex = 14;
            this.comboBoxCommunityList.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommunityList_SelectedIndexChanged);
            // 
            // linkLabelSelectNone
            // 
            this.linkLabelSelectNone.AutoSize = true;
            this.linkLabelSelectNone.Location = new System.Drawing.Point(63, 320);
            this.linkLabelSelectNone.Name = "linkLabelSelectNone";
            this.linkLabelSelectNone.Size = new System.Drawing.Size(78, 13);
            this.linkLabelSelectNone.TabIndex = 13;
            this.linkLabelSelectNone.TabStop = true;
            this.linkLabelSelectNone.Text = "Clear Selection";
            this.linkLabelSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSelectNone_LinkClicked);
            // 
            // linkLabelSelectAll
            // 
            this.linkLabelSelectAll.AutoSize = true;
            this.linkLabelSelectAll.Location = new System.Drawing.Point(6, 320);
            this.linkLabelSelectAll.Name = "linkLabelSelectAll";
            this.linkLabelSelectAll.Size = new System.Drawing.Size(51, 13);
            this.linkLabelSelectAll.TabIndex = 12;
            this.linkLabelSelectAll.TabStop = true;
            this.linkLabelSelectAll.Text = "Select All";
            this.linkLabelSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSelectAll_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Select your universe(s):";
            // 
            // btnCancel2
            // 
            this.btnCancel2.Location = new System.Drawing.Point(339, 320);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(92, 23);
            this.btnCancel2.TabIndex = 10;
            this.btnCancel2.Text = "Cancel Setup";
            this.btnCancel2.UseVisualStyleBackColor = true;
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCompleteSetup
            // 
            this.btnCompleteSetup.Location = new System.Drawing.Point(237, 320);
            this.btnCompleteSetup.Name = "btnCompleteSetup";
            this.btnCompleteSetup.Size = new System.Drawing.Size(96, 23);
            this.btnCompleteSetup.TabIndex = 9;
            this.btnCompleteSetup.Text = "Complete Setup";
            this.btnCompleteSetup.UseVisualStyleBackColor = true;
            this.btnCompleteSetup.Click += new System.EventHandler(this.btnCompleteSetup_Click);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 399);
            this.ControlBox = false;
            this.Controls.Add(this.tabControlSetup);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(481, 433);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(481, 433);
            this.Name = "Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tool Setup";
            this.Shown += new System.EventHandler(this.Setup_Shown);
            this.tabControlSetup.ResumeLayout(false);
            this.tabPageMyPassword.ResumeLayout(false);
            this.tabPageMyPassword.PerformLayout();
            this.tabPageMyUniverse.ResumeLayout(false);
            this.tabPageMyUniverse.PerformLayout();
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
        private System.Windows.Forms.Button btnSetPassword;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel1;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.Button btnCompleteSetup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabelSelectNone;
        private System.Windows.Forms.LinkLabel linkLabelSelectAll;
        private System.Windows.Forms.LinkLabel linkLabelEditPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxCommunityList;
    }
}