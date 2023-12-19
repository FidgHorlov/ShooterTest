using TestShooter.Player;
using UnityEngine;

namespace TestShooter.UpgradeSystem
{
    public class Flying : MonoBehaviour
    {
        [SerializeField] private Rigidbody _bubbleRigidbody;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _flyingSpeed;
        [SerializeField] private float _mouseSensitivity;

        private PlayerMovement _playerMovement;
        private PlayerController _player;
        private Transform _previousPlayerParent;
        private Transform _currentTransform;
        private Vector3 _defaultPosition;

        private Rigidbody _playerRigidbody;
        private Collider _playerCollider;

        private RigidbodyConstraints _previousPlayerConstraints;

        private float RotationAxis => Input.GetAxis("Mouse X");

        private void Awake()
        {
            _currentTransform = transform;
            _playerMovement = new PlayerMovement();
            _defaultPosition = _currentTransform.position;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);

            if (!isActive)
            {
                _player.transform.SetParent(_previousPlayerParent);
                _player.IsMovementDisabled = false;
                _playerRigidbody.useGravity = true;
                _playerCollider.isTrigger = false;
                _playerRigidbody.constraints = _previousPlayerConstraints;
                _currentTransform.position = _defaultPosition;
            }
        }

        public void Init(PlayerController playerController)
        {
            _player = playerController;
            _previousPlayerParent = _player.transform.parent;
            _player.transform.SetParent(_currentTransform);
            _player.transform.localPosition = Vector3.zero;
            _player.transform.localEulerAngles = Vector3.zero;
            _player.IsMovementDisabled = true;

            if (_playerRigidbody == null)
            {
                _playerRigidbody = playerController.GetComponent<Rigidbody>();

                if (_playerRigidbody == null)
                {
                    Debug.LogError($"Can't find a {typeof(Rigidbody)} on the {playerController.name}");
                    return;
                }
            }

            if (_playerCollider == null)
            {
                _playerCollider = playerController.GetComponent<Collider>();

                if (_playerCollider == null)
                {
                    Debug.LogError($"Can't find {typeof(Collider)} on the {playerController.name}");
                }
            }

            _previousPlayerConstraints = _playerRigidbody.constraints;
            _playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
            _playerCollider.isTrigger = true;
        }

        private void LateUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            _bubbleRigidbody.velocity = _playerMovement.GetVelocity(_currentTransform.right * horizontal, _currentTransform.forward * vertical, _movementSpeed);

            if (Input.GetKey(KeyCode.Space))
            {
                _bubbleRigidbody.AddForce(_currentTransform.up * _flyingSpeed, ForceMode.VelocityChange);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _bubbleRigidbody.velocity = Vector3.zero;
            }

            float mouseX = RotationAxis * _mouseSensitivity * Time.deltaTime;
            _currentTransform.Rotate(_playerMovement.GetRotation(mouseX));
        }
    }
}