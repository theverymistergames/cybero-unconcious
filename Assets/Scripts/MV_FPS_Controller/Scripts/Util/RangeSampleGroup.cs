using System;
using System.Collections.Generic;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Util {
    
    [Serializable]
    public class RangeSampleGroup {

        [Range(0f, 1f)]
        public float minValue;
        
        [Range(0f, 1f)]
        public float maxValue;
        
        public List<AudioClip> items;


        public bool ContainsValue(float value) {
            if (value >= 1f) return maxValue >= 1f;
            if (value <= 0f) return minValue <= 0f;
            return minValue <= value && value < maxValue;
        }

    }
    
}