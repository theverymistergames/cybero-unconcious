using System;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Config;
using UnityEngine;
using static MV_FPS_Controller.Scripts.Util.MathUtils;

namespace MV_FPS_Controller.Scripts.Audio.Feet {
    
    public class StepsSounds {
    
        public event Action<AudioClip, float> OnPlayOneShot = delegate { };
        
        private StepsSoundsConfig mConfig;
        
        private string mLastGroundTag = "";
        private int mLastIndex = -1;
        private List<AudioClip> mSamples = new List<AudioClip>();
        

        public StepsSounds(StepsSoundsConfig config) {
            SetConfig(config);
        }

        public void SetConfig(StepsSoundsConfig newConfig) {
            mConfig = newConfig;
        }
        
        public void SetGroundTag(string groundTag) {
            if (!mConfig.enabled) return;
            
            if (groundTag == mLastGroundTag) return;
            
            mLastGroundTag = groundTag;
            mLastIndex = -1;
            
            var group = mConfig.materialSampleGroups
                .Find(it => it.name == groundTag);
            
            if (group == null) {
                mSamples = null;
                return;
            }

            mSamples = group.items;
        }
        
        public void OnStep(float magnitude) {
            if (!mConfig.enabled) return;
            
            if (mSamples == null || mSamples.Count == 0) return;
            
            var volume = mConfig.volumeByMagnitude.Evaluate(magnitude) * mConfig.volumeMultiplier;
            OnPlayOneShot.Invoke(NextSample(), volume);
        }
        
        private AudioClip NextSample() {
            var nextIndex = NextRandomIndex(mLastIndex, mSamples.Count);
            mLastIndex = nextIndex;
            return mSamples[nextIndex];
        }

    }
    
}