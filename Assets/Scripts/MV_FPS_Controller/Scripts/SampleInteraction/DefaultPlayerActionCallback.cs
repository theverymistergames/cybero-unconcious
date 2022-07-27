using MV_FPS_Controller.Scripts.Player;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.SampleInteraction {

    public class DefaultPlayerActionCallback : PlayerActionsCallback {
        
        public override void OnPause() {
            Debug.Log("OnPause");
        }

        public override void OnResume() {
            Debug.Log("OnResume");
        }

        public override void OnLook(Vector2 delta) {
            Debug.Log($"OnLook, delta {delta}");
        }

        public override void OnMove(Vector2 dir) {
            Debug.Log($"OnMove, dir {dir}");
        }

        public override void OnTargetSpeedChanged(float normalizedSpeed) {
            Debug.Log($"OnTargetSpeedChanged, normalized speed {normalizedSpeed}");
        }

        public override void OnLean(int dir) {
            Debug.Log($"OnLean, dir {dir}");
        }

        public override void OnJump(float force) {
            Debug.Log($"OnJump, force {force}");
        }

        public override void OnFell() {
            Debug.Log("OnFell");
        }

        public override void OnLanded(float normalizedForce) {
            Debug.Log($"OnLanded, normalized force {normalizedForce}");
        }

        public override void OnStartSlide() {
            Debug.Log("OnStartSlide");
        }

        public override void OnStopSlide() {
            Debug.Log("OnStopSlide");
        }

        public override void OnCrouch() {
            Debug.Log("OnCrouch");
        }

        public override void OnStand() {
            Debug.Log("OnStand");
        }

        public override void OnInhale(float period, float amplitude) {
            Debug.Log($"OnInhale, period {period}, amplitude {amplitude}");
        }

        public override void OnExhale(float period, float amplitude) {
            Debug.Log($"OnExhale, period {period}, amplitude {amplitude}");
        }

        public override void OnStopBreath() {
            Debug.Log("OnStopBreath");
        }

        public override void OnStep(float length) {
            Debug.Log($"OnStep, length {length}");
        }

        public override void OnColliderHit(bool isGround, ControllerColliderHit hit) {
            //Debug.Log($"OnColliderHit, is ground {isGround}");
        }

        public override void OnEnergyChanged(float energy) {
            Debug.Log($"OnEnergyChanged, energy {energy}");
        }

        public override void OnSetCrouchHeight(float height) {
            //Debug.Log($"OnSetCrouchHeight, height {height}");
        }

        public override void OnSetStandHeight(float height) {
            //Debug.Log($"OnSetStandHeight, height {height}");
        }

        public override void OnSetVolume(float volume) {
            //Debug.Log($"OnSetVolume, volume {volume}");
        }

        public override void OnGroundMaterialChanged(string groundTag) {
            Debug.Log($"OnGroundMaterialChanged, ground tag {groundTag}");
        }

        public override void UpdateVelocity(Vector3 velocity) {
            //Debug.Log($"UpdateVelocity, velocity {velocity}");
        }

        public override void UpdateNormalizedHorizontalMagnitude(float normalizedMagnitude) {
            //Debug.Log($"UpdateNormalizedHorizontalMagnitude, normalized magnitude {normalizedMagnitude}");
        }
        
    }
    
}