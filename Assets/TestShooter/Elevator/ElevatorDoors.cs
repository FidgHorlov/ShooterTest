using DG.Tweening;
using UnityEngine;

namespace TestShooter.Elevator
{
    public class ElevatorDoors : MonoBehaviour
    {
        [SerializeField] private Transform _rightDoor;
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private bool _isClosedDefaultState;

        private Vector3 _leftDoorClosed;
        private Vector3 _rightDoorClosed;
        private Vector3 _leftDoorOpened;
        private Vector3 _rightDoorOpened;

        public float DoorMovementTime { private get; set; }

        private void Awake()
        {
            _leftDoorOpened =  _leftDoor.localEulerAngles;
            _rightDoorOpened = _rightDoor.localEulerAngles;
            _leftDoorClosed = Vector3.zero;
            _rightDoorClosed = Vector3.zero;

            if (!_isClosedDefaultState)
            {
                return;
            }

            _leftDoor.localEulerAngles = _leftDoorClosed;
            _rightDoor.localEulerAngles = _rightDoorClosed;
        }
        
        [ContextMenu("Open door")]
        internal void OpenDoor()
        {
            DOTween.Kill(_leftDoor);
            DOTween.Kill(_rightDoor);
            _leftDoor.DOLocalRotateQuaternion(Quaternion.Euler(_leftDoorOpened), DoorMovementTime).SetId(_leftDoor);
            _rightDoor.DOLocalRotateQuaternion(Quaternion.Euler(_rightDoorOpened), DoorMovementTime).SetId(_rightDoor);
        }

        [ContextMenu("Close door")]
        internal void CloseDoor()
        {
            DOTween.Kill(_leftDoor);
            DOTween.Kill(_rightDoor);
            _leftDoor.DOLocalRotateQuaternion(Quaternion.Euler(_leftDoorClosed), DoorMovementTime);
            _rightDoor.DOLocalRotateQuaternion(Quaternion.Euler(_rightDoorClosed), DoorMovementTime);
        }
    }
}