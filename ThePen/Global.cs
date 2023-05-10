using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Ink;
using System.Windows.Media;

using System.Windows.Input;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;

namespace ThePen
{
	/*
	Stamp
	::Presets
	Preset1 - Type, Size, Color, 
	Preset2
	Preset3
	Preset4 
	Preset5 


	::Colors

	::Mod Keys
	CTRL - [None, 
	ALT
	SHIFT

	::Overlays
	Overlay1 Text, Background, Margin, TextAlign, Size, Position, Font, Image
	Overlay2
	Overlay3
	Overlay4

	Timer, Text,

	::Covers



	::Hotkeys


	----------------------------

	Size
	Kind
	Color
	Presets
	Overlay

	Menu
		Info
		Window - Min, Horizontal Small, Horizontal Large, Vertical Small, Vertical Large
		Setting

	*/
	public static class Global
	{
		public static double GetAngle(System.Windows.Media.Matrix m)
		{
            return Math.Round(Math.Asin(m.M12) / Math.PI * 180, 0);
        }

		public static Matrix GetMatrix(double angle)
		{
			Matrix m = Matrix.Identity;
			m.Rotate(angle);
			return m;
		}


		static Global()
		{

		}

		public static SettingData SettingData = new SettingData();
		public static int CurrentPen = 1;

		public static bool KeyPressed = false;

		public static void SaveToReg()
		{
			foreach (var prop in typeof(SettingData).GetFields())
			{
				dynamic val = prop.GetValue(SettingData);
				if (val is System.Windows.Ink.DrawingAttributes _drawing)
				{
					Reg.Write(prop.Name, _drawing);
				}
				else if (val is Color _color)
				{
					Reg.Write(prop.Name, _color);
				}
				else if (val is ValueTuple<uint, uint> _key)
				{
					Reg.Write(prop.Name, _key);
				}
				else if (val is uint _uint)
				{
					Reg.Write(prop.Name, _uint);
				}
				else if (val is double _double)
				{
					Reg.Write(prop.Name, _double);
				}
				else if (val is int _int)
				{
					Reg.Write(prop.Name, _int);
				}
				else if (val is bool _bool)
				{
					Reg.Write(prop.Name, _bool);
				}
				else if (val is Key _one)
				{
					Reg.Write(prop.Name, _one);
				}
				else if (val is System.Windows.Ink.DrawingAttributes _pen)
				{
					Reg.Write(prop.Name, _pen);
				}
				else
				{
					System.Windows.MessageBox.Show(val.ToString());
				}
			}

			Reg.Write(nameof(CurrentPen), CurrentPen);

			SettingChanged?.Invoke(null, null);
		}

		public static void LoadFromReg()
		{
			try
			{
				foreach (var prop in typeof(SettingData).GetFields())
				{
					var val = prop.GetValue(SettingData);

					if (val == null)
					{
						System.Windows.MessageBox.Show(prop.Name);
					}
					if (val is System.Windows.Ink.DrawingAttributes _drawing)
					{
						Reg.Read(prop.Name, out _drawing);
						prop.SetValue(SettingData, _drawing);
					}
					else if (val is Color _color)
					{
						Reg.Read(prop.Name, out _color);
						prop.SetValue(SettingData, _color);
					}
					else if (val is ValueTuple<uint, uint> _key)
					{
						Reg.Read(prop.Name, out _key);
						prop.SetValue(SettingData, _key);
					}
					else if (val is uint _uint)
					{
						Reg.Read(prop.Name, out _uint);
						prop.SetValue(SettingData, _uint);
					}
					else if (val is double _double)
					{
						Reg.Read(prop.Name, out _double);
						prop.SetValue(SettingData, _double);
					}
					else if (val is int _int)
					{
						Reg.Read(prop.Name, out _int);
						prop.SetValue(SettingData, _int);
					}
					else if (val is bool _bool)
					{
						Reg.Read(prop.Name, out _bool);
						prop.SetValue(SettingData, _bool);
					}
					else if (val is Key _one)
					{
						Reg.Read(prop.Name, out _one);
						prop.SetValue(SettingData, _one);
					}
					else if (val is System.Windows.Ink.DrawingAttributes _pen)
					{
                        Reg.Read(prop.Name, out _pen);
                        prop.SetValue(SettingData, _pen);
                    }
					else
					{
						System.Windows.MessageBox.Show(val.ToString());
					}
				}

				Reg.Read(nameof(CurrentPen), out CurrentPen);
			}
			catch (Exception e)
			{
				
			}

			SettingChanged?.Invoke(null, null);
		}

		public static event EventHandler SettingChanged;
	}
}
