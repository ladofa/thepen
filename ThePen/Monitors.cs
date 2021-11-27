using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ThePen
{
    class Monitors
    {
        private static List<Monitors.Screen> Screens = null;

        internal static List<Monitors.Screen> GetScreens(double rate = 1)
        {
            Screens = new List<Monitors.Screen>();

            var handler = new NativeMethods.DisplayDevicesMethods.EnumMonitorsDelegate(Monitors.MonitorEnumProc);
            NativeMethods.DisplayDevicesMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, handler, IntPtr.Zero); // should be sequential

			foreach (var screen in Screens)
			{
				screen.ApplyRate(rate);
				if (!screen.IsPrimary)
				{
					screen.ApplyRate(rate);
				}
			}



			return Screens;
        }

        private static bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, NativeMethods.DisplayDevicesMethods.RECT rect, IntPtr dwData)
        {
            NativeMethods.DisplayDevicesMethods.MONITORINFO mi = new();

            if (NativeMethods.DisplayDevicesMethods.GetMonitorInfo(hMonitor, mi))
            {
                Monitors.Screens.Add(new Monitors.Screen(
                    (mi.dwFlags & 1) == 1, // 1 = primary monitor
                    mi.rcMonitor.Left,
                    mi.rcMonitor.Top,
                    Math.Abs(mi.rcMonitor.Right - mi.rcMonitor.Left),
                    Math.Abs(mi.rcMonitor.Bottom - mi.rcMonitor.Top)));
            }

            return true;
        }

        /// <summary>
        /// Represents a display device on a single system.
        /// </summary>
        internal sealed class Screen
        {
            /// <summary>
            /// Initializes a new instance of the Screen class.
            /// </summary>
            /// <param name="primary">A value indicating whether the display is the primary screen.</param>
            /// <param name="x">The display's top corner X value.</param>
            /// <param name="y">The display's top corner Y value.</param>
            /// <param name="w">The width of the display.</param>
            /// <param name="h">The height of the display.</param>
            internal Screen(bool primary, int x, int y, int w, int h)
            {
                this.IsPrimary = primary;
                this.TopX = x;
                this.TopY = y;
                this.Width = w;
                this.Height = h;
            }

            /// <summary>
            /// Gets a value indicating whether the display device is the primary monitor.
            /// </summary>
            internal bool IsPrimary { get; private set; }

            /// <summary>
            /// Gets the display's top corner X value.
            /// </summary>
            internal int TopX { get; private set; }

            /// <summary>
            /// Gets the display's top corner Y value.
            /// </summary>
            internal int TopY { get; private set; }

            /// <summary>
            /// Gets the width of the display.
            /// </summary>
            internal int Width { get; private set; }

            /// <summary>
            /// Gets the height of the display.
            /// </summary>
            internal int Height { get; private set; }

            internal void ApplyRate(double rate)
			{
                TopX = (int)(TopX / rate);
                TopY = (int)(TopY / rate);
                Width = (int)(Width / rate);
                Height = (int)(Height / rate);
            }
        }
    }

    internal static class NativeMethods
    {
        /// <summary>
        /// Methods for retrieving display devices.
        /// </summary>
        internal static class DisplayDevicesMethods
        {
            internal delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, NativeMethods.DisplayDevicesMethods.RECT rect, IntPtr dwData);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

            /// <summary>
            /// Retrieves information about a display monitor.
            /// </summary>
            /// <param name="hmonitor">A handle to the display monitor of interest.</param>
            /// <param name="info">A pointer to a MONITORINFO or MONITORINFOEX structure that receives information about the specified display monitor.</param>
            /// <returns>If the function succeeds, the return value is nonzero.</returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out] NativeMethods.DisplayDevicesMethods.MONITORINFO info);

            /// <summary>
            /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            internal struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            /// <summary>
            /// The MONITORINFO structure contains information about a display monitor.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
            internal class MONITORINFO
            {
                internal int cbSize = Marshal.SizeOf(typeof(NativeMethods.DisplayDevicesMethods.MONITORINFO));
                internal NativeMethods.DisplayDevicesMethods.RECT rcMonitor = new();
                internal NativeMethods.DisplayDevicesMethods.RECT rcWork = new();
                internal int dwFlags;
            }
        }
    }
}
