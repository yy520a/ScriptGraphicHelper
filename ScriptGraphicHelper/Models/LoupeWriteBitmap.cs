using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace 综合图色助手.Models
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
                        if (i >= 110 && i <= 130 && j <= 8 && j >= 7)
                        {
                            pBackBuffer[k] = 60;
                            pBackBuffer[k + 1] = 20;
                            pBackBuffer[k + 2] = 220;
                            pBackBuffer[k + 3] = 255;

                        }
                        else
                        {
                            pBackBuffer[k] = 75;
                            pBackBuffer[k + 1] = 75;
                            pBackBuffer[k + 2] = 75;
                            pBackBuffer[k + 3] = 255;
                        }
                    }
                }
                int t = 0;
                for (int j = 0; j < 16; j++)
                {
                    for (int i = 0; i < 241; i++)
                    {
                        int k = i * Bitmap.BackBufferStride + j * 16 * 4;
                        if (i >= 110 && i <= 130 && j <= 8 && j >= 7)
                        {
                            pBackBuffer[k] = 60;
                            pBackBuffer[k + 1] = 20;
                            pBackBuffer[k + 2] = 220;
                            pBackBuffer[k + 3] = 255;
                        }
                        else
                        {
                            pBackBuffer[k] = 75;
                            pBackBuffer[k + 1] = 75;
                            pBackBuffer[k + 2] = 75;
                            pBackBuffer[k + 3] = 255;
                        }
                        t++;
                    }
                }
            }
            Bitmap.AddDirtyRect(new Int32Rect(0, 0, 241, 241));
            Bitmap.Unlock();
            return Bitmap;
        }

        public static bool WriteColor(this WriteableBitmap bmp, List<byte[]> colors)
        {
            bmp.Lock();
            unsafe
            {
                var pBackBuffer = (byte*)bmp.BackBuffer;
                int s = (7 * 16 + 1) * Bitmap.BackBufferStride + (7 * 16 + 1) * 4;
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
                                    pBackBuffer[k] = 60;
                                    pBackBuffer[k + 1] = 20;
                                    pBackBuffer[k + 2] = 220;
                                    pBackBuffer[k + 3] = 255;
                                }
                                else if (j >= 110 && j <= 130 && ((i >= 110 && i <= 112) || (i >= 128 && i <= 130)))
                                {
                                    pBackBuffer[k] = 60;
                                    pBackBuffer[k + 1] = 20;
                                    pBackBuffer[k + 2] = 220;
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
