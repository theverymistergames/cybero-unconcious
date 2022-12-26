using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace MisterGames.CyberoUnconcious.Editor {

    public static class LightUpdateMenu {

        [MenuItem("MisterGames/Tools/Render shadow map for OnDemand lights #r")]
        private static void UpdateLights() {
            if (Application.isPlaying) {
                Debug.LogWarning($"Cannot render shadow map for OnDemand lights while in play mode");
                return;
            }

            Debug.Log($"Requested shadow map rendering for OnDemand lights");

            var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            int lightRequestsCount = 0;

            foreach (var gameObject in gameObjects) {
                var lights = gameObject.GetComponentsInChildren<HDAdditionalLightData>();
                if (lights.Length == 0) continue;

                foreach (var light in lights) {
                    if (light.shadowUpdateMode != ShadowUpdateMode.OnDemand) continue;

                    light.RequestShadowMapRendering();
                    lightRequestsCount++;

                    EditorUtility.SetDirty(light);
                    Debug.Log($"Requested shadow map rendering for light on game object '{light.gameObject}'");
                }
            }

            if (lightRequestsCount == 0) {
                Debug.Log($"No game objects on the current scene have OnDemand lights, " +
                          $"nothing to render.");
                return;
            }

            Debug.Log($"Requested shadow map rendering for OnDemand lights done, " +
                      $"{lightRequestsCount} renders were performed.");
        }
    }

}
