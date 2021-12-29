using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ThePen
{
    public static class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void Start()
        {
            _hookID = SetHook(_proc);
            //Application.Run();
        }

        public static void Stop()
	    {
            //UnhookWindowsHookEx(_hookID);
        }

        static Dictionary<uint, Action> hotkeys = new();
        public static void Hook(List<(uint, uint, Action)> hotkeys)
		{
            foreach (var hotkey in hotkeys)
			{
				KeyboardHook.hotkeys[hotkey.Item2] = hotkey.Item3;
			}
		}

        public static void Unhook()
		{
            hotkeys = new();
		}

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                uint vkCode = (uint)Marshal.ReadInt32(lParam);
                Action handler = null;
                hotkeys.TryGetValue(vkCode, out handler);
                if (handler != null)
				{
                    handler.Invoke();
                    Debug.WriteLine("HOTKEY!!");
                    Debug.WriteLine((Keys)vkCode);
                    return CallNextHookEx((IntPtr)0, -1, (IntPtr)0, (IntPtr)0);
                }
                
                
                Debug.WriteLine((Keys)vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
