using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using FileBrowser.Model;

namespace FileBrowser.Commands
{
    /// <summary>
    /// класс - команда "Копирование"
    /// </summary>
    public class CommandCopyFiles : CommandAsync
    {
        ModelProcessCopy model;
        const int maxFilePathLength = 260;
        readonly byte[] buffer = new byte[1024 * 1024]; //1 Mb buffer
        Dictionary<string, string> source;
        private readonly bool _isMove;
        private bool showMessages;

        /// <summary>
        /// копирование файлов 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="res"></param>
        /// <param name="showMsg"></param>
        /// <param name="mod"></param>
        /// <param name="isMove">delete after copy</param>
        public CommandCopyFiles(bool cond, string res, bool showMsg, ModelProcessCopy mod, bool isMove)
        {
            showMessages = showMsg;
            Condition = cond;
            errorMessage = res;
            _isMove = isMove;
            model = mod;
            model.TotalProgress = 0;
            source = new Dictionary<string, string>();
            if (!String.Equals(model.SourceCurrentPath, model.DestPath, StringComparison.CurrentCultureIgnoreCase))
            {
                var fileList = GetAllFiles(model.Source);
                if (fileList != null)
                {
                    foreach (string file in fileList)
                    {
                        source.Add(file, Path.Combine(model.DestPath, file.Remove(0, model.SourceCurrentPath.Length + 1)));
                    }
                }
            }
            else
                ViewMainForm.GetInstance.MessageShow(strings.copyOnItself, strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //получение списка всех файлов для копирования
        private List<string> GetAllFiles(List<string> filesList)
        {
            List<string> allFiles = new List<string>();
            try
            {
                foreach (string file in filesList)
                {
                    if (File.Exists(file))
                    {
                        allFiles.Add(file);
                    }
                    else if (Directory.Exists(file))
                    {
                        allFiles.Add(file);
                        allFiles.AddRange(Directory.GetFiles(file, "*", SearchOption.AllDirectories));
                    }
                }
                //узнаем суммарный размер файлов
                foreach (string file in allFiles)
                {
                    model.MaxValTotalLong += GetFileSize(file);
                }
                return allFiles;
            }
            catch (UnauthorizedAccessException ex)
            {
                MyLog.WriteError(ex.Message, ex.StackTrace);
                ViewMainForm.GetInstance.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        long GetFileSize(string path)
        {
            if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                return fileInfo.Length;
            }
            return 0;
        }

        public override void Execute()
        {
           CopyFiles(_isMove);
        }

        //выполнение работы в отдельном потоке
        public void CopyFiles(bool isMove)
        {
            try
            {
                foreach (var kvp in source)
                {
                    if (File.Exists(kvp.Key) || Directory.Exists(kvp.Key))
                    {
                        string fileName = Path.GetFileName(kvp.Key);
                        if (Directory.Exists(kvp.Key))//если это папка то просто создаем ее и пропускаем шаг
                        {
                            Directory.CreateDirectory(kvp.Value);
                            continue;
                        }
                        if (!Directory.Exists(Path.GetDirectoryName(kvp.Value)))//создаем папку для этого файла
                            Directory.CreateDirectory(Path.GetDirectoryName(kvp.Value));
                        model.CurrFile = kvp.Key; //устанавливаем следующий файл
                        model.FileDest = kvp.Value;
                        model.CurrentFileSize = GetFileSize(model.CurrFile);
                        //копирование файла
                        using (var reader = new FileStream(kvp.Key, FileMode.Open, FileAccess.Read))
                        {
                            if ((kvp.Value.Length) > maxFilePathLength)//если невозможно сделать имя меньше 260 то выход
                            {
                                ViewMainForm.GetInstance.MessageShow(strings.longFileNameMsgShort, strings.Info,
                                                         MessageBoxButtons.OK, MessageBoxIcon.Information, kvp.Value, kvp.Value.Length.ToString());
                                break;
                            }
                            if (File.Exists(kvp.Value))//если файл существует выводим предуперждение
                            {
                                DialogResult res = ViewMainForm.GetInstance.MessageShow(strings.fileExists, strings.question,
                                                   MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, kvp.Value);
                                if (res == DialogResult.No)
                                {
                                    model.Source.Remove(kvp.Key);//т.к. файл не будет скопирован, удаляем эего из модели, 
                                    continue;                    //чтобы после цикла он не удалился
                                }
                                else if (res == DialogResult.Cancel)
                                {
                                    model.Source.Remove(kvp.Key);
                                    break; //braek foreach
                                }
                            }
                            using (var writer = new FileStream(kvp.Value, FileMode.Create, FileAccess.Write))
                            {
                                model.Progress = 0;
                                int currentBlockSize = 0;
                                while ((currentBlockSize = reader.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    if (IsPaused)
                                    {
                                        busy.WaitOne();
                                    }

                                    if (IsCanceled)
                                        break;
                                    model.Progress += currentBlockSize;
                                    writer.Write(buffer, 0, currentBlockSize);
                                }
                            }
                            if (IsCanceled)
                            {
                                File.Delete(kvp.Value); //удаляем недозаписанный файл
                                break; //выходим из foreach
                            }
                        }
                    }
                }
                if (!IsCanceled && isMove)//если вызвано перемещение то удаляем файлы
                {
                    var del = new CommandDeleteFiles(true, String.Empty, false, model.Source);
                    del.Execute();
                }
            }
            catch (Exception ex)
            {
                MyLog.WriteError(ex.Message, ex.StackTrace);
                ViewMainForm.GetInstance.MessageShow(ex.Message, strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
