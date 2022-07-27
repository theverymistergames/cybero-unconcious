using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Animation.Move {
    
    public class JumpAnimator {
        
        private readonly Transform mContainer;
        private readonly SingleCoroutine mJob;
        
        private JumpAnimationConfig mConfig;


        public JumpAnimator(MonoBehaviour player, Transform container, JumpAnimationConfig config) {
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

        public void SetConfig(JumpAnimationConfig newConfig) {
            mConfig = newConfig;
        }

        public void OnJump(float force) {
            if (!mConfig.enabled) return;
            Jump(force);
        }

        private void Jump(float force) {
            var random = Random.insideUnitSphere * mConfig.random;
            mJob.OverrideAndStart(
                getProcess: time => time / mConfig.duration,
                block: process => {
                    var value = mConfig.jumpCurve.Evaluate(process) * force;
                    mContainer.localEulerAngles = value * (Vector3.left + random);
                }
            );
        }
        
    }
    
}