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

namespace ThePen
{


	/// <summary>
	/// Interaction logic for Setting.xaml
	/// </summary>
	public partial class Setting : Window
	{
		public Setting()
		{
			InitializeComponent();
		}

		public SettingData Value
		{
			set
			{
				try
				{
					SettingPen1.Value = value.Pen1;
					SettingPen2.Value = value.Pen2;
					SettingPen3.Value = value.Pen3;

					SettingPen1.SecondColor = value.Qen1;
                    SettingPen2.SecondColor = value.Qen2;
                    SettingPen3.SecondColor = value.Qen3;



                    Palette1.Value = value.Palette1;
                    Palette2.Value = value.Palette2;
                    Palette3.Value = value.Palette3;
                    Palette4.Value = value.Palette4;
                    Palette5.Value = value.Palette5;
                    Palette6.Value = value.Palette6;

                    Palette1.SecondValue = value.Qalette1;
					Palette2.SecondValue = value.Qalette2;
					Palette3.SecondValue = value.Qalette3;
					Palette4.SecondValue = value.Qalette4;
					Palette5.SecondValue = value.Qalette5;
					Palette6.SecondValue = value.Qalette6;

                    HotKeyBoard1.Value = value.HotBoard1;
                    HotKeyBoard2.Value = value.HotBoard2;
                    HotKeyBoard3.Value = value.HotBoard3;

                    HotKeyColor1.Value = value.HotColor1;
					HotKeyColor2.Value = value.HotColor2;
					HotKeyColor3.Value = value.HotColor3;
					HotKeyColor4.Value = value.HotColor4;
					HotKeyColor5.Value = value.HotColor5;
					HotKeyColor6.Value = value.HotColor6;
                    HotKeySwapPalette.Value = value.HotSwapPalette;

                    //HotKeyStampX.Value = value.HotStampX;
                    //HotKeyStampO.Value = value.HotStampO;
                    //HotKeyStampTri.Value = value.HotStampTri;
                    //HotKeyStampDot.Value = value.HotStampDot;
                    HotKeyErase.Value = value.HotErase;
					HotKeyOverlay1.Value = value.HotOverlay1;
					HotKeyOverlay2.Value = value.HotOverlay2;
					HotKeyOverlay3.Value = value.HotOverlay3;
					HotKeyOverlay4.Value = value.HotOverlay4;
					HotKeyPen1.Value = value.HotPen1;
					HotKeyPen2.Value = value.HotPen2;
					HotKeyPen3.Value = value.HotPen3;
					HotKeyNormal.Value = value.HotNormal;
                    HotKeyClearNormal.Value = value.HotClearNormal;
                    HotKeyClear.Value = value.HotClear;
					HotKeyUndo.Value = value.HotUndo;

					OneKeyColor1.Value = value.OneColor1;
					OneKeyColor2.Value = value.OneColor2;
					OneKeyColor3.Value = value.OneColor3;
					OneKeyColor4.Value = value.OneColor4;
					OneKeyColor5.Value = value.OneColor5;
					OneKeyColor6.Value = value.OneColor6;
					OneKeySwapPalette.Value = value.OneSwapPalette;
					OneKeyErase.Value = value.OneErase;
					OneKeyOverlay1.Value = value.OneOverlay1;
					OneKeyOverlay2.Value = value.OneOverlay2;
					OneKeyOverlay3.Value = value.OneOverlay3;
					OneKeyOverlay4.Value = value.OneOverlay4;
					OneKeyStampX.Value = value.OneStampX;
					OneKeyStampO.Value = value.OneStampO;
					OneKeyStampTri.Value = value.OneStampTri;
					OneKeyStampDot.Value = value.OneStampDot;
					OneKeyShapeLine1.Value = value.OneShapeLine1;
					OneKeyShapeLine2.Value = value.OneShapeLine2;
					OneKeyShapeEllipse.Value = value.OneShapeEllipse;
					OneKeyShapeRectangle.Value = value.OneShapeRectangle;
					OneKeyShapeGrid.Value = value.OneShapeGrid;
					OneKeyShapeArrow.Value = value.OneShapeArrow;
					OneKeyPen1.Value = value.OnePen1;
					OneKeyPen2.Value = value.OnePen2;
					OneKeyPen3.Value = value.OnePen3;
					OneKeyNormal.Value = value.OneNormal;
					OneKeyClearNormal.Value = value.OneClearNormal;
					OneKeyClear.Value = value.OneClear;
					OneKeyUndo.Value = value.OneUndo;
					ImmeRadioButton.IsChecked = value.OneKeyImme;
					TempRadioButton.IsChecked = !value.OneKeyImme;

					EasySwitch.IsChecked = value.EasySwitch;
					EdgeWidth.Value = value.EdgeWidth;
					ShakeToClearAll.IsChecked = value.ShakeToClearAll;

					BlockTouch.IsChecked = value.BlockTouch;
					ShapeLine1Arrow.IsChecked = value.ShapeLine1Arrow;
					ShapeLine2Arrow.IsChecked = value.ShapeLine2Arrow;

					Display1.IsChecked = value.Display1;
					Display2.IsChecked = value.Display2;
					Display3.IsChecked = value.Display3;
					Display4.IsChecked = value.Display4;

					MouseEffectMove.IsChecked = value.MouseEffectMove;
					MouseEffectLeftDown.IsChecked = value.MouseEffectLeftDown;
					MouseEffectRightDown.IsChecked = value.MouseEffectRightDown;

					var screens = Monitors.GetScreens();
					if (screens.Count < 4)
					{
						Display4.IsEnabled = false;
					}

					if (screens.Count < 3)
					{
						Display3.IsEnabled = false;
					}

					if (screens.Count < 2)
					{
						Display2.IsEnabled = false;
					}

					StampWidth.Value = value.StampWidth;
					ArrowWidth.Value = value.ArrowWidth;
					EraserSize.Value = value.EraserSize;
				}
				catch(Exception e)
				{
					MessageBox.Show("Error occured while load settings from registry.");
				}
			}

			get => new()
			{
				Pen1 = SettingPen1.Value,
				Pen2 = SettingPen2.Value,
				Pen3 = SettingPen3.Value,

				Qen1 = SettingPen1.SecondColor,
                Qen2 = SettingPen2.SecondColor,
                Qen3 = SettingPen3.SecondColor,

                Palette1 = Palette1.Value,
				Palette2 = Palette2.Value,
				Palette3 = Palette3.Value,
				Palette4 = Palette4.Value,
				Palette5 = Palette5.Value,
				Palette6 = Palette6.Value,

                Qalette1 = Palette1.SecondValue,
                Qalette2 = Palette2.SecondValue,
                Qalette3 = Palette3.SecondValue,
                Qalette4 = Palette4.SecondValue,
                Qalette5 = Palette5.SecondValue,
                Qalette6 = Palette6.SecondValue,

                HotBoard1 = HotKeyBoard1.Value,
                HotBoard2 = HotKeyBoard2.Value,
                HotBoard3 = HotKeyBoard3.Value,

				HotColor1 = HotKeyColor1.Value,
				HotColor2 = HotKeyColor2.Value,
				HotColor3 = HotKeyColor3.Value,
				HotColor4 = HotKeyColor4.Value,
				HotColor5 = HotKeyColor5.Value,
				HotColor6 = HotKeyColor6.Value,
				HotSwapPalette = HotKeySwapPalette.Value,
				//HotStampX = HotKeyStampX.Value,
				//HotStampO = HotKeyStampO.Value,
				//HotStampTri = HotKeyStampTri.Value,
				//HotStampDot = HotKeyStampDot.Value,
				HotErase = HotKeyErase.Value,
				HotOverlay1 = HotKeyOverlay1.Value,
				HotOverlay2 = HotKeyOverlay2.Value,
				HotOverlay3 = HotKeyOverlay3.Value,
				HotOverlay4 = HotKeyOverlay4.Value,
				HotPen1 = HotKeyPen1.Value,
				HotPen2 = HotKeyPen2.Value,
				HotPen3 = HotKeyPen3.Value,
				HotNormal = HotKeyNormal.Value,
                HotClearNormal = HotKeyClearNormal.Value,
                HotClear = HotKeyClear.Value,
				HotUndo = HotKeyUndo.Value,

				OneColor1 = OneKeyColor1.Value,
				OneColor2 = OneKeyColor2.Value,
				OneColor3 = OneKeyColor3.Value,
				OneColor4 = OneKeyColor4.Value,
				OneColor5 = OneKeyColor5.Value,
				OneColor6 = OneKeyColor6.Value,
				OneSwapPalette = OneKeySwapPalette.Value,
				OneErase = OneKeyErase.Value,
				OneOverlay1 = OneKeyOverlay1.Value,
				OneOverlay2 = OneKeyOverlay2.Value,
				OneOverlay3 = OneKeyOverlay3.Value,
				OneOverlay4 = OneKeyOverlay4.Value,
				OneStampX = OneKeyStampX.Value,
				OneStampO = OneKeyStampO.Value,
				OneStampTri = OneKeyStampTri.Value,
				OneStampDot = OneKeyStampDot.Value,
				OneShapeLine1 = OneKeyShapeLine1.Value,
				OneShapeLine2 = OneKeyShapeLine2.Value,
				OneShapeEllipse = OneKeyShapeEllipse.Value,
				OneShapeRectangle = OneKeyShapeRectangle.Value,
				OneShapeGrid = OneKeyShapeGrid.Value,
				OneShapeArrow = OneKeyShapeArrow.Value,
				OnePen1 = OneKeyPen1.Value,
				OnePen2 = OneKeyPen2.Value,
				OnePen3 = OneKeyPen3.Value,
				OneNormal = OneKeyNormal.Value,
				OneClearNormal = OneKeyClearNormal.Value,
				OneClear = OneKeyClear.Value,
				OneUndo = OneKeyUndo.Value,
				OneKeyImme = ImmeRadioButton.IsChecked.Value,
				EasySwitch = EasySwitch.IsChecked.Value,
				EdgeWidth = EdgeWidth.Value,
				ShakeToClearAll = ShakeToClearAll.IsChecked.Value,
				BlockTouch = BlockTouch.IsChecked.Value,
				ShapeLine1Arrow = ShapeLine1Arrow.IsChecked.Value,
				ShapeLine2Arrow = ShapeLine2Arrow.IsChecked.Value,
				Display1 = Display1.IsChecked.Value,
				Display2 = Display2.IsChecked.Value,
				Display3 = Display3.IsChecked.Value,
				Display4 = Display4.IsChecked.Value,
				MouseEffectMove = MouseEffectMove.IsChecked.Value,
				MouseEffectLeftDown = MouseEffectLeftDown.IsChecked.Value,
				MouseEffectRightDown = MouseEffectRightDown.IsChecked.Value,
				StampWidth = StampWidth.Value,
				ArrowWidth = ArrowWidth.Value,
				EraserSize = EraserSize.Value,
			};
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (exitWithoutDialog) return;

			var result = MessageBox.Show("You may lose not saved data.", "Warning", MessageBoxButton.OKCancel);
			if (result != MessageBoxResult.OK)
			{
				e.Cancel = true;
			}
		}

		bool exitWithoutDialog = false;

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			exitWithoutDialog = true;
			Close();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			Global.SettingData = Value;
			Global.SaveToReg();
			exitWithoutDialog = true;
			Close();
		}

		private void DefaultButton_Click(object sender, RoutedEventArgs e)
		{
			Value = new SettingData();
		}

	}
}
