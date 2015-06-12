using System;
using FileBrowser.Commands;
using System.Windows.Forms;
using FileBrowser;
namespace FileBrowser
{
    class PresenterProgress
    {
        double val;
        double totalVal;
        int totalProgress;
        Model.ModelProcessCopy model;
        Model.ModelProcessCopy currModel;
        CommandCopyFiles command;
        ProgressCopy progressForm;

        public PresenterProgress()
        {
            progressForm = new ProgressCopy();
            ViewMainForm.GetInstance.LanguageChange += OnLanguageChange;
        }

        public void Show()
        {
            progressForm.Show();
        }

        public void AddModel(Model.ModelProcessCopy m)
        {
            model = m;
            m.ProgressChanged += new EventHandler(OnProgressChanged);
            totalProgress = 0;
        }

        internal void AddCommand(CommandCopyFiles cmd)
        {
            command = cmd;
            if (command != null)
            {
                command.OnStarted += OnCopyStarted;
                command.OnPaused += OnPaused;
                command.OnCanceled += OnCancelChanged;
                command.OnFinished += OnFinishedChanged;
                command.OnResumed += OnCommandResume;
            }
        }


        private void OnCommandResume(object sender, EventArgs e)
        {
             progressForm.RenameButton(command.IsPaused);
        }

        private void OnLanguageChange(object sender, EventArgs e)
        {
             progressForm.Localize();
        }

        private void OnFinishedChanged(object sender, EventArgs e)
        {
             progressForm.HideForm();
        }

        private void OnPaused(object sender, EventArgs e)
        {
             progressForm.RenameButton(command.IsPaused);
        }

        private void OnCancelChanged(object sender, EventArgs e)
        {
            progressForm.HideForm();
        }

        private void OnFileChanged(object sender, EventArgs e)
        {
            totalProgress = model.TotalProgress;
            progressForm.SetNexFileName(model.CurrFile, model.FileDest);
        }

        private void OnPausedPress(object sender, EventArgs e)
        {
             command.Pause();
        }

        private void OnCancelPress(object sender, EventArgs e)
        {
           command.Cancel();
        }

        private void OnProgressChanged(object sender, EventArgs e)
        {
            currModel = sender as Model.ModelProcessCopy;
            if (currModel != null)
                NextStep(currModel.Progress);
        }

        private void OnCopyStarted(object sender, EventArgs e)
        {
            model.FileChanged += OnFileChanged;
            progressForm.Paused += OnPausedPress;
            progressForm.Canceled += OnCancelPress;
        }

        public void NextStep(long size)
        {
            try
            {
                val = ((double)size * 100.0 / (double)model.CurrentFileSize);
                totalVal = (double)size * 100.0 / (double)model.MaxValTotalLong;
                if (!Double.IsNaN(totalVal) && !Double.IsInfinity(totalVal) && !Double.IsNaN(val))
                {
                    if ((model.TotalProgress = Convert.ToInt32(totalProgress + totalVal)) > 100)
                        model.TotalProgress = 100;
                    if (val > 100)
                        val = 100;
                   progressForm.SetCurrentProgress(Convert.ToInt32(val));
                   progressForm.SetCurrentPercent(Convert.ToInt32(val));
                   progressForm.SetTotalProgress(model.TotalProgress);
                   progressForm.SetTotalPercent(model.TotalProgress);
                }
            }
            catch (Exception ex)
            {
                MyLog.WriteError(ex.Message, ex.StackTrace);
            }
        }
    }
}
