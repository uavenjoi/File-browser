using System;
using System.IO;
using System.Windows.Forms;

namespace FileBrowser.Commands
{
    class CommandNewFolder:Command
    {
        private string sourcePath;
      
        public CommandNewFolder(bool cond, string srcPath)
        {
            sourcePath = srcPath;
            Condition = cond;
        }

        public override void Execute()
        {
             CreateFolder();
        }

        //создание новой директории
        internal void CreateFolder()
        {
            //создаем форму для ввода имени новой папки
            string value = ViewMainForm.GetInstance.ShowInputBox(strings.enterFolderName, strings.folderName, strings.FolderDefaultName);
            try
            {
                if (!String.IsNullOrEmpty(value.Trim())) //если пустое значение просто ничего не делаем
                    if (value.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                    {
                        if (!Directory.Exists(Path.Combine(sourcePath, value)))
                        {
                            Directory.CreateDirectory(Path.Combine(sourcePath, value));
                        }
                        else
                            ViewMainForm.GetInstance.MessageShow(strings.folderExists, strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        ViewMainForm.GetInstance.MessageShow(strings.folderNameInvalid, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                MyLog.WriteError(ex.Message, ex.StackTrace);
                ViewMainForm.GetInstance.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
