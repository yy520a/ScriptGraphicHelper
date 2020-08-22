using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Environment;
using System;
using System.Drawing;
using System.Windows;
using System.Threading.Tasks;

namespace ScriptGraphicHelper.Models
{
    public class LdEmulatorHelper : EmulatorHelper
    {
        public override string Path { get; set; } = string.Empty;
        public override string Name { get; set; } = string.Empty;
        public string BmpPath { get; set; }
        public LdEmulatorHelper(int version)//初始化 , 获取雷电模拟器路径
        {
            try
            {
                if (version == 0)
                {
                    Name = "雷电模拟器3.0";
                    RegistryKey Hkml = Registry.CurrentUser;
                    RegistryKey Aimdir = Hkml.OpenSubKey("Software\\ChangZhi2\\dnplayer", true);
                    Path = Aimdir.GetValue("InstallDir").ToString();
                    if (Path == string.Empty)
                    {
                        string path = GetInkTargetPath(CurrentDirectory, "雷电模拟器3.0.lnk");
                        if (path == string.Empty)
                        {
                            int index = path.LastIndexOf("\\");
                            Path = path.Substring(0, index + 1).Trim('"');
                        }
                    }
                }
                else if (version == 1)
                {
                    Name = "雷电模拟器4.0";
                    RegistryKey Hkml = Registry.CurrentUser;
                    RegistryKey Aimdir = Hkml.OpenSubKey("Software\\leidian\\ldplayer", true);
                    Path = Aimdir.GetValue("InstallDir").ToString();
                    if (Path == string.Empty)
                    {
                        string path = GetInkTargetPath(CurrentDirectory, "雷电模拟器4.0.lnk");
                        if (path != string.Empty)
                        {
                            int index = path.LastIndexOf("\\");
                            Path = path.Substring(0, index + 1).Trim('"');
                        }
                    }
                }
            }
            catch
            {
                Path = string.Empty;
            }
            if (Path != string.Empty)
            {
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
        public string PipeCmd(string theCommand, bool select = false)
        {
            string ThePath = Path + "dnconsole.exe";
            if (select)
                ThePath = Path + "ld.exe";
            ProcessStartInfo start = new ProcessStartInfo(ThePath)
            {
                Arguments = theCommand,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                Verb = "runas"
            };
            Process pipe = Process.Start(start);
            StreamReader readStream = pipe.StandardOutput;
            string OutputStr = readStream.ReadToEnd();
            pipe.WaitForExit(10000);
            pipe.Close();
            readStream.Close();
            return OutputStr;
        }
        public string[] List(string ldName)//获取模拟器信息
                                           //返回数组 , 顺序为:序号，标题，顶层窗口句柄，绑定窗口句柄，是否进入android，进程PID，VBox进程PID
        {
            string[] resultArray = PipeCmd("list2").Trim("\n".ToCharArray()).Split("\n".ToCharArray());
            for (int i = 0; i < resultArray.Length; i++)
            {
                string[] LineArray = resultArray[i].Split(',');
                if (LineArray.Length > 1)
                {
                    if (LineArray[1] == ldName)
                    {
                        return LineArray;
                    }
                }
            }
            return new string[] { };
        }
        public string[] List(int ldIndex)
        {
            string[] resultArray = PipeCmd("list2").Trim("\n".ToCharArray()).Split("\n".ToCharArray());
            for (int i = 0; i < resultArray.Length; i++)
            {
                string[] LineArray = resultArray[i].Split(',');
                if (LineArray.Length > 1)
                {
                    if (LineArray[0] == ldIndex.ToString())
                    {
                        return LineArray;
                    }
                }
            }
            return new string[] { };
        }
        public override bool IsStart(int ldIndex)
        {
            string[] resultArray = PipeCmd("list2").Trim("\n".ToCharArray()).Split("\n".ToCharArray());
            for (int i = 0; i < resultArray.Length; i++)
            {
                string[] LineArray = resultArray[i].Split(',');
                if (LineArray.Length > 1)
                {
                    if (LineArray[0] == ldIndex.ToString())
                    {
                        return LineArray[4] == "1";
                    }
                }
            }
            return false;
        }
        public override List<KeyValuePair<int, string>> ListAll()
        {
            string[] resultArray = PipeCmd("list2").Trim("\n".ToCharArray()).Split("\n".ToCharArray());
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < resultArray.Length; i++)
            {
                string[] LineArray = resultArray[i].Split(',');
                result.Add(new KeyValuePair<int, string>(key: int.Parse(LineArray[0].Trim()), value: LineArray[1]));
            }
            return result;
        }
        public override async Task<Bitmap> ScreenShot(int Index)
        {
            var task = Task.Run(() =>
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
            });
            return await task;
        }
        public void Screencap(int ldIndex, string savePath, string saveName)//截图
        {
            PipeCmd("-s " + ldIndex.ToString() + " /system/bin/screencap -p " + savePath.TrimEnd('/') + "/" + saveName, true);
        }
        public string BmpPathGet()
        {
            try
            {
                StreamReader streamReader = new StreamReader(Path + "\\vms\\config\\leidian0.config", false);
                string ret = streamReader.ReadToEnd();
                streamReader.Close();
                JObject jsonObj = JObject.Parse(ret);
                return jsonObj["statusSettings.sharedPictures"].ToString();
            }
            catch
            {
                return "";
            }

        }
        public void Launch(string ldName)//启动模拟器
        {
            PipeCmd("launch --name " + ldName);
        }
        public void Launch(int ldIndex)
        {
            PipeCmd("launch --index " + ldIndex.ToString());
        }
        public void Quit()//关闭模拟器
        {
            PipeCmd("quitall");
        }
        public void Quit(string ldName)
        {
            PipeCmd("quit --name " + ldName);
        }
        public void Quit(int ldIndex)
        {
            PipeCmd("quit --index " + ldIndex.ToString());
        }
        public void Reboot(string ldName)//重启模拟器
        {
            PipeCmd("reboot --name " + ldName);
        }
        public void Reboot(int ldIndex)
        {
            PipeCmd("reboot --index " + ldIndex.ToString());
        }
        public void RebootToApp(string ldName, string appid = "null")//重启模拟器并打开指定应用
        {
            PipeCmd("action --name " + ldName + " --key call.reboot--value " + appid);

        }
        public void RebootToApp(int ldIndex, string appid = "null")
        {
            PipeCmd("action --index " + ldIndex.ToString() + " --key call.reboot--value " + appid);

        }
        public void Add(string ldName)//新建模拟器
        {
            PipeCmd("add --name " + ldName);
        }
        public void Copy(string ldName, int ldIndex)//复制模拟器
        {
            PipeCmd("copy --name " + ldName + " --from " + ldIndex.ToString());
        }
        public void Remove(string ldName)//删除模拟器
        {
            PipeCmd("remove  --name " + ldName);
        }
        public void Remove(int ldIndex)//删除模拟器
        {
            PipeCmd("remove  --index " + ldIndex.ToString());
        }
        public void RunApp(string ldName, string appid)//启动app
        {
            PipeCmd("runapp --name " + ldName + " --packagename " + appid);
        }
        public void RunApp(int ldIndex, string appid)
        {
            PipeCmd("runapp --index " + ldIndex.ToString() + " --packagename " + appid);
        }
        public void Killapp(string ldName, string appid)//关闭app
        {
            PipeCmd("killapp --name " + ldName + " --packagename " + appid);
        }
        public void Killapp(int ldIndex, string appid)
        {
            PipeCmd("killapp --index " + ldIndex.ToString() + " --packagename " + appid);
        }
        public void Installapp(string ldName, string filePath)//安装app
        {
            PipeCmd("installapp --name " + ldName + " --filename " + filePath);
        }
        public void Installapp(int ldIndex, string filePath)
        {
            PipeCmd("installapp --index " + ldIndex.ToString() + " --filename " + filePath);
        }
        public void Uninstallapp(string ldName, string appid)//卸载app
        {
            PipeCmd("uninstallapp --name " + ldName + " --packagename " + appid);
        }
        public void Uninstallapp(int ldIndex, string appid)
        {
            PipeCmd("uninstallapp --index " + ldIndex.ToString() + " --packagename " + appid);
        }
        public void Keyboard(string ldName, string keyValue)//执行安卓按键(back/home/menu/volumeup/volumedown)
        {
            PipeCmd("action --name " + ldName + " --key call.keyboard --value " + keyValue);
        }
        public void Keyboard(int ldIndex, string keyValue)
        {
            PipeCmd("action --index " + ldIndex.ToString() + " --key call.keyboard --value " + keyValue);
        }
        public void Locate(string ldName, string lngLat)//修改经纬度
        {
            PipeCmd("action --name " + ldName + " --key call.locate --value " + lngLat);
        }
        public void Locate(int ldIndex, string lngLat)
        {
            PipeCmd("action --index " + ldIndex.ToString() + " --key call.locate --value " + lngLat);
        }
        public void Shake(string ldName)//摇一摇
        {
            PipeCmd("action --name " + ldName + " --key call.shake --value null");
        }
        public void Shake(int ldIndex)
        {
            PipeCmd("action --index " + ldIndex.ToString() + " --key call.shake --value null");
        }
        public void Input(string ldName, string inputStr)//文字输入
        {
            PipeCmd("action --name " + ldName + " --key call.input --value " + inputStr);
        }
        public void Input(int ldIndex, string inputStr)
        {
            PipeCmd("action --index " + ldIndex.ToString() + " --key call.input --value " + inputStr);
        }
        public void Modify(string ldName, short width, short height, short dpi, short cpu, short memory, string manufacturer, string model, long phoneNumber, string imei = "auto", string imsi = "auto", string simserial = "auto", string androidid = "auto", string mac = "auto")//模拟器属性设置
        {
            PipeCmd("modify --name " + ldName + " --resolution " + width.ToString() + "," + height.ToString() + "," + dpi.ToString() + " --cpu " +
               cpu.ToString() + " --memory " + memory.ToString() + " --manufacturer " + manufacturer + " --model " + model + " --pnumber " +
                phoneNumber.ToString() + " --imei " + imei + " --imsi " + imsi + " --simserial " + simserial + " --androidid " + androidid + " --mac " + mac);
        }
        public void Modify(int ldIndex, short width, short height, short dpi, short cpu, short memory, string manufacturer, string model, long phoneNumber, string imei = "auto", string imsi = "auto", string simserial = "auto", string androidid = "auto", string mac = "auto")
        {
            PipeCmd("modify --index " + ldIndex.ToString() + " --resolution " + width.ToString() + "," + height.ToString() + "," + dpi.ToString() + " --cpu " +
               cpu.ToString() + " --memory " + memory.ToString() + " --manufacturer " + manufacturer + " --model " + model + " --pnumber " +
                phoneNumber.ToString() + " --imei " + imei + " --imsi " + imsi + " --simserial " + simserial + " --androidid " + androidid + " --mac " + mac);
        }
        public void Scan(string ldName, string filePath)//扫描二维码,需要app先启动扫描,再调用这个命令
        {
            PipeCmd("qrpicture --name " + ldName + " --file " + filePath);
        }
        public void Scan(int ldIndex, string filePath)
        {
            PipeCmd("qrpicture --index " + ldIndex.ToString() + " --file " + filePath);
        }
        public void SortWnd()//一键排序 , 需先在多开器配置排序规则
        {
            PipeCmd("sortWnd");
        }
        public void ClearApp(int ldIndex, string appid)//清除应用数据
        {
            PipeCmd("-s " + ldIndex.ToString() + " pm clear " + appid, true);
        }
        public void InputKey(int ldIndex, short keyCode)//模拟按键 , 具体键值请百度
        {
            PipeCmd("-s " + ldIndex.ToString() + " input keyevent " + keyCode, true);
        }
        public void InputText(int ldIndex, string text)//文本输入 , 不支持中文
        {
            PipeCmd("-s " + ldIndex.ToString() + " input text " + text, true);
        }
        public void Click(int ldIndex, short X, short Y)//点击
        {
            PipeCmd("-s " + ldIndex.ToString() + " input tap " + X.ToString() + " " + Y.ToString(), true);
        }
        public void Swipe(int ldIndex, short startX, short startY, short endX, short endY, short time = 1000)//滑动
        {
            PipeCmd("-s " + ldIndex.ToString() + " input swipe " + startX.ToString() + " " + startY.ToString() + " " + endX.ToString() + " " + endY.ToString() + " " + time.ToString(), true);
        }
    }
}
