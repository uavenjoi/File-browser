using System;
using System.Windows.Forms;
using System.Threading;

namespace FileBrowser
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var presenterMainForm = new PresenterMainForm<ViewMainForm>(ViewMainForm.GetInstance);
            var handler = new ExceptionHandler();
            AppDomain.CurrentDomain.UnhandledException +=  new UnhandledExceptionEventHandler(handler.OnUnhandledExeption);
            Application.ThreadException +=new ThreadExceptionEventHandler(handler.Application_ThreadException);
            Application.Run(ViewMainForm.GetInstance);
        }
    }
}
