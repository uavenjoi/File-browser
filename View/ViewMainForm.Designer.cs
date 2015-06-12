namespace FileBrowser
{
    partial class ViewMainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.russianItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.panelActions = new System.Windows.Forms.Panel();
            this.btnNewFolder = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fileViewerLeft = new FileBrowser.Controls.FileViewer();
            this.fileViewerRight = new FileBrowser.Controls.FileViewer();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panelActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1070, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHiddenToolStripMenuItem,
            this.languageToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // showHiddenToolStripMenuItem
            // 
            this.showHiddenToolStripMenuItem.Checked = global::FileBrowser.Properties.Settings.Default.ShowHidden;
            this.showHiddenToolStripMenuItem.CheckOnClick = true;
            this.showHiddenToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showHiddenToolStripMenuItem.Name = "showHiddenToolStripMenuItem";
            this.showHiddenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showHiddenToolStripMenuItem.Text = "Show Hidden";
            this.showHiddenToolStripMenuItem.Click += new System.EventHandler(this.showHiddenToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.russianItem,
            this.englishItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.languageToolStripMenuItem.Text = "Language";
            // 
            // russianItem
            // 
            this.russianItem.Checked = global::FileBrowser.Properties.Settings.Default.RusLang;
            this.russianItem.Name = "russianItem";
            this.russianItem.Size = new System.Drawing.Size(114, 22);
            this.russianItem.Text = "Russian";
            this.russianItem.Click += new System.EventHandler(this.ChangeLanguage);
            // 
            // englishItem
            // 
            this.englishItem.Checked = global::FileBrowser.Properties.Settings.Default.EngLang;
            this.englishItem.Name = "englishItem";
            this.englishItem.Size = new System.Drawing.Size(114, 22);
            this.englishItem.Text = "English";
            this.englishItem.Click += new System.EventHandler(this.ChangeLanguage);
            // 
            // panelStatus
            // 
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 534);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(1070, 15);
            this.panelStatus.TabIndex = 1;
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.btnNewFolder);
            this.panelActions.Controls.Add(this.btnDelete);
            this.panelActions.Controls.Add(this.btnReplace);
            this.panelActions.Controls.Add(this.btnCopy);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(0, 505);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(1070, 29);
            this.panelActions.TabIndex = 2;
            // 
            // btnNewFolder
            // 
            this.btnNewFolder.Location = new System.Drawing.Point(320, 3);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new System.Drawing.Size(97, 23);
            this.btnNewFolder.TabIndex = 3;
            this.btnNewFolder.TabStop = false;
            this.btnNewFolder.Text = "F7 New folder";
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new System.EventHandler(this.BtnNewFolderClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(428, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "F8 Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDeleteClick);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(197, 3);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(107, 23);
            this.btnReplace.TabIndex = 1;
            this.btnReplace.TabStop = false;
            this.btnReplace.Text = "F6 Move";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(74, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(101, 23);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.TabStop = false;
            this.btnCopy.Text = "F5 Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fileViewerLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fileViewerRight);
            this.splitContainer1.Size = new System.Drawing.Size(1070, 481);
            this.splitContainer1.SplitterDistance = 503;
            this.splitContainer1.TabIndex = 3;
            this.splitContainer1.TabStop = false;
            // 
            // fileViewerLeft
            // 
            this.fileViewerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileViewerLeft.IsSelected = false;
            this.fileViewerLeft.Location = new System.Drawing.Point(0, 0);
            this.fileViewerLeft.Name = "fileViewerLeft";
            this.fileViewerLeft.Size = new System.Drawing.Size(503, 481);
            this.fileViewerLeft.TabIndex = 0;
            this.fileViewerLeft.Enter += new System.EventHandler(this.fileViewer_Enter);
            this.fileViewerLeft.Leave += new System.EventHandler(this.fileViewer_Leave);
            // 
            // fileViewerRight
            // 
            this.fileViewerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileViewerRight.IsSelected = false;
            this.fileViewerRight.Location = new System.Drawing.Point(0, 0);
            this.fileViewerRight.Name = "fileViewerRight";
            this.fileViewerRight.Size = new System.Drawing.Size(563, 481);
            this.fileViewerRight.TabIndex = 0;
            this.fileViewerRight.Enter += new System.EventHandler(this.fileViewer_Enter);
            this.fileViewerRight.Leave += new System.EventHandler(this.fileViewer_Leave);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ViewMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 549);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ViewMainForm";
            this.Text = "File Browser 0.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFileBrowser_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmFileBrowser_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHiddenToolStripMenuItem;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnNewFolder;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem russianItem;
        private System.Windows.Forms.ToolStripMenuItem englishItem;
        private Controls.FileViewer fileViewerRight;
        private Controls.FileViewer fileViewerLeft;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;

    }
}

