using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Server.HostServer.Components
{
	public delegate void TikTok(int time, bool done);
	public class Simer
	{
		public TikTok? Tick;
		DispatcherTimer timer;

		DateTime timeBegin;
		int timeLimit, curTime;
		public bool IsEnabled;

		public static int ToInt(TimeSpan span)
		{
			return (span.Minutes * 60 * 1000 + span.Seconds * 1000 + span.Milliseconds) / 10;
		}
		public int getTime()
		{
			TimeSpan span = DateTime.Now - timeBegin;
			return ToInt(span);
		}

		public Simer(int timeLimit = 1000) {
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(2);
			timer.Tick += Timer_Tick;
			IsEnabled = false;
			SetTimeLimit(timeLimit);
		}
		public Simer(TimeSpan span)
		{
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(2);
			timer.Tick += Timer_Tick;
			IsEnabled = false;
			SetTimeLimit(span);
		}
		public void SetTimeLimit(int timeLimit)
		{
			this.timeLimit = timeLimit;
		}
		public void SetTimeLimit(TimeSpan span)
		{
			this.timeLimit = ToInt(span);
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{
			int time = getTime();
			bool done = false; 
			if (time >= timeLimit) { done = true; Stop(); }
			Tick?.Invoke(time, done);
		}

		public void Start()
		{
			timeBegin = DateTime.Now;
			IsEnabled = true;
			timer.Start();
		}
		public void Start(int timeLimit)
		{
			SetTimeLimit(timeLimit);
			Start();
		}
		public void Start(TimeSpan span)
		{
			Start(ToInt(span));
		}

		public void Pause() {
			curTime = getTime(); timer.Stop();
		}
		public void Resume() {
			timeBegin = DateTime.Now - TimeSpan.FromMilliseconds(curTime * 10);
			timer.Start();
		}

		public void Stop()
		{
			IsEnabled = false;
			timer.Stop();
		}
	}
}
