using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lights
{
    public class AnimatedLightWithMeshBase : MonoBehaviour
    {
        public Renderer emissiveObject;
        [NonSerialized] public float Multiplier = 1;
        [NonSerialized] public float DelayMultiplier = 1;
        
        private bool _isAnimationPlaying, _isAnimationStopped;
        protected Light _light;
        
        private void Start()
        {
            _light = GetComponent<Light>();
        }
        
        private void Update()
        {
            if (!_isAnimationPlaying && !_isAnimationStopped)
            {
                StartCoroutine(StartAnimation());
            }
        }
        
        private IEnumerator StartAnimation()
        {
            _isAnimationPlaying = true;
            yield return StartCoroutine(Animate());
            _isAnimationPlaying = false;
        }

        public void StopAnimation()
        {
            _isAnimationStopped = true;
        }

        public void ResumeAnimation()
        {
            _isAnimationStopped = false;
        }
        
        public virtual IEnumerator Animate()
        {
            throw new NotImplementedException();
        }
    }
}
