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
			MouseHook.Shaked += MouseHook_Shaked;
			MouseHook.Start();

			AdoptSetting();

			PreviewTouchDown += Window1_PreviewTouchDown;
			PreviewTouchMove += Window1_PreviewTouchMove;
			PreviewTouchUp += Window1_PreviewTouchUp;
		}

		#region block touch

		private void Window1_PreviewTouchUp(object sender, TouchEventArgs e)
		{
			e.Handled = setting.BlockTouch;
		}

		private void Window1_PreviewTouchMove(object sender, TouchEventArgs e)
		{
			e.Handled = setting.BlockTouch;
		}

		private void Window1_PreviewTouchDown(object sender, TouchEventArgs e)
		{
			e.Handled = setting.BlockTouch;
		}

		#endregion

		SettingData setting;

		private void AdoptSetting()
		{
			setting = Global.SettingData;

			//init mouse click effect
			if (setting.MouseEffectMove)
			{
				MouseEffectMove.Visibility = Visibility.Visible;
			}
			else
			{
				MouseEffectMove.Visibility = Visibility.Collapsed;
			}

			AutoDrawAreaLeft.Width = setting.EdgeWidth;
			AutoDrawAreaRight.Width = setting.EdgeWidth;
			AutoDrawAreaBottom.Height = setting.EdgeWidth;
		}

		public delegate void MainPenChangedEventHandler(object sender, int num);
		public event MainPenChangedEventHandler MainPenChanged;

		public void ChangeMainPen(int num)
		{
			List<DrawingAttributes> pens = new()
			{
				setting.Pen1,
				setting.Pen2,
				setting.Pen3,
			};
			Board.DefaultDrawingAttributes = pens[num - 1].Clone();
			previousPen = Board.DefaultDrawingAttributes.Clone();
			MainPenChanged?.Invoke(this, num);
		}

		public delegate void ColorChangedEventHandler(object sender, Color color);
		public event ColorChangedEventHandler ColorChanged;
		public void ChangeColor(Color color)
		{
			Board.DefaultDrawingAttributes.Color = color;
			ColorChanged?.Invoke(this, color);
		}

		private DrawingAttributes previousPen;

		bool isPressed = false;
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);

			if (DrawingMode != DrawingModes.Draw) return;

			if (isPressed) return;

			//all code must be behind here.

			previousPen = Board.DefaultDrawingAttributes.Clone();

			if (e.Key == setting.OneErase)
			{
				DrawingMode = DrawingModes.Erase;
			}

			else if (e.Key == setting.OneClear)
			{
				ClearAll();
			}

			else if (e.Key == setting.OneSelect)
			{
				DrawingMode = DrawingModes.Select;
			}
			else if (e.Key == setting.OneStampX)
			{
				StampX();
			}
			else if (e.Key == setting.OneStampO)
			{
				StampO();
			}
			else if (e.Key == setting.OneStampTri)
			{
				StampTri();
			}
			else if (e.Key == setting.OneStampDot)
			{
				StampDot();
			}
			//pen changes
			else if (!setting.OneKeyImme)
			{
				if (e.Key == setting.OneColor1)
				{
					Board.DefaultDrawingAttributes.Color = setting.Palette1;
				}
				if (e.Key == setting.OneColor2)
				{
					Board.DefaultDrawingAttributes.Color = setting.Palette2;
				}
				if (e.Key == setting.OneColor3)
				{
					Board.DefaultDrawingAttributes.Color = setting.Palette3;
				}
				if (e.Key == setting.OneColor4)
				{
					Board.DefaultDrawingAttributes.Color = setting.Palette4;
				}
				if (e.Key == setting.OneColor5)
				{
					Board.DefaultDrawingAttributes.Color = setting.Palette5;
				}
				if (e.Key == setting.OneColor6)
				{
					Board.DefaultDrawingAttributes.Color = setting.Palette6;
				}
				if (e.Key == setting.OnePen1)
				{
					Board.DefaultDrawingAttributes = setting.Pen1.Clone();
				}
				if (e.Key == setting.OnePen2)
				{
					Board.DefaultDrawingAttributes = setting.Pen2.Clone();
				}
				if (e.Key == setting.OnePen3)
				{
					Board.DefaultDrawingAttributes = setting.Pen3.Clone();
				}

			}
			else
			{
				if (e.Key == setting.OneColor1)
				{
					ChangeColor(setting.Palette1);
				}
				if (e.Key == setting.OneColor2)
				{
					ChangeColor(setting.Palette2);
				}
				if (e.Key == setting.OneColor3)
				{
					ChangeColor(setting.Palette3);
				}
				if (e.Key == setting.OneColor4)
				{
					ChangeColor(setting.Palette4);
				}
				if (e.Key == setting.OneColor5)
				{
					ChangeColor(setting.Palette5);
				}
				if (e.Key == setting.OneColor6)
				{
					ChangeColor(setting.Palette6);
				}
				if (e.Key == setting.OnePen1)
				{
					DrawingMode = DrawingModes.Draw;
					ChangeMainPen(1);
				}
				if (e.Key == setting.OnePen2)
				{
					DrawingMode = DrawingModes.Draw;
					ChangeMainPen(2);
				}
				if (e.Key == setting.OnePen3)
				{
					DrawingMode = DrawingModes.Draw;
					ChangeMainPen(3);
				}
			}

			

			isPressed = true;
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);

			DrawingMode = DrawingModes.Select;
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyUp(e);

			if (e.Key == setting.OneErase)
			{
				DrawingMode = DrawingModes.Draw;
			}

			if (!setting.OneKeyImme)
			{
				Board.DefaultDrawingAttributes = previousPen.Clone();
			}

			isPressed = false;
		}

		#region shake to clear all
		private void MouseHook_Shaked(object sender, EventArgs e)
		{
			
			if (Mouse.LeftButton != MouseButtonState.Pressed)
			{
				ClearAll();

			}

		}
		#endregion

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

			if (message == MouseHook.MouseMessages.WM_LBUTTONDOWN && setting.MouseEffectLeftDown)
			{
				MouseEffectLeftDown.Visibility = Visibility.Visible;

				if (setting.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Collapsed;
				}
			}
			else if (message == MouseHook.MouseMessages.WM_LBUTTONUP && setting.MouseEffectLeftDown)
			{
				MouseEffectLeftDown.Visibility = Visibility.Collapsed;
				if (setting.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Visible;
				}
			}
			else if (message == MouseHook.MouseMessages.WM_RBUTTONDOWN && setting.MouseEffectRightDown)
			{
				MouseEffectRightDown.Visibility = Visibility.Visible;
				if (setting.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Collapsed;
				}
			}
			else if (message == MouseHook.MouseMessages.WM_RBUTTONUP && setting.MouseEffectRightDown)
			{
				MouseEffectRightDown.Visibility = Visibility.Collapsed;
				if (setting.MouseEffectMove)
				{
					MouseEffectMove.Visibility = Visibility.Visible;
				}
			}

			if (setting.EasySwitch)
			{
				if (e.ExtraInfo == (IntPtr)(0))
				{
					if (drawingMode != DrawingModes.Select)
					{
						DrawingMode = DrawingModes.Select;
					}
				}
				else
				{
					if (drawingMode == DrawingModes.Select)
					{
						DrawingMode = DrawingModes.Draw;
					}
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
				if (value == DrawingModes.Erase)
				{
					Board.EditingMode = InkCanvasEditingMode.EraseByStroke;
				}
				else
				{
					Board.EditingMode = InkCanvasEditingMode.Ink;
				}

				bool drawing = true;
				if (value == DrawingModes.Select)
				{
					drawing = false;
				}

				if (drawing)
				{
					BackBoard.Fill = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
					Overlay.Visibility = Visibility.Collapsed;
					Board.Focus();
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

				if (_added.Count > 1000)
				{
					_added.RemoveAt(0);
					_removed.RemoveAt(0);
				}
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

		#region stamp

		private void StampX()
		{
			double width = setting.StampWidth;
			double w2 = width / 2;
			var center = Mouse.GetPosition(Board);
			double left = center.X - w2;
			double top = center.Y - w2;
			double right = left + width;
			double bottom = top + width;

			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(new List<Point>
			{
					new Point(left, top), new Point(right, bottom),
			}))
			{

				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			var s2 = new System.Windows.Ink.Stroke(new StylusPointCollection(new List<Point>
			{
					new Point(left, bottom), new Point(right, top),
			}))
			{

				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			Board.Strokes.Add(s1);
			Board.Strokes.Add(s2);
		}

		private void StampO()
		{
			double width = setting.StampWidth;
			double w2 = width / 2;
			var center = Mouse.GetPosition(Board);
			var cx = center.X;
			var cy = center.Y;

			List<Point> points = new();
			for (double r = 0; r < Math.PI * 2; r += Math.PI / 8)
			{
				double vx = Math.Cos(r);
				double vy = Math.Sin(r);

				points.Add(new Point(cx + w2 * vx, cy + w2 * vy));
			}
			points.Add(points[0]);

			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
				points
			))
			{

				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			Board.Strokes.Add(s1);
		}

		private void StampDot()
		{
			double width = setting.StampWidth;
			double w2 = width / 2;
			var center = Mouse.GetPosition(Board);
			double left = center.X - w2;
			double top = center.Y - w2;
			double right = left + width;
			double bottom = top + width;

			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(new List<Point>
			{
					new Point(left, top), new Point(right, top),
					new Point(right, bottom), new Point(left, bottom), new Point(left, top)
			}))
			{

				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			Board.Strokes.Add(s1);
		}

		private void StampTri()
		{
			double width = setting.StampWidth;
			double w2 = width / 2;
			var center = Mouse.GetPosition(Board);
			var cx = center.X;
			var cy = center.Y;

			List<Point> points = new();
			for (double r = 0; r < Math.PI * 2; r += Math.PI / 1.5)
			{
				double vx = Math.Cos(r);
				double vy = Math.Sin(r);

				points.Add(new Point(cx + w2 * vx, cy + w2 * vy));
			}
			points.Add(points[0]);

			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
				points
			))
			{
				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			Board.Strokes.Add(s1);
		}

		#endregion

		#region shape


		bool shapePushed = false;
		Stroke shapeStroke;
		Point shapeStartPoint;
		private void ShapeLineArea_MouseMove(object sender, MouseEventArgs e)
		{
			if (!shapePushed)
				return;

			var p = Mouse.GetPosition(Board);

			if (true)
			{
				var len = (shapeStartPoint - p).Length;
				var ang = Math.Atan2(p.Y - shapeStartPoint.Y, p.X - shapeStartPoint.X);
				const double step = Math.PI / 32;
				const double step2 = step / 2;

				Debug.WriteLine("angle " + ang);

				for (double c = step2 - Math.PI; c < Math.PI + step; c += step)
				{
					if (ang < c)
					{
						ang = c - step2;
						break;
					}
				}

				var dx = Math.Cos(ang) * len;
				var dy = Math.Sin(ang) * len;
				p = shapeStartPoint + new Vector(dx, dy);
				
			}

			shapeStroke.StylusPoints[1] = new StylusPoint(p.X, p.Y);
		}

		private void ShapeLineArea_MouseDown(object sender, MouseButtonEventArgs e)
		{
			shapePushed = true;
			var center = Mouse.GetPosition(Board);
			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
				new List<Point>{ center, center}
			))
			{
				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			shapeStroke = s1;
			shapeStartPoint = center;

			Board.Strokes.Add(s1);
		}

		private void ShapeLineArea_MouseUp(object sender, MouseButtonEventArgs e)
		{
			shapePushed = false;
		}
		private void ShapeLineArea_MouseLeave(object sender, MouseEventArgs e)
		{
			shapePushed = false;
		}

		#endregion


	}
}
