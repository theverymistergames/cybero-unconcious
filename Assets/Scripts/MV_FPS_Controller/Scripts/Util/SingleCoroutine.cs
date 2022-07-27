using System;
using System.Collections;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Util {
    
    public class SingleCoroutine {
        
        private readonly MonoBehaviour mParent;
        private IEnumerator mRoutine;

        private bool mPaused = false;
        

        public SingleCoroutine(MonoBehaviour parent) {
            mParent = parent;
        }

        public void OverrideAndStart(Func<float, float> getProcess, Action<float> block) {
            OverrideAndStart(UpdateRoutine(getProcess, block));
        }
        
        public void OverrideAndStart(Func<float, float> getProcess, Action<float> block, Action onFinish) {
            OverrideAndStart(UpdateRoutine(getProcess, block, onFinish));
        }
        
        private void OverrideAndStart(IEnumerator routine) {
            if (mRoutine != null) Stop();
            mRoutine = routine;
            
            mParent.StartCoroutine(mRoutine);
        }

        public void Pause() {
            mPaused = true;
        }

        public void Resume() {
            mPaused = false;
        }
        
        public void Stop() {
            mParent.StopCoroutine(mRoutine);
        }

        private IEnumerator UpdateRoutine(Func<float, float> getProcess, Action<float> block) {
            return UpdateRoutine(getProcess, block, () => { });
        }
        
        private IEnumerator UpdateRoutine(Func<float, float> getProcess, Action<float> block, Action onFinish) {
            var time = 0f;
            var process = 0f;
            
            while (process <= 1) {
                if (mPaused) {
                    yield return null;
                    continue;
                }
                
                time += Time.deltaTime;
                process = getProcess.Invoke(time);
                
                block.Invoke(process);
                yield return null;
            }
            
            onFinish.Invoke();
        }
        
    }
    
}