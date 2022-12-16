using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {

    public class SimpleDoorButton : MonoBehaviour {

        [SerializeField] private SimpleDoor doorController;
        [SerializeField] private Material blockedMaterial;
        [SerializeField] private Material normalMaterial;

        private void Start() {
            if (!doorController) return;

            var image = GetComponent<Image>();
            
            if (doorController.Blocked) {
                image.material = blockedMaterial;
            } else {
                image.material = normalMaterial;
            }
        }
    }

}
