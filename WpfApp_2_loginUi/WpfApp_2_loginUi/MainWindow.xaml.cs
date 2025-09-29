using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_2_loginUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        LoginVM loginVm;
        public MainWindow()
        {
            InitializeComponent();
            loginVm = new LoginVM();

            this.DataContext = loginVm;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //string username = txtUserName.Text;
            //string password = txtPwd.Text;

            if (loginVm.LoginM.Username == "admin" && loginVm.LoginM.Password == "admin")
            {
                MessageBox.Show("登录成功！");
                Index index = new Index();
                index.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
                loginVm.LoginM.Username = "";
                loginVm.LoginM.Password = "";
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.wpfsoft.com");
        }
    }
}