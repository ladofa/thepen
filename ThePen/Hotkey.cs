using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ThePen
{
	/// <summary>
	/// System wide hot keys (RegisterHotKey) delivered to the main window.
	/// </summary>
	public static class Hotkey
	{
		//fsModifiers of RegisterHotKey
		public const uint MOD_NONE = 0x0000;
		public const uint MOD_ALT = 0x0001;
		public const uint MOD_CTRL = 0x0002;
		public const uint MOD_SHIFT = 0x0004;
		public const uint MOD_WIN = 0x0008;
		public const uint MOD_NOREPEAT = 0x4000;

		//display name -> virtual key code
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
			{"~",0xC0},
			{"-",0xBD},
			{"+",0xBB},
			{"[",0xDB},
			{"]",0xDD},
			{"\\",0xDC},
			{";",0xBA},
			{"\"",0xDE},
			{"<",0xBC},
			{">",0xBE},
			{"/",0xBF},
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

			{"Browser Back", 0xa6 },
			{"Browser Forward", 0xa7 },
			{"Browser Refresh", 0xa8 },
			{"Browser Stop", 0xa9 },
			{"Browser Search", 0xaa },
			{"Browser Favorite", 0xab },
			{"Browser Home", 0xac },
			{"Volume Mute", 0xad },
			{"Volume Down", 0xae },
			{"Volume Up", 0xaf },
			{"Media Next Track", 0xb0 },
			{"Media Prev Track", 0xb1 },
			{"Media Stop", 0xb2 },
			{"Media Play Pause", 0xb3 },
			{"Launch Mail", 0xb4 },
			{"Launch Media Select", 0xb5 },
			{"Launch App1", 0xb6 },
			{"Launch App2", 0xb7 },
		};

		public static Dictionary<uint, string> TrigKeysInv = new();
		public static List<string> TrigKeyStrings = new();
		public static List<string> OneKeyStrings = new();

		static Hotkey()
		{
			foreach (var (name, code) in TrigKeys)
			{
				TrigKeysInv[code] = name;
				TrigKeyStrings.Add(name);
			}

			foreach (System.Windows.Input.Key key in Enum.GetValues(typeof(System.Windows.Input.Key)))
			{
				OneKeyStrings.Add(key.ToString());
			}
		}

		const int WM_HOTKEY = 0x0312;
		const int FirstId = 9030;

		//hot key id -> what to do
		static readonly Dictionary<int, Action> actions = new();
		static HwndSource source;

		/// <summary>
		/// Registers the hot keys (modifiers, virtual key, action) on the window.
		/// The previous ones are released first.
		/// </summary>
		public static void Hook(Window window, List<(uint, uint, Action)> hotkeys)
		{
			Unhook();

			source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
			if (source == null) return;

			source.AddHook(WindowMessage);

			int id = FirstId;
			foreach (var (modifiers, key, action) in hotkeys)
			{
				//fails when another program already owns the combination
				if (RegisterHotKey(source.Handle, id, modifiers | MOD_NOREPEAT, key))
				{
					actions[id] = action;
				}
				id++;
			}
		}

		public static void Unhook()
		{
			if (source == null) return;

			foreach (int id in actions.Keys)
			{
				UnregisterHotKey(source.Handle, id);
			}
			actions.Clear();

			source.RemoveHook(WindowMessage);
			source = null;
		}

		private static IntPtr WindowMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WM_HOTKEY && actions.TryGetValue(wParam.ToInt32(), out Action action))
			{
				action();
				handled = true;
			}
			return IntPtr.Zero;
		}

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
	}
}
