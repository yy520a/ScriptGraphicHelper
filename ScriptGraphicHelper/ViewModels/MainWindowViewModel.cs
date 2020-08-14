using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using ScriptGraphicHelper.Models;
using ScriptGraphicHelper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;
using Range = ScriptGraphicHelper.Models.Range;

namespace ScriptGraphicHelper.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "综合图色助手 V1.0.8";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _emulatorOptionsHint;
        public string EmulatorOptionsHint
        {
            get { return _emulatorOptionsHint; }
            set { SetProperty(ref _emulatorOptionsHint, value); }
        }
        private string _testResult;
        public string TestResult
        {
            get { return _testResult; }
            set { SetProperty(ref _testResult, value); }
        }

        private string _range;
        public string Range
        {
            get { return _range; }
            set { SetProperty(ref _range, value); }
        }

        private string _createStr;
        public string CreateStr
        {
            get { return _createStr; }
            set { SetProperty(ref _createStr, value); }
        }
        private ObservableCollection<string> _emulatorInfo;
        public ObservableCollection<string> EmulatorInfo
        {
            get { return _emulatorInfo; }
            set => SetProperty(ref _emulatorInfo, value);
        }

        private ObservableCollection<ColorInfo> _colorInfos;
        public ObservableCollection<ColorInfo> ColorInfos
        {
            get { return _colorInfos; }
            set => SetProperty(ref _colorInfos, value);
        }

        private WriteableBitmap _loupeWriteBmp;
        public WriteableBitmap LoupeWriteBmp
        {
            get { return _loupeWriteBmp; }
            set => SetProperty(ref _loupeWriteBmp, value);
        }

        private Thickness _loupeMargin;
        public Thickness LoupeMargin
        {
            get { return _loupeMargin; }
            set => SetProperty(ref _loupeMargin, value);
        }

        private Visibility _loupeVisibility;
        public Visibility LoupeVisibility
        {
            get { return _loupeVisibility; }
            set { SetProperty(ref _loupeVisibility, value); }
        }

        private double _pointX;
        public double PointX
        {
            get { return _pointX; }
            set { SetProperty(ref _pointX, value); }
        }

        private double _pointY;
        public double PointY
        {
            get { return _pointY; }
            set { SetProperty(ref _pointY, value); }
        }

        private ImageSource _imgSource;
        public ImageSource ImgSource
        {
            get { return _imgSource; }
            set => SetProperty(ref _imgSource, value);
        }

        private double _imgWidth = 0;
        public double ImgWidth
        {
            get { return _imgWidth; }
            set => SetProperty(ref _imgWidth, value);
        }

        private double _imgHeight = 0;
        public double ImgHeight
        {
            get { return _imgHeight; }
            set => SetProperty(ref _imgHeight, value);
        }
        public Bitmap Bmp { get; set; }

        private int _formatSelectedIndex;
        public int FormatSelectedIndex
        {
            get { return _formatSelectedIndex; }
            set { SetProperty(ref _formatSelectedIndex, value); }
        }

        private int _simSelectedIndex;
        public int SimSelectedIndex
        {
            get { return _simSelectedIndex; }
            set { SetProperty(ref _simSelectedIndex, value); }
        }

        private Thickness _selectRectangleMargin;
        public Thickness SelectRectangleMargin
        {
            get { return _selectRectangleMargin; }
            set { SetProperty(ref _selectRectangleMargin, value); }
        }

        private Visibility _selectRectangleVisibility;
        public Visibility SelectRectangleVisibility
        {
            get { return _selectRectangleVisibility; }
            set { SetProperty(ref _selectRectangleVisibility, value); }
        }

        private double _selectRectangleWidth;
        public double SelectRectangleWidth
        {
            get { return _selectRectangleWidth; }
            set { SetProperty(ref _selectRectangleWidth, value); }
        }

        private double _selectRectangleHeight;
        public double SelectRectangleHeight
        {
            get { return _selectRectangleHeight; }
            set { SetProperty(ref _selectRectangleHeight, value); }
        }

        private Thickness _findResultMargin;
        public Thickness FindResultMargin
        {
            get { return _findResultMargin; }
            set { SetProperty(ref _findResultMargin, value); }
        }

        private Visibility _findResultVisibility;
        public Visibility FindResultVisibility
        {
            get { return _findResultVisibility; }
            set { SetProperty(ref _findResultVisibility, value); }
        }

        private int _colorInfoSelect;
        public int ColorInfoSelect
        {
            get { return _colorInfoSelect; }
            set { SetProperty(ref _colorInfoSelect, value); }
        }

        public IEnumerable<string> AnchorsValue => new[] { "L", "C", "R" };

        public MainWindowViewModel()
        {
            EmulatorOptionsHint = "模拟器";
            LoupeVisibility = Visibility.Hidden;
            SelectRectangleVisibility = Visibility.Hidden;
            FindResultVisibility = Visibility.Hidden;
            LoupeWriteBmp = LoupeWriteBitmap.Init(241, 241);
            EmulatorInfo = MyEmulator.Init();
            ColorInfos = new ObservableCollection<ColorInfo>();

        }

        public ICommand Clear_Click => new DelegateCommand(() =>
                 {
                     if (CreateStr == string.Empty)
                     {
                         if (ColorInfos.Count == 0)
                         {
                             if (MyEmulator.IsInit == 2)
                             {
                                 FormatSelectedIndex = -1;
                                 MyEmulator.Dispose();
                                 EmulatorInfo.Clear();
                                 EmulatorInfo = MyEmulator.Init();
                             }
                         }
                         else
                         {
                             ColorInfos.Clear();
                         }
                     }
                     else
                     {
                         CreateStr = string.Empty;
                         Range = string.Empty;
                         TestResult = string.Empty;
                     }
                 });
        public ICommand ClearData_Click => new DelegateCommand(() =>
        {
            if (ColorInfos.Count != 0)
            {
                ColorInfos.Clear();
            }

        });
        public ICommand Emulator_SelectionChanged => new DelegateCommand<MainWindow>((e) =>
                 {
                     if (MyEmulator.IsInit == 2)
                     {
                         MyEmulator.Index = e.EmulatorOptions.SelectedIndex;
                     }
                     else if (MyEmulator.IsInit == 0)
                     {
                         if (EmulatorInfo.Count != 0)
                         {
                             if (e.EmulatorOptions.SelectedIndex == EmulatorInfo.Count - 1)
                             {
                                 EmulatorOptionsHint = "tcp连接";
                             }
                             else
                             {
                                 EmulatorOptionsHint = "模拟器";
                             }
                         }

                         e.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                         {
                             e.Cursor = Cursors.Wait;
                             MyEmulator.Changed(e.EmulatorOptions.SelectedIndex);
                             EmulatorInfo = MyEmulator.GetAll();
                             e.EmulatorOptions.SelectedIndex = -1;
                             e.Cursor = Cursors.Arrow;
                         }));

                     }
                     else if (MyEmulator.IsInit == 1)
                     {
                         MyEmulator.IsInit = 2;
                     }

                 });
        public ICommand ScreenShot_Click => new DelegateCommand<MainWindow>((e) =>
        {
            e.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                e.ScreenShot.Cursor = Cursors.Wait;
                e.Cursor = Cursors.Wait;
                if (MyEmulator.Select == -1 || MyEmulator.Index == -1)
                {
                    MessageBox.Show("请先选择模拟器");
                    e.Cursor = Cursors.Arrow;
                    return;
                }
                Bitmap bmp = MyEmulator.ScreenShot();
                if (bmp.Width != 1)
                {
                    Bmp = bmp;
                    ImgHeight = Bmp.Height;
                    ImgWidth = Bmp.Width;
                    ImgSource = Bitmap2BitmapImage(Bmp);
                    GraphicHelper.KeepScreen(Bmp);
                }
                e.ScreenShot.Cursor = Cursors.Arrow;
                e.Cursor = Cursors.Arrow;
            }));
        });
        public ICommand Save_Click => new DelegateCommand(() =>
                 {
                     if (Bmp != null)
                     {
                         SaveFileDialog saveFileDialog = new SaveFileDialog
                         {
                             Filter = "png files   (*.png)|*.png",
                             FilterIndex = 2,
                             RestoreDirectory = true
                         };
                         if (saveFileDialog.ShowDialog() == true)
                         {
                             string savefire = saveFileDialog.FileName.ToString();
                             Bmp.Save(savefire);
                         }
                     }
                 });
        public ICommand TurnRight_Click => new DelegateCommand(() =>
                 {
                     if (Bmp != null)
                     {
                         Bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                         ImgHeight = Bmp.Height;
                         ImgWidth = Bmp.Width;
                         ImgSource = Bitmap2BitmapImage(Bmp);
                         GraphicHelper.KeepScreen(Bmp);
                     }
                 });
        public ICommand Test_Click => new DelegateCommand<MainWindow>((e) =>
                 {
                     if (Bmp != null && ColorInfos.Count > 0)
                     {
                         byte[] sim = new byte[] { 100, 95, 90, 85, 80 };
                         if (FormatSelectedIndex == 0)
                         {
                             string str = CreateColorStrHelper.Create(0, ColorInfos);
                             TestResult = GraphicHelper.CompareColor(str.Trim('"'), sim[SimSelectedIndex]).ToString();
                         }
                         else if (FormatSelectedIndex == 8)
                         {
                             double width = ColorInfos[0].Width;
                             double height = ColorInfos[1].Height;
                             string str = CreateColorStrHelper.Create(9, ColorInfos);
                             TestResult = GraphicHelper.AnchorsCompareColor(width, height, str.Trim('"'), sim[SimSelectedIndex]).ToString();
                         }
                         else
                         {
                             Range rect = GetRange();
                             string str = CreateColorStrHelper.Create(1, ColorInfos, rect);
                             string[] strArray = str.Split("\",\"");
                             if (strArray[1].Length <= 3)
                             {
                                 MessageBox.Show("多点找色至少需要勾选两个颜色才可进行测试!", "错误");
                                 TestResult = "error";
                                 return;
                             }
                             string[] _ = strArray[0].Split(",\"");
                             System.Drawing.Point result = GraphicHelper.FindMultiColor((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom, _[1].Trim('"'), strArray[1].Trim('"'), sim[SimSelectedIndex]);
                             if (result.X >= 0 && result.Y >= 0)
                             {
                                 Point point = e.Img.TranslatePoint(new Point(result.X, result.Y), e);
                                 FindResultMargin = new Thickness(point.X - 14, point.Y - 38, 0, 0);
                                 FindResultVisibility = Visibility.Visible;
                             }
                             TestResult = result.ToString();
                         }
                     }
                 });
        public ICommand Create_Click => new DelegateCommand(() =>
                 {
                     if (ColorInfos.Count > 0)
                     {
                         Range rect = GetRange();
                         if (Range != null)
                         {
                             if (Range.IndexOf("m") != -1)
                             {
                                 Range = "m," + rect.Left.ToString() + "," + rect.Top.ToString() + "," + rect.Right.ToString() + "," + rect.Bottom.ToString();
                             }
                             else
                             {
                                 Range = rect.Left.ToString() + "," + rect.Top.ToString() + "," + rect.Right.ToString() + "," + rect.Bottom.ToString();
                             }
                         }
                         else
                         {
                             Range = rect.Left.ToString() + "," + rect.Top.ToString() + "," + rect.Right.ToString() + "," + rect.Bottom.ToString();
                         }
                         CreateStr = CreateColorStrHelper.Create(FormatSelectedIndex, ColorInfos, rect);
                     }
                 });
        public ICommand Format_SelectionChanged => new DelegateCommand<MainWindow>((e) =>
        {
            if (FormatSelectedIndex == 8)
            {
                e.TheAnchors.Visibility = Visibility.Visible;
                ColorInfos.Clear();
            }
            else
            {
                if (e.TheAnchors.Visibility != Visibility.Collapsed)
                {
                    e.TheAnchors.Visibility = Visibility.Collapsed;
                }
            }
        });
        public Range GetRange()
        {
            if (Range != null)
            {
                if (Range.IndexOf("m") != -1)
                {
                    string[] range = Range.Split(',');
                    return new Range(int.Parse(range[1].Trim()), int.Parse(range[2].Trim()), int.Parse(range[3].Trim()), int.Parse(range[4].Trim()));
                }
            }
            double imgWidth = ImgWidth - 1;
            double imgHeight = ImgHeight - 1;
            double left = ImgWidth;
            double top = ImgHeight;
            double right = 0;
            double bottom = 0;
            foreach (ColorInfo colorInfo in ColorInfos)
            {
                if (colorInfo.IsChecked)
                {
                    if (colorInfo.ThePoint.X < left)
                    {
                        left = colorInfo.ThePoint.X;
                    }
                    if (colorInfo.ThePoint.X > right)
                    {
                        right = colorInfo.ThePoint.X;
                    }
                    if (colorInfo.ThePoint.Y < top)
                    {
                        top = colorInfo.ThePoint.Y;
                    }
                    if (colorInfo.ThePoint.Y > bottom)
                    {
                        bottom = colorInfo.ThePoint.Y;
                    }
                }
            }
            return new Range(left >= 25 ? left - 25 : 0, top >= 25 ? top - 25 : 0, right + 25 > imgWidth ? imgWidth : right + 25, bottom + 25 > imgHeight ? imgHeight : bottom + 25);
        }

        private Point StartPoint { get; set; }
        private Point EndPoint { get; set; }

        public ICommand Img_MouseDown => new DelegateCommand<MainWindow>((e) =>
        {
            e.Img.CaptureMouse();
            LoupeVisibility = Visibility.Collapsed;
            SelectRectangleVisibility = Visibility.Visible;
            Point mousePoint = Mouse.GetPosition(e);
            SelectRectangleMargin = new Thickness(mousePoint.X - 1, mousePoint.Y - 1, 0, 0);
            SelectRectangleWidth = 0;
            SelectRectangleHeight = 0;
            StartPoint = Mouse.GetPosition(e.Img);
            EndPoint = StartPoint;
        });
        public ICommand Img_MouseMove => new DelegateCommand<MainWindow>((e) =>
        {
            if (SelectRectangleVisibility == Visibility.Visible)
            {
                EndPoint = Mouse.GetPosition(e.Img);
                if (EndPoint.X >= ImgWidth || EndPoint.Y >= ImgHeight)
                {
                    SelectRectangleVisibility = Visibility.Collapsed;
                    return;
                }
                double width = EndPoint.X - StartPoint.X - 1;
                double height = EndPoint.Y - StartPoint.Y - 1;
                if (width > 0 && height > 0)
                {
                    SelectRectangleWidth = width;
                    SelectRectangleHeight = height;
                }
                return;
            }
            Point mousePoint = Mouse.GetPosition(e);
            if (mousePoint.Y >= e.ActualHeight - 350)
                LoupeMargin = new Thickness(mousePoint.X + 20, mousePoint.Y - 261, 0, 0);
            else
                LoupeMargin = new Thickness(mousePoint.X + 20, mousePoint.Y + 20, 0, 0);
            Point imgPoint = Mouse.GetPosition(e.Img);
            PointX = Math.Floor(imgPoint.X);
            PointY = Math.Floor(imgPoint.Y);
            imgPoint.X = PointX - 7;
            imgPoint.Y = PointY - 7;
            List<byte[]> colors = new List<byte[]>();
            for (int j = 0; j < 15; j++)
            {
                for (int i = 0; i < 15; i++)
                {
                    int x = (int)imgPoint.X + i;
                    int y = (int)imgPoint.Y + j;

                    if (x >= 0 && y >= 0 && x < Bmp.Width && y < Bmp.Height)
                    {
                        colors.Add(GraphicHelper.GetPixel(x, y));
                    }
                    else
                    {
                        colors.Add(new byte[] { 0, 0, 0 });
                    }
                }
            }
            LoupeWriteBmp.WriteColor(colors);
        });
        public ICommand Img_MouseLeftButtonUp => new DelegateCommand<System.Windows.Controls.Image>((e) =>
        {
            e.ReleaseMouseCapture();
            if (SelectRectangleVisibility == Visibility.Visible && EndPoint.X >= StartPoint.X + 50 && EndPoint.Y >= StartPoint.Y + 50)
            {
                Range = "m," + StartPoint.X.ToString() + "," + StartPoint.Y.ToString() + "," + EndPoint.X.ToString() + "," + EndPoint.Y.ToString();
                SelectRectangleVisibility = Visibility.Collapsed;
                LoupeVisibility = Visibility.Visible;
                return;
            }
            else if (SelectRectangleVisibility == Visibility.Visible)
            {
                SelectRectangleVisibility = Visibility.Collapsed;
                LoupeVisibility = Visibility.Visible;
            }
            byte[] bytes = GraphicHelper.GetPixel((int)PointX, (int)PointY);
            Color color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);
            if (FormatSelectedIndex != 8)
            {
                ColorInfos.Add(new ColorInfo(ColorInfos.Count, new Point(PointX, PointY), color));
            }
            else
            {
                string anchors = string.Empty;
                double _ = ImgWidth / 4;

                if (PointX > _ * 3)
                    anchors = "R";
                else if (PointX > _)
                    anchors = "C";
                else if (PointX < _)
                    anchors = "L";

                ColorInfos.Add(new ColorInfo(ColorInfos.Count, anchors, new Point(PointX, PointY), color, ImgWidth, ImgHeight));

            }
        });
        public ICommand Img_MouseEnter => new DelegateCommand(() =>
        {
            if (SelectRectangleVisibility != Visibility.Visible)
            {
                LoupeVisibility = Visibility.Visible;
            }
        });
        public ICommand Img_MouseLeave => new DelegateCommand<MainWindow>((e) =>
        {
            LoupeVisibility = Visibility.Collapsed;
        });
        public ICommand Open_Click => new DelegateCommand(() =>
                 {
                     OpenFileDialog fileDialog = new OpenFileDialog
                     {
                         RestoreDirectory = true,
                         Multiselect = false,
                         Title = "请选择文件",
                         Filter = "位图文件 |*.jpg;*.png;*.bmp"
                     };
                     if (fileDialog.ShowDialog() == true)
                     {
                         string FileName = fileDialog.FileName;
                         FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                         Bmp = (Bitmap)System.Drawing.Image.FromStream(fileStream);
                         ImgHeight = Bmp.Height;
                         ImgWidth = Bmp.Width;
                         ImgSource = Bitmap2BitmapImage(Bmp);
                         fileStream.Close();
                         fileStream.Dispose();
                         GraphicHelper.KeepScreen(Bmp);
                     }
                 });

        public ICommand Offset_Click => new DelegateCommand(() =>
        {
            if (ColorInfos.Count>0)
            {
                if (ColorInfoSelect != -1)
                {
                    Offset offset = new Offset();
                    if ((bool)offset.ShowDialog())
                    {
                        ColorInfos[ColorInfoSelect].OffsetColor = offset.Result;
                    }
                }
            }
        });
        public ICommand AllOffset_Click => new DelegateCommand(() =>
        {
            Offset offset = new Offset();
            if ((bool)offset.ShowDialog())
            {
                for (int i = 0; i < ColorInfos.Count; i++)
                {
                    if (ColorInfos[i].OffsetColor == "000000")
                    {
                        ColorInfos[i].OffsetColor = offset.Result;
                    }
                }
                ColorInfo.AllOffsetColor = offset.Result;
            }
        });
        public ICommand AllOffsetClear_Click => new DelegateCommand(() =>
        {
            for (int i = 0; i < ColorInfos.Count; i++)
            {
                if (ColorInfos[i].OffsetColor == ColorInfo.AllOffsetColor)
                {
                    ColorInfos[i].OffsetColor = "000000";
                }
            }
            ColorInfo.AllOffsetColor = "000000";
        });
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        private ImageSource Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hBitmap);
            return imageSource;
        }
    }
}
