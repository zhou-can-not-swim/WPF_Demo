using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WpfD.Utils
{
    public class GlobalHotkeyManager
    {
        #region Windows API
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(nint hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(nint hWnd, int id);

        [DllImport("kernel32.dll")]
        private static extern uint GlobalAddAtom(string lpString);

        [DllImport("kernel32.dll")]
        private static extern uint GlobalDeleteAtom(uint nAtom);

        private const int WM_HOTKEY = 0x0312;

        [Flags]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }
        #endregion

        private static GlobalHotkeyManager _instance;
        public static GlobalHotkeyManager Instance => _instance ??= new GlobalHotkeyManager();

        private Dictionary<int, HotkeyInfo> _registeredHotkeys;
        private nint _windowHandle;
        private HwndSource _hwndSource;
        private int _nextHotkeyId = 1000;

        public event Action<string> HotkeyPressed;

        private GlobalHotkeyManager()
        {
            _registeredHotkeys = new Dictionary<int, HotkeyInfo>();
        }

        public void Initialize(Window window)
        {
            _windowHandle = new WindowInteropHelper(window).Handle;
            _hwndSource = HwndSource.FromHwnd(_windowHandle);
            _hwndSource.AddHook(HwndHook);
        }

        public bool RegisterHotkey(string name, KeyModifiers modifiers, uint virtualKey)
        {
            try
            {
                int hotkeyId = _nextHotkeyId++;
                uint atomId = GlobalAddAtom($"Hotkey_{hotkeyId}");

                bool success = RegisterHotKey(_windowHandle, (int)atomId, (uint)modifiers, virtualKey);

                if (success)
                {
                    _registeredHotkeys[(int)atomId] = new HotkeyInfo
                    {
                        Name = name,
                        Modifiers = modifiers,
                        VirtualKey = virtualKey,
                        AtomId = atomId
                    };
                }

                return success;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"注册热键失败: {ex.Message}");
                return false;
            }
        }

        public bool UnregisterHotkey(string name)
        {
            foreach (var kvp in _registeredHotkeys)
            {
                if (kvp.Value.Name == name)
                {
                    bool success = UnregisterHotKey(_windowHandle, kvp.Key);
                    if (success)
                    {
                        GlobalDeleteAtom(kvp.Value.AtomId);
                        _registeredHotkeys.Remove(kvp.Key);
                    }
                    return success;
                }
            }
            return false;
        }

        public void UnregisterAllHotkeys()
        {
            foreach (var kvp in _registeredHotkeys)
            {
                UnregisterHotKey(_windowHandle, kvp.Key);
                GlobalDeleteAtom(kvp.Value.AtomId);
            }
            _registeredHotkeys.Clear();
        }

        public string GetHotkeyDisplayString(string name)
        {
            foreach (var hotkey in _registeredHotkeys.Values)
            {
                if (hotkey.Name == name)
                {
                    return HotkeyToString(hotkey.Modifiers, hotkey.VirtualKey);
                }
            }
            return "未设置";
        }

        private string HotkeyToString(KeyModifiers modifiers, uint virtualKey)
        {
            List<string> parts = new List<string>();

            if (modifiers.HasFlag(KeyModifiers.Control)) parts.Add("Ctrl");
            if (modifiers.HasFlag(KeyModifiers.Shift)) parts.Add("Shift");
            if (modifiers.HasFlag(KeyModifiers.Alt)) parts.Add("Alt");
            if (modifiers.HasFlag(KeyModifiers.Win)) parts.Add("Win");

            string keyName = VirtualKeyToChar(virtualKey);
            parts.Add(keyName);

            return string.Join("+", parts);
        }

        private string VirtualKeyToChar(uint virtualKey)
        {
            if (virtualKey >= 0x30 && virtualKey <= 0x39) // 0-9
                return ((char)virtualKey).ToString();
            if (virtualKey >= 0x41 && virtualKey <= 0x5A) // A-Z
                return ((char)virtualKey).ToString();
            if (virtualKey >= 0x70 && virtualKey <= 0x7B) // F1-F12
                return $"F{virtualKey - 0x6F}";

            return virtualKey switch
            {
                0x20 => "Space",
                0x0D => "Enter",
                0x1B => "Esc",
                0x08 => "Backspace",
                0x09 => "Tab",
                _ => $"Key_{virtualKey}"
            };
        }

        private nint HwndHook(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                int hotkeyId = wParam.ToInt32();
                if (_registeredHotkeys.ContainsKey(hotkeyId))
                {
                    HotkeyPressed?.Invoke(_registeredHotkeys[hotkeyId].Name);
                    handled = true;
                }
            }
            return nint.Zero;
        }

        private class HotkeyInfo
        {
            public string Name { get; set; }
            public KeyModifiers Modifiers { get; set; }
            public uint VirtualKey { get; set; }
            public uint AtomId { get; set; }
        }
    }
}