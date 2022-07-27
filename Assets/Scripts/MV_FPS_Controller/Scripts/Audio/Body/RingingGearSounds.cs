using System;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Config;
using UnityEngine;
using static MV_FPS_Controller.Scripts.Util.MathUtils;

namespace MV_FPS_Controller.Scripts.Audio.Body {
    
    public class RingingGearSounds {
    
        public event Action<AudioClip, float> OnPlayOneShot = delegate { };
        
        private RingingGearSoundsConfig mConfig;
        private int[] mLastIndices;
        

        public RingingGearSounds(RingingGearSoundsConfig config) {
            SetConfig(config);
        }

        public void SetConfig(RingingGearSoundsConfig newConfig) {
            mConfig = newConfig;
            ResetLastIndices(mConfig.soundLayers.Count);
        }

        private void ResetLastIndices(int count) {
            if (count == 0) {
                mLastIndices = new int[0];
                return;
            }
            
            mLastIndices = new int[count];
            
            for (var i = 0; i < count; i++) {
                mLastIndices[i] = -1;
            }
        }
        
        public void OnStep(float magnitude) {
            if (!mConfig.enabled) return;
            
            var volume = mConfig.volumeByMagnitude.Evaluate(magnitude) * mConfig.volumeMultiplier;
            PlayLayers(volume, layer => layer.onStepVolumeMultiplier, layer => layer.enabledOnStep);
        }
        
        public void OnLanded(float force) {
            if (!mConfig.enabled) return;
            
            var volume = mConfig.volumeByLandingForce.Evaluate(force) * mConfig.volumeMultiplier;
            PlayLayers(volume, layer => layer.onLandedVolumeMultiplier, layer => layer.enabledOnLanding);
        }
        
        public void OnJump(float magnitude) {
            if (!mConfig.enabled) return;
            
            var volume = mConfig.volumeByMagnitude.Evaluate(magnitude) * mConfig.volumeMultiplier;
            PlayLayers(volume, layer => layer.onJumpVolumeMultiplier, layer => layer.enabledOnJump);
        }
        
        public void OnFell(float magnitude) {
            if (!mConfig.enabled) return;
            
            var volume = mConfig.volumeByMagnitude.Evaluate(magnitude) * mConfig.volumeMultiplier;
            PlayLayers(volume, layer => layer.onFellVolumeMultiplier, layer => layer.enabledOnFell);
        }
        
        public void OnStand(float magnitude) {
            if (!mConfig.enabled) return;
            
            var volume = mConfig.volumeByMagnitude.Evaluate(magnitude) * mConfig.volumeMultiplier;
            PlayLayers(volume, layer => layer.onStandVolumeMultiplier, layer => layer.enabledOnStand);
        }
        
        public void OnCrouch(float magnitude) {
            if (!mConfig.enabled) return;
         
            var volume = mConfig.volumeByMagnitude.Evaluate(magnitude) * mConfig.volumeMultiplier;
            PlayLayers(volume, layer => layer.onCrouchVolumeMultiplier, layer => layer.enabledOnCrouch);
        }

        private void PlayLayers(
            float volume, 
            Func<RingingGearSoundsConfig.RingingGearLayer, float> volumeMultiplier,
            Func<RingingGearSoundsConfig.RingingGearLayer, bool> enabled
        ) {
            if (mLastIndices.Length == 0) return;
            for (var i = 0; i < mLastIndices.Length; i++) {
                var layer = mConfig.soundLayers[i];
                if (!layer.enabled || !enabled.Invoke(layer)) continue;

                var sample = NextSample(layer.samples, mLastIndices[i]);
                if (sample == null) continue;

                var finalVolume = volume * volumeMultiplier.Invoke(layer);
                OnPlayOneShot.Invoke(sample, finalVolume);
            }
        }
        
        private static AudioClip NextSample(IReadOnlyList<AudioClip> from, int lastIndex) {
            if (from == null || from.Count == 0) return null;
            
            var nextIndex = NextRandomIndex(lastIndex, from.Count);
            return from[nextIndex];
        }
        
    }
    
}