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
using System.Linq.Expressions;
using System.Reflection;


namespace ThePen
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<Window1> canvasWindows = new();

		public MainWindow()
		{
            InitializeComponent();

			HideAllPopup();

			Global.SettingChanged += Global_SettingChanged;


			//-------------------------------------
		}

		

		private void Global_SettingChanged(object sender, EventArgs e)
		{
			setScreen();
			Hook();

			(ColorButton1.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Palette1);
			(ColorButton2.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Palette2);
			(ColorButton3.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Palette3);
			(ColorButton4.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Palette4);
			(ColorButton5.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Palette5);
			(ColorButton6.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Palette6);

			(DrawButton1.Image as PenRepresenter).Value = Global.SettingData.Pen1;
			(DrawButton2.Image as PenRepresenter).Value = Global.SettingData.Pen2;
			(DrawButton3.Image as PenRepresenter).Value = Global.SettingData.Pen3;

			canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Normal);
			canvasWindows.ForEach(w => w.ChangeMainPen(Global.CurrentPen));
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Global.LoadFromReg();
		}

		List<Monitors.Screen> screens;

		/// <summary>
		/// check multi display
		/// </summary>
		void setScreen()
		{
			//clear existing
			Owner = null;
			canvasWindows.ForEach(w => w.Close());
			canvasWindows.Clear();

			PresentationSource source = PresentationSource.FromVisual(this);

			double dpiX = 0, dpiY = 0;
			if (source != null)
			{
				dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
				dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
			}

			screens = Monitors.GetScreens(source.CompositionTarget.TransformToDevice.M11);

			//entire display size
			Rect getRect(List<Monitors.Screen> screens)
			{
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

				return new Rect() { X = minX, Y = minY, Width = maxX - minX, Height = maxY - minY };
			}

			

			//size of drawing
			List<Monitors.Screen> drawingScreen = new();
			if (Global.SettingData.Display1 && screens.Count >= 1)
			{
				drawingScreen.Add(screens[0]);
			}
			if (Global.SettingData.Display2 && screens.Count >= 2)
			{
				drawingScreen.Add(screens[1]);
			}
			if (Global.SettingData.Display3 && screens.Count >= 3)
			{
				drawingScreen.Add(screens[2]);
			}
			if (Global.SettingData.Display4 && screens.Count >= 4)
			{
				drawingScreen.Add(screens[3]);
			}

			if (drawingScreen.Count > 0)
			{
				var rectWindow = getRect(drawingScreen);
				var win = new Window1();
				win.Show();
				//this.Owner = win;
				canvasWindows.Add(win);
				win.Left = rectWindow.X;
				win.Top = rectWindow.Y;
				win.Width = rectWindow.Width;
				win.Height = rectWindow.Height - 1;
				//win.WindowState = WindowState.Maximized;

				var rectBoard = getRect(drawingScreen);
				win.SetDrawingArea(rectWindow, rectBoard);
				win.MainPenChanged += Win_MainPenChanged;
				win.DrawingModeChanged += Win_DrawingModeChanged;
				win.ColorChanged += Win_ColorChanged;
			}
		}

		private void Win_MainPenChanged(object sender, int num)
		{
			DrawButton1.Checked = false;
			DrawButton2.Checked = false;
			DrawButton3.Checked = false;

			if (num == 1)
			{
				DrawButton1.Checked = true;
				(ColorButton.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Pen1.Color);
			}
			else if (num == 2)
			{
				DrawButton2.Checked = true;
				(ColorButton.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Pen2.Color);
			}
			else
			{
				DrawButton3.Checked = true;
				(ColorButton.Image as Rectangle).Fill = new SolidColorBrush(Global.SettingData.Pen3.Color);
			}
		}

		private void Win_ColorChanged(object sender, Color color)
		{
			(ColorButton.Image as Rectangle).Fill = new SolidColorBrush(color);
		}

		private void Win_DrawingModeChanged(object sender, EventArgs e)
		{
			Window1 w = (Window1)sender;
			if (w.DrawingMode == Window1.DrawingModes.Normal)
			{
				SelectButton.Checked = true;
				DrawButton.Checked = false;
				EraseButon.Checked = false;
			}
			else if (w.DrawingMode == Window1.DrawingModes.Draw)
			{
				SelectButton.Checked = false;
				DrawButton.Checked = true;
				EraseButon.Checked = false;
			}
			else
			{
				SelectButton.Checked = false;
				DrawButton.Checked = false;
				EraseButon.Checked = true;
			}
		}

		public void Hook()
		{
			SettingData data = Global.SettingData;
			Hotkey.Unhook();
			List<(uint, uint, Action)> hotkeys = new();

			void setHotkey((uint, uint) key, Action action)
			{
				if (key.Item2 == 0) return;
				hotkeys.Add((key.Item1, key.Item2, action));
			}

			setHotkey(data.HotClear, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ClearAll());
			}));

			setHotkey(data.HotClear, new Action(() =>
			{
				canvasWindows.ForEach(w => w.Undo(null, null));
			}));

			setHotkey(data.HotColor1, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeColor(0));
			}));

			setHotkey(data.HotColor2, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeColor(1));
			}));

			setHotkey(data.HotColor3, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeColor(2));
			}));

			setHotkey(data.HotColor4, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeColor(3));
			}));

			setHotkey(data.HotColor5, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeColor(4));
			}));

			setHotkey(data.HotColor6, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeColor(5));
			}));

            setHotkey(data.HotSwapPalette, new Action(() =>
            {
                canvasWindows.ForEach(w => w.SwapPalette());
            }));

            setHotkey(data.HotErase, new Action(() =>
			{
				canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Erase);
			}));

			setHotkey(data.HotOverlay1, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ToggleOverlay1());
			}));

			setHotkey(data.HotOverlay2, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ToggleOverlay2());
			}));

			setHotkey(data.HotOverlay3, new Action(() =>
			{
				;
			}));

			setHotkey(data.HotOverlay4, new Action(() =>
			{
				;
			}));

			setHotkey(data.HotPen1, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeMainPen(1));
				canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Draw);
			}));

			setHotkey(data.HotPen2, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeMainPen(2));
				canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Draw);
			}));

			setHotkey(data.HotPen3, new Action(() =>
			{
				canvasWindows.ForEach(w => w.ChangeMainPen(3));
				canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Draw);
			}));

			setHotkey(data.HotNormal, new Action(() =>
			{
				canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Normal);
			}));

            setHotkey(data.HotClearNormal, new Action(() =>
            {
                canvasWindows.ForEach(w => w.ClearNormal());
            }));

            setHotkey(data.HotBoard1, new Action(() =>
            {
                canvasWindows.ForEach(w => w.ToggleCurrentBoard(0));
            }));

            setHotkey(data.HotBoard2, new Action(() =>
            {
                canvasWindows.ForEach(w => w.ToggleCurrentBoard(1));
            }));

            setHotkey(data.HotBoard3, new Action(() =>
            {
                canvasWindows.ForEach(w => w.ToggleCurrentBoard(2));
            }));



            Hotkey.Hook(this, hotkeys);
		}


		#region titlebar move
		//thannks to https://gigong.tistory.com/99

		bool titleBarMoved = false;
		private void Hat_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (this.WindowState == WindowState.Maximized && Math.Abs(startPos.Y - e.GetPosition(null).Y) > 2)
				{
					var point = PointToScreen(e.GetPosition(null));

					this.WindowState = WindowState.Normal;

					this.Left = point.X - this.ActualWidth / 2;
					this.Top = point.Y - Hat.ActualHeight / 2;
				}
				DragMove();

				titleBarMoved = true;
			}
		}

		private Point startPos;

		#endregion

		private void Window_Closed(object sender, EventArgs e)
		{
			canvasWindows.ForEach(w => w.Close());
		}


		#region undo/redo
		private void Undo(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.Undo(sender, e));
		}

		private void Redo(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.Redo(sender, e));
		}

		#endregion



		#region main button click
		private void SelectButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Normal);
		}

		private void DrawButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Draw);
			ShowPopup(DrawButton, DrawPopup);
		}

		private void EraseButon_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Erase);
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			canvasWindows.ForEach(w => w.ClearAll());
		}

		private void OverlayButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			ShowPopup(OverlayButton, OverlayPopup);
		}

		private void ShapeButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			ShowPopup(ShapeButton, ShapePopup);
		}

		private void StampButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			ShowPopup(StampButton, StampPopup);
		}

		private void ColorButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			ShowPopup(ColorButton, ColorPopup);
		}


		#endregion

		#region Hat
		public Orientation Orientation
		{
			get
			{
				return MainStackPanel.Orientation;
			}

			set
			{
				MainStackPanel.Orientation = value;
				MainButtonsStackPanel.Orientation = value;
				DrawPopup.Orientation = value;
				OverlayPopup.Orientation = value;
				ColorPopup.Orientation = value;
			}
		}
		private void Hat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			titleBarMoved = false;
		}

		private void Hat_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			//it must be here
			if (titleBarMoved)
			{
				return;
			}

			HideAllPopup();

			ShowPopup(Hat, HatPopup);
	
		}

		private void Hat_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			titleBarMoved = false;
		}

		private void Hat_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			//it must be here
			if (titleBarMoved)
			{
				return;
			}

			HideAllPopup();

			//change orientation
			if (MainButtonsStackPanel.Visibility == Visibility.Collapsed)
			{
				MainButtonsStackPanel.Visibility = Visibility.Visible;
				Orientation = Orientation.Horizontal;
			}
			else if (Orientation == Orientation.Horizontal)
			{
				Orientation = Orientation.Vertical;
			}
			else// if (Orientation == Orientation.Vertical)
			{
				MainButtonsStackPanel.Visibility = Visibility.Collapsed;
			}

			HideAllPopup();

			Reg.Write("test", 3.14);
			Reg.Read("test", out double k);
			Debug.WriteLine(k);

			Color a = Color.FromArgb(10, 20, 30, 40);
			Reg.Write("test2", a);
			Reg.Read("test2", out Color b);
			Debug.WriteLine(b.ToString());

			foreach (PropertyInfo prop in typeof(Global).GetProperties())
			{
				object asdf = prop.GetValue(a, null);
				Debug.WriteLine("{0} = {1}", prop.Name, prop.GetValue(a, null));
			}
		}

		#endregion



		#region popup

		void HideAllPopup()
		{
			DrawPopup.Visibility = Visibility.Hidden;
			OverlayPopup.Visibility = Visibility.Hidden;
			ColorPopup.Visibility = Visibility.Hidden;
			HatPopup.Visibility = Visibility.Hidden;
			ShapePopup.Visibility = Visibility.Hidden;
			StampPopup.Visibility = Visibility.Hidden;
		}

		Thickness getMarginInScreen(UIElement element)
		{
			var posHat = element.TranslatePoint(new Point(0, 0), this);
			posHat.X += Application.Current.MainWindow.Left;
			posHat.Y += Application.Current.MainWindow.Top;
			int currentScreen = 0;

			for (int i = 1; i < screens.Count; i++)
			{
				var screen = screens[i];

				if (screen.TopX <= posHat.X && posHat.X < screen.TopX + screen.Width &&
					screen.TopY <= posHat.Y && posHat.Y < screen.TopY + screen.Height)
				{
					currentScreen = i;
				}
			}

			var scr = screens[currentScreen];
			posHat.X -= scr.TopX;
			posHat.Y -= scr.TopY;

			return new Thickness(posHat.X, posHat.Y, scr.Width - posHat.X, scr.Height - posHat.Y);
		}

		void ShowPopup(FrameworkElement button, StackPanel popup)
		{
			var buttonWidth = button.ActualWidth;
			var buttonHeight = button.ActualHeight;

			var popupWidth = popup.ActualWidth;
			var popupHeight = popup.ActualHeight;



			var pad = getMarginInScreen(button);

			//left-top corner of popup window
			var pos = button.TranslatePoint(new Point(0, 0), MainGrid);

			//to left
			if (pad.Right - popupWidth - buttonWidth < 0)
			{
				pos.X -= popupWidth;
			}
			//to right
			else
			{
				if (Orientation == Orientation.Vertical)
				{
					pos.X += buttonWidth;
				}
			}

			//to top
			if (pad.Bottom - popupHeight - buttonHeight < 0)
			{
				pos.Y -= popupHeight;
			}
			//to bottom
			else
			{
				if (Orientation == Orientation.Horizontal)
				{
					pos.Y += buttonHeight;
				}
			}

			popup.Margin = new Thickness(pos.X, pos.Y, 0, 0);
			popup.Visibility = Visibility.Visible;
		}


		private void Window_LostFocus(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
		}

		private void Window_MouseLeave(object sender, MouseEventArgs e)
		{
			HideAllPopup();
		}

		#endregion

		private void ColorButton1_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeColor(0));
			HideAllPopup();
		}

		private void ColorButton2_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeColor(1));
			HideAllPopup();
		}

		private void ColorButton3_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeColor(2));
			HideAllPopup();
		}

		private void ColorButton4_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeColor(3));
			HideAllPopup();
		}

		private void ColorButton5_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeColor(4));
			HideAllPopup();
		}

		private void ColorButton6_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeColor(5));
			HideAllPopup();
		}

		private void DrawButton1_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeMainPen(1));
			canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Draw);
			Global.CurrentPen = 1;
			HideAllPopup();
		}

		private void DrawButton2_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeMainPen(2));
			canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Draw);
			Global.CurrentPen = 2;
			HideAllPopup();
		}

		private void DrawButton3_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ChangeMainPen(3));
			canvasWindows.ForEach(w => w.DrawingMode = Window1.DrawingModes.Draw);
			Global.CurrentPen = 3;
			HideAllPopup();
		}

		private void OverlayButton1_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ToggleOverlay1());
			HideAllPopup();
		}
		
		private void OverlayButton2_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ToggleOverlay2());
			HideAllPopup();
		}

		private void OverlayButton3_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ToggleOverlay3());
			HideAllPopup();
		}

		private void OverlayButton4_Click(object sender, RoutedEventArgs e)
		{
			canvasWindows.ForEach(w => w.ToggleOverlay4());
			HideAllPopup();
		}

		private void HatSettingButton_Click(object sender, RoutedEventArgs e)
		{
			var setting = new Setting();
			setting.Owner = this;
			setting.Value = Global.SettingData;
			setting.ShowDialog();
			HideAllPopup();
		}

		private void HatInfoButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			string info =
				"ThePen alpha10";
			MessageBox.Show(info);
		}

		private void HatExitButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
			Close();
		}

		private void ShapeLine1Button_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
		}

		private void ShapeLine2Button_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
		}

		private void ShapeEllipseButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
		}

		private void ShapeRectangleButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
		}

		private void ShapeGridButton_Click(object sender, RoutedEventArgs e)
		{
			HideAllPopup();
		}

		public void GetCanvasActivate()
		{
			canvasWindows[0].Activate();
		}
	}
}
