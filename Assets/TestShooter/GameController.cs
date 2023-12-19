using TestShooter.Hud;
using TestShooter.Player;
using TestShooter.PowerUp;
using UnityEngine;

namespace TestShooter
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private HudController _hudController;
        [SerializeField] private BonusController _bonusController;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _playerController.Init(_hudController);
            _bonusController.Init(_playerController, _hudController);
        }
    }
}
