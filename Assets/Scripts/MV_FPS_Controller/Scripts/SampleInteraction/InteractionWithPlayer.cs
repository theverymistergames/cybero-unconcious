using System;
using MV_FPS_Controller.Scripts.Player.Movement;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.SampleInteraction {
    
    public class InteractionWithPlayer : MonoBehaviour {
        
        private void OnTriggerEnter(Collider other) {
            var playerSettings = other.GetComponent<IPlayerSettings>();
            if (playerSettings != null) DoInteraction(playerSettings);
        }

        private void DoInteraction(IPlayerSettings playerSettings) {
            playerSettings.GoToTarget(Vector3.zero, 3f, () => {
                Debug.Log("Target reached");
                playerSettings.ReleaseTarget();
            });
        }
        
    }
    
}