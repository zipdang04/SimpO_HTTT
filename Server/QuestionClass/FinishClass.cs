using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class FinishClass
	{
		public const int PLAYERS = 4;
		public const int QUES_TYPE = 3;
		public const int QUES_CNT= 3;
		public static readonly int[] QUES_POINT		  = new int[QUES_TYPE]{10, 20, 30};
		public static readonly int[] QUES_TIME		  = new int[QUES_TYPE]{10, 15, 20};
		public static readonly int[] PRAC_TIME		  = new int[QUES_TYPE]{30, 45, 60};
		public static readonly int[] REMAIN_PRAC_TIME = new int[QUES_TYPE]{20, 30, 40};

		public OQuestion[][][] ques = new OQuestion[4][][];
		public FinishClass()
		{
			for (int i = 0; i < PLAYERS; i++) {
				ques[i] = new OQuestion[QUES_TYPE][];
				for (int j = 0; j < QUES_TYPE; j++) {
					ques[i][j] = new OQuestion[QUES_CNT];
					for (int k = 0; k < QUES_CNT; k++)
						ques[i][j][k] = new OQuestion();
				}
			}
		}
		public OQuestion[][][] questions {
			get { return ques; }
			set { ques = value; }
		}
	}
}
