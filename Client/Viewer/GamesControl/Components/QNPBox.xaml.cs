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

using Server.Information;

namespace Client.Viewer.GamesControl.Components
{
	/// <summary>
	/// Interaction logic for QNPBox.xaml
	/// </summary>
	public partial class QNPBox : UserControl
	{
		List<Label> lblNames, lblPoints;
		int[] playerRemain = new int[3] {1, 2, 3};

		PlayerClass? playerClass;

		public QNPBox()
		{
			InitializeComponent();
			lblNames = new List<Label>(); lblNames.Add(lblName1); lblNames.Add(lblName2); lblNames.Add(lblName3); 
			lblPoints = new List<Label>(); lblPoints.Add(lblPoint1); lblPoints.Add(lblPoint2); lblPoints.Add(lblPoint3);

			txtblQuestion.Text = "";
			lblLabel.Content = "";

			DataContext = playerClass;
		}
		public void SetContext(PlayerClass playerClass)
		{
			this.playerClass = playerClass;
			DataContext = playerClass;
		}
		public void SetChosenOne(int player)
		{
			Dispatcher.Invoke(() => {
				txtblQuestion.Text = "";
				lblLabel.Content = "";

				Binding bindName= new Binding(string.Format("names[{0}]", player));
				bindName.Source = playerClass;
				lblName.SetBinding(Label.ContentProperty, bindName);
				Binding bindPoint = new Binding(string.Format("points[{0}]", player));
				bindPoint.Source = playerClass;
				lblPoint.SetBinding(Label.ContentProperty, bindPoint);

				int ptr = 0;
				for (int i = 0; i < 4; i++)
				{
					if (i == player) continue;
					
					playerRemain[ptr] = i;
					Binding bExtraName = new Binding(string.Format("names[{0}]", i));
					bExtraName.Source = playerClass;
					lblNames[ptr].SetBinding(Label.ContentProperty, bExtraName);
					Binding bExtraPoint = new Binding(string.Format("points[{0}]", i));
					bExtraPoint.Source = playerClass;
					lblPoints[ptr].SetBinding(Label.ContentProperty, bExtraPoint);

					ptr++;
				}
			});
		}

		public void SetQuestion(string question) {
			Dispatcher.Invoke(() => {
				txtblQuestion.Text = question;
			});
		}
		public void SetLabel(string str) {
			Dispatcher.Invoke(() => { 
				lblLabel.Content = str; 
			});
		}
	}
}
