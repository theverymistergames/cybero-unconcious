using System;
using MV_FPS_Controller.Scripts.Config;
using MV_FPS_Controller.Scripts.Player.Movement;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Inputs {
    
    public class SettingsInput : MonoBehaviour {

        public KeyCode pause = KeyCode.P;
        public KeyCode resume = KeyCode.R;
        
        public KeyCode enableMotionInput = KeyCode.M;
        public KeyCode disableMotionInput = KeyCode.N;
        
        public KeyCode enableLookInput = KeyCode.L;
        public KeyCode disableLookInput = KeyCode.K;
        
        public KeyCode enableHClamp90 = KeyCode.H;
        public KeyCode disableHClamp = KeyCode.G;
        
        public KeyCode enableVClamp90 = KeyCode.V;
        public KeyCode disableVClamp = KeyCode.C;

        public KeyCode moveSmooth2 = KeyCode.Alpha1;
        public KeyCode moveSmooth20 = KeyCode.Alpha2;
        
        public KeyCode lookSmooth2 = KeyCode.Alpha3;
        public KeyCode lookSmooth20 = KeyCode.Alpha4;
        
        public KeyCode setEnergy0 = KeyCode.Alpha5;
        public KeyCode setEnergy1 = KeyCode.Alpha6;
        public KeyCode addEnergy01 = KeyCode.Alpha7;
        public KeyCode subEnergy01 = KeyCode.Alpha8;

        private IPlayerSettings mPlayerSettings;
        
        
        private void Awake() {
            mPlayerSettings = GetComponent<IPlayerSettings>();
        }

        private void Update() {
            if (Input.GetKey(pause)) mPlayerSettings.Pause();
            if (Input.GetKey(resume)) mPlayerSettings.Resume();
            
            if (Input.GetKey(enableMotionInput)) mPlayerSettings.EnableMotionInput();
            if (Input.GetKey(disableMotionInput)) mPlayerSettings.DisableMotionInput();
            
            if (Input.GetKey(enableLookInput)) mPlayerSettings.EnableLookInput();
            if (Input.GetKey(disableLookInput)) mPlayerSettings.DisableLookInput();
            
            if (Input.GetKey(enableHClamp90)) mPlayerSettings.EnableLookHorizontalClamp(90f);
            if (Input.GetKey(disableHClamp)) mPlayerSettings.DisableLookHorizontalClamp();
            
            if (Input.GetKey(enableVClamp90)) mPlayerSettings.EnableLookVerticalClamp(90f);
            if (Input.GetKey(disableVClamp)) mPlayerSettings.DisableLookVerticalClamp();
            
            if (Input.GetKey(moveSmooth2)) mPlayerSettings.SetMotionSmoothAcceleration(2f);
            if (Input.GetKey(moveSmooth20)) mPlayerSettings.SetMotionSmoothAcceleration(20f);
            
            if (Input.GetKey(lookSmooth2)) mPlayerSettings.SetLookSmoothAcceleration(2f);
            if (Input.GetKey(lookSmooth20)) mPlayerSettings.SetLookSmoothAcceleration(20f);
            
            if (Input.GetKey(setEnergy0)) mPlayerSettings.SetEnergy(0f);
            if (Input.GetKey(setEnergy1)) mPlayerSettings.SetEnergy(1f);
            if (Input.GetKey(addEnergy01)) mPlayerSettings.AddEnergy(0.1f);
            if (Input.GetKey(subEnergy01)) mPlayerSettings.AddEnergy(-0.1f);
        }

    }

}