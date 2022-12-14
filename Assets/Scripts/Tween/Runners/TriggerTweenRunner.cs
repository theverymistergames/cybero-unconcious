using UnityEngine;

namespace Tween {

    public class TriggerTweenRunner : TweenRunner {
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                RunTween();
            }
        }
    }

}
