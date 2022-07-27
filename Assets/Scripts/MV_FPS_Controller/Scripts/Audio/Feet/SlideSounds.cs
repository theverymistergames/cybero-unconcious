using System;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Player.Collision;
using UnityEngine;
using static MV_FPS_Controller.Scripts.Util.MathUtils;

namespace MV_FPS_Controller.Scripts.Audio.Feet {
    
    public class SlideSounds {
    
        public event Action<AudioClip> OnPlay = delegate { };
        public event Action OnStop = delegate { };
        public event Action OnPause = delegate { };
        public event Action<float> OnSetVolume = delegate { };
        
        private SlideSoundsConfig mConfig;

        private List<AudioClip> mSamples = new List<AudioClip>();
        private AudioClip mSample;
        
        private string mLastGroundTag = "";
        private int mLastIndex = -1;
        private float mMagnitude = 0f;
        private bool mIsSliding = false;


        public SlideSounds(SlideSoundsConfig config) {
            SetConfig(config);
        }

        public void SetConfig(SlideSoundsConfig newConfig) {
            mConfig = newConfig;
        }

        public void Pause() {
            if (mIsSliding) OnPause.Invoke();
        }

        public void Resume() {
            if (mIsSliding) OnPlay.Invoke(mSample);
        }

        public void Update(float magnitude) {
            mMagnitude = magnitude;
            if (mIsSliding) OnSlide();
        }
        
        public void SetGroundTag(string groundTag) {
            if (!mConfig.enabled) return;
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

            if (!mIsSliding) return;
            mSample = NextSample();
            OnPlay.Invoke(mSample);
        }

        public void OnStartSlide() {
            mIsSliding = true;
            if (mSamples == null || mSamples.Count == 0) return;
            
            mSample = NextSample();
            OnPlay.Invoke(mSample);
        }

        public void OnStopSlide() {
            mIsSliding = false;
            OnStop.Invoke();
        }

        private void OnSlide() {
            if (!mConfig.enabled) return;
            
            var volume = mConfig.volumeByMagnitude.Evaluate(mMagnitude) * mConfig.volumeMultiplier;
            OnSetVolume.Invoke(volume);
        }

        private AudioClip NextSample() {
            var nextIndex = NextRandomIndex(mLastIndex, mSamples.Count);
            mLastIndex = nextIndex;
            return mSamples[nextIndex];
        }

    }
    
}