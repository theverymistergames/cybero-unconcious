using System;
using System.Collections;
using System.Collections.Generic;
using MisterGames.Character.Pose;
using MisterGames.Fsm.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Manhole : MonoBehaviour {
    public Transform inPoint;
    public Transform startPoint;
    public Transform endPoint;
    public Transform outPoint;
    public AnimationCurve motionCurve;
    public AnimationCurve transitionCurve;

    public float transitionTime = 0.5f;
    public float moveTime = 3f;
    
    private Transform _player;
    private bool _canMove;
    private float _storedPlayerY;

    private float _moveTimer;
    private GameObject _pose;
    private bool _triggered;
    private Transform[] _movePoints;
    private float _timer;

    private void SetPlayerMotionActive(bool active) {
        _player.GetComponent<CharacterController>().enabled = active;
        _pose.SetActive(active);
    }

    private bool IsPlayerLookingAtManhole() {
        var dirFromAtoB = (transform.position - _player.position).normalized;
        var dotProd = Vector3.Dot(dirFromAtoB, _player.forward);

        return dotProd > 0.25;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || !other.GetComponent<CharacterController>().enabled || _triggered) return;

        _triggered = true;
        
        if (!_player) {
            _player = other.transform;
            _pose = _player.GetComponentInChildren<PoseConditions>().gameObject;
            _storedPlayerY = _player.position.y;
        }

        if (!IsPlayerLookingAtManhole()) return;

        SetPlayerMotionActive(false);

        if (Vector3.Distance(_player.position, startPoint.position) < Vector3.Distance(_player.position, endPoint.position)) {
            _movePoints = new[] { startPoint, endPoint, outPoint };
        } else {
            _movePoints = new[] { endPoint, startPoint, inPoint };
        }
        
        StartCoroutine(MoveIn());
    }

    private void OnTriggerExit(Collider other) {
        _triggered = false;
    }

    IEnumerator MoveIn() {
        _timer = 0f;
        var currentPosition = _player.position;
        var targetPosition = _movePoints[0].position;

        while (_timer < transitionTime) {
            _timer += Time.deltaTime;
            MovePlayerToPoint(currentPosition, targetPosition, transitionTime, transitionCurve);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartCoroutine(Move());
    }

    IEnumerator Move() {
        _timer = 0f;
        var currentPosition = _player.position;
        var targetPosition = _movePoints[1].position;

        while (_timer < moveTime) {
            if (!Input.GetKey(KeyCode.W)) {
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }
            
            _timer += Time.deltaTime;
            MovePlayerToPoint(currentPosition, targetPosition, moveTime, motionCurve);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartCoroutine(MoveOut());
    }

    IEnumerator MoveOut() {
        _timer = 0f;
        var currentPosition = _player.position;
        var targetPosition = _movePoints[2].position;

        while (_timer < transitionTime) {
            _timer += Time.deltaTime;
            MovePlayerToPoint(currentPosition, targetPosition, transitionTime, transitionCurve);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        SetPlayerMotionActive(true);
    }

    void MovePlayerToPoint(Vector3 start, Vector3 end, float maxTime, AnimationCurve curve) {
        var timerValue = _timer / maxTime;
        
        _player.position = new Vector3(
            Mathf.Lerp(start.x, end.x, timerValue),
            _storedPlayerY + curve.Evaluate(timerValue),
            Mathf.Lerp(start.z, end.z, timerValue)
        );
    }
}
