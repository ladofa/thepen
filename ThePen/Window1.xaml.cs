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

			Canvas.DefaultDrawingAttributes = new System.Windows.Ink.DrawingAttributes()
			{
				Color = Colors.Red,
				Width = 10
			};

			SetDrawing(false);


			Canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;

			InputManager.Current.PreProcessInput += Current_PreProcessInput;
		}

		int pp = 0;

		private void Current_PreProcessInput(object sender, PreProcessInputEventArgs e)
		{
			var a = e.PeekInput();
			if (a == null)
			{
				Debug.WriteLine("null??");
				return;
			}
			else
			{
				Debug.WriteLine(a.ToString());
				Debug.WriteLine(pp.ToString());
				pp++;
			}

		}


		//private void Window_SourceInitialized(object sender, EventArgs e)
		//{
		//    base.OnSourceInitialized(e);
		//    var hwnd = new WindowInteropHelper(this).Handle;
		//    WindowsService.SetWindowExTransparent(hwnd);
		//}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);

			if (Keyboard.IsKeyDown(Key.LeftShift))
			{
				Canvas.DefaultDrawingAttributes = new System.Windows.Ink.DrawingAttributes()
				{
					Width = 100,
					Height = 100
				};
				Canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
			}
			else
			{
				Canvas.DefaultDrawingAttributes = new System.Windows.Ink.DrawingAttributes()
				{
					Color = Colors.Red,
					Width = 5,
					Height = 5,
				};

				Canvas.EditingMode = InkCanvasEditingMode.Ink;
			}
		}



		public void SetDrawing(bool val = true)
		{
			if (val)
			{
				Canvas.Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
			}
			else
			{
				Canvas.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			}

			IsHitTestVisible = val;
			Canvas.IsHitTestVisible = val;
			IsEnabled = val;
			Canvas.IsEnabled = val;
			Focusable = val;
			Canvas.Focusable = val;
		}

		public void ClearAll()
		{
			Canvas.Strokes.Clear();

			var s = new System.Windows.Ink.Stroke(new StylusPointCollection(new List<Point> {
				new Point(0, 0), new Point(100, 100)}))
			{

				DrawingAttributes = Canvas.DefaultDrawingAttributes.Clone()
			};

			Canvas.Strokes.Add(s);
		}

		private void Grid_StylusInAirMove(object sender, StylusEventArgs e)
		{
			Debug.WriteLine( "MOVE");
		}

		#region undo/redo
		//thanks to https://stackoverflow.com/questions/6368517/undo-redo-command-stack-for-inkcanvas/52911463#52911463

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
			Canvas.Strokes.Remove(a);
			Canvas.Strokes.Add(r);
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
			Canvas.Strokes.Add(a);
			Canvas.Strokes.Remove(r);
			_added.Add(a);
			_removed.Add(r);
			_added_redo.RemoveAt(_added_redo.Count - 1);
			_removed_redo.RemoveAt(_removed_redo.Count - 1);
			handle = true;
		}
		#endregion
	}
}
