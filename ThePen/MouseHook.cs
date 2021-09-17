using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Diagnostics;


//thanks to https://poorman.tistory.com/297
namespace ThePen
{

	public static class MouseHook
	{
		public delegate void MouseHookEventHandler(object sender, MouseHookEventArgs message);
		public static event MouseHookEventHandler MouseHookEvent;

		public static void Start()
		{
			_hookID = SetHook(_proc);
		}

		public static void stop()
		{
			UnhookWindowsHookEx(_hookID);
		}

		private static LowLevelMouseProc _proc = HookCallback;
		private static IntPtr _hookID = IntPtr.Zero;

		private static IntPtr SetHook(LowLevelMouseProc proc)
		{
			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
			{
					return SetWindowsHookEx(WH_MOUSE_LL, proc,
					GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		private static IntPtr HookCallback(
			int nCode, IntPtr wParam, IntPtr lParam)
		{
			MouseMessages message = (MouseMessages)wParam;
			if (nCode >= 0)
			{
				
				MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				MouseHookEvent?.Invoke(null, new MouseHookEventArgs() { Message = message, Point = hookStruct.pt });
			}
			return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		private const int WH_MOUSE_LL = 14;
		private const int WH_MOUSE = 7;

		public class MouseHookEventArgs
		{
			public MouseMessages Message
			{
				get;set;
			}
			public POINT Point
			{
				get;set;
			}
		}

		//more messages
		//https://docs.microsoft.com/en-us/windows/win32/inputdev/mouse-input-notifications
		public enum MouseMessages
		{
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSEWHEEL = 0x020A,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MSLLHOOKSTRUCT
		{
			public POINT pt;
			public uint mouseData;
			public uint flags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		//https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa?redirectedfrom=MSDN
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook,
			LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

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