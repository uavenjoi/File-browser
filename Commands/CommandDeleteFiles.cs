using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileBrowser.Commands
{
  public  class CommandDeleteFiles:CommandAsync
    {
      private List<string> filesToDel;
      private bool showMessages;

      public CommandDeleteFiles(bool cond, string msg, bool showMsg, List<string> src)
      {
            filesToDel = src;
            Condition = cond;
            errorMessage = msg;
            showMessages = showMsg;
      }

      public override void Execute()
      {
          var validFiles = true;
          string listDetetingFiles = string.Empty;
          var sb = new StringBuilder();
          for (int i = 0; i < filesToDel.Count; i++)//составляем список чтобы отобразить в диалоговом окне
          {
              if (!String.Equals(Path.GetFileName(filesToDel[i]), "...", StringComparison.CurrentCultureIgnoreCase))
                  sb.Append(Path.GetFileName(filesToDel[i]) + "\n");
              else
              {
                  if (showMessages)
                      ViewMainForm.GetInstance.MessageShow(strings.notAllowDelete, strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  validFiles = false;
                  break;
              }
              if (i > 10)
              {
                  sb.Append("...");
                  break;
              }
          }
          if (showMessages && validFiles && ViewMainForm.GetInstance.MessageShow(strings.deleteConfirm, strings.question, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                          filesToDel.Count.ToString(), sb.ToString(), Directory.GetParent(filesToDel[0]).FullName) == DialogResult.Yes)
          {
              DeleteFiles(filesToDel);
          }
          if (String.IsNullOrEmpty(errorMessage))
          {
              DeleteFiles(filesToDel);
          }
      }

      //удаление файлов после перемещения, или после отмены копировавния
      public void DeleteFiles(List<string> dirs)
      {
          foreach (string filedir in dirs)
          {
              try
              {
                  if (File.Exists(filedir))
                  {
                      File.Delete(filedir);
                  }
                  else if (Directory.Exists(filedir))
                  {
                      Directory.Delete(filedir, true);
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
}
