﻿using System;
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
	/// Interaction logic for SimpleColorPicker.xaml
	/// </summary>
	public partial class SettingColor : UserControl
	{
		public SettingColor()
		{
			InitializeComponent();
		}

		public Color Value
		{
			get => new()
			{
				A = (byte)Picker.Color.A,
				R = (byte)Picker.Color.RGB_R,
				G = (byte)Picker.Color.RGB_G,
				B = (byte)Picker.Color.RGB_B,

			};

			set
			{
				Picker.Color.A = value.A;
				Picker.Color.RGB_R = value.R;
				Picker.Color.RGB_G = value.G;
				Picker.Color.RGB_B = value.B;
            }
		}

		public Color SecondValue
		{
            get => new()
            {
                A = (byte)Picker.SecondColor.A,
                R = (byte)Picker.SecondColor.RGB_R,
                G = (byte)Picker.SecondColor.RGB_G,
                B = (byte)Picker.SecondColor.RGB_B,

            };

            set
            {
                Picker.SecondColor.A = value.A;
                Picker.SecondColor.RGB_R = value.R;
                Picker.SecondColor.RGB_G = value.G;
                Picker.SecondColor.RGB_B = value.B;
            }
        }
	}
}
