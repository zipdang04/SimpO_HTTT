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
using System.Windows.Shapes;
using System.Text.Json;
using Server.QuestionClass;
using System.IO;
using Microsoft.Win32;

namespace Server.ExamMaker
{
	/// <summary>
	/// Interaction logic for ExamMakerWindow.xaml
	/// </summary>
	public partial class ExamMakerWindow : Window
	{
		MainWindow main;
		WholeExamClass? wholeExam;
		public static readonly string filePath = "Resources/OExam.json";

		public ExamMakerWindow(MainWindow main)
		{
			InitializeComponent();
			this.main = main;

			try {
				byte[] jsonString = File.ReadAllBytes(filePath);
				var utf8Reader = new Utf8JsonReader(jsonString);
				wholeExam = JsonSerializer.Deserialize<WholeExamClass>(ref utf8Reader);
			} catch {
				wholeExam = new WholeExamClass();
			}

			if (wholeExam == null)
				wholeExam = new WholeExamClass();

			tabStart.Content = new StartControl(wholeExam.startQuestions);
			tabObsta.Content = new ObstaControl(wholeExam.obstacle);
			tabAccel.Content = new AccelControl(wholeExam.acceleration);
			tabFinsh.Content = new FinishControl(wholeExam.finish);
		}

		private void SaveFile()
		{
			JsonSerializerOptions jso = new JsonSerializerOptions();
			jso.WriteIndented = true;

			byte[] jsonString = JsonSerializer.SerializeToUtf8Bytes(wholeExam, jso);
			File.WriteAllBytes(filePath, jsonString);
		}

		private void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			SaveFile();
			main.Show();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFile();
		}

		private void btnOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
				wholeExam = Information.HelperClass.ExcelToWholeExam(openFileDialog.FileName);
			tabStart.Content = new StartControl(wholeExam.startQuestions);
			tabObsta.Content = new ObstaControl(wholeExam.obstacle);
			tabAccel.Content = new AccelControl(wholeExam.acceleration);
			tabFinsh.Content = new FinishControl(wholeExam.finish);
		}
	}
}
