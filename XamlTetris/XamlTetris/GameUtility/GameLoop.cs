using System;

namespace GameUtility.Timers
{
    public abstract class GameLoop
    {
        protected DateTime lastTick;
        public delegate void UpdateHandler(TimeSpan elapsed);
        public event UpdateHandler Update;

        public void Tick()
        {
            DateTime now = DateTime.Now;
            TimeSpan elapsed = now - lastTick;
            lastTick = now;
            if (Update != null) Update(elapsed);
        }

        public virtual void Start()
        {
            lastTick = DateTime.Now;
        }

        public virtual void Stop()
        {
        }
    }
}
