using System.Windows;

namespace ScriptGraphicHelper.Models
{
    public class Setting
    {
        public bool LastSize { get; set; } = true;
        public double LastWidth { get; set; } = 1760;
        public double LastHeight { get; set; } = 990;
        public int LastSim { get; set; } = 0;
        public int LastFormat { get; set; } = 0;
        public bool LastOffsetColorShow { get; set; } = false;
        public int LastHintColorShow { get; set; } = 0;
        public string LastAllOffset { get; set; } = "000000";
    }
}
