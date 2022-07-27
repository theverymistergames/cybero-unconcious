using System;
using MV_FPS_Controller.Scripts.Config;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Movement {
    
    public class MoveHelper {
        
        private readonly CharacterController mController;
        private readonly Transform mPlayer;

        private MoveConfig mConfig;

        private Vector2 mMoveInput = Vector2.zero;
        private Vector2 mTargetMoveInput = Vector2.zero;
        
        private Vector3 mGroundNormal = Vector3.zero;
        private Vector3 mFallDir = Vector3.down;

        private Vector3 mMove = Vector3.zero;
        private Vector3 mJump = Vector3.zero;
        private Vector3 mFall = Vector3.zero;
        
        private Vector3 mTarget = Vector3.zero;
        private Action mOnTargetReached;
        private float mTargetMinDistance = 0f;
        private bool mIsTargeting = false;

        private bool mIsGrounded = false;
        private bool mIsSliding = false;
        

        public MoveHelper(Transform player, CharacterController controller, MoveConfig config) {
            mController = controller;
            mPlayer = player;

            SetConfig(config);
        }

        public void ResetInput() {
            if (mIsGrounded) mMove = Vector3.zero;
            else mJump = Vector3.zero;
        }

        public void SetConfig(MoveConfig newConfig) {
            mConfig = newConfig;
        }

        public void SetTarget(Vector3 target, float speed, float minDistance, Action onTargetReached) {
            mTarget = target;
            mIsTargeting = true;
            mTargetMinDistance = minDistance;
            mOnTargetReached = onTargetReached;
            
            var direction = target - mPlayer.position;
            mMove = direction.normalized * speed;
        }

        public void ReleaseTarget() {
            mTarget = Vector3.zero;
            mIsTargeting = false;
        }

        public void SetGroundNormal(Vector3 normal) {
            if (mGroundNormal == normal) return;
            mGroundNormal = normal;
            if (mIsSliding) OnContinueSlide();
        }

        private void OnContinueSlide() {
            mMove = Vector3.ProjectOnPlane(mMove, mGroundNormal);
            mFall = Vector3.ProjectOnPlane(mFall, mGroundNormal);
            mFallDir = Vector3.ProjectOnPlane(Vector3.down, mGroundNormal).normalized;
        }

        public void OnCollision(ControllerColliderHit hit) {
            if (mIsGrounded) return;
            mMove = Vector3.ProjectOnPlane(mMove, hit.normal);
        }

        public void UpdateMove() {
            UpdateInput();
            
            if (mIsGrounded) UpdateMotionOnGround();
            else UpdateMotionInAir();
            
            CheckTarget();
            mController.Move(Motion());
        }

        private void UpdateInput() {
            mMoveInput = mConfig.smoothEnabled
                ? Vector2.Lerp(mMoveInput, mTargetMoveInput, Time.deltaTime * mConfig.smoothAcceleration)
                : mTargetMoveInput;
        }

        private Vector3 Motion() {
            return (mMove + mJump + mFall) * Time.deltaTime;
        }
        
        private void UpdateMotionOnGround() {
            if (mIsTargeting) return;
            mMove = CurrentMove(mMove);
        }
        
        private void UpdateMotionInAir() {
            mFall += GravityDecrement();
            
            if (mIsTargeting) return;
            mJump = CurrentMove(mJump);
        }

        private void CheckTarget() {
            if (!mIsTargeting) return;
            if (Vector3.Distance(mPlayer.position, mTarget) > mTargetMinDistance) return;
            
            mOnTargetReached?.Invoke();
            mMove = Vector3.zero;
        }

        private Vector3 GravityDecrement() {
            return mFallDir * (mConfig.gravity * Time.deltaTime);
        }

        private Vector3 CurrentMove(Vector3 from) {
            var targetMove = Vector3.ProjectOnPlane(InputToMoveDir(mMoveInput), mGroundNormal);
            
            var currentMove = mConfig.smoothEnabled 
                ? Vector3.Lerp(from, targetMove, Time.deltaTime * mConfig.smoothAcceleration)
                : targetMove;

            return currentMove;
        }

        public void OnMove(Vector2 dir) {
            mTargetMoveInput = dir;
        }

        public void OnJump(float force) {
            mIsGrounded = false;

            mFall = mIsSliding 
                ? mGroundNormal.normalized * force
                : Vector3.up * force;
            
            mMove *= mMove.magnitude / (mMove + mFall).magnitude;

            mIsSliding = false;
        }

        public void OnFell() {
            mIsGrounded = false;
            //mFall = Vector3.zero;
            mFall = Mathf.Abs(mController.velocity.y) * Vector3.down;
        }

        public void OnLanded() {
            mIsGrounded = true;
            mJump = Vector2.zero;
            mMove += Vector3.ProjectOnPlane(mFall, mGroundNormal);
            mFall = Vector3.zero;
        }
        
        public void OnStartSlide() {
            mIsSliding = true;
        }
        
        public void OnStopSlide() {
            mIsSliding = false;
            mFallDir = Vector3.down;
        }

        private Vector3 InputToMoveDir(Vector2 input) {
            return mPlayer.right * input.x + mPlayer.forward * input.y;
        }

    }
    
}