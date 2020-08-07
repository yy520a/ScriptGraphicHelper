using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using 综合图色助手.Models;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace 综合图色助手.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer showTimer = new DispatcherTimer();
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(Theme.Dark);
            theme.SecondaryMid = new ColorPair(Color.FromRgb(0x66, 0x66, 0x66), Colors.White);
            theme.PrimaryMid = new ColorPair(Color.FromRgb(0x66, 0x66, 0x66), Colors.White);
            paletteHelper.SetTheme(theme);
            FocusManager.SetFocusedElement(Img, SelectRectangle);

            showTimer.Tick += new EventHandler(Panel_5_SetVisibility);

        }

        private void CopyPoint_Click(object sender, RoutedEventArgs e)
        {
            if (Color_Info.SelectedIndex != -1)
            {
                IList<ColorInfo> colorInfos = (IList<ColorInfo>)Color_Info.ItemsSource;
                ColorInfo colorInfo = colorInfos[Color_Info.SelectedIndex];
                Clipboard.SetText(colorInfo.PointStr);
            }
        }

        private void CopyColor_Click(object sender, RoutedEventArgs e)
        {
            if (Color_Info.SelectedIndex != -1)
            {
                IList<ColorInfo> colorInfos = (IList<ColorInfo>)Color_Info.ItemsSource;
                ColorInfo colorInfo = colorInfos[Color_Info.SelectedIndex];
                Clipboard.SetText(colorInfo.ColorStr);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Panel_2.MaxWidth = ActualWidth - Panel_1.ActualWidth - Panel_3.ActualWidth - 20;
            Panel_2.MaxHeight = ActualHeight - 30;
            Color_Info.MaxHeight = ActualHeight - 100;
        }

        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            Point point = PointToScreen(Mouse.GetPosition(this));
            if (Panel_4.Visibility == Visibility.Visible)
            {
                if (key == Key.Up)
                {
                    SetCursorPos((int)point.X, (int)point.Y - 1);
                }
                else if (key == Key.Down)
                {
                    SetCursorPos((int)point.X, (int)point.Y + 1);
                }
                else if (key == Key.Left)
                {
                    SetCursorPos((int)point.X - 1, (int)point.Y);
                }
                else if (key == Key.Right)
                {
                    SetCursorPos((int)point.X + 1, (int)point.Y);
                }
                e.Handled = true;
            }

        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ColorString.Text);
        }

        private void Panel_5_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Panel_5.Visibility == Visibility.Visible)
            {
                showTimer.Interval = new TimeSpan(0, 0, 0, 5);
                showTimer.Start();
            }
        }

        private void Panel_5_SetVisibility(object sender, EventArgs e)
        {
            if (Panel_5.Visibility == Visibility.Visible)
            {
                Panel_5.Visibility = Visibility.Hidden;
                showTimer.Stop();
            }
        }
    }
}
