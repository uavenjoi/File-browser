namespace FileBrowser.Commands
{
    class CommandShowHidden : Command
    {
        readonly bool showHidden;

        public CommandShowHidden(bool show, Controls.FileViewer src, Controls.FileViewer dest)
        {
            showHidden = show;
            Condition = true;
         }

        public override void Execute()
        {
            Properties.Settings.Default.ShowHidden = showHidden;
       }
    }
}
