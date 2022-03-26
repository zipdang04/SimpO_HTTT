using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.QuestionClass
{
	public class ImageClass
	{
		string path;
		public ImageClass()
		{
			path = "";
		}
		public ImageClass (string path)
		{
			this.path = path;
		}
		public string imagePath {
			get { return path; }
			set { path = value; }
		}
	}
}
