using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp_10_nav
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public Action<object, object> LoginSuccess { get; internal set; }

        public LoginWindow()
        {
            InitializeComponent();
            Loaded += LoginWindow_Loaded;//事件源.事件名称 += 事件处理方法  当事件加载完成之后调用方法
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置焦点到用户名输入框
            UsernameTextBox.Focus();
        }

        

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateLogin())
            {
                // 登录成功，打开主窗口
                //OpenMainWindow();
                LoginSuccess?.Invoke(sender,e);
                //this.Close();
            }
            else
            {
                MessageBox.Show("登录失败，请检查用户名和密码", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBox.Clear();
                PasswordBox.Focus();
            }
        }

        private bool ValidateLogin()
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // 简单的验证逻辑 - 实际应用中应该更复杂
            MessageBox.Show($"登录成功！用户名：{username},密码：{password}");
            return true;
        }

        private void OpenMainWindow()
        {
            // 创建并显示主窗口
            MainWindow mainWindow = new MainWindow();

            // 可以传递用户信息给主窗口
            // mainWindow.CurrentUser = UsernameTextBox.Text.Trim();

            mainWindow.Show();

            //// 可选：设置主窗口位置
            //mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // 支持回车键登录
        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PasswordBox.Focus();
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}
