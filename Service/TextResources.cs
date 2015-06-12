using System;
using System.Resources;
using System.Threading;
using System.Globalization;

namespace FileBrowser.Service
{
    class TextRes
    {
        public static  ResourceManager res;
      
        private static string currentCulture="en-GB";
        public static string CurrentCulture
        {
            get {return currentCulture; }
            set 
            {
                currentCulture = value;
                SetCulture();
            }
        }

        public static void SetCulture()
        {
            try
            {
                if (!String.IsNullOrEmpty(CurrentCulture))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                MyLog.logger.Error(ex.StackTrace);
            }
        }
    }
}
