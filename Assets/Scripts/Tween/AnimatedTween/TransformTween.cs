using System;
using MisterGames.Tick.Core;
using UnityEngine;

namespace Tween {

    public enum TransformTypes {
        Position,
        Rotation,
        Scale
    }
    
    [Serializable]
    public class TransformTween : AnimatedTween {

        [SerializeField] private TransformTypes type = TransformTypes.Position;
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 end;
        [SerializeField] private bool local = true;

        private Transform _transform;

        public void SetStartPosition(Vector3 pos) {
            start = pos;
        }
        
        public void SetEndPosition(Vector3 pos) {
            end = pos;
        }

        public override void Init(GameObject gameobj, PlayerLoopStage stage) {
            base.Init(gameobj, stage);

            _transform = tweenableObject.transform;
        }

        protected override void ProceedUpdate(float progress) {
            var value = GetCurveValue(progress);
            var point = Vector3.Lerp(start, end, value);
            
            switch (type) {
                case TransformTypes.Position:
                    if (local) _transform.localPosition = point;
                    else _transform.position = point;
                    break;
                case TransformTypes.Rotation:
                    if (local) _transform.localEulerAngles = point;
                    else _transform.eulerAngles = point;
                    break;
                default:
                    _transform.localScale = point;
                    break;
            }
        }
    }

}
