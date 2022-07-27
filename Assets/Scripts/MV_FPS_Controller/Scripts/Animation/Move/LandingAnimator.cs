using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Animation.Move {
    
    public class LandingAnimator {
        
        private readonly Transform mContainer;
        private readonly SingleCoroutine mJob;
        
        private LandingConfig mConfig;


        public LandingAnimator(MonoBehaviour player, Transform container, LandingConfig config) {
            mContainer = container;
            SetConfig(config);
            mJob = new SingleCoroutine(player);
        }
        
        public void Pause() {
            mJob.Pause();
        }

        public void Resume() {
            mJob.Resume();
        }

        public void SetConfig(LandingConfig newConfig) {
            mConfig = newConfig;
        }

        public void OnLanded(float force) {
            if (!mConfig.enabled) return;

            var duration = mConfig.durationByForce.Evaluate(force) * mConfig.durationMultiplier;
            if (duration < mConfig.minDuration) return;
                
            var kSquat = mConfig.squatAmountByForce.Evaluate(force) * mConfig.squatAmountMultiplier;
            var kNod = mConfig.nodAmountByForce.Evaluate(force) * mConfig.nodAmountMultiplier;
            
            Landing(duration, kSquat, kNod);
        }
        
        private void Landing(float duration, float kSquat, float kNod) {
            var random = Random.insideUnitSphere * mConfig.nodRandomAddition;
            
            mJob.OverrideAndStart(
                getProcess: time => time / duration,
                block: process => {
                    var squat = mConfig.squatCurve.Evaluate(process) * mConfig.squatCurveMultiplier * kSquat;
                    var nod = mConfig.nodCurve.Evaluate(process) * mConfig.nodCurveMultiplier * -kNod;
                
                    mContainer.localPosition = Vector3.up * squat;
                    mContainer.localEulerAngles = (Vector3.right + random) * nod;
                }
            );
        }
        
    }
    
}