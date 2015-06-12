using NUnit.Framework;
using System.Collections.Generic;
using FileBrowser.Commands;
using FileBrowser.Model;
using System.IO;
using System;

namespace TestModel
{
    [TestFixture]
    public class Test
    {
        List<string> files = new List<string>();
        string fileName;
        private ModelProcessCopy CreateModel(ModelProcessCopy m,List<string> files)
        {
            m.Source = files; ;
            m.SourceCurrentPath = "D:\\Temp";
            m.DestPath = "D:\\Temp\\folder";
            return m;
        }

        [Test]
        public void TestDeleteValidFile()
        {
            files.Clear();
            fileName="D:\\Temp\\delete.txt";
            files.Add(fileName);
            File.Create(fileName).Close();
            if (File.Exists(fileName))
            {
                CommandDeleteFiles cmdDel = new CommandDeleteFiles(true, string.Empty, false, files);
                try
                {
                   
                    CommandManager.Execute(cmdDel);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
                finally
                {
                    Assert.IsTrue(!cmdDel.HasError);
                }
            }
            else
            {
                Assert.Fail("File not exist");
            }
        }

        [Test]
        public void TestDeleteNullFile()
        {
            fileName = String.Empty;
            files.Add(fileName);
            try
            {
                CommandDeleteFiles cmdDel = new CommandDeleteFiles(true, string.Empty, false, files);
                CommandManager.Execute(cmdDel);
                Assert.IsTrue(!cmdDel.HasError);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TestDeleteInValidFile()
        {
            string fileName = "D:\\Test\\InvalidFiledel.txt";
            files.Add(fileName);
            if (File.Exists(fileName))
            {
                try
                {
                    CommandDeleteFiles cmdDel = new CommandDeleteFiles(true, string.Empty, false, files);
                    CommandManager.Execute(cmdDel);
                    Assert.IsTrue(!cmdDel.HasError);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
            else
            {
                Assert.True(true, "File not exist");
            }
        }
        [Test]
        public void TestCopyValidFile()
        {
            files.Clear();
            fileName = "D:\\Temp\\copyValid.txt";
            files.Add(fileName);
            ModelProcessCopy model = new ModelProcessCopy();
            model = CreateModel(model,files);
            CreateModel(model, files);
            string newFile = model.DestPath + "\\copyValid.txt";
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            CommandCopyFiles cmdCopy = new CommandCopyFiles(true, string.Empty, false, model, false);
            try
            {
                CommandManager.Execute(cmdCopy);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                Assert.True(!cmdCopy.HasError && File.Exists(newFile));
                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }
            }
        }

        [Test]
        public void TestCopyInValidFile()
        {
            string fileName = "D:\\Test\\testInValid.txt";
            List<string> files = new List<string>();
            files.Add(fileName);
            ModelProcessCopy model = new ModelProcessCopy();
            model = CreateModel(model, files);
            try
            {
                CommandCopyFiles cmdCopy = new CommandCopyFiles(true, string.Empty, false, model, false);
                CommandManager.Execute(cmdCopy);
                Assert.True(!cmdCopy.HasError);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message + " " + ex.StackTrace);
            }
        }

        [Test]
        public void TestCopyNullFile()
        {
            string fileName = String.Empty;
            List<string> files = new List<string>();
            files.Add(fileName);
            ModelProcessCopy model = new ModelProcessCopy();
            model = CreateModel(model, files);
            CommandCopyFiles cmdCopy = new CommandCopyFiles(true, string.Empty, false, model, false);
            try
            {
                CommandManager.Execute(cmdCopy);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                Assert.True(!cmdCopy.HasError);
            }
        }

        [Test]
        public void TestReplaceNullFile()
        {
            files.Clear();
            fileName = string.Empty;
            files.Add(fileName);
            ModelProcessCopy model = new ModelProcessCopy();
            model = CreateModel(model, files);
            CreateModel(model, files);
            CommandCopyFiles cmdCopy = new CommandCopyFiles(true, string.Empty, false, model, false);
            try
            {
                CommandManager.Execute(cmdCopy);
           }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                Assert.True(!cmdCopy.HasError);
            }
        }

        [Test]
        public void TestReplaceInvalidFile()
        {
            files.Clear();
            fileName = "D:\\Temp\\invalidFile.txt";
            files.Add(fileName);
            ModelProcessCopy model = new ModelProcessCopy();
            model = CreateModel(model, files);
            CreateModel(model, files);
            try
            {
                CommandCopyFiles cmdCopy = new CommandCopyFiles(true, string.Empty, false, model, false);
                CommandManager.Execute(cmdCopy);
                Assert.True(!cmdCopy.HasError);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message + " " + ex.StackTrace);
            }
        }

        [Test]
        public void TestReplaceValidlFile()
        {
            files.Clear();
            fileName = "D:\\Temp\\replaceValid.txt";
            files.Add(fileName);
            ModelProcessCopy model = new ModelProcessCopy();
            model = CreateModel(model, files);
            CreateModel(model, files);
            string newFile = model.DestPath + "\\replaceValid.txt";
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            CommandCopyFiles cmdCopy = new CommandCopyFiles(true, string.Empty, false, model, true);
            try
            {
                CommandManager.Execute(cmdCopy);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                 Assert.True(!cmdCopy.HasError && File.Exists(newFile));
                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }
            }
        }
    }
}
