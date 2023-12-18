using System;
using DG.Tweening;
using TestShooter.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestShooter.PowerUp
{


    public class Bonus : MonoBehaviour
    {
        public enum BonusType
        {
            MovementBoost,
            DamageBoost
        }

        private const float ShowBonusTime = 1.5f;
        private const float HideBonusTime = 1f;
        private const float DetectedBonusMoveTime = 0.75f;
        private const float DetectedBonusScaleTime = 0.25f;
        private const float BonusTime = 4f;
        
        private readonly Vector3 DetectedScaleBonus = new Vector3(0.5f, 0.5f, 0.5f);

        public event Action<BonusType, float> DetectedBonusEvent;

        [SerializeField] private Transform _powerUpTransform;
        [SerializeField] private TriggerEventReceiver _triggerReceiver;

        private BonusType _bonusType;
        private Vector3 _targetLocation;
        private bool _isActive;
        
        public Vector3 TargetLocation { private get; set; }

        private void OnEnable()
        {
            _triggerReceiver.TriggerEnterEvent += TriggerReceiverHandler;
        }

        private void OnDisable()
        {
            _triggerReceiver.TriggerEnterEvent -= TriggerReceiverHandler;
        }

        public void SetActiveImmediately(bool isActive)
        {
            _powerUpTransform.gameObject.SetActive(isActive);
            _powerUpTransform.localScale = isActive ? Vector3.one : Vector3.zero;
            _isActive = isActive;
        }

        public void SetActive(bool isActive)
        {
            if (_isActive == isActive)
            {
                return;
            }

            if (isActive)
            {
                _powerUpTransform.gameObject.SetActive(true);
                _bonusType = GetBonusType();
                _powerUpTransform.position = TargetLocation;
            }

            float animationDuration = isActive ? ShowBonusTime : HideBonusTime;

            _powerUpTransform.DOKill(_powerUpTransform);
            _powerUpTransform.DOScale(GetTargetScale(isActive), animationDuration).OnComplete(() =>
            {
                if (!isActive)
                {
                    _powerUpTransform.gameObject.SetActive(false);
                }

                _isActive = isActive;
            }).SetId(_powerUpTransform);
        }

        private void TriggerReceiverHandler()
        {
            SetActive(false);
            DetectedBonusEvent?.Invoke(_bonusType, BonusTime);
        }

        public void BonusDetectedHide()
        {
            _powerUpTransform.DOKill(_powerUpTransform);
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_powerUpTransform.DOMoveY(GetDetectedBonusPositionY(), DetectedBonusMoveTime))
                .Append(_powerUpTransform.DOScale(DetectedScaleBonus, DetectedBonusScaleTime))
                .OnComplete(() => SetActiveImmediately(false))
                .SetId(_powerUpTransform);
            sequence.Play();
        }

        private float GetDetectedBonusPositionY()
        {
            float targetHeight = _powerUpTransform.position.y;

            if (targetHeight == 0)
            {
                targetHeight = 1f;
            }

            return targetHeight * 1.5f;
        }

        private Vector3 GetTargetScale(bool isActive) => isActive ? Vector3.one : Vector3.zero;
        private BonusType GetBonusType() => Random.Range(0, 2) % 2 == 0f ? BonusType.MovementBoost : BonusType.DamageBoost;


    }
}