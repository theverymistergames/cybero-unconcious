using System;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Movement {
    
    public interface IPlayerSettings {

        void Pause();
        
        void Resume();
        
        void EnableMotionInput();
        
        void DisableMotionInput();
        
        void EnableLookInput();
        
        void DisableLookInput();

        void EnableLookHorizontalClamp(float angle);

        void EnableLookVerticalClamp(float angle);
        
        void DisableLookHorizontalClamp();
        
        void DisableLookVerticalClamp();

        void SetLookSmoothAcceleration(float acceleration);
        
        void SetMotionSmoothAcceleration(float acceleration);

        void SetEnergy(float energy);
        
        void AddEnergy(float energy);
        
        void SetVolume(float volume);
        
        void GoToTarget(Vector3 target, float minDistance, Action onTargetReached);

        void ReleaseTarget();
        
        void LookAt(Vector3 target);
        
        void LookAtDirection(Vector3 direction);

        void LockOn(Vector3 target);
        
        void LockOn(Transform target);

        void ReleaseLock();

    }
    
}