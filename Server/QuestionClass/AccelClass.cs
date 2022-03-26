using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class AccelClass
	{
		public const int CNT_ACCEL = 4;

		public AccelQuestion[] accel = new AccelQuestion[CNT_ACCEL];

		public AccelClass()
		{
			for (int i = 0; i < CNT_ACCEL; i++)
				accel[i] = new AccelQuestion();
		}

		public AccelQuestion[] accelQuestions {
			get { return accel; }
			set { accel = value; }
		}
	}
}
