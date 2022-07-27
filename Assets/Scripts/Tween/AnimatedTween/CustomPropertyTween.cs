using System;
using System.Reflection;
using MisterGames.Common.Routines;
using UnityEngine;

namespace Tween
{
    [Serializable]public class CustomTween : AnimatedTween {
        [SerializeField] private string componentName;
        [SerializeField] private string fieldName;
        [SerializeField] private float startValue = 0;
        [SerializeField] private float endValue = 1;

        private Component _component;
        private FieldInfo _field;
        private PropertyInfo _property;
        
        private BindingFlags _flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
        
        public override void Init(GameObject gameobj, TimeDomain domain) {
            base.Init(gameobj, domain);
            
            _component = tweenableObject.GetComponent(componentName);
            
            if (_component == null) {
                Debug.LogWarning("No component with name " + componentName);
                return;
            }
            
            _field = _component.GetType().GetField(fieldName, _flags);
            
            if (_field == null) _property = _component.GetType().GetProperty(fieldName, _flags);

            if (_field == null && _property == null) {
                Debug.LogWarning("No field with name " + fieldName);
                return;
            }
        }
        
        protected override void ProceedUpdate(float progress) {
            if (_component == null || (_field == null && _property == null)) return;
            
            var value = GetCurveValue(progress);
            
            if (_field != null) {
                _field.SetValue(_component, Mathf.Lerp(startValue, endValue, value));
            } else {
                _property.SetValue(_component, Mathf.Lerp(startValue, endValue, value));
            }
        }
    }
}