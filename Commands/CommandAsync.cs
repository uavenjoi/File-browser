using System;
using System.ComponentModel;
using System.Threading;

namespace FileBrowser.Commands
{
    public abstract class CommandAsync: Command
    {
        public event EventHandler OnCanceled;
        public event EventHandler OnError;
        public event EventHandler OnFinished;
        public event EventHandler OnStarted;
        public event EventHandler OnPaused;
        public event EventHandler OnResumed;
        
        public bool HasError { get; protected set; }
        public bool IsCanceled { get; protected set; }

        public bool IsPaused { get; private set; }
        protected AutoResetEvent busy;
        private BackgroundWorker bw;

        public CommandAsync()
        {
            busy = new AutoResetEvent(false);
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.RunWorkerCompleted += BwRunWorkerCompleted;
            bw.DoWork += (s, e) =>
            {
                if (OnStarted != null)
                    OnStarted(this, EventArgs.Empty);
                try
                {
                    Execute();
                }
                catch
                {
                    HasError = true;
                }
                finally
                {
                    IsFinished = true;
                }
                    if (IsCanceled)
                {
                    OnAbort();
                }
                if (HasError)
                {
                    OnErrorOccured();
                }
            };
        }

        private void BwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsFinished && OnFinished != null)
            {
                OnFinished(this, e);
            }
        }

        internal void Start()
        {
            bw.RunWorkerAsync();
        }

        internal void Pause()
        {
            IsPaused = !IsPaused;
            if (!IsPaused)
            {
                Resume();
            }
            if (IsPaused && OnPaused != null)
            {
                OnPaused(this, EventArgs.Empty);
            }
        }

        internal void Resume()
        {
            busy.Set();
            if (!IsPaused && OnResumed != null)
            {
                OnResumed(this,EventArgs.Empty);
            }
        }

        public void Stop()
        {
            Cancel();
        }

        internal void Cancel()
        {
            IsCanceled = true;
            if (IsPaused)
            {
                Resume();
            }
        }

        protected void OnErrorOccured()
        {
            if (OnError != null)
            {
                OnError(this, EventArgs.Empty);
            }
        }

        protected void OnAbort()
        {
            if (OnCanceled != null)
            {
                OnCanceled(this, EventArgs.Empty);
            }
        }
    }
}
