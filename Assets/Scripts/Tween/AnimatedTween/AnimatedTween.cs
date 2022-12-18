using System;
using MisterGames.Tick.Core;
using Tweens.Easing;
using UnityEngine;

namespace Tween {

    [Serializable]
    public abstract class AnimatedTween : Tween {

        [SerializeField] private EasingType easing = EasingType.Linear;
        
        private AnimationCurve _curve;

        public override void Init(GameObject gameobj, PlayerLoopStage stage) {
            base.Init(gameobj, stage);

            _curve = easing.ToAnimationCurve();
        }

        protected float GetCurveValue(float value) {
            return _curve.Evaluate(value);
        }
    }
}
