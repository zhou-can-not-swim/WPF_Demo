using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfApp_10_nav
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);

            //try
            //{
            //    // 显示登录窗口
            //    LoginWindow loginWindow = new LoginWindow();

            //    // 显示为模态对话框，等待登录完成
            //    bool? loginResult = loginWindow.ShowDialog();

            //    //// 如果登录窗口被直接关闭（没有点击登录），则退出应用
            //    //if (loginResult != true)
            //    //{
            //    //    Shutdown();
            //    //    return;
            //    //}

            //    // 登录成功，应用继续运行，主窗口已经在登录成功后打开
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"应用程序启动失败: {ex.Message}", "错误",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    Shutdown();
            //}

            base.OnStartup(e);

            // 显示登录窗口
            LoginWindow loginWindow = new LoginWindow();

            // 订阅登录成功事件
            loginWindow.LoginSuccess += (sender, args) =>
            {
                // 登录成功后创建主窗口
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            };

            // 显示登录窗口
            loginWindow.ShowDialog();

            // 检查是否还有其他窗口打开，如果没有则退出应用
            if (Current.Windows.Count == 0)
            {
                Shutdown();
            }
        }
    }

}
