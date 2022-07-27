using System;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Config;
using UnityEngine;
using static MV_FPS_Controller.Scripts.Util.MathUtils;

namespace MV_FPS_Controller.Scripts.Audio.Body {
    
    public class PositionSounds {
    
        public event Action<AudioClip, float> OnPlayOneShot = delegate { };
        private PositionSoundsConfig mConfig;
        

        public PositionSounds(PositionSoundsConfig config) {
            SetConfig(config);
        }

        public void SetConfig(PositionSoundsConfig newConfig) {
            mConfig = newConfig;
        }
        
        public void OnStand(float magnitude) {
            if (!mConfig.enabled) return;
            if (mConfig.standSamples == null || mConfig.standSamples.Count == 0) return;
            
            var sample = NextSample(mConfig.standSamples);
            if (sample != null) OnPositionChanged(sample, magnitude, mConfig.standVolumeMultiplier);
        }
        
        public void OnCrouch(float magnitude) {
            if (!mConfig.enabled) return;
            if (mConfig.crouchSamples == null || mConfig.crouchSamples.Count == 0) return;
            
            var sample = NextSample(mConfig.crouchSamples);
            if (sample != null) OnPositionChanged(sample, magnitude, mConfig.crouchVolumeMultiplier);
        }

        private void OnPositionChanged(AudioClip sample, float magnitude, float volumeMultiplier) {
            var volume = mConfig.volumeByMagnitude.Evaluate(magnitude) * volumeMultiplier;
            OnPlayOneShot.Invoke(sample, volume);
        }
        
        private static AudioClip NextSample(IReadOnlyList<AudioClip> from) {
            var nextIndex = NextRandomIndex(from.Count);
            return from[nextIndex];
        }
        
    }
    
}