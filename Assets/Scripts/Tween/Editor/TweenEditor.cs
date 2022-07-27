#if (UNITY_EDITOR)
    using MisterGames.Common.Editor.Utils;
    using Tween;
    using UnityEditor;
    using UnityEngine;


    namespace Tween {
        [CustomPropertyDrawer(typeof(Tween))]
        public class TweenPropertyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                EditorGUI.PropertyField(position, property, label, true);
                
                if (property.isExpanded) {
                    var targetObject = ((TweenController)property.serializedObject.targetObject).gameObject;
                    var type = (TransformTypes)property.FindPropertyRelative("type").GetValue();

                    var startButton = GUI.Button(new Rect(position.x, position.height + position.y - 40, position.width, 18), new GUIContent("Set start value"));
                    var endButton = GUI.Button(new Rect(position.x, position.height + position.y - 20, position.width, 18), new GUIContent("Set end value")); 
                    
                    if (startButton || endButton) {
                        var point = property.FindPropertyRelative(startButton ? "start" : "end");

                        point.vector3Value = type switch {
                            TransformTypes.Position => targetObject.transform.localPosition,
                            TransformTypes.Rotation => targetObject.transform.eulerAngles,
                            _ => targetObject.transform.localScale
                        };
                    }
                }
            }
            
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
                if (property.isExpanded) {
                    return EditorGUI.GetPropertyHeight(property) + 40;
                }
                
                return EditorGUI.GetPropertyHeight(property);
            }
        }
    }
#endif