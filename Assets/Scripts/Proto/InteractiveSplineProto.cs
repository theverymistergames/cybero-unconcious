using Cysharp.Threading.Tasks;
using MisterGames.Character.Access;
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

        [Header("Start interaction")]
        [SerializeField] [Min(0f)] private float _enterDuration;

        [Header("Bounds")]
        [SerializeField] [Min(0f)] private float _startReserveBoundLength;
        [SerializeField] [Min(0f)] private float _endReserveBoundLength;

        private ITimeSource _timeSource;
        private float _progress;

        private void Awake() {
            _timeSource = TimeSources.Get(PlayerLoopStage.Update);
        }
        /*
        private  void OnEnable() {
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
            _progress = GetClosestSplineInterpolation(position);
            var startPosition = _splineContainer.EvaluatePosition(_progress);

            await MoveToStartPosition(startPosition, _enterDuration);

            adapter.SetMotionOverride(delta => {
                float prevT = _progress;
                _progress = _splineContainer.MoveAlongSpline(delta, _progress);

                if (prevT < 1f && _progress >= 1f || prevT > 0f && _progress <= 0f) _interactive.StopInteractByUser(user);

                adapter.Position = _splineContainer.EvaluatePosition(_progress);
            });
        }

        private void OnStopInteract() {
            var adapter = CharacterAccess.Instance.CharacterAdapter;

            adapter.EnableGravity(true);
            adapter.EnableCharacterController(true);
            adapter.SetMotionOverride(null);
        }

        private float GetClosestSplineInterpolation(Vector3 position) {
            float splineLength = _splineContainer.CalculateLength();
            if (splineLength <= 0f) return 0f;

            _splineContainer.GetNearestPoint(position, out float t);

            float inverseSplineLength = 1f / splineLength;
            float startReserveT = math.clamp(_startReserveBoundLength * inverseSplineLength, 0f, 0.5f);
            float endReserveT = math.clamp((splineLength - _endReserveBoundLength) * inverseSplineLength, 0.5f, 1f);

            return math.clamp(t, startReserveT, endReserveT);
        }

        private async UniTask MoveToStartPosition(Vector3 position, float duration) {
            var fromPosition = adapter.Position;

            float progress = 0f;
            while (true) {
                progress = Mathf.Clamp01(duration > 0f ? progress + _timeSource.DeltaTime / duration : 1f);
                adapter.Position = Vector3.Lerp(fromPosition, position, progress);

                if (progress >= 1f) break;
                await UniTask.Yield();
            }
        }
        */
    }

}
