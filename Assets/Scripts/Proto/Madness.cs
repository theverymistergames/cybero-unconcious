using MisterGames.Common.Layers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Madness : MonoBehaviour {
    private Volume _volume;
    
    private ChromaticAberration _aberration;
    private LensDistortion _distortion;
    private FilmGrain _grain;
    private ColorCurves _curves;
    
    private float _pulseTimer;
    private bool _safe;
    private float _madnessLevel;

    private TextureCurve _curve;
    private Collider[] _colliders = new Collider[5];
    private RaycastHit _hit;
    
    private LayerMask _castMask;
    private LayerMask _wallMask;
    private LayerMask _enemyMask;

    private float _maxDistance;
    
    [Header("Madness settings")]
    public AnimationCurve madnessByTimeCurve;
    public float madnessTime = 10;
    public float upSpeedMultiplier = 2;
    public float downSpeedMultiplier = 5;
    public float enemyMultiplier = 5;

    [Header("Pulse settings")]
    public float pulseSpeed = 5;
    public float pulseAmplitude = 1;
    
    [Header("Distance settings")]
    public AnimationCurve distanceCurve;
    public AnimationCurve enemyDistanceCurve;
    public float psyMaxDistance = 5;
    public float enemyMaxDistance = 5;

    private void Start() {
        _volume = FindObjectOfType<Volume>();
        VolumeManager.instance.Register(_volume, 0);
        
        _volume.profile.TryGet(out _aberration);
        _volume.profile.TryGet(out _distortion);
        _volume.profile.TryGet(out _grain);
        _volume.profile.TryGet(out _curves);
        
        _curve = new TextureCurve(new [] {
            new Keyframe(0, 0.5f),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(0.63f, 0.5f),
            new Keyframe(0.75f, 0.5f),
            new Keyframe(1, 0.5f)
        }, 0, false, new Vector2(0, 1));

        _enemyMask = LayerMask.GetMask("Enemy");
        _castMask = LayerMask.GetMask("Psy", "Enemy");
        _wallMask = LayerMask.GetMask("Wall", "Door");

        _maxDistance = Mathf.Max(psyMaxDistance, enemyMaxDistance);
    }

    private void Update() {
        var madnessLevelDown = 0f;
        var madnessLevelUp = upSpeedMultiplier;
        var position = transform.position;
        
        var collidersCount = Physics.OverlapSphereNonAlloc(position, _maxDistance, _colliders, _castMask);
        
        for (var i = 0; i < collidersCount; i++) {
            var target = _colliders[i].gameObject.transform;
            var targetPosition = target.position;
            
            Physics.Raycast(new Ray(transform.position, targetPosition - position), out _hit);

            var distance = Vector3.Distance(targetPosition, position);
            if (_hit.distance < distance && _wallMask.Contains(_hit.collider.gameObject.layer)) continue;

            var layer = target.gameObject.layer;
            
            if (_enemyMask.Contains(layer)) {
                madnessLevelUp += enemyDistanceCurve.Evaluate(distance / enemyMaxDistance) * enemyMultiplier;
            } else {
                madnessLevelDown += distanceCurve.Evaluate(distance / psyMaxDistance) * downSpeedMultiplier;
            }
        }
        
        _pulseTimer += Time.deltaTime;
        _madnessLevel += Time.deltaTime * (madnessLevelUp - madnessLevelDown) / madnessTime;
        
        if (_madnessLevel <= 0) _madnessLevel = 0;
        else if (_madnessLevel >= 1) _madnessLevel = 1;

        var madnessValue = madnessByTimeCurve.Evaluate(_madnessLevel);
        var pulseValue = Mathf.Sin(_pulseTimer * pulseSpeed) * pulseAmplitude;

        _aberration.intensity.value = madnessValue;
        _distortion.xMultiplier.value = 0.35f + madnessValue * 0.1f + pulseValue * madnessValue * 0.2f;
        _grain.intensity.value = madnessValue * 0.3f;

        for (var i = 0; i < 5; i++) {
            _curve.MoveKey(i, new Keyframe(new [] { 0, 0.5f, 0.63f, 0.75f, 1f }[i], 0.5f - 0.5f * _madnessLevel * (i == 2 ? -1 : 1)));
        }

        _curves.hueVsSat.value = _curve;
    }
}
