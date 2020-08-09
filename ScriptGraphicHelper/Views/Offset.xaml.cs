using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScriptGraphicHelper.Views
{
    /// <summary>
    /// Offset.xaml 的交互逻辑
    /// </summary>
    public partial class Offset : Window
    {
        public string Result { get; set; } = string.Empty;
        public Offset()
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
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (SetText.Text.Trim().Length==6)
            {
                Result = SetText.Text.Trim();
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("偏色字符串长度应为6", "错误");
            }
        }
    }
}
