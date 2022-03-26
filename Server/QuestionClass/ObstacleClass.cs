using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class ObstacleClass
	{
		public static readonly int QUES_NO = 5;
		
		public string keyword { get; set; }
		public string attach { get; set; }
		OQuestion[] ques = new OQuestion[QUES_NO];

		public ObstacleClass() {
			keyword = ""; 
			attach = "";
			for (int i = 0; i < QUES_NO; i++) ques[i] = new OQuestion();
		}
		public OQuestion[] questions {
			get { return ques; }
			set { ques = value; }
		}
		
	}
}
