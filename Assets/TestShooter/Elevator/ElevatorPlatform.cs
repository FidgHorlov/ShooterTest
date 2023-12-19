using System;
using UnityEngine;

namespace TestShooter.Elevator
{
    [RequireComponent(typeof(Collider))]
    public class ElevatorPlatform : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        public event Action TriggerEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                TriggerEvent?.Invoke();
            }
        }
    }
}