using UnityEngine;

namespace MV_FPS_Controller.Scripts.Support {
    
    /// <summary>
    ///     Implement this interface to know about motion and rotation of game object
    ///     with <see cref="MovingPlatformSupport"/> component. 
    /// </summary>
    public interface IExternalMovable {

        /// <summary>
        ///     Called when collider enters external source trigger zone.
        /// </summary>
        /// <param name="source">Motion source.</param>
        void OnStartExternalMotion(ISubscription<IExternalMovable> source);
        
        /// <summary>
        ///     Called when collider exits external source trigger zone.
        /// </summary>
        /// <param name="source">Motion source.</param>
        void OnFinishExternalMotion(ISubscription<IExternalMovable> source);
        
        /// <summary>
        ///     Called each frame to provide current frame motion.
        /// </summary>
        /// <param name="source">Transform of motion source.</param>
        /// <param name="motion">Motion delta.</param>
        void ExternalMove(Transform source, Vector3 motion);
        
        /// <summary>
        ///     Called each frame to provide current frame rotation.
        /// </summary>
        /// <param name="source">Transform of motion source.</param>
        /// <param name="angles">Rotation delta in Euler angles.</param>
        void ExternalRotate(Transform source, Vector3 angles);
        
    }
    
}