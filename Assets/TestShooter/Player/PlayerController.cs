using System;
using TestShooter.Hud;
using TestShooter.Weapon;
using UnityEngine;

namespace TestShooter.Player
{
    [Serializable]
    public class PlayerSettings
    {
        public float Speed = 500f;
        public float JumpForce = 1000f;
        public float MouseSensitivity = 100f;
        public float FallingForce = 20f;
    }

    public class PlayerController : MonoBehaviour
    {
        private const float MaxLandDistance = 1.7f;
        private const float MovementThreshold = 0.1f;
        private const float BonusIncreaseValue = 1.5f;
        private const float BonusTime = 3f;

        [SerializeField] private WeaponController _weaponController;
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private PlayerSettings _playerSettings;

        private HudController _hud;
        private PlayerMovement _playerMovement;
        private Transform _playerTransform;
        private bool _isJumped;
        private float _currentSpeed;
        private float _currentJumpForce;
        private float _currentJumpFall;

        private float RotationAxis => Input.GetAxis("Mouse X");

        private void Awake()
        {
            _playerTransform = transform;
            _currentSpeed = _playerSettings.Speed;
            _currentJumpForce = _playerSettings.JumpForce;
            _currentJumpFall = _playerSettings.FallingForce;
            _playerMovement = new PlayerMovement();
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            _playerRigidbody.velocity = _playerMovement.GetVelocity(_playerTransform.right * horizontal, _playerTransform.forward * vertical, _currentSpeed);
            if (Mathf.Abs(horizontal) != 0f || Mathf.Abs(vertical) != 0f)
            {
                _hud.SetMovementState(Enums.MovementState.Move);
            }
            
            if (_playerRigidbody.velocity.y >= 0f)
            {
                _playerRigidbody.velocity += _playerMovement.JumpPolishingVelocity(_currentJumpFall);
            }

            if (IsLanded() && _isJumped)
            {
                _playerRigidbody.AddForce(Vector3.up * _currentJumpForce, ForceMode.Impulse);
                _isJumped = false;
            } 
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJumped = true;
            }
            
            float mouseX = RotationAxis * _playerSettings.MouseSensitivity * Time.deltaTime;
            _playerTransform.Rotate(_playerMovement.GetRotation(mouseX));
        }

        private void LateUpdate()
        {
            HudStateWriting();
            WeaponHandler();
        }
        
        public void Init(HudController hudController)
        {
            _hud = hudController;
        }

        public void BoostDamage()
        {
            
        }

        public void BoostMovement()
        {
            _currentJumpForce *= BonusIncreaseValue;
            _currentSpeed *= BonusIncreaseValue;
            _currentJumpFall *= BonusIncreaseValue;
            Invoke(nameof(RestoreDefaultMovement), BonusTime);
        }

        private void RestoreDefaultMovement()
        {
            _currentJumpForce /= BonusIncreaseValue;
            _currentSpeed /= BonusIncreaseValue;
            _currentJumpFall /= BonusIncreaseValue;
        }
        
        private bool IsLanded() => Physics.Raycast(_playerTransform.position, Vector3.down, MaxLandDistance);

        private bool IsValueInLimit(float value) => Mathf.Abs(value) > MovementThreshold;

        private void HudStateWriting()
        {
            Vector3 velocity = _playerRigidbody.velocity;
            Enums.MovementState currentState = Enums.MovementState.Idle;

            if (IsValueInLimit(velocity.y))
            {
                currentState = Enums.MovementState.Jump;
            }
            else if (RotationAxis != 0f)
            {
                currentState = Enums.MovementState.Rotate;
            }
            else if (IsValueInLimit(velocity.x) || IsValueInLimit(velocity.z))
            {
                currentState = Enums.MovementState.Move;
            }

            _hud.SetMovementState(currentState);
        }
        
        private void WeaponHandler()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _weaponController.FastWeaponShoot();
            }
            
            if (Input.GetKey(KeyCode.Mouse1))
            {
                _weaponController.HardWeaponShoot();
            }
        }
    }
}