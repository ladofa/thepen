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
	/// Interaction logic for NumericTextBox.xaml
	/// </summary>
	public partial class NumericTextBox : TextBox
	{
		public NumericTextBox()
		{
			InitializeComponent();
			TextChanged += NumericTextBox_TextChanged;
            LostFocus += NumericTextBox_LostFocus;
		}

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Value < Min)
			{
				Value = Min;
			}

			if (Value > Max) { Value = Max;}
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (Text == "-")
				return;

			bool succeed = Double.TryParse(Text, out double result);
			if (!succeed)
			{
				Text = "";
			}
		}

		public double Value
		{
			get
			{
				if (Text == "" || Text == "-")
				{
					return 0;
				}
				else
				{
					return Double.Parse(Text);
				}
			}

			set
			{
				Text = value.ToString();
			}
		}

		double min = Double.MinValue;
		public double Min
		{
			get { return min; } set {  min = value; }
		}

		double max = Double.MaxValue;
		public double Max
		{
			get => max; set { max = value; }
		}
	}
}
