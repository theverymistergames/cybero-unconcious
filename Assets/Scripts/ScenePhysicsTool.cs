#if (UNITY_EDITOR)
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.PlayerLoop;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public class ScenePhysicsTool : EditorWindow
    {
        // [SerializeField]
        private float speed = 1;
        
        private void OnGUI()
        {
            speed = EditorGUILayout.Slider(new GUIContent("Speed"), speed, 0, 10);
            
            if (GUILayout.Button("Run physics"))
            {
                RunPhysics();
            }
            
            if (GUILayout.Button("Stop physics"))
            {
                StopPhysics();
            }
            
            if (GUILayout.Button("Step physics"))
            {
                StepPhysics();
            }
            
            if (GUILayout.Button("Enable/disable ragdoll"))
            {
                SwitchRagdoll();
            }
        }
        
        private void RunPhysics()
        {
            Physics.autoSimulation = false;
            EditorApplication.update += Update;
        }
        
        private void StopPhysics()
        {
            EditorApplication.update -= Update;
            Physics.autoSimulation = true;
        }
        
        private void Update()
        {
            Physics.Simulate(Time.fixedDeltaTime * speed / 5);
        }

        private void StepPhysics()
        {
            Physics.autoSimulation = false;
            Physics.Simulate(Time.fixedDeltaTime);
            Physics.autoSimulation = true;
        }

        private void SwitchRagdoll()
        {
            if (!Selection.activeGameObject) return;
            
            var rbs = Selection.activeGameObject.GetComponentsInChildren<Rigidbody>();
            
            foreach (var rigidbody in rbs)
            {
                rigidbody.freezeRotation = !rigidbody.freezeRotation;
            }
        }

        [MenuItem("Tools/Scene Physics")]
        private static void OpenWindow()
        {
            GetWindow<ScenePhysicsTool>(false, "Physics", true);
        }
    }
#endif