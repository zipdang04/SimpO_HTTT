using Server.QuestionClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
