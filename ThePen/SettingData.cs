using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace ThePen
{
	public class SettingData
	{

		public DrawingAttributes Pen1 = new DrawingAttributes()
		{
			Color = Color.FromArgb(255, 255, 49, 152),
			Width = 2,
			Height = 2,
		};

		public DrawingAttributes Pen2 = new DrawingAttributes()
		{
			Color = Color.FromArgb(255, 128, 255, 0),
			Width = 4,
			Height = 10,
			IsHighlighter = true,
			StylusTipTransform = Global.GetMatrix(30),
		};

		public DrawingAttributes Pen3 = new DrawingAttributes()
		{
			Color = Color.FromArgb(255, 0x49, 0xcf, 0xd7),
			Width = 3.5,
			Height = 3.5,
		};

		public Color Palette1 = Colors.Red;
		public Color Palette2 = Colors.Orange;
		public Color Palette3 = Colors.Olive;
		public Color Palette4 = Colors.Green;
		public Color Palette5 = Colors.SkyBlue;
		public Color Palette6 = Colors.Violet;

		public bool Screen1 = false;
		public bool Screen2 = true;
		public bool Screen3 = true;
		public bool Screen4 = true;

		public bool MouseEffectMove = false;
		public bool MouseEffectLeftDown = true;
		public bool MouseEffectRightDown = true;

		public bool OneKeyImme = false;

		public bool EasySwitch = false;
		public int EasySwitchEdgeWidth = 5;

		public (uint, uint) HotErase = (0, 0);
		public (uint, uint) HotSelect = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["2"]);
		public (uint, uint) HotClear = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["7"]);
		public (uint, uint) HotPen1 = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["3"]);
		public (uint, uint) HotPen2 = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["4"]);
		public (uint, uint) HotPen3 = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["5"]);

		public (uint, uint) HotColor1 = (0, 0);
		public (uint, uint) HotColor2 = (0, 0);
		public (uint, uint) HotColor3 = (0, 0);
		public (uint, uint) HotColor4 = (0, 0);
		public (uint, uint) HotColor5 = (0, 0);
		public (uint, uint) HotColor6 = (0, 0);

		public (uint, uint) HotOverlay1 = (0, 0);
		public (uint, uint) HotOverlay2 = (0, 0);
		public (uint, uint) HotOverlay3 = (0, 0);
		public (uint, uint) HotOverlay4 = (0, 0);

		public Key OneSelect = Key.None;
		public Key OneErase = Key.R;
		public Key OneClear = Key.T;
		public Key OnePen1 = Key.Q;
		public Key OnePen2 = Key.W;
		public Key OnePen3 = Key.E;

		public Key OneOverlay1 = Key.None;
		public Key OneOverlay2 = Key.None;
		public Key OneOverlay3 = Key.None;
		public Key OneOverlay4 = Key.None;

		public Key OneColor0 = Key.OemTilde;
		public Key OneColor1 = Key.D1;
		public Key OneColor2 = Key.D2;
		public Key OneColor3 = Key.D3;
		public Key OneColor4 = Key.D4;
		public Key OneColor5 = Key.D5;
		public Key OneColor6 = Key.D6;

		//public List<string> InputKeys;

		//---------------------------

	}

}
