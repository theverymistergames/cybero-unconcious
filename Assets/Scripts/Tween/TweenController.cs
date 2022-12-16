using System;
using UnityEngine;

namespace Tween
{
    [Obsolete("Need to replace TweenController with MisterGames.Tweens.Core.TweenRunner")]
    public class TweenController : MonoBehaviour, ITweenController
    {
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
