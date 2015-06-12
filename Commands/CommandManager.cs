using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace FileBrowser.Commands
{
    public class CommandManager
    {
        public static event EventHandler OnCommandFinished;
        public static event EventHandler OnAllCommandsCompleted;
        private static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private static List<Command> _listCommands = new List<Command>();

        private static List<Command> ListCommands
        {
            get
            {
                locker.EnterReadLock();
                try
                {
                    return _listCommands;
                }
                finally
                {
                    locker.ExitReadLock();
                }
            }
            set
            {
                locker.EnterWriteLock();
                try
                {
                    ViewMainForm.GetInstance.Refresh();
                    _listCommands = value;
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }
        }

        //количество запущенных команд
        public static int GetActiveCommandsCount
        {
            get { return ListCommands.Count; }
        }

        public static void Execute(Command cmd)
        {
            if (cmd.Condition)
            {
                if (cmd is CommandAsync)
                {
                    var command = (cmd as CommandAsync);
                    ListCommands.Add(command);
                    command.OnFinished += (s, e) => ListCommands.Remove(cmd as CommandAsync);
                    command.OnFinished += new EventHandler(OnFinished);
                    command.OnCanceled += new EventHandler(OnCanceled);
                    command.OnError += new EventHandler(OnError);
                    command.Start();
                }
                else if (cmd is Command)
                {
                    cmd.Execute();
                    if (OnCommandFinished != null)
                    {
                        OnCommandFinished(null, EventArgs.Empty);
                    }
                }
            }
            else ViewMainForm.GetInstance.MessageShow(cmd.errorMessage, strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void OnFinished(object sender, EventArgs e)
        {
            if (OnCommandFinished != null)
            {
                OnCommandFinished(null, null);
            }
            if (ListCommands.Count == 0 && OnAllCommandsCompleted != null)
            {
                OnAllCommandsCompleted(null, EventArgs.Empty);
            }
        }

        private static void OnError(object sender, EventArgs e)
        {
         ViewMainForm.GetInstance.MessageShow(strings.operationCanceled, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void OnCanceled(object sender, EventArgs e)
        {
            if (OnAllCommandsCompleted == null)
            {
                ViewMainForm.GetInstance.MessageShow(strings.operationCanceled, strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void CancelAllCommands()
        {
            foreach (CommandAsync cmd in ListCommands)
            {
                cmd.Stop();
            }
        }
    }
}
