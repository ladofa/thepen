using System.Collections.Generic;

namespace ThePen
{
	/// <summary>
	/// List of the displays attached to this system.
	/// </summary>
	class Monitors
	{
		/// <param name="rate">dpi scale of the primary display. Non-primary displays are divided by it.</param>
		internal static List<Screen> GetScreens(double rate = 1)
		{
			List<Screen> screens = new();

			foreach (var display in System.Windows.Forms.Screen.AllScreens)
			{
				var bounds = display.Bounds;
				var screen = new Screen(display.Primary, bounds.X, bounds.Y, bounds.Width, bounds.Height);

				if (!screen.IsPrimary)
				{
					screen.ApplyRate(rate);
				}

				screens.Add(screen);
			}

			return screens;
		}

		internal sealed class Screen
		{
			internal Screen(bool primary, int x, int y, int w, int h)
			{
				IsPrimary = primary;
				TopX = x;
				TopY = y;
				Width = w;
				Height = h;
			}

			internal bool IsPrimary { get; private set; }

			//left-top corner
			internal int TopX { get; private set; }
			internal int TopY { get; private set; }

			internal int Width { get; private set; }
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
}
