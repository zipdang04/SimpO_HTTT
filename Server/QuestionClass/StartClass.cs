using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class StartClass
	{
		public const int PLAYERS = 4;
		public const int QUES_CNT = 20;

		public OQuestion[][] ques = new OQuestion[PLAYERS][];

		public StartClass()
		{
			for (int i = 0; i < PLAYERS; i++)
			{
				ques[i] = new OQuestion[QUES_CNT];
				for (int j = 0; j < QUES_CNT; j++)
					ques[i][j] = new OQuestion();
			}
		}
		public OQuestion[][] questions {
			get { return ques; }
			set { ques = value; }
		}
	}
}
