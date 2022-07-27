using System;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Player.Movement;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Collision {
    
    public class GroundDetector {
        
        public const string AirTag = "Air";

        public event Action<string> OnGroundTagChanged = delegate {  };

        private readonly Transform mPlayer;
        private readonly MoveController mCallback;
        private readonly CharacterController mController;
        private PlayerConfig mConfig;

        private RaycastHit mHit;
        private int mHitCount = 0;
        private readonly RaycastHit[] mHits = new RaycastHit[6];
        private readonly RaycastHit[] mHitsSingle = new RaycastHit[1];

        private Vector3 mPlayerPosition = Vector3.zero;
        private Vector3 mOffset = Vector3.zero;
        private Vector3 mNormal = Vector3.up;
        
        private string mGroundTag = AirTag;
        private int mGroundHash = -1;

        private float mLandingForce = 0f;
        private bool mWasGrounded = false;
        private bool mIsGrounded = false;

        private bool mWasSliding = false;
        private bool mIsSliding = false;
        private bool mIsJumping = false;

        private bool mIsMovingVertical = false;
        private bool mIsMovingVerticalByRotation = false;
        private float mVerticalSpeed = 0f;

        private float mInitialStepOffset = 0f;


        public GroundDetector(Transform player, MoveController callback, CharacterController controller, PlayerConfig config) {
            mPlayer = player;
            mCallback = callback;
            mController = controller;
            
            SetConfig(config);
        }
        
        public void SetConfig(PlayerConfig newConfig) {
            mConfig = newConfig;
            mInitialStepOffset = mController.stepOffset;
            RevalidateTouchOffset();
        }
        
        public void SetVerticalSpeed(float speed) {
            mVerticalSpeed = speed;
        }
        
        public void SetPlayerPosition(Vector3 position) {
            mPlayerPosition = position;
        }
        
        private void RevalidateTouchOffset() {
            mOffset = Vector3.down * (mConfig.size.standHeight / 2f - mController.radius);
        }

        public void InitGround() {
            UpdateHits();
        }

        public void SetExternalMotion(Vector3 motion) {
            mIsMovingVertical = motion.y < 0f || motion.y > 0f;
        }
        
        public void SetExternalMotionFromRotation(Vector3 motion) {
            mIsMovingVerticalByRotation = motion.y < 0f || motion.y > 0f;
        }

        public void OnFinishExternalMotion() {
            if (IsOnMovingVertical() && !mIsGrounded) mCallback.OnFell();
            mIsMovingVertical = false;
            mIsMovingVerticalByRotation = false;
        }

        public bool IsGround(Transform transform) {
            return transform.GetHashCode() == mGroundHash;
        }

        public Vector3 GetNormal() {
            return mNormal;
        }
        
        public void UpdateGround() {
            mWasGrounded = mIsGrounded;
            mWasSliding = mIsSliding;
            UpdateHits();
            UpdateStickToGroundForce();
            ClarifyNormal();
            UpdateLandingForce();
            NotifyPosition();
        }

        private void UpdateHits() {
            mHitCount = HitCount();

            if (mHitCount > 0) {
                mHit = mHits[0];
                mIsGrounded = true;
                
                var prevHash = mGroundHash;
                mGroundHash = mHit.transform.GetHashCode();

                if (prevHash == mGroundHash) return;
                mGroundTag = mHit.transform.tag;
                OnGroundTagChanged.Invoke(mGroundTag);
                return;
            }

            mGroundHash = -1;
            mIsGrounded = false;
            
            if (IsOnMovingVertical() || mGroundTag.Equals(AirTag)) return;
            //if (mGroundTag.Equals(AirTag)) return;
            mGroundTag = AirTag;
            OnGroundTagChanged.Invoke(mGroundTag);
        }

        private void UpdateStickToGroundForce() {
            if (!mIsGrounded && !mWasGrounded || mIsJumping || mIsMovingVertical || mIsMovingVerticalByRotation) return;
            mController.Move(Vector3.down * (mConfig.move.stickToGroundForce * Time.deltaTime));
        }

        private void ClarifyNormal() {
            if (mIsGrounded) {
                mNormal = CalculateNormal();

                var angle = SlopeAngle();
                var slopeLimit = mController.slopeLimit;

                mIsGrounded = angle <= slopeLimit;
                mIsSliding = slopeLimit < angle && angle < 90f;
                return;
            }
            
            mIsSliding = false;
            mNormal = Vector3.up;
        }

        private void UpdateLandingForce() {
            if (mIsGrounded && mWasGrounded) return;
            if (mVerticalSpeed > mLandingForce) mLandingForce = mVerticalSpeed;
        }

        private void NotifyPosition() {
            if (mWasGrounded && !mIsGrounded) {
                mController.stepOffset = 0f;
                if (!IsOnMovingVertical()) mCallback.OnFell();
            }
            else if (!mWasGrounded && mIsGrounded) {
                mCallback.OnLanded(mLandingForce / mConfig.move.maxLandingForce);
                mIsJumping = false;
                mController.stepOffset = mInitialStepOffset;
                mLandingForce = 0f;
            }

            if (mWasSliding && !mIsSliding) {
                mCallback.OnStopSlide();
            }
            else if (!mWasSliding && mIsSliding) {
                mCallback.OnStartSlide();
            }
        }

        public void OnJump() {
            mIsJumping = true;
        }

        private float SlopeAngle() {
            return Vector3.Angle(mNormal, Vector3.up);
        }

        private bool IsOnMovingVertical() {
            return mIsMovingVertical || mIsMovingVerticalByRotation;
        }

        private Vector3 CalculateNormal() {
            var normal = Vector3.zero;
            for (var i = 0; i < mHitCount; i++) {
                if (ClarifyNormalAtPoint(mHits[i].point)) {
                    normal += mHitsSingle[0].normal;
                }
            }
            return normal.normalized;
        }

        private bool ClarifyNormalAtPoint(Vector3 point) {
            var origin = point + Vector3.up * 0.2f;
            return SphereCast(origin, 0.05f, 0.2f, mHitsSingle) > 0;
        }
        
        private int HitCount() {
            return SphereCast(mPlayerPosition + mOffset, mController.radius, mConfig.touch.touchDistance + mController.skinWidth, mHits);
        }
        
        private int SphereCast(Vector3 origin, float radius, float distance, RaycastHit[] raycastHits) {
            return Physics.SphereCastNonAlloc(
                origin, 
                radius,
                Vector3.down,
                raycastHits, 
                distance,
                mConfig.touch.layerMask, 
                QueryTriggerInteraction.Ignore
            );
        }

    }
    
}