using System;
using MV_FPS_Controller.Scripts.Player.Movement;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.SampleInteraction {
    
    public class GoToTargetTrigger : MonoBehaviour {

        public Transform target;
        
        
        private void OnTriggerEnter(Collider other) {
            var playerSettings = other.GetComponent<IPlayerSettings>();
            if (playerSettings != null) LookAtAndGoToTarget(playerSettings);
        }

        private void LookAtAndGoToTarget(IPlayerSettings playerSettings) {
            var minDistance = 3f;
            var onTargetReached = new Action(() => {
                playerSettings.ReleaseLock();
                playerSettings.ReleaseTarget();
                Debug.Log("Target reached");
            });
            
            playerSettings.LockOn(target);
            playerSettings.GoToTarget(target.position, minDistance, onTargetReached);
        }
        
    }
    
}