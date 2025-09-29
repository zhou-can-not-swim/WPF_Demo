using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WpfApp_14_GlobalKey
{
    public class HotkeyManager
    {
        private const int HOTKEY_ID = 9000;
        private IntPtr _hwnd;
        private HwndSource _source;
        public event Action? HotkeyPressed;

        public bool RegisterHotkey(Window window, int modifiers, int key)
        {
            // 1) 把 WPF 窗口转成 Win32 句柄
            _hwnd = new WindowInteropHelper(window).Handle;
            // 2) 拿到消息泵，准备挂钩子
            _source = HwndSource.FromHwnd(_hwnd);
            _source?.AddHook(HwndHook); // 挂的是下面那个 HwndHook 方法

            // 3) 真正调用 Win32 API 注册全局热键
            //“当用户按下 modifiers+key 时，请发一条 0x0312 消息到 _hwnd，并把 wParam 设成 9000。”
            bool ok = NativeMethods.RegisterHotKey(_hwnd, HOTKEY_ID, modifiers, key);
            if (!ok)
            {
                // ← 这里告诉你为什么失败
                int err = Marshal.GetLastWin32Error();
                MessageBox.Show(
                    $"注册失败！错误码 {err}\n" +
                    "常见原因：\n" +
                    "1. 快捷键已被其它程序占用（QQ、微信、网易云…）\n" +
                    "2. 传进来的 modifiers/key 为 0\n" +
                    "3. 句柄为 0（窗口还没加载完）",
                    "RegisterHotKey");
            }
            return ok;
        }

        public void UnregisterHotkey()
        {
            // 1) 如果句柄有效，就告诉操作系统“把 9000 号热键注销掉”
            if (_hwnd != IntPtr.Zero)
                NativeMethods.UnregisterHotKey(_hwnd, HOTKEY_ID);
            // 2) 把之前挂的钩子摘掉，防止内存泄漏
            _source?.RemoveHook(HwndHook);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // 只有当消息是 WM_HOTKEY 并且 wParam 等于我们之前申请的 9000 号时 
            if (msg == NativeMethods.WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                HotkeyPressed?.Invoke(); // 广播事件
                handled = true;          // 告诉 WPF“这条消息我处理掉了”
            }
            return IntPtr.Zero;          // 没处理的消息继续往下传
        }
    }


    //HotkeyManager 把“Win32 全局热键”包装成“WPF 友好事件”：
    //调用 RegisterHotkey → 系统级热键生效；
    //按下热键 → HwndHook 收到 WM_HOTKEY → 触发 HotkeyPressed 事件；
    //调用 UnregisterHotkey → 热键被释放，钩子被移除。
}