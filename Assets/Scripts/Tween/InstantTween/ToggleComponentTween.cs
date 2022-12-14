using System;
using MisterGames.Tick.Core;
using UnityEngine;

namespace Tween {

    [Serializable]
    public class ToggleComponentTween : Tween {

        [SerializeField] private string componentName;
        [SerializeField] private bool enable;

        private Behaviour _behaviour;
        private CharacterController _controller;
        private float _oldProgress = -1;
        
        public override void Init(GameObject gameobj, PlayerLoopStage stage) {
            base.Init(gameobj, stage);
            
            var component = tweenableObject.GetComponent(componentName);
            
            if (component == null) {
                Debug.LogWarning("No component with name " + componentName);
                return;
            }

            try {
                _behaviour = (Behaviour) component;
            }
            catch (Exception) {
                _controller = (CharacterController) component;
            }
        }

        private void Toggle() {
            if (_behaviour) {
                _behaviour.enabled = enable;
            }

            if (_controller) {
                _controller.enabled = enable;
            }
        }
        
        protected override void ProceedUpdate(float progress) {
            if (_behaviour == null && _controller == null) return;
            
            if (_oldProgress == 0 && progress > 0) Toggle();

            _oldProgress = progress;
        }
    }

}
