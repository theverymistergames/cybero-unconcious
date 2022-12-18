using System;
using MisterGames.Tick.Core;
using UnityEngine;

namespace Tween {

    [Serializable]
    public class MaterialTween : AnimatedTween {

        [SerializeField] private string fieldName;
        [SerializeField] private float startValue = 0;
        [SerializeField] private float endValue = 1;
        [SerializeField] private bool useColor;

        private Material _material;
        private Color _color;

        public override void Init(GameObject gameobj, PlayerLoopStage stage) {
            base.Init(gameobj, stage);

            var renderer = tweenableObject.GetComponent<Renderer>();
            
            if (renderer == null) {
                Debug.LogWarning("No mesh renderer detected");
                return;
            }
            
            _material = new Material(renderer.sharedMaterial);
            renderer.material = _material;
            _color = _material.color;
            
            if (_material == null) {
                Debug.LogWarning("No material");
                return;
            }
        }

        protected override void ProceedUpdate(float progress) {
            if (_material == null) return;

            var value = GetCurveValue(progress);
            
            if (useColor) {
                _material.SetColor(fieldName, Mathf.Lerp(startValue, endValue, value) * _color);
            } else {
                _material.SetFloat(fieldName, Mathf.Lerp(startValue, endValue, value));
            }
        }
    }

}
