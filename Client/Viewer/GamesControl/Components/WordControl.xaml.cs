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
		public void Reset()
		{
			for (int i = 0; i < MAXLEN; i++)
			{
				letters[i].SetChar(' ');
				letters[i].SetNull();
			}
		}
		public void SetWord(int num)
		{
			string processed = "";
			for (int i = 0; i < num; i++) processed += 'X';
			bool addLeft = false;
			while (processed.Length < MAXLEN)
			{
				if (addLeft) processed = " " + processed;
				else processed += " ";
				addLeft = !addLeft;
			}
			for (int i = 0; i < MAXLEN; i++)
				if (processed[i] != ' ')
					letters[i].SetNormal();
				else
					letters[i].SetNull();
		}
		public void ShowAnswer(int index, string answer)
		{
			int ptr = 0;
			if (index < 4)
				for (int i = 0; i < MAXLEN; i++)
					if (letters[i].Visibility == Visibility.Visible) { 
						letters[i].SetChar(answer[ptr]); 
						letters[i].SetEnabled(); ptr++; 
					}
		}
		public void DisAnswer(int index)
		{
			for (int i = 0; i < MAXLEN; i++)
				if (letters[i].Visibility == Visibility.Visible)
					letters[i].SetDisabled();
		}
		public void SetChoosing() { 
			for (int i = 0; i < MAXLEN; i++) 
				if (letters[i].Visibility == Visibility.Visible)
					letters[i].SetChoosing();
		}
	}
}
