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
	/// Interaction logic for SettingOneKey.xaml
	/// </summary>
	public partial class SettingOneKey : UserControl
	{
		public SettingOneKey()
		{
			InitializeComponent();

			TriggerComboBox.ItemsSource = Hotkey.OneKeyStrings;
		}

		public Key Value
		{
			set
			{
				TriggerComboBox.SelectedValue = value.ToString();
			}

			get
			{
				Key key = (Key)Enum.Parse(typeof(Key), TriggerComboBox.SelectedValue as string);
				return key;
			}
		}
	}
}
