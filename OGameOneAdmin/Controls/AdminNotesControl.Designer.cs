namespace OGameOneAdmin.Controls
{
    partial class AdminNotesControl
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
            this.splitContainerAdminNotes = new System.Windows.Forms.SplitContainer();
            this.btnMenu = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkForNewNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAGeneralNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendAMessageToAnAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxInProgress = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxUniverseList = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxCommunityList = new System.Windows.Forms.ComboBox();
            this.tabControlAdminNotes = new System.Windows.Forms.TabControl();
            this.tabPagePersonalNotes = new System.Windows.Forms.TabPage();
            this.tabPageGeneralNotes = new System.Windows.Forms.TabPage();
            this.splitContainerAdminNotes.Panel1.SuspendLayout();
            this.splitContainerAdminNotes.Panel2.SuspendLayout();
            this.splitContainerAdminNotes.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).BeginInit();
            this.tabControlAdminNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerAdminNotes
            // 
            this.splitContainerAdminNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAdminNotes.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerAdminNotes.IsSplitterFixed = true;
            this.splitContainerAdminNotes.Location = new System.Drawing.Point(0, 0);
            this.splitContainerAdminNotes.Name = "splitContainerAdminNotes";
            this.splitContainerAdminNotes.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerAdminNotes.Panel1
            // 
            this.splitContainerAdminNotes.Panel1.Controls.Add(this.btnMenu);
            this.splitContainerAdminNotes.Panel1.Controls.Add(this.pictureBoxInProgress);
            this.splitContainerAdminNotes.Panel1.Controls.Add(this.label1);
            this.splitContainerAdminNotes.Panel1.Controls.Add(this.comboBoxUniverseList);
            this.splitContainerAdminNotes.Panel1.Controls.Add(this.label5);
            this.splitContainerAdminNotes.Panel1.Controls.Add(this.comboBoxCommunityList);
            this.splitContainerAdminNotes.Panel1MinSize = 5;
            // 
            // splitContainerAdminNotes.Panel2
            // 
            this.splitContainerAdminNotes.Panel2.Controls.Add(this.tabControlAdminNotes);
            this.splitContainerAdminNotes.Size = new System.Drawing.Size(600, 400);
            this.splitContainerAdminNotes.SplitterDistance = 35;
            this.splitContainerAdminNotes.SplitterIncrement = 4;
            this.splitContainerAdminNotes.SplitterWidth = 1;
            this.splitContainerAdminNotes.TabIndex = 2;
            // 
            // btnMenu
            // 
            this.btnMenu.ContextMenuStrip = this.contextMenuStrip1;
            this.btnMenu.Location = new System.Drawing.Point(8, 6);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(67, 21);
            this.btnMenu.TabIndex = 25;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForNewNotesToolStripMenuItem,
            this.createAGeneralNoteToolStripMenuItem,
            this.sendAMessageToAnAdminToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(188, 70);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // checkForNewNotesToolStripMenuItem
            // 
            this.checkForNewNotesToolStripMenuItem.Name = "checkForNewNotesToolStripMenuItem";
            this.checkForNewNotesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.checkForNewNotesToolStripMenuItem.Text = "Check for New Notes";
            this.checkForNewNotesToolStripMenuItem.Click += new System.EventHandler(this.checkForNewNotesToolStripMenuItem_Click);
            // 
            // createAGeneralNoteToolStripMenuItem
            // 
            this.createAGeneralNoteToolStripMenuItem.Enabled = false;
            this.createAGeneralNoteToolStripMenuItem.Name = "createAGeneralNoteToolStripMenuItem";
            this.createAGeneralNoteToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.createAGeneralNoteToolStripMenuItem.Text = "Create a General Note";
            // 
            // sendAMessageToAnAdminToolStripMenuItem
            // 
            this.sendAMessageToAnAdminToolStripMenuItem.Enabled = false;
            this.sendAMessageToAnAdminToolStripMenuItem.Name = "sendAMessageToAnAdminToolStripMenuItem";
            this.sendAMessageToAnAdminToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.sendAMessageToAnAdminToolStripMenuItem.Text = "Send a Message to an Admin";
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
            // tabControlAdminNotes
            // 
            this.tabControlAdminNotes.Controls.Add(this.tabPagePersonalNotes);
            this.tabControlAdminNotes.Controls.Add(this.tabPageGeneralNotes);
            this.tabControlAdminNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAdminNotes.Location = new System.Drawing.Point(0, 0);
            this.tabControlAdminNotes.Name = "tabControlAdminNotes";
            this.tabControlAdminNotes.Padding = new System.Drawing.Point(10, 5);
            this.tabControlAdminNotes.SelectedIndex = 0;
            this.tabControlAdminNotes.Size = new System.Drawing.Size(600, 364);
            this.tabControlAdminNotes.TabIndex = 0;
            this.tabControlAdminNotes.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlAdminNotes_Selected);
            // 
            // tabPagePersonalNotes
            // 
            this.tabPagePersonalNotes.Location = new System.Drawing.Point(4, 26);
            this.tabPagePersonalNotes.Name = "tabPagePersonalNotes";
            this.tabPagePersonalNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePersonalNotes.Size = new System.Drawing.Size(592, 334);
            this.tabPagePersonalNotes.TabIndex = 0;
            this.tabPagePersonalNotes.Text = "Personal Notes";
            this.tabPagePersonalNotes.UseVisualStyleBackColor = true;
            // 
            // tabPageGeneralNotes
            // 
            this.tabPageGeneralNotes.Location = new System.Drawing.Point(4, 26);
            this.tabPageGeneralNotes.Name = "tabPageGeneralNotes";
            this.tabPageGeneralNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneralNotes.Size = new System.Drawing.Size(592, 339);
            this.tabPageGeneralNotes.TabIndex = 1;
            this.tabPageGeneralNotes.Text = "General Notes";
            this.tabPageGeneralNotes.UseVisualStyleBackColor = true;
            // 
            // AdminNotesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerAdminNotes);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AdminNotesControl";
            this.Size = new System.Drawing.Size(600, 400);
            this.splitContainerAdminNotes.Panel1.ResumeLayout(false);
            this.splitContainerAdminNotes.Panel1.PerformLayout();
            this.splitContainerAdminNotes.Panel2.ResumeLayout(false);
            this.splitContainerAdminNotes.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).EndInit();
            this.tabControlAdminNotes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerAdminNotes;
        private System.Windows.Forms.TabControl tabControlAdminNotes;
        private System.Windows.Forms.TabPage tabPagePersonalNotes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxUniverseList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxCommunityList;
        private System.Windows.Forms.TabPage tabPageGeneralNotes;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem checkForNewNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createAGeneralNoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendAMessageToAnAdminToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBoxInProgress;
    }
}
