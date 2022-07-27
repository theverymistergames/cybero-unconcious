using MV_FPS_Controller.Scripts.Player.Movement;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.SampleInteraction {
    
    public class EnergyBuffTrigger : MonoBehaviour {
        
        private void OnTriggerEnter(Collider other) {
            var playerSettings = other.GetComponent<IPlayerSettings>();
            if (playerSettings != null) EnergyBuff(playerSettings);
        }

        private static void EnergyBuff(IPlayerSettings playerSettings) {
            playerSettings.AddEnergy(0.5f);
        }
        
    }
    
}