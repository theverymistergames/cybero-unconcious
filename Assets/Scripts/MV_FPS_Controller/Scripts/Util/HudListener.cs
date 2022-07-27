using System;
using UnityEngine;

namespace MV_FPS_Controller.Scripts.Util
{
    public class HudListener : MonoBehaviour
    {

        [SerializeField]
        private GameObject hudObject;

        private void Awake()
        {
            hudObject.SetActive(false);
        }

        public void SetIsHudActive(bool isActive)
        {
            hudObject.SetActive(isActive);
        }
        
    }
}