using System;

namespace Tween {
    public interface ITweenController {

        public event Action OnFinished;
        
        public void Pause();

        public void Stop();

        public void Resume();

        public void Rewind();

        public void Wind();

        public void Play();

        public void Revert(bool reverted);
        
    }
}