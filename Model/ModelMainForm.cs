using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileBrowser.Model
{
    public enum Side
    {
        Left, 
        Right 
    }

    [Serializable()]
    public class Tabs
    {
        public static void SaveTabs(Dictionary<int, string> tabsDic, Side side)
        {
            string tabs = string.Empty;

            if (tabsDic == null) return;
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, tabsDic);
                    ms.Position = 0;
                    byte[] buffer = new byte[(int)ms.Length];
                    ms.Read(buffer, 0, buffer.Length);
                    tabs = Convert.ToBase64String(buffer);
                }
            }
            switch (side)
            {
                case Side.Right: Properties.Settings.Default.RightTabs = tabs;
                    break;
                case Side.Left: Properties.Settings.Default.LeftTabs = tabs;
                    break;
            }
        }

        public static Dictionary<int, string> LoadTabs(Side side)
        {
            string tabs = string.Empty;
            switch (side)
            {
                case Side.Right: tabs = Properties.Settings.Default.RightTabs;
                    break;
                case Side.Left: tabs = Properties.Settings.Default.LeftTabs;
                    break;
            }

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(tabs)))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (Dictionary<int, string>)bf.Deserialize(ms);
                }
                catch (Exception ex)
                {
                    MyLog.logger.Error(ex.StackTrace);
                    return null;
                }
            }
        }
    }
}
