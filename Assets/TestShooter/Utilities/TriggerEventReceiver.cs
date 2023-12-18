using System;
using UnityEngine;

namespace TestShooter.Utilities
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEventReceiver : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        public event Action TriggerEnterEvent;

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!otherCollider.CompareTag(PlayerTag))
            {
                return;
            }

            TriggerEnterEvent?.Invoke();
        }
    }
}