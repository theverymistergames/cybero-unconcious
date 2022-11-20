using System;
using MisterGames.Common.Attributes;
using MisterGames.Tick.Core;
using MisterGames.Tick.Jobs;
using MisterGames.Tick.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Tween
{
    public class TweenController : MonoBehaviour, ITweenController
    {
        [SerializeField] private float globalDelay;
        [SerializeField] private float globalDuration;
        [SerializeField] private Vector2 globalSpeedRange = new Vector2(1, 1);
        [SerializeField] private bool autoStart = true;
        [SerializeField] private bool loop;
        [SerializeField] private bool chain; //TODO
        [SerializeField][SubclassSelector][SerializeReference] private Tween[] tweens;
        [FormerlySerializedAs("timeDomainLauncher")] [SerializeField] private TimeDomain timeDomain;

        public event Action OnFinished = delegate { };
        
        private IJob _job;
        
        private void Awake() {
            foreach (var tween in tweens) {
                tween.Init(gameObject, timeDomain.Source);
            }
        }

        private void Start() {
            Rewind();
            
            if (autoStart) {
                _job?.Stop();
                _job = JobSequence.Create()
                    .Delay(globalDelay)
                    .Action(Play)
                    .RunFrom(timeDomain.Source);
            }
        }

        public void Pause() {
            _job?.Stop();
            
            foreach (var tween in tweens) {
                tween.Pause();
            }
        }

        public void Stop() {
            _job?.Stop();
            
            foreach (var tween in tweens) {
                tween.Stop();
            }
        }

        public void Resume() {
            _job?.Start();
            
            foreach (var tween in tweens) {
                if (tween.Paused) tween.Resume();
            }
        }

        public void Rewind() {
            foreach (var tween in tweens) {
                tween.Rewind();
            }
        }

        public void Wind() {
            foreach (var tween in tweens) {
                tween.Wind();
            }
        }

        public void Play() {
            var speed = Random.Range(globalSpeedRange.x, globalSpeedRange.y);
            
            foreach (var tween in tweens) {
                tween.GlobalSpeedMultiplier = speed;
                if (tween.IsAutoStart) tween.Play();
            }

            if (loop && globalDuration > 0) {
                _job?.Stop();

                _job = JobSequence.Create()
                    .Delay(globalDuration * speed)
                    .Action(Rewind)
                    .Action(Play)
                    .RunFrom(timeDomain.Source);

                return;
            }

            if (globalDuration > 0) {
                _job?.Stop();

                _job = JobSequence.Create()
                    .Delay(globalDuration * speed)
                    .Action(() => {
                        Stop();
                        OnFinished.Invoke();
                    })
                    .RunFrom(timeDomain.Source);
            }
        }

        public void Revert(bool reverted) {
            foreach (var tween in tweens) {
                tween.Reverted = reverted;
            }
        }

        public Tween GetTweenById(string id) {
            return Array.Find(tweens, tween => tween.ID == id);
        }
    }
}
