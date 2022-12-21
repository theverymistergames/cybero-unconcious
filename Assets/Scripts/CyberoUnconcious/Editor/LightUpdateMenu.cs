using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace MisterGames.CyberoUnconcious.Editor {

    public static class LightUpdateMenu {

        [MenuItem("MisterGames/Tools/Render shadow map for OnDemand lights %&L")]
        private static void UpdateLights() {
            if (Application.isPlaying) {
                Debug.LogWarning($"Cannot render shadow map for OnDemand lights while in play mode");
                return;
            }

            Debug.Log($"Requested shadow map rendering for light with ShadowUpdateMode {ShadowUpdateMode.OnDemand}");
            int lightRequestsCount = 0;

            var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var gameObject in gameObjects) {
                var lights = gameObject.GetComponentsInChildren<HDAdditionalLightData>();
                if (lights.Length == 0) continue;

                foreach (var light in lights) {
                    if (light.shadowUpdateMode != ShadowUpdateMode.OnDemand) continue;

                    light.RequestShadowMapRendering();
                    lightRequestsCount++;

                    Debug.Log($"Requested shadow map rendering for light on game object '{light.gameObject}'");
                    EditorUtility.SetDirty(light);
                }
            }

            if (lightRequestsCount == 0) {
                Debug.Log($"No game objects on the current scene have lights with ShadowUpdateMode {ShadowUpdateMode.OnDemand}. " +
                          $"Nothing to render.");
                return;
            }

            Debug.Log($"Requested shadow map rendering for light with ShadowUpdateMode {ShadowUpdateMode.OnDemand} done. " +
                      $"Rendered {lightRequestsCount} lights.");
        }
    }

}
