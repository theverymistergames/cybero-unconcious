using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Proto;
using UnityEngine;

public class Cycleaner : MonoBehaviour, IOverloadable {
    public bool isEnabled = true;
    public MeshRenderer emissive;
    public Transform psyPoint;
    public Light blueLight;
    public ParticleSystem system;
    public float overloadTime = 4f;
    public float toggleTime = 0.5f;

    [Header("Curves")]
    public AnimationCurve overloadCurve;
    
    private float _toggleTimer = 1;
    
    private float _angleOffset;
    private float _emissionIntensity;
    private float _lightIntensity;
    private static readonly int EmissionIntensity = Shader.PropertyToID(EmissionKey);
    private static readonly int AngleOffset = Shader.PropertyToID(OffsetSpeedKey);

    private const string EmissionKey = "Vector1_5E4C4145";
    private const string OffsetSpeedKey = "_Angle_offset_speed";

    void Start() {
        _lightIntensity = blueLight.intensity;
        _emissionIntensity = emissive.material.GetFloat(EmissionIntensity);
        _angleOffset = emissive.material.GetFloat(AngleOffset);

        if (!isEnabled) {
            emissive.material.SetFloat(EmissionIntensity, 0);
            emissive.material.SetFloat(AngleOffset, 0);

            _toggleTimer = 0;
            blueLight.enabled = false;
            psyPoint.gameObject.SetActive(false);
            system.Stop();
        }
    }

    private void SetIntensityByValue(float value) {
        emissive.material.SetFloat(EmissionIntensity, _emissionIntensity * value);
        emissive.material.SetFloat(AngleOffset, _angleOffset * value);
        blueLight.intensity = _lightIntensity * value;
    }

    public void Overload() {
        StartCoroutine(StartOverload());
    }

    private IEnumerator StartOverload() {
        _toggleTimer = 1;
        
        while (_toggleTimer > 0) {
            _toggleTimer -= Time.deltaTime / overloadTime;

            SetIntensityByValue(overloadCurve.Evaluate(1 - _toggleTimer));
            
            yield return new WaitForSeconds(Time.deltaTime);   
        }

        _toggleTimer = 0;
        psyPoint.gameObject.SetActive(false);
        system.Stop();

        isEnabled = false;
    }

    private IEnumerator ToggleOff() {
        system.Stop();
        
        while (_toggleTimer > 0) {
            _toggleTimer -= Time.deltaTime / toggleTime;

            SetIntensityByValue(_toggleTimer);
            
            yield return new WaitForSeconds(Time.deltaTime);   
        }

        _toggleTimer = 0;
        
        psyPoint.gameObject.SetActive(false);
    }

    private IEnumerator ToggleOn() {
        psyPoint.gameObject.SetActive(true);
        system.Play();
        
        while (_toggleTimer < 1) {
            _toggleTimer += Time.deltaTime / toggleTime;

            SetIntensityByValue(_toggleTimer);
            
            yield return new WaitForSeconds(Time.deltaTime);   
        }

        _toggleTimer = 1;
    }

    public void Toggle() {
        if (isEnabled) {
            StopCoroutine(ToggleOn());
            StartCoroutine(ToggleOff());
        } else {
            StopCoroutine(ToggleOff());
            StartCoroutine(ToggleOn());
        }
        
        isEnabled = !isEnabled;
    }
}
