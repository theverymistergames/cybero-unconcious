using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lights
{
    [Serializable]
    public struct FlickParams
    {
        public Vector2 delayTime;
        public float flickTime;
        public float maxDelayBase;
    }
    
    public class FlickeringLight : AnimatedLightWithMeshBase
    {
        public FlickParams config;

        public override IEnumerator Animate()
        {
            var delay = Random.Range(config.delayTime[0], config.delayTime[1]) / Multiplier;
            yield return new WaitForSeconds(delay / Multiplier);
        
            emissiveObject.enabled = true;
            Light.enabled = true;
            
            yield return new WaitForSeconds(config.flickTime / Multiplier);
            
            emissiveObject.enabled = false;
            Light.enabled = false;
            
            yield return new WaitForSeconds(config.maxDelayBase * DelayMultiplier);
        }
    }
}
