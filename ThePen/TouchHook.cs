using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThePen
{
	public class TouchHook
	{
		[DllImport("user32.dll")]
		public static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
		   uint wMsgFilterMax);


		const int WM_NCPOINTERUPDATE = 0x0241;
		const int WM_NCPOINTERDOWN = 0x0242;
		const int WM_NCPOINTERUP = 0x0243;
		const int WM_POINTERUPDATE = 0x0245; //581
		const int WM_POINTERDOWN = 0x0246;
		const int WM_POINTERUP = 0x0247; //583
		const int WM_POINTERENTER = 0x0249; //585
		const int WM_POINTERLEAVE = 0x024A; //586
		const int WM_POINTERACTIVATE = 0x024B;
		const int WM_POINTERCAPTURECHANGED = 0x024C;
		const int WM_TOUCHHITTESTING = 0x024D;
		const int WM_POINTERWHEEL = 0x024E;
		const int WM_POINTERHWHEEL = 0x024F;
		const int DM_POINTERHITTEST = 0x0250;

		[StructLayout(LayoutKind.Sequential)]
		public struct MSG
		{
			public IntPtr hwnd;
			public UInt32 message;
			public IntPtr wParam;
			public IntPtr lParam;
			public UInt32 time;
			public POINT pt;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;

			public System.Drawing.Point ToPoint()
			{
				return new System.Drawing.Point(x, y);
			}
		}

		public enum PointerInputType
		{
			/// <summary>
			/// Generic pointer type. This type never appears in pointer messages or pointer data. Some data query functions allow the caller to restrict the query to specific pointer type. The PT_POINTER type can be used in these functions to specify that the query is to include pointers of all types
			/// </summary>
			POINTER = 0x00000001,
			/// <summary>
			/// Touch pointer type.
			/// </summary>
			TOUCH = 0x00000002,
			/// <summary>
			/// Pen pointer type.
			/// </summary>
			PEN = 0x00000003,
			/// <summary>
			/// Mouse pointer type
			/// </summary>
			MOUSE = 0x00000004
		};

		[DllImport("User32")]
		static extern bool RegisterPointerInputTarget(IntPtr hwnd, PointerInputType pointerType);
		//[DllImport("User32")]
		//static extern bool GetPointerFrameTouchInfo(UInt32 pointerId, ref UInt32 pointerCount, [Out] PointerTouchInfo[] touchInfo);
		//static uint GET_POINTERID_WPARAM(uint wParam) { return ((HiLoWord)wParam).Low; }

		public void Install(IntPtr handle)
		{ 
			var ok = RegisterPointerInputTarget(handle, PointerInputType.PEN);
			if (!ok)
			{
				Debug.WriteLine("失败 RegisterPointerInputTarget: ");
				return; //todo: !!
			}
			else
			{
				Debug.WriteLine("TouchHook Installed.");
			}



		}

		public void Run()
		{
			MSG msg;
			int ret;

			//var sim = new InputSimulator();

			//TouchInjector.InitializeTouchInjection();
			//var screenBounds = Rectangle.Empty;


			ret = GetMessage(out msg, IntPtr.Zero, 0, 0);

			if (ret == -1)
			{
				Debug.WriteLine("Error!");
			}
			//var pointerId = GET_POINTERID_WPARAM((uint)msg.wParam.ToInt32());

			//TODO: refactor... just hacking...
			switch (msg.message)
			{
				case WM_POINTERDOWN:
					Debug.WriteLine("Touch Down: ");

					break;

				case WM_POINTERUPDATE:
					Debug.Write('.');

					break;


				case WM_POINTERUP:
					Debug.WriteLine("Touch Up");
					break;

				case WM_POINTERENTER:
					Debug.WriteLine("Touch Enter");
					//ConvertToNewTouchInfo(contacts, PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT);
					break;

				case WM_POINTERLEAVE:
					Debug.WriteLine("Touch Leave");
					//ConvertToNewTouchInfo(contacts, PointerFlags.UP | PointerFlags.INRANGE | PointerFlags.INCONTACT);
					break;

				default:
					Debug.WriteLine("Unhandled Msg: " + msg.message);
					break;
			}
			Debug.WriteLine("???");
		}
	}
}
