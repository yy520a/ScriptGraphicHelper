using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptGraphicHelper.Models
{
   public static class DiyFormat
    {
        public static int ColorMode { get; set; } = 0;//0:字符串找色 1:字符串比色 2:数组找色 3:数组比色
        public static bool StartPoint { get; set; } = false;
        public static bool BGR { get; set; } = false;
        public static string ParentSplit { get; set; } = string.Empty;
        public static string ChildSplit { get; set; } = string.Empty;
        public static string ColorPrefix { get; set; } = string.Empty;
        public static int Range { get; set; } = -1;
        public static int Point { get; set; } = -1;
        public static string Prefix { get; set; } = string.Empty;
        public static string Suffix { get; set; } = string.Empty;
        

    }
}
