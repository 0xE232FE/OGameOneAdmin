namespace OGameOneAdmin.Controls
{
    partial class TemplateControl
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).BeginInit();
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
            this.splitContainer.Size = new System.Drawing.Size(600, 400);
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(36, 4);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // TemplateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "TemplateControl";
            this.Size = new System.Drawing.Size(600, 400);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).EndInit();
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
    }
}
