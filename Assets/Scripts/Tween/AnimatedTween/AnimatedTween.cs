using System;
using EasingCurve;
using MisterGames.Common.Routines;
using UnityEngine;

namespace Tween {
    [Serializable]public abstract class AnimatedTween : Tween {
        [SerializeField] private EasingFunctions.Ease easing = EasingFunctions.Ease.Linear;
        
        private AnimationCurve _curve;

        public override void Init(GameObject gameobj, TimeDomain domain) {
            base.Init(gameobj, domain);

            _curve = EasingAnimationCurve.EaseToAnimationCurve(easing);
        }

        protected float GetCurveValue(float value) {
            return _curve.Evaluate(value);
        }
    }
}