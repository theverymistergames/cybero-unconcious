using MV_FPS_Controller.Scripts.Config;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Animation.Look {
    
    public class FovAnimator {
        
        private readonly Camera mCamera;
        private FovConfig mConfig;
        
        private bool mIsMovingForward = false;
        

        public FovAnimator(Camera camera, FovConfig config) {
            mCamera = camera;
            SetConfig(config);
        }
        
        public void SetConfig(FovConfig newConfig) {
            mConfig = newConfig;
            mCamera.fieldOfView = mConfig.idleAngle;
        }

        public void SetIsMovingForward(bool isMovingForward) {
            mIsMovingForward = isMovingForward;
        }

        public void ToInitialPosition() {
            mCamera.fieldOfView = mConfig.idleAngle;
        }
        
        public void UpdateFov(float magnitude) {
            if (!mConfig.enabled) return;

            var fov = CalculateFov(magnitude);
            mCamera.fieldOfView = Mathf.Lerp(mCamera.fieldOfView, fov, Time.deltaTime * mConfig.smoothing);
        }

        private float CalculateFov(float magnitude) {
            return mConfig.idleAngle + (
                mIsMovingForward 
                    ? mConfig.amountBySpeed.Evaluate(magnitude) * mConfig.amountMultiplier 
                    : 0f
            );
        }

    }
    
}