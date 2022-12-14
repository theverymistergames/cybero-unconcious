using System;
using System.Threading;
using MisterGames.Common.Attributes;
using MisterGames.Tick.Core;
using MisterGames.Tick.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tween
{
    public class TweenController : MonoBehaviour, ITweenController
    {
        [SerializeField] private PlayerLoopStage _timeSourceStage = PlayerLoopStage.Update;

        [SerializeField] private float globalDelay;
        [SerializeField] private float globalDuration;
        [SerializeField] private Vector2 globalSpeedRange = new Vector2(1, 1);
        [SerializeField] private bool autoStart = true;
        [SerializeField] private bool loop;
        [SerializeField] private bool chain; //TODO
        [SerializeField][SubclassSelector][SerializeReference] private Tween[] tweens;

        public event Action OnFinished = delegate { };

        private Job _job;
        
        private void Awake() {
            foreach (var tween in tweens) {
                tween.Init(gameObject, _timeSourceStage);
            }
        }

        private void OnDestroy() {
            _job.Dispose();
        }

        private void Start() {
            Rewind();

            if (autoStart) StartPlayAfterDelay(globalDelay);
        }

        private void StartPlayAfterDelay(float delay) {
            _job.Dispose();

            _job = JobSequence.Create(_timeSourceStage)
                .Delay(delay)
                .Action(Play)
                .Push()
                .Start();
        }

        public void Pause() {
            _job.Stop();
            
            foreach (var tween in tweens) {
                tween.Pause();
            }
        }

        public void Stop() {
            _job.Stop();
            
            foreach (var tween in tweens) {
                tween.Stop();
            }
        }

        public void Resume() {
            _job.Start();
            
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
                _job.Dispose();

                _job = JobSequence.Create(_timeSourceStage)
                    .Delay(globalDuration * speed)
                    .Action(Rewind)
                    .Action(Play)
                    .Push()
                    .Start();

                return;
            }

            if (globalDuration > 0) {
                _job.Dispose();

                _job = JobSequence.Create(_timeSourceStage)
                    .Delay(globalDuration * speed)
                    .Action(() => {
                        Stop();
                        OnFinished.Invoke();
                    })
                    .Push()
                    .Start();
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
