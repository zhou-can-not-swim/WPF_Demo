using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_3_ProgressBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        Dispatcher.Invoke(() => {

                            _TextBlock.Text = $"{i}%";
                            _ProgressBar.Value = i;
                        });

                        Task.Delay(25).Wait();
                    }
                });
            };

        }
    }
}