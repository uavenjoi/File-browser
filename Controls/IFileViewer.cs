using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace FileBrowser.Controls
{
   public interface IFileViewer
    {
        void SetLocalDrives(Button[] drives);
        void SetFilesList(ListViewItem[] items,ImageList icons);
        void ClearSelected();
        void ClearFileList();
        void SetCurrentPathHeader(string path);
        void SetActive();
        void SetNotActive();
        void SetTabHeaders(string s);
        void SetNewTab(int index,string s);
        void Localize();
        void ShowPastMenu(bool isVisible);
        void TabChange(ListView listView);
        void DeleteTab(TabPage page);
        void ClearTabs();
        void SetTabPageActive(int index);
        void SetDefaultPageTag(int i);
      

        DialogResult MessageShow(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        ListView.SelectedListViewItemCollection GetSelectedItems();
        ListViewItem GetItemDest{get;}
        int GetCurrentTab { get; }
       
       event EventHandler Loaded;
        event EventHandler ItemActivated;
        event EventHandler DragDropped;
        event EventHandler MouseViewerLeaved;
        event EventHandler FileRenamed; 
        event EventHandler FileDeleted;
        event EventHandler FileCopy;
        event EventHandler FilePaste;
        event EventHandler FileCut;
        event EventHandler ContextMenuOpened;
        event EventHandler TabActivated;
        event EventHandler NewTabAdded;
        event EventHandler TabDeleted;
    }
}
