using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public sealed class SimpleFlashlight : MonoBehaviour {

    [SerializeField] [FormerlySerializedAs("light")] private GameObject _light;
    [SerializeField] [FormerlySerializedAs("flashlightMesh")] private GameObject _flashlightMesh;
    
    private bool _active;
    private Vector3 _flashlightPosition;
    private const float _flashlightShowTime = 0.3f;

    void Start() {
        _light.SetActive(_active);
        _flashlightPosition = _flashlightMesh.transform.localPosition;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            StopCoroutine(MoveFlashlight(_active));
            
            _active = !_active;

            StartCoroutine(MoveFlashlight(_active));
        }
    }

    IEnumerator MoveFlashlight(bool active) {
        if (active) _light.SetActive(true);
        
        var timer = 0f;
        
        var transform = _flashlightMesh.transform;
        
        var offPosition = _flashlightPosition;
        offPosition.y -= 1;
        offPosition.z += 8;

        while (timer < _flashlightShowTime) {
            if (active) {
                transform.localPosition = Vector3.Lerp(offPosition, _flashlightPosition, timer / _flashlightShowTime);
            } else {
                transform.localPosition = Vector3.Lerp(_flashlightPosition, offPosition, timer / _flashlightShowTime);
            }

            timer += Time.deltaTime;
            yield return null;
        }
        
        if (!active) _light.SetActive(false);
    }
}
