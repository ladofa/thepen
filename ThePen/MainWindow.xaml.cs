using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;

namespace ThePen
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			
			
			
		}


		List<Window1> canvasWindows = new();

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			setScreen();
		}

		void setScreen()
		{
			Owner = null;
			canvasWindows.ForEach(w => w.Close());
			canvasWindows.Clear();

			var screens = Monitors.GetScreens();

			screens.RemoveAt(0);

			int minX = screens[0].TopX;
			int minY = screens[0].TopY;
			int maxX = screens[0].TopX + screens[0].Width;
			int maxY = screens[0].TopY + screens[0].Height;



			foreach (var screen in screens)
			{
				if (minX > screen.TopX)
				{
					minX = screen.TopX;
				}

				if (minY > screen.TopY)
				{
					minY = screen.TopY;
				}

				if (maxX < screen.TopX + screen.Width)
				{
					maxX = screen.TopX + screen.Width;
				}

				if (maxY < screen.TopY + screen.Height)
				{
					maxY = screen.TopY + screen.Height;
				}

			}

			var win = new Window1();
			win.Show();
			this.Owner = win;
			canvasWindows.Add(win);
			win.Left = minX;
			win.Top = minY;
			win.Width = maxX - minX;
			win.Height = maxY - minY;
			//win.WindowState = WindowState.Maximized;

			canvasWindows.ForEach(w => w.SetDrawing(false));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		//draw
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.SetDrawing());
		}

		//point
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.SetDrawing(false));
		}

		private void TestButton_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void Hook_Click(object sender, RoutedEventArgs e)
		{
			List<(uint, uint, Action)> hotkeys = new();
			hotkeys.Add((Hotkey.MOD_CTRL, Hotkey.TrigKeys["A"], new Action(()=>
			{
				canvasWindows.ForEach(w => w.SetDrawing());
			})));
			hotkeys.Add((Hotkey.MOD_CTRL, Hotkey.TrigKeys["Backspace"], new Action(() =>
			{
				canvasWindows.ForEach(w => w.SetDrawing(false));
			})));
			Hotkey.hook(this, hotkeys);
		}

		private void Unhook_Click(object sender, RoutedEventArgs e)
		{
			Hotkey.unhook();
		}


		#region titlebar move
		//thannks to https://gigong.tistory.com/99
		private void border_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (this.WindowState == WindowState.Maximized && Math.Abs(startPos.Y - e.GetPosition(null).Y) > 2)
				{
					var point = PointToScreen(e.GetPosition(null));

					this.WindowState = WindowState.Normal;

					this.Left = point.X - this.ActualWidth / 2;
					this.Top = point.Y - border.ActualHeight / 2;
				}
				DragMove();
			}
		}

		private Point startPos;

		#endregion

		private void Window_Closed(object sender, EventArgs e)
		{
			canvasWindows.ForEach(w => w.Close());
		}

		private void ClearAll_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ClearAll());
		}

		private void border_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			
		}

		private void Undo(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.Undo(sender, e));
		}

		private void Redo(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.Redo(sender, e));
		}


	}
}
