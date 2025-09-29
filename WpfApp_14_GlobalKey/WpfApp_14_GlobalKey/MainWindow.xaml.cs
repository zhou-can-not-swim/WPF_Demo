using System.Diagnostics;
using System.Windows;

namespace WpfApp_14_GlobalKey
{
    public partial class MainWindow : Window
    {
        private HotkeyManager _hotkeyManager = new HotkeyManager();

        public MainWindow()
        {
            InitializeComponent();
            // 不要在这里调 LoadHotkey(),这句话意味着当窗口调出来后，再去注册热键
            this.SourceInitialized += (_, _) => LoadHotkey();
        }

        //热键按下时,“无论这个demo藏在哪，按下热键就把我瞬间叫到最前面，并抢过键盘焦点。”
        private void OnHotkeyPressed()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
            this.Topmost = true;//临时把窗口置顶（放在所有非置顶窗口之上）。
            this.Topmost = false;//立即取消置顶，恢复普通层级。
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
            _hotkeyManager.UnregisterHotkey();
            LoadHotkey();
        }

        protected override void OnClosed(EventArgs e)
        {
            _hotkeyManager.HotkeyPressed -= OnHotkeyPressed; // 可选，好习惯
            _hotkeyManager.UnregisterHotkey();
            base.OnClosed(e);
        }

        //
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
    }
}
