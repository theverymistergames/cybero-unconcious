using MV_FPS_Controller.Scripts.Config;
using UnityEngine;
using static MV_FPS_Controller.Scripts.Util.MathUtils;

namespace MV_FPS_Controller.Scripts.Player.Movement {
    
    public class LookHelper {
        
        private readonly Transform mPlayer;
        private readonly Transform mCamera;
        private LookConfig mConfig;

        private Vector2 mLookInput = Vector2.zero;
        private Vector2 mLook = Vector2.zero;

        private float mHorizontalClampFrom = 0f;
        private float mHorizontalClampTo = 0f;
        private bool mIsHorizontalClampEnabled = false;

        private bool mLocked = false;
        private bool mLockedOnTransform = false;
        private Transform mLockTarget;
        private Vector3 mLockTargetPosition;
        private bool _isHudActive = false;

        public LookHelper(Transform player, Transform camera, LookConfig config) {
            mPlayer = player;
            mCamera = camera;
            
            SetConfig(config);
            InitLook();
        }

        public void SetConfig(LookConfig newConfig) {
            mConfig = newConfig;
            CheckHorizontalClamp();
        }

        private void CheckHorizontalClamp() {
            if (mConfig.horizontalClampEnabled == mIsHorizontalClampEnabled) return;
            mIsHorizontalClampEnabled = mConfig.horizontalClampEnabled;
            
            mHorizontalClampFrom = mLookInput.y - mConfig.horizontalClamp;
            mHorizontalClampTo = mLookInput.y + mConfig.horizontalClamp;
        }

        public void Pause() {
            Cursor.lockState = CursorLockMode.None;
        }

        public void Resume() {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void SetIsHudActive(bool isHudActive)
        {
            _isHudActive = isHudActive;
            if (isHudActive) return;

            var cameraRotatedY = mCamera.localEulerAngles.y;
            mCamera.localRotation = Quaternion.Euler(mCamera.eulerAngles.x, 0f, 0f);
            mPlayer.Rotate(0f, cameraRotatedY, 0f);
        }
        
        public void LookAt(Vector3 target) {
            ReleaseLock();
            LookAtDirectionInternal(target - mPlayer.position);
        }

        public void LookAtDirection(Vector3 direction) {
            ReleaseLock();
            LookAtDirectionInternal(direction);
        }

        public void LockOn(Transform target) {
            mLocked = true;
            mLockedOnTransform = true;
            mLockTarget = target;
            mLockTargetPosition = mLockTarget.position;
        }

        public void LockOn(Vector3 target) {
            mLocked = true;
            mLockedOnTransform = false;
            mLockTargetPosition = target;
        }

        private void LookAtDirectionInternal(Vector3 direction) {
            var rotation = Quaternion.LookRotation(direction).eulerAngles;
            var diff = new Vector2(rotation.x - mLookInput.x, rotation.y - mLookInput.y).SignedAngles();
            mLookInput += diff;
        }

        public void ReleaseLock() {
            mLocked = false;
        }

        private void InitLook() {
            Resume();
            ReleaseLock();

            var initialVertical = mCamera.localEulerAngles.SignedAngles().x;
            var initialHorizontal = mPlayer.localEulerAngles.SignedAngles().y;

            if (initialVertical < -90f || 90f < initialVertical) {
                initialVertical -= Mathf.Sign(initialVertical) * 90f;
                initialHorizontal += 180f;
            }
            
            mCamera.localRotation = Quaternion.Euler(initialVertical, 0f, 0f);
            mPlayer.localRotation = Quaternion.Euler(0f, initialHorizontal, 0f);
            
            mLookInput.x = initialVertical;
            mLookInput.y = initialHorizontal;
            
            mLook = mLookInput;
        }

        public void OnLook(Vector2 delta) {
            ClampInput(delta);
        }

        public void UpdateLook() {
            if (mLocked) {
                if (mLockedOnTransform) mLockTargetPosition = mLockTarget.position;
                LookAtDirectionInternal(mLockTargetPosition - mPlayer.position);
            }
            InputToLook();
        }

        private void InputToLook() {
            var prevLook = mLook;
            mLook = mConfig.smoothEnabled
                ? Vector2.Lerp(mLook, mLookInput, Time.deltaTime * mConfig.smoothAcceleration)
                : mLookInput;
            
            Rotate(mLook - prevLook);
        }

        private void Rotate(Vector2 rotation) {
            if (_isHudActive) {
                mCamera.Rotate(rotation.x, rotation.y, 0f);
                mCamera.rotation = Quaternion.Euler(mCamera.eulerAngles.x, mCamera.eulerAngles.y, 0f);
                return;
            }
            mCamera.Rotate(rotation.x, 0f, 0f);
            mPlayer.Rotate(0f, rotation.y, 0f);
        }

        private void ClampInput(Vector2 delta) {
            mLookInput.x -= delta.y;
            mLookInput.y += delta.x;

            if (mConfig.verticalClampEnabled) {
                mLookInput.x = Mathf.Clamp(mLookInput.x, -mConfig.verticalClamp, mConfig.verticalClamp);
            }
            
            if (mIsHorizontalClampEnabled) {
                mLookInput.y = Mathf.Clamp(mLookInput.y, mHorizontalClampFrom, mHorizontalClampTo);
            }
        }
        
    }
    
}