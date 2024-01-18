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
using System.Security.Cryptography.Xml;

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
			//KeyboardHook.Start();

			AdoptSetting();

			PreviewTouchDown += Window1_PreviewTouchDown;
			PreviewTouchMove += Window1_PreviewTouchMove;
			PreviewTouchUp += Window1_PreviewTouchUp;


			Overlay1.Apply("overlay1.txt");
			Overlay2.Apply("overlay2.txt");
			Overlay3.Apply("overlay3.txt");
			Overlay4.Apply("overlay4.txt");
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

			MouseEffectErase.Width = setting.EraserSize;
            MouseEffectErase.Height = setting.EraserSize;

            Board.EraserShape = new EllipseStylusShape(setting.EraserSize, setting.EraserSize);
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

			//arrow key exception
            if (e.Key == setting.OneShapeArrow)
            {
                ArrowMode = true;
				return;
            }

            //prevent repeating
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
			else if (e.Key == setting.OneUndo)
			{
				Undo(null, null);
			}

			else if (e.Key == setting.OneSelect)
			{
				DrawingMode = DrawingModes.Select;
			}
			else if (e.Key == setting.OneStampX)
			{
				setStampMode(0);
			}
			else if (e.Key == setting.OneStampO)
			{
				setStampMode(1);
			}
			else if (e.Key == setting.OneStampTri)
			{
				setStampMode(2);
			}
			else if (e.Key == setting.OneStampDot)
			{
				setStampMode(3);
			}
			else if (e.Key == setting.OneShapeLine1)
			{
				setShapeMode(0);
			}
			else if (e.Key == setting.OneShapeLine2)
			{
				setShapeMode(1);
			}
			else if (e.Key == setting.OneShapeEllipse)
			{
				setShapeMode(2);
			}
			else if (e.Key == setting.OneShapeRectangle)
			{
				setShapeMode(3);
			}
			else if (e.Key == setting.OneShapeGrid)
			{
				setShapeMode(4);
			}
			//pen changes

			else if (e.Key == setting.OneColor1)
			{
				ChangeColor(setting.Palette1);
			}
			else if (e.Key == setting.OneColor2)
			{
				ChangeColor(setting.Palette2);
			}
			else if (e.Key == setting.OneColor3)
			{
				ChangeColor(setting.Palette3);
			}
			else if (e.Key == setting.OneColor4)
			{
				ChangeColor(setting.Palette4);
			}
			else if (e.Key == setting.OneColor5)
			{
				ChangeColor(setting.Palette5);
			}
			else if (e.Key == setting.OneColor6)
			{
				ChangeColor(setting.Palette6);
			}
			else if (e.Key == setting.OnePen1)
			{
				DrawingMode = DrawingModes.Draw;
				ChangeMainPen(1);
			}
			else if (e.Key == setting.OnePen2)
			{
				DrawingMode = DrawingModes.Draw;
				ChangeMainPen(2);
			}
			else if (e.Key == setting.OnePen3)
			{
				DrawingMode = DrawingModes.Draw;
				ChangeMainPen(3);
			}

			

			isPressed = true;
			Global.KeyPressed = true;
		}

		

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (setting.EasySwitch)
			{ 
				DrawingMode = DrawingModes.Select;
			}
			Global.KeyPressed = false;
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyUp(e);

            if (e.Key == setting.OneShapeArrow)
            {
                ArrowMode = false;
				return;
            }

            if (e.Key == setting.OneErase)
			{
				DrawingMode = DrawingModes.Draw;
			}
			else if (e.Key == setting.OneStampX)
			{
				releaseStampMode();
			}
			else if (e.Key == setting.OneStampO)
			{
				releaseStampMode();
			}
			else if (e.Key == setting.OneStampDot)
			{
				releaseStampMode();
			}
			else if (e.Key == setting.OneStampTri)
			{
				releaseStampMode();
			}
			else if (e.Key == setting.OneShapeLine1)
			{
				releaseShapeMode();
			}
			else if (e.Key == setting.OneShapeLine2)
			{
				releaseShapeMode();
			}
			else if (e.Key == setting.OneShapeEllipse)
			{
				releaseShapeMode();
			}
			else if (e.Key == setting.OneShapeRectangle)
			{
				releaseShapeMode();
			}
			else if (e.Key == setting.OneShapeGrid)
			{
				releaseShapeMode();
			}


			

			isPressed = false;
			Global.KeyPressed = false;
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
            var p = new Point(e.Point.x - this.Left, e.Point.y - this.Top);

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

		public void SetDrawingArea(Rect rectWindow, Rect rectBoard)
		{
			GridBoard.Margin = new Thickness() { Left = rectBoard.X - rectWindow.X, Top = rectBoard.Y - rectWindow.Y };
			GridBoard.Width = rectBoard.Width;
			GridBoard.Height = rectBoard.Height;
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
					Board.EditingMode = InkCanvasEditingMode.None;
					Board.EditingMode = InkCanvasEditingMode.EraseByStroke;
					MouseEffectErase.Visibility = Visibility.Visible;
				}
				else
				{
					Board.EditingMode = InkCanvasEditingMode.Ink;
					MouseEffectErase.Visibility = Visibility.Collapsed;
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

				this.Activate();
				Board.Focus();

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

		int stampKind = 0;
		private void setStampMode(int stampKind)
		{
			this.stampKind = stampKind;
			StampeArea.Visibility = Visibility.Visible;
		}

		private void releaseStampMode()
		{
			StampeArea.Visibility = Visibility.Hidden;
		}

		private void StampeArea_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (stampKind == 0)
			{
				StampX();
			}
			else if (stampKind == 1)
			{
				StampO();
			}
			else if (stampKind == 2)
			{
				StampDot();
			}
			else
			{
				StampTri();
			}
		}

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
			for (double r = Math.PI / 6; r < Math.PI * 2; r += Math.PI / 1.5)
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


		void setShapeMode(int shapeKind)
		{
			if (shapeKind == 0)
			{
				ShapeLineArea.Visibility = Visibility.Visible;
			}
			else if (shapeKind == 1)
			{
				ShapeLine2Area.Visibility = Visibility.Visible;
			}
			else if (shapeKind == 2)
			{
				ShapeEllipseArea.Visibility = Visibility.Visible;
			}
			else if (shapeKind == 3)
			{
				ShapeRectangleArea.Visibility = Visibility.Visible;
			}
			else if (shapeKind == 4)
			{
				ShapeGridArea.Visibility = Visibility.Visible;
				BoardTemp.Visibility = Visibility.Visible;
				BoardTemp.Strokes.Clear();
				shapeGridCopyMode = false;
			}
		}

		void releaseShapeMode()
		{
			ShapeLineArea.Visibility = Visibility.Collapsed;
			ShapeLine2Area.Visibility = Visibility.Collapsed;
			ShapeEllipseArea.Visibility = Visibility.Collapsed;
			ShapeRectangleArea.Visibility = Visibility.Collapsed;
			ShapeGridArea.Visibility = Visibility.Collapsed;
			BoardTemp.Visibility = Visibility.Collapsed;

			foreach (var stroke in BoardTemp.Strokes)
			{
				Board.Strokes.Add(stroke);
			}

			BoardTemp.Strokes.Clear();
		}


		bool shapePushed = false;
		Stroke shapeStroke;
		Point shapeStartPoint;

		double shapeLineDx = 0;
		double shapeLineDy = 0;
		Point shapeEndPoint;
		private void ShapeLineArea_MouseMove(object sender, MouseEventArgs e)
		{
			if (!shapePushed)
				return;

			var p = Mouse.GetPosition(Board);

			if (true)
			{
				var len = (shapeStartPoint - p).Length;
				var ang = Math.Atan2(p.Y - shapeStartPoint.Y, p.X - shapeStartPoint.X);
				const double step = Math.PI / 8;
				const double step2 = step / 2;


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

				//for arrow
				shapeLineDx = dx;
                shapeLineDy = dy;
				shapeEndPoint = p;
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
			if (ArrowMode || setting.ShapeLine1Arrow)
			{
				drawArrow(shapeEndPoint, shapeLineDx, shapeLineDy);
			}
		}
		private void ShapeLineArea_MouseLeave(object sender, MouseEventArgs e)
		{
			shapePushed = false;
		}

		//----


		private void ShapeLine2Area_MouseMove(object sender, MouseEventArgs e)
		{
			if (!shapePushed)
				return;

			var p = Mouse.GetPosition(Board);

			shapeStroke.StylusPoints[1] = new StylusPoint(p.X, p.Y);
        }

		private void ShapeLine2Area_MouseDown(object sender, MouseButtonEventArgs e)
		{

			shapePushed = true;
			var center = Mouse.GetPosition(Board);
			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
				new List<Point> { center, center }
			))
			{
				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			shapeStroke = s1;
			shapeStartPoint = center;

			Board.Strokes.Add(s1);
		}

		private void ShapeLine2Area_MouseUp(object sender, MouseButtonEventArgs e)
		{
			shapePushed = false;

			if (ArrowMode || setting.ShapeLine2Arrow)
			{
				var p0 = shapeStroke.StylusPoints[0];
				var p1 = shapeStroke.StylusPoints[1];

				var dx = (p1.X - p0.X);
				var dy = (p1.Y - p0.Y);
				drawArrow(new Point(p1.X, p1.Y), dx, dy);
            }
        }

		private void ShapeLine2Area_MouseLeave(object sender, MouseEventArgs e)
		{
			shapePushed = false;
		}

		//----

		List<double> ellipseVectorX;
		List<double> ellipseVectorY;

		private void ShapeEllipseArea_MouseMove(object sender, MouseEventArgs e)
		{
			if (!shapePushed)
				return;

			var p = Mouse.GetPosition(Board);
			var r = ((Vector)(p - shapeStartPoint)).Length / 2;

			List<Point> points = new();
			double pi2 = Math.PI * 2;
			double step = Math.PI / 32;
			double sx = (shapeStartPoint.X + p.X) / 2;
			double sy = (shapeStartPoint.Y + p.Y) / 2;

			if (ellipseVectorX == null)
			{
				ellipseVectorX = new();
				ellipseVectorY = new();
				for (double rad = 0; rad < Math.PI * 2; rad += step)
				{
					ellipseVectorX.Add(Math.Cos(rad));
					ellipseVectorY.Add(Math.Sin(rad));
				}
			}

			for (int i = 0; i < ellipseVectorX.Count; i++)
			{
				points.Add(new Point(ellipseVectorX[i] * r + sx, ellipseVectorY[i] * r + sy));
			}

			points.Add(points[0]);
			shapeStroke.StylusPoints = new StylusPointCollection(points);
		}

		private void ShapeEllipseArea_MouseDown(object sender, MouseButtonEventArgs e)
		{
			shapePushed = true;
			var center = Mouse.GetPosition(Board);

			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
				new List<Point> { center, center }
			))
			{
				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			shapeStroke = s1;
			shapeStartPoint = center;
			Board.Strokes.Add(s1);
		}

		private void ShapeEllipseArea_MouseUp(object sender, MouseButtonEventArgs e)
		{
			shapePushed = false;
		}

		private void ShapeEllipseArea_MouseLeave(object sender, MouseEventArgs e)
		{
			shapePushed = false;
		}

		//----

		private void ShapeRectangleArea_MouseMove(object sender, MouseEventArgs e)
		{
			if (!shapePushed)
				return;

			var p = Mouse.GetPosition(Board);

			double x1 = p.X;
			double y1 = p.Y;
			double x2 = shapeStartPoint.X;
			double y2 = shapeStartPoint.Y;

			List<Point> points = new()
			{
				new Point(x1, y1),
				new Point(x2, y1),
				new Point(x2, y2),
				new Point(x1, y2)
			};
			points.Add(points[0]);

			shapeStroke.StylusPoints = new StylusPointCollection(points);
		}

		private void ShapeRectangleArea_MouseDown(object sender, MouseButtonEventArgs e)
		{
			shapePushed = true;
			var center = Mouse.GetPosition(Board);

			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
				new List<Point> { center, center }
			))
			{
				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			shapeStroke = s1;
			shapeStartPoint = center;
			Board.Strokes.Add(s1);
		}

		private void ShapeRectangleArea_MouseUp(object sender, MouseButtonEventArgs e)
		{
			shapePushed = false;
		}

		private void ShapeRectangleArea_MouseLeave(object sender, MouseEventArgs e)
		{
			shapePushed = false;
		}

		//-----

		bool shapeGridCopyMode;

		List<List<Point>> shapeGridGetStrokes(int gx1, int gy1, int gx2, int gy2)
		{
			List<List<Point>> strokes = new();
			double sx = shapeStartPoint.X;
			double sy = shapeStartPoint.Y;
			double left = gx1 * shapeGridW + sx;
			double top = gy1 * shapeGridH + sy;
			double right = (gx2 + 1) * shapeGridW + sx;
			double bottom = (gy2 + 1) * shapeGridH + sy;

			for (int gx = gx1; gx <= gx2 + 1; gx++)
			{
				double x = gx * shapeGridW + sx;
				strokes.Add(new List<Point>
				{
					new Point(x, top),
					new Point(x, bottom)
				});
			}

			for (int gy = gy1; gy <= gy2 + 1; gy++)
			{
				double y = gy * shapeGridH + sy;
				strokes.Add(new List<Point>
				{
					new Point(left, y),
					new Point(right, y)
				});
			}

			return strokes;
		}

		private void ShapeGridArea_MouseMove(object sender, MouseEventArgs e)
		{
			Debug.WriteLine(shapePushed + ", " + shapeGridCopyMode + "," + shapeStartPoint);
			if (shapeGridCopyMode)
			{
				//check current area
				var p = Mouse.GetPosition(Board);
				var v = p - shapeStartPoint;

				int gx = (int)(v.X / shapeGridW);
				int gy = (int)(v.Y / shapeGridH);

				if (gx != shapeGridCurX || gy != shapeGridCurY)
				{
					BoardTemp.Strokes.Clear();

					if (v.X < 0) gx += -1;
					if (v.Y < 0) gy += -1;

					int gx1 = Math.Min(gx, 0);
					int gx2 = Math.Max(gx, 0);
					int gy1 = Math.Min(gy, 0);
					int gy2 = Math.Max(gy, 0);

					var strokes = shapeGridGetStrokes(gx1, gy1, gx2, gy2);
					foreach (var stroke in strokes)
					{
						var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
						stroke
						))
						{
							DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
						};
						BoardTemp.Strokes.Add(s1);
					}
				}
			}
			else if (shapePushed)
			{
				var p = Mouse.GetPosition(Board);

				double x1 = p.X;
				double y1 = p.Y;
				double x2 = shapeStartPoint.X;
				double y2 = shapeStartPoint.Y;

				List<Point> points = new()
				{
					new Point(x1, y1),
					new Point(x2, y1),
					new Point(x2, y2),
					new Point(x1, y2)
				};
				points.Add(points[0]);

				Debug.WriteLine($"{x1}, {y1}, {x2}, {y2}");

				shapeStroke.StylusPoints = new StylusPointCollection(points);
			}

		}

		private void ShapeGridArea_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (shapeGridCopyMode)
			{
				foreach (var stroke in BoardTemp.Strokes)
				{
					Board.Strokes.Add(stroke);
				}

				BoardTemp.Strokes.Clear();

				return;
			}

			shapePushed = true;
			shapeGridCopyMode = false;

			var center = Mouse.GetPosition(Board);

			var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
				new List<Point> { center, center }
			))
			{
				DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
			};

			shapeStroke = s1;
			shapeStartPoint = center;
			BoardTemp.Strokes.Add(s1);
		}

		double shapeGridW;
		double shapeGridH;
		int shapeGridCurX;
		int shapeGridCurY;

        private bool ArrowMode;

        private void ShapeGridArea_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (shapeGridCopyMode)
			{
				shapeGridCopyMode = false;
				return;
			}

			shapePushed = false;
			shapeGridCopyMode = true;

			BoardTemp.Strokes.Clear();

			var p = Mouse.GetPosition(Board);

			shapeGridW = Math.Max(Math.Abs(p.X - shapeStartPoint.X), 15);
			shapeGridH = Math.Max(Math.Abs(p.Y - shapeStartPoint.Y), 15);

			shapeStartPoint.X = Math.Min(p.X, shapeStartPoint.X);
			shapeStartPoint.Y = Math.Min(p.Y, shapeStartPoint.Y);

			shapeGridCurX = -10000000;
			shapeGridCurY = -10000000;
		}

		private void ShapeGridArea_MouseLeave(object sender, MouseEventArgs e)
		{
			shapePushed = false;
			shapeGridCopyMode = false;
		}

		#endregion

		#region overlay

		public void ToggleOverlay1()
		{

			if (Overlay1.Visibility == Visibility.Collapsed)
			{
				Overlay1.Apply("overlay1.txt");
				Overlay1.Visibility = Visibility.Visible;
				Overlay1.Show();
			}
			else
			{
				Overlay1.Visibility = Visibility.Collapsed;
			}
		}

		public void ToggleOverlay2()
		{
			if (Overlay2.Visibility == Visibility.Collapsed)
			{
				
				Overlay2.Apply("overlay2.txt");
				Overlay2.Visibility = Visibility.Visible;
				Overlay2.Show();
			}
			else
			{
				Overlay2.Visibility = Visibility.Collapsed;
			}
		}

		public void ToggleOverlay3()
		{
			if (Overlay3.Visibility == Visibility.Collapsed)
			{
				
				Overlay3.Apply("overlay3.txt");
				Overlay3.Visibility = Visibility.Visible;
				Overlay3.Show();
			}
			else
			{
				Overlay3.Visibility = Visibility.Collapsed;
			}
		}

		public void ToggleOverlay4()
		{
			if (Overlay4.Visibility == Visibility.Collapsed)
			{
				
				Overlay4.Apply("overlay4.txt");
				Overlay4.Visibility = Visibility.Visible;
				Overlay4.Show();
			}
			else
			{
				Overlay4.Visibility = Visibility.Collapsed;
			}
		}



        #endregion

		private void drawArrow(Point center, double dx = 0, double dy = 0)
		{
			if (dx == 0 && dy == 0)
			{
                dx = MouseHook.dx;
                dy = MouseHook.dy;
            }
            
            double rad = Math.Atan2(dy, dx);
            double cx = center.X;
            double cy = center.Y;

			double size = setting.ArrowWidth;

            //-135
            double leftWingX = Math.Cos(rad - Math.PI * 0.75) * size + cx;
            double leftWingY = Math.Sin(rad - Math.PI * 0.75) * size + cy;
            Point leftWing = new Point(leftWingX, leftWingY);

            var s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
                new List<Point> { center, leftWing }
            ))
            {
                DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
            };

            Board.Strokes.Add(s1);

            //+135
            double rightWingX = Math.Cos(rad + Math.PI * 0.75) * size + cx;
            double rightWingY = Math.Sin(rad + Math.PI * 0.75) * size + cy;
            Point rightWing = new Point(rightWingX, rightWingY);

            s1 = new System.Windows.Ink.Stroke(new StylusPointCollection(
                new List<Point> { center, rightWing }
            ))
            {
                DrawingAttributes = Board.DefaultDrawingAttributes.Clone()
            };

            Board.Strokes.Add(s1);
        }

        private void Board_MouseUp(object sender, MouseButtonEventArgs e)
        {
			if (ArrowMode)
			{
                var center = Mouse.GetPosition(Board);
				drawArrow(center);
            }
        }

   

        private void Board_StylusDown(object sender, StylusDownEventArgs e)
        {
			if (e.Inverted)
			{
                MouseEffectErase.Visibility = Visibility.Visible;
            }
        }

        private void Board_StylusUp(object sender, StylusEventArgs e)
        {
			if (e.Inverted)
			{
				MouseEffectErase.Visibility = Visibility.Hidden;
			}
        }

        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(this);
            Canvas.SetLeft(MouseEffectEraseGroup, p.X - 100);
            Canvas.SetTop(MouseEffectEraseGroup, p.Y - 100);
        }
    }
}
