using System;
using UnityEngine;
using MisterGames.Tick.Core;
using Random = UnityEngine.Random;

namespace Tween {
    [Serializable]
    public abstract class Tween : IUpdate {

        [SerializeField] private string id;
        [SerializeField] protected GameObject tweenableObject;
        [SerializeField] private bool loop;
        [SerializeField] private bool yoyo;
        [SerializeField] private bool autoStart = true;
        [SerializeField] private Vector2 speedRange = new Vector2(1, 1);
        [SerializeField] private float duration = 1;
        [SerializeField] private float delay = 0;

        protected ITimeSource timeSource;
        
        public event Action OnCompleted = delegate { };
        public event Action OnLooped = delegate { };

        public bool IsAutoStart => autoStart;
        
        public bool Playing => _playing;
        
        public bool Paused => _paused;
        
        public bool Reverted {
            set => _reverted = value;
            get => _reverted;
        }
        
        public bool Active {
            set => _active = value;
        }

        public string ID {
            get => id;
        }

        public float GlobalSpeedMultiplier {
            set => _globalSpeedMultiplier = value;
        }

        private float _progress;
        private float _globalSpeedMultiplier = 1;
        private float _speed = 1;
        private bool _yoyoReverse;
        private bool _playing;
        private bool _reverted;
        private bool _paused = false;
        private bool _active = true;

        public virtual void Init(GameObject gameObject, ITimeSource source) {
            if (tweenableObject == null) {
                tweenableObject = gameObject;
            }

            timeSource = source;
            timeSource.Subscribe(this);
        }

        public virtual void Play() {
            _speed = Random.Range(speedRange.x, speedRange.y);
            
            _paused = false;
            _playing = true;
            
            // if (_reverted) Wind();
            // else
            Rewind();
        }
        
        public virtual void Pause() {
            _paused = true;
        }
        
        public virtual void Resume() {
            _paused = false;
        }

        public void Wind() {
            _progress = 1;
            
            OnProgressUpdate();
        }

        public virtual void Stop() {
            Pause();
            
            // if (_reverted) Wind();
            // else Rewind();

            Wind();
        }

        public virtual void Rewind() {
            _progress = 0;
            
            OnProgressUpdate();
        }

        public void OnComplete() {
            Pause();

            OnCompleted.Invoke();
        }

        public void OnLoop() {
            Play();

            OnLooped.Invoke();
        }
        
        public void OnUpdate(float dt) {
            if (!_playing || _paused || !_active) return;
            
            var deltaProgress = _globalSpeedMultiplier * _speed * dt / (duration + delay);
            
            if (_yoyoReverse) _progress -= deltaProgress;
            else _progress += deltaProgress;

            if (_progress > 1) {
                _progress = 1;
                
                if (loop) {
                    if (yoyo) {
                        _yoyoReverse = true;
                    } else {
                        OnLoop();
                    }
                } else {
                    if (yoyo) {
                        if (_yoyoReverse) {
                            _yoyoReverse = false;
                            OnComplete();
                        } else {
                            _yoyoReverse = true;
                        }
                    } else {
                        OnComplete();
                    }
                }
            } else if (_progress < 0) {
                _progress = 0;
                _yoyoReverse = false;

                if (!loop) {
                    OnComplete();
                } else {
                    OnLoop();
                }
            }
            
            OnProgressUpdate();
        }

        private void OnProgressUpdate() {
            var progress = _progress;

            if (delay > 0) {
                var totalDuration = delay + duration;
                
                if (progress < delay / totalDuration) {
                    progress = 0;
                } else {
                    if (duration == 0 && progress >= 1) {
                        // progress = (progress - delay / totalDuration) * totalDuration / duration;
                    } else {
                        progress = (progress - delay / totalDuration) * totalDuration / duration;
                    }
                }
            }
            
            if (_reverted) progress = 1 - _progress;
            
            ProceedUpdate(progress);
        }

        protected virtual void ProceedUpdate(float progress) {
            throw new NotImplementedException();
        }
    }
}
