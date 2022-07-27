using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Player.Movement;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Collision {
    
    public class CeilingDetector {
        
        private readonly MoveController mCallback;
        private readonly CharacterController mController;
        private PlayerConfig mConfig;
        
        private readonly RaycastHit[] mHits = new RaycastHit[1];

        private Vector3 mPlayerPosition = Vector3.zero;
        private Vector3 mOffset = Vector3.zero;

        private bool mHasCeiling = false;

        
        public CeilingDetector(MoveController callback, CharacterController controller, PlayerConfig config) {
            mCallback = callback;
            mController = controller;
            mConfig = config;
            
            RevalidateTouchOffset();
        }

        public void SetConfig(PlayerConfig newConfig) {
            mConfig = newConfig;
            RevalidateTouchOffset();
        }

        public void SetPlayerPosition(Vector3 position) {
            mPlayerPosition = position;
        }

        private void RevalidateTouchOffset() {
            mOffset = Vector3.up * (mConfig.size.standHeight / 2f - mController.radius);
        }

        public void InitCeiling() {
            mHasCeiling = SphereCast();
        }
        
        public void UpdateCeiling() {
            var hadCeiling = mHasCeiling;
            mHasCeiling = SphereCast();

            if (mHasCeiling == hadCeiling) return;

            if (hadCeiling && !mHasCeiling) mCallback.OnCeilingGone();
            else mCallback.OnCeilingAppeared();
        }

        private bool SphereCast() {
            var count = Physics.SphereCastNonAlloc(
                mPlayerPosition + mOffset, 
                mController.radius,
                Vector3.up,
                mHits, 
                mConfig.touch.touchDistance,
                mConfig.touch.layerMask, 
                QueryTriggerInteraction.Ignore
            );
            
            return count > 0;
        }

    }
    
}