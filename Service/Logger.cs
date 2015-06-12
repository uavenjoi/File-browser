using NLog;

namespace FileBrowser
{
    class MyLog
    {
        public static Logger logger = LogManager.GetLogger("Log.txt");

        public static void WriteError(string msgError)
        {
            logger.Error(msgError);
        }

        public static void WriteError(string msgError, string trace)
        {
            WriteError(msgError+" "+trace);
        }

        public static void WriteInfo(string msgInfo)
        {
            logger.Info(msgInfo);
        }
    }
}
