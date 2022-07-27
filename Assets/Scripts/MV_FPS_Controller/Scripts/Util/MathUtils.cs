using UnityEngine;

namespace MV_FPS_Controller.Scripts.Util {
    
    public static class MathUtils {

        public static bool IsNotZero(this Vector2 vector2) {
            return vector2.sqrMagnitude > 0f;
        }
        
        public static bool IsNotZero(this Vector3 vector3) {
            return vector3.sqrMagnitude > 0f;
        }

        public static Vector2 SignedAngles(this Vector2 from) {
            return new Vector2(SignedAngle(from.x), SignedAngle(from.y));
        }
        
        public static Vector3 SignedAngles(this Vector3 from) {
            return new Vector3(SignedAngle(from.x), SignedAngle(from.y), SignedAngle(from.z));
        }
        
        public static float SignedAngle(float from) {
            var result = Mathf.Repeat(from, 360f);
            if (result > 180f) result -= 360f;
            return result;
        }

        public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Vector3 angles) {
            return point.RotateAround(pivot, Quaternion.Euler(angles));
        }
        
        public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Quaternion angles) {
            return angles * (point - pivot) + pivot;
        }
        
        public static int NextRandomIndex(int exceptIndex, int size) {
            switch (size) {
                case 0:
                    return -1;
                case 1:
                    return 0;
            }

            var nextIndex = NextRandomIndex(size);
            if (nextIndex != exceptIndex) return nextIndex;
            
            return nextIndex > 0
                ? nextIndex - 1
                : nextIndex + 1;
        }

        public static int NextRandomIndex(int size) {
            if (size == 0) return -1;
            return Random.Range(0, size);
        }

    }

}