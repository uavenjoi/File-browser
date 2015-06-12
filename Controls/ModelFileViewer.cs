using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileBrowser.Controls
{
    class ModelFileViewer
    {
        public ModelFileViewer()
        {
            _listTabs = new Dictionary<int, string>();
        }

        public string CurrentPath { get; set; }

        public ListView ListViewFiles { get; set; }

        public int CurrentTab { get; set; }

        private  Dictionary<int, string> _listTabs ;

        public int GetTabsCount
        {
            get
            {
                if (_listTabs == null)
                    return 0;
                return _listTabs.Count; 
            }
        }

        public Dictionary<int, string> ListTabs
        {
            get { return _listTabs; }
            set { _listTabs = value; }
        }
    }

}

