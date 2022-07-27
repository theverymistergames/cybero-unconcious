using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class SimpleRaycast : MonoBehaviour {
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private Image cursor;
        [SerializeField] private float maxCursorDistance = 3f;
        
        private Camera _camera;
        private EventSystem _system;
        
        private void Awake() {
            Application.targetFrameRate = 120;
        }
        
        private void Start() {
            _camera = Camera.main;
            _system = EventSystem.current;
        }

        private void Update() {
            var pointerData = new PointerEventData(_system) {
                position = Input.mousePosition
            };
                
            var results = new List<RaycastResult>();
            _system.RaycastAll(pointerData, results);
            results = results.Where(result => result.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast")).ToList();

            if (results.Count > 0) {
                var distance = Vector3.Distance(results[0].gameObject.transform.position, gameObject.transform.position);
                
                Debug.Log(results[0].gameObject.name);
                if (distance < maxCursorDistance) {
                    SetImageAlpha(1 - distance / maxCursorDistance);
                } else {
                    SetImageAlpha(0);
                }
            }
            else {
                SetImageAlpha(0);
            }
            
            if (Input.GetMouseButtonDown(0)) {
                if (results.Count > 0) {
                    foreach (var raycastResult in results) {
                        if (raycastResult.gameObject.GetComponent<Button>()) {
                            raycastResult.gameObject.GetComponent<Button>().onClick.Invoke();
                        }
                    }
                }
            }
        }

        private void SetImageAlpha(float value) {
            var tempColor = cursor.color;
            tempColor.a = value;
            cursor.color = tempColor;
        }
    }
}