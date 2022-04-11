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

namespace Client.Viewer.GamesControl.Components
{
	/// <summary>
	/// Interaction logic for WordControl.xaml
	/// </summary>
	public partial class WordControl : UserControl
	{
		const int MAXLEN = 17;
		Letter[] letters = new Letter[17];
		public WordControl()
		{
			InitializeComponent();
			for (int i = 0; i < MAXLEN; i++){
				letters[i] = new Letter();
				grid.Children.Add(letters[i]);
				Grid.SetColumn(letters[i], i * 2);
				letters[i].Visibility = Visibility.Hidden;
			}
		}
		public void setWord(string s)
		{
			string processed = "";
			foreach (char c in s) if (c != ' ')
				processed += c;
			bool addLeft = false;
			while (processed.Length < MAXLEN)
			{
				if (addLeft) processed = " " + processed;
				else processed += " ";
				addLeft = !addLeft;
			}
			for (int i = 0; i < MAXLEN; i++)
				letters[i].SetChar(processed[i]);
		}

		public void SetNormal() {
			for (int i = 0; i < MAXLEN; i++) letters[i].SetNormal();
		}
		public void SetChoosing() { 
			for (int i = 0; i < MAXLEN; i++) letters[i].SetChoosing();
		}
		public void SetEnabled() { 
			for (int i = 0; i < MAXLEN; i++) letters[i].SetEnabled();
		}
		public void SetDisabled() { 
			for (int i = 0; i < MAXLEN; i++) letters[i].SetDisabled();
		}
	}
}
