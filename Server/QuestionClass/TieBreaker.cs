using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class TieBreaker
	{
		public const int QUES_CNT = 310;
		public OQuestion[] questions { get; set; }
		public TieBreaker()
		{
			questions = new OQuestion[QUES_CNT];
			for(int i = 0; i < QUES_CNT; i++)
				questions[i] = new OQuestion();
		}
	}
}
