﻿using Server.Information;
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
	/// Interaction logic for Letter.xaml
	/// </summary>
	public partial class Letter : UserControl
	{
		public enum LetterState { NORMAL, CHOOSING, ENABLE, DISABLE };
		public static Uri NORMAL = new Uri(HelperClass.PathString("Effects", "VCNV_LetterNormal.png"));
		public static Uri CHOOSING = new Uri(HelperClass.PathString("Effects", "VCNV_LetterChossing.png"));
		public static Uri ENABLED = new Uri(HelperClass.PathString("Effects", "VCNV_LetterEnabled.png"));
		public static Uri DISABLED = new Uri(HelperClass.PathString("Effects", "VCNV_LetterDisabled.png"));
		public Letter()
		{
			InitializeComponent();
			SetNormal();
		}
		public Letter(char c): this() { SetChar(c); }

		public void SetChar(char c) { lblChar.Content = c; }

		public void SetNormal() { background.ImageSource = new BitmapImage(NORMAL); lblChar.Visibility = Visibility.Hidden; }
		public void SetChoosing() { background.ImageSource = new BitmapImage(CHOOSING); lblChar.Visibility = Visibility.Hidden; }
		public void SetEnabled() { background.ImageSource = new BitmapImage(ENABLED); lblChar.Visibility = Visibility.Visible; }
		public void SetDisabled() { background.ImageSource = new BitmapImage(DISABLED); lblChar.Visibility = Visibility.Hidden; }
	}
}