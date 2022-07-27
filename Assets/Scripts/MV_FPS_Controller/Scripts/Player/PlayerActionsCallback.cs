using MV_FPS_Controller.Scripts.Player.Movement;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player {
    
    /// <summary>
    ///     Extend this class to know about all <see cref="Player"/> actions.
    /// </summary>
    public abstract class PlayerActionsCallback : MonoBehaviour, IMovablePlayer {
        
        public virtual void OnPause() { }
        
        public virtual void OnResume() { }
        
        public virtual void OnLook(Vector2 delta) { }

        public virtual void OnMove(Vector2 dir) { }
        
        public virtual void OnTargetSpeedChanged(float normalizedSpeed) { }

        public virtual void OnLean(int dir) { }

        public virtual void OnJump(float force) { }

        public virtual void OnFell() { }

        public virtual void OnLanded(float normalizedForce) { }

        public virtual void OnStartSlide() { }

        public virtual void OnStopSlide() { }

        public virtual void OnCrouch() { }

        public virtual void OnStand() { }
        
        public virtual void OnInhale(float period, float amplitude) { }
        
        public virtual void OnExhale(float period, float amplitude) { }
        
        public virtual void OnStopBreath() { }
        
        public virtual void OnStep(float length) { }

        public virtual void OnColliderHit(bool isGround, ControllerColliderHit hit) { }
        
        public virtual void OnEnergyChanged(float energy) { }
        
        public virtual void OnSetCrouchHeight(float height) { }
        
        public virtual void OnSetStandHeight(float height) { }
        
        public virtual void OnSetVolume(float volume) { }

        public virtual void OnGroundMaterialChanged(string groundTag) { }
        
        public virtual void UpdateVelocity(Vector3 velocity) { }
        
        public virtual void UpdateNormalizedHorizontalMagnitude(float normalizedMagnitude) { }

    }
    
}