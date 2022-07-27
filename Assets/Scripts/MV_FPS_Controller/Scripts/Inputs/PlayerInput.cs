using MV_FPS_Controller.Scripts.Config;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Inputs {
    
    /// <summary>
    ///     Takes input and passes it to the <see cref="InputReceiver"/>.
    /// </summary>
    [AddComponentMenu("MV FPS Controller/PlayerInput")]
    public class PlayerInput : MonoBehaviour {

        public InputReceiver inputReceiver;
        public PlayerInputConfig config;
        
        private Vector2 mMouseDelta = Vector2.zero;
        private Vector2 mMove = Vector2.zero;

        private int mForwardDir = 0;
        private int mRightDir = 0;
        
        private int mLeanDir = 0;
        
        private bool mIsRunActive = false;
        private bool mIsCrouchActive = false;
        private bool mIsJumpActive = false;
        
        
        private void Update() {
            Look();
            Lean();
            Move();
            Run();
            Crouch();
            Jump();
            Hud();
        }

        private void Look() {
            mMouseDelta.x = Input.GetAxis("Mouse X") * config.horizontalSensitivity;
            mMouseDelta.y = Input.GetAxis("Mouse Y") * config.verticalSensitivity;

            if (config.inverseVerticalAxis) {
                mMouseDelta.y = -mMouseDelta.y;
            }

            if (mMouseDelta == Vector2.zero) return;
            inputReceiver.OnLook(mMouseDelta);
        }
        
        private void Move() {
            var isForwardActive = Input.GetKey(config.forward);
            var isBackActive = Input.GetKey(config.back);
            
            var prevForwardDir = mForwardDir;
            mForwardDir = isForwardActive == isBackActive 
                ? 0
                : isForwardActive ? 1 : -1;
            
            var isLeftActive = Input.GetKey(config.left);
            var isRightActive = Input.GetKey(config.right);
            
            var prevRightDir = mRightDir;
            mRightDir = isRightActive == isLeftActive 
                ? 0 
                : isRightActive ? 1 : -1;
            
            if (prevForwardDir == mForwardDir && prevRightDir == mRightDir) return;
            
            mMove.x = mRightDir;
            mMove.y = mForwardDir;
            inputReceiver.OnMove(mMove);
        }

        private void Lean() {
            var isLeanLeftActive = Input.GetKey(config.leanLeft);
            var isLeanRightActive = Input.GetKey(config.leanRight);

            var prevLeanDir = mLeanDir;
            mLeanDir = isLeanLeftActive == isLeanRightActive 
                ? 0
                : isLeanRightActive ? 1 : -1;
            
            if (mLeanDir == prevLeanDir) return;
            inputReceiver.OnLean(mLeanDir);
        }

        private void Run() {
            if (config.activateRunOnToggle) {
                if (Input.GetKeyDown(config.run)) inputReceiver.OnToggleRun();
                return;
            }
            
            var wasRunActive = mIsRunActive;
            mIsRunActive = Input.GetKey(config.run);
            
            if (wasRunActive != mIsRunActive) inputReceiver.OnRun(mIsRunActive);
        }

        private void Crouch() {
            if (config.activateCrouchOnToggle) {
                if (Input.GetKeyDown(config.crouch)) inputReceiver.OnToggleCrouch();
                return;
            }
            
            var wasCrouchActive = mIsCrouchActive;
            mIsCrouchActive = Input.GetKey(config.crouch);
            
            if (wasCrouchActive != mIsCrouchActive) inputReceiver.OnCrouch(mIsCrouchActive);
        }

        private void Jump() {
            var wasJumpActive = mIsJumpActive;
            mIsJumpActive = Input.GetKey(config.jump);
            
            if (!wasJumpActive && mIsJumpActive) inputReceiver.OnJump();
        }

        private void Hud()
        {
            if (Input.GetKeyDown(config.hud)) inputReceiver.OnHud();
        }

    }

}