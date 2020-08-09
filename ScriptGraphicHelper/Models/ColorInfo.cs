using System;
using System.Drawing;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;

namespace ScriptGraphicHelper.Models
{
    public class ColorInfo
    {
        public int Index { get; set; }
        public string Anchors { get; set; }
        public string PointStr { get; set; }
        public string ColorStr { get; set; }
        public string OffsetColor { get; set; }
        public bool IsChecked { get; set; }
        public Brush TheBrush { get; set; }
        public System.Drawing.Color TheColor { get; set; }
        public Point ThePoint { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public static string AllOffsetColor { get; set; } = "000000";

        public ColorInfo(int index, Point point, System.Drawing.Color color)
        {
            TheColor = color;
            point.X = Math.Round(point.X, 0);
            point.Y = Math.Round(point.Y, 0);
            ThePoint = point;

            Index = index;
            Anchors = "L";
            PointStr = point.X.ToString() + "," + point.Y.ToString();
            ColorStr = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            OffsetColor = AllOffsetColor;
            TheBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
            IsChecked = true;
        }
        public ColorInfo(int index, string anchors, Point point, System.Drawing.Color color, double width, double height)
        {
            TheColor = color;
            point.X = Math.Round(point.X, 0);
            point.Y = Math.Round(point.Y, 0);
            ThePoint = point;
            Index = index;
            Anchors = anchors;
            PointStr = point.X.ToString() + "," + point.Y.ToString();
            ColorStr = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            OffsetColor = AllOffsetColor;
            TheBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
            IsChecked = true;
            Width = width;
            Height = height;
        }
    }
}
