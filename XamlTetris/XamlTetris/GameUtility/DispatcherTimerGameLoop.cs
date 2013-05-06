using System;
using System.Windows.Threading;

namespace GameUtility.Timers
{
    public class DispatcherTimerGameLoop : GameLoop
    {
        DispatcherTimer t = new DispatcherTimer();

        public TimeSpan Interval 
        {
            get { return t.Interval; } 
            set { t.Interval = value; } 
        }

        public bool IsEnabled
        {
            get { return t.IsEnabled; }
        }

        public DispatcherTimerGameLoop() : this(0)
        {
        }

        public DispatcherTimerGameLoop(double milliseconds)
        {
            t.Interval = TimeSpan.FromMilliseconds(milliseconds);
            t.Tick += new EventHandler(t_Tick);
        }

        public override void Start()
        {
            t.Start();
            base.Start();
        }

        public override void Stop()
        {
            t.Stop();
            base.Stop();
        }

        void t_Tick(object sender, EventArgs e)
        {
            base.Tick();
        }
    }
}
