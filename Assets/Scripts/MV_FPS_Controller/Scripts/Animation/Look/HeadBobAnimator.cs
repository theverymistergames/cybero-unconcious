using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MV_FPS_Controller.Scripts.Animation.Look {
    
    public class HeadBobAnimator {
        
        private readonly Transform mContainer;
        
        private HeadBobConfig mConfig;

        private Vector3 mTarget;
        private Vector3 mRotationTarget;
        
        private float mPointer = 0f;
        private float mTargetDir = 1f;
        
        private bool mIsGrounded = false;
        private bool mIsMoving = false;
        private float mStepLength = 0f;


        public HeadBobAnimator(Transform container, HeadBobConfig config) {
            mContainer = container;
            SetConfig(config);
            
            mTarget = NextTarget(0.5f);
            mRotationTarget = RotationTargetFrom(mTarget);
        }

        public void SetConfig(HeadBobConfig newConfig) {
            mConfig = newConfig;
        }
        
        public void SetIsGrounded(bool isGrounded) {
            mIsGrounded = isGrounded;
            if (!mIsGrounded) mPointer = 0f;
        }

        public void SetIsMoving(bool isMoving) {
            mIsMoving = isMoving;
            if (!mIsMoving) mPointer = 0f;
        }

        public void SetStepLength(float length) {
            mStepLength = length;
        }

        public void ToInitialPosition() {
            mContainer.localPosition = Vector3.zero;
            mContainer.localEulerAngles = Vector3.zero;
        }
        
        public void UpdateCycle(float magnitude, float normalizedMagnitude) {
            var position = Vector3.zero;
            var rotation = Vector3.zero;

            if (mConfig.enabled && mIsGrounded && mIsMoving && magnitude > 0f) {
                var process = Process(magnitude, normalizedMagnitude);
                var value = mConfig.headBobCurve.Evaluate(process);
                
                position = value * mConfig.offsetMultiplier * mTarget;
                rotation = value * mConfig.rotationMultiplier * mRotationTarget;   
            }
            else {
                mPointer = 0f;
            }

            var speed = mConfig.smoothing * Time.deltaTime;

            mContainer.localPosition = Vector3.Lerp(mContainer.localPosition, position, speed);
            mContainer.localEulerAngles = Vector3.Lerp(mContainer.localEulerAngles.SignedAngles(), rotation, speed);
        }

        private float Process(float magnitude, float normalizedMagnitude) {
            var duration = mStepLength / magnitude;

            if (mPointer < duration) mPointer += Time.deltaTime;
            else {
                mPointer = 0f;
                mTarget = NextTarget(normalizedMagnitude);
                mRotationTarget = RotationTargetFrom(mTarget);
            }
            
            return mPointer / duration;
        }

        private Vector3 NextTarget(float normalizedMagnitude) {
            mTargetDir = -mTargetDir;
            
            var amount = mConfig.amountMinimum + mConfig.amountBySpeed.Evaluate(normalizedMagnitude) * mConfig.amountMultiplier;
            
            var random = Random.insideUnitSphere * mConfig.random;
            random.y = -Mathf.Abs(random.y);
            
            return (Vector3.right * mTargetDir + Vector3.down + random) * amount;
        }

        private Vector3 RotationTargetFrom(Vector3 to) {
            return mConfig.rotationMultiplier * (
                to.x * mConfig.rotationYMultiplier * Vector3.up + 
                to.y * mConfig.rotationXMultiplier * Vector3.left + 
                to.z * mConfig.rotationZMultiplier * Vector3.forward
            );
        }
        
    }
    
}