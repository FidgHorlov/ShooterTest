using System;
using System.Collections;
using DG.Tweening;
using TestShooter.Utilities;
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
        [SerializeField] private TriggerPlayerEventReceiver _platformEventReceiver;
        [SerializeField] private TriggerPlayerEventReceiver _callElevatorEventReceiver;
        [SerializeField] private Transform _elevator;

        private bool _isElevatorOnTop;

        private void Awake()
        {
            _frontDoor.DoorMovementTime = DoorMovementTime;
            _rearDoor.DoorMovementTime = DoorMovementTime;
            
            _callElevatorEventReceiver.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _platformEventReceiver.TriggerEnter += PlayerOnPlatform;
            _callElevatorEventReceiver.TriggerEnter += CallElevatorDown;
        }

        private void OnDisable()
        {
            _platformEventReceiver.TriggerEnter -= PlayerOnPlatform;
            _callElevatorEventReceiver.TriggerEnter -= CallElevatorDown;
        }

        [ContextMenu("Elevate")]
        private void PlayerOnPlatform(GameObject playerGameObject)
        {
            StopCoroutine(nameof(ElevatorDown));
            StopCoroutine(nameof(ElevatorUp));
            StartCoroutine(_isElevatorOnTop ? nameof(ElevatorDown) : nameof(ElevatorUp));
        }

        private void CallElevatorDown(GameObject playerGameObject)
        {
            StartCoroutine(nameof(ElevatorDown));
            _callElevatorEventReceiver.gameObject.SetActive(false);
        }

        private IEnumerator ElevatorUp()
        {
            _frontDoor.CloseDoor();
            yield return new WaitForSeconds(DoorMovementTime);
            LocalMoveY(TopPoseY, () => _rearDoor.OpenDoor());
            
            _callElevatorEventReceiver.gameObject.SetActive(true);
        }

        private IEnumerator ElevatorDown()
        {
            _rearDoor.CloseDoor();
            yield return new WaitForSeconds(DoorMovementTime);
            LocalMoveY(BottomY, () => _frontDoor.OpenDoor());
        }

        private void LocalMoveY(float poseY, Action callback)
        {
            _elevator.DOLocalMoveY(poseY, ElevateTime).OnComplete(() =>
            {
                _isElevatorOnTop = !_isElevatorOnTop;
                callback?.Invoke();
            });
        }
    }
}