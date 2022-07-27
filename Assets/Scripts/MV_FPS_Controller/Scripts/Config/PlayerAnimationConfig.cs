using System;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Config {

    [Serializable]
    public class PlayerAnimationConfig {

        public bool enabled = true;

        public BreathConfig breath;
        public FovConfig fov;
        public HeadBobConfig headBob;
        
        public LeanTiltConfig leanTilt;
        public JumpAnimationConfig jump;
        public LandingConfig landing;
        public PositionsConfig positions;
        
    }


    [Serializable]
    public class BreathConfig {

        public bool enabled = true;
        
        [Header("Conditions")]
        public bool enabledWhileFalling = false;

        [Header("Amount")]
        [Min(0f)]
        public float amountMultiplier = 0.2f;
        [Min(0f)]
        public float amountMinimum = 0.05f;
        public AnimationCurve amountByEnergy = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        [Header("Motion")]
        [Min(0f)]
        public float rotationMultiplier = 5f;
        [Min(0f)]
        public float offsetMultiplier = 0.1f;
        [Min(0f)]
        public float random = 0.3f;
        public AnimationCurve breathCurve = AnimationCurves.Arc(0f, 0f, 0f, 1f, 1f, 0f);
        
    }


    [Serializable]
    public class FovConfig {

        public bool enabled = true;
        [Min(0f)]
        public float idleAngle = 60f;
        [Min(0f)]
        public float smoothing = 1f;
        [Min(0f)]
        public float amountMultiplier = 10f;
        public AnimationCurve amountBySpeed = AnimationCurves.Arc(0f, 0f, 0f, 1f, 1f, 3f);
        
    }

    [Serializable]
    public class HeadBobConfig {

        public bool enabled = true;
        
        [Header("Amount")]
        [Min(0f)]
        public float amountMultiplier = 0.8f;
        [Min(0f)]
        public float amountMinimum = 0.1f;
        public AnimationCurve amountBySpeed = AnimationCurves.Arc(0f, 0f, 0f, 1f, 1f, 3f);
        
        [Header("Rotation")]
        [Min(0f)]
        public float rotationMultiplier = 3f;
        [Min(0f)]
        public float rotationXMultiplier = 1f;
        [Min(0f)]
        public float rotationYMultiplier = 0.25f;
        [Min(0f)]
        public float rotationZMultiplier = 0.25f;
        
        [Header("Position offset")]
        [Min(0f)]
        public float offsetMultiplier = 0.3f;
        
        [Header("Motion")]
        [Min(0f)]
        public float smoothing = 1.5f;
        [Min(0f)]
        public float random = 0.3f;
        public AnimationCurve headBobCurve = AnimationCurves.Arc(
            0f, 0f, 0f, 
            0.5f, 1f, 0f, 0f,
            1f, 0f, 0f
        );

    }
    

    [Serializable]
    public class LeanTiltConfig {

        [Header("Tilt")]
        public bool tiltEnabled = true;
        [Min(0f)]
        public float tiltAngle = 2f;
        [Min(0f)]
        public float tiltDuration = 0.7f;
        
        [Header("Lean")]
        public bool leanEnabled = true;
        public bool canLeanWhileMoving = true;
        public bool canLeanWhileRunning = false;
        public bool canLeanWhileJumping = false;
        public bool canLeanWhileSliding = false;
        
        [Min(0f)]
        public float leanAngle = 25f;
        [Min(0f)]
        public float leanDuration = 0.5f;
        public AnimationCurve leanTiltCurve = AnimationCurves.Arc(0f, 0f, 3f, 1f, 1f, 0f);

    }
    
    
    [Serializable]
    public class JumpAnimationConfig {

        public bool enabled = true;
        [Min(0f)]
        public float duration = 0.5f;
        [Min(0f)]
        public float random = 0.4f;
        public AnimationCurve jumpCurve = AnimationCurves.Arc(0f, 0f, -6f, 1f, 0f, -1f);
        
    }


    [Serializable]
    public class LandingConfig {

        public bool enabled = true;

        [Header("Duration")]
        [Min(0f)]
        public float minDuration = 0.01f;
        [Min(0f)]
        public float durationMultiplier = 1f;
        public AnimationCurve durationByForce = AnimationCurves.Arc(0f, 0f, 2f, 1f, 1f, 0f);

        [Header("Squat")] 
        [Min(0f)]
        public float squatAmountMultiplier = 5f;
        public AnimationCurve squatAmountByForce = AnimationCurves.Arc(0f, 0f, 0f, 1f, 1f, 3f);
        [Min(0f)]
        public float squatCurveMultiplier = 0.2f;
        public AnimationCurve squatCurve = AnimationCurves.Arc(0f, 0f, -6f, 1f, 0f, -1f);

        [Header("Nod")] 
        [Min(0f)]
        public float nodAmountMultiplier = 5f;
        public AnimationCurve nodAmountByForce = AnimationCurves.Arc(0f, 0f, 0f, 1f, 1f, 3f);
        [Min(0f)]
        public float nodCurveMultiplier = 1f;
        [Min(0f)]
        public float nodRandomAddition = 0.4f;
        public AnimationCurve nodCurve = AnimationCurves.Arc(0f, 0f, -5f, 1f, 0f, -1f);
        
    }
    
    
    [Serializable]
    public class PositionsConfig {

        public bool enabled = true;
        
        [Header("Crouch")]
        [Min(0f)]
        public float crouchDuration = 0.3f;
        public AnimationCurve crouchCurve = AnimationCurves.Arc(0f, 1f, -2f, 1f, 0f, 0f);
        
        [Header("Stand")]
        [Min(0f)]
        public float standDuration = 0.5f;
        public AnimationCurve standCurve = AnimationCurves.Arc(0f, 0f, 2f, 1f, 1f, 0f);
        
    }

}