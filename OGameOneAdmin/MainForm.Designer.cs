namespace OGameOneAdmin
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            OGameOneAdmin.Utilities.ListViewColumnSorter listViewColumnSorter1 = new OGameOneAdmin.Utilities.ListViewColumnSorter();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToAllUniversesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutFromAllUniversesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.synchronizeDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dashboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDashboard = new System.Windows.Forms.TabPage();
            this.listViewUniverse = new OGameOneAdmin.Controls.FlickerFreeListView();
            this.columnHeaderCommunityName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderUniNumber = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderUniName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDomain = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSessionStatus = new System.Windows.Forms.ColumnHeader();
            this.universeListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.universeIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quickLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginWithInternetExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginWithAUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkSessionStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.EmptyToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataDownloadedToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataDownloadedtoolStripStatusLabelValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.mainMenu.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageDashboard.SuspendLayout();
            this.universeListContextMenuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenu.Size = new System.Drawing.Size(792, 24);
            this.mainMenu.TabIndex = 5;
            this.mainMenu.Text = "mainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToAllUniversesToolStripMenuItem,
            this.logoutFromAllUniversesToolStripMenuItem,
            this.toolStripSeparator5,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loginToAllUniversesToolStripMenuItem
            // 
            this.loginToAllUniversesToolStripMenuItem.Name = "loginToAllUniversesToolStripMenuItem";
            this.loginToAllUniversesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
            this.loginToAllUniversesToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.loginToAllUniversesToolStripMenuItem.Text = "Login to All Universes";
            this.loginToAllUniversesToolStripMenuItem.Click += new System.EventHandler(this.loginToAllUniversesToolStripMenuItem_Click);
            // 
            // logoutFromAllUniversesToolStripMenuItem
            // 
            this.logoutFromAllUniversesToolStripMenuItem.Name = "logoutFromAllUniversesToolStripMenuItem";
            this.logoutFromAllUniversesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F2)));
            this.logoutFromAllUniversesToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.logoutFromAllUniversesToolStripMenuItem.Text = "Logout from All Universes";
            this.logoutFromAllUniversesToolStripMenuItem.Click += new System.EventHandler(this.logoutFromAllUniversesToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(233, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.Visible = false;
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(114, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(114, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lockApplicationToolStripMenuItem,
            this.minimizeToTrayToolStripMenuItem,
            this.synchronizeDataToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // lockApplicationToolStripMenuItem
            // 
            this.lockApplicationToolStripMenuItem.Name = "lockApplicationToolStripMenuItem";
            this.lockApplicationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.lockApplicationToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.lockApplicationToolStripMenuItem.Text = "Lock Application";
            this.lockApplicationToolStripMenuItem.Click += new System.EventHandler(this.lockApplicationToolStripMenuItem_Click);
            // 
            // minimizeToTrayToolStripMenuItem
            // 
            this.minimizeToTrayToolStripMenuItem.Name = "minimizeToTrayToolStripMenuItem";
            this.minimizeToTrayToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.minimizeToTrayToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.minimizeToTrayToolStripMenuItem.Text = "Minimize to tray";
            this.minimizeToTrayToolStripMenuItem.Click += new System.EventHandler(this.minimizeToTrayToolStripMenuItem_Click);
            // 
            // synchronizeDataToolStripMenuItem
            // 
            this.synchronizeDataToolStripMenuItem.Name = "synchronizeDataToolStripMenuItem";
            this.synchronizeDataToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.synchronizeDataToolStripMenuItem.Text = "Synchronize Dashboard";
            this.synchronizeDataToolStripMenuItem.Visible = false;
            this.synchronizeDataToolStripMenuItem.Click += new System.EventHandler(this.synchronizeDataToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem,
            this.tipsToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // tipsToolStripMenuItem
            // 
            this.tipsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dashboardToolStripMenuItem});
            this.tipsToolStripMenuItem.Name = "tipsToolStripMenuItem";
            this.tipsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.tipsToolStripMenuItem.Text = "Tips";
            // 
            // dashboardToolStripMenuItem
            // 
            this.dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            this.dashboardToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.dashboardToolStripMenuItem.Text = "Dashboard";
            this.dashboardToolStripMenuItem.Click += new System.EventHandler(this.dashboardToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(160, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageDashboard);
            this.tabControlMain.Location = new System.Drawing.Point(14, 36);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.Padding = new System.Drawing.Point(10, 5);
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(766, 497);
            this.tabControlMain.TabIndex = 6;
            this.tabControlMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlMain_Selected);
            // 
            // tabPageDashboard
            // 
            this.tabPageDashboard.Controls.Add(this.listViewUniverse);
            this.tabPageDashboard.Location = new System.Drawing.Point(4, 26);
            this.tabPageDashboard.Name = "tabPageDashboard";
            this.tabPageDashboard.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDashboard.Size = new System.Drawing.Size(758, 467);
            this.tabPageDashboard.TabIndex = 0;
            this.tabPageDashboard.Text = "Dashboard";
            this.tabPageDashboard.UseVisualStyleBackColor = true;
            // 
            // listViewUniverse
            // 
            this.listViewUniverse.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewUniverse.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCommunityName,
            this.columnHeaderUniNumber,
            this.columnHeaderUniName,
            this.columnHeaderDomain,
            this.columnHeaderSessionStatus});
            this.listViewUniverse.ContextMenuStrip = this.universeListContextMenuStrip;
            this.listViewUniverse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewUniverse.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewUniverse.FullRowSelect = true;
            this.listViewUniverse.GridLines = true;
            this.listViewUniverse.LabelWrap = false;
            listViewColumnSorter1.Order = System.Windows.Forms.SortOrder.Ascending;
            listViewColumnSorter1.SortColumn = 0;
            this.listViewUniverse.ListViewColumnSorter = listViewColumnSorter1;
            this.listViewUniverse.Location = new System.Drawing.Point(3, 3);
            this.listViewUniverse.MultiSelect = false;
            this.listViewUniverse.Name = "listViewUniverse";
            this.listViewUniverse.Size = new System.Drawing.Size(752, 461);
            this.listViewUniverse.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewUniverse.TabIndex = 0;
            this.listViewUniverse.UseCompatibleStateImageBehavior = false;
            this.listViewUniverse.View = System.Windows.Forms.View.Details;
            this.listViewUniverse.SelectedIndexChanged += new System.EventHandler(this.listViewUniverse_SelectedIndexChanged);
            this.listViewUniverse.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewUniverse_ColumnClick);
            // 
            // columnHeaderCommunityName
            // 
            this.columnHeaderCommunityName.Text = "Community";
            this.columnHeaderCommunityName.Width = 84;
            // 
            // columnHeaderUniNumber
            // 
            this.columnHeaderUniNumber.Text = "Uni Number";
            this.columnHeaderUniNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderUniNumber.Width = 73;
            // 
            // columnHeaderUniName
            // 
            this.columnHeaderUniName.Text = "Uni Name";
            this.columnHeaderUniName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderUniName.Width = 102;
            // 
            // columnHeaderDomain
            // 
            this.columnHeaderDomain.Text = "Domain";
            this.columnHeaderDomain.Width = 100;
            // 
            // columnHeaderSessionStatus
            // 
            this.columnHeaderSessionStatus.Text = "Session Status";
            this.columnHeaderSessionStatus.Width = 100;
            // 
            // universeListContextMenuStrip
            // 
            this.universeListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.universeIdToolStripMenuItem,
            this.toolStripSeparator1,
            this.quickLoginToolStripMenuItem,
            this.loginWithToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.openInBrowserToolStripMenuItem,
            this.checkSessionStatusToolStripMenuItem});
            this.universeListContextMenuStrip.Name = "universeListContextMenuStrip";
            this.universeListContextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.universeListContextMenuStrip.ShowImageMargin = false;
            this.universeListContextMenuStrip.Size = new System.Drawing.Size(152, 142);
            this.universeListContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.universeListContextMenuStrip_Opening);
            // 
            // universeIdToolStripMenuItem
            // 
            this.universeIdToolStripMenuItem.Enabled = false;
            this.universeIdToolStripMenuItem.Name = "universeIdToolStripMenuItem";
            this.universeIdToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.universeIdToolStripMenuItem.Text = "Universe Id";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
            // 
            // quickLoginToolStripMenuItem
            // 
            this.quickLoginToolStripMenuItem.Name = "quickLoginToolStripMenuItem";
            this.quickLoginToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.quickLoginToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.quickLoginToolStripMenuItem.Text = "Quick Login";
            this.quickLoginToolStripMenuItem.Click += new System.EventHandler(this.quickLoginToolStripMenuItem_Click);
            // 
            // loginWithToolStripMenuItem
            // 
            this.loginWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginWithInternetExplorerToolStripMenuItem,
            this.loginWithAUrlToolStripMenuItem});
            this.loginWithToolStripMenuItem.Name = "loginWithToolStripMenuItem";
            this.loginWithToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.loginWithToolStripMenuItem.Text = "Or Login with...";
            // 
            // loginWithInternetExplorerToolStripMenuItem
            // 
            this.loginWithInternetExplorerToolStripMenuItem.Name = "loginWithInternetExplorerToolStripMenuItem";
            this.loginWithInternetExplorerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.loginWithInternetExplorerToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.loginWithInternetExplorerToolStripMenuItem.Text = "Internet Explorer";
            this.loginWithInternetExplorerToolStripMenuItem.Click += new System.EventHandler(this.loginWithInternetExplorerToolStripMenuItem_Click);
            // 
            // loginWithAUrlToolStripMenuItem
            // 
            this.loginWithAUrlToolStripMenuItem.Name = "loginWithAUrlToolStripMenuItem";
            this.loginWithAUrlToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.loginWithAUrlToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.loginWithAUrlToolStripMenuItem.Text = "A Url";
            this.loginWithAUrlToolStripMenuItem.Click += new System.EventHandler(this.loginWithAUrlToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // openInBrowserToolStripMenuItem
            // 
            this.openInBrowserToolStripMenuItem.Name = "openInBrowserToolStripMenuItem";
            this.openInBrowserToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.openInBrowserToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.openInBrowserToolStripMenuItem.Text = "Open in Browser";
            this.openInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openInBrowserToolStripMenuItem_Click);
            // 
            // checkSessionStatusToolStripMenuItem
            // 
            this.checkSessionStatusToolStripMenuItem.Name = "checkSessionStatusToolStripMenuItem";
            this.checkSessionStatusToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.checkSessionStatusToolStripMenuItem.Text = "Check Session Status";
            this.checkSessionStatusToolStripMenuItem.Click += new System.EventHandler(this.checkSessionStatusToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowItemReorder = true;
            this.statusStrip1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EmptyToolStripStatusLabel,
            this.dataDownloadedToolStripStatusLabel,
            this.dataDownloadedtoolStripStatusLabelValue});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "bottomStatusStrip";
            // 
            // EmptyToolStripStatusLabel
            // 
            this.EmptyToolStripStatusLabel.Name = "EmptyToolStripStatusLabel";
            this.EmptyToolStripStatusLabel.Size = new System.Drawing.Size(648, 17);
            this.EmptyToolStripStatusLabel.Spring = true;
            // 
            // dataDownloadedToolStripStatusLabel
            // 
            this.dataDownloadedToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.dataDownloadedToolStripStatusLabel.Name = "dataDownloadedToolStripStatusLabel";
            this.dataDownloadedToolStripStatusLabel.Size = new System.Drawing.Size(113, 17);
            this.dataDownloadedToolStripStatusLabel.Text = "Data Downloaded:";
            this.dataDownloadedToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.dataDownloadedToolStripStatusLabel.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // dataDownloadedtoolStripStatusLabelValue
            // 
            this.dataDownloadedtoolStripStatusLabelValue.Name = "dataDownloadedtoolStripStatusLabelValue";
            this.dataDownloadedtoolStripStatusLabelValue.Size = new System.Drawing.Size(14, 17);
            this.dataDownloadedtoolStripStatusLabelValue.Text = "0";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "OGame One Admin";
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.tabControlMain);
            this.Enabled = false;
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OGame One Admin";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDashboard.ResumeLayout(false);
            this.universeListContextMenuStrip.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDashboard;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private OGameOneAdmin.Controls.FlickerFreeListView listViewUniverse;
        private System.Windows.Forms.ColumnHeader columnHeaderUniNumber;
        private System.Windows.Forms.ColumnHeader columnHeaderUniName;
        private System.Windows.Forms.ColumnHeader columnHeaderCommunityName;
        private System.Windows.Forms.ColumnHeader columnHeaderDomain;
        private System.Windows.Forms.ColumnHeader columnHeaderSessionStatus;
        private System.Windows.Forms.ContextMenuStrip universeListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem universeIdToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel dataDownloadedToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel dataDownloadedtoolStripStatusLabelValue;
        private System.Windows.Forms.ToolStripStatusLabel EmptyToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem quickLoginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginWithInternetExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginWithAUrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkSessionStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem synchronizeDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem minimizeToTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToAllUniversesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutFromAllUniversesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tipsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dashboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}