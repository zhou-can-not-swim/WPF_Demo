using System.Text;
using System.Windows;
using WpfApp_11_page.ViewPage;
namespace WpfApp_11_page
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Page1());
        }

        private void NavigateToPage1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1());
        }

        private void NavigateToPage2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page2());
        }

        private void NavigateToPage3(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page3());
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }

        private void GoForward(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoForward)
                MainFrame.GoForward();
        }
    }
}