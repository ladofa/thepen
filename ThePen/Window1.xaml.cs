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
using System.Windows.Shapes;
using System.Windows.Interop;

using System.Windows.Ink;

using System.Diagnostics;



namespace ThePen
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();

			//for undo/redo
			Board.Strokes.StrokesChanged += Strokes_StrokesChanged;

			//for mouse effect
			MouseHook.MouseHookEvent += MouseHook_MouseHookEvent;
			MouseHook.Start();

			AdoptSetting();
		}

		private void AdoptSetting()
		{
			SettingData data = Global.SettingData;
			mainPen = data.Pen1;

			//init mouse click effect
			if (Global.SettingData.MouseEffectMove)
			{
				MouseEffectMove.Visibility = Visibility.Visible;
			}
			else
			{
				MouseEffectMove.Visibility = Visibility.Collapsed;
			}
		}

		DrawingAttributes mainPen;
		public delegate void MainPenChangedEventHandler(object sender, int num);
		public event MainPenChangedEventHandler MainPenChanged;

		public void ChangeMainPen(int num)
		{
			List<DrawingAttributes> pens = new()
			{
				Global.SettingData.Pen1,
				Global.SettingData.Pen2,
				Global.SettingData.Pen3,
			};
			mainPen = pens[num - 1];
			Board.DefaultDrawingAttributes = mainPen;
			previousPen = mainPen.Clone();
			MainPenChanged?.Invoke(this, num);
		}

		public delegate void ColorChangedEventHandler(object sender, Color color);
		public event ColorChangedEventHandler ColorChanged;
		public void ChangeColor(Color color)
		{
			mainPen.Color = color;
			ColorChanged?.Invoke(this, color);
		}

		private DrawingAttributes previousPen;

		bool isPressed = false;
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);

			if (isPressed) return;

			var data = Global.SettingData;

			if (e.Key == data.OneErase)
			{
				Board.EditingMode = InkCanvasEditingMode.EraseByStroke;
			}

			if (!data.OneKeyImme)
			{
				previousPen = Board.DefaultDrawingAttributes.Clone();
			}

			if (e.Key == data.OneColor1)
			{
				Board.DefaultDrawingAttributes.Color = data.Palette1;
				Debug.WriteLine(previousPen.Color);
				Debug.WriteLine(Board.DefaultDrawingAttributes.Color);
			}

			isPressed = true;
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			isPressed = false;
			Board.DefaultDrawingAttributes = previousPen;
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyUp(e);

			var data = Global.SettingData;

			if (e.Key == data.OneErase)
			{
				Board.EditingMode = InkCanvasEditingMode.Ink;
			}

			if (!data.OneKeyImme)
			{
				Board.DefaultDrawingAttributes = previousPen.Clone();
			}

			isPressed = false;
		}

		#region mouse effect
		private void MouseHook_MouseHookEvent(object sender, MouseHook.MouseHookEventArgs e)
		{
			var p = new Point(e.Point.x, e.Point.y);
			var message = e.Message;

			if (message == MouseHook.MouseMessages.WM_MOUSEMOVE)
			{
				Canvas.SetLeft(MouseEffectGroup, p.X - 50);
				Canvas.SetTop(MouseEffectGroup, p.Y - 50);
				//MouseEffectMove.Margin = new Thickness() { Left = p.X - 15, Top = p.Y - 15 };
			}

			if (message == MouseHook.MouseMessages.WM_LBUTTONDOWN && Global.SettingData.MouseEffectLeftDown)
			{
				MouseEffectLeftDown.Visibility = Visibility.Visible;

				if (Global.SettingData.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Collapsed;
				}
			}
			else if (message == MouseHook.MouseMessages.WM_LBUTTONUP && Global.SettingData.MouseEffectLeftDown)
			{
				MouseEffectLeftDown.Visibility = Visibility.Collapsed;
				if (Global.SettingData.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Visible;
				}
			}
			else if (message == MouseHook.MouseMessages.WM_RBUTTONDOWN && Global.SettingData.MouseEffectRightDown)
			{
				MouseEffectRightDown.Visibility = Visibility.Visible;
				if (Global.SettingData.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Collapsed;
				}
			}
			else if (message == MouseHook.MouseMessages.WM_RBUTTONUP && Global.SettingData.MouseEffectRightDown)
			{
				MouseEffectRightDown.Visibility = Visibility.Collapsed;
				if (Global.SettingData.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Visible;
				}
			}
		}
		#endregion


		//private void Window_SourceInitialized(object sender, EventArgs e)
		//{
		//    base.OnSourceInitialized(e);
		//    var hwnd = new WindowInteropHelper(this).Handle;
		//    WindowsService.SetWindowExTransparent(hwnd);
		//}

		

		//--------------------------------------------------------------------------

		public void SetDrawingArea(Rect rect)
		{
			GridBoard.Margin = new Thickness() { Left = rect.X, Top = rect.Y };
			GridBoard.Width = rect.Width;
			GridBoard.Height = rect.Height;
		}

		public enum DrawingModes
		{
			Select,
			Draw,
			Erase
		}

		DrawingModes drawingMode = DrawingModes.Select;

		public event EventHandler DrawingModeChanged;
		public DrawingModes DrawingMode
		{
			get => drawingMode;
			set
			{
				bool drawing = true;
				if (value == DrawingModes.Select)
				{
					drawing = false;
				}

				if (drawing)
				{
					BackBoard.Fill = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
					Overlay.Visibility = Visibility.Collapsed;
				}
				else
				{
					BackBoard.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
					Overlay.Visibility = Visibility.Visible;
				}

				//IsHitTestVisible = drawing;
				Board.IsHitTestVisible = drawing;
				//IsEnabled = drawing;
				Board.IsEnabled = drawing;
				Focusable = drawing;
				//Board.Focusable = drawing;

				drawingMode = value;
				DrawingModeChanged?.Invoke(this, new EventArgs());
			}
		}

		public bool DrawingVisibility
		{
			get => Board.Visibility == Visibility.Visible;

			set
			{
				if (value)
				{
					Board.Visibility = Visibility.Visible;
				}
				else
				{
					Board.Visibility = Visibility.Hidden;
				}
			}
		}


		public void ClearAll()
		{
			Board.Strokes.Clear();

			//var s = new System.Windows.Ink.Stroke(new StylusPointCollection(new List<Point> {
			//	new Point(0, 0), new Point(100, 100)}))
			//{

			//	DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			//};

			//Board.Strokes.Add(s);
		}


		#region undo/redo
		//thanks to https://stackoverflow.com/questions/6368517/undo-redo-command-stack-for-inkBoard/52911463#52911463

		List<System.Windows.Ink.StrokeCollection> _added = new();
		List<System.Windows.Ink.StrokeCollection> _removed = new();
		List<System.Windows.Ink.StrokeCollection> _added_redo = new();
		List<System.Windows.Ink.StrokeCollection> _removed_redo = new();

		private bool handle = true;

		private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
		{
			if (handle)
			{
				_added.Add(e.Added);
				_removed.Add(e.Removed);
				_added_redo.Clear();
				_removed_redo.Clear();
			}
		}


		internal void Undo(object sender, RoutedEventArgs e)
		{
			if (_added.Count == 0) return;
			handle = false;
			var a = _added.Last();
			var r = _removed.Last();
			Board.Strokes.Remove(a);
			Board.Strokes.Add(r);
			_added_redo.Add(a);
			_removed_redo.Add(r);
			_added.RemoveAt(_added.Count - 1);
			_removed.RemoveAt(_removed.Count - 1);
			handle = true;
		}

		internal void Redo(object sender, RoutedEventArgs e)
		{
			if (_added_redo.Count == 0) return;
			handle = false;
			var a = _added_redo.Last();
			var r = _removed_redo.Last();
			Board.Strokes.Add(a);
			Board.Strokes.Remove(r);
			_added.Add(a);
			_removed.Add(r);
			_added_redo.RemoveAt(_added_redo.Count - 1);
			_removed_redo.RemoveAt(_removed_redo.Count - 1);
			handle = true;
		}
		#endregion


		#region EasySwitch
		private void AutoDrawAreaLeft_StylusEnter(object sender, StylusEventArgs e)
		{
			if (Global.SettingData.EasySwitch)
				DrawingMode = DrawingModes.Draw;
		}

		private void AutoDrawAreaRight_StylusEnter(object sender, StylusEventArgs e)
		{
			if (Global.SettingData.EasySwitch)
				DrawingMode = DrawingModes.Draw;
		}

		private void AutoDrawAreaTop_StylusEnter(object sender, StylusEventArgs e)
		{
			if (Global.SettingData.EasySwitch)
				DrawingMode = DrawingModes.Draw;
		}

		private void AutoDrawAreaBottom_StylusEnter(object sender, StylusEventArgs e)
		{
			if (Global.SettingData.EasySwitch)
				DrawingMode = DrawingModes.Draw;
		}

		int mouseMoveCount = 0;

		private void Board_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			mouseMoveCount++;
			if (mouseMoveCount > 10)
			{
				if (Global.SettingData.EasySwitch)
					DrawingMode = DrawingModes.Select;
			}
		}

		private void Board_PreviewStylusInAirMove(object sender, StylusEventArgs e)
		{
			mouseMoveCount = 0;
		}

		private void Board_PreviewStylusMove(object sender, StylusEventArgs e)
		{
			mouseMoveCount = 0;
		}
		#endregion
	}
}
