using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptGraphicHelper.Models
{
    public class Setting
    {
        public bool LastSize { get; set; }
        public bool LastSim { get; set; }
        public bool LastFormat { get; set; }
        public double LastWidth { get; set; }
        public double LastHeight { get; set; }
        public int SimSelect { get; set; }
        public int FormatSelect { get; set; }

    }
}
