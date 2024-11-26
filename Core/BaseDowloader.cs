using System;

namespace Core
{
    public abstract class BaseDowloader
    {
        protected bool isPaused;
        protected bool isCancelled;
        public void TogglePause()
        {
            isPaused = !isPaused;
        }

        public void Cancelled()
        {
            isCancelled = true;
        }

        public abstract void OnDispose();
    }
}
