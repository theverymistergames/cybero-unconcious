namespace MV_FPS_Controller.Scripts.Support {
    
    public interface ISubscription<in T> {

        void Subscribe(T subscriber);
        
        void Unsubscribe(T subscriber);
        
    }
    
}