using System;
using TestShooter.Data;
using TestShooter.Hud;
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

        [SerializeField] private HudController _hud;
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private PlayerSettings _playerSettings;

        private Transform _playerTransform;
        private float RotationAxis => Input.GetAxis("Mouse X"); 
        private bool _isJumped;

        private void Awake()
        {
            _playerTransform = transform;
        }

        private void FixedUpdate()
        {
            PlayerMovement();
            JumpPolishing();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJumped = true;
            }

            PlayerRotation();
        }

        private void LateUpdate()
        {
            HudStateWriting();
        }

        private bool IsValueInLimit(float value) => Mathf.Abs(value) > 0.1f;

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
            
            _hud.SetState(currentState);
        }

        private void JumpPolishing()
        {
            if (_playerRigidbody.velocity.y <= 0f)
            {
                _playerRigidbody.velocity += Vector3.up * Physics.gravity.y * _playerSettings.FallingForce * Time.deltaTime;
            }
        }

        private void PlayerMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = _playerTransform.right * horizontal + _playerTransform.forward * vertical;
            direction.Normalize();
            _playerRigidbody.velocity = direction * _playerSettings.Speed * Time.deltaTime;

            if (Mathf.Abs(horizontal) != 0f || Mathf.Abs(vertical) != 0f)
            {
                _hud.SetState(Enums.MovementState.Move);
            }

            if (IsLanded() && _isJumped)
            {
                _playerRigidbody.AddForce(Vector3.up * _playerSettings.JumpForce, ForceMode.Impulse);
                _isJumped = false;
            }
        }

        private void PlayerRotation()
        {
            float mouseX = RotationAxis * _playerSettings.MouseSensitivity * Time.deltaTime;
            _playerTransform.Rotate(Vector3.up * mouseX);
        }

        private bool IsLanded() => Physics.Raycast(_playerTransform.position, Vector3.down, MaxLandDistance);
    }
}