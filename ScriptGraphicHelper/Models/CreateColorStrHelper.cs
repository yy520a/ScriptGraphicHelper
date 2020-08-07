using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace 综合图色助手.Models
{
    public static class CreateColorStrHelper
    {
        public static string Create(int index, ObservableCollection<ColorInfo> colorInfos, Range rect = null)
        {
            return index switch
            {
                0 => CompareStr(colorInfos),
                1 => DmFindStr(colorInfos, rect),
                2 => AjFindStr(colorInfos, rect),
                3 => AjCompareStr(colorInfos),
                4 => CdFindStr(colorInfos, rect),
                5 => CdCompareStr(colorInfos),
                6 => AutojsFindStr(colorInfos, rect),
                7 => EcFindStr(colorInfos, rect),
                8 => AnchorsCompareStr(colorInfos),
                9 => AnchorsCompareStrTest(colorInfos),
                _ => CompareStr(colorInfos),
            };
        }

        public static string DmFindStr(ObservableCollection<ColorInfo> colorInfos, Range rect)
        {
            string result = rect.ToStr() + ",";
            bool isInit = false;
            Point startPoint = new Point();
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (!isInit)
                    {
                        isInit = true;
                        startPoint = colorInfo.ThePoint;
                        result += "\"" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "\",\"";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + ",";
                    }
                }
            }
            result = result.Trim(',');
            result += "\"";
            return result;
        }
        public static string CdFindStr(ObservableCollection<ColorInfo> colorInfos, Range rect)
        {
            string result = string.Empty;
            Point startPoint = new Point();
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (result == string.Empty)
                    {
                        startPoint = colorInfo.ThePoint;
                        result += "0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + ",\"";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + ",";
                    }
                }
            }
            result = result.Trim(',');
            result += "\",90," + rect.ToStr();
            return result;
        }
        public static string AjFindStr(ObservableCollection<ColorInfo> colorInfos, Range rect)
        {
            string result = rect.ToStr() + ","; ;
            bool isInit = false;
            Point startPoint = new Point();
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (!isInit)
                    {
                        isInit = true;
                        startPoint = colorInfo.ThePoint;
                        result += "\"" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "\",\"";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.R.ToString("x2") + ",";
                    }
                }
            }
            result = result.Trim(',');
            result += "\"";
            return result;
        }
        public static string AutojsFindStr(ObservableCollection<ColorInfo> colorInfos, Range rect)
        {
            string result = string.Empty;
            Point startPoint = new Point();
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (result == string.Empty)
                    {
                        startPoint = colorInfo.ThePoint;
                        result = "\"#" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "\",[";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        result += "[" + OffsetX.ToString() + "," + OffsetY.ToString() + ",\"#" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + "\"],";
                    }
                }
            }
            result = result.Trim(',');
            result += "],{region:[" + rect.ToStr(1) + "],threshold:[26]}";
            return result;

        }
        public static string EcFindStr(ObservableCollection<ColorInfo> colorInfos, Range rect)
        {
            string result = string.Empty;
            Point startPoint = new Point();
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (result==string.Empty)
                    {
                        startPoint = colorInfo.ThePoint;
                        result += "\"0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "\",\"";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + ",";
                    }
                }
            }
            result = result.Trim(',');
            result += "\",0.9,"+rect.ToStr();
            return result;
        }
        public static string CompareStr(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = "\"";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    result += colorInfo.ThePoint.X.ToString() + "|" + colorInfo.ThePoint.Y.ToString() + "|" + colorInfo.TheColor.R.ToString("x2") +
                        colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + ",";
                }
            }
            result = result.Trim(',');
            result += "\"";
            return result;
        }
        public static string AjCompareStr(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = "\"";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    result += colorInfo.ThePoint.X.ToString() + "|" + colorInfo.ThePoint.Y.ToString() + "|" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + ",";
                }
            }
            result = result.Trim(',');
            result += "\"";
            return result;
        }
        public static string CdCompareStr(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = "[";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    result += "[" + colorInfo.ThePoint.X.ToString() + "," + colorInfo.ThePoint.Y.ToString() + ",0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "],";
                }
            }
            result = result.Trim(',');
            result += "]";
            return result;
        }



        public static string AnchorsCompareStr(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = colorInfos[0].Width.ToString() + "," + colorInfos[0].Height.ToString() + ",{";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (colorInfo.Anchors == "L")
                        result += "{Anchors.Left,";
                    if (colorInfo.Anchors == "C")
                        result += "{Anchors.Center,";
                    if (colorInfo.Anchors == "R")
                        result += "{Anchors.Right,";

                    string r = Rgb2JavaByte(colorInfo.TheColor.R);
                    string g = Rgb2JavaByte(colorInfo.TheColor.G);
                    string b = Rgb2JavaByte(colorInfo.TheColor.B);
                    result += colorInfo.ThePoint.X.ToString() + "," + colorInfo.ThePoint.Y.ToString() + "," + r + "," + g + "," + b + "},";
                }
            }
            result = result.Trim(',');
            result += "}";
            return result;
        }
        public static string AnchorsCompareStrTest(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = "\"";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    result += colorInfo.Anchors + "|" + colorInfo.ThePoint.X.ToString() + "|" + colorInfo.ThePoint.Y.ToString() + "|" + colorInfo.TheColor.R.ToString("x2") +
                        colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + ",";
                }
            }
            result = result.Trim(',');
            result += "\"";
            return result;
        }


        private static string Rgb2JavaByte(double number)
        {
            int num = (int)number;
            int result = (int)num;
            if (number > 128)
            {
                result = num % 128;
                result = -1 * (128 - result);
            }
            return result.ToString();
        }
    }
}
