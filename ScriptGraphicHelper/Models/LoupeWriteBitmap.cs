using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScriptGraphicHelper.Models
{
    public static class LoupeWriteBitmap
    {
        private static WriteableBitmap Bitmap { get; set; }
        public static WriteableBitmap Init(int width, int height)
        {
            Bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            Bitmap.Lock();
            unsafe
            {
                var pBackBuffer = (byte*)Bitmap.BackBuffer;
                for (int j = 0; j < 16; j++)
                {
                    for (int i = 0; i < 241; i++)
                    {
                        int k = j * 16 * Bitmap.BackBufferStride + i * 4;
                        pBackBuffer[k] = 75;
                        pBackBuffer[k + 1] = 75;
                        pBackBuffer[k + 2] = 75;
                        pBackBuffer[k + 3] = 255;
                    }
                }
                for (int j = 0; j < 16; j++)
                {
                    for (int i = 0; i < 241; i++)
                    {
                        int k = i * Bitmap.BackBufferStride + j * 16 * 4;
                        pBackBuffer[k] = 75;
                        pBackBuffer[k + 1] = 75;
                        pBackBuffer[k + 2] = 75;
                        pBackBuffer[k + 3] = 255;
                    }
                }
            }
            Bitmap.AddDirtyRect(new Int32Rect(0, 0, 241, 241));
            Bitmap.Unlock();
            return Bitmap;
        }

        private static void ChangeHighLight(WriteableBitmap bmp, byte[] highLightColor)
        {
            unsafe
            {
                var pBackBuffer = (byte*)Bitmap.BackBuffer;
                for (int j = 0; j < 16; j++)
                {
                    for (int i = 0; i < 241; i++)
                    {
                        int k = j * 16 * Bitmap.BackBufferStride + i * 4;
                        if (i >= 110 && i <= 130 && j <= 8 && j >= 7)
                        {
                            pBackBuffer[k] = highLightColor[0];
                            pBackBuffer[k + 1] = highLightColor[1];
                            pBackBuffer[k + 2] = highLightColor[2];
                            pBackBuffer[k + 3] = 255;

                        }
                    }
                }
                for (int j = 0; j < 16; j++)
                {
                    for (int i = 0; i < 241; i++)
                    {
                        int k = i * Bitmap.BackBufferStride + j * 16 * 4;
                        if (i >= 110 && i <= 130 && j <= 8 && j >= 7)
                        {
                            pBackBuffer[k] = highLightColor[0];
                            pBackBuffer[k + 1] = highLightColor[1];
                            pBackBuffer[k + 2] = highLightColor[2];
                            pBackBuffer[k + 3] = 255;
                        }
                    }
                }
            }
        }

        private static int IsOffsetSame(List<byte[]> colors)
        {
            int result = 24;
            double similarity = 8;
            byte[] color = colors[7 * 15 + 7];
            List<byte[]> offsetIndex = new List<byte[]>
            {
                new byte[]{ 6, 7},
                new byte[]{ 6, 8},
                new byte[]{ 7, 8},
                new byte[]{ 8, 8},
                new byte[]{ 8, 7},
                new byte[]{ 8, 6},
                new byte[]{ 7, 6},
                new byte[]{ 6, 6},
                new byte[]{ 5, 6},
                new byte[]{ 5, 7},
                new byte[]{ 5, 8},
                new byte[]{ 5, 9},
                new byte[]{ 6, 9},
                new byte[]{ 7, 9},
                new byte[]{ 8, 9},
                new byte[]{ 9, 9},
                new byte[]{ 9, 8},
                new byte[]{ 9, 7},
                new byte[]{ 9, 6},
                new byte[]{ 9, 5},
                new byte[]{ 8, 5},
                new byte[]{ 7, 5},
                new byte[]{ 6, 5},
                new byte[]{ 5, 5},
            };
            for (int i = 0; i < 24; i++)
            {
                byte[] offsetColor = colors[offsetIndex[i][0] * 15 + offsetIndex[i][1]];
                if (Math.Abs(color[0] - offsetColor[0]) > similarity || Math.Abs(color[1] - offsetColor[1]) > similarity || Math.Abs(color[2] - offsetColor[2]) > similarity)
                {
                    result = i;
                    break;
                }
            }
            if (result == 24)
                return 2;
            else if (result >= 8)
                return 1;
            else
                return 0;
        }
        public static bool WriteColor(this WriteableBitmap bmp, List<byte[]> colors)
        {
            List<byte[]> highLightColor = new List<byte[]>
            {
            new byte[]{ 60, 20, 220 },
            new byte[]{ 0x1A, 0xB1, 0xF9 },
            new byte[]{ 0x14, 0xB8, 0x6E }
            };
            int offsetRange = IsOffsetSame(colors);
            bmp.Lock();
            unsafe
            {
                ChangeHighLight(bmp, highLightColor[offsetRange]);
                var pBackBuffer = (byte*)bmp.BackBuffer;
                for (int y = 0; y < 15; y++)
                {
                    for (int x = 0; x < 15; x++)
                    {
                        byte[] color = colors[y * 15 + x];
                        for (int i = y * 16 + 1; i < y * 16 + 16; i++)
                        {
                            for (int j = x * 16 + 1; j < x * 16 + 16; j++)
                            {
                                int k = i * Bitmap.BackBufferStride + j * 4;
                                if (i >= 110 && i <= 130 && ((j >= 110 && j <= 112) || (j >= 128 && j <= 130)))
                                {
                                    pBackBuffer[k] = highLightColor[offsetRange][0];
                                    pBackBuffer[k + 1] = highLightColor[offsetRange][1];
                                    pBackBuffer[k + 2] = highLightColor[offsetRange][2];
                                    pBackBuffer[k + 3] = 255;
                                }
                                else if (j >= 110 && j <= 130 && ((i >= 110 && i <= 112) || (i >= 128 && i <= 130)))
                                {
                                    pBackBuffer[k] = highLightColor[offsetRange][0];
                                    pBackBuffer[k + 1] = highLightColor[offsetRange][1];
                                    pBackBuffer[k + 2] = highLightColor[offsetRange][2];
                                    pBackBuffer[k + 3] = 255;
                                }
                                else
                                {
                                    pBackBuffer[k] = color[2];
                                    pBackBuffer[k + 1] = color[1];
                                    pBackBuffer[k + 2] = color[0];
                                    pBackBuffer[k + 3] = 255;
                                }
                            }
                        }
                    }
                }
            }
            bmp.AddDirtyRect(new Int32Rect(0, 0, 241, 241));
            bmp.Unlock();
            return true;
        }
    }
}
