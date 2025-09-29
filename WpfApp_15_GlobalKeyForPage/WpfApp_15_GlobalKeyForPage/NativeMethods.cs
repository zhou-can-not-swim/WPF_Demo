﻿using System.Runtime.InteropServices;

namespace WpfApp_15_GlobalKeyForPage
{
    public static class NativeMethods
    {
        public const int WM_HOTKEY = 0x0312;
        public const int MOD_NONE = 0x0000;
        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int MOD_SHIFT = 0x0004;
        public const int MOD_WIN = 0x0008;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }

}
