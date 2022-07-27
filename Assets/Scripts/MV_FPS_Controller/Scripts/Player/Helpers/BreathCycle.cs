using System;
using MV_FPS_Controller.Scripts.Config;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Helpers {
    
    public class BreathCycle {
        
        public event Action<float, float> OnInhale = delegate {  };
        public event Action<float, float> OnExhale = delegate {  };
        public event Action OnStopBreath = delegate {  };
        
        private BreathCycleConfig mConfig;

        private float mPointer = 0f;
        private float mPeriod = 0f;
        private float mNextPeriod = 0f;
        private float mEnergy = 1f;
        
        private bool mInhale = true;
        private bool mNotify = true;
        private float mAmplitude = 0f;


        public BreathCycle(BreathCycleConfig config) {
            SetConfig(config);
            
            mPeriod = mNextPeriod;
            mPointer = mPeriod;
        }
        
        public void SetConfig(BreathCycleConfig newConfig) {
            mConfig = newConfig;
            InitPeriod();

            if (mConfig.enabled) return;
            mPointer = 0f;
            OnStopBreath.Invoke();
        }
        
        public void SetIsGrounded(bool isGrounded) {
            mNotify = isGrounded || mConfig.enabledWhileFalling;
            
            if (mNotify) return;
            mPointer = 0f;
            OnStopBreath.Invoke(); 
        }

        public void OnEnergyChanged(float value) {
            mEnergy = value;
            InitPeriod();
        }

        private void InitPeriod() {
            mNextPeriod = mConfig.periodMultiplier * mConfig.periodByEnergy.Evaluate(mEnergy) + mConfig.periodMinimum;
        }

        public void UpdateCycle(float normalizedMagnitude) {
            if (!mConfig.enabled || !mNotify) return;

            if (mPointer < mPeriod) {
                mPointer += Time.deltaTime;
                return;
            }

            mPointer = Time.deltaTime;
            mPeriod = mNextPeriod;

            if (mNotify) OnBreath(normalizedMagnitude);
        }

        private void OnBreath(float normalizedMagnitude) {
            mAmplitude = Mathf.Clamp(mConfig.amplitudeBySpeed.Evaluate(normalizedMagnitude), 0f, 1f);

            if (mInhale) OnInhale.Invoke(mPeriod, mAmplitude);
            else OnExhale.Invoke(mPeriod, mAmplitude);

            mInhale = !mInhale;
        }
        
    }
    
}
