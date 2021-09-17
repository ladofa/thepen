using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ThePen
{
	public static class Reg
	{

		static Microsoft.Win32.RegistryKey rootKey;

		//-- 1. set root registry key as you want
		static Reg()
		{
			rootKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\ThePen");
		}

		//-- 2. make your own property like below
		//--    copy, paste and just modify type, name, and defaultValue
		public static int TestInt
		{
			get
			{
				var defaultValue = 0;
				var info = System.Reflection.MethodBase.GetCurrentMethod() as System.Reflection.MethodInfo;
				var name = info.Name.Substring(4);
				string str = rootKey.GetValue(name, defaultValue.ToString()) as string;
				return (dynamic)Convert.ChangeType(str, info.ReturnType);
			}

			set
			{
				var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4);
				rootKey.SetValue(name, value.ToString());
			}
		}

		public static double TestDouble
		{
			get
			{
				var defaultValue = 0;
				var info = System.Reflection.MethodBase.GetCurrentMethod() as System.Reflection.MethodInfo;
				var name = info.Name.Substring(4);
				string str = rootKey.GetValue(name, defaultValue.ToString()) as string;
				return (dynamic)Convert.ChangeType(str, info.ReturnType);
			}

			set
			{
				var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4);
				rootKey.SetValue(name, value.ToString());
			}
		}

		public static void Write(string key, bool val)
		{
			rootKey.SetValue(key, val, Microsoft.Win32.RegistryValueKind.DWord);
		}

		public static void Read(string key, out bool val)
		{
			int? r = rootKey.GetValue(key) as int?;
			val = r.Value != 0;
		}

		public static void Write(string key, int val)
		{
			rootKey.SetValue(key, val, Microsoft.Win32.RegistryValueKind.DWord);
		}

		public static void Read(string key, out int val)
		{
			int? r = rootKey.GetValue(key) as int?;
			val = r.Value;
		}

		public static void Write(string key, uint val)
		{
			rootKey.SetValue(key, val, Microsoft.Win32.RegistryValueKind.DWord);
		}

		public static void Read(string key, out uint val)
		{
			int? r = rootKey.GetValue(key) as int?;
			val = (uint)r.Value;
		}

		public static void Write(string key, double val)
		{
			rootKey.SetValue(key, (int)(val * 10000), Microsoft.Win32.RegistryValueKind.DWord);
		}

		public static void Read(string key, out double val)
		{
			int? r = rootKey.GetValue(key) as int?;
			val = r.Value / 10000.0;
		}

		public static void Write(string key, System.Windows.Media.Color color)
		{
			byte[] arr = new byte[4]
			{
				color.A, color.R, color.G, color.B
			};
			rootKey.SetValue(key, arr, Microsoft.Win32.RegistryValueKind.Binary);
		}

		public static void Read(string key, out System.Windows.Media.Color color)
		{
			byte[] arr = rootKey.GetValue(key) as byte[];
			color = System.Windows.Media.Color.FromArgb(arr[0], arr[1], arr[2], arr[3]);
		}

		public static void Write(string key, System.Windows.Ink.DrawingAttributes pen)
		{
			Write(key + "_color", pen.Color);
			Write(key + "_width", pen.Width);
			Write(key + "_height", pen.Height);
			Write(key + "_fit", pen.FitToCurve);
			Write(key + "_ignore", pen.IgnorePressure);
			Write(key + "_high", pen.IsHighlighter);
			Write(key + "_angle", Global.GetAngle(pen.StylusTipTransform));
		}

		public static void Read(string key, out System.Windows.Ink.DrawingAttributes outPen)
		{
			System.Windows.Ink.DrawingAttributes pen = new();
			Read(key + "_color", out System.Windows.Media.Color color);
			Read(key + "_width", out double width);
			Read(key + "_height", out double height);
			Read(key + "_fit", out bool fit);
			Read(key + "_ignore", out bool ignore);
			Read(key + "_high", out bool high);
			Read(key + "_angle", out double angle);
			System.Windows.Media.Matrix m = System.Windows.Media.Matrix.Identity;
			m.Rotate(angle);
			outPen = new()
			{
				Color = color,
				Width = width,
				Height = height,
				FitToCurve = fit,
				IgnorePressure = ignore,
				IsHighlighter = high,
				StylusTipTransform = m
			};
		}

		public static void Write(string key, (uint, uint) value)
		{
			Write(key + "1", value.Item1);
			Write(key + "2", value.Item2);
		}

		public static void Read(string key, out (uint, uint) value)
		{
			Read(key + "1", out uint v1);
			Read(key + "2", out uint v2);
			value = (v1, v2);
		}

		public static void Write(string key, System.Windows.Input.Key val)
		{
			Write(key, (uint)val);
		}

		public static void Read(string key, out System.Windows.Input.Key val)
		{
			Read(key, out uint _val);
			val = (System.Windows.Input.Key)_val;
		}

	}
}
