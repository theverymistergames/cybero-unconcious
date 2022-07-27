using System.Collections.Generic;
using JetBrains.Annotations;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Support {
    
    /// <summary>
    ///     Provides motion and rotation when Collider stays in trigger zone to targets
    ///     which implement <see cref="IExternalMovable"/> interface.
    ///     Note that motion and rotation are measured in LateUpdate.
    /// </summary>
    [AddComponentMenu("MV FPS Controller/MovingPlatformSupport")]
    public class MovingPlatformSupport : MonoBehaviour, ISubscription<IExternalMovable> {
        
        private readonly List<IExternalMovable> mTargets = new List<IExternalMovable>();
        private Transform mPlatform;
        private MotionMeter mMotionMeter;
        private bool mIsActive = true;

        
        private void Awake() {
            mPlatform = transform;
            mMotionMeter = new MotionMeter(mPlatform);
        }

        private void OnDestroy() {
            NotifyAllFinish();
            mTargets.Clear();
        }

        private void OnEnable() {
            mIsActive = true;
        }

        private void OnDisable() {
            mIsActive = false;
            NotifyAllFinish();
        }

        private void LateUpdate() {
            mMotionMeter.Update();
            if (mIsActive) {
                NotifyAllMotion(mMotionMeter.Motion, mMotionMeter.Rotation);
            }
        }

        private void OnTriggerEnter(Collider other) {
            var target = GetExternalMovable(other);
            target?.OnStartExternalMotion(this);
        }

        private void OnTriggerExit(Collider other) {
            var target = GetExternalMovable(other);
            target?.OnFinishExternalMotion(this);
        }

        void ISubscription<IExternalMovable>.Subscribe(IExternalMovable subscriber) {
            mTargets.Add(subscriber);
        }

        void ISubscription<IExternalMovable>.Unsubscribe(IExternalMovable subscriber) {
            mTargets.Remove(subscriber);
        }

        private void NotifyAllMotion(Vector3 motion, Vector3 rotation) {
            if (motion == Vector3.zero && rotation == Vector3.zero) return;
            mTargets.ForEach(target => NotifyMotion(target, motion, rotation));
        }

        private void NotifyMotion(IExternalMovable target, Vector3 motion, Vector3 rotation) {
            target.ExternalMove(mPlatform, motion);
            target.ExternalRotate(mPlatform, rotation);
        }

        private void NotifyAllFinish() {
            foreach (var target in mTargets) {
                target.OnFinishExternalMotion(this);
            }
        }

        [CanBeNull]
        private static IExternalMovable GetExternalMovable(Component component) {
            return component.GetComponent<IExternalMovable>();
        }
        
    }
    
}