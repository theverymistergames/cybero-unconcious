using System;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;
using static MV_FPS_Controller.Scripts.Util.MathUtils;

namespace MV_FPS_Controller.Scripts.Audio.Head {
    
    public class BreathSounds {
        
        public event Action<AudioClip, float> OnPlayOneShot = delegate { };
        
        private BreathSoundsConfig mConfig;
        private float mEnergy = 1f;

        
        public BreathSounds(BreathSoundsConfig config) {
            SetConfig(config);
        }

        public void SetConfig(BreathSoundsConfig newConfig) {
            mConfig = newConfig;
        }

        public void SetEnergy(float energy) {
            mEnergy = energy;
        }
        
        public void OnInhale(float period, float amplitude) {
            if (!mConfig.enabled) return;
            
            var sample = NextSample(mConfig.inhaleSamples);
            if (sample != null) OnBreath(amplitude, sample);
        }
        
        public void OnExhale(float period, float amplitude) {
            if (!mConfig.enabled) return;
            
            var sample = NextSample(mConfig.exhaleSamples);
            if (sample != null) OnBreath(amplitude, sample);
        }

        private void OnBreath(float amplitude, AudioClip sample) {
            var volume = mConfig.volumeByAmplitude.Evaluate(amplitude) * mConfig.volumeMultiplier;
            OnPlayOneShot.Invoke(sample, volume);
        }
        
        private AudioClip NextSample(List<RangeSampleGroup> from) {
            var group = from.Find(it => it.ContainsValue(mEnergy));
            if (group == null || group.items.Count == 0) return null;
            
            var nextIndex = NextRandomIndex(group.items.Count);
            return group.items[nextIndex];
        }
        
    }
    
}