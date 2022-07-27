using System;
using System.Collections;
using System.Collections.Generic;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Inputs;
using MV_FPS_Controller.Scripts.Player.Collision;
using MV_FPS_Controller.Scripts.Player.Helpers;
using MV_FPS_Controller.Scripts.Player.Movement;
using MV_FPS_Controller.Scripts.Support;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player {
    
    [AddComponentMenu("MV FPS Controller/Player")]
    [RequireComponent(typeof(CharacterController))]
    public class Player : InputReceiver, IMovablePlayer, IPlayerSettings, IExternalMovable
    {

        [SerializeField]
        private HudListener _hud;
        
        public new Camera camera;
        
        public PlayerActionsCallback playerAudio;
        public PlayerActionsCallback playerAnimator;
        public PlayerActionsCallback playerActionsCallback;
        
        public PlayerConfig config;

        private Transform mPlayer;
        private CharacterController mCharacterController;
        private MoveController mController;

        private LookHelper mLookHelper;
        private MoveHelper mMoveHelper;
        
        private CeilingDetector mCeilingDetector;
        private GroundDetector mGroundDetector;
        
        private BreathCycle mBreathCycle;
        private StepsCycle mStepsCycle;
        private EnergyMeter mEnergyMeter;

        private bool mHasAnimator = false;
        private bool mHasAudio = false;
        private bool mHasCallback = false;
        
        private bool mPaused = false;
        private bool mPausedMotionInput = false;
        private bool mPausedLookInput = false;
        private readonly List<ISubscription<IExternalMovable>> mExternalMotionSources = new List<ISubscription<IExternalMovable>>();
        
        //test
        public GameObject sphere;

        private void OnValidate() {
            InitConfig();
        }

        private void InitConfig() {
            if (config == null) return;
            
            mController?.SetConfig(config);
            
            mLookHelper?.SetConfig(config.look);
            mMoveHelper?.SetConfig(config.move);
            
            mCeilingDetector?.SetConfig(config);
            mGroundDetector?.SetConfig(config);

            mBreathCycle?.SetConfig(config.breathCycle);
            mStepsCycle?.SetConfig(config.stepsCycle);
            mEnergyMeter?.SetConfig(config.energy);
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
            InitController();

            InitMotion();
            InitCollision();
            
            InitHelpers();
            InitActionCallbacks();
        }

        private void InitCharacterController() {
            mCharacterController = GetComponent<CharacterController>();
            mCharacterController.minMoveDistance = 0f;
        }

        private void InitController() {
            mController = new MoveController(this, config);
        }

        private void InitMotion() {
            mPlayer = transform;
            mLookHelper = new LookHelper(mPlayer, camera.transform, config.look);
            mMoveHelper = new MoveHelper(mPlayer, mCharacterController, config.move);
        }
        
        private void InitCollision() {
            mCeilingDetector = new CeilingDetector(mController, mCharacterController, config);
            mGroundDetector = new GroundDetector(mPlayer, mController, mCharacterController, config);
        }

        private void InitHelpers() {
            mStepsCycle = new StepsCycle(config.stepsCycle);
            mBreathCycle = new BreathCycle(config.breathCycle);
            mEnergyMeter = new EnergyMeter(config.energy);
            
            mEnergyMeter.OnEnergyChanged += CheckCanRun;
            mEnergyMeter.OnEnergyChanged += mBreathCycle.OnEnergyChanged;
        }

        private void InitActionCallbacks() {
            mHasAnimator = playerAnimator != null;
            mHasAudio = playerAudio != null;
            mHasCallback = playerActionsCallback != null;

            if (mHasAnimator) {
                mStepsCycle.OnStep += playerAnimator.OnStep;
                mBreathCycle.OnInhale += playerAnimator.OnInhale;
                mBreathCycle.OnExhale += playerAnimator.OnExhale;
                mBreathCycle.OnStopBreath += playerAnimator.OnStopBreath;
                mEnergyMeter.OnEnergyChanged += playerAnimator.OnEnergyChanged;
                mGroundDetector.OnGroundTagChanged += playerAnimator.OnGroundMaterialChanged;
            }

            if (mHasAudio) {
                mStepsCycle.OnStep += playerAudio.OnStep;
                mBreathCycle.OnInhale += playerAudio.OnInhale;
                mBreathCycle.OnExhale += playerAudio.OnExhale;
                mBreathCycle.OnStopBreath += playerAudio.OnStopBreath;
                mEnergyMeter.OnEnergyChanged += playerAudio.OnEnergyChanged;
                mGroundDetector.OnGroundTagChanged += playerAudio.OnGroundMaterialChanged;
            }

            if (mHasCallback) {
                mStepsCycle.OnStep += playerActionsCallback.OnStep;
                mBreathCycle.OnInhale += playerActionsCallback.OnInhale;
                mBreathCycle.OnExhale += playerActionsCallback.OnExhale;
                mBreathCycle.OnStopBreath += playerActionsCallback.OnStopBreath;
                mEnergyMeter.OnEnergyChanged += playerActionsCallback.OnEnergyChanged;
                mGroundDetector.OnGroundTagChanged += playerActionsCallback.OnGroundMaterialChanged;
            }
        }

        private void OnDestroy() {
            foreach (var source in mExternalMotionSources) {
                source.Unsubscribe(this);   
            }
        }

        private void Start() {
            mGroundDetector.InitGround();
            mCeilingDetector.InitCeiling();
            mEnergyMeter.InitEnergy();
            
            if (mHasAnimator) {
                playerAnimator.OnSetCrouchHeight(config.size.crouchHeight);
                playerAnimator.OnSetStandHeight(config.size.standHeight);
            }

            if (mHasAudio) {
                playerAudio.OnSetCrouchHeight(config.size.crouchHeight);
                playerAudio.OnSetStandHeight(config.size.standHeight);
            }

            if (mHasCallback) {
                playerActionsCallback.OnSetCrouchHeight(config.size.crouchHeight);
                playerActionsCallback.OnSetStandHeight(config.size.standHeight);
            }
        }

        private void Update() {
            if (mPaused || !enabled) return;

            mCeilingDetector.SetPlayerPosition(mPlayer.position);
            mCeilingDetector.UpdateCeiling();
            
            mLookHelper.UpdateLook();
            mMoveHelper.UpdateMove();
            
            var velocity = mCharacterController.velocity;

            velocity.y = 0f;
            var horizontalMagnitude = velocity.magnitude;
            var normalizedHorizontalMagnitude = horizontalMagnitude / config.move.runSpeed;

            mEnergyMeter.UpdateEnergy(normalizedHorizontalMagnitude);
            mStepsCycle.UpdateCycle(horizontalMagnitude, normalizedHorizontalMagnitude);
            mBreathCycle.UpdateCycle(normalizedHorizontalMagnitude);

            if (mHasAnimator) {
                playerAnimator.UpdateVelocity(velocity);
                playerAnimator.UpdateNormalizedHorizontalMagnitude(normalizedHorizontalMagnitude);
            }

            if (mHasAudio) {
                playerAudio.UpdateVelocity(velocity);
                playerAudio.UpdateNormalizedHorizontalMagnitude(normalizedHorizontalMagnitude);
            }

            if (mHasCallback) {
                playerActionsCallback.UpdateVelocity(velocity);
                playerActionsCallback.UpdateNormalizedHorizontalMagnitude(normalizedHorizontalMagnitude);   
            }
            
            //test
            if (Input.GetKeyDown("1"))
            {
                var wave = Instantiate(sphere);
                sphere.transform.localPosition = transform.position;
                sphere.transform.localScale *= 2;
                StartCoroutine(Scan(wave));
            }
        }

        private IEnumerator Scan(GameObject wave)
        {
            var timer = 0f;
            while (timer < 3)
            {
                wave.transform.localScale += Vector3.one * 0.4f;
                yield return new WaitForSeconds(Time.deltaTime);
                timer += Time.deltaTime;
            }
        }

        private void LateUpdate() {
            if (mPaused || !enabled) return;
            
            var velocity = mCharacterController.velocity;
            var verticalSpeed = Mathf.Abs(velocity.y);
            var position = mPlayer.position;
            
            mGroundDetector.SetPlayerPosition(position);
            mGroundDetector.SetVerticalSpeed(verticalSpeed);
            mGroundDetector.UpdateGround();

            mMoveHelper.SetGroundNormal(mGroundDetector.GetNormal());
        }

        private bool _isHudActive = false;
        public override void OnHud()
        {
            _isHudActive = !_isHudActive;

            config.look.horizontalClampEnabled = _isHudActive;
            config.look.horizontalClamp = 45f;
            InitConfig();
            
            mLookHelper.SetIsHudActive(_isHudActive);
            _hud.SetIsHudActive(_isHudActive);
        }

        public override void OnLook(Vector2 delta) {
            if (mPausedLookInput || !enabled) return;
            mController.OnLook(delta);
        }

        public override void OnMove(Vector2 dir) {
            if (mPausedMotionInput || !enabled) return;
            mController.OnMove(dir);
        }

        public override void OnLean(int dir) {
            if (mPausedMotionInput || !enabled) return;
            mController.OnLean(dir);
        }

        public override void OnJump() {
            if (mPausedMotionInput || !enabled) return;
            mController.OnPerformJump();
        }

        public override void OnCrouch(bool isActive) {
            if (mPausedMotionInput || !enabled) return;
            
            if (isActive) mController.OnStartCrouch();
            else mController.OnCancelCrouch();
        }

        public override void OnToggleCrouch() {
            if (mPausedMotionInput || !enabled) return;
            mController.ToggleCrouch();
        }

        public override void OnRun(bool isActive) {
            if (mPausedMotionInput || !enabled) return;
            
            if (isActive) mController.OnStartRun();
            else mController.OnCancelRun();
        }

        public override void OnToggleRun() {
            if (mPausedMotionInput || !enabled) return;
            mController.ToggleRun();
        }

        void IExternalMovable.OnStartExternalMotion(ISubscription<IExternalMovable> source) {
            mExternalMotionSources.Add(source);
            source.Subscribe(this);
        }

        void IExternalMovable.OnFinishExternalMotion(ISubscription<IExternalMovable> source) {
            source.Unsubscribe(this);
            mExternalMotionSources.Remove(source);
            
            if (mPaused || !enabled) return;
            
            mGroundDetector.OnFinishExternalMotion();
        }

        void IExternalMovable.ExternalMove(Transform source, Vector3 motion) {
            if (mPaused || !enabled) return;
            
            mGroundDetector.SetExternalMotion(motion);
            mCharacterController.Move(motion);
        }

        void IExternalMovable.ExternalRotate(Transform source, Vector3 angles) {
            if (mPaused || !enabled) return;
            mPlayer.Rotate(Vector3.up, angles.y);
            
            var motion = MotionFromRotation(source.position, angles);
            mGroundDetector.SetExternalMotionFromRotation(motion);
            mCharacterController.Move(motion);
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit) {
            mMoveHelper.OnCollision(hit);

            var isGround = mGroundDetector.IsGround(hit.transform);
            
            if (mHasAnimator) playerAnimator.OnColliderHit(isGround, hit);
            if (mHasAudio) playerAudio.OnColliderHit(isGround, hit);
            if (mHasCallback) playerActionsCallback.OnColliderHit(isGround, hit);
        }
        
        void IMovablePlayer.OnLook(Vector2 delta) {
            if (mPaused || !enabled) return;
            
            mLookHelper.OnLook(delta);
            
            if (mHasAnimator) playerAnimator.OnLook(delta);
            if (mHasAudio) playerAudio.OnLook(delta);
            if (mHasCallback) playerActionsCallback.OnLook(delta);
        }

        void IMovablePlayer.OnMove(Vector2 dir) {
            if (mPaused || !enabled) return;

            mMoveHelper.OnMove(dir);

            var speed = dir.magnitude;
            var normalizedSpeed = speed / config.move.runSpeed;
            
            mEnergyMeter.SetIsRunningForward(speed >= config.move.runSpeed);
            mStepsCycle.OnTargetSpeedChanged(normalizedSpeed);

            if (mHasAnimator) {
                playerAnimator.OnMove(dir);
                playerAnimator.OnTargetSpeedChanged(normalizedSpeed);
            }

            if (mHasAudio) {
                playerAudio.OnMove(dir);
                playerAudio.OnTargetSpeedChanged(normalizedSpeed);
            }

            if (mHasCallback) {
                playerActionsCallback.OnMove(dir);
                playerActionsCallback.OnTargetSpeedChanged(normalizedSpeed);
            }
        }

        void IMovablePlayer.OnLean(int dir) {
            if (mPaused) return;
            
            if (mHasAnimator) playerAnimator.OnLean(dir);
            if (mHasAudio) playerAudio.OnLean(dir);
            if (mHasCallback) playerActionsCallback.OnLean(dir);
        }

        void IMovablePlayer.OnJump(float force) {
            if (mPaused) return;
            
            mStepsCycle.SetIsGrounded(false);
            mBreathCycle.SetIsGrounded(false);
            
            mGroundDetector.OnJump();
            mMoveHelper.OnJump(force);
            
            if (mHasAnimator) playerAnimator.OnJump(force);
            if (mHasAudio) playerAudio.OnJump(force);
            if (mHasCallback) playerActionsCallback.OnJump(force);
        }

        void IMovablePlayer.OnFell() {
            if (mPaused) return;
            
            mStepsCycle.SetIsGrounded(false);
            mBreathCycle.SetIsGrounded(false);
            
            mMoveHelper.OnFell();
            
            if (mHasAnimator) playerAnimator.OnFell();
            if (mHasAudio) playerAudio.OnFell();
            if (mHasCallback) playerActionsCallback.OnFell();
        }

        void IMovablePlayer.OnLanded(float normalizedForce) {
            if (mPaused) return;
            
            mStepsCycle.SetIsGrounded(true);
            mBreathCycle.SetIsGrounded(true);

            mMoveHelper.OnLanded();
            
            if (mHasAnimator) playerAnimator.OnLanded(normalizedForce);
            if (mHasAudio) playerAudio.OnLanded(normalizedForce);
            if (mHasCallback) playerActionsCallback.OnLanded(normalizedForce);
        }

        void IMovablePlayer.OnCrouch() {
            if (mPaused) return;
            
            if (mHasAnimator) playerAnimator.OnCrouch();
            if (mHasAudio) playerAudio.OnCrouch();
            if (mHasCallback) playerActionsCallback.OnCrouch();
        }

        void IMovablePlayer.OnStand() {
            if (mPaused) return;
            
            if (mHasAnimator) playerAnimator.OnStand();
            if (mHasAudio) playerAudio.OnStand();
            if (mHasCallback) playerActionsCallback.OnStand();
        }

        void IMovablePlayer.OnStartSlide() {
            if (mPaused) return;
            
            mMoveHelper.OnStartSlide();
            
            if (mHasAnimator) playerAnimator.OnStartSlide();
            if (mHasAudio) playerAudio.OnStartSlide();
            if (mHasCallback) playerActionsCallback.OnStartSlide();
        }

        void IMovablePlayer.OnStopSlide() {
            if (mPaused) return;
            
            mMoveHelper.OnStopSlide();
            
            if (mHasAnimator) playerAnimator.OnStopSlide();
            if (mHasAudio) playerAudio.OnStopSlide();
            if (mHasCallback) playerActionsCallback.OnStopSlide();
        }

        void IPlayerSettings.Pause() {
            mPaused = true;
            Pause();
        }

        void IPlayerSettings.Resume() {
            mPaused = false;
            Resume();
        }

        private void Pause() {
            mLookHelper.Pause();
            
            if (mHasAnimator) playerAnimator.OnPause();
            if (mHasAudio) playerAudio.OnPause();
            if (mHasCallback) playerActionsCallback.OnPause();
        }

        private void Resume() {
            mLookHelper.Resume();
            
            if (mHasAnimator) playerAnimator.OnResume();
            if (mHasAudio) playerAudio.OnResume();
            if (mHasCallback) playerActionsCallback.OnResume();
        }

        void IPlayerSettings.EnableMotionInput() {
            mPausedMotionInput = false;
        }

        void IPlayerSettings.DisableMotionInput() {
            mPausedMotionInput = true;
            mMoveHelper.ResetInput();
        }

        void IPlayerSettings.EnableLookInput() {
            mPausedLookInput = false;
        }

        void IPlayerSettings.DisableLookInput() {
            mPausedLookInput = true;
        }

        void IPlayerSettings.EnableLookHorizontalClamp(float angle) {
            config.look.horizontalClampEnabled = true;
            config.look.horizontalClamp = angle;
            mLookHelper.SetConfig(config.look);
        }

        void IPlayerSettings.EnableLookVerticalClamp(float angle) {
            config.look.verticalClampEnabled = true;
            config.look.verticalClamp = angle;
            mLookHelper.SetConfig(config.look);
        }

        void IPlayerSettings.DisableLookHorizontalClamp() {
            config.look.horizontalClampEnabled = false;
            mLookHelper.SetConfig(config.look);
        }

        void IPlayerSettings.DisableLookVerticalClamp() {
            config.look.verticalClampEnabled = false;
            mLookHelper.SetConfig(config.look);
        }

        void IPlayerSettings.SetLookSmoothAcceleration(float acceleration) {
            config.look.smoothAcceleration = acceleration;
            mLookHelper.SetConfig(config.look);
        }

        void IPlayerSettings.SetMotionSmoothAcceleration(float acceleration) {
            config.move.smoothAcceleration = acceleration;
            mMoveHelper.SetConfig(config.move);
        }

        void IPlayerSettings.SetEnergy(float energy) {
            mEnergyMeter.SetEnergy(energy);
        }

        void IPlayerSettings.AddEnergy(float energy) {
            mEnergyMeter.AddEnergy(energy);
        }

        void IPlayerSettings.SetVolume(float volume) {
            playerAudio.OnSetVolume(volume);
        }

        void IPlayerSettings.GoToTarget(Vector3 target, float minDistance, Action onTargetReached) {
            mPausedMotionInput = true;
            mController.OnCancelCrouch();
            mMoveHelper.SetTarget(target, config.move.walkSpeed, minDistance, () => {
                mPausedMotionInput = false;
                mController.OnMove(Vector2.zero);
                onTargetReached.Invoke();
            });
        }

        void IPlayerSettings.ReleaseTarget() {
            mPausedMotionInput = false;
            mMoveHelper.ReleaseTarget();
            mController.OnMove(Vector2.zero);
        }

        void IPlayerSettings.LookAt(Vector3 target) {
            mLookHelper.LookAt(target);
        }

        void IPlayerSettings.LookAtDirection(Vector3 direction) {
            mLookHelper.LookAtDirection(direction);
        }

        void IPlayerSettings.LockOn(Vector3 target) {
            mLookHelper.LockOn(target);
        }

        void IPlayerSettings.LockOn(Transform target) {
            mLookHelper.LockOn(target);
        }

        void IPlayerSettings.ReleaseLock() {
            mLookHelper.ReleaseLock();
        }

        private void CheckCanRun(float energy) {
            if (energy <= 0f) mController.SetCanRun(false);
            else if (energy >= config.energy.recoveryGap) mController.SetCanRun(true);
        }

        private Vector3 MotionFromRotation(Vector3 pivot, Vector3 angles) {
            var position = mPlayer.position;
            return position.RotateAround(pivot, angles) - position;
        }

    }
    
}
