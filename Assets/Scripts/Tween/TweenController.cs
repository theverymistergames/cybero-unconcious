using System;
using MisterGames.Common.Attributes;
using MisterGames.Tick.Core;
using UnityEngine;

namespace Tween
{
    [Obsolete("Need to replace TweenController with MisterGames.Tweens.Core.TweenRunner")]
    public class TweenController : MonoBehaviour, ITweenController {

        [SerializeField] private PlayerLoopStage _timeSourceStage = PlayerLoopStage.Update;
        [SerializeField] private float globalDelay;
        [SerializeField] private float globalDuration;
        [SerializeField] private Vector2 globalSpeedRange = new Vector2(1, 1);
        [SerializeField] private bool autoStart = true;
        [SerializeField] private bool loop;
        [SerializeField] private bool chain; //TODO
        [SerializeField][SubclassSelector][SerializeReference] private Tween[] tweens;

        public event Action OnFinished = delegate { };

        private void Awake() {
            Debug.LogError($"TweenController is obsolete: gameObject {gameObject.name}. " +
                           $"Need to replace TweenController with MisterGames.Tweens.Core.TweenRunner");
        }

        public void Pause() {
            throw new NotImplementedException();
        }

        public void Stop() {
            throw new NotImplementedException();
        }

        public void Resume() {
            throw new NotImplementedException();
        }

        public void Rewind() {
            throw new NotImplementedException();
        }

        public void Wind() {
            throw new NotImplementedException();
        }

        public void Play() {
            throw new NotImplementedException();
        }

        public void Revert(bool reverted) {
            throw new NotImplementedException();
        }
    }
}
