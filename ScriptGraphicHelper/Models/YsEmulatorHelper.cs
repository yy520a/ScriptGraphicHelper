using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using static System.Environment;

namespace ScriptGraphicHelper.Models
{
    class YsEmulatorHelper : EmulatorHelper
    {
        public override string Path { get; set; } = string.Empty;
        public override string Name { get; set; } = string.Empty;
        public string BmpPath { get; set; }
        public YsEmulatorHelper()//初始化 , 获取模拟器路径
        {
            Name = "夜神模拟器";
            string path = GetInkTargetPath(@"C:\Users\" + UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu", "夜神模拟器.lnk");
            if (path == string.Empty)
            {
                path = GetInkTargetPath(@"C:\ProgramData\Microsoft\Windows\Start Menu", "夜神模拟器.lnk");
                if (path == string.Empty)
                {
                    path = GetInkTargetPath(CurrentDirectory, "夜神模拟器.lnk");
                }
            }
            if (path != string.Empty)
            {
                int index = path.LastIndexOf("\\");
                Path = path.Substring(0, index + 1).Trim('"');
                BmpPath = BmpPathGet();
            }
        }
        public override void Dispose() { }
        public string GetInkTargetPath(string path, string fileName)
        {
            string result = string.Empty;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
            foreach (var item in fileSystemInfos)
            {
                try
                {
                    string fullName = item.FullName;
                    if (Directory.Exists(fullName))
                    {
                        result = GetInkTargetPath(fullName, fileName);
                        if (result != string.Empty)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        if (item.Name == fileName)
                        {
                            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();
                            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(fullName);
                            return shortcut.TargetPath;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return result;
        }
        public string[] List(int index)
        {
            string[] resultArray = PipeCmd("list").Trim("\n".ToCharArray()).Split("\n".ToCharArray());
            for (int i = 0; i < resultArray.Length; i++)
            {
                string[] LineArray = resultArray[i].Split(',');
                if (LineArray.Length > 1)
                {
                    if (LineArray[0] == index.ToString())
                    {
                        return LineArray;
                    }
                }
            }
            return new string[] { };
        }
        public override bool IsStart(int index)
        {
            string[] resultArray = PipeCmd("list").Trim("\n".ToCharArray()).Split("\n".ToCharArray());
            for (int i = 0; i < resultArray.Length; i++)
            {
                string[] LineArray = resultArray[i].Split(',');
                if (LineArray.Length > 1)
                {
                    if (LineArray[0] == index.ToString())
                    {
                        return LineArray[6] != "-1";
                    }
                }
            }
            return false;
        }
        public override List<KeyValuePair<int, string>> ListAll()
        {
            string[] resultArray = PipeCmd("list").Trim("\n".ToCharArray()).Split("\n".ToCharArray());
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < resultArray.Length; i++)
            {
                string[] LineArray = resultArray[i].Split(',');
                result.Add(new KeyValuePair<int, string>(key: int.Parse(LineArray[0].Trim()), value: LineArray[2]));
            }
            return result;
        }
        public override Bitmap ScreenShot(int Index)
        {
            if (!IsStart(Index))
            {
                MessageBox.Show("模拟器未启动 ! ");
                return new Bitmap(1, 1);
            }
            string BmpName = "Screen_" + DateTime.Now.ToString("yy-MM-dd-HH-mm-ss") + ".png";
            Screencap(Index, "/mnt/sdcard/Pictures", BmpName);
            try
            {
                FileStream fileStream = new FileStream(BmpPath + "\\" + BmpName, FileMode.Open, FileAccess.Read);
                Bitmap bmp = (Bitmap)Image.FromStream(fileStream);
                fileStream.Close();
                fileStream.Dispose();
                return bmp;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new Bitmap(1, 1);
            }
        }
        public void Screencap(int index, string savePath, string saveName)//截图
        {
            PipeCmd("adb -index:" + index.ToString() + " -command:\"shell /system/bin/screencap -p " + savePath.TrimEnd('/') + "/Screenshots/" + saveName+"\"");
        }

        public string BmpPathGet()
        {
            try
            {
                return @"C:\Users\" +UserName+ @"\Nox_share\ImageShare\Screenshots\";
            }
            catch
            {
                return "";
            }
        }
        public string PipeCmd(string theCommand)
        {
            string path = Path + "NoxConsole.exe";
            ProcessStartInfo start = new ProcessStartInfo(path)
            {
                Arguments = theCommand,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };
            Process pipe = Process.Start(start);
            StreamReader readStream = pipe.StandardOutput;
            string OutputStr = readStream.ReadToEnd();
            pipe.WaitForExit(10000);
            pipe.Close();
            readStream.Close();
            return OutputStr;
        }
    }
}
