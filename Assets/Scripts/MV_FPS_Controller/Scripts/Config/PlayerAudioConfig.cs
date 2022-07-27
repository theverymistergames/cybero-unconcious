using System;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Config {
    
    [Serializable]
    public class PlayerAudioConfig {
        
        public bool enabled = true;
        
        public AudioSourcesConfig audioSources;
        
        public BreathSoundsConfig breathSounds;
        
        public RingingGearSoundsConfig ringingGearSounds;
        public PositionSoundsConfig positionSounds;

        public StepsSoundsConfig stepsSounds;
        public LandingSoundsConfig landingSounds;
        public JumpSoundsConfig jumpSounds;
        public FallSoundsConfig fallSounds;
        public SlideSoundsConfig slideSounds;
    }

    
    [Serializable]
    public class AudioSourcesConfig {
        
        [Tooltip("For breath sounds")]
        public AudioSource headAudioSource;
        
        [Tooltip("For ringing gear and position changing sounds")]
        public AudioSource bodyAudioSource;
        
        [Tooltip("For steps, landing, jump and fall sounds")]
        public AudioSource feetAudioSource;

    }
    

    [Serializable]
    public class BreathSoundsConfig {
        
        public bool enabled = true;
        
        [Range(0f, 1f)] public float volumeMultiplier = 0.5f;
        public AnimationCurve volumeByAmplitude = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        public List<RangeSampleGroup> inhaleSamples;
        public List<RangeSampleGroup> exhaleSamples;
        
    }

    
    [Serializable]
    public class RingingGearSoundsConfig {

        public bool enabled = true;
        
        [Range(0f, 1f)] public float volumeMultiplier = 0.5f;
        public AnimationCurve volumeByMagnitude = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        public AnimationCurve volumeByLandingForce = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        public List<RingingGearLayer> soundLayers;

        
        [Serializable]
        public class RingingGearLayer {

            public string name;
            
            [Header("Conditions")]
            public bool enabled = true;
            public bool enabledOnStep = true;
            public bool enabledOnLanding = true;
            public bool enabledOnJump = true;
            public bool enabledOnFell = true;
            public bool enabledOnStand = true;
            public bool enabledOnCrouch = true;
            
            [Header("Volume")]
            [Range(0f, 1f)] public float onStepVolumeMultiplier = 1f;
            [Range(0f, 1f)] public float onLandedVolumeMultiplier = 1f;
            [Range(0f, 1f)] public float onJumpVolumeMultiplier = 1f;
            [Range(0f, 1f)] public float onFellVolumeMultiplier = 1f;
            [Range(0f, 1f)] public float onStandVolumeMultiplier = 1f;
            [Range(0f, 1f)] public float onCrouchVolumeMultiplier = 1f;

            public List<AudioClip> samples;

        }    

    }
    
    
    [Serializable]
    public class StepsSoundsConfig {
        
        public bool enabled = true;
        
        [Range(0f, 1f)] public float volumeMultiplier = 0.2f;
        public AnimationCurve volumeByMagnitude = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        public List<MaterialSampleGroup> materialSampleGroups;
        
    }

    
    [Serializable]
    public class LandingSoundsConfig {
        
        public bool enabled = true;
        
        [Range(0f, 1f)] public float volumeMultiplier = 0.5f;
        public AnimationCurve volumeByForce = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        public List<MaterialSampleGroup> materialSampleGroups;
        
    }
    
    
    [Serializable]
    public class JumpSoundsConfig {
        
        public bool enabled = true;
        
        [Range(0f, 1f)] public float volumeMultiplier = 0.3f;
        public AnimationCurve volumeByMagnitude = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        public List<MaterialSampleGroup> materialSampleGroups;
        
    }

    
    [Serializable]
    public class FallSoundsConfig {
        
        public bool enabled = true;
        public bool useJumpSounds = true;
        
        [Range(0f, 1f)] public float volumeMultiplier = 0.3f;
        public AnimationCurve volumeByMagnitude = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        public List<MaterialSampleGroup> materialSampleGroups;
        
    }
    
    
    [Serializable]
    public class SlideSoundsConfig {
        
        public bool enabled = true;
        
        [Range(0f, 1f)] public float volumeMultiplier = 0.2f;
        public AnimationCurve volumeByMagnitude = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        public List<MaterialSampleGroup> materialSampleGroups;
        
    }
    
    [Serializable]
    public class PositionSoundsConfig {
        
        public bool enabled = true;
        
        [Header("Volume")]
        [Range(0f, 1f)] public float standVolumeMultiplier = 0.1f;
        [Range(0f, 1f)] public float crouchVolumeMultiplier = 0.1f;
        public AnimationCurve volumeByMagnitude = AnimationCurves.Arc(0f, 0.5f, 1.5f, 1f, 1f, 0f);
        
        [Header("Samples")]
        public List<AudioClip> standSamples;
        public List<AudioClip> crouchSamples;
        
    }

}