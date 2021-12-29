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

using System.Windows.Threading;

namespace ThePen
{
	/// <summary>
	/// Interaction logic for Overlay.xaml
	/// </summary>
	/// 

	
	


	public partial class Overlay : UserControl
	{
		public Overlay()
		{
			InitializeComponent();

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();

			VarGrid.Visibility = Visibility.Collapsed;
		}
		Dictionary<string, string> variables;
		List<(TextBlock, List<(int, string)>)> textList;
		private void Timer_Tick(object sender, EventArgs e)
		{
			if (textList == null)
			{
				return;
			}

			foreach (var (text, content) in textList)
			{
				string res = "";
				foreach(var (mod, str) in content)
				{
					if (mod == 0)
					{
						res += str;
					}
					else if (mod == 2)
					{
						res += DateTime.Now.ToString(str);
					}
					else
					{
						res += variables[str];
					}
				}

				text.Text = res;
				
			}
		}

		public void Apply(string path)
		{
			try
			{ 
				if (System.IO.File.Exists(path))
				{
					var lines = System.IO.File.ReadAllLines(path);
					var items = OverlayScript.Parse(lines);
					this.Apply(items);
				}
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		//text to object
		public void Apply(List<Dictionary<string, OverlayScript.Prop>> items)
		{
			textList = new();
			variables = new();
			MainGrid.Children.Clear();
			VariableStackPanel.Children.Clear();

			foreach (var item in items)
			{
				var kind = item["kind"];
				if (kind == "image")
				{
					var e = GetImage(item);
					MainGrid.Children.Add(e);
				}
				else if (kind == "text")
				{
					var e = GetText(item);
					MainGrid.Children.Add(e);
				}
			}

			foreach (var (name, val) in variables)
			{
				StackPanel stackPanel = new()
				{
					Orientation = Orientation.Horizontal,
					Margin = new Thickness(0, 0, 0, 4)
				};

				TextBlock block = new()
				{
					Width = 200,
					VerticalAlignment = VerticalAlignment.Center,
					Text = name
				};

				TextBox box = new()
				{
					Height = 20,
					Width = 200
				};
				box.TextChanged += new TextChangedEventHandler((object sender, TextChangedEventArgs args) =>
				{
					variables[name] = (sender as TextBox).Text;
				});
				box.KeyDown += Box_KeyDown;

				stackPanel.Children.Add(block);
				stackPanel.Children.Add(box);
				VariableStackPanel.Children.Add(stackPanel);

			}
		}

		private void Box_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				showMain();
			}
			
		}

		private TextBlock GetText(Dictionary<string, OverlayScript.Prop> item)
		{
			TextBlock text = new();
			foreach (var pair in item)
			{
				var key = pair.Key;
				var prop = pair.Value;
				if (key == "background")
				{
					text.Background = prop;
				}
				else if (key == "size")
				{
					text.FontSize = prop;
				}
				else if (key == "font")
				{
					text.FontFamily = new FontFamily(prop);
				}
				else if (key == "color")
				{
					text.Foreground = prop;
				}
				else if (key == "text")
				{
					textList.Add((text, parseText(prop)));
				}
			}

			SetCommonProperty(text, item);

			return text;
		}

		private Image GetImage(Dictionary<string, OverlayScript.Prop> item)
		{
			Image image = new();
			foreach (var pair in item)
			{
				var key = pair.Key;
				var prop = pair.Value;
				if (key == "path")
				{
					image.Source = new BitmapImage(new Uri((string)prop, UriKind.RelativeOrAbsolute));
				}
			}

			SetCommonProperty(image, item);

			return image;
		}

		private void SetCommonProperty(FrameworkElement element, Dictionary<string, OverlayScript.Prop> item)
		{
			foreach (var pair in item)
			{
				var key = pair.Key;
				var prop = pair.Value;
				
				if (key == "width")
				{
					element.Width = prop;
				}
				else if (key == "height")
				{
					element.Height = prop;
				}
				else if (key == "margin")
				{
					if (prop.Count == 1)
					{
						element.Margin = new Thickness((double)prop);
					}
					else
					{
						List<double> margin = prop;
						element.Margin = new Thickness(margin[0], margin[1], margin[2], margin[3]);
					}
				}
				else if (key == "halign")
				{
					element.HorizontalAlignment = prop;
				}
				else if (key == "valign")
				{
					element.VerticalAlignment = prop;
				}
				else if (key == "opacity")
				{
					element.Opacity = prop;
				}
			}
		}

		private List<(int, string)> parseText(string line)
		{
			line = line.Replace("\"", "");
			List<(int, string)> res = new();
			int mod = 0;
			string word = "";
			for (int i = 0; i < line.Length; i++)
			{
				if (mod == 0)
				{
					if (line[i] == '$')
					{
						res.Add((mod, word));
						mod = 1;
						word = "";
					}
					else if (line[i] == '%')
					{
						res.Add((mod, word));
						mod = 2;
						word = "";
					}
					else
					{
						word += line[i];
					}
				}
				else if (mod == 1)
				{
					if (line[i] == '$')
					{
						res.Add((mod, word));
						mod = 0;
						variables.Add(word, "");
						word = "";
					}
					else
					{
						word += line[i];
					}
				}
				else if (mod == 2)
				{
					if (line[i] == '%')
					{
						res.Add((mod, word));
						mod = 0;
						word = "";
					}
					else
					{
						word += line[i];
					}
				}
			}
			res.Add((mod, word));

			return res;
		}

		public void Show()
		{
			if (variables.Count > 0)
			{
				MainGrid.Visibility = Visibility.Collapsed;
				VarGrid.Visibility = Visibility.Visible;
				var first = VariableStackPanel.Children[0] as StackPanel;
				var input = (first.Children[1] as TextBox);
				(Application.Current.MainWindow as MainWindow).GetCanvasActivate();
				input.Focus();

			}
			else
			{
				showMain();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			showMain();
		}

		private void showMain()
		{
			MainGrid.Visibility = Visibility.Visible;
			VarGrid.Visibility = Visibility.Collapsed;
		}
	}
}
