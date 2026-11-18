using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace ThePen
{
	/// <summary>
	/// Stroke history of one board.
	/// Every change of the stroke collection is kept as one step that can be taken back and replayed.
	/// </summary>
	internal class UndoReDo
	{
		const int Capacity = 1000;

		readonly record struct Step(StrokeCollection Added, StrokeCollection Removed);

		readonly List<Step> done = new();
		readonly List<Step> undone = new();

		//true while this class itself is changing the strokes
		bool replaying;

		public InkCanvas Board;

		public UndoReDo(InkCanvas board)
		{
			Board = board;
		}

		public void Strokes_StrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
		{
			if (replaying) return;

			done.Add(new Step(e.Added, e.Removed));
			if (done.Count > Capacity)
			{
				done.RemoveAt(0);
			}

			//a new change makes the old redo steps meaningless
			undone.Clear();
		}

		public void Undo(object sender, RoutedEventArgs e)
		{
			if (done.Count == 0) return;

			Step step = done[^1];
			done.RemoveAt(done.Count - 1);
			Replace(step.Added, step.Removed);
			undone.Add(step);
		}

		public void Redo(object sender, RoutedEventArgs e)
		{
			if (undone.Count == 0) return;

			Step step = undone[^1];
			undone.RemoveAt(undone.Count - 1);
			Replace(step.Removed, step.Added);
			done.Add(step);
		}

		private void Replace(StrokeCollection takeOut, StrokeCollection putIn)
		{
			replaying = true;
			try
			{
				Board.Strokes.Remove(takeOut);
				Board.Strokes.Add(putIn);
			}
			finally
			{
				replaying = false;
			}
		}
	}
}
