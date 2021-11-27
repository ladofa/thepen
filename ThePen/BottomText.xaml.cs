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
	/// Interaction logic for BottomText.xaml
	/// </summary>
	public partial class BottomText : UserControl
	{
		public BottomText()
		{
			InitializeComponent();

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(1000);
			timer.Tick += new EventHandler(timer_Tick);
			timer.Start();
		}


		private void timer_Tick(object sender, EventArgs e)
		{
			CurrentTime.Text = DateTime.Now.ToString("yyyy년 M월 d일 HH:mm ");
		}
	}
}
