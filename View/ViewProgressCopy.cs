using System;
using System.Windows.Forms;

namespace FileBrowser
{
    public partial class ProgressCopy : Form,IProgressView
    {
        public ProgressCopy()
        {
            InitializeComponent();
            Localize();
        }

        private void InvokeOrExecute(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public void Localize()
        {
            this.Text = strings.ProgressCopyName;
            lblFrom.Text = strings.FromTo;
            lblTo.Text = strings.To;
            lblCurrent.Text = strings.CurrentProgress;
            lblTotal.Text = strings.TotalProgress;
            btnCancel.Text = strings.Cancel;
            btnPause.Text = strings.Pause;
        }

        public void SetTotalProgress(int progress)
        {
           Action RunCommand = delegate()
          {
              pbTotalProgress.Value = progress;
          };
            InvokeOrExecute(RunCommand);
        }

        public void SetCurrentProgress(int progress)
        {
            Action RunCommand = delegate()
            {
                pbCurrentProgress.Value = progress;
            };
            InvokeOrExecute(RunCommand);
        }

        public void SetNexFileName(string source, string dest)
        {
            Action RunCommand = delegate()
            {
                lblSource.Text =  source;
                lblDest.Text =  dest;
            };
            InvokeOrExecute(RunCommand);
        }

        public void SetCurrentPercent(int percent)
        {
            Action RunCommand = delegate()
            {
                lblProgress.Text = percent.ToString();
            };
            InvokeOrExecute(RunCommand);
        }

        public void SetTotalPercent(int percent)
        {
            Action RunCommand = delegate()
            {
                lblTotalProgress.Text = percent.ToString();
            };
            InvokeOrExecute(RunCommand);
        }

        public void ShowForm()
        {
            Action RunCommand = Show;
            InvokeOrExecute(RunCommand);
        }

        public void HideForm()
        {
            Action RunCommand = Hide;
            InvokeOrExecute(RunCommand);
        }

         public void RenameButton(bool isPaused)
         {
             Action RunCommand = delegate()
             {
                 btnPause.Text = isPaused ? strings.Start : strings.Pause;
             };
             InvokeOrExecute(RunCommand);
         }

         public void ShowMessageError(string msg)
         {
             MessageBox.Show(msg,strings.Error,MessageBoxButtons.OK,MessageBoxIcon.Error);
         }

         #region EVENTS

        public event EventHandler<EventArgs> Canceled;
        public event EventHandler<EventArgs> Paused;

         private void btnCancel_Click(object sender, EventArgs e)
         {
            if(Canceled!=null)
                Canceled(this, e);
         }

         private void btnPause_Click(object sender, EventArgs e)
         {
             if (Paused != null)
                 Paused(this, e);
         }
         #endregion
    }
}