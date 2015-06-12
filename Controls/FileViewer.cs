using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using FileBrowser.Service;
using FileBrowser.Model;
using FileBrowser.Commands;

namespace FileBrowser.Controls
{
    public partial class FileViewer : UserControl, IFileViewer
    {
        private static ListView.SelectedListViewItemCollection items;
        private static ListViewItem itemDest;
        public event EventHandler Loaded;
        public event EventHandler ItemActivated;
        public event EventHandler DragDropped;
        public event EventHandler MouseViewerLeaved;
        public event EventHandler FileRenamed;
        public event EventHandler FileDeleted;
        public event EventHandler FileCopy;
        public event EventHandler FilePaste;
        public event EventHandler FileCut;
        public event EventHandler ContextMenuOpened;
        public event EventHandler TabActivated;
        public event EventHandler NewTabAdded;
        public event EventHandler TabDeleted;

        private int pageIndex;

        public ListView.SelectedListViewItemCollection GetItems
        {
            get { return items; }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (isSelected)
                    SetActive();
                else
                    SetNotActive();
            }
        }

        public void Localize()
        {
            deleteToolStripMenuItem.Text = strings.DeleteMenu;
            copyToolStripMenuItem.Text = strings.CopyMenu;
            cutToolStripMenuItem.Text = strings.CutMenu;
            pasteToolStripMenuItem.Text = strings.PasteMenu;
            renameToolStripMenuItem.Text = strings.RenameMenu;
            openToolStripMenuItem.Text = strings.OpenMenu;
            addToolStripMenuItem.Text = strings.AddTab;
            deleteTabToolStripMenuItem.Text = strings.DelTab;
        }

        //получение папки, куда хотим скопировать файлы при Drag&Drop
        public ListViewItem GetItemDest
        {
            get { return itemDest; }
        }
        public FileViewer()
        {
            InitializeComponent();
            
        }

        public void SetTabHeaders(string name)
        {
            tabControl.TabPages[tabControl.SelectedIndex].Text = name;
        }

        private void FileViewer_Load(object sender, EventArgs e)
        {
            if (Loaded != null)
                Loaded(this, EventArgs.Empty);
        }

        public void ShowPastMenu(bool isEnabled)
        {
            pasteToolStripMenuItem.Enabled = isEnabled;
        }

        //заполнение панели с дисками
        public void SetLocalDrives(Button[] drives)
        {
            drivesPanel.Controls.AddRange(drives);
        }

        //снимаем выделение
        public void ClearSelected()
        {
            listViewFiles.SelectedIndices.Clear();
        }

        public void ClearFileList()
        {
            if (this.InvokeRequired)
                this.Invoke((Action)(() =>
                {
                    listViewFiles.Clear();
                }));
            else listViewFiles.Clear();
        }

        public void BuildHeaders(ListView lv)
        {
            ColumnHeader[] columns = new ColumnHeader[4];
            columns[0] = new ColumnHeader();
            columns[0].Text = strings.FileColumn;
            columns[0].Width = 300;
            columns[1] = new ColumnHeader();
            columns[1].Text = strings.TypeColumn;
            columns[1].Width = 60;
            columns[2] = new ColumnHeader();
            columns[2].Text = strings.SizeColumn;
            columns[2].Width = 60;
            columns[3] = new ColumnHeader();
            columns[3].Text = strings.DateColumn;
            columns[3].Width = 120;
            lv.Columns.AddRange(columns);
        }

        public void SetFilesList(ListViewItem[] items, ImageList icons)
        {
            listViewFiles.SmallImageList = icons;
            BuildHeaders(listViewFiles);
            listViewFiles.BeginUpdate();
            ListView.ListViewItemCollection lvic = new ListView.ListViewItemCollection(listViewFiles);
            lvic.AddRange(items);
            listViewFiles.EndUpdate();
            CancelSelection();
        }

        //отображение текущей директории
        public void SetCurrentPathHeader(string path)
        {
            txtCurrentPath.Text = path;
        }

        //получение выделенных файлов
        public ListView.SelectedListViewItemCollection GetSelectedItems()
        {
            var selectedItems = listViewFiles.SelectedItems;
            if (selectedItems.Count == 0)
                selectedItems = GetItems;
            return selectedItems;
        }

        //количество выделенных элементов
        public int GetSourceCount
        {
            get { return listViewFiles.SelectedItems.Count; }
        }

        //граница контрола меняет вид на активный
        public void SetActive()
        {
            listViewFiles.BorderStyle = BorderStyle.Fixed3D;
        }

