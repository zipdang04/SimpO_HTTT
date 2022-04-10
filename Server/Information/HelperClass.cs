using Server.QuestionClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace Server.Information
{
	public static class HelperClass
	{
		public class Token
		{
			public string content = "";
			public int posi = 0;
		}
		public static List<string> ParseToken(string s)
		{
			List<Token> tokens = new List<Token>();
			int length = s.Length;
			Token curToken = new Token();
			for (int i = 0; i < length; i++) {
				char c = s[i];
				if (c != ' ') {
					if (curToken.content == "") 
						curToken.posi = i;
					curToken.content += c;
				}
				else {
					if (curToken.content != "") {
						tokens.Add(curToken);
						curToken = new Token();
					}
				}
			}
			if (curToken.content != "")
				tokens.Add(curToken);

			List<string> answer = new List<string>();
			int tokenLength = tokens.Count;
			int posBeg = -1;
			for (int i = 0; i < tokenLength; i++) {
				Token token = tokens[i];
				if (token.content == "BEGBEGBEG")
					posBeg = tokens[i + 1].posi;
				else if (token.content == "ENDENDEND") {
					int posEnd = token.posi;
					answer.Add(s.Substring(posBeg, posEnd - posBeg));
					posBeg = -1;
				} else {
					if (posBeg == -1)
						answer.Add(token.content);
				}
			}
			return answer;
		}
		
		public static string ServerNameCommand(string[] names)
		{
			string answer = "OLPA INFO NAME ";
			foreach (string name in names)
				answer += "BEGBEGBEG " + name + " ENDENDEND ";

			return answer;
		}
		public static string ServerNameCommand(ObservableCollection<string> names)
		{
			string answer = "OLPA INFO NAME ";
			foreach (string name in names)
				answer += "BEGBEGBEG " + name + " ENDENDEND ";

			return answer;
		}
		public static string ServerPointCommand(int[] points)
		{
			string answer = "OLPA INFO POINT ";
			foreach (int point in points)
				answer += "BEGBEGBEG " + point.ToString() + " ENDENDEND ";

			return answer;
		}
		public static string ServerPointCommand(ObservableCollection<int> points)
		{
			string answer = "OLPA INFO POINT ";
			foreach (int point in points)
				answer += "BEGBEGBEG " + point.ToString() + " ENDENDEND ";

			return answer;
		}
		public static string ServerJoinQA(OQuestion ques)
		{
			string question = "BEGBEGBEG " + ques.question + " ENDENDEND";
			string attach = "BEGBEGBEG " + ques.attach + " ENDENDEND";
			return question + " " + attach;
		}
		public static string MakeString(string s)
		{
			return "BEGBEGBEG " + s + " ENDENDEND";
		}
	
		public static WholeExamClass ExcelToWholeExam(string path)
		{
			Excel.Application xlApp = new Excel.Application();
			Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
			Excel.Worksheet xlWorksheet = xlWorkbook.Worksheets[1];
			string getValue(int row, int col) {
				if (xlWorksheet.Cells[row, col].Value == null) return "";
				else return xlWorksheet.Cells[row, col].Value.ToString();
			}
			OQuestion process(int row, int col){
				OQuestion question = new OQuestion();
				question.question = getValue(row, col);
				question.answer = getValue(row, col + 1);
				question.attach = getValue(row, col + 2);
				return question;
			}

			WholeExamClass whole = new WholeExamClass();
			// Khởi động!
			int[] startRow = new int[4]{ 12, 12, 36, 36 };
			int[] startCol = new int[4]{ 2, 7, 2, 7 };
			for (int player = 0; player < 4; player++) {
				int row = startRow[player], col = startCol[player];
				for (int ques = 0; ques < StartClass.QUES_CNT; ques++)
					whole.startQuestions.questions[player][ques] = process(row + ques, col);
			}

			// VCNV!
			whole.obstacle.keyword = getValue(60, 3);
			whole.obstacle.attach = getValue(61, 3);
			int vcnvRow = 63, vcnvCol = 2;
			for (int ques = 0; ques < ObstacleClass.QUES_NO; ques++)
				whole.obstacle.questions[ques] = process(vcnvRow + ques, vcnvCol);

			// TT!
			int accelRow = 61, accelCol = 7;
			for (int i = 0; i < 4; i++)
			{
				whole.acceleration.accelQuestions[i].question = process(accelRow + i, accelCol);
				whole.acceleration.accelQuestions[i].isVideo = true;
				whole.acceleration.accelQuestions[i].cntImage = 0;
			}

			// Về đích!
			int[] finishRow = new int[4] { 77, 77, 90, 90 };
			int[] finishCol = new int[4] { 2, 7, 2, 7 };
			for (int player = 0; player < 4; player++) {
				int row = finishRow[player], col = finishCol[player], ptr = 0;
				for (int diff = 0; diff < 3; diff++)
					for (int i = 0; i < 3; i++) {
						whole.finish.questions[player][diff][i] = process(row + ptr, col);
						ptr++;
					}
			}
			xlWorkbook.Close();

			return whole;
		}
		public static void ClearDirectory(System.IO.DirectoryInfo directory)
		{
			foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
			foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
		}
	
		public static string PathString(string attach)
		{
			return Directory.GetCurrentDirectory() + @"\Resources\" + attach;
		}
		public static string PathString(string dir, string attach)
		{
			return Directory.GetCurrentDirectory() + string.Format(@"\Resources\{0}\{1}", dir, attach);
		}
	}
}
