using System;
using MV_FPS_Controller.Scripts.Config;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Helpers {
    
    public class StepsCycle {
        
        public event Action<float> OnStep = delegate {  };

        private StepCycleConfig mConfig;

        private float mStepMotion = 0f;
        private float mStepLength;
        private bool mIsGrounded = false;


        public StepsCycle(StepCycleConfig config) {
            SetConfig(config);
        }
        
        public void SetConfig(StepCycleConfig newConfig) {
            mConfig = newConfig;
        }

        public void OnTargetSpeedChanged(float normalizedMagnitude) {
            if (normalizedMagnitude > 0f && mStepMotion <= 0f) {
                OnStep(CalculateStepLength(normalizedMagnitude));
            }
        }

        public void SetIsGrounded(bool value) {
            mIsGrounded = value;
        }
        
        public void UpdateCycle(float magnitude, float normalizedMagnitude) {
            if (!mConfig.enabled) return;

            mStepLength = CalculateStepLength(normalizedMagnitude);

            if (!mIsGrounded || magnitude <= 0f || mStepLength <= 0f) {
                mStepMotion = 0f;
                return;
            }

            if (mStepMotion < mStepLength) {
                mStepMotion += magnitude * Time.deltaTime;
                return;
            }
            
            mStepMotion = 0f;
            OnStep.Invoke(mStepLength);
        }

        private float CalculateStepLength(float speed) {
            return mConfig.lengthMultiplier * mConfig.lengthBySpeed.Evaluate(speed) + mConfig.minLength;
        }

    }
    
}