        public void SetNotActive()
        {
            listViewFiles.BorderStyle = BorderStyle.None;
        }

        //отмена выделения
        public void CancelSelection()
        {
            if (listViewFiles.Items.Count > 0)
                for (int i = 0; i < listViewFiles.SelectedIndices.Count; i++)
                {
                    listViewFiles.Items[listViewFiles.SelectedIndices[i]].Selected = false;
                }
        }

        //нажатие на файл или папку
        private void lv_ItemActivate(object sender, EventArgs e)
        {
            ListView list = ((ListView)sender);
            if (ItemActivated != null)
            {
                ItemActivated(sender, EventArgs.Empty);
            }
        }

        private void listViewFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var listView = sender as ListView;
            listView.DoDragDrop(listView.SelectedItems, DragDropEffects.Copy);
        }

        private void listViewFiles_DragEnter(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;
            if (e.Data.GetDataPresent("System.Windows.Forms.ListView+SelectedListViewItemCollection") && e.AllowedEffect == DragDropEffects.Copy)
                e.Effect = DragDropEffects.Copy;
        }

        private void listViewFiles_DragDrop(object sender, DragEventArgs e)
        {
            items = e.Data.GetData("System.Windows.Forms.ListView+SelectedListViewItemCollection") as ListView.SelectedListViewItemCollection;
            Point p = new Point(e.X, e.Y);
            itemDest = GetItemFromPoint(listViewFiles, p);
            if (DragDropped != null)
            {
                DragDropped(this, EventArgs.Empty);
            }
        }

        //получение ListViewItem на которую наведен курсор
        private ListViewItem GetItemFromPoint(ListView listView, Point mousePosition)
        {
            Point localPoint = listView.PointToClient(mousePosition);
            return listView.GetItemAt(localPoint.X, localPoint.Y);
        }

        private void listViewFiles_MouseLeave(object sender, EventArgs e)
        {
            if (MouseViewerLeaved != null)
                MouseViewerLeaved(this, EventArgs.Empty);
        }

        public DialogResult MessageShow(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult res = MessageBox.Show(message, caption, buttons, icon);
            return res;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sender = listViewFiles;
            if (FileRenamed != null)
            {
                FileRenamed(sender,EventArgs.Empty);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sender = listViewFiles;
            if (ItemActivated != null)
            {
                ItemActivated(sender, EventArgs.Empty);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sender = listViewFiles;
            if (FileDeleted != null)
            {
                FileDeleted(sender,EventArgs.Empty);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sender = listViewFiles;
            if (FileCopy != null)
            {
                FileCopy(sender,EventArgs.Empty);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sender = listViewFiles;
            if (FilePaste != null)
            {
                FilePaste(sender,EventArgs.Empty); 
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sender = listViewFiles;
            if (FileCut != null)
            {
                FileCut(sender,EventArgs.Empty);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ContextMenuOpened != null)
            {
                ContextMenuOpened(this,EventArgs.Empty);
            }
        }

        private void TabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabActivated != null)
            {
                TabActivated(listViewFiles, EventArgs.Empty);
            }
        }

        public void TabChange( ListView listView)
        {
            tabControl.SelectedTab.Controls.Add(listView);
            tabControl.SelectedTab.Controls.Add(txtCurrentPath);
        }

        public void SetNewTab(int index, string text)
        {
            var page = new TabPage(text);
            page.Tag=index;
            tabControl.TabPages.Add(page);
        }

        public void DeleteTab(TabPage page)
        {
            tabControl.TabPages.Remove(page);
        }

        public void ClearTabs()
        {
            tabControl.TabPages.Clear();
        }

        public void SetTabPageActive(int index)
        {
            tabControl.TabPages[index].Controls.Add(listViewFiles);
        }

        public int GetCurrentTab
        {
            get
            {
                return Convert.ToInt32(tabControl.SelectedTab.Tag);
            }
        }

        public void SetDefaultPageTag(int index)
        {
            tabControl.TabPages["tabPage1"].Tag = index;
        }

        private void TabControlDoubleClick(object sender, EventArgs e)
        {
            if (NewTabAdded != null)
            {
                NewTabAdded(listViewFiles, EventArgs.Empty);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControlDoubleClick(sender, e);
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabDeleted != null)
            {
                TabDeleted(tabControl.TabPages[pageIndex],EventArgs.Empty);
            }
        }

        private void tabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    Rectangle r = tabControl.GetTabRect(i);
                    if (r.Contains(e.Location))
                    {
                        pageIndex = i;
                    }
                }
            }
        }
    }
}
