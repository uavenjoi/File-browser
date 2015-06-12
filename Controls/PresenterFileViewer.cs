using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using FileBrowser.Model;
using FileBrowser.Commands;
using FileBrowser.Service;

namespace FileBrowser.Controls
{
    public class PresenterFileViewer<TView> where TView : class,IFileViewer
    {
        #region variables, properties, constructors
        private ImageList ilIcon;
        private const string prev = "...";
        static string fromPath;
        readonly ModelFileViewer model;
        ModelProcessCopy modelCopy;
        bool isSelected;
        static bool isMove;
        private int currentTab;
        int defaultPageIndex = 0;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (isSelected)
                    view.SetActive();
                else
                {
                    view.SetNotActive();
                    view.ClearSelected();
                }
            }
        }

        public bool ShowHidden
        { get; set; }

        public string CurrentPath
        {
            get
            {
                if (model.ListTabs.ContainsKey(defaultPageIndex) && !String.IsNullOrEmpty(model.ListTabs[defaultPageIndex]))
                    return model.ListTabs[defaultPageIndex];
                else return model.CurrentPath;
            }
            set
            {
                string dir = value;
                if (!Directory.Exists(dir))
                    dir = "C:\\";
                model.CurrentPath = dir;
                view.SetCurrentPathHeader(dir);
                string tabName = string.Empty;

                if (!String.IsNullOrEmpty(Path.GetFileName(dir)))
                {
                    tabName = GetShortNameTab(dir);
                }
                else
                    tabName = dir;
                view.SetTabHeaders(tabName);
            }
        }

        public TView view
        {
            get;
            private set;
        }

        public PresenterFileViewer(TView v)
        {
            model = new ModelFileViewer();
            IContainer components = null;
            model.ListViewFiles = new ListView();
            view = v;
            view.Loaded += OnLoaded;
            view.ItemActivated += OnItemActivated;
            view.DragDropped += OnDragDropped;
            view.MouseViewerLeaved += OnMouseLeaved;
            view.FileRenamed+=OnFileRename;
            view.FileCopy += OnFileCopy;
            view.FileDeleted+=OnFileDeleted;
            view.FilePaste += OnFilePaste;
            view.FileCut+=OnFileCut;
            view.ContextMenuOpened+=OnContextMenuOpened;
            view.TabActivated+=OnTabActivated;
            view.NewTabAdded+=OnNewTabAdded;
            view.TabDeleted += OnDeleteTab;
            components = new Container();
            ilIcon = new ImageList(components);
            ilIcon.ColorDepth = ColorDepth.Depth8Bit;
            ilIcon.ImageSize = new Size(16, 16);
            ilIcon.TransparentColor = Color.Transparent;
            TextRes.CurrentCulture = Properties.Settings.Default.Lang;
            view.Localize();
        }
        #endregion

        #region Tabs
        public void SetTabs(Dictionary<int, string> tabs)
        {
            if (tabs != null)
            {
                model.ListTabs = tabs;
                foreach (KeyValuePair<int, string> kvp in tabs)
                {
                    view.SetNewTab(kvp.Key, GetShortNameTab(kvp.Value));
                }
                defaultPageIndex = GetTabPageKey(tabs.Keys.ToList<int>());
                model.ListTabs.Add(defaultPageIndex, CurrentPath);
                view.SetDefaultPageTag(defaultPageIndex);
            }
        }

        private string GetShortNameTab(string path)
        {
            if (path.Length > 3)
                path = Path.GetFileName(path);
            if (path.Length > 20)
            {
                path = Path.GetFileName(path).Remove(20, Path.GetFileName(path).Length - 20);
            }
            return path;
        }

        private int GetTabPageKey(List<int> keys)
        {
            int key = 0;
            while(keys.Contains(key))
            {
                key++;
            }
            return key;
        }

        public Dictionary<int, string> GetTabs
        {
            get 
            {
                model.ListTabs.Remove(defaultPageIndex);
                return model.ListTabs; 
            }
        }

        private void OnDeleteTab(object sender, EventArgs e)
        {
            if (model.ListTabs.Count == 1)
                return;
            TabPage page = (sender as TabPage);
            view.DeleteTab(page);
            model.ListTabs.Remove(Convert.ToInt32(page.Tag));
        }

        private void OnNewTabAdded(object sender, EventArgs e)
        {
            model.ListViewFiles = ((ListView)sender);
            BuildList(CurrentPath, model.ListViewFiles, ShowHidden);
            int index = GetTabPageKey(model.ListTabs.Keys.ToList<int>());
            model.ListTabs.Add(index, CurrentPath);
            view.SetNewTab(index, GetShortNameTab(CurrentPath));
            view.TabChange(model.ListViewFiles);
        }

        private void OnTabActivated(object sender, EventArgs e)
        {
            currentTab = view.GetCurrentTab;
            CurrentPath = model.ListTabs[currentTab];
            model.ListViewFiles = ((ListView)sender);
            BuildList(model.ListTabs[currentTab], model.ListViewFiles, ShowHidden);
            view.TabChange(model.ListViewFiles);
            view.SetCurrentPathHeader(CurrentPath);
        }
        #endregion

        #region FileViewer
        public List<string> GetSelectedItems
        {
            get
            {
                List<string> selectedItems = new List<string>();
                var items = view.GetSelectedItems();
                if (items != null)
                {

                    foreach (ListViewItem lvi in items)
                    {
                        if (model.CurrentPath.Length - 1 < 3)
                            selectedItems.Add(CurrentPath +
                                               lvi.Text +
                                               lvi.SubItems[1].Text);
                        else
                            selectedItems.Add(Path.Combine(CurrentPath, lvi.Text + lvi.SubItems[1].Text));
                    }
                }
                return selectedItems;
            }
        }

        //переход на один уровень вверх
        public void FolderBack()
        {
            if (CurrentPath.Length > 3)
            {
                string parent = Directory.GetParent(CurrentPath).FullName;
                if (Directory.Exists(parent))
                    BuildList(parent, model.ListViewFiles, ShowHidden);
                CurrentPath = parent;
            }
        }

        public void RefreshView()
        {
            BuildList(CurrentPath, model.ListViewFiles, ShowHidden);
        }

        private void OpenFile(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MyLog.WriteError(ex.Message, ex.Source);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //подготовка локального пути
        private static string PrepareLocalPath(string path, string changed)
        {
            string changePath;
            var dirInfo = new DirectoryInfo(path);
            if (String.Equals(changed, prev, StringComparison.CurrentCultureIgnoreCase))
            {
                string cPath;
                cPath = dirInfo.Parent.FullName;
                changePath = cPath;
            }
            else
            {
                changePath = Path.Combine(dirInfo.FullName, changed);
            }
            return changePath;
        }

        private void OnItemActivated(object sender, EventArgs e)
        {
            try
            {
                ListView list = ((ListView)sender);
                if (list.SelectedItems.Count > 0 && list.FocusedItem != null)
                {
                    string focusedItem = list.FocusedItem.Text;
                    currentTab = view.GetCurrentTab;
                    string path = Path.Combine(model.ListTabs[currentTab], list.FocusedItem.Text + list.FocusedItem.SubItems[1].Text);

                    if (File.Exists(path))//если файл то открываем
                        OpenFile(path);

                    else if (Directory.Exists(path))//если папка то выводим содержимое
                    {
                        if (BuildList(PrepareLocalPath(model.CurrentPath, focusedItem), list, ShowHidden))
                        {
                            model.ListTabs[currentTab] = PrepareLocalPath(model.CurrentPath, focusedItem);
                            CurrentPath = model.ListTabs[currentTab];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.WriteError(ex.Message, ex.StackTrace);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                BuildList(CurrentPath, model.ListViewFiles, ShowHidden);
            }
            fromPath = String.Copy(CurrentPath);
        }

        private void OnMouseLeaved(object sender, EventArgs e)
        {
            fromPath = string.Copy(CurrentPath);
        }

        private void OnDragDropped(object sender, EventArgs e)
        {
            var items = view.GetSelectedItems();
            List<string> selectedItems = new List<string>();
            ListViewItem itemDest = view.GetItemDest;
            foreach (ListViewItem item in items)
            {
                if (String.Equals(item.Text, prev, StringComparison.CurrentCultureIgnoreCase))
                    selectedItems.Add(fromPath);
                else if (fromPath.Length <= 3)
                    selectedItems.Add(Path.Combine(fromPath, item.Text + item.SubItems[1].Text));
                else
                    selectedItems.Add(Path.Combine(fromPath, item.Text + item.SubItems[1].Text));
            }
            modelCopy = new ModelProcessCopy();
            modelCopy.Source = selectedItems;
            if (itemDest != null && !File.Exists(Path.Combine(CurrentPath, itemDest.Text + itemDest.SubItems[1].Text)))
            {
                modelCopy.DestPath = Path.Combine(CurrentPath, itemDest.Text);
            }
            else
            {
                modelCopy.DestPath = CurrentPath;
            }
            modelCopy.SourceCurrentPath = Directory.GetParent(modelCopy.Source[0]).FullName;
            var cmd = new CommandCopyFiles(modelCopy.Source.Count > 0 && !String.IsNullOrEmpty(modelCopy.DestPath),
                                            strings.noSelectedFiles,false, modelCopy, false);
            var presenterProgress = new PresenterProgress();
            if (modelCopy != null)
                presenterProgress.AddModel(modelCopy);
            presenterProgress.AddCommand(cmd);
            presenterProgress.Show();
     
            CommandManager.Execute(cmd);
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            BuildLocalDrives();
            BuildList(model.CurrentPath, model.ListViewFiles, ShowHidden);
            view.SetCurrentPathHeader(model.CurrentPath);
        }

        //отображение локальных дисков
        private void BuildLocalDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            List<Button> btnDrives = new List<Button>();
            int sPoint = 6;
            foreach (DriveInfo ld in drives)
            {
                Button btnDrive = new Button();
                btnDrive.Location = new Point(sPoint, 2);
                btnDrive.Name = ld.Name;
                btnDrive.Size = new Size(30, 20);
                btnDrive.TabStop = false;
                btnDrive.Text = ld.Name;
                btnDrive.UseVisualStyleBackColor = true;
                btnDrive.Font = new Font(btnDrive.Font.FontFamily, 7);
                btnDrive.Click += new EventHandler(DriveChange);
                btnDrive.Tag = "1";
                btnDrives.Add(btnDrive);
                sPoint = sPoint + 37;
            }
            view.SetLocalDrives(btnDrives.ToArray());
        }

        private void DriveChange(object sender, EventArgs e)
        {
            try
            {
                string disk = ((Button)sender).Name;
                if (Directory.Exists(disk))
                {
                    CurrentPath = disk;
                    model.ListTabs[currentTab] = disk;
                    view.SetCurrentPathHeader(model.CurrentPath);
                }
                else
                    view.MessageShow(strings.diskNotFound + disk, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                BuildList(model.CurrentPath, model.ListViewFiles, ShowHidden);
                view.SetCurrentPathHeader(model.CurrentPath);
            }
            catch (Exception ex)
            {
                MyLog.WriteError(ex.Message, ex.Source);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ShowFileSize(FileInfo dirFile)
        {
            long fileSize = dirFile.Length;
            string size = String.Empty;

            if (fileSize < 1024)
                size = String.Format("{0} bytes", fileSize);
            else if (fileSize < 1024 * 1024)
                size = String.Format("{0:0.00} Kb", (double)fileSize / 1024);
            else if (fileSize < 1024 * 1024 * 1024)
                size = String.Format("{0:0.00} Mb", (double)fileSize / (1048576));
            else
                size = String.Format("{0:0.00} Gb", (double)fileSize / (1073741824));
            return size;
        }

        //построение списка папок и файлов
        private bool BuildList(string path, ListView lvV, bool showHidden)
        {
            var listExt = new Dictionary<string, int>();//расширение файла и индекс иконки
            int nIco = 0;
            showHidden = Properties.Settings.Default.ShowHidden;
            var list = new List<ListViewItem>();
            view.ClearFileList();
            ilIcon = new ImageList();
            string ext;
            try
            {
                var dirInfo = new DirectoryInfo(path);
                var dirFromArr = dirInfo.GetDirectories();
                var dirFiles = dirInfo.GetFiles();
                if (!String.Equals(path, dirInfo.Root.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    list.Add(new ListViewItem(new[] { "...", "", "<Folder>", "" }));

                int countDir = 0;
                //формируем список папок
                foreach (DirectoryInfo dirName in dirFromArr)
                {
                    if (!showHidden)
                    {
                        if ((dirName.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            continue;
                        }
                    }
                    Icon iicn = SystemIcon.GetFolderIcon(path + dirName.Name, SystemIcon.IconSize.Small, false);
                    ilIcon.Images.Add(iicn);

                    ListViewItem lvDir = new ListViewItem(new string[]{dirName.Name, "","<Folder>",
                                                    dirName.LastAccessTime.ToShortDateString()+
                                                    " "+ 
                                                    dirName.LastAccessTime.ToShortTimeString()});
                    if (countDir == 0)
                    {
                        lvDir.Selected = true;
                        lvDir.Focused = true;
                    }
                    lvDir.ImageIndex = nIco++;
                    list.Add(lvDir);
                    countDir++;
                }
                //формируем список файлов
                foreach (FileInfo dirFile in dirFiles)
                {
                    if (!showHidden)
                    {
                        if ((dirFile.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            continue;
                        }
                    }
                    ext = Path.GetExtension(dirFile.Name);
                    ListViewItem lvFile = new ListViewItem(new string[]{Path.GetFileNameWithoutExtension(Path.Combine(path, dirFile.Name)),
                                                                dirFile.Extension,
                                                                ShowFileSize(dirFile),
                                                                dirFile.LastAccessTime.ToShortDateString()+" "+
                                                                dirFile.LastAccessTime.ToShortTimeString()});
                    if (!listExt.ContainsKey(ext))
                    {
                        listExt.Add(ext, nIco++);
                        ilIcon.Images.Add(SystemIcon.GetFileIcon(path + dirFile.Name, SystemIcon.IconSize.Small, false));
                    }
                    lvFile.ImageIndex = listExt[ext];
                    list.Add(lvFile);
                }
                ListViewItem[] arr = list.ToArray();
                view.SetFilesList(arr, ilIcon);
                return true;
            }
            catch (IOException ex)
            {
                MyLog.WriteError(ex.Message, ex.Source);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                BuildList(CurrentPath, model.ListViewFiles, ShowHidden);
                return false;
            }
            finally
            {
                view.ClearSelected();
            }
        }
        #endregion

        #region ContextMenu
        private void OnFileDeleted(object sender, EventArgs e)
        {
            List<string> filesToDel = new List<string>();
            try
            {
                ListView list = ((ListView)sender);
                if (list.SelectedItems.Count > 0 && list.FocusedItem != null)
                {
                    filesToDel = GetSelectedItems;
                    CommandManager.Execute(new CommandDeleteFiles(filesToDel.Count > 0, strings.noSelectedFiles, true, filesToDel));
                }
                else
                {
                    view.MessageShow(strings.noSelectedFiles, strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                BuildList(CurrentPath, model.ListViewFiles, ShowHidden);
            }
            catch (Exception ex)
            {
                MyLog.logger.Error(ex.StackTrace);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void OnFilePaste(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsData(DataFormats.FileDrop))
                {
                    var paths = new StringCollection();
                    paths = Clipboard.GetFileDropList();
                    modelCopy = new ModelProcessCopy();
                    modelCopy.Source = paths.Cast<string>().ToList();
                    modelCopy.DestPath = CurrentPath;
                    modelCopy.SourceCurrentPath = Directory.GetParent(modelCopy.Source[0]).FullName;
                    var cmd = new CommandCopyFiles(modelCopy.Source.Count > 0 && !String.IsNullOrEmpty(modelCopy.DestPath),
                                           strings.noSelectedFiles, true, modelCopy, isMove);
                    var presenterProgress = new PresenterProgress();
                    if (modelCopy != null)
                        presenterProgress.AddModel(modelCopy);
                    presenterProgress.AddCommand(cmd);
                    presenterProgress.Show();
                    CommandManager.Execute(cmd);
                }
                if (isMove)
                    Clipboard.Clear();
            }
            catch (Exception ex)
            {
                MyLog.logger.Error(ex.StackTrace);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFileCut(object sender, EventArgs e)
        {
            isMove = true;
            try
            {
                ListView list = ((ListView)sender);
                if (list.SelectedItems.Count > 0 && list.FocusedItem != null)
                {
                    StringCollection paths = new StringCollection();
                    paths.Add(Path.Combine(CurrentPath, list.FocusedItem.Text + list.FocusedItem.SubItems[1].Text));
                    Clipboard.SetFileDropList(paths);
                }
            }
            catch (Exception ex)
            {
                MyLog.logger.Error(ex.StackTrace);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFileCopy(object sender, EventArgs e)
        {
            isMove = false;
            try
            {
                ListView list = ((ListView)sender);
                if (list.SelectedItems.Count > 0 && list.FocusedItem != null)
                {
                    StringCollection paths = new StringCollection();
                    paths.Add(Path.Combine(CurrentPath, list.FocusedItem.Text + list.FocusedItem.SubItems[1].Text));
                    Clipboard.SetFileDropList(paths);
                }
            }
            catch (Exception ex)
            {
                MyLog.logger.Error(ex.StackTrace);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFileRename(object sender, EventArgs e)
        {
            try
            {
                ListView list = ((ListView)sender);
                if (list.SelectedItems.Count > 0 && list.FocusedItem != null)
                {
                    string path = Path.Combine(CurrentPath, list.FocusedItem.Text + list.FocusedItem.SubItems[1].Text);
                    string newName = ViewMainForm.GetInstance.ShowInputBox(strings.NewName, strings.Rename, list.FocusedItem.Text + list.FocusedItem.SubItems[1].Text);
                    if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && !String.IsNullOrEmpty(newName))
                    {
                        if (File.Exists(path))
                        {
                            FileInfo fileInfo = new FileInfo(path);
                            fileInfo.MoveTo(Path.Combine(Directory.GetParent(path).FullName, newName));
                        }
                        else if (Directory.Exists(path))
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(path);
                            dirInfo.MoveTo(Path.Combine(Directory.GetParent(path).FullName, newName));
                        }
                        BuildList(CurrentPath, model.ListViewFiles, ShowHidden);
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.logger.Error(ex.StackTrace);
                view.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //если в буфере обмена есть файлы, то меню Вставить отображается
        private void OnContextMenuOpened(object sender, EventArgs e)
        {
            bool isEnabled;
            if (!Clipboard.ContainsData(DataFormats.FileDrop))
            {
                isEnabled = false;
            }
            else
            {
                isEnabled = true;
            }
            view.ShowPastMenu(isEnabled);
        }
        #endregion
    }
}
