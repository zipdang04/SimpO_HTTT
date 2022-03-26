using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Information
{
	public class PlayerClass
	{
		public ObservableCollection<string> names { get; set; }
		public ObservableCollection<int> points { get; set; }
		//string[] name = new string[4];
		//int[] pts = new int[] { 0, 0, 0, 0 };
		public PlayerClass()
		{
			names = new ObservableCollection<string> { "", "", "", ""};
			points = new ObservableCollection<int> { 0, 0, 0, 0};
			//name = new string[4];
			//pts = new int[] { 0, 0, 0, 0 };
		}
		//public int[] points {
		//	get { return pts; }
		//	set { pts = value; }
		//}
		//public string[] names {
		//	get { return name; }
		//	set { name = value; }
		//}
	}
}
