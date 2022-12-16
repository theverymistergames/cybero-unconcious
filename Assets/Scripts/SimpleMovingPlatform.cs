using System;
using UnityEngine;

namespace DefaultNamespace {

    [Obsolete("SimpleMovingPlatform is obsolete")]
    public class SimpleMovingPlatform : MonoBehaviour {

        private void Awake() {
            Debug.LogError($"SimpleMovingPlatform is obsolete: gameObject {gameObject.name}.");
        }
    }

}
