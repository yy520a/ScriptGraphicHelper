using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;

namespace ScriptGraphicHelper.Models
{
    public static class MyEmulator
    {
        public static int IsInit { get; set; } = -1;
        public static ObservableCollection<string> Result { get; set; }
        public static List<KeyValuePair<int, string>> Info { get; set; }
        public static int Select { get; set; } = -1;

        private static int _index;
        public static int Index
        {
            get { return _index; }
            set
            {
                if (value != -1)
                {
                    _index = Info[value].Key;
                }
            }
        }

        public static List<EmulatorHelper> Emulators = new List<EmulatorHelper>();
        public static ObservableCollection<string> Init()
        {
            Emulators.Add(new LdEmulatorHelper(0));
            Emulators.Add(new LdEmulatorHelper(1));
            Emulators.Add(new YsEmulatorHelper());
            Emulators.Add(new XyEmulatorHelper());
            Emulators.Add(new MobileTcpHelper());
            Result = new ObservableCollection<string>();

            foreach (var emulator in Emulators)
            {
                if (emulator.Path != string.Empty)
                {
                    Result.Add(emulator.Name);
                }
            }
            IsInit = 0;
            return Result;
        }
        public static void Dispose()
        {
            foreach (var emulator in Emulators)
            {
                emulator.Dispose();
            }
            Result.Clear();
            Info.Clear();
            Emulators.Clear();

            IsInit = -1;

        }
        public static void Changed(int index)
        {
            if (index>=0)
            {
                for (int i = 0; i < Emulators.Count; i++)
                {
                    if (Emulators[i].Name == Result[index])
                    {
                        Select = i;
                        IsInit = 1;
                    }
                }
            }
            else
            {
                Select = -1;
                IsInit = -1;
            }
        }
        public static ObservableCollection<string> GetAll()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            Info = Emulators[Select].ListAll();
            foreach (var item in Info)
            {
                result.Add(item.Value);
            }
            return result;
        }
        public static Bitmap ScreenShot()
        {
            return Emulators[Select].ScreenShot(Index);
        }
    }
}
