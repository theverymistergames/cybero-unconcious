using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FastEmissionBlink : MonoBehaviour
{
    private Material _material;
    private Color _color = new Color(.81f, .93f, .16f);
    private bool _isActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
        {
            _isActive = true;
            StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        _material.SetColor("_EmissiveColor", Color.red * 100f);
        yield return new WaitForSeconds(Random.Range(0, 5));

        _material.SetColor("_EmissiveColor", Color.black * 100f);
        yield return new WaitForSeconds(Random.Range(0, 5));

        _isActive = false;
    }
}
