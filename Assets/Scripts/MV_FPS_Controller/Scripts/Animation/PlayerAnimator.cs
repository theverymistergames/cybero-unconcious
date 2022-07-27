using MV_FPS_Controller.Scripts.Animation.Look;
using MV_FPS_Controller.Scripts.Animation.Move;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Player;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Animation {

    [AddComponentMenu("MV FPS Controller/PlayerAnimator")]
    public class PlayerAnimator : PlayerActionsCallback {

        public new Camera camera;
        public PlayerAnimationConfig config;
        
        private CharacterController mCharacterController;

        private FovAnimator mFovAnimator;
        private BreathAnimator mBreathAnimator;
        private HeadBobAnimator mHeadBobAnimator;

        private JumpAnimator mJumpAnimator;
        private LandingAnimator mLandingAnimator;
        private LeanTiltAnimator mLeanTiltAnimator;
        private PositionsAnimator mPositionsAnimator;

        private float mNormalizedHorizontalMagnitude = 0f;
        private float mHorizontalMagnitude = 0f;

        private bool mPaused = false;
        
        
        private void OnValidate() {
            if (config == null) return;
            
            mFovAnimator?.SetConfig(config.fov);
            mBreathAnimator?.SetConfig(config.breath);
            mHeadBobAnimator?.SetConfig(config.headBob);

            mJumpAnimator?.SetConfig(config.jump);
            mLandingAnimator?.SetConfig(config.landing);
            mLeanTiltAnimator?.SetConfig(config.leanTilt);
            mPositionsAnimator?.SetConfig(config.positions);

            if (config.enabled) return;
            
            mBreathAnimator?.ToInitialPosition();
            mFovAnimator?.ToInitialPosition();
            mHeadBobAnimator?.ToInitialPosition();
            mLeanTiltAnimator?.ToInitialPosition();
            mPositionsAnimator?.OnStand();
        }

        private void OnEnable() {
            if (mPaused) return;
            Resume();
        }

        private void OnDisable() {
            Pause();
        }

        private void Awake() {
            InitCharacterController();
            InitAnimators();
        }
        
        private void InitCharacterController() {
            mCharacterController = GetComponent<CharacterController>();
        }
        
        private void InitAnimators() {
            var playerTransform = transform;
            var cameraTransform = camera.transform;

            var landingContainer = InitContainer("LandingContainer", playerTransform);
            var positionsContainer = InitContainer("PositionContainer", landingContainer);
            var leanContainer = InitContainer("LeanContainer", positionsContainer);
            var jumpContainer = InitContainer("JumpContainer", leanContainer);
            var breathContainer = InitContainer("BreathContainer", jumpContainer);
            var headBobContainer = InitContainer("HeadBobContainer", breathContainer);
            
            var cameraPosition = cameraTransform.localPosition;
            cameraTransform.parent = headBobContainer;
            cameraTransform.localPosition = cameraPosition;
            
            mFovAnimator = new FovAnimator(camera, config.fov);
            mBreathAnimator = new BreathAnimator(this, breathContainer, config.breath);
            mHeadBobAnimator = new HeadBobAnimator(headBobContainer, config.headBob);
            
            mJumpAnimator = new JumpAnimator(this, jumpContainer, config.jump);
            mLandingAnimator = new LandingAnimator(this, landingContainer, config.landing);
            mLeanTiltAnimator = new LeanTiltAnimator(this, leanContainer, config.leanTilt);
            mPositionsAnimator = new PositionsAnimator(this, positionsContainer, mCharacterController, config.positions);
        }

        private void Update() {
            if (!config.enabled || mPaused || !enabled) return;
            
            mFovAnimator.UpdateFov(mNormalizedHorizontalMagnitude);
            mHeadBobAnimator.UpdateCycle(mHorizontalMagnitude, mNormalizedHorizontalMagnitude);
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
            mBreathAnimator?.Pause();
            mJumpAnimator?.Pause();
            mLandingAnimator?.Pause();
            mLeanTiltAnimator?.Pause();
            mPositionsAnimator?.Pause();
        }

        private void Resume() {
            mBreathAnimator?.Resume();
            mJumpAnimator?.Resume();
            mLandingAnimator?.Resume();
            mLeanTiltAnimator?.Resume();
            mPositionsAnimator?.Resume();
        }

        public override void UpdateVelocity(Vector3 velocity) {
            velocity.y = 0f;
            mHorizontalMagnitude = velocity.magnitude;
        }

        public override void UpdateNormalizedHorizontalMagnitude(float normalizedMagnitude) {
            mNormalizedHorizontalMagnitude = normalizedMagnitude;
        }

        public override void OnMove(Vector2 dir) {
            mFovAnimator.SetIsMovingForward(dir.y > 0f);
            
            if (!config.enabled || !enabled) return;
            mLeanTiltAnimator.OnTilt(-dir.normalized.x);
        }

        public override void OnTargetSpeedChanged(float normalizedSpeed) {
            mHeadBobAnimator.SetIsMoving(normalizedSpeed > 0f);
            mLeanTiltAnimator.OnTargetSpeedChanged(normalizedSpeed);
        }

        public override void OnLean(int dir) {
            if (!config.enabled || !enabled) return;
            mLeanTiltAnimator.OnLean(dir);
        }

        public override void OnJump(float force) {
            mPositionsAnimator.SetIsGrounded(false);
            mBreathAnimator.SetIsGrounded(false);
            mHeadBobAnimator.SetIsGrounded(false);
            mLeanTiltAnimator.SetIsGrounded(false);
            
            if (!config.enabled || !enabled) return;
            mJumpAnimator.OnJump(force);
        }

        public override void OnFell() {
            mPositionsAnimator.SetIsGrounded(false);
            mBreathAnimator.SetIsGrounded(false);
            mHeadBobAnimator.SetIsGrounded(false);
            mLeanTiltAnimator.SetIsGrounded(false);
        }

        public override void OnLanded(float normalizedForce) {
            mPositionsAnimator.SetIsGrounded(true);
            mBreathAnimator.SetIsGrounded(true);
            mHeadBobAnimator.SetIsGrounded(true);
            mLeanTiltAnimator.SetIsGrounded(true);
            
            if (!config.enabled || !enabled) return;
            mLandingAnimator.OnLanded(normalizedForce);
        }

        public override void OnStartSlide() {
            mPositionsAnimator.SetIsSliding(true);
            mLeanTiltAnimator.SetIsSliding(true);
        }

        public override void OnStopSlide() {
            mPositionsAnimator.SetIsSliding(false);
            mLeanTiltAnimator.SetIsSliding(false);
        }

        public override void OnCrouch() {
            if (!config.enabled || !enabled) return;
            mPositionsAnimator.OnCrouch();
        }

        public override void OnStand() {
            if (!config.enabled || !enabled) return;
            mPositionsAnimator.OnStand();
        }

        public override void OnSetCrouchHeight(float height) {
            mPositionsAnimator.SetCrouchHeight(height);
        }

        public override void OnSetStandHeight(float height) {
            mPositionsAnimator.SetStandHeight(height);
        }

        public override void OnStep(float length) {
            mHeadBobAnimator.SetStepLength(length);
        }

        public override void OnInhale(float period, float amplitude) {
            if (!config.enabled || !enabled) return;
            mBreathAnimator.OnInhale(period, amplitude);
        }

        public override void OnExhale(float period, float amplitude) {
            if (!config.enabled || !enabled) return;
            mBreathAnimator.OnExhale(period, amplitude);
        }

        public override void OnStopBreath() {
            if (!config.enabled || !enabled) return;
            mBreathAnimator.ToInitialPosition();
        }

        public override void OnEnergyChanged(float energy) {
            mBreathAnimator.SetEnergy(energy);
        }
        
        private static Transform InitContainer(string name, Transform parent) {
            var obj = new GameObject(name);
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            return obj.transform;
        }
        
    }
    
}