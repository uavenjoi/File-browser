using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace FileBrowser
{
    public partial class ViewMainForm : Form, IView
    {
        public Controls.FileViewer Source
        { get; set; }

        public Controls.FileViewer Dest
        { get; set; }

        public string SelectedLanguage
        { get; private set; }

       public event EventHandler<EventArgs> CopyPerformed;
       public event EventHandler<EventArgs> ReplacePerformed;
       public event EventHandler<EventArgs> NewFolderPerformed;
       public event EventHandler<EventArgs> DeletePerformed;
       public event EventHandler<EventArgs> FormClose;
       public event EventHandler<EventArgs> ShowHidden;
       public event EventHandler<EventArgs> BeforeFormClosing;
       public event EventHandler<EventArgs> LanguageChange;
       public event EventHandler<EventArgs> LeftActivated;
       public event EventHandler<EventArgs> RightActivated;
       public event EventHandler<EventArgs> BackSpacePressed;

       private ViewMainForm()
       {
           InitializeComponent();
           Source = fileViewerLeft;
           Dest = fileViewerRight;
           showHiddenToolStripMenuItem.Checked = Properties.Settings.Default.ShowHidden;
       }

      private static volatile object syncLock=new object();

      private static ViewMainForm instance;

      public static ViewMainForm GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                            instance = new ViewMainForm();
                    }
                }
                return instance;
            }
        }

      private void showHiddenToolStripMenuItem_Click(object sender, EventArgs e)
      {
        if (ShowHidden != null)
        {
            ShowHidden(this,EventArgs.Empty);
        }
      }

      private void btnCopy_Click(object sender, EventArgs e)
      {
          if (CopyPerformed != null)
          {
              CopyPerformed(this, EventArgs.Empty);
          }
      }

      private void btnReplace_Click(object sender, EventArgs e)
      {
          if (ReplacePerformed != null)
          {
              ReplacePerformed(this, EventArgs.Empty);
          }

      }

      private void BtnDeleteClick(object sender, EventArgs e)
      {
        if (DeletePerformed != null)
          {
              DeletePerformed(this,EventArgs.Empty);
          }
      }

      private void BtnNewFolderClick(object sender, EventArgs e)
      {
          if (NewFolderPerformed != null)
          {
              NewFolderPerformed(this,EventArgs.Empty);
          }
      }

      private void exitToolStripMenuItem_Click(object sender, EventArgs e)
      {
          if (FormClose != null)
          {
              FormClose(this, EventArgs.Empty);
          }
      }

      private void fileViewer_Enter(object sender, EventArgs e)
      {
          var viewer = sender as Controls.FileViewer;
          if (viewer != null)
              if (String.Equals(viewer.Name, "fileViewerLeft",StringComparison.CurrentCultureIgnoreCase) && LeftActivated != null)
              {
                  LeftActivated(this, EventArgs.Empty);
              }
              else if (String.Equals(viewer.Name , "fileViewerRight",StringComparison.CurrentCultureIgnoreCase) && RightActivated != null)
              {
                  RightActivated(this, EventArgs.Empty);
              }
      }

      private void fileViewer_Leave(object sender, EventArgs e)
      {
          var viewer = sender as Controls.FileViewer;
          if (viewer != null)
          {
              viewer.IsSelected = false;
          }
      }

      private void frmFileBrowser_FormClosing(object sender, FormClosingEventArgs e)
      {
          if (e.CloseReason != CloseReason.ApplicationExitCall)//если был вызван Application.Exit() то просто выходим
          {
              e.Cancel = true;
              if (BeforeFormClosing != null)
              {
                  BeforeFormClosing(this,e);
              }
          }
        }

      public void AppClose(object sender, EventArgs e)
      {
          Application.Exit();
      }

      private void frmFileBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F5: if(CopyPerformed != null)
                                 CopyPerformed(this, EventArgs.Empty);
                    break;
                case Keys.F6: if (ReplacePerformed != null)
                                  ReplacePerformed(this, EventArgs.Empty);
                    break;
                case Keys.F7: if(NewFolderPerformed != null)
                                 NewFolderPerformed(this, EventArgs.Empty);
                    break;
                case Keys.F8:     DeletePerform();
                    break;
                case Keys.Delete: DeletePerform();
                   break;
                case Keys.Back:
                   if (BackSpacePressed != null)
                       BackSpacePressed(this,EventArgs.Empty);
                    break;
            }
        }

      private void DeletePerform()
      {
          if (DeletePerformed != null)
              DeletePerformed(this, EventArgs.Empty);
      }

      public DialogResult MessageShow(string msg, string caption,
                                      MessageBoxButtons buttons, MessageBoxIcon icon, params string[] p)
      {
          string message = String.Format(msg, p);
          DialogResult res = MessageShow(message, caption, buttons, icon);
          return res;
      }

      public DialogResult MessageShow(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
      {
          DialogResult res = MessageBox.Show(message, caption, buttons, icon);
          return res;
      }

      public string ShowInputBox(string msg, string caption, string defaultValue)
      {
        return  Interaction.InputBox(msg, caption, defaultValue, 500, 500);
      }

      private void ChangeLanguage(object sender, EventArgs e)
      {
          var item = sender as ToolStripMenuItem;
          SelectedLanguage = item.Name;
          if (LanguageChange != null)
          {
              LanguageChange(this,EventArgs.Empty);
          }
      }

      public void Localize()
      {
          btnCopy.Text = strings.Copy;
          btnDelete.Text = strings.Delete;
          btnNewFolder.Text = strings.NewFolder;
          btnReplace.Text = strings.Move;
          exitToolStripMenuItem.Text = strings.Exit;
          languageToolStripMenuItem.Text = strings.Lang;
          settingsToolStripMenuItem.Text = strings.Settings;
          fileToolStripMenuItem.Text = strings.File;
          showHiddenToolStripMenuItem.Text = strings.ShowHidden;
          englishItem.Checked = Properties.Settings.Default.EngLang;
          russianItem.Checked = Properties.Settings.Default.RusLang;
      }

      private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
      {
          MessageBox.Show("File browser 0.5 \nKaraganda \nSokolov M. 2012","About...");
      }
    }
}