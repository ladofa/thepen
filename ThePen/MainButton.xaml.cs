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
	/// Interaction logic for MainButton.xaml
	/// </summary>
	public partial class MainButton : UserControl
	{
		public MainButton()
		{
			InitializeComponent();
		}

		public UIElement Image
		{
			get
			{
				if (GridContent.Children.Count == 0)
					return null;
				else
					return GridContent.Children[0];
			}

			set
			{
				GridContent.Children.Clear();
				GridContent.Children.Add(value);
			}
		}

		public bool Checked
		{
			get
			{
				return RectChecked.Visibility == Visibility.Visible;
			}

			set
			{
				if (value)
				{
					RectChecked.Visibility = Visibility.Visible;
				}
				else
				{
					RectChecked.Visibility = Visibility.Hidden;
				}
			}
		}



		public event RoutedEventHandler Click;

		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			GridContent.Margin = new Thickness(2, 2, 0, 0);
		}

		private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Click?.Invoke(this, e);
			GridContent.Margin = new Thickness(0);
		}

		private void Grid_MouseEnter(object sender, MouseEventArgs e)
		{
			Hover.Visibility = Visibility.Visible;
		}

		private void Grid_MouseLeave(object sender, MouseEventArgs e)
		{
			Hover.Visibility = Visibility.Hidden;
			GridContent.Margin = new Thickness(0);
		}
	}
}
