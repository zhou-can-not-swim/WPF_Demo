using System.Windows;
using System.Windows.Controls;
using WpfD.Model;
using WpfD.ViewModel;

namespace WpfD.ViewPage
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            //初始化组件
            InitializeComponent();

            ClassVModel mainViewModel = new ClassVModel();
            this.DataContext = mainViewModel;
        }





    }
}

