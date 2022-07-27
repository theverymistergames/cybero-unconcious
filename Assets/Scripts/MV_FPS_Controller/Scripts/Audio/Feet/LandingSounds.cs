using System;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Player.Collision;
using UnityEngine;
using static MV_FPS_Controller.Scripts.Util.MathUtils;

namespace MV_FPS_Controller.Scripts.Audio.Feet {
    
    public class LandingSounds {

        public event Action<AudioClip, float> OnPlayOneShot = delegate { };
        
        private LandingSoundsConfig mConfig;

        private string mLastGroundTag = "";
        private int mLastIndex = -1;
        private List<AudioClip> mSamples = new List<AudioClip>();
        

        public LandingSounds(LandingSoundsConfig config) {
            SetConfig(config);
        }

        public void SetConfig(LandingSoundsConfig newConfig) {
            mConfig = newConfig;
        }
        
        public void SetGroundTag(string groundTag) {
            if (!mConfig.enabled) return;
            if (groundTag == mLastGroundTag || groundTag == GroundDetector.AirTag) return;
            
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
        
        public void OnLanded(float force) {
            if (!mConfig.enabled) return;
            if (mSamples == null || mSamples.Count == 0) return;
            
            var volume = mConfig.volumeByForce.Evaluate(force) * mConfig.volumeMultiplier;
            OnPlayOneShot.Invoke(NextSample(), volume);
        }
        
        private AudioClip NextSample() {
            var nextIndex = NextRandomIndex(mLastIndex, mSamples.Count);
            mLastIndex = nextIndex;
            return mSamples[nextIndex];
        }

    }
    
}