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
		private delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		private static IntPtr HookCallback(
			int nCode, IntPtr wParam, IntPtr lParam)
		{
			MouseMessages message = (MouseMessages)wParam;

			if (nCode >= 0)
			{
				MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				MouseHookEvent?.Invoke(null, new MouseHookEventArgs() { 
					Message = message, Point = hookStruct.pt, ExtraInfo = hookStruct.dwExtraInfo,
				});

				//Debug.WriteLine(wParam);
				//Debug.WriteLine(hookStruct.dwExtraInfo);
				//Debug.WriteLine(hookStruct.flags);
				//Debug.WriteLine(hookStruct.mouseData);
				//Debug.WriteLine(hookStruct.time);
				//Debug.WriteLine(hookStruct.pt.x);


				if (message == MouseMessages.WM_MOUSEMOVE && Global.SettingData.ShakeToClearAll)
				{
					if (System.Windows.Input.Mouse.LeftButton != System.Windows.Input.MouseButtonState.Pressed)
						analyseGesture2(hookStruct);
				}
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

			public IntPtr ExtraInfo
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

		#region gesture

		static List<uint> hsTime = new();
		static List<POINT> hsPoint = new();
		static List<(double, double, uint)> hsVector = new();

		public static event EventHandler Shaked;

		private static void analyseGesture2(MSLLHOOKSTRUCT hookStruct)
		{
			POINT p = hookStruct.pt;
			uint t = hookStruct.time;

			if (hsPoint.Count >= 3)
			{
				var pt = hsTime[0];
				double dt = t - pt;
				if (dt == 0) return;

				double dx = p.x - hsPoint[0].x;
				double dy = p.y - hsPoint[0].y;
				if (dx == 0 && dy == 0)return;

				double len = Math.Sqrt(dx * dx + dy * dy);
				var vel =  len / (double)dt;
				double dnx = dx / len;
				double dny = dy / len;

				if (vel > 2.5)
				{
					void _check()
					{
						if (hsVector.Count >= 5)
						{
							if (t - hsVector[0].Item3 < 700)
							{
								Shaked?.Invoke(null, null);
							}
							hsVector.RemoveAt(0);
						}
					}

					while (hsVector.Count != 0)
					{
						(_, _, var lt) = hsVector.Last();
						if (t - lt > 500)
						{
							hsVector.RemoveAt(hsVector.Count - 1);
						}
						else
						{
							break;
						}
					}

					if (hsVector.Count == 0)
					{
						hsVector.Add((dnx, dny, t));
						_check();
					}
					else
					{
						(var pdnx, var pdny, _) = hsVector.Last();
						var sx = pdnx + dnx;
						var sy = pdny + dny;
						var s = sx * sx + sy * sy;

						
						if (s < 0.005)
						{
							hsVector.Add((dnx, dny, t));
							_check();
						}
					}
				}

				hsPoint.RemoveAt(0);
				hsTime.RemoveAt(0);

			}


			hsPoint.Add(p);
			hsTime.Add(t);
		}




		#endregion
	}
}