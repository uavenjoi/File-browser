using System;

namespace FileBrowser
{
    interface IProgressView
    {
        void SetTotalProgress(int progress);
        void SetCurrentProgress(int progress);
        void SetNexFileName(string source,string dest);
        void SetCurrentPercent(int percent);
        void SetTotalPercent(int percent);
        void ShowMessageError(string msg);
        void RenameButton(bool b);
        void Localize();
        void ShowForm();
        void HideForm();
        event EventHandler<EventArgs> Paused;
        event EventHandler<EventArgs> Canceled;
   }
}
