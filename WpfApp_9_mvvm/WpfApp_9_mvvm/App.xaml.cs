using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfApp_9_mvvm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 初始化数据库
            UserDatabaseHelper.InitializeDatabase();
        }
    }

}
