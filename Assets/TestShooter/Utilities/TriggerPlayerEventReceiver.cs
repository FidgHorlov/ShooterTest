using System;
using UnityEngine;

namespace TestShooter.Utilities
{
    [RequireComponent(typeof(Collider))]
    public class TriggerPlayerEventReceiver : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        public event Action<GameObject> TriggerEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                TriggerEnter?.Invoke(other.gameObject);
            }
        }
    }
}