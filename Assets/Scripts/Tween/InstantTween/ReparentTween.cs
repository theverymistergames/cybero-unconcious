using System;
using UnityEngine;

namespace Tween {
    [Serializable]public class ReparentTween : Tween {
        [SerializeField] private GameObject reparentFrom;
        [SerializeField] private GameObject reparentTo;

        private float _oldProgress = -1;
        
        protected override void ProceedUpdate(float progress) {
            // if (progress == 0) {
            //     if (reparentFrom) {
            //         tweenableObject.transform.SetParent(reparentFrom.transform, true);
            //     }
            //     else tweenableObject.transform.parent = null;
            // } else {
                if (_oldProgress == 0 && progress > 0) {
                    if (reparentTo) tweenableObject.transform.SetParent(reparentTo.transform, true);
                    else tweenableObject.transform.parent = null;
                }                
            // }

            _oldProgress = progress;
        }
    }
}