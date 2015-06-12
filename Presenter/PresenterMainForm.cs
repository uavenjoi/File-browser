using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using FileBrowser.Model;
using FileBrowser.Service;
using FileBrowser.Commands;
using FileBrowser.Properties;
using FileBrowser.Controls;
using System.Configuration;

namespace FileBrowser
{
    class PresenterMainForm<TView> where TView : class,IView
    {
        const string eng = "en-GB";
        const string rus = "ru-RU";
        PresenterProgress _presenterProgress;
        readonly PresenterFileViewer<Controls.FileViewer> PresenterSource;
        readonly PresenterFileViewer<Controls.FileViewer> PresenterDest;
        Settings set = Settings.Default;

        public TView View
        {
            get;
            private set;
        }

        public PresenterMainForm(TView v)
        {
            View = v;
            View.ReplacePerformed += new EventHandler<EventArgs>(OnMovePerformed);
            View.NewFolderPerformed += new EventHandler<EventArgs>(OnNewFolderPerformed);
            View.FormClose += new EventHandler<EventArgs>(OnBeforeFormClosing);
            View.ShowHidden += new EventHandler<EventArgs>(OnShowHidden);
            View.BeforeFormClosing += new EventHandler<EventArgs>(OnBeforeFormClosing);
            View.CopyPerformed += new EventHandler<EventArgs>(OnCopyPerformed);
            View.DeletePerformed += new EventHandler<EventArgs>(OnDeletePerformed);
            View.LanguageChange += new EventHandler<EventArgs>(OnLanguageChange);
            CommandManager.OnCommandFinished += new EventHandler(FileViewerRefresh);
            View.RightActivated+=new EventHandler<EventArgs>(OnRightActivated);
            View.LeftActivated+=new EventHandler<EventArgs>(OnLeftActivated);
            View.BackSpacePressed+=new EventHandler<EventArgs>(OnBackSpacePressed);
            PresenterSource = new PresenterFileViewer<Controls.FileViewer>(ViewMainForm.GetInstance.Source);
            PresenterDest = new PresenterFileViewer<Controls.FileViewer>(ViewMainForm.GetInstance.Dest);
            PresenterSource.CurrentPath = set.LeftPath;
            PresenterDest.CurrentPath = set.RightPath;
            PresenterDest.ShowHidden = set.ShowHidden;
            PresenterSource.ShowHidden = set.ShowHidden;
            TextRes.CurrentCulture = set.Lang;
            PresenterSource.SetTabs(Model.Tabs.LoadTabs(Model.Side.Left));
            PresenterDest.SetTabs(Model.Tabs.LoadTabs(Model.Side.Right));
            View.Localize();
        }

        private void OnBackSpacePressed(object sender, EventArgs e)
        {
            if (PresenterSource.IsSelected) PresenterSource.FolderBack();
            else if (PresenterDest.IsSelected) PresenterDest.FolderBack();
        }
        private void OnRightActivated(object sender, EventArgs e)
        {
            PresenterSource.IsSelected = false;
            PresenterDest.IsSelected = true;
        }

        private void OnLeftActivated(object sender, EventArgs e)
        {
            PresenterSource.IsSelected = true;
            PresenterDest.IsSelected = false;
        }

        private void SaveSettigns()
        {
            set.LeftPath = PresenterSource.CurrentPath;
            set.RightPath = PresenterDest.CurrentPath;
            Model.Tabs.SaveTabs(PresenterSource.GetTabs, Model.Side.Left);
            Model.Tabs.SaveTabs(PresenterDest.GetTabs, Model.Side.Right);
            set.Lang = TextRes.CurrentCulture;
            set.Save();
        }

        private void OnBeforeFormClosing(object sender, EventArgs e)
        {
            SaveSettigns();
            CommandManager.Execute(new CommandClose(true));
        }

        public void CreateProgressForm(ModelProcessCopy model, CommandCopyFiles cmd)
        {
            _presenterProgress = new PresenterProgress();
            if (model != null)
             _presenterProgress.AddModel(model);
            _presenterProgress.AddCommand(cmd);
            _presenterProgress.Show();
        }

        //заполнение модели
        private ModelProcessCopy CreateNewModel(ModelProcessCopy m)
        {
            if (PresenterSource.IsSelected)
            {
                m.Source = PresenterSource.GetSelectedItems;
                m.SourceCurrentPath = PresenterSource.CurrentPath;
                m.DestPath = PresenterDest.CurrentPath;
            }
            else if (PresenterDest.IsSelected)
            {
                m.Source = PresenterDest.GetSelectedItems;
                m.SourceCurrentPath = PresenterDest.CurrentPath;
                m.DestPath = PresenterSource.CurrentPath;
            }
            return m;
        }

        private void OnCopyPerformed(object sender, EventArgs e)
        {
            var model = new ModelProcessCopy();
            model= CreateNewModel(model);
            bool condition = model.Source.Count > 0;
            var cmd = new CommandCopyFiles(model.Source.Count > 0, strings.noSelectedFiles,true, model, false);
            if (condition)
            {
                CreateProgressForm(model,cmd);
             }
            CommandManager.Execute(cmd);
            
        }

        private void OnMovePerformed(object sender, EventArgs e)
        {
            var model = new ModelProcessCopy();
            model = CreateNewModel(model);
            bool condition = model.Source.Count > 0;
            var cmd = new CommandCopyFiles(condition, strings.noSelectedFiles,true, model, true);
            if (condition)
            {
                CreateProgressForm(model,cmd);
            }
            CommandManager.Execute(cmd);
        }

        private void OnDeletePerformed(object sender, EventArgs e)
        {
            List<string> filesToDel=new List<string>();
            if (PresenterSource.IsSelected)
                filesToDel = PresenterSource.GetSelectedItems;
            else if (PresenterDest.IsSelected)
                filesToDel = PresenterDest.GetSelectedItems;
            CommandManager.Execute(new CommandDeleteFiles(filesToDel.Count > 0, strings.noSelectedFiles, true, filesToDel));
        }

        private void OnNewFolderPerformed(object sender, EventArgs e)
        {
            string currentPath = String.Empty;
            if (PresenterSource.IsSelected)
                currentPath = PresenterSource.CurrentPath;
            else if (PresenterDest.IsSelected)
                currentPath = PresenterDest.CurrentPath;
            CommandManager.Execute(new CommandNewFolder(!String.IsNullOrEmpty(currentPath), currentPath));
        }

        private void OnShowHidden(object sender, EventArgs e)
        {
                  CommandManager.Execute(new CommandShowHidden(!Settings.Default.ShowHidden,
                                     ViewMainForm.GetInstance.Source, ViewMainForm.GetInstance.Dest));
        }

        private void FileViewerRefresh(object sender, EventArgs e)
        {
            PresenterSource.RefreshView();
            PresenterDest.RefreshView();
        }

        private void OnLanguageChange(object sender, EventArgs e)
        {
            string culture = eng;
            if (ViewMainForm.GetInstance.SelectedLanguage != null)
            {
                if (String.Equals(ViewMainForm.GetInstance.SelectedLanguage, "englishItem",StringComparison.CurrentCultureIgnoreCase))
                {
                    set.EngLang = true;
                    set.RusLang = false;
                }
                else if (String.Equals(ViewMainForm.GetInstance.SelectedLanguage, "russianItem",StringComparison.CurrentCultureIgnoreCase))
                {
                    culture = rus;
                    set.RusLang = true;
                    set.EngLang = false;
                }
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
                TextRes.CurrentCulture = culture;
                set.Save();
                ViewMainForm.GetInstance.Localize();
            }
        }
    }
}
