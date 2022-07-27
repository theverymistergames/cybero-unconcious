using UnityEngine;

namespace MV_FPS_Controller.Scripts.Inputs {
    
    /// <summary>
    ///     Implement this class to receive inputs from <see cref="PlayerInput"/>.
    /// </summary>
    public abstract class InputReceiver : MonoBehaviour {

        /// <summary>
        ///     Called when the mouse changes its position.
        /// </summary>
        /// <param name="delta">Mouse position delta (X - horizontal axis, Y - vertical axis).</param>
        public virtual void OnLook(Vector2 delta) { }
        
        /// <summary>
        ///     Called when motion input changes.
        /// </summary>
        /// <param name="dir">
        ///     Normalized motion direction: X - right (positive) and left (negative),
        ///     Y - forward (positive) and backward (negative).
        /// </param>
        public virtual void OnMove(Vector2 dir) { }

        /// <summary>
        ///     Called when lean input changes.
        /// </summary>
        /// <param name="dir">
        ///     Integer which takes -1, 0 or 1 values. Value 1 - lean to right,
        ///     value -1 - lean to left, value 0 - no lean.
        /// </param>
        public virtual void OnLean(int dir) { }

        /// <summary>
        ///     Called on jump input.
        /// </summary>
        public virtual void OnJump() { }
        
        public virtual void OnHud() { }
        
        /// <summary>
        ///     Called when crouch input changes in non-toggle mode.
        /// </summary>
        /// <param name="isActive">If crouch input is active.</param>
        public virtual void OnCrouch(bool isActive) { }

        /// <summary>
        ///     Called when crouch input is activated in toggle mode.
        /// </summary>
        public virtual void OnToggleCrouch() { }
        
        /// <summary>
        ///     Called when run input changes in non-toggle mode.
        /// </summary>
        /// <param name="isActive">If run input is active.</param>
        public virtual void OnRun(bool isActive) { }
        
        /// <summary>
        ///     Called when run input is activated in toggle mode.
        /// </summary>
        public virtual void OnToggleRun() { }
        
    }
    
}