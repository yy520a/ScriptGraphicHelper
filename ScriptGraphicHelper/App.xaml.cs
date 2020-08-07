using Prism.Ioc;
using 综合图色助手.Views;
using System.Windows;
using System;

namespace 综合图色助手
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Exception exception = e.Exception;
            MessageBox.Show("程序即将退出! \r\n\r\n" + exception.ToString(), "捕获到未处理的异常 , 请将本提示截图给开发者");
            Current.Shutdown();

        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            MessageBox.Show("程序即将退出! \r\n\r\n" + exception.ToString(), "捕获到未处理的异常 , 请将本提示截图给开发者");
            Current.Shutdown();
        }
    }
}
