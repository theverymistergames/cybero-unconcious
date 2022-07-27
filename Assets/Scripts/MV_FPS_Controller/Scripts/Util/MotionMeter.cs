using UnityEngine;

namespace MV_FPS_Controller.Scripts.Util {
    
    /// <summary>
    ///     Measures motion and rotation deltas of given Transform between Update method calls.
    /// </summary>
    public class MotionMeter {
        
        /// <summary>
        ///     <para>Motion delta after last Update call.</para>
        /// </summary>
        public Vector3 Motion { get; private set; } = Vector3.zero;
        
        /// <summary>
        ///     <para>Rotation delta in Euler angles after last Update call.</para>
        /// </summary>
        public Vector3 Rotation { get; private set; } = Vector3.zero;

        private readonly Transform mTarget;
        private Vector3 mLastPosition;
        private Vector3 mLastRotation;
        
        
        public MotionMeter(Transform target) {
            mTarget = target;
            mLastPosition = mTarget.localPosition;
            mLastRotation = mTarget.eulerAngles.SignedAngles();
        }

        /// <summary>
        ///     Calculate motion and rotation deltas since last call.
        /// </summary>
        public void Update() {
            CalculateMotion();
            CalculateRotation();
        }

        private void CalculateMotion() {
            var position = mTarget.localPosition;
            Motion = position - mLastPosition;
            mLastPosition = position;
        }

        private void CalculateRotation() {
            var rotation = mTarget.eulerAngles.SignedAngles();
            Rotation = rotation - mLastRotation;
            mLastRotation = rotation;
        }

    }
    
}