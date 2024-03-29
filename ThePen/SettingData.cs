﻿using System;
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

		public DrawingAttributes PenErase = new DrawingAttributes()
		{
			Color = Color.FromArgb(255, 0xb7, 0xff, 0),
			Width = 30,
			Height = 30,
			IgnorePressure = true,
		};

		//bright
		//# FF FB 50 5E
		//#FFFFAC16
		//#FFDCDC00
		//# FF 53 A3 00
		//# FF 03 A7 B9
		//# FF 6C 7A E1
		//# FF B5 55 D8

		//static public Color FromText(string text)
		//{
		//	text = text.Replace("#", "");
		//	byte a = Byte.Parse(text[0..2], System.Globalization.NumberStyles.HexNumber);
		//	byte r = Byte.Parse(text[2..4], System.Globalization.NumberStyles.HexNumber);
		//	byte g = Byte.Parse(text[4..6], System.Globalization.NumberStyles.HexNumber);
		//	byte b = Byte.Parse(text[6..8], System.Globalization.NumberStyles.HexNumber);
		//	return Color.FromArgb(a, r, g, b);
		//}

		//public DrawingAttributes Pen1 = new DrawingAttributes()
		//{
		//	Color = Color.FromArgb(0xff, 0xfb, 0x50, 0x5e),
		//	Width = 3.5,
		//	Height = 1.5,
		//};

		//public Color Palette1 = FromText("FFFFAC16");
		//public Color Palette2 = FromText("#FFDCDC00");
		//public Color Palette3 = Color.FromArgb(255, 0x53, 0xa3, 0x00);
		//public Color Palette4 = Color.FromArgb(255, 0x03, 0xa7, 0xb9);
		//public Color Palette5 = Color.FromArgb(255, 0x6c, 0x7a, 0xe1);
		//public Color Palette6 = Color.FromArgb(255, 0xb5, 0x55, 0xd8);

        // pastel
        public DrawingAttributes Pen1 = new DrawingAttributes()
        {
            Color = Color.FromArgb(255, 0xb0, 0x6d, 0xb9),
            Width = 3.5,
            Height = 1.5,
        };

        public static Color genColor(string hex)
        {
			hex = hex.Replace("#", "");
			byte a = Convert.ToByte(hex[..2], 16);
            byte r = Convert.ToByte(hex[2..4], 16);
            byte g = Convert.ToByte(hex[4..6], 16);
            byte b = Convert.ToByte(hex[6..], 16);

			return Color.FromArgb(a, r, g, b);

        }


        public Color Qen1 = genColor("#FFCC1CE4");
		public Color Qen2 = Color.FromArgb(255, 0x0b, 0x37, 0x39);
        public Color Qen3 = genColor("#FFFFFFFF");

        public Color Palette1 = Color.FromArgb(255, 0xe4, 0x5c, 0x76);
        public Color Palette2 = Color.FromArgb(0xff, 0xdc, 0x8c, 0x49);
        public Color Palette3 = Color.FromArgb(255, 0xcc, 0xb3, 0x21);
        public Color Palette4 = Color.FromArgb(255, 0x69, 0x9d, 0x00);
        public Color Palette5 = Color.FromArgb(255, 0x46, 0xaa, 0xaa);
        public Color Palette6 = Color.FromArgb(255, 0x70, 0x76, 0xbd);

		public Color Qalette1 = genColor("#FFFF3058");
        public Color Qalette2 = genColor("#FFFF8722");
        public Color Qalette3 = genColor("#FFFFDA00");
        public Color Qalette4 = genColor("#FF82C300");
        public Color Qalette5 = genColor("#FF25D6D6");
		public Color Qalette6 = genColor("#FF3A49FF");


        public bool Display1 = true;
		public bool Display2 = false;
		public bool Display3 = false;
		public bool Display4 = false;

		public bool MouseEffectMove = false;
		public bool MouseEffectLeftDown = false;
		public bool MouseEffectRightDown = false;

		public bool OneKeyImme = true;

		public bool BlockTouch = true;

		public bool EasySwitch = false;
		public double EdgeWidth = 5;

		public bool ShakeToClearAll = true;

		public double StampWidth = 15;
        public double ArrowWidth = 20;

        public bool ShapeLine1Arrow = false;
        public bool ShapeLine2Arrow = false;

        public (uint, uint) HotErase = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["1"]);
		public (uint, uint) HotNormal = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["2"]);
        public (uint, uint) HotClearNormal = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["3"]);
        public (uint, uint) HotClear = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["7"]);
		public (uint, uint) HotUndo = (0, 0);
		public (uint, uint) HotPen1 = (0, Hotkey.TrigKeys["~"]);
		public (uint, uint) HotPen2 = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["4"]);
		public (uint, uint) HotPen3 = (Hotkey.MOD_CTRL + Hotkey.MOD_SHIFT, Hotkey.TrigKeys["5"]);

        public (uint, uint) HotBoard1 = (0, Hotkey.TrigKeys["F1"]);
        public (uint, uint) HotBoard2 = (0, Hotkey.TrigKeys["F2"]);
        public (uint, uint) HotBoard3 = (0, Hotkey.TrigKeys["F3"]);

        public (uint, uint) HotColor1 = (0, 0);
		public (uint, uint) HotColor2 = (0, 0);
		public (uint, uint) HotColor3 = (0, 0);
		public (uint, uint) HotColor4 = (0, 0);
		public (uint, uint) HotColor5 = (0, 0);
		public (uint, uint) HotColor6 = (0, 0);

        public (uint, uint) HotSwapPalette = (0, 0);

        public (uint, uint) HotOverlay1 = (0, 0);
		public (uint, uint) HotOverlay2 = (0, 0);
		public (uint, uint) HotOverlay3 = (0, 0);
		public (uint, uint) HotOverlay4 = (0, 0);

		public (uint, uint) HotStampX = (0, 0);
		public (uint, uint) HotStampO = (0, 0);
		public (uint, uint) HotStampTri = (0, 0);
		public (uint, uint) HotStampDot = (0, 0);

		public Key OneNormal = Key.Escape;
        public Key OneClearNormal = Key.Q;
        public Key OneErase = Key.R;
		public Key OneClear = Key.T;
		public Key OnePen1 = Key.None;
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
        public Key OneSwapPalette = Key.D7;

        public Key OneStampX = Key.X;
		public Key OneStampO = Key.C;
		public Key OneStampTri = Key.V;
		public Key OneStampDot = Key.B;

		public Key OneShapeLine1 = Key.A;
		public Key OneShapeLine2 = Key.S;
		public Key OneShapeEllipse = Key.D;
		public Key OneShapeRectangle = Key.F;
		public Key OneShapeGrid = Key.G;
        public Key OneShapeArrow = Key.LeftShift;
        public Key OneUndo = Key.Z;
        public double EraserSize = 100;

		public void SwapPalette()
		{
            (Palette1, Qalette1) = (Qalette1, Palette1);
			(Palette2,  Qalette2) = (Qalette2, Palette2);
			(Palette3,  Qalette3) = (Qalette3, Palette3);
			(Palette4,  Qalette4) = (Qalette4, Palette4);
			(Palette5,  Qalette5) = (Qalette5, Palette5);
			(Palette6, Qalette6) = (Qalette6, Palette6);

			(Pen1.Color, Qen1) = (Qen1, Pen1.Color);
            (Pen2.Color,  Qen2) = (Qen2, Pen2.Color);
			(Pen3.Color,  Qen3) = (Qen3, Pen3.Color);
        }


        //public List<string> InputKeys;

        //---------------------------

    }

}
