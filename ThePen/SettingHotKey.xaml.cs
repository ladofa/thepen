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
    /// Interaction logic for SettingHotKey.xaml
    /// </summary>
    public partial class SettingHotKey : UserControl
    {
        public SettingHotKey()
        {
            //TriggerComboBox.ItemsSource = new List<string>
            //{
            //    "a", "b", "c"
            //};
            InitializeComponent();

            
            TriggerComboBox.ItemsSource = Hotkey.TrigKeyStrings;
        }

        public (uint, uint) Value
		{
            set
            {

                uint mod = value.Item1;
                if ((mod & Hotkey.MOD_CTRL) > 0)
                {
                    CtrlCheckBox.IsChecked = true;
                }

                if ((mod & Hotkey.MOD_ALT) > 0)
                {
                    AltCheckBox.IsChecked = true;
                }

                if ((mod & Hotkey.MOD_SHIFT) > 0)
                {
                    ShiftCheckBox.IsChecked = true;
                }

                if ((mod & Hotkey.MOD_WIN) > 0)
                {
                    WinCheckBox.IsChecked = true;
                }

                string trigStr = Hotkey.TrigKeysInv[value.Item2];
                TriggerComboBox.SelectedValue = trigStr;
            }

            get
			{
                uint mod = 0;
                if (CtrlCheckBox.IsChecked.Value)
				{
                    mod += Hotkey.MOD_CTRL;
				}
                if (AltCheckBox.IsChecked.Value)
                {
                    mod += Hotkey.MOD_ALT;
                }
                if (ShiftCheckBox.IsChecked.Value)
                {
                    mod += Hotkey.MOD_SHIFT;
                }
                if (WinCheckBox.IsChecked.Value)
                {
                    mod += Hotkey.MOD_WIN;
                }

                string trigStr = (string)TriggerComboBox.SelectedValue;
                uint trig = Hotkey.TrigKeys[trigStr];

                return (mod, trig);
			}
		}
    }
}
