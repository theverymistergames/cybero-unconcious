using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Animation.Move {
    
    public class LeanTiltAnimator {
        
        private readonly Transform mContainer;
        private readonly SingleCoroutine mJob;
        private LeanTiltConfig mConfig;

        private float mLastTilt = 0f;
        private bool mIsLeanActive = false;
        private bool mIsGrounded = false;
        private bool mIsSliding = false;
        private bool mIsMoving = false;
        private bool mIsRunning = false;


        public LeanTiltAnimator(MonoBehaviour player, Transform container, LeanTiltConfig config) {
            mContainer = container;
            SetConfig(config);
            mJob = new SingleCoroutine(player);
        }
        
        public void Pause() {
            mJob.Pause();
        }

        public void Resume() {
            mJob.Resume();
        }

        public void SetConfig(LeanTiltConfig newConfig) {
            mConfig = newConfig;
        }

        public void SetIsGrounded(bool isGrounded) {
            mIsGrounded = isGrounded;
            CheckLean();
        }
        
        public void SetIsSliding(bool isSliding) {
            mIsSliding = isSliding;
            CheckLean();
        }

        public void OnTargetSpeedChanged(float normalizedSpeed) {
            mIsMoving = normalizedSpeed > 0f;
            mIsRunning = normalizedSpeed >= 1f;
            CheckLean();
        }

        public void ToInitialPosition() {
            Lean(0f, 0f, 1f);
        }
        
        public void OnTilt(float dir) {
            if (!mConfig.tiltEnabled) return;
            mLastTilt = dir;
            
            if (mIsLeanActive) return;
            Lean(dir, mConfig.tiltAngle, mConfig.tiltDuration);
        }

        public void OnLean(float dir) {
            mIsLeanActive = Mathf.Abs(dir) > 0f && mConfig.leanEnabled && CanLean();
            
            var nextLean = mIsLeanActive ? -dir : mLastTilt;
            var angle = mIsLeanActive ? mConfig.leanAngle : mConfig.tiltAngle;
            var duration = mIsLeanActive ? mConfig.leanDuration : mConfig.tiltDuration;
            
            Lean(nextLean, angle, duration);
        }

        private void CheckLean() {
            if (!mIsLeanActive || CanLean()) return;
            Lean(mLastTilt, mConfig.tiltAngle, mConfig.tiltDuration);
        }

        private bool CanLean() {
            if (!mIsGrounded) {
                return mIsSliding 
                    ? mConfig.canLeanWhileSliding 
                    : mConfig.canLeanWhileJumping;
            }
            
            if (mIsMoving) {
                return mIsRunning 
                    ? mConfig.canLeanWhileRunning 
                    : mConfig.canLeanWhileMoving;
            }

            return true;
        }

        private void Lean(float k, float angle, float duration) {
            var from = mContainer.localEulerAngles.SignedAngles();
            var to = Vector3.forward * (angle * k);
            
            mJob.OverrideAndStart(
                getProcess: time => time / duration,
                block: process => {
                    mContainer.localEulerAngles = mConfig.leanTiltCurve.Evaluate(process) * (to - from) + from;
                }
            );
        }
        
    }
    
}