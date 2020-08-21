using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;

namespace ScriptGraphicHelper.Models
{
    public class ColorInfo : BindableBase
    {
        public int Index { get; set; }
        public string Anchors { get; set; }
        public string PointStr { get; set; }

        private string _colorStr;
        public string ColorStr
        {
            get { return _colorStr; }
            set { SetProperty(ref _colorStr, value); }
        }
        private string _offsetColor;
        public string OffsetColor
        {
            get { return _offsetColor; }
            set { SetProperty(ref _offsetColor, value.ToUpper()); }
        }
        public bool IsChecked { get; set; }

        public Brush Brush
        {
            get
            {
                if (BrushMode == 0)
                {
                   return Brush_1;
                }
                else
                {
                    return Brush_2;
                }
            }
            set
            {
                if (BrushMode == 0)
                {
                    Brush_1 = value;
                    Brush_2 = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66));
                }
                else
                {
                    Brush_1 = new SolidColorBrush(Color.FromRgb(0XFF, 0XFF, 0XFF));
                    Brush_2 = value;
                }
            }
        }
        private Brush brush_1;
        public Brush Brush_1
        {
            get { return brush_1; }
            set { SetProperty(ref brush_1, value); }
        }
        private Brush brush_2;
        public Brush Brush_2
        {
            get { return brush_2; }
            set { SetProperty(ref brush_2, value); }
        }
        public System.Drawing.Color TheColor { get; set; }
        public Point ThePoint { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public static string AllOffsetColor { get; set; } = "000000";
        public static int BrushMode { get; set; } = 0;

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
            OffsetColor = AllOffsetColor.ToString();
            if (BrushMode == 0)
            {
                Brush_1 = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                Brush_2 = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66));
            }
            else
            {
                Brush_1 = new SolidColorBrush(Color.FromRgb(0XFF, 0XFF, 0XFF));
                Brush_2 = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
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
            if (BrushMode == 0)
            {
                Brush_1 = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                Brush_2 = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66));
            }
            else
            {
                Brush_1 = new SolidColorBrush(Color.FromRgb(0XFF, 0XFF, 0XFF));
                Brush_2 = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            IsChecked = true;
            Width = width;
            Height = height;
        }


    }
}
