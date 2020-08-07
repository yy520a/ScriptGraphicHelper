using System;
using System.Collections.Generic;
using System.Text;

namespace 综合图色助手.Models
{
    public class Range
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }

        public Range(double left, double top, double right, double bottom)
        {
            Left = left; 
            Top = top;
            Right = right; 
            Bottom = bottom;
        }
        public string ToStr(int mode=0)
        {
            if (mode==0)
            {
                return Left.ToString() + "," + Top.ToString() + "," + Right.ToString() + "," + Bottom.ToString();
            }
            else
            {
                double width = Right - Left;
                double height = Bottom - Top;
                return Left.ToString() + "," + Top.ToString() + "," + width.ToString() + "," + height.ToString();
            }
        }
    }
}
