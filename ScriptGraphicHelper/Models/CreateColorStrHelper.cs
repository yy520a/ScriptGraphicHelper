using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using static System.Environment;

namespace ScriptGraphicHelper.Models
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
                8 => DiyStr(colorInfos, rect),
                9 => AnchorsCompareStr(colorInfos),
                10 => AnchorsCompareStrTest(colorInfos),
                _ => CompareStr(colorInfos),
            };
        }

        public static string DiyStr(ObservableCollection<ColorInfo> colorInfos, Range rect)
        {
            try
            {
                StreamReader _sr = File.OpenText(CurrentDirectory + "\\diyFormat.json");
                string result = _sr.ReadToEnd();
                _sr.Close();
                _DiyFormat diyFormat = JsonConvert.DeserializeObject<_DiyFormat>(result);
                if (diyFormat.ColorMode > 1)
                    diyFormat.ColorMode = 0;
                if (diyFormat.Range > 2)
                    diyFormat.ColorMode = 0;
                if (diyFormat.Range > 2)
                    diyFormat.ColorMode = 2;

                DiyFormat.ColorMode = diyFormat.ColorMode;
                DiyFormat.StartPoint = diyFormat.StartPoint;
                DiyFormat.ParentSplit = diyFormat.ParentSplit;
                DiyFormat.ChildSplit = diyFormat.ChildSplit;
                DiyFormat.BGR = diyFormat.BGR;
                DiyFormat.ColorPrefix = diyFormat.ColorPrefix;
                DiyFormat.Range = diyFormat.Range;
                DiyFormat.Point = diyFormat.Point;
                DiyFormat.Prefix = diyFormat.Prefix;
                DiyFormat.Suffix = diyFormat.Suffix;
            }
            catch
            {
                MessageBox.Show("diyFormat.json文件不存在或自定义格式错误!");
            }
            if (DiyFormat.ColorMode == 0)
            {
                return DiyFindStr(colorInfos, rect);
            }
            else if (DiyFormat.ColorMode == 1)
            {
                return DiyCompareStr(colorInfos);
            }
            else
            {
                return DiyFindStr(colorInfos, rect);
            }
        }
        public static string DiyCompareStr(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = "\"";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (colorInfo.OffsetColor == "000000")
                        if (!DiyFormat.BGR)
                            result += colorInfo.ThePoint.X.ToString() + DiyFormat.ChildSplit + colorInfo.ThePoint.Y.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + DiyFormat.ParentSplit;
                        else
                            result += colorInfo.ThePoint.X.ToString() + DiyFormat.ChildSplit + colorInfo.ThePoint.Y.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + DiyFormat.ParentSplit;
                    else
                    {
                        if (!DiyFormat.BGR)
                            result += colorInfo.ThePoint.X.ToString() + DiyFormat.ChildSplit + colorInfo.ThePoint.Y.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "-" + colorInfo.OffsetColor + DiyFormat.ParentSplit;
                        else
                        {
                            string offsetColor = colorInfo.OffsetColor.Substring(4, 2) + colorInfo.OffsetColor.Substring(2, 2) + colorInfo.OffsetColor.Substring(0, 2);
                            result += colorInfo.ThePoint.X.ToString() + DiyFormat.ChildSplit + colorInfo.ThePoint.Y.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + DiyFormat.ParentSplit;
                        }
                    }
                }
            }
            result = result.Trim(DiyFormat.ParentSplit.ToCharArray());
            result += "\"";
            if (DiyFormat.Point == 0)
            {
                result = "x,y," + result;
            }
            else if (DiyFormat.Point > 0)
            {
                result += ",x,y";
            }
            return DiyFormat.Prefix + result + DiyFormat.Suffix;
        }
        public static string DiyFindStr(ObservableCollection<ColorInfo> colorInfos, Range rect)
        {
            bool isInit = false;
            Point startPoint = new Point();
            string result = "\"";

            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (!isInit)
                    {
                        isInit = true;
                        startPoint = colorInfo.ThePoint;
                        if (DiyFormat.StartPoint)
                        {
                            result += "0" + DiyFormat.ChildSplit + "0" + DiyFormat.ChildSplit;
                        }
                        if (colorInfo.OffsetColor == "000000")
                        {
                            if (!DiyFormat.BGR)
                                result += DiyFormat.ColorPrefix + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "\"" + DiyFormat.ParentSplit + "\"";
                            else
                                result += DiyFormat.ColorPrefix + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "\"" + DiyFormat.ParentSplit + "\"";
                        }
                        else
                        {
                            if (!DiyFormat.BGR)
                                result += DiyFormat.ColorPrefix + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "-" + DiyFormat.ColorPrefix + colorInfo.OffsetColor + "\"" + DiyFormat.ParentSplit + "\"";
                            else
                            {
                                string offsetColor = colorInfo.OffsetColor.Substring(4, 2) + colorInfo.OffsetColor.Substring(2, 2) + colorInfo.OffsetColor.Substring(0, 2);
                                result += DiyFormat.ColorPrefix + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "-" + DiyFormat.ColorPrefix + offsetColor + "\"" + DiyFormat.ParentSplit + "\"";
                            }
                        }
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        if (colorInfo.OffsetColor == "000000")
                        {
                            if (!DiyFormat.BGR)
                                result += OffsetX.ToString() + DiyFormat.ChildSplit + OffsetY.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + DiyFormat.ParentSplit;
                            else
                                result += OffsetX.ToString() + DiyFormat.ChildSplit + OffsetY.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + DiyFormat.ParentSplit;
                        }
                        else
                        {
                            if (!DiyFormat.BGR)
                                result += OffsetX.ToString() + DiyFormat.ChildSplit + OffsetY.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "-" + DiyFormat.ColorPrefix + colorInfo.OffsetColor + DiyFormat.ParentSplit;
                            else
                            {
                                string offsetColor = colorInfo.OffsetColor.Substring(4, 2) + colorInfo.OffsetColor.Substring(2, 2) + colorInfo.OffsetColor.Substring(0, 2);
                                result += OffsetX.ToString() + DiyFormat.ChildSplit + OffsetY.ToString() + DiyFormat.ChildSplit + DiyFormat.ColorPrefix + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "-" + DiyFormat.ColorPrefix + offsetColor + DiyFormat.ParentSplit;
                            }
                        }
                    }
                }
            }
            result = result.Trim(DiyFormat.ParentSplit.ToCharArray());
            result += "\"";

            if (DiyFormat.Range == 0)
                result = rect.ToStr() + "," + result;
            else if (DiyFormat.Point == 0)
            {
                result = "x,y," + result;
            }
            if (DiyFormat.Range == 1)
                result += "," + rect.ToStr();
            else if (DiyFormat.Point == 1)
            {
                result += ",x,y";
            }
            if (DiyFormat.Range == 2)
                result += "," + rect.ToStr();
            else if (DiyFormat.Point == 2)
            {
                result += ",x,y";
            }
            return DiyFormat.Prefix + result + DiyFormat.Suffix;
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
                        if (colorInfo.OffsetColor == "000000")
                            result += "\"" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "\",\"";
                        else
                            result += "\"" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "-" + colorInfo.OffsetColor + "\",\"";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        if (colorInfo.OffsetColor == "000000")
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + ",";
                        else
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + "-" + colorInfo.OffsetColor + ",";
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
                        if (colorInfo.OffsetColor == "000000")
                            result += "0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + ",\"";
                        else
                            result += "0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "|0x" + colorInfo.OffsetColor + ",\"";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        if (colorInfo.OffsetColor == "000000")
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + ",";
                        else
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + "|0x" + colorInfo.OffsetColor + ",";
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
                        if (colorInfo.OffsetColor == "000000")
                            result += "\"" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "\",\"";
                        else
                        {
                            string offsetColor = colorInfo.OffsetColor.Substring(4, 2) + colorInfo.OffsetColor.Substring(2, 2) + colorInfo.OffsetColor.Substring(0, 2);
                            result += "\"" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "-" + offsetColor + "\",\"";
                        }
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        if (colorInfo.OffsetColor == "000000")
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.R.ToString("x2") + ",";
                        else
                        {
                            string offsetColor = colorInfo.OffsetColor.Substring(4, 2) + colorInfo.OffsetColor.Substring(2, 2) + colorInfo.OffsetColor.Substring(0, 2);
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "-" + offsetColor + ",";
                        }
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
                    if (result == string.Empty)
                    {
                        startPoint = colorInfo.ThePoint;
                        if (colorInfo.OffsetColor == "000000")
                            result += "\"0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "\",\"";
                        else
                            result += "\"0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "-0x" + colorInfo.OffsetColor + "\",\"";
                    }
                    else
                    {
                        double OffsetX = colorInfo.ThePoint.X - startPoint.X;
                        double OffsetY = colorInfo.ThePoint.Y - startPoint.Y;
                        if (colorInfo.OffsetColor == "000000")
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                            colorInfo.TheColor.B.ToString("x2") + ",";
                        else
                            result += OffsetX.ToString() + "|" + OffsetY.ToString() + "|0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") +
                           colorInfo.TheColor.B.ToString("x2") + "-0x" + colorInfo.OffsetColor + ",";
                    }
                }
            }
            result = result.Trim(',');
            result += "\",0.9," + rect.ToStr();
            return result;
        }
        public static string CompareStr(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = "\"";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (colorInfo.OffsetColor == "000000")
                        result += colorInfo.ThePoint.X.ToString() + "|" + colorInfo.ThePoint.Y.ToString() + "|" + colorInfo.TheColor.R.ToString("x2") +
                        colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + ",";
                    else
                        result += colorInfo.ThePoint.X.ToString() + "|" + colorInfo.ThePoint.Y.ToString() + "|" + colorInfo.TheColor.R.ToString("x2") +
                        colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "-" + colorInfo.OffsetColor + ",";
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
                    if (colorInfo.OffsetColor == "000000")
                        result += colorInfo.ThePoint.X.ToString() + "|" + colorInfo.ThePoint.Y.ToString() + "|" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + ",";
                    else
                    {
                        string offsetColor = colorInfo.OffsetColor.Substring(4, 2) + colorInfo.OffsetColor.Substring(2, 2) + colorInfo.OffsetColor.Substring(0, 2);
                        result += colorInfo.ThePoint.X.ToString() + "|" + colorInfo.ThePoint.Y.ToString() + "|" + colorInfo.TheColor.B.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.R.ToString("x2") + "-" + offsetColor + ",";
                    }
                }
            }
            result = result.Trim(',');
            result += "\"";
            return result;
        }
        public static string CdCompareStr(ObservableCollection<ColorInfo> colorInfos)
        {
            string result = "{";
            foreach (ColorInfo colorInfo in colorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    result += "{" + colorInfo.ThePoint.X.ToString() + "," + colorInfo.ThePoint.Y.ToString() + ",0x" + colorInfo.TheColor.R.ToString("x2") + colorInfo.TheColor.G.ToString("x2") + colorInfo.TheColor.B.ToString("x2") + "},";
                }
            }
            result = result.Trim(',');
            result += "}";
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
