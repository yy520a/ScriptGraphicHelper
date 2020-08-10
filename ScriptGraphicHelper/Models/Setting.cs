using System.Windows;

namespace ScriptGraphicHelper.Models
{
    public class Setting
    {
        public bool LastSize { get; set; } = true;
        public bool LastSim { get; set; } = true;
        public bool LastFormat { get; set; } = true;
        public double LastWidth { get; set; } = 1760;
        public double LastHeight { get; set; } = 990;
        public int SimSelect { get; set; } = 0;
        public int FormatSelect { get; set; } = 0;

    }
}
