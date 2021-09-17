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

namespace ThePen
{
	/// <summary>
	/// Interaction logic for PenRepresenter.xaml
	/// </summary>
	public partial class PenRepresenter : UserControl
	{
		public PenRepresenter()
		{
			InitializeComponent();
		}

		public System.Windows.Ink.DrawingAttributes Value
		{ 
			set
			{
				Dot.Fill = new SolidColorBrush(value.Color);
				Dot.Width = value.Width * 3;
				Dot.Height = value.Height * 3;
				Dot.RenderTransform = new MatrixTransform(value.StylusTipTransform);
			}
		}
	}
}
