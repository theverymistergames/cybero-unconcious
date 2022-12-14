using System;
using UnityEngine;

namespace Tween {

    internal enum ActionType {
        Play,
        Stop,
        Pause,
        Resume,
        Rewind
    }
    
    public abstract class TweenRunner : MonoBehaviour {

        [SerializeField] private GameObject obj;
        [SerializeField] private ActionType actionType = ActionType.Play;
        
        private TweenController _controller;

        private void Start() {
            if (!obj) throw new Exception("No tween game object detected: " + gameObject.name);
        
            _controller = obj.GetComponent<TweenController>();
        
            if (!_controller) throw new Exception("No tween controller detected: " + obj.name);
        }
        
        protected void RunTween() {
            switch (actionType) {
                case ActionType.Play:
                    _controller.Play();
                    break;
                case ActionType.Rewind:
                    _controller.Rewind();
                    break;
                case ActionType.Stop:
                    _controller.Stop();
                    break;
                case ActionType.Resume:
                    _controller.Resume();
                    break;
                case ActionType.Pause:
                    _controller.Pause();
                    break;
            }
        }
    }

}
