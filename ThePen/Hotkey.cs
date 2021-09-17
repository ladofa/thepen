//thanks to
//https://social.technet.microsoft.com/wiki/contents/articles/30568.wpf-implementing-global-hot-keys.aspx

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ThePen
{
	public static class Hotkey
	{
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		private const int HOTKEY_ID = 9030;

		//Modifiers:
		public const uint MOD_NONE = 0x0000; //(none)
		public const uint MOD_ALT = 0x0001; //ALT
		public const uint MOD_CTRL = 0x0002; //CTRL
		public const uint MOD_SHIFT = 0x0004; //SHIFT
		public const uint MOD_WIN = 0x0008; //WINDOWS
											 //CAPS LOCK:
		private const uint VK_CAPITAL = 0x32;


		//https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
		public static Dictionary<string, uint> TrigKeys = new Dictionary<string, uint>
		{
			{"None", 0x00 },
			{"ESC",0x1B},
			{"Backspace",0x08},
			{"Tab",0x09},
			{"Enter",0x0d},
			{"Space",0x20},
			{"PageUp",0x21},
			{"PageDown",0x22},
			{"Home",0x24},
			{"End",0x23},
			{"Insert",0x2D},
			{"Delete",0x2E},
			{"LeftArrow",0x25},
			{"UpArrow",0x26},
			{"RightArrow",0x27},
			{"DownArrow",0x28},
			{"0",0x30},
			{"1",0x31},
			{"2",0x32},
			{"3",0x33},
			{"4",0x34},
			{"5",0x35},
			{"6",0x36},
			{"7",0x37},
			{"8",0x38},
			{"9",0x39},
			{"A",0x41},
			{"B",0x42},
			{"C",0x43},
			{"D",0x44},
			{"E",0x45},
			{"F",0x46},
			{"G",0x47},
			{"H",0x48},
			{"I",0x49},
			{"J",0x4a},
			{"K",0x4b},
			{"L",0x4c},
			{"M",0x4d},
			{"N",0x4e},
			{"O",0x4f},
			{"P",0x50},
			{"Q",0x51},
			{"R",0x52},
			{"S",0x53},
			{"T",0x54},
			{"U",0x55},
			{"V",0x56},
			{"W",0x57},
			{"X",0x58},
			{"Y",0x59},
			{"Z",0x5a},
			{"NumPad0",0x60},
			{"NumPad1",0x61},
			{"NumPad2",0x62},
			{"NumPad3",0x63},
			{"NumPad4",0x64},
			{"NumPad5",0x65},
			{"NumPad6",0x66},
			{"NumPad7",0x67},
			{"NumPad8",0x68},
			{"NumPad9",0x69},
			{"F1",0x70},
			{"F2",0x71},
			{"F3",0x72},
			{"F4",0x73},
			{"F5",0x74},
			{"F6",0x75},
			{"F7",0x76},
			{"F8",0x77},
			{"F9",0x78},
			{"F10",0x79},
			{"F11",0x7a},
			{"F12",0x7b},
			{"Pause",0x13},
			{"CapsLock", 0x14 },
			{"ScrollLock", 0x91},
			{"PrintScreen",0x2c},
			{"NumLock", 0x90 },
		};

		public static Dictionary<uint, string> TrigKeysInv;
		public static List<string> TrigKeyStrings;
		public static List<string> OneKeyStrings;

		static List<(uint, uint, Action)> hotkeys;
		

		static Hotkey()
		{
			TrigKeysInv = new();
			foreach (var pair in TrigKeys)
			{
				TrigKeysInv.Add(pair.Value, pair.Key);
			}

			TrigKeyStrings = new();
			foreach (var item in Hotkey.TrigKeys)
			{
				TrigKeyStrings.Add(item.Key);
			}

			OneKeyStrings = new();
			var enumValues = Enum.GetValues(typeof(System.Windows.Input.Key));
			foreach (System.Windows.Input.Key enumValue in (System.Windows.Input.Key[])enumValues)
			{
				OneKeyStrings.Add(enumValue.ToString());
			}

		}

		private static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			const int WM_HOTKEY = 0x0312;
			if (msg == WM_HOTKEY)
			{
				int index = wParam.ToInt32() - HOTKEY_ID;
				hotkeys[index].Item3();
				handled = true;
			}
			return IntPtr.Zero;
		}

		private static IntPtr _windowHandle;
		private static HwndSource _source;


		public static void hook(Window window, List<(uint, uint, Action)> hotkeys)
		{
			_windowHandle = new WindowInteropHelper(window).Handle;
			_source = HwndSource.FromHwnd(_windowHandle);
			_source.AddHook(HwndHook);

			for (int i = 0; i < hotkeys.Count; i++)
			{
				var hotkey = hotkeys[i];
				RegisterHotKey(_windowHandle, HOTKEY_ID + i, (uint)hotkey.Item1, (uint)hotkey.Item2);
			}

			Hotkey.hotkeys = hotkeys;
		}

		public static void unhook()
		{
			if (_source == null) return;

			_source.RemoveHook(HwndHook);

			for (int i = 0; i < hotkeys.Count; i++)
			{
				UnregisterHotKey(_windowHandle, HOTKEY_ID + i);
			}
		}
	}
}
