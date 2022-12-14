using System;
using MisterGames.Common.Attributes;
using Tween;
using UnityEngine;
using UnityEngine.Events;

public enum DoorState {
    Opened,
    Closed
}

public class SimpleDoor : MonoBehaviour {

    [SerializeField] private DoorState state = DoorState.Closed;
    [SerializeField] private bool blocked = false;
    
    private ITweenController _controller;
    private bool _inProgress;
    private AudioSource _source;

    public DoorState State => state;
    public bool Blocked => blocked;

    public void Toggle() {
        if (blocked || _inProgress) return;
        
        if (_source) {
            _source.Play();
        }
        
        _inProgress = true;
        _controller.Play();
    }

    public void Open() {
        if (blocked || state == DoorState.Opened) return;
        
        _inProgress = true;

        if (_source) {
            _source.Play();
        }
        
        _controller.Revert(false);
        _controller.Play();
    }

    public void Close() {
        if (blocked || state == DoorState.Closed) return;
        
        _inProgress = true;

        if (_source) {
            _source.Play();
        }
        
        _controller.Revert(true);
        _controller.Play();
    }

    private void Start() {
        _controller = GetComponent<ITweenController>();
        if (_controller == null) return;

        if (state == DoorState.Closed) {
            _controller.Rewind();
        } else {
            _controller.Wind();
        }
        
        _controller.Revert(state == DoorState.Opened);

        _controller.OnFinished += OnControllerFinished;

        _source = GetComponentInChildren<AudioSource>();
    }

    private void OnControllerFinished() {
        if (blocked) return;
        
        if (state == DoorState.Closed) state = DoorState.Opened;
        else state = DoorState.Closed;
        
        _controller.Revert(state == DoorState.Opened);

        _inProgress = false;
    }
}
