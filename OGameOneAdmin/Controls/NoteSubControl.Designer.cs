namespace OGameOneAdmin.Controls
{
    partial class NoteSubControl
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
            OGameOneAdmin.Utilities.ListViewColumnSorter listViewColumnSorter1 = new OGameOneAdmin.Utilities.ListViewColumnSorter();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.listViewNotes = new OGameOneAdmin.Controls.FlickerFreeListView();
            this.columnHeaderUniverseName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNoteDateTime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNoteOwner = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNotePreview = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBoxNoteDetails = new System.Windows.Forms.RichTextBox();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.listViewNotes);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.richTextBoxNoteDetails);
            this.splitContainerMain.Size = new System.Drawing.Size(700, 400);
            this.splitContainerMain.SplitterDistance = 228;
            this.splitContainerMain.TabIndex = 0;
            // 
            // listViewNotes
            // 
            this.listViewNotes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUniverseName,
            this.columnHeaderNoteDateTime,
            this.columnHeaderNoteOwner,
            this.columnHeaderNotePreview});
            this.listViewNotes.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewNotes.FullRowSelect = true;
            this.listViewNotes.GridLines = true;
            this.listViewNotes.LabelWrap = false;
            listViewColumnSorter1.Order = System.Windows.Forms.SortOrder.Ascending;
            listViewColumnSorter1.SortColumn = 0;
            this.listViewNotes.ListViewColumnSorter = listViewColumnSorter1;
            this.listViewNotes.Location = new System.Drawing.Point(0, 0);
            this.listViewNotes.Name = "listViewNotes";
            this.listViewNotes.Size = new System.Drawing.Size(700, 228);
            this.listViewNotes.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewNotes.TabIndex = 1;
            this.listViewNotes.UseCompatibleStateImageBehavior = false;
            this.listViewNotes.View = System.Windows.Forms.View.Details;
            this.listViewNotes.SelectedIndexChanged += new System.EventHandler(this.listViewNotes_SelectedIndexChanged);
            this.listViewNotes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewNotes_ColumnClick);
            // 
            // columnHeaderUniverseName
            // 
            this.columnHeaderUniverseName.Text = "Universe";
            this.columnHeaderUniverseName.Width = 102;
            // 
            // columnHeaderNoteDateTime
            // 
            this.columnHeaderNoteDateTime.Text = "Date";
            this.columnHeaderNoteDateTime.Width = 110;
            // 
            // columnHeaderNoteOwner
            // 
            this.columnHeaderNoteOwner.Text = "Admin";
            // 
            // columnHeaderNotePreview
            // 
            this.columnHeaderNotePreview.Text = "Note Preview";
            this.columnHeaderNotePreview.Width = 125;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteNoteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // deleteNoteToolStripMenuItem
            // 
            this.deleteNoteToolStripMenuItem.Name = "deleteNoteToolStripMenuItem";
            this.deleteNoteToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.deleteNoteToolStripMenuItem.Text = "Delete Note";
            this.deleteNoteToolStripMenuItem.Click += new System.EventHandler(this.deleteNoteToolStripMenuItem_Click);
            // 
            // richTextBoxNoteDetails
            // 
            this.richTextBoxNoteDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxNoteDetails.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxNoteDetails.Name = "richTextBoxNoteDetails";
            this.richTextBoxNoteDetails.Size = new System.Drawing.Size(700, 168);
            this.richTextBoxNoteDetails.TabIndex = 1;
            this.richTextBoxNoteDetails.Text = "";
            this.richTextBoxNoteDetails.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxNoteDetails_LinkClicked);
            // 
            // NoteSubControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NoteSubControl";
            this.Size = new System.Drawing.Size(700, 400);
            this.Resize += new System.EventHandler(this.NoteSubControl_Resize);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private FlickerFreeListView listViewNotes;
        private System.Windows.Forms.ColumnHeader columnHeaderUniverseName;
        private System.Windows.Forms.ColumnHeader columnHeaderNoteDateTime;
        private System.Windows.Forms.ColumnHeader columnHeaderNoteOwner;
        private System.Windows.Forms.ColumnHeader columnHeaderNotePreview;
        private System.Windows.Forms.RichTextBox richTextBoxNoteDetails;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteNoteToolStripMenuItem;
    }
}
