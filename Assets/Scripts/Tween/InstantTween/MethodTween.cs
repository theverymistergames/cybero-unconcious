using System;
using System.Reflection;
using MisterGames.Common.Routines;
using UnityEngine;

namespace Tween {
    [Serializable]public class MethodTween : Tween {
        [SerializeField] private string componentName;
        [SerializeField] private string methodName;
        [SerializeField] private float args;
        [SerializeField] private bool useArgs;

        private float _oldProgress = -1;
        private Component _component;
        private MethodInfo _method;
        private object[] _args;
        
        private BindingFlags _flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
        
        public override void Init(GameObject gameobj, TimeDomain domain) {
            base.Init(gameobj, domain);

            _args = new object[1];
            _args[0] = args;
            
            _component = tweenableObject.GetComponent(componentName);
            
            if (_component == null) {
                Debug.LogWarning("No component with name " + componentName);
                return;
            }

            var methods = _component.GetType().GetMethods(_flags);
            
            foreach (var methodInfo in methods) {
                if (methodInfo.Name.Contains(methodName)) {
                    _method = methodInfo;
                    break;
                }
            }

            if (_method == null) {
                Debug.LogWarning("No method with name " + methodName);
                return;
            }
        }
        
        protected override void ProceedUpdate(float progress) {
            if (_component == null || _method == null) return;

            if (_oldProgress == 0 && progress > 0) {
                _method.Invoke(_component, useArgs ? _args : null);
            }

            _oldProgress = progress;
        }
    }
}