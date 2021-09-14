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
	/// Interaction logic for SettingPen.xaml
	/// </summary>
	public partial class SettingPen : UserControl
	{
		public SettingPen()
		{
			InitializeComponent();
		}

		public System.Windows.Ink.DrawingAttributes Value
		{
			set
			{
				ColorPicker.Color.A = value.Color.A;
				ColorPicker.Color.RGB_R = value.Color.R;
				ColorPicker.Color.RGB_G = value.Color.G;
				ColorPicker.Color.RGB_B = value.Color.B;

				FitToCurve.IsChecked = value.FitToCurve;
				Highlighter.IsChecked = value.IsHighlighter;
				IgnorePresure.IsChecked = value.IgnorePressure;
				Size.Text = value.Width.ToString();

			}

			get
			{
				var color = new Color()
				{
					A = (byte)ColorPicker.Color.A,
					R = (byte)ColorPicker.Color.RGB_R,
					G = (byte)ColorPicker.Color.RGB_G,
					B = (byte)ColorPicker.Color.RGB_B,
				};

				var size = Int32.Parse(Size.Text);


				return new System.Windows.Ink.DrawingAttributes()
				{
					Color = color,
					FitToCurve = FitToCurve.IsChecked.Value,
					IsHighlighter = Highlighter.IsChecked.Value,
					IgnorePressure = IgnorePresure.IsChecked.Value,
					Width = size,
					Height = size,
					StylusTip = System.Windows.Ink.StylusTip.Ellipse
				};
			}
		}

		private void Size_TextChanged(object sender, TextChangedEventArgs e)
		{
			bool succeed = Int32.TryParse(Size.Text, out int result);
			if (!succeed)
			{
				Size.Text = "";
			}
		}
	}
}
