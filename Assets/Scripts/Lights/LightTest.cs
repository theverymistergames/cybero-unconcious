using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightTest : MonoBehaviour
{
    private HDAdditionalLightData _light;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<HDAdditionalLightData>();
        // _light.Invoke("RequestShadowMapRendering", 5);
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(10);
        _light.RequestShadowMapRendering();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
