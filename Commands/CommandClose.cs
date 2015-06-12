using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FileBrowser;

namespace FileBrowser.Commands
{
    class CommandClose:Command
    {
        public CommandClose(bool cond)
        {
            Condition = cond;
        }

        public override void Execute()
        {
            CloseApp();
        }

        private void CloseApp()
        {
            if (CommandManager.GetActiveCommandsCount > 0)
            {
                DialogResult res = ViewMainForm.GetInstance.MessageShow(strings.copyInProgress, strings.question,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    CommandManager.OnAllCommandsCompleted += AppClose;
                    CommandManager.CancelAllCommands();
                }
            }
            else
                ViewMainForm.GetInstance.AppClose(this, EventArgs.Empty);
        }

        private void AppClose(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
