using System;
using MisterGames.Tick.Core;
using Tween;
using UnityEngine;

namespace DefaultNamespace {

    public class SimpleMovingPlatform : MonoBehaviour {

        [SerializeField] private Vector3[] positions = new Vector3[2];
        [SerializeField] private SimpleDoor[] doors;
        [SerializeField] private int startPositionID = 0;
        [SerializeField] private string transformTweenID = "Transform";
        
        private bool _inProgress;
        private int _currentPositionID;
        private int _positionsTotal = 0;
        private TweenController _controller;
        private TransformTween _tween;

        private void Start() {
            _positionsTotal = positions.Length;
            _currentPositionID = startPositionID;
            _controller = gameObject.GetComponent<TweenController>();
            
            _tween = _controller.GetTweenById(transformTweenID) as TransformTween;
            
            if (_tween == null) {
                throw new NullReferenceException("No tween with ID " + transformTweenID);
            }
            
            _tween.SetStartPosition(positions[startPositionID]);
            
            _controller.Rewind();
            
            _controller.OnFinished += OnControllerFinished;
        }

        private void OnControllerFinished() {
            _inProgress = false;

            if (doors[_currentPositionID]) {
                doors[_currentPositionID].Open();
            }
        }

        public void MoveToPositionById(int newPositionID) {
            if (newPositionID >= _positionsTotal || newPositionID < 0 || _inProgress) return;

            if (newPositionID == _currentPositionID) {
                if (doors[_currentPositionID]) {
                    doors[_currentPositionID].Open();
                }
                
                return;
            }
            
            _tween.SetStartPosition(positions[_currentPositionID]);
            _tween.SetEndPosition(positions[newPositionID]);
            
            if (doors[_currentPositionID]) {
                doors[_currentPositionID].Close();
            }

            _currentPositionID = newPositionID;
            
            _controller.Play();

            _inProgress = true;
        }

        public void MoveForward() {
            if (_currentPositionID >= _positionsTotal || _inProgress) return;

            MoveToPositionById(_currentPositionID + 1);
        }
        
        public void MoveBackward() {
            if (_currentPositionID <= 0 || _inProgress) return;
            
            MoveToPositionById(_currentPositionID - 1);
        }
    }

}
