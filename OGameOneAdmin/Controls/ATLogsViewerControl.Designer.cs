namespace OGameOneAdmin.Controls
{
    partial class ATLogsViewerControl
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txbResult2 = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labeStatus = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.labelToDate = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxGO = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.retrieveOperatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
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
            this.splitContainer.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Size = new System.Drawing.Size(800, 600);
            this.splitContainer.SplitterDistance = 35;
            this.splitContainer.TabIndex = 3;
            // 
            // btnMenu
            // 
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
            this.comboBoxUniverseList.Location = new System.Drawing.Point(364, 6);
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
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.txbResult2);
            this.groupBox5.Location = new System.Drawing.Point(8, 167);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(785, 381);
            this.groupBox5.TabIndex = 43;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Output:";
            // 
            // txbResult2
            // 
            this.txbResult2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbResult2.Location = new System.Drawing.Point(3, 17);
            this.txbResult2.Name = "txbResult2";
            this.txbResult2.Size = new System.Drawing.Size(779, 361);
            this.txbResult2.TabIndex = 1;
            this.txbResult2.Text = "";
            this.txbResult2.WordWrap = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.labeStatus);
            this.groupBox4.Location = new System.Drawing.Point(168, 110);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(625, 51);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Status:";
            // 
            // labeStatus
            // 
            this.labeStatus.AutoSize = true;
            this.labeStatus.Location = new System.Drawing.Point(8, 25);
            this.labeStatus.Name = "labeStatus";
            this.labeStatus.Size = new System.Drawing.Size(0, 13);
            this.labeStatus.TabIndex = 42;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnCancel);
            this.groupBox3.Controls.Add(this.btnGenerate);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(168, 57);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(625, 47);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Generate Stats for:";
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(378, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 23);
            this.btnCancel.TabIndex = 42;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(273, 15);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(99, 23);
            this.btnGenerate.TabIndex = 41;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "NickName:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(84, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(172, 21);
            this.textBox1.TabIndex = 38;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dateTimePickerTo);
            this.groupBox2.Controls.Add(this.dateTimePickerFrom);
            this.groupBox2.Controls.Add(this.labelFromDate);
            this.groupBox2.Controls.Add(this.labelToDate);
            this.groupBox2.Location = new System.Drawing.Point(168, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(625, 48);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Period:";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(303, 19);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(223, 21);
            this.dateTimePickerTo.TabIndex = 7;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(44, 19);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(223, 21);
            this.dateTimePickerFrom.TabIndex = 4;
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Location = new System.Drawing.Point(8, 23);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(36, 13);
            this.labelFromDate.TabIndex = 5;
            this.labelFromDate.Text = "From";
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Location = new System.Drawing.Point(277, 23);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(21, 13);
            this.labelToDate.TabIndex = 6;
            this.labelToDate.Text = "To";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxGO);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 158);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Operator";
            // 
            // listBoxGO
            // 
            this.listBoxGO.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listBoxGO.FormattingEnabled = true;
            this.listBoxGO.Location = new System.Drawing.Point(8, 24);
            this.listBoxGO.Name = "listBoxGO";
            this.listBoxGO.Size = new System.Drawing.Size(135, 121);
            this.listBoxGO.TabIndex = 33;
            this.listBoxGO.SelectedIndexChanged += new System.EventHandler(this.listBoxGO_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.retrieveOperatorToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // retrieveOperatorToolStripMenuItem
            // 
            this.retrieveOperatorToolStripMenuItem.Name = "retrieveOperatorToolStripMenuItem";
            this.retrieveOperatorToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.retrieveOperatorToolStripMenuItem.Text = "Retrieve Operator";
            this.retrieveOperatorToolStripMenuItem.Click += new System.EventHandler(this.retrieveOperatorToolStripMenuItem_Click);
            // 
            // ATLogsViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "ATLogsViewerControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxGO;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox txbResult2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label labeStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem retrieveOperatorToolStripMenuItem;
    }
}
