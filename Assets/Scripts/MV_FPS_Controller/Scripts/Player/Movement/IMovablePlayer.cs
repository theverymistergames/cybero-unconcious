using UnityEngine;

namespace MV_FPS_Controller.Scripts.Player.Movement {
    
    public interface IMovablePlayer {

        void OnLook(Vector2 delta);
        
        void OnMove(Vector2 dir);

        void OnLean(int dir);

        void OnJump(float force);

        void OnFell();

        void OnLanded(float normalizedForce);

        void OnStartSlide();

        void OnStopSlide();

        void OnCrouch();

        void OnStand();
        
    }

}
