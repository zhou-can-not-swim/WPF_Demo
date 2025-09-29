using System.Diagnostics;
using System.Windows;
using WpfApp_15_GlobalKeyForPage.Pages;

namespace WpfApp_15_GlobalKeyForPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotkeyManager _hotkeyManager = new HotkeyManager();
        private bool _isVisible = true;   // 记录当前状态
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Page1());
            this.SourceInitialized += (_, _) => LoadHotkey();


        }
        private void LoadHotkey()
        {
            //存放“组合键”(Ctrl/Alt/Shift/Win)的位掩码；
            int m = Properties.Settings.Default.Modifiers;
            //存放真正的字母/数字虚拟键码。
            int k = Properties.Settings.Default.Key;

            // ← 看看到底读出来的是多少
            Debug.WriteLine($"[LoadHotkey] modifiers={m} key={k}");

            //没有设置快捷键就不注册了
            if (m == 0 || k == 0)
            {
                MessageBox.Show("还没有设置快捷键，先去“设置”里配一个！");
                return;
            }

            // 先清掉上一次（如果有）
            _hotkeyManager.HotkeyPressed -= OnHotkeyPressed;
            _hotkeyManager.UnregisterHotkey();

            // 再注册
            _hotkeyManager.RegisterHotkey(this, m, k);
            _hotkeyManager.HotkeyPressed += OnHotkeyPressed;
        }

        //热键按下时,“无论这个demo藏在哪，按下热键就把我瞬间叫到最前面，并抢过键盘焦点。”
        private void OnHotkeyPressed()
        {
            _isVisible = !_isVisible;     // 翻转状态

            if (_isVisible)
            {
                this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
            this.Topmost = true;//临时把窗口置顶（放在所有非置顶窗口之上）。
            this.Topmost = false;//立即取消置顶，恢复普通层级。
            }
            else
            {
                // 要么隐藏，要么最小化，二选一
                //this.Hide();                          // 彻底从任务栏消失
                // 如果想“最小化到任务栏”就用下面两行：
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = true;
            }
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
