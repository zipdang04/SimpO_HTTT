using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class WholeExamClass
	{
		StartClass startClass = new StartClass();
		ObstacleClass obstaClass = new ObstacleClass();
		AccelClass accelClass = new AccelClass();
		FinishClass finishClass = new FinishClass();
		TieBreaker tieClass = new TieBreaker();

		public StartClass startQuestions { 
			get { return startClass; }
			set { startClass = value; }
		}
		public ObstacleClass obstacle {
			get { return obstaClass; }
			set { obstaClass = value; }
		}
		public AccelClass acceleration {
			get { return accelClass; }
			set { accelClass = value; }
		}
		public FinishClass finish {
			get { return finishClass; }
			set { finishClass = value; }
		}
		public TieBreaker tieBreaker {
			get { return tieClass; }
			set { tieClass = value; }
		}
	}
}
