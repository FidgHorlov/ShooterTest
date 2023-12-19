using TestShooter.Player;
using TestShooter.Utilities;
using UnityEngine;

namespace TestShooter.UpgradeSystem
{
    public class UpgradeFunctionality : MonoBehaviour
    {
        private const float FlyingTime = 10f;  
            
        [SerializeField] private TriggerPlayerEventReceiver _triggerPlayerEventReceiver;
        [SerializeField] private Flying _flyingBubble;

        private PlayerController _playerController;

        private void OnEnable()
        {
            _triggerPlayerEventReceiver.TriggerEnter += InsideUpgrade;
        }
        
        private void OnDisable()
        {
            _triggerPlayerEventReceiver.TriggerEnter -= InsideUpgrade;
        }
        
        private void InsideUpgrade(GameObject playerGameObject)
        {
            if (_playerController == null)
            {
                _playerController = playerGameObject.GetComponent<PlayerController>();
                if (_playerController == null)
                {
                    Debug.LogError($"Player doesn't have {typeof(PlayerController)}");
                    return;
                }
            }
            
            _triggerPlayerEventReceiver.gameObject.SetActive(false);
            _flyingBubble.SetActive(true);
            _flyingBubble.Init(_playerController);
            OutsideUpgrade();
        }

        private void OutsideUpgrade()
        {
            Invoke(nameof(ShowUpgradeInTime), FlyingTime);
        }

        private void ShowUpgradeInTime()
        {
            _flyingBubble.SetActive(false);
            _triggerPlayerEventReceiver.gameObject.SetActive(true);
        }

        [ContextMenu("Inside of player")]
        private void InsideOfPlayer()
        {
            InsideUpgrade(FindObjectOfType<PlayerController>().gameObject);
        }
    }
}
