using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

enum Anchors
{
    L = 0,
    C = 1,
    R = 2
}

namespace ScriptGraphicHelper.Models
{
    public static class GraphicHelper
    {
        public static int Width = 0;
        public static int Height = 0;
        public static int FormatSize;
        public static int Stride;
        public static byte[] Bytes;

        public static void KeepScreen(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            BitmapData Data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr IntPtr = Data.Scan0;
            FormatSize = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            Stride = Data.Stride;
            Bytes = new byte[Stride * Height];
            Marshal.Copy(IntPtr, Bytes, 0, Stride * Height);
            bitmap.UnlockBits(Data);
        }
        public static byte[] GetPixel(int x, int y)
        {
            byte[] retRGB = new byte[] { 0, 0, 0 };
            try
            {
                if (x < Width && y < Height)
                {
                    int location = x * FormatSize + Stride * y;
                    retRGB[0] = Bytes[location + 2];
                    retRGB[1] = Bytes[location + 1];
                    retRGB[2] = Bytes[location];
                }
            }
            catch
            {
                retRGB = new byte[] { 0, 0, 0 };
            }
            return retRGB;
        }

        public static bool AnchorsCompareColor(double width, double height, string colorString, byte sim = 95)
        {
            string color = colorString.Trim("\"".ToCharArray());
            string[] findColorArr = color.Split(',');

            double multiple = Height / height;
            string result = string.Empty;
            for (int i = 0; i < findColorArr.Length; i++)
            {
                string[] findColor = findColorArr[i].Split('|');
                double findX = int.Parse(findColor[1]);
                double findY = int.Parse(findColor[2]);
                if (findColor[0] == "L")
                {
                    findX = Math.Floor(findX * multiple);
                    findY = Math.Floor(findY * multiple);
                }
                else if (findColor[0] == "C")
                {
                    findX = Math.Floor(Width / 2 - (width / 2 - findX) * multiple);
                    findY = Math.Floor(findY * multiple);
                }
                else if (findColor[0] == "R")
                {
                    findX = Math.Floor(Width - (width - findX) * multiple);
                    findY = Math.Floor(findY * multiple);
                }
                result += findX.ToString() + "|" + findY.ToString() + "|" + findColor[3] + ",";
            }
            result = result.Trim(',');
            return CompareColor(result, sim);

        }
        public static bool CompareColor(string colorString, byte sim = 95, int X = 0, int Y = 0)
        {
            int findX;
            int findY;
            colorString = colorString.Trim("\"".ToCharArray());
            double similarity = 255 - 255 * (sim / 100.0);
            string[] findColorA = colorString.Split(',');
            if (findColorA.Length != 0)
            {
                for (byte i = 0; i < findColorA.Length; i++)
                {
                    string[] findColorP = findColorA[i].Split('|');
                    string[] findColorS = findColorP[2].Split('-');
                    byte[] findRGB = { 0, 0, 0 };
                    findRGB[0] = Convert.ToByte(findColorS[0].Substring(0, 2), 16);
                    findRGB[1] = Convert.ToByte(findColorS[0].Substring(2, 2), 16);
                    findRGB[2] = Convert.ToByte(findColorS[0].Substring(4, 2), 16);
                    findX = X + int.Parse(findColorP[0]);
                    findY = Y + int.Parse(findColorP[1]);
                    if (findX < 0 || findY < 0 || findX > Width || findY > Height)
                    {
                        return false;
                    }
                    byte[] OffsetRGB = { 0, 0, 0 };
                    if (findColorS.Length > 1)
                    {
                        OffsetRGB[0] = Convert.ToByte(findColorS[1].Substring(0, 2), 16);
                        OffsetRGB[1] = Convert.ToByte(findColorS[1].Substring(2, 2), 16);
                        OffsetRGB[2] = Convert.ToByte(findColorS[1].Substring(4, 2), 16);
                    }
                    byte[] GetRGB = GetPixel(findX, findY);
                    if (Math.Abs(GetRGB[0] - findRGB[0]) > (OffsetRGB[0] + similarity) ||
                        Math.Abs(GetRGB[1] - findRGB[1]) > (OffsetRGB[1] + similarity) ||
                        Math.Abs(GetRGB[2] - findRGB[2]) > (OffsetRGB[2] + similarity))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public static Point FindMultiColor(int startX, int startY, int endX, int endY, string findcolorString, string compareColorString, byte sim = 95)
        {
            if (startX < endX && endX < Width && startY < endY && endY < Height)
            {
                string[] findColorS = findcolorString.Split('-');
                byte findR = Convert.ToByte(findColorS[0].Substring(0, 2), 16);
                byte findG = Convert.ToByte(findColorS[0].Substring(2, 2), 16);
                byte findB = Convert.ToByte(findColorS[0].Substring(4, 2), 16);
                byte offsetR = 0;
                byte offsetG = 0;
                byte offsetB = 0;
                if (findColorS.Length > 1)
                {
                    offsetR = Convert.ToByte(findColorS[1].Substring(0, 2), 16);
                    offsetG = Convert.ToByte(findColorS[1].Substring(2, 2), 16);
                    offsetB = Convert.ToByte(findColorS[1].Substring(4, 2), 16);
                }
                double similarity = 255 - 255 * (sim / 100.0);

                for (int i = startY; i <= endY; i++)
                {
                    for (int j = startX; j <= endX; j++)
                    {
                        int location = j * FormatSize + Stride * i;
                        if (Math.Abs(Bytes[location + 2] - findR) <= offsetR + similarity)
                        {
                            if (Math.Abs(Bytes[location + 1] - findG) <= offsetG + similarity)
                            {
                                if (Math.Abs(Bytes[location] - findB) <= offsetB + similarity)
                                {
                                    if (CompareColor(compareColorString, sim, j, i))
                                    {
                                        return new Point(j, i);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new Point(-1, -1);
        }
    }
}
