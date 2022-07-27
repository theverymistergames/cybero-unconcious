using MV_FPS_Controller.Scripts.Audio.Body;
using MV_FPS_Controller.Scripts.Audio.Feet;
using MV_FPS_Controller.Scripts.Audio.Head;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Player;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Audio {

    [AddComponentMenu("MV FPS Controller/PlayerAudio")]
    public class PlayerAudio : PlayerActionsCallback {

        public PlayerAudioConfig config;

        private BreathSounds mBreathSounds;
        
        private RingingGearSounds mRingingGearSounds;
        private PositionSounds mPositionSounds;
        
        private StepsSounds mStepsSounds;
        private LandingSounds mLandingSounds;
        private JumpSounds mJumpSounds;
        private FallSounds mFallSounds;
        private SlideSounds mSlideSounds;

        private float mNormalizedHorizontalMagnitude = 0f;
        private float mMagnitude = 0f;
        private bool mPaused = false;


        private void OnValidate() {
            if (config == null) return;
            
            mBreathSounds?.SetConfig(config.breathSounds);
            mStepsSounds?.SetConfig(config.stepsSounds);
            
            mLandingSounds?.SetConfig(config.landingSounds);
            mJumpSounds?.SetConfig(config.jumpSounds);
            mFallSounds?.SetConfig(config.fallSounds);
            
            mSlideSounds?.SetConfig(config.slideSounds);
            mPositionSounds?.SetConfig(config.positionSounds);
            
            mRingingGearSounds?.SetConfig(config.ringingGearSounds);
            
            if (config.enabled) return;
            mSlideSounds?.OnStopSlide();
        }

        private void OnEnable() {
            if (mPaused) return;
            Resume();
        }

        private void OnDisable() {
            Pause();
        }
        
        private void Awake() {
            mBreathSounds = new BreathSounds(config.breathSounds);
            mBreathSounds.OnPlayOneShot += config.audioSources.headAudioSource.PlayOneShot;
            
            mPositionSounds = new PositionSounds(config.positionSounds);
            mPositionSounds.OnPlayOneShot += config.audioSources.bodyAudioSource.PlayOneShot;
            
            mRingingGearSounds = new RingingGearSounds(config.ringingGearSounds);
            mRingingGearSounds.OnPlayOneShot += config.audioSources.bodyAudioSource.PlayOneShot;
            
            mStepsSounds = new StepsSounds(config.stepsSounds);
            mStepsSounds.OnPlayOneShot += config.audioSources.feetAudioSource.PlayOneShot;
            
            mLandingSounds = new LandingSounds(config.landingSounds);
            mLandingSounds.OnPlayOneShot += config.audioSources.feetAudioSource.PlayOneShot;
            
            mJumpSounds = new JumpSounds(config.jumpSounds);
            mJumpSounds.OnPlayOneShot += config.audioSources.feetAudioSource.PlayOneShot;
            
            mFallSounds = new FallSounds(config.fallSounds);
            mFallSounds.OnPlayOneShot += config.audioSources.feetAudioSource.PlayOneShot;
            mFallSounds.OnUseJumpSounds += () => config.jumpSounds.materialSampleGroups;

            mSlideSounds = new SlideSounds(config.slideSounds);
            mSlideSounds.OnSetVolume += volume => config.audioSources.feetAudioSource.volume = volume;
            mSlideSounds.OnStop += () => {
                config.audioSources.feetAudioSource.clip = null;
            };
            mSlideSounds.OnPause += config.audioSources.feetAudioSource.Pause;
            mSlideSounds.OnPlay += clip => {
                config.audioSources.feetAudioSource.clip = clip;
                config.audioSources.feetAudioSource.Play();
            };
        }

        private void Update() {
            mSlideSounds.Update(mMagnitude);
        }

        public override void OnPause() {
            mPaused = true;
            Pause();
        }

        public override void OnResume() {
            mPaused = false;
            Resume();
        }

        private void Pause() {
            if (config.audioSources.headAudioSource != null) {
                config.audioSources.headAudioSource.Pause();
            }

            if (config.audioSources.bodyAudioSource != null) {
                config.audioSources.bodyAudioSource.Pause();
            }

            if (config.audioSources.feetAudioSource != null) {
                config.audioSources.feetAudioSource.Pause();
            }

            mSlideSounds?.Pause();
        }

        private void Resume() {
            mSlideSounds?.Resume();
        }
        
        public override void OnGroundMaterialChanged(string groundTag) {
            mStepsSounds.SetGroundTag(groundTag);
            mLandingSounds.SetGroundTag(groundTag);
            mJumpSounds.SetGroundTag(groundTag);
            mFallSounds.SetGroundTag(groundTag);
            mSlideSounds.SetGroundTag(groundTag);
        }

        public override void UpdateVelocity(Vector3 velocity) {
            mMagnitude = velocity.magnitude;
        }

        public override void UpdateNormalizedHorizontalMagnitude(float normalizedMagnitude) {
            mNormalizedHorizontalMagnitude = normalizedMagnitude;
        }
        
        public override void OnEnergyChanged(float energy) {
            mBreathSounds.SetEnergy(energy);
        }

        public override void OnInhale(float period, float amplitude) {
            if (!config.enabled || mPaused || !enabled) return;
            mBreathSounds.OnInhale(period, amplitude);
        }

        public override void OnExhale(float period, float amplitude) {
            if (!config.enabled || mPaused || !enabled) return;
            mBreathSounds.OnExhale(period, amplitude);
        }

        public override void OnStep(float length) {
            if (!config.enabled || mPaused || !enabled) return;
            
            mStepsSounds.OnStep(mNormalizedHorizontalMagnitude);
            mRingingGearSounds.OnStep(mNormalizedHorizontalMagnitude);
        }

        public override void OnStartSlide() {
            if (!config.enabled || mPaused || !enabled) return;
            mSlideSounds.OnStartSlide();
        }
        
        public override void OnStopSlide() {
            if (!config.enabled || mPaused || !enabled) return;
            mSlideSounds.OnStopSlide();
        }

        public override void OnJump(float force) {
            if (!config.enabled || mPaused || !enabled) return;
            
            mJumpSounds.OnJump(mNormalizedHorizontalMagnitude);
            mRingingGearSounds.OnJump(mNormalizedHorizontalMagnitude);
        }

        public override void OnFell() {
            if (!config.enabled || mPaused || !enabled) return;
            
            mFallSounds.OnFell(mNormalizedHorizontalMagnitude);
            mRingingGearSounds.OnFell(mNormalizedHorizontalMagnitude);
        }

        public override void OnLanded(float normalizedForce) {
            if (!config.enabled || mPaused || !enabled) return;
            
            mLandingSounds.OnLanded(normalizedForce);
            mRingingGearSounds.OnLanded(normalizedForce);
        }

        public override void OnStand() {
            if (!config.enabled || mPaused || !enabled) return;
            
            mPositionSounds.OnStand(mNormalizedHorizontalMagnitude);
            mRingingGearSounds.OnStand(mNormalizedHorizontalMagnitude);
        }

        public override void OnCrouch() {
            if (!config.enabled || mPaused || !enabled) return;
            
            mPositionSounds.OnCrouch(mNormalizedHorizontalMagnitude);
            mRingingGearSounds.OnCrouch(mNormalizedHorizontalMagnitude);
        }

        public override void OnSetVolume(float volume) {
            config.audioSources.headAudioSource.volume = volume;
            config.audioSources.bodyAudioSource.volume = volume;
            config.audioSources.feetAudioSource.volume = volume;
        }
        
    }
    
}