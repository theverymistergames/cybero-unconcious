using System;
using System.Collections;
using System.Collections.Generic;
using MisterGames.Common.Attributes;
using MisterGames.Common.Routines;
using Unity.VisualScripting;
using UnityEngine;
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
        [SerializeField] private TimeDomain timeDomain;

        public event Action OnFinished = delegate { };
        
        private readonly SingleJobHandler _handler = new SingleJobHandler();
        
        private void Awake() {
            if (!timeDomain) return;
            
            foreach (var tween in tweens) {
                tween.Init(gameObject, timeDomain);
                // tween.Domain = timeDomain;
            }
        }

        private void Start() {
            Rewind();
            
            if (autoStart) {
                Jobs.Do(timeDomain.Delay(globalDelay)).Then(Play).StartFrom(_handler);
            }
        }

        public void Pause() {
            _handler.Pause();
            
            foreach (var tween in tweens) {
                tween.Pause();
            }
        }

        public void Stop() {
            _handler.Stop();
            
            foreach (var tween in tweens) {
                tween.Stop();
            }
        }

        public void Resume() {
            _handler.Resume();
            
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
                Jobs.Do(timeDomain.Delay(globalDuration * speed)).Then(Rewind).Then(Play).StartFrom(_handler);
            } else if (globalDuration > 0) {
                Jobs.Do(timeDomain.Delay(globalDuration * speed)).Then(() => {
                    Stop();
                    OnFinished.Invoke();
                }).StartFrom(_handler);
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