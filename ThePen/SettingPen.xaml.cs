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
				Picker.Value = value.Color;
				FitToCurve.IsChecked = value.FitToCurve;
				Highlighter.IsChecked = value.IsHighlighter;
				IgnorePresure.IsChecked = value.IgnorePressure;
				Width.Text = value.Width.ToString();
				Height.Text = value.Height.ToString();
				Angle.Text = Global.GetAngle(value.StylusTipTransform).ToString();
			}

			get
			{ 
				return new System.Windows.Ink.DrawingAttributes()
				{
					Color = Picker.Value,
					FitToCurve = FitToCurve.IsChecked.Value,
					IsHighlighter = Highlighter.IsChecked.Value,
					IgnorePressure = IgnorePresure.IsChecked.Value,
					Width = double.Parse(Width.Text),
					Height = double.Parse(Height.Text),
					StylusTip = System.Windows.Ink.StylusTip.Ellipse,
					StylusTipTransform = Global.GetMatrix(double.Parse(Angle.Text)),
				};
			}
		}

		private void Size_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;

			bool succeed = Double.TryParse(textBox.Text, out double result);
			if (!succeed)
			{
				textBox.Text = "";
			}
		}
	}
}
