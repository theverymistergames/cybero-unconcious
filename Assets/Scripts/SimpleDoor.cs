using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MisterGames.Tweens.Core;
using UnityEngine;

public class SimpleDoor : MonoBehaviour {

    [SerializeField] private TweenRunner _tweenRunner;
    [SerializeField] private State _initialState;
    [SerializeField] private bool _isBlocked = false;

    public bool Blocked => _isBlocked;

    private CancellationTokenSource _cts;
    private State _currentState;
    private bool _isProcessing;

    private enum State {
        Closed,
        Opened,
    }

    private void Start() {
        _cts = new CancellationTokenSource();
        _currentState = _initialState;

        switch (_currentState) {
            case State.Opened:
                _tweenRunner.Wind();
                break;

            case State.Closed:
                _tweenRunner.Rewind();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDestroy() {
        _cts.Cancel();
        _cts.Dispose();
    }

    public void Toggle() {
        var nextState = _currentState switch {
            State.Opened => State.Closed,
            State.Closed => State.Opened,
            _ => throw new ArgumentOutOfRangeException()
        };

        SetStateAsync(nextState, _cts.Token).Forget();
    }

    public void Open() {
        SetStateAsync(State.Opened, _cts.Token).Forget();
    }

    public void Close() {
        SetStateAsync(State.Closed, _cts.Token).Forget();
    }

    private async UniTaskVoid SetStateAsync(State targetState, CancellationToken token) {
        if (_isBlocked || _isProcessing || _currentState == targetState) return;

        _isProcessing = true;

        _tweenRunner.Invert(targetState == State.Closed);
        await _tweenRunner.Play(token);

        _currentState = targetState;
        _isProcessing = false;
    }

}
