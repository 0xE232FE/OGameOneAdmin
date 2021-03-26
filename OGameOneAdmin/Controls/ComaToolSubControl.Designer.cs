namespace OGameOneAdmin.Controls
{
    partial class ComaToolSubControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnLogout = new System.Windows.Forms.Button();
            this.linkLabelClosedTicket = new System.Windows.Forms.LinkLabel();
            this.linkLabelOpenTickets = new System.Windows.Forms.LinkLabel();
            this.linkLabelMyTicket = new System.Windows.Forms.LinkLabel();
            this.pictureBoxInProgress = new System.Windows.Forms.PictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.labelPageNr = new System.Windows.Forms.Label();
            this.dataGridViewTicket = new System.Windows.Forms.DataGridView();
            this.TicketId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TicketValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Server = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nickname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StaffNickName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AnswerNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControlTicket = new System.Windows.Forms.TabControl();
            this.tabPageReadTicket = new System.Windows.Forms.TabPage();
            this.webBrowserTicketView = new System.Windows.Forms.WebBrowser();
            this.tabPageAnswerTicket = new System.Windows.Forms.TabPage();
            this.btnSelectAnswerTemplate = new System.Windows.Forms.Button();
            this.btnSubmitAnswerTicket = new System.Windows.Forms.Button();
            this.comboBoxAnswerTemplate = new System.Windows.Forms.ComboBox();
            this.txtBoxAnswerTicket = new System.Windows.Forms.RichTextBox();
            this.tabPageAddNotes = new System.Windows.Forms.TabPage();
            this.linkLabelAllPlayerQuestion = new System.Windows.Forms.LinkLabel();
            this.btnGenerateNote = new System.Windows.Forms.Button();
            this.checkBoxAddToAllPlayers = new System.Windows.Forms.CheckBox();
            this.checkBoxInclUID = new System.Windows.Forms.CheckBox();
            this.btnSubmitNote = new System.Windows.Forms.Button();
            this.comboBoxNoteTitle = new System.Windows.Forms.ComboBox();
            this.txtBoxNote = new System.Windows.Forms.RichTextBox();
            this.groupBoxPlayersInvolved = new System.Windows.Forms.GroupBox();
            this.btnOpenBrowser = new System.Windows.Forms.Button();
            this.btnRemoveAccountFromList = new System.Windows.Forms.Button();
            this.btnAddToList = new System.Windows.Forms.Button();
            this.txtBoxAddAccount = new System.Windows.Forms.TextBox();
            this.listBoxAccountList = new System.Windows.Forms.ListBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTicket)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControlTicket.SuspendLayout();
            this.tabPageReadTicket.SuspendLayout();
            this.tabPageAnswerTicket.SuspendLayout();
            this.tabPageAddNotes.SuspendLayout();
            this.groupBoxPlayersInvolved.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnLogout);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabelClosedTicket);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabelOpenTickets);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabelMyTicket);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxInProgress);
            this.splitContainer1.Panel1MinSize = 5;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(743, 435);
            this.splitContainer1.SplitterDistance = 30;
            this.splitContainer1.TabIndex = 3;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.Location = new System.Drawing.Point(665, 4);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // linkLabelClosedTicket
            // 
            this.linkLabelClosedTicket.AutoSize = true;
            this.linkLabelClosedTicket.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelClosedTicket.Location = new System.Drawing.Point(201, 9);
            this.linkLabelClosedTicket.Name = "linkLabelClosedTicket";
            this.linkLabelClosedTicket.Size = new System.Drawing.Size(90, 13);
            this.linkLabelClosedTicket.TabIndex = 5;
            this.linkLabelClosedTicket.TabStop = true;
            this.linkLabelClosedTicket.Text = "Closed Tickets";
            this.linkLabelClosedTicket.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClosedTicket_LinkClicked);
            // 
            // linkLabelOpenTickets
            // 
            this.linkLabelOpenTickets.AutoSize = true;
            this.linkLabelOpenTickets.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelOpenTickets.Location = new System.Drawing.Point(93, 9);
            this.linkLabelOpenTickets.Name = "linkLabelOpenTickets";
            this.linkLabelOpenTickets.Size = new System.Drawing.Size(102, 13);
            this.linkLabelOpenTickets.TabIndex = 3;
            this.linkLabelOpenTickets.TabStop = true;
            this.linkLabelOpenTickets.Text = "Open Tickets (0)";
            this.linkLabelOpenTickets.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOpenTickets_LinkClicked);
            // 
            // linkLabelMyTicket
            // 
            this.linkLabelMyTicket.AutoSize = true;
            this.linkLabelMyTicket.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelMyTicket.Location = new System.Drawing.Point(4, 9);
            this.linkLabelMyTicket.Name = "linkLabelMyTicket";
            this.linkLabelMyTicket.Size = new System.Drawing.Size(88, 13);
            this.linkLabelMyTicket.TabIndex = 1;
            this.linkLabelMyTicket.TabStop = true;
            this.linkLabelMyTicket.Text = "My Tickets (0)";
            this.linkLabelMyTicket.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelMyTicket_LinkClicked);
            // 
            // pictureBoxInProgress
            // 
            this.pictureBoxInProgress.Image = global::OGameOneAdmin.Properties.Resources.ajax_loader_small_bar;
            this.pictureBoxInProgress.Location = new System.Drawing.Point(296, 11);
            this.pictureBoxInProgress.Name = "pictureBoxInProgress";
            this.pictureBoxInProgress.Size = new System.Drawing.Size(16, 11);
            this.pictureBoxInProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxInProgress.TabIndex = 0;
            this.pictureBoxInProgress.TabStop = false;
            this.pictureBoxInProgress.Visible = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.labelPageNr);
            this.splitContainer2.Panel1.Controls.Add(this.dataGridViewTicket);
            this.splitContainer2.Panel1.Controls.Add(this.btnNext);
            this.splitContainer2.Panel1.Controls.Add(this.btnPrevious);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(743, 401);
            this.splitContainer2.SplitterDistance = 168;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            // 
            // labelPageNr
            // 
            this.labelPageNr.AutoSize = true;
            this.labelPageNr.Location = new System.Drawing.Point(286, 136);
            this.labelPageNr.Name = "labelPageNr";
            this.labelPageNr.Size = new System.Drawing.Size(26, 13);
            this.labelPageNr.TabIndex = 3;
            this.labelPageNr.Text = "1/1";
            // 
            // dataGridViewTicket
            // 
            this.dataGridViewTicket.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTicket.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewTicket.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewTicket.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewTicket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTicket.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TicketId,
            this.TicketValue,
            this.Server,
            this.Subject,
            this.Nickname,
            this.StaffNickName,
            this.AnswerNumber,
            this.Date,
            this.Date2});
            this.dataGridViewTicket.Location = new System.Drawing.Point(1, -1);
            this.dataGridViewTicket.MultiSelect = false;
            this.dataGridViewTicket.Name = "dataGridViewTicket";
            this.dataGridViewTicket.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewTicket.RowHeadersVisible = false;
            this.dataGridViewTicket.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTicket.Size = new System.Drawing.Size(739, 135);
            this.dataGridViewTicket.TabIndex = 2;
            this.dataGridViewTicket.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTicket_CellClick);
            // 
            // TicketId
            // 
            this.TicketId.DataPropertyName = "TicketId";
            this.TicketId.HeaderText = "Ticket Id";
            this.TicketId.Name = "TicketId";
            this.TicketId.ReadOnly = true;
            this.TicketId.Visible = false;
            // 
            // TicketValue
            // 
            this.TicketValue.DataPropertyName = "TicketValue";
            this.TicketValue.HeaderText = "Ticket Value";
            this.TicketValue.Name = "TicketValue";
            this.TicketValue.ReadOnly = true;
            this.TicketValue.Visible = false;
            // 
            // Server
            // 
            this.Server.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Server.DataPropertyName = "Server";
            this.Server.HeaderText = "Server";
            this.Server.Name = "Server";
            this.Server.ReadOnly = true;
            this.Server.Width = 71;
            // 
            // Subject
            // 
            this.Subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Subject.DataPropertyName = "Subject";
            this.Subject.HeaderText = "Subject";
            this.Subject.Name = "Subject";
            this.Subject.ReadOnly = true;
            // 
            // Nickname
            // 
            this.Nickname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Nickname.DataPropertyName = "NickName";
            this.Nickname.HeaderText = "Player";
            this.Nickname.Name = "Nickname";
            this.Nickname.ReadOnly = true;
            this.Nickname.Width = 68;
            // 
            // StaffNickName
            // 
            this.StaffNickName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StaffNickName.DataPropertyName = "StaffNickName";
            this.StaffNickName.HeaderText = "Staff";
            this.StaffNickName.Name = "StaffNickName";
            this.StaffNickName.ReadOnly = true;
            this.StaffNickName.Width = 59;
            // 
            // AnswerNumber
            // 
            this.AnswerNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AnswerNumber.DataPropertyName = "OpenedTickets";
            this.AnswerNumber.HeaderText = "#";
            this.AnswerNumber.Name = "AnswerNumber";
            this.AnswerNumber.ReadOnly = true;
            this.AnswerNumber.Width = 41;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Date.DataPropertyName = "DateString";
            this.Date.HeaderText = "First action";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 94;
            // 
            // Date2
            // 
            this.Date2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Date2.DataPropertyName = "DateString2";
            this.Date2.HeaderText = "Last action";
            this.Date2.Name = "Date2";
            this.Date2.ReadOnly = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(665, 140);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Location = new System.Drawing.Point(0, 140);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 0;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControlTicket);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBoxPlayersInvolved);
            this.splitContainer3.Size = new System.Drawing.Size(743, 232);
            this.splitContainer3.SplitterDistance = 582;
            this.splitContainer3.SplitterWidth = 1;
            this.splitContainer3.TabIndex = 0;
            // 
            // tabControlTicket
            // 
            this.tabControlTicket.Controls.Add(this.tabPageReadTicket);
            this.tabControlTicket.Controls.Add(this.tabPageAnswerTicket);
            this.tabControlTicket.Controls.Add(this.tabPageAddNotes);
            this.tabControlTicket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTicket.Location = new System.Drawing.Point(0, 0);
            this.tabControlTicket.Name = "tabControlTicket";
            this.tabControlTicket.SelectedIndex = 0;
            this.tabControlTicket.Size = new System.Drawing.Size(582, 232);
            this.tabControlTicket.TabIndex = 0;
            // 
            // tabPageReadTicket
            // 
            this.tabPageReadTicket.Controls.Add(this.webBrowserTicketView);
            this.tabPageReadTicket.Location = new System.Drawing.Point(4, 22);
            this.tabPageReadTicket.Name = "tabPageReadTicket";
            this.tabPageReadTicket.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReadTicket.Size = new System.Drawing.Size(574, 206);
            this.tabPageReadTicket.TabIndex = 0;
            this.tabPageReadTicket.Text = "Read Ticket";
            this.tabPageReadTicket.UseVisualStyleBackColor = true;
            // 
            // webBrowserTicketView
            // 
            this.webBrowserTicketView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserTicketView.Location = new System.Drawing.Point(3, 3);
            this.webBrowserTicketView.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserTicketView.Name = "webBrowserTicketView";
            this.webBrowserTicketView.ScriptErrorsSuppressed = true;
            this.webBrowserTicketView.Size = new System.Drawing.Size(568, 200);
            this.webBrowserTicketView.TabIndex = 0;
            // 
            // tabPageAnswerTicket
            // 
            this.tabPageAnswerTicket.Controls.Add(this.btnSelectAnswerTemplate);
            this.tabPageAnswerTicket.Controls.Add(this.btnSubmitAnswerTicket);
            this.tabPageAnswerTicket.Controls.Add(this.comboBoxAnswerTemplate);
            this.tabPageAnswerTicket.Controls.Add(this.txtBoxAnswerTicket);
            this.tabPageAnswerTicket.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnswerTicket.Name = "tabPageAnswerTicket";
            this.tabPageAnswerTicket.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnswerTicket.Size = new System.Drawing.Size(571, 193);
            this.tabPageAnswerTicket.TabIndex = 1;
            this.tabPageAnswerTicket.Text = "Answer Ticket";
            this.tabPageAnswerTicket.UseVisualStyleBackColor = true;
            // 
            // btnSelectAnswerTemplate
            // 
            this.btnSelectAnswerTemplate.Location = new System.Drawing.Point(195, 10);
            this.btnSelectAnswerTemplate.Name = "btnSelectAnswerTemplate";
            this.btnSelectAnswerTemplate.Size = new System.Drawing.Size(92, 23);
            this.btnSelectAnswerTemplate.TabIndex = 6;
            this.btnSelectAnswerTemplate.Text = "Select Reply";
            this.btnSelectAnswerTemplate.UseVisualStyleBackColor = true;
            this.btnSelectAnswerTemplate.Click += new System.EventHandler(this.btnSelectAnswerTemplate_Click);
            // 
            // btnSubmitAnswerTicket
            // 
            this.btnSubmitAnswerTicket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmitAnswerTicket.Location = new System.Drawing.Point(488, 165);
            this.btnSubmitAnswerTicket.Name = "btnSubmitAnswerTicket";
            this.btnSubmitAnswerTicket.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitAnswerTicket.TabIndex = 5;
            this.btnSubmitAnswerTicket.Text = "Submit";
            this.btnSubmitAnswerTicket.UseVisualStyleBackColor = true;
            this.btnSubmitAnswerTicket.Click += new System.EventHandler(this.btnSubmitAnswerTicket_Click);
            // 
            // comboBoxAnswerTemplate
            // 
            this.comboBoxAnswerTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnswerTemplate.FormattingEnabled = true;
            this.comboBoxAnswerTemplate.Location = new System.Drawing.Point(6, 11);
            this.comboBoxAnswerTemplate.Name = "comboBoxAnswerTemplate";
            this.comboBoxAnswerTemplate.Size = new System.Drawing.Size(183, 21);
            this.comboBoxAnswerTemplate.TabIndex = 4;
            this.comboBoxAnswerTemplate.SelectedIndexChanged += new System.EventHandler(this.comboBoxAnswerTemplate_SelectedIndexChanged);
            // 
            // txtBoxAnswerTicket
            // 
            this.txtBoxAnswerTicket.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxAnswerTicket.Location = new System.Drawing.Point(6, 38);
            this.txtBoxAnswerTicket.Name = "txtBoxAnswerTicket";
            this.txtBoxAnswerTicket.Size = new System.Drawing.Size(559, 149);
            this.txtBoxAnswerTicket.TabIndex = 2;
            this.txtBoxAnswerTicket.Text = "";
            // 
            // tabPageAddNotes
            // 
            this.tabPageAddNotes.Controls.Add(this.linkLabelAllPlayerQuestion);
            this.tabPageAddNotes.Controls.Add(this.btnGenerateNote);
            this.tabPageAddNotes.Controls.Add(this.checkBoxAddToAllPlayers);
            this.tabPageAddNotes.Controls.Add(this.checkBoxInclUID);
            this.tabPageAddNotes.Controls.Add(this.btnSubmitNote);
            this.tabPageAddNotes.Controls.Add(this.comboBoxNoteTitle);
            this.tabPageAddNotes.Controls.Add(this.txtBoxNote);
            this.tabPageAddNotes.Location = new System.Drawing.Point(4, 22);
            this.tabPageAddNotes.Name = "tabPageAddNotes";
            this.tabPageAddNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAddNotes.Size = new System.Drawing.Size(571, 193);
            this.tabPageAddNotes.TabIndex = 2;
            this.tabPageAddNotes.Text = "Add Notes";
            this.tabPageAddNotes.UseVisualStyleBackColor = true;
            // 
            // linkLabelAllPlayerQuestion
            // 
            this.linkLabelAllPlayerQuestion.AutoSize = true;
            this.linkLabelAllPlayerQuestion.Location = new System.Drawing.Point(555, 16);
            this.linkLabelAllPlayerQuestion.Name = "linkLabelAllPlayerQuestion";
            this.linkLabelAllPlayerQuestion.Size = new System.Drawing.Size(13, 13);
            this.linkLabelAllPlayerQuestion.TabIndex = 11;
            this.linkLabelAllPlayerQuestion.TabStop = true;
            this.linkLabelAllPlayerQuestion.Text = "?";
            this.linkLabelAllPlayerQuestion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAllPlayerQuestion_LinkClicked);
            // 
            // btnGenerateNote
            // 
            this.btnGenerateNote.Location = new System.Drawing.Point(195, 10);
            this.btnGenerateNote.Name = "btnGenerateNote";
            this.btnGenerateNote.Size = new System.Drawing.Size(72, 23);
            this.btnGenerateNote.TabIndex = 10;
            this.btnGenerateNote.Text = "Generate";
            this.btnGenerateNote.UseVisualStyleBackColor = true;
            this.btnGenerateNote.Click += new System.EventHandler(this.btnGenerateNote_Click);
            // 
            // checkBoxAddToAllPlayers
            // 
            this.checkBoxAddToAllPlayers.AutoSize = true;
            this.checkBoxAddToAllPlayers.Checked = true;
            this.checkBoxAddToAllPlayers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddToAllPlayers.Location = new System.Drawing.Point(354, 15);
            this.checkBoxAddToAllPlayers.Name = "checkBoxAddToAllPlayers";
            this.checkBoxAddToAllPlayers.Size = new System.Drawing.Size(207, 17);
            this.checkBoxAddToAllPlayers.TabIndex = 9;
            this.checkBoxAddToAllPlayers.Text = "Add note to all players involved";
            this.checkBoxAddToAllPlayers.UseVisualStyleBackColor = true;
            // 
            // checkBoxInclUID
            // 
            this.checkBoxInclUID.AutoSize = true;
            this.checkBoxInclUID.Checked = true;
            this.checkBoxInclUID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInclUID.Location = new System.Drawing.Point(273, 14);
            this.checkBoxInclUID.Name = "checkBoxInclUID";
            this.checkBoxInclUID.Size = new System.Drawing.Size(77, 17);
            this.checkBoxInclUID.TabIndex = 8;
            this.checkBoxInclUID.Text = "Incl. UID";
            this.checkBoxInclUID.UseVisualStyleBackColor = true;
            // 
            // btnSubmitNote
            // 
            this.btnSubmitNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmitNote.Location = new System.Drawing.Point(488, 165);
            this.btnSubmitNote.Name = "btnSubmitNote";
            this.btnSubmitNote.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitNote.TabIndex = 7;
            this.btnSubmitNote.Text = "Submit";
            this.btnSubmitNote.UseVisualStyleBackColor = true;
            this.btnSubmitNote.Click += new System.EventHandler(this.btnSubmitNote_Click);
            // 
            // comboBoxNoteTitle
            // 
            this.comboBoxNoteTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNoteTitle.FormattingEnabled = true;
            this.comboBoxNoteTitle.Location = new System.Drawing.Point(6, 11);
            this.comboBoxNoteTitle.Name = "comboBoxNoteTitle";
            this.comboBoxNoteTitle.Size = new System.Drawing.Size(183, 21);
            this.comboBoxNoteTitle.TabIndex = 6;
            this.comboBoxNoteTitle.SelectedIndexChanged += new System.EventHandler(this.comboBoxNoteTitle_SelectedIndexChanged);
            // 
            // txtBoxNote
            // 
            this.txtBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxNote.Location = new System.Drawing.Point(6, 38);
            this.txtBoxNote.Name = "txtBoxNote";
            this.txtBoxNote.Size = new System.Drawing.Size(559, 149);
            this.txtBoxNote.TabIndex = 3;
            this.txtBoxNote.Text = "";
            // 
            // groupBoxPlayersInvolved
            // 
            this.groupBoxPlayersInvolved.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPlayersInvolved.Controls.Add(this.btnOpenBrowser);
            this.groupBoxPlayersInvolved.Controls.Add(this.btnRemoveAccountFromList);
            this.groupBoxPlayersInvolved.Controls.Add(this.btnAddToList);
            this.groupBoxPlayersInvolved.Controls.Add(this.txtBoxAddAccount);
            this.groupBoxPlayersInvolved.Controls.Add(this.listBoxAccountList);
            this.groupBoxPlayersInvolved.Location = new System.Drawing.Point(3, 17);
            this.groupBoxPlayersInvolved.Name = "groupBoxPlayersInvolved";
            this.groupBoxPlayersInvolved.Size = new System.Drawing.Size(154, 212);
            this.groupBoxPlayersInvolved.TabIndex = 0;
            this.groupBoxPlayersInvolved.TabStop = false;
            this.groupBoxPlayersInvolved.Text = "Players involved";
            // 
            // btnOpenBrowser
            // 
            this.btnOpenBrowser.Location = new System.Drawing.Point(79, 186);
            this.btnOpenBrowser.Name = "btnOpenBrowser";
            this.btnOpenBrowser.Size = new System.Drawing.Size(69, 23);
            this.btnOpenBrowser.TabIndex = 34;
            this.btnOpenBrowser.Text = "Open Acc";
            this.btnOpenBrowser.UseVisualStyleBackColor = true;
            this.btnOpenBrowser.Click += new System.EventHandler(this.btnOpenBrowser_Click);
            // 
            // btnRemoveAccountFromList
            // 
            this.btnRemoveAccountFromList.Location = new System.Drawing.Point(6, 186);
            this.btnRemoveAccountFromList.Name = "btnRemoveAccountFromList";
            this.btnRemoveAccountFromList.Size = new System.Drawing.Size(70, 23);
            this.btnRemoveAccountFromList.TabIndex = 33;
            this.btnRemoveAccountFromList.Text = "Remove";
            this.btnRemoveAccountFromList.UseVisualStyleBackColor = true;
            this.btnRemoveAccountFromList.Click += new System.EventHandler(this.btnRemoveAccountFromList_Click);
            // 
            // btnAddToList
            // 
            this.btnAddToList.Location = new System.Drawing.Point(110, 17);
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.Size = new System.Drawing.Size(38, 21);
            this.btnAddToList.TabIndex = 32;
            this.btnAddToList.Text = "Add";
            this.btnAddToList.UseVisualStyleBackColor = true;
            this.btnAddToList.Click += new System.EventHandler(this.btnAddToList_Click);
            // 
            // txtBoxAddAccount
            // 
            this.txtBoxAddAccount.Location = new System.Drawing.Point(6, 17);
            this.txtBoxAddAccount.Name = "txtBoxAddAccount";
            this.txtBoxAddAccount.Size = new System.Drawing.Size(101, 21);
            this.txtBoxAddAccount.TabIndex = 31;
            // 
            // listBoxAccountList
            // 
            this.listBoxAccountList.FormattingEnabled = true;
            this.listBoxAccountList.Location = new System.Drawing.Point(6, 44);
            this.listBoxAccountList.Name = "listBoxAccountList";
            this.listBoxAccountList.Size = new System.Drawing.Size(142, 134);
            this.listBoxAccountList.TabIndex = 1;
            // 
            // ComaToolSubControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "ComaToolSubControl";
            this.Size = new System.Drawing.Size(743, 435);
            this.Resize += new System.EventHandler(this.ComaToolSubControl_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInProgress)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTicket)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.tabControlTicket.ResumeLayout(false);
            this.tabPageReadTicket.ResumeLayout(false);
            this.tabPageAnswerTicket.ResumeLayout(false);
            this.tabPageAddNotes.ResumeLayout(false);
            this.tabPageAddNotes.PerformLayout();
            this.groupBoxPlayersInvolved.ResumeLayout(false);
            this.groupBoxPlayersInvolved.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBoxInProgress;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.LinkLabel linkLabelClosedTicket;
        private System.Windows.Forms.LinkLabel linkLabelOpenTickets;
        private System.Windows.Forms.LinkLabel linkLabelMyTicket;
        private System.Windows.Forms.TabControl tabControlTicket;
        private System.Windows.Forms.TabPage tabPageReadTicket;
        private System.Windows.Forms.TabPage tabPageAnswerTicket;
        private System.Windows.Forms.TabPage tabPageAddNotes;
        private System.Windows.Forms.ListBox listBoxAccountList;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.DataGridView dataGridViewTicket;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.TextBox txtBoxAddAccount;
        private System.Windows.Forms.Button btnRemoveAccountFromList;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.GroupBox groupBoxPlayersInvolved;
        private System.Windows.Forms.DataGridViewTextBoxColumn TicketId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TicketValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Server;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nickname;
        private System.Windows.Forms.DataGridViewTextBoxColumn StaffNickName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AnswerNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date2;
        private System.Windows.Forms.Label labelPageNr;
        private System.Windows.Forms.WebBrowser webBrowserTicketView;
        private System.Windows.Forms.ComboBox comboBoxAnswerTemplate;
        private System.Windows.Forms.RichTextBox txtBoxAnswerTicket;
        private System.Windows.Forms.Button btnSubmitAnswerTicket;
        private System.Windows.Forms.Button btnSelectAnswerTemplate;
        private System.Windows.Forms.Button btnGenerateNote;
        private System.Windows.Forms.CheckBox checkBoxAddToAllPlayers;
        private System.Windows.Forms.CheckBox checkBoxInclUID;
        private System.Windows.Forms.Button btnSubmitNote;
        private System.Windows.Forms.ComboBox comboBoxNoteTitle;
        private System.Windows.Forms.RichTextBox txtBoxNote;
        private System.Windows.Forms.LinkLabel linkLabelAllPlayerQuestion;
        private System.Windows.Forms.Button btnOpenBrowser;
    }
}
