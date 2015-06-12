using System;
using System.Collections.Generic;

namespace FileBrowser.Model
{
    public class ModelProcessCopy
    {
        public event EventHandler ProgressChanged;
        public event EventHandler FileChanged;

        long progress;
        public long Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                if (ProgressChanged != null)
                    ProgressChanged(this, null);
            }
        }

        string currFile;//копируемый файл
        public string CurrFile
        {
            get { return currFile; }
            set
            {
                currFile = value;
                if (FileChanged != null)
                    FileChanged(this, null);
            }
        }

        string fileDest;
        public string FileDest
        {
            get { return fileDest; }
            set
            {
                fileDest = value;
                if (FileChanged != null)
                    FileChanged(this, null);
            }
        }

        public long CurrentFileSize { get; set; }

        public long MaxValTotalLong { get; set; }

        public int TotalProgress { get; set; }

        public List<string> Source { get; set; }

        public string SourceCurrentPath { get; set; }

        public string DestPath  { get; set; }
    }
}
