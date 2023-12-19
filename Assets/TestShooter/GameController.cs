using TestShooter.Hud;
using TestShooter.Player;
using TestShooter.PowerUp;
using UnityEngine;

namespace TestShooter
{
    public class GameController : MonoBehaviour
    {
        private const float EscapeTime = 8f;

        [SerializeField] private PlayerController _playerController;
        [SerializeField] private HudController _hudController;
        [SerializeField] private BonusController _bonusController;

        private float _timer;
        private bool _isEscapePressed;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _playerController.Init(_hudController);
            _bonusController.Init(_playerController, _hudController);
            _hudController.HideQuitAlert();
        }

        private void LateUpdate()
        {
            if (_isEscapePressed)
            {
                _timer += Time.deltaTime;
                if (_timer > EscapeTime)
                {
                    Debug.Log($"Time out.");
                    _isEscapePressed = false;
                }
            }

            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }

            if (!_isEscapePressed)
            {
                _hudController.ShowQuitAlert();
                _isEscapePressed = true;
                return;
            }

            if (_timer <= EscapeTime)
            {
                Application.Quit();
            }
        }
    }
}