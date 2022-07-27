using System;
using MV_FPS_Controller.Scripts.Config;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Helpers {
    
    public class EnergyMeter {
        
        public event Action<float> OnEnergyChanged = delegate {  };
        
        private EnergyConfig mConfig;
        
        private float mEnergy = 1f;
        private bool mIsRunning = false;


        public EnergyMeter(EnergyConfig config) {
            SetConfig(config);
        }
        
        public void SetConfig(EnergyConfig newConfig) {
            mConfig = newConfig;
        }

        public void InitEnergy() {
            OnEnergyChanged(mEnergy);
        }

        public void SetIsRunningForward(bool isRunning) {
            mIsRunning = isRunning;
        }
        
        public void UpdateEnergy(float normalizedMagnitude) {
            if (!mConfig.consumeOnRun) return;
            
            if (mIsRunning && normalizedMagnitude > 0.8f) {
                Consume();
                return;
            }
            
            if (mConfig.recoverOnIdleOnly && normalizedMagnitude > 0f) return;
            Recover();
        }

        public void SetEnergy(float energy) {
            var prevEnergy = mEnergy;
            mEnergy = Mathf.Clamp(energy, 0f, 1f);
            
            if (mEnergy >= 1f && prevEnergy >= 1f || mEnergy <= 0f && prevEnergy <= 0f) return;
            OnEnergyChanged(mEnergy);
        }
        
        public void AddEnergy(float addition) {
            SetEnergy(mEnergy + addition);
        }

        private void Consume() {
            var k = mConfig.consumptionSpeedByEnergy.Evaluate(mEnergy) * mConfig.consumeSpeedOnRunMultiplier + mConfig.consumeSpeedOnRunMin;
            AddEnergy(-k * Time.deltaTime);
        }

        private void Recover() {
            var k = mConfig.recoverySpeedByEnergy.Evaluate(mEnergy) * mConfig.recoverySpeedMultiplier + mConfig.recoverySpeedMin;
            AddEnergy(k * Time.deltaTime);
        }

    }
    
}