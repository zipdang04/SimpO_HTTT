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
		List<Label> normalName, normalPoint;
		List<Label> chosenName, chosenPoint;

		PlayerClass? playerClass;

		public QNPBox()
		{
			InitializeComponent();
			normalName = new List<Label>(); normalName.Add(lblNormalName1); normalName.Add(lblNormalName2); normalName.Add(lblNormalName3); normalName.Add(lblNormalName4);
			chosenName = new List<Label>(); chosenName.Add(lblChosenName1); chosenName.Add(lblChosenName2); chosenName.Add(lblChosenName3); chosenName.Add(lblChosenName4);
			normalPoint = new List<Label>(); normalPoint.Add(lblNormalPoint1); normalPoint.Add(lblNormalPoint2); normalPoint.Add(lblNormalPoint3); normalPoint.Add(lblNormalPoint4);
			chosenPoint = new List<Label>(); chosenPoint.Add(lblChosenPoint1); chosenPoint.Add(lblChosenPoint2); chosenPoint.Add(lblChosenPoint3); chosenPoint.Add(lblChosenPoint4);

			SetHiddenAll();
			txtblQuestion.Text = "";
			lblPoint.Content = 0;
			label.Content = "";

			DataContext = playerClass;
		}
		public void SetContext(PlayerClass playerClass)
		{
			this.playerClass = playerClass;
			DataContext = playerClass;
		}
		public void SetPlayerHidden(int player) {
			normalName[player].Visibility = Visibility.Hidden;
			normalPoint[player].Visibility = Visibility.Hidden;
			chosenName[player].Visibility = Visibility.Hidden;
			chosenPoint[player].Visibility = Visibility.Hidden;
		}
		public void SetPlayerVisible(int player) {
			normalName[player].Visibility = Visibility.Visible;
			normalPoint[player].Visibility = Visibility.Visible;
			chosenName[player].Visibility = Visibility.Visible;
			chosenPoint[player].Visibility = Visibility.Visible;
		}
		public void SetHiddenAll()
		{
			for (int i = 0; i < 4; i++) {
				normalName[i].Visibility = Visibility.Hidden;
				normalPoint[i].Visibility = Visibility.Hidden;
				chosenName[i].Visibility = Visibility.Hidden;
				chosenPoint[i].Visibility = Visibility.Hidden;
			}
			txtblQuestion.Text = ""; 
			lblPoint.Visibility = Visibility.Hidden;
			label.Visibility = Visibility.Hidden;
		}
		public void SetVisibleAll()
		{
			for (int i = 0; i < 4; i++) {
				normalName[i].Visibility = Visibility.Visible;
				normalPoint[i].Visibility = Visibility.Visible;
				chosenName[i].Visibility = Visibility.Hidden;
				chosenPoint[i].Visibility = Visibility.Hidden;
			}
			txtblQuestion.Text = "";
			lblPoint.Visibility = Visibility.Visible;
			label.Visibility = Visibility.Visible;
		}
		public void SetChosenOne(int player)
		{
			Dispatcher.Invoke(() => {
				for (int i = 0; i < 4; i++) {
					normalName[i].Visibility = (i == player) ? Visibility.Hidden : Visibility.Visible;
					normalPoint[i].Visibility = (i == player) ? Visibility.Hidden : Visibility.Visible;
					chosenName[i].Visibility = (i == player) ? Visibility.Visible : Visibility.Hidden;
					chosenPoint[i].Visibility = (i == player) ? Visibility.Visible : Visibility.Hidden;
				}
				txtblQuestion.Text = "";
				lblPoint.Visibility = Visibility.Visible;
				label.Visibility = Visibility.Visible;
				Binding binding = new Binding(string.Format("points[{0}]", player));
				binding.Source = playerClass;
				lblPoint.SetBinding(Label.ContentProperty, binding);
			});
		}

		public void SetQuestion(string question) {
			Dispatcher.Invoke(() => {
				txtblQuestion.Text = question;
			});
		}
		public void SetLabel(string str) {
			Dispatcher.Invoke(() => { 
				label.Content = str; 
			});
		}
	}
}
