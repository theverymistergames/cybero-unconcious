using System;
using MV_FPS_Controller.Scripts.Util;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Config {
    
    [Serializable]
    public class PlayerConfig {

        public LookConfig look;
        public MoveConfig move;
        public JumpConfig jump;
        public SizeConfig size;
        public TouchConfig touch;
        public EnergyConfig energy;
        public StepCycleConfig stepsCycle;
        public BreathCycleConfig breathCycle;

    }
    

    [Serializable]
    public class LookConfig {

        [Header("Smooth")]
        public bool smoothEnabled = true;
        [Min(0.1f)]
        public float smoothAcceleration = 17f;
        
        [Header("Constraints")]
        public bool verticalClampEnabled = true;
        public bool horizontalClampEnabled = false;
        
        [Min(0f)]
        public float verticalClamp = 90f;
        [Min(0f)]
        public float horizontalClamp = 90f;

    }


    [Serializable]
    public class MoveConfig {
        
        [Header("Smooth")]
        public bool smoothEnabled = true;
        [Min(0.1f)]
        public float smoothAcceleration = 17f;
        
        [Header("Speed")]
        [Min(0f)]
        public float walkSpeed = 4f;
        [Min(0f)]
        public float runSpeed = 7f;
        [Min(0f)]
        public float sneakSpeed = 2f;
        [Min(0f)]
        public float jumpMoveSpeed = 2f;
        
        [Header("Speed correction")]
        [Min(0f)]
        public float backwardsCorr = 0.6f;
        [Min(0f)]
        public float strafeCorr = 0.7f;
        
        [Header("Forces")]
        [Min(0f)]
        public float gravity = 15f;
        [Min(0f)]
        public float stickToGroundForce = 7f;
        [Min(0f)]
        public float maxLandingForce = 15f;

    }

    
    [Serializable]
    public class JumpConfig {
        
        [Header("Force")]
        [Min(0f)]
        public float forceFromStand = 5f;
        [Min(0f)]
        public float forceFromCrouch = 2f;
        [Min(0f)]
        public float forceInAir = 4f;

        [Header("Conditions")]
        public bool canJump = true;
        public bool canJumpWhileCrouching = false;
        public bool canJumpWhileSliding = false;
        public bool jumpWhileCrouchingCausesStandUp = false;
        public bool multiJump = false;
        [Min(0)]
        public int additionalJumps = 1;
        
    }
    
    
    [Serializable]
    public class EnergyConfig {
        
        [Header("Consumption")]
        [Tooltip("When energy value becomes zero, player can not run until energy restores to recovery gap value.")]
        public bool consumeOnRun = true;
        [Min(0f)]
        public float consumeSpeedOnRunMin = 0.01f;
        [Min(0f)]
        public float consumeSpeedOnRunMultiplier = 0.1f;
        public AnimationCurve consumptionSpeedByEnergy = AnimationCurves.Arc(0f, 0f, 3f, 1f, 1f, 0f);
        
        [Header("Recovery")]
        public bool recoverOnIdleOnly = false;
        
        [Tooltip("Energy value after which player can run again, if energy was at zero value previously.")]
        [Min(0f)]
        public float recoveryGap = 0.3f;
        [Min(0f)]
        public float recoverySpeedMin = 0.01f;
        [Min(0f)]
        public float recoverySpeedMultiplier = 0.1f;
        public AnimationCurve recoverySpeedByEnergy = AnimationCurves.Arc(0f, 0f, 3f, 1f, 1f, 0f);

    }


    [Serializable]
    public class StepCycleConfig {

        public bool enabled = true;
        [Min(0f)]
        public float lengthMultiplier = 1.5f;
        [Min(0f)]
        public float minLength = 0.6f;
        public AnimationCurve lengthBySpeed = AnimationCurves.Arc(0f, 0f, 3f, 1f, 1f, 0f);

    }
    
    [Serializable]
    public class BreathCycleConfig {

        public bool enabled = true;
        
        [Header("Conditions")]
        public bool enabledWhileFalling = true;
        public AnimationCurve amplitudeBySpeed = AnimationCurves.Arc(0f, 1f, 0f, 1f, 0f, -3f);
        
        [Header("Period")]
        [Min(0f)]
        public float periodMultiplier = 1.4f;
        [Min(0f)]
        public float periodMinimum = 0.6f;
        public AnimationCurve periodByEnergy = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
    }
    

    [Serializable]
    public class SizeConfig {

        [Min(0f)]
        public float standHeight = 2f;
        [Min(0f)]
        public float crouchHeight = 1f;

    }
    
    
    [Serializable]
    public class TouchConfig {

        [Min(0f)]
        [Tooltip("Distance for raycast to the ground and ceiling.")]
        public float touchDistance = 0.1f;
        
        [Tooltip("Defines layers which are considered as ground for player. Player's layer must not be included here.")]
        public LayerMask layerMask = 1;

    }

}