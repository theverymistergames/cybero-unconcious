using Cysharp.Threading.Tasks;
using MisterGames.Character.Adapters;
using MisterGames.Interact.Core;
using MisterGames.Splines.Utils;
using MisterGames.Tick.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Proto {

    public class InteractiveSplineProto : MonoBehaviour {

        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private Interactive _interactive;
        [SerializeField] [Min(0f)] private float _enterDuration;
        [SerializeField] [Min(0f)] private float _enterReserveBoundLength;

        private ITimeSource _timeSource;
        private float _t;

        private void Awake() {
            _timeSource = TimeSources.Get(PlayerLoopStage.Update);
        }

        private void OnEnable() {
            _interactive.OnStartInteract += OnStartInteract;
            _interactive.OnStopInteract += OnStopInteract;
        }

        private void OnDisable() {
            _interactive.OnStartInteract -= OnStartInteract;
            _interactive.OnStopInteract -= OnStopInteract;
        }

        private async void OnStartInteract(InteractiveUser user) {
            var adapter = CharacterAccess.Instance.CharacterAdapter;

            // Disable gravity and motion input
            adapter.EnableGravity(false);
            adapter.EnableCharacterController(false);
            adapter.SetMotionOverride(_ => { });

            var position = adapter.Position;
            var startPosition = _splineContainer.GetNearestPoint(position, out float t);

            float progress = 0f;
            while (true) {
                progress = Mathf.Clamp01(_enterDuration > 0f ? progress + _timeSource.DeltaTime / _enterDuration : 1f);
                adapter.Position = Vector3.Lerp(position, startPosition, progress);

                if (progress >= 1f) break;
                await UniTask.Yield();
            }

            float splineLength = _splineContainer.CalculateLength();
            float reserveOffsetT = splineLength > 0f ? math.clamp(_enterReserveBoundLength / splineLength, 0f, 0.5f) : 0f;
            _t = math.min(math.max(reserveOffsetT, t), 1 - reserveOffsetT);

            adapter.SetMotionOverride(delta => {
                float prevT = _t;
                _t = _splineContainer.MoveAlongSpline(delta, _t);

                if (prevT < 1f && _t >= 1f || prevT > 0f && _t <= 0f) _interactive.StopInteractByUser(user);

                adapter.Position = _splineContainer.EvaluatePosition(_t);
            });
        }

        private void OnStopInteract() {
            var adapter = CharacterAccess.Instance.CharacterAdapter;

            adapter.EnableGravity(true);
            adapter.EnableCharacterController(true);
            adapter.SetMotionOverride(null);
        }
    }

}
