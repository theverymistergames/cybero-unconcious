using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Madness : MonoBehaviour
{
    private Volume _volume;
    private ChromaticAberration _aberration;
    private LensDistortion _distortion;
    private FilmGrain _grain;
    public AnimationCurve pulseCurve = AnimationCurve.Constant(0, 1, 1);
    private float _pulseTimer;

    public float madnessTime = 10;
    private bool _safe = false;
    private float _madnessLevel;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("jija"))
        {
            _safe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("jija"))
        {
            _safe = false;
        }
    }

    private void Start()
    {
        _volume = FindObjectOfType<Volume>();
        VolumeManager.instance.Register(_volume, 0);
        _volume.profile.TryGet(out _aberration);
        _volume.profile.TryGet(out _distortion);
        _volume.profile.TryGet(out _grain);
    }

    private void Update()
    {
        _pulseTimer = (_pulseTimer + Time.deltaTime) % 1;
        
        if (!_safe && _madnessLevel < 1) _madnessLevel += Time.deltaTime / madnessTime;
        else if (_safe && _madnessLevel > 0) _madnessLevel -= Time.deltaTime * 5 / madnessTime;

        if (_madnessLevel <= 0) return;
        
        // _aberration.intensity.value = curve.Evaluate(_madnessLevel);
        // _distortion.xMultiplier.value = 0.5f - curve.Evaluate(_madnessLevel) * 0.1f;
        // _grain.intensity.value = curve.Evaluate(_madnessLevel) * 0.4f;
        
        _aberration.intensity.value = _madnessLevel;
        _distortion.xMultiplier.value = 0.3f + _madnessLevel * 0.1f + pulseCurve.Evaluate(_pulseTimer) * _madnessLevel * 0.2f;
        _grain.intensity.value = _madnessLevel * 0.4f;
    }
}
