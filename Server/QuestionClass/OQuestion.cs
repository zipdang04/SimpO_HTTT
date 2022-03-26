using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class OQuestion
	{
		public string question { get; set; }
		public string answer { get; set; }
		public string attach { get; set; }
		
		public OQuestion()
		{
			question = "";
			answer = "";
			attach = "";
		}
		public OQuestion(string question, string answer, string attach)
		{
			this.question = question;
			this.answer = answer;
			this.attach = attach;
		}
	}
}
