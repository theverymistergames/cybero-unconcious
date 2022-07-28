using UnityEditor;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace MisterGames.CyberoUnconcious.Editor {

    public static class LightUpdateMenu {

        [MenuItem("MisterGames/Render shadow map for OnDemand lights")]
        private static void UpdateLights() {
            var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var gameObject in gameObjects) {
                var lights = gameObject.GetComponentsInChildren<HDAdditionalLightData>();
                if (lights.Length == 0) continue;

                foreach (var light in lights) {
                    if (light.shadowUpdateMode != ShadowUpdateMode.OnDemand) continue;

                    light.RequestShadowMapRendering();
                }
            }
        }
    }

}
