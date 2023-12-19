using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace TestShooter.Elevator
{
    public class ElevatorController : MonoBehaviour
    {
        private const float DoorMovementTime = 1f;
        private const float ElevateTime = 4f;
        private const float BottomY = 0f;
        private const float TopPoseY = 8.03f;

        [SerializeField] private ElevatorDoors _frontDoor;
        [SerializeField] private ElevatorDoors _rearDoor;
        [SerializeField] private ElevatorPlatform _elevatorPlatform;
        [SerializeField] private Transform _elevator;

        private bool _isElevatorOnTop;

        private void Awake()
        {
            _frontDoor.DoorMovementTime = DoorMovementTime;
            _rearDoor.DoorMovementTime = DoorMovementTime;
        }

        private void OnEnable()
        {
            _elevatorPlatform.TriggerEvent += PlayerOnPlatform;
        }

        private void OnDisable()
        {
            _elevatorPlatform.TriggerEvent -= PlayerOnPlatform;
        }

        [ContextMenu("Elevate")]
        private void PlayerOnPlatform()
        {
            StopCoroutine(nameof(ElevatorDown));
            StopCoroutine(nameof(ElevatorUp));
            StartCoroutine(_isElevatorOnTop ? nameof(ElevatorDown) : nameof(ElevatorUp));
        }

        private IEnumerator ElevatorUp()
        {
            _frontDoor.CloseDoor();
            yield return new WaitForSeconds(DoorMovementTime);
            LocalMoveY(TopPoseY, () => _rearDoor.OpenDoor());
        }

        private IEnumerator ElevatorDown()
        {
            _rearDoor.CloseDoor();
            yield return new WaitForSeconds(DoorMovementTime);
            LocalMoveY(BottomY, () => _frontDoor.OpenDoor());
        }

        private void LocalMoveY(float poseY, Action callback)
        {
            Debug.Log($"Move elevator to the -> {poseY}");
            _elevator.DOLocalMoveY(poseY, ElevateTime).OnComplete(() =>
            {
                Debug.Log($"End of movement");
                _isElevatorOnTop = !_isElevatorOnTop;
                callback?.Invoke();
            });
        }
    }
}