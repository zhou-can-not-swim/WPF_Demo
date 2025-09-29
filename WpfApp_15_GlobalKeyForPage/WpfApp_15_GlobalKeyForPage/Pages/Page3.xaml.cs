using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp_15_GlobalKeyForPage.Pages
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page3 : Page
    {
        private int _modifiers = 0;
        private int _key = 0;
        public Page3()
        {
            InitializeComponent();
            this.KeyDown += SettingsWindow_KeyDown;

        }

        private void SettingsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            string key = e.Key.ToString();
            _modifiers = 0;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                _modifiers |= NativeMethods.MOD_CONTROL;
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                _modifiers |= NativeMethods.MOD_ALT;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                _modifiers |= NativeMethods.MOD_SHIFT;

            _key = KeyInterop.VirtualKeyFromKey(e.Key);
            HotkeyTextBox.Text = $"快捷键: {e.Key}";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Modifiers = _modifiers;
            Properties.Settings.Default.Key = _key;
            Properties.Settings.Default.Save();

            MessageBox.Show("重启之后生效");
        }
    }
}