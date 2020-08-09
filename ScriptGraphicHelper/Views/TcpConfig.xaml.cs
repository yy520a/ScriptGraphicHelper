using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Media;

namespace ScriptGraphicHelper.Views
{
    /// <summary>
    /// TcpInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TcpConfig : Window
    {
        public string MyAddress { get; set; } = string.Empty;
        public int MyPort { get; set; } = -1;
        public TcpConfig()
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
            if (Address.Text != string.Empty && Port.Text != string.Empty)
            {
                MyAddress = Address.Text.Trim();
                MyPort =int.Parse(Port.Text.Trim());
                this.DialogResult = true;
            }
        }
    }
}
