using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Movement {
    
    public class MoveController {
        
        private readonly IMovablePlayer mTarget;
        private PlayerConfig mConfig;

        private Vector2 mInputDir = Vector2.zero;
        private Vector2 mMoveDir = Vector2.zero;
        
        private float mSpeed = 0f;
        private float mSpeedCorr = 0f;

        private bool mCanRun = true;
        
        private bool mIsMoveInputActive = false;
        private bool mIsRunInputActive = false;
        private bool mIsCrouchInputActive = false;
        
        private bool mIsInJump = false;
        private bool mIsGrounded = false;
        private bool mIsSliding = false;
        private bool mHasCeiling = false;
        private bool mIsCrouching = false;

        private int mJumpCounter = 0;

        
        public MoveController(IMovablePlayer target, PlayerConfig config) {
            mTarget = target;
            mConfig = config;
        }

        public void SetConfig(PlayerConfig newConfig) {
            mConfig = newConfig;
            mSpeedCorr = CalcSpeedCorr();
            RevalidateSpeedAndMove();
        }

        public void OnLook(Vector2 delta) {
            mTarget.OnLook(delta);
        }

        public void OnMove(Vector2 delta) {
            mInputDir = Vector2.ClampMagnitude(delta, 1f);
            mIsMoveInputActive = delta.IsNotZero();

            if (!mIsMoveInputActive) {
                OnCancelRun();
            }
            
            mSpeedCorr = CalcSpeedCorr();
            RevalidateSpeedAndMove();
        }

        public void OnLean(int dir) {
            mTarget.OnLean(dir);
        }

        public void OnPerformJump() {
            if (mIsGrounded && mIsCrouching && mConfig.jump.jumpWhileCrouchingCausesStandUp) {
                ToggleCrouch();
                return;
            }
            
            if (!mConfig.jump.canJump) return;
            
            if (mIsGrounded) {
                if (mIsCrouching && !mConfig.jump.canJumpWhileCrouching) return;
            }
            else {
                if (mIsSliding) {
                    if (mIsCrouching) {
                        if (!mConfig.jump.canJumpWhileCrouching || !mConfig.jump.canJumpWhileSliding) return;
                    } 
                    else if (!mConfig.jump.canJumpWhileSliding) return;
                }
                else {
                    if (mConfig.jump.multiJump) {
                        if (mJumpCounter > mConfig.jump.additionalJumps) return;
                    } 
                    else return;
                }
            }

            mIsInJump = true;
            mIsGrounded = false;
            mJumpCounter++;
            
            mTarget.OnJump(CalcJumpForce());
            RevalidateSpeedAndMove();
        }

        public void ToggleCrouch() {
            if (mIsCrouching) {
                if (mHasCeiling) return;
                OnCancelCrouch();
            }
            else OnStartCrouch();
        }
        
        public void OnStartCrouch() {
            mIsCrouchInputActive = true;
            
            if (mIsCrouching) return;
            mIsCrouching = true;
            
            OnCancelRun();
            
            mTarget.OnCrouch();
            RevalidateSpeedAndMove();
        }

        public void OnCancelCrouch() {
            if (!mIsCrouchInputActive) return;
            mIsCrouchInputActive = false;
            
            if (!mIsCrouching || mIsGrounded && mHasCeiling) return;
            mIsCrouching = false;
            
            mTarget.OnStand();
            RevalidateSpeedAndMove();
        }

        public void ToggleRun() {
            if (mIsRunInputActive) {
                OnCancelRun();
            }
            else {
                if (!mIsMoveInputActive || !mCanRun) return;
                OnStartRun();
            }
        }
        
        public void OnStartRun() {
            if (mIsRunInputActive) return;
            mIsRunInputActive = true;

            if (!IsWalkingOrRunning()) return;
            RevalidateSpeedAndMove();
        }

        public void OnCancelRun() {
            if (!mIsRunInputActive) return;
            mIsRunInputActive = false;

            if (IsWalkingOrRunning()) RevalidateSpeedAndMove();
        }

        public void OnFell() {
            if (!mIsGrounded || mIsInJump) return;
            mIsGrounded = false;
            mJumpCounter++;
            
            mTarget.OnFell();
            RevalidateSpeedAndMove();
        }

        public void OnLanded(float force) {
            if (mIsGrounded) return;
            
            mIsGrounded = true;
            mIsInJump = false;
            mJumpCounter = 0;
            
            mTarget.OnLanded(force);
            RevalidateSpeedAndMove();
        }

        public void OnCeilingGone() {
            mHasCeiling = false;
            if (mIsCrouchInputActive || !mIsGrounded || !mIsCrouching) return;
            
            mIsCrouching = false;
            mTarget.OnStand();
            RevalidateSpeedAndMove();
        }

        public void OnCeilingAppeared() {
            mHasCeiling = true;
            if (!mIsGrounded) mTarget.OnFell();
        }

        public void OnStartSlide() {
            mIsSliding = true;
            mJumpCounter = 0;
            mTarget.OnStartSlide();
        }

        public void OnStopSlide() {
            mIsSliding = false;
            mTarget.OnStopSlide();
        }

        public void SetCanRun(bool canRun) {
            if (canRun == mCanRun) return;
            mCanRun = canRun;
            
            RevalidateSpeedAndMove();
        }

        private void RevalidateSpeedAndMove() {
            mSpeed = CalcSpeed();
            mMoveDir = CalcMoveDir();
            mTarget.OnMove(mMoveDir);
        }

        private bool IsWalkingOrRunning() {
            return mIsGrounded && !mIsSliding && !mIsCrouching && mIsMoveInputActive;
        }
        
        private Vector2 CalcMoveDir() {
            return mInputDir * (mSpeed * mSpeedCorr);
        }

        private float CalcJumpForce() {
            return mIsGrounded 
                ? mIsCrouching ? mConfig.jump.forceFromCrouch 
                : mConfig.jump.forceFromStand
                : mConfig.jump.forceInAir;
        }
        
        /**
         * Calculate current speed corresponding to priority of states descending:
         * 1. In air (falling or jumping)
         * 2. Sliding
         * 3. Sneak mode
         * 4. Run mode
         * 5. Is moving
         * 6. Idle
         */
        private float CalcSpeed() {
            return mIsMoveInputActive
                ? !mIsGrounded ? mConfig.move.jumpMoveSpeed
                : mIsCrouching ? mConfig.move.sneakSpeed
                : mIsRunInputActive && mCanRun ? mConfig.move.runSpeed
                : mConfig.move.walkSpeed
                : 0f;
        }

        /**
         * Calculate speed correction depending on the input vector:
         * - if not walking or running - no adjustment;
         * - if backwards input is enabled - significantly decrease speed;
         * - if forward or forward and sideways inputs are enabled - no adjustment;
         * - if only sideways input is enabled - slightly decrease speed.
         */
        private float CalcSpeedCorr() {
            // No adjustment
            if (!IsWalkingOrRunning()) return 1f;
            
            // Moving backwards OR backwards + sideways
            if (mInputDir.y < 0) return mConfig.move.backwardsCorr;
            
            // Moving forwards OR forwards + sideways: no adjustment
            if (mInputDir.y > 0) return 1f;
            
            // Moving sideways only
            return mConfig.move.strafeCorr;
        }

    }

}