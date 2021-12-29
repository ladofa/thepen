using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;


namespace ThePen
{
	public class OverlayScript
	{
		public static List<Dictionary<string, Prop>> Parse(string[] lines)
		{
			List<Dictionary<string, Prop>> items = new();
			Dictionary<string, Prop> item = null;

			foreach (var line in lines)
			{
				if (line.Length == 0) continue;
				if (line[0] == '#') continue;

				if (line[0] == '[' && line[^1] == ']')
				{
					if (item != null)
					{
						items.Add(item);
					}
					item = new();
					item.Add("kind", line[1..^1].ToLower());
				}
				else
				{
					if (item == null)
					{
						throw new Exception("[]");
					}

					var words = System.Text.RegularExpressions.Regex.Matches(line, @"[\""].+?[\""]|[^ ]+")
									.Cast<System.Text.RegularExpressions.Match>()
									.Select(m => m.Value)
									.ToList();

					//var words = line.Trim().Split(new char[] { ' ', '\t' });
					var key = words[0];
					var values = new List<string>();

					//List<string> q = null;
					//foreach (var word in words[1..])
					//{
					//	if (word.Length == 0) continue;

					//	//for "asdfasdf asdfasdf"
					//	if (q != null)
					//	{
					//		if (word[^1] == '"')
					//		{
					//			q.Add(word[..^1]);
					//			values.Add(String.Join("", q));
					//			q = null;
					//		}
					//		else
					//		{
					//			q.Add(word);
					//		}
					//	}

					//	if (word[0] == '"')
					//	{
					//		q = new();
					//		q.Add(word[1..]);
					//	}
					//	else
					//	{
					//		values.Add(word);
					//	}
					//}


					words.RemoveAt(0);
					item.Add(key.ToLower(), words);
				}


			}

			if (item != null)
			{
				items.Add(item);
			}

			return items;
		}


		public class Prop
		{
			public List<string> Words;
			public Prop(string word)
			{
				Words = new();
				Words.Add(word);
			}

			public Prop(List<string> words)
			{
				Words = words;
			}

			public static implicit operator Prop(string word) => new Prop(word);
			public static implicit operator Prop(List<string> words) => new Prop(words);
			public static implicit operator string(Prop prop) => prop.Words[0];
			public static implicit operator double(Prop prop) => Double.Parse(prop.Words[0]);
			public static implicit operator List<double>(Prop prop)
			{
				var res = from word in prop.Words select Double.Parse(word);
				return res.ToList();
			}

			public static implicit operator HorizontalAlignment(Prop prop)
			{
				string word = prop;
				string lower = word.ToLower();
				if (lower == "left")
				{
					return HorizontalAlignment.Left;
				}
				else if (lower == "right")
				{
					return HorizontalAlignment.Right;
				}
				else if (lower == "center")
				{
					return HorizontalAlignment.Center;
				}

				return HorizontalAlignment.Stretch;
			}

			public static implicit operator VerticalAlignment(Prop prop)
			{
				string word = prop;
				string lower = word.ToLower();
				if (lower == "top")
				{
					return VerticalAlignment.Top;
				}
				else if (lower == "bottom")
				{
					return VerticalAlignment.Bottom;
				}
				else if (lower == "center")
				{
					return VerticalAlignment.Center;
				}

				return VerticalAlignment.Stretch;
			}

			public static implicit operator Brush(Prop prop)
			{
				var words = prop.Words;
				Color color;
				if (words.Count == 3)
				{
					color = Color.FromRgb(Byte.Parse(words[0]), Byte.Parse(words[1]), Byte.Parse(words[2]));
				}
				else if (words.Count == 4)
				{
					color = Color.FromArgb(Byte.Parse(words[0]), Byte.Parse(words[1]),
						Byte.Parse(words[2]), Byte.Parse(words[3]));
				}
				else if (words.Count == 1)
				{
					color = (Color)ColorConverter.ConvertFromString(words[0]);
				}
				return new SolidColorBrush(color);
			}

			public int Count => Words.Count;

		}

	}

}
