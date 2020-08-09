using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ScriptGraphicHelper.Models
{
    public abstract class EmulatorHelper: IDisposable
    {
        public abstract string Path { get; set; }
        public abstract string Name { get; set; }
        public abstract bool IsStart(int Index);
        public abstract List<KeyValuePair<int, string>> ListAll();
        public abstract Bitmap ScreenShot(int Index);
        public abstract void Dispose();
    }
}
