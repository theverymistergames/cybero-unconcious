using System;
using UnityEngine;

namespace Tween {

    [Serializable]
    public class ActivateTween : Tween {

        [SerializeField] private bool active;

        private float _oldProgress = -1;
        
        private void Toggle() {
            tweenableObject.SetActive(active);
        }
        
        protected override void ProceedUpdate(float progress) {
            if (_oldProgress == 0 && progress > 0) Toggle();

            _oldProgress = progress;
        }
    }

}
