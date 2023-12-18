using UnityEngine;

namespace TestShooter.Player
{
    public class PlayerMovement
    {
        public Vector3 JumpPolishingVelocity(float currentFall) => Vector3.up * Physics.gravity.y * currentFall * Time.deltaTime;

        public Vector3 GetVelocity(Vector3 rightVector, Vector3 forwardVector, float currentSpeed)
        {
            Vector3 direction = rightVector + forwardVector;
            direction.Normalize();
            return direction * currentSpeed * Time.deltaTime;
        }

        public Vector3 GetRotation(float mouseX)
        {
            return Vector3.up * mouseX;
        }
    }
}