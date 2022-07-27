using System;
using MisterGames.Common.Attributes;
using MisterGames.Common.Routines;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tween {
    [Serializable]public class SimpleTweenController : MonoBehaviour, ITweenController {
        
        [SerializeField][SubclassSelector][SerializeReference] private Tween tween;
        [SerializeField] private TimeDomain timeDomain;

        public event Action OnFinished = delegate { };
        
        private void Awake() {
            if (!timeDomain) return;
            
            tween.Init(gameObject, timeDomain);
        }

        private void Start() {
            Rewind();
            
            if (tween.IsAutoStart) {
                Play();
            }
        }

        public void Pause() {
            tween.Pause();
        }

        public void Stop() {
            tween.Stop();
        }

        public void Resume() {
            if (tween.Paused) tween.Resume();
        }

        public void Rewind() {
            tween.Rewind();
        }

        public void Wind() {
            tween.Wind();
        }

        public void Play() {
            tween.Play();
        }

        public void Revert(bool reverted) {
            tween.Reverted = reverted;
        }

        public Tween GetTween() {
            return tween;
        }
    }
}