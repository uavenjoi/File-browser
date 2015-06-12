using System;

namespace FileBrowser
{
    interface IView
    {
        event EventHandler<EventArgs> CopyPerformed;
        event EventHandler<EventArgs> ReplacePerformed;
        event EventHandler<EventArgs> NewFolderPerformed;
        event EventHandler<EventArgs> DeletePerformed;
        event EventHandler<EventArgs> FormClose;
        event EventHandler<EventArgs> ShowHidden;
        event EventHandler<EventArgs> BeforeFormClosing;
        event EventHandler<EventArgs> LanguageChange;
        event EventHandler<EventArgs> LeftActivated;
        event EventHandler<EventArgs> RightActivated;
        event EventHandler<EventArgs> BackSpacePressed;
        void Localize();
     }
}