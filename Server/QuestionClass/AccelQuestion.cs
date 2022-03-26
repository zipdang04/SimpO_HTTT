using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class AccelQuestion
	{
		public const int MAX_IMG = 30;

		public OQuestion ques = new OQuestion();
		bool isVid = false;
		int cntImg = 0;
		public ImageClass[] imgs = new ImageClass[MAX_IMG];

		public AccelQuestion()
		{
			ques = new OQuestion();
			isVid = false;
			cntImg = 0;
			imgs = new ImageClass[MAX_IMG];
			for (int i = 0; i < MAX_IMG; i++)
				imgs[i] = new ImageClass();
		}

		public OQuestion question {
			get { return ques; }
			set { ques = value; }
		}
		public bool isVideo {
			get { return isVid; }
			set { isVid = value; }
		}
		public int cntImage {
			get { return cntImg; }
			set { cntImg = value; }
		}
		public ImageClass[] images {
			get { return imgs; }
			set { imgs = value; }
		}
	}
}
