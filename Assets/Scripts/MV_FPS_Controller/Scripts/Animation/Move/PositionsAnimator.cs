using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Animation.Move {
    
    public class PositionsAnimator {
        
        private PositionsConfig mConfig;
        
        private readonly Transform mContainer;
        private readonly CharacterController mController;
        private readonly SingleCoroutine mJob;

        private float mDiff = 0f;
        private float mStandHeight = 0f;
        private float mCrouchHeight = 0f;
        
        private bool mIsGrounded = false;
        private bool mIsSliding = false;


        public PositionsAnimator(MonoBehaviour player, Transform container, CharacterController controller, PositionsConfig config) {
            mContainer = container;
            mController = controller;
            SetConfig(config);

            mJob = new SingleCoroutine(player);
        }
        
        public void Pause() {
            mJob.Pause();
        }

        public void Resume() {
            mJob.Resume();
        }

        public void SetConfig(PositionsConfig newConfig) {
            mConfig = newConfig;
        }

        public void SetStandHeight(float height) {
            mStandHeight = height;
            mDiff = mStandHeight - mCrouchHeight;
        }
        
        public void SetCrouchHeight(float height) {
            mCrouchHeight = height;
            mDiff = mStandHeight - mCrouchHeight;
        }
        
        public void SetIsGrounded(bool isGrounded) {
            mIsGrounded = isGrounded;
        }

        public void SetIsSliding(bool isSliding) {
            mIsSliding = isSliding;
        }
        
        public void OnCrouch() {
            if (mConfig.enabled) AnimatedCrouch();
            else InstantCrouch();
        }

        public void OnStand() {
            if (mConfig.enabled) AnimatedStand();
            else InstantStand();
        }
        
        private void AnimatedCrouch() {
            var duration = DurationForCrouch();
            if (duration <= 0f) return;

            ChangePosition(mCrouchHeight, duration, mConfig.crouchCurve, -1f);
        } 
        
        private void AnimatedStand() {
            var duration = DurationForStand();
            if (duration <= 0f) return;
                
            ChangePosition(mStandHeight, duration, mConfig.standCurve, 0f);
        } 

        private void InstantCrouch() {
            var offset = Vector3.down * mDiff;
            mController.height = mCrouchHeight;
            mController.center = offset / 2f;
            mContainer.localPosition = offset;
            if (!mIsGrounded && !mIsSliding) mController.Move(-offset);
        }
        
        private void InstantStand() {
            mController.height = mStandHeight;
            mController.center = Vector3.zero;
            mContainer.localPosition = Vector3.zero;
            if (!mIsGrounded && !mIsSliding) mController.Move(Vector3.down * mDiff);
        }
        
        private float DurationForCrouch() {
            return (mController.height - mCrouchHeight) / mDiff * mConfig.crouchDuration;
        }

        private float DurationForStand() {
            return (mStandHeight - mController.height) / mDiff * mConfig.standDuration;
        }

        private void ChangePosition(float targetHeight, float duration, AnimationCurve curve, float offset) {
            var initialHeight = mController.height;
            var toTargetHeight = Mathf.Abs(targetHeight - initialHeight);
            var height = initialHeight;
            
            float newHeight;
            Vector3 tillStand;
            
            mJob.OverrideAndStart(
                getProcess: time => time / duration,
                block: process => {
                    newHeight = (curve.Evaluate(process) + offset) * toTargetHeight + initialHeight;
                    tillStand = Vector3.down * (mStandHeight - newHeight);
                
                    mController.height = newHeight;
                    mController.center = tillStand / 2f;
                    mContainer.localPosition = tillStand;
                    if (!mIsGrounded && !mIsSliding) mController.Move(Vector3.up * (height - newHeight));

                    height = newHeight;
                }
            );
        }

    }
    
}