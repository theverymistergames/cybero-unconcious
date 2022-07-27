using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MV_FPS_Controller.Scripts.Animation.Look {
    
    public class BreathAnimator {
        
        private readonly Transform mContainer;
        private readonly SingleCoroutine mJob;
        
        private BreathConfig mConfig;

        private bool mEnabledConditions = false;
        private bool mIsGrounded = false;
        
        private float mEnergy = 1f;
        private float mAmount;


        public BreathAnimator(MonoBehaviour player, Transform container, BreathConfig config) {
            mContainer = container;
            mConfig = config;
            InitParams();

            mJob = new SingleCoroutine(player);
        }

        public void Pause() {
            mJob.Pause();
        }

        public void Resume() {
            mJob.Resume();
        }
        
        public void SetConfig(BreathConfig newConfig) {
            mConfig = newConfig;
            InitParams();
            if (!mConfig.enabled) ToInitialPosition();
        }

        public void ToInitialPosition() {
            Breath(Vector3.zero, 1f);
        }

        private void InitParams() {
            mEnabledConditions = mIsGrounded || mConfig.enabledWhileFalling;
            InitAmount();
        }

        public void SetEnergy(float energy) {
            mEnergy = energy;
        }
        
        public void SetIsGrounded(bool isGrounded) {
            mIsGrounded = isGrounded;
            mEnabledConditions = mIsGrounded || mConfig.enabledWhileFalling;
            
            if (!mEnabledConditions) ToInitialPosition();
        }

        public void OnInhale(float period, float amplitude) {
            OnNextPeriod(period, amplitude);
        }

        public void OnExhale(float period, float amplitude) {
            OnNextPeriod(period, -amplitude);
        }

        private void OnNextPeriod(float period, float amplitude) {
            if (!mConfig.enabled || !mEnabledConditions) return;
            
            InitAmount();
            Breath(NextTarget(amplitude), period);
        }

        private Vector3 NextTarget(float amplitude) {
            var random = Random.insideUnitSphere * mConfig.random;
            return (Vector3.up + random) * (mAmount * amplitude);
        }

        private void InitAmount() {
            mAmount = mConfig.amountMinimum + mConfig.amountMultiplier * mConfig.amountByEnergy.Evaluate(mEnergy);
        }

        private void Breath(Vector3 to, float duration) {
            var fromAngles = mContainer.localEulerAngles.SignedAngles();
            var toAngles = (to.y * Vector3.left + to.x * Vector3.forward) * mConfig.rotationMultiplier;

            to *= mConfig.offsetMultiplier;
            var from = mContainer.localPosition;
            
            mJob.OverrideAndStart(
                getProcess: time => time / duration,
                block: process => {
                    var value = mConfig.breathCurve.Evaluate(process);
                    mContainer.localPosition = value * (to - from) + from;
                    mContainer.localEulerAngles = value * (toAngles - fromAngles) + fromAngles;
                }
            );
        }

    }
    
}