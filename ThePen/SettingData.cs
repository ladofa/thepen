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
			Color = Color.FromArgb(0xff, 0xdc, 0x8c, 0x49),
			Width = 3.5,
			Height = 1.5,
		};

		public DrawingAttributes Pen2 = new DrawingAttributes()
		{
			Color = Color.FromArgb(255, 0xb7, 0xff, 0),
			Width = 4,
			Height = 10,
			IsHighlighter = true,
			IgnorePressure = true,
			StylusTipTransform = Global.GetMatrix(30),
		};

		public DrawingAttributes Pen3 = new DrawingAttributes()
		{
			Color = Color.FromArgb(255, 0x0b, 0x37, 0x39),
			Width = 1,
			Height = 1,
			FitToCurve = true,
		};

		public Color Palette1 = Color.FromArgb(255, 0xe4, 0x5c, 0x76);
		public Color Palette2 = Color.FromArgb(255, 0xcc, 0xb3, 0x21);
		public Color Palette3 = Color.FromArgb(255, 0x69, 0x9d, 0x00);
		public Color Palette4 = Color.FromArgb(255, 0x46, 0xaa, 0xaa);
		public Color Palette5 = Color.FromArgb(255, 0x70, 0x76, 0xbd);
		public Color Palette6 = Color.FromArgb(255, 0xb0, 0x6d, 0xb9);

		public bool Display1 = true;
		public bool Display2 = true;
		public bool Display3 = true;
		public bool Display4 = true;

		public bool MouseEffectMove = false;
		public bool MouseEffectLeftDown = false;
		public bool MouseEffectRightDown = false;

		public bool OneKeyImme = true;

		public bool BlockTouch = true;

		public bool EasySwitch = true;
		public double EdgeWidth = 5;

		public bool ShakeToClearAll = true;

		public double StampWidth = 15;

		public (uint, uint) HotErase = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["1"]);
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

		public (uint, uint) HotStampX = (0, 0);
		public (uint, uint) HotStampO = (0, 0);
		public (uint, uint) HotStampTri = (0, 0);
		public (uint, uint) HotStampDot = (0, 0);

		public Key OneSelect = Key.Escape;
		public Key OneErase = Key.R;
		public Key OneClear = Key.OemTilde;
		public Key OnePen1 = Key.Q;
		public Key OnePen2 = Key.W;
		public Key OnePen3 = Key.E;

		public Key OneOverlay1 = Key.None;
		public Key OneOverlay2 = Key.None;
		public Key OneOverlay3 = Key.None;
		public Key OneOverlay4 = Key.None;

		public Key OneColor1 = Key.D1;
		public Key OneColor2 = Key.D2;
		public Key OneColor3 = Key.D3;
		public Key OneColor4 = Key.D4;
		public Key OneColor5 = Key.D5;
		public Key OneColor6 = Key.D6;

		public Key OneStampX = Key.X;
		public Key OneStampO = Key.C;
		public Key OneStampTri = Key.V;
		public Key OneStampDot = Key.B;

		public Key OneShapeLine1 = Key.A;
		public Key OneShapeLine2 = Key.S;
		public Key OneShapeEllipse = Key.D;
		public Key OneShapeRectangle = Key.F;
		public Key OneShapeGrid = Key.G;

		//public List<string> InputKeys;

		//---------------------------

	}

}
