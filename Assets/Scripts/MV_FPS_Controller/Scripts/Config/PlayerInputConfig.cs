using System;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Config {
    
    [Serializable]
    public class PlayerInputConfig {

        [Header("Mouse")]
        public bool inverseVerticalAxis = false;
        [Min(0f)]
        public float verticalSensitivity = 1f;
        [Min(0f)]
        public float horizontalSensitivity = 1f;
        
        [Header("Bindings")]
        public KeyCode forward = KeyCode.W;
        public KeyCode back = KeyCode.S;
        public KeyCode right = KeyCode.D;
        public KeyCode left = KeyCode.A;

        public KeyCode leanLeft = KeyCode.Q;
        public KeyCode leanRight = KeyCode.E;
        
        public KeyCode jump = KeyCode.Space;
        public KeyCode run = KeyCode.LeftShift;
        public KeyCode crouch = KeyCode.LeftControl;
        
        public KeyCode hud = KeyCode.Tab;
        
        [Header("Activation conditions")]
        [Tooltip("Action is enabled and disabled by toggle if true, enabled only while button is pressed if false")]
        public bool activateRunOnToggle = false;
        [Tooltip("Action is enabled and disabled by toggle if true, enabled only while button is pressed if false")]
        public bool activateCrouchOnToggle = false;
        
    }
    
}