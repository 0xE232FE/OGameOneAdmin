namespace OGameOneAdmin.Controls
{
    partial class MultiCheckerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnMenu = new System.Windows.Forms.Button();
            this.pictureBoxInProgress = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxUniverseList = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxCommunityList = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCheckMulti = new System.Windows.Forms.Button();
            this.btnOpenAllAcc = new System.Windows.Forms.Button();
            this.btnOpenAcc = new System.Windows.Forms.Button();
            this.btnRemoveAccountFromList = new System.Windows.Forms.Button();
            this.btnAddToList = new System.Windows.Forms.Button();
            this.txtBoxAddAccount = new System.Windows.Forms.TextBox();
            this.listBoxAccountList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxMatchingData = new System.Windows.Forms.ListBox();
            this.btnSearchMatchingData = new System.Windows.Forms.Button();
            this.checkBoxMultiEmail = new System.Windows.Forms.CheckBox();
            this.checkBoxMultiAlliance = new System.Windows.Forms.CheckBox();
            this.checkBoxMultiIP = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.btnMenu);
            this.splitContainer.Panel1.Controls.Add(this.pictureBoxInProgress);
            this.splitContainer.Panel1.Controls.Add(this.label1);
            this.splitContainer.Panel1.Controls.Add(this.comboBoxUniverseList);
            this.splitContainer.Panel1.Controls.Add(this.label5);
            this.splitContainer.Panel1.Controls.Add(this.comboBoxCommunityList);
            this.splitContainer.Panel1MinSize = 5;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Size = new System.Drawing.Size(737, 422);
            this.splitContainer.SplitterDistance = 35;
            this.splitContainer.TabIndex = 3;
            // 
            // btnMenu
            // 
            this.btnMenu.Enabled = false;
            this.btnMenu.Location = new System.Drawing.Point(8, 6);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(67, 21);
            this.btnMenu.TabIndex = 25;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pictureBoxInProgress
            // 
            this.pictureBoxInProgress.Image = global::OGameOneAdmin.Properties.Resources.ajax_loader_small_bar;
            this.pictureBoxInProgress.Location = new System.Drawing.Point(525, 11);
            this.pictureBoxInProgress.Name = "pictureBoxInProgress";
            this.pictureBoxInProgress.Size = new System.Drawing.Size(16, 11);
            this.pictureBoxInProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxInProgress.TabIndex = 0;
            this.pictureBoxInProgress.TabStop = false;
            this.pictureBoxInProgress.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(295, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Universe:";
            // 
            // comboBoxUniverseList
            // 
            this.comboBoxUniverseList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUniverseList.FormattingEnabled = true;
            this.comboBoxUniverseList.Location = new System.Drawing.Point(369, 6);
            this.comboBoxUniverseList.Name = "comboBoxUniverseList";
            this.comboBoxUniverseList.Size = new System.Drawing.Size(140, 21);
            this.comboBoxUniverseList.TabIndex = 23;
            this.comboBoxUniverseList.SelectedIndexChanged += new System.EventHandler(this.comboBoxUniverseList_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Community:";
            // 
            // comboBoxCommunityList
            // 
            this.comboBoxCommunityList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCommunityList.FormattingEnabled = true;
            this.comboBoxCommunityList.Location = new System.Drawing.Point(168, 6);
            this.comboBoxCommunityList.Name = "comboBoxCommunityList";
            this.comboBoxCommunityList.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCommunityList.TabIndex = 21;
            this.comboBoxCommunityList.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommunityList_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.webBrowser1);
            this.groupBox3.Location = new System.Drawing.Point(297, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(424, 230);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Results";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 17);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(418, 210);
            this.webBrowser1.TabIndex = 38;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCheckMulti);
            this.groupBox2.Controls.Add(this.btnOpenAllAcc);
            this.groupBox2.Controls.Add(this.btnOpenAcc);
            this.groupBox2.Controls.Add(this.btnRemoveAccountFromList);
            this.groupBox2.Controls.Add(this.btnAddToList);
            this.groupBox2.Controls.Add(this.txtBoxAddAccount);
            this.groupBox2.Controls.Add(this.listBoxAccountList);
            this.groupBox2.Location = new System.Drawing.Point(297, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 134);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Accounts";
            // 
            // btnCheckMulti
            // 
            this.btnCheckMulti.Location = new System.Drawing.Point(360, 16);
            this.btnCheckMulti.Name = "btnCheckMulti";
            this.btnCheckMulti.Size = new System.Drawing.Size(58, 108);
            this.btnCheckMulti.TabIndex = 40;
            this.btnCheckMulti.Text = "Check";
            this.btnCheckMulti.UseVisualStyleBackColor = true;
            this.btnCheckMulti.Click += new System.EventHandler(this.btnCheckMulti_Click);
            // 
            // btnOpenAllAcc
            // 
            this.btnOpenAllAcc.Location = new System.Drawing.Point(8, 101);
            this.btnOpenAllAcc.Name = "btnOpenAllAcc";
            this.btnOpenAllAcc.Size = new System.Drawing.Size(101, 23);
            this.btnOpenAllAcc.TabIndex = 39;
            this.btnOpenAllAcc.Text = "Open All Acc";
            this.btnOpenAllAcc.UseVisualStyleBackColor = true;
            this.btnOpenAllAcc.Click += new System.EventHandler(this.btnOpenAllAcc_Click);
            // 
            // btnOpenAcc
            // 
            this.btnOpenAcc.Location = new System.Drawing.Point(8, 73);
            this.btnOpenAcc.Name = "btnOpenAcc";
            this.btnOpenAcc.Size = new System.Drawing.Size(101, 23);
            this.btnOpenAcc.TabIndex = 38;
            this.btnOpenAcc.Text = "Open Acc";
            this.btnOpenAcc.UseVisualStyleBackColor = true;
            this.btnOpenAcc.Click += new System.EventHandler(this.btnOpenAcc_Click);
            // 
            // btnRemoveAccountFromList
            // 
            this.btnRemoveAccountFromList.Location = new System.Drawing.Point(47, 45);
            this.btnRemoveAccountFromList.Name = "btnRemoveAccountFromList";
            this.btnRemoveAccountFromList.Size = new System.Drawing.Size(62, 22);
            this.btnRemoveAccountFromList.TabIndex = 37;
            this.btnRemoveAccountFromList.Text = "Remove";
            this.btnRemoveAccountFromList.UseVisualStyleBackColor = true;
            this.btnRemoveAccountFromList.Click += new System.EventHandler(this.btnRemoveAccountFromList_Click);
            // 
            // btnAddToList
            // 
            this.btnAddToList.Location = new System.Drawing.Point(8, 45);
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.Size = new System.Drawing.Size(38, 22);
            this.btnAddToList.TabIndex = 36;
            this.btnAddToList.Text = "Add";
            this.btnAddToList.UseVisualStyleBackColor = true;
            this.btnAddToList.Click += new System.EventHandler(this.btnAddToList_Click);
            // 
            // txtBoxAddAccount
            // 
            this.txtBoxAddAccount.Location = new System.Drawing.Point(8, 18);
            this.txtBoxAddAccount.Name = "txtBoxAddAccount";
            this.txtBoxAddAccount.Size = new System.Drawing.Size(101, 21);
            this.txtBoxAddAccount.TabIndex = 35;
            // 
            // listBoxAccountList
            // 
            this.listBoxAccountList.FormattingEnabled = true;
            this.listBoxAccountList.Location = new System.Drawing.Point(115, 16);
            this.listBoxAccountList.Name = "listBoxAccountList";
            this.listBoxAccountList.Size = new System.Drawing.Size(239, 108);
            this.listBoxAccountList.TabIndex = 34;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.listBoxMatchingData);
            this.groupBox1.Controls.Add(this.btnSearchMatchingData);
            this.groupBox1.Controls.Add(this.checkBoxMultiEmail);
            this.groupBox1.Controls.Add(this.checkBoxMultiAlliance);
            this.groupBox1.Controls.Add(this.checkBoxMultiIP);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 370);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Matching Data";
            // 
            // listBoxMatchingData
            // 
            this.listBoxMatchingData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMatchingData.FormattingEnabled = true;
            this.listBoxMatchingData.Location = new System.Drawing.Point(10, 45);
            this.listBoxMatchingData.Name = "listBoxMatchingData";
            this.listBoxMatchingData.Size = new System.Drawing.Size(261, 316);
            this.listBoxMatchingData.TabIndex = 9;
            this.listBoxMatchingData.SelectedIndexChanged += new System.EventHandler(this.listBoxMatchingData_SelectedIndexChanged);
            // 
            // btnSearchMatchingData
            // 
            this.btnSearchMatchingData.Location = new System.Drawing.Point(201, 16);
            this.btnSearchMatchingData.Name = "btnSearchMatchingData";
            this.btnSearchMatchingData.Size = new System.Drawing.Size(72, 23);
            this.btnSearchMatchingData.TabIndex = 8;
            this.btnSearchMatchingData.Text = "Search";
            this.btnSearchMatchingData.UseVisualStyleBackColor = true;
            this.btnSearchMatchingData.Click += new System.EventHandler(this.btnSearchMatchingData_Click);
            // 
            // checkBoxMultiPassword
            // 
            this.checkBoxMultiEmail.AutoSize = true;
            this.checkBoxMultiEmail.Location = new System.Drawing.Point(50, 20);
            this.checkBoxMultiEmail.Name = "checkBoxMultiPassword";
            this.checkBoxMultiEmail.Size = new System.Drawing.Size(57, 17);
            this.checkBoxMultiEmail.TabIndex = 4;
            this.checkBoxMultiEmail.Text = "Email";
            this.checkBoxMultiEmail.UseVisualStyleBackColor = true;
            // 
            // checkBoxMultiAlliance
            // 
            this.checkBoxMultiAlliance.AutoSize = true;
            this.checkBoxMultiAlliance.Location = new System.Drawing.Point(110, 20);
            this.checkBoxMultiAlliance.Name = "checkBoxMultiAlliance";
            this.checkBoxMultiAlliance.Size = new System.Drawing.Size(70, 17);
            this.checkBoxMultiAlliance.TabIndex = 7;
            this.checkBoxMultiAlliance.Text = "Alliance";
            this.checkBoxMultiAlliance.UseVisualStyleBackColor = true;
            // 
            // checkBoxMultiIP
            // 
            this.checkBoxMultiIP.AutoSize = true;
            this.checkBoxMultiIP.Location = new System.Drawing.Point(12, 20);
            this.checkBoxMultiIP.Name = "checkBoxMultiIP";
            this.checkBoxMultiIP.Size = new System.Drawing.Size(38, 17);
            this.checkBoxMultiIP.TabIndex = 5;
            this.checkBoxMultiIP.Text = "IP";
            this.checkBoxMultiIP.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(36, 4);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // MultiCheckerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "MultiCheckerControl";
            this.Size = new System.Drawing.Size(737, 422);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.PictureBox pictureBoxInProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxUniverseList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxCommunityList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.CheckBox checkBoxMultiAlliance;
        private System.Windows.Forms.CheckBox checkBoxMultiIP;
        private System.Windows.Forms.CheckBox checkBoxMultiEmail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearchMatchingData;
        private System.Windows.Forms.ListBox listBoxMatchingData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemoveAccountFromList;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.TextBox txtBoxAddAccount;
        private System.Windows.Forms.ListBox listBoxAccountList;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCheckMulti;
        private System.Windows.Forms.Button btnOpenAllAcc;
        private System.Windows.Forms.Button btnOpenAcc;
    }
}
