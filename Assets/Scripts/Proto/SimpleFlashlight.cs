using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlashlight : MonoBehaviour {
    public GameObject light;
    public GameObject flashlightMesh;
    
    private bool _active;
    private Vector3 _flashlightPosition;
    private float _flashlightShowTime = 0.3f;
    
    void Start() {
        light.SetActive(_active);
        _flashlightPosition = flashlightMesh.transform.localPosition;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            StopCoroutine(MoveFlashlight(_active));
            
            _active = !_active;

            StartCoroutine(MoveFlashlight(_active));
        }
    }

    IEnumerator MoveFlashlight(bool active) {
        if (active) light.SetActive(true);
        
        var timer = 0f;
        
        var transform = flashlightMesh.transform;
        
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
        
        if (!active) light.SetActive(false);
    }
}
