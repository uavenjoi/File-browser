using System;
using System.Windows.Forms;
using System.Threading;

namespace FileBrowser
{
    internal class ExceptionHandler
    {
        // Handles the thread exception.
        public void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Show(e.Exception);
        }

        public void OnUnhandledExeption(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            Show(ex);
        }

        void Show(Exception ex)
        {
            try
            { // Exit the program if the user clicks Abort.
                DialogResult result = ShowExceptionDialog(ex);

                if (result == DialogResult.Abort)
                    Application.Exit();
            }
            catch
            { // Fatal error, terminate program
                try
                {
                    MessageBox.Show("Fatal Error", "Fatal Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MyLog.WriteError("Fatal Error");
                }
                finally
                {
                    MyLog.WriteError("Application exit");
                    Application.Exit();
                }
            }
        }

        private DialogResult ShowExceptionDialog(Exception ex)
        {
            string errorMessage =
                "Unhandled Exception:\n\n" +
                ex.Message + "\n\n" +
                ex.GetType() +
                "\n\nStack Trace:\n" +
                ex.StackTrace;

            MyLog.WriteError(errorMessage);//write to log

            return MessageBox.Show(errorMessage,
                "Application Error",
                MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Stop);
        }
    }
}
