using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestShooter.PowerUp
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
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
        private const float AnimationDuration = 5f;

        private const float DefaultBonusPoseY = 1.5f;
        private const float BonusPoseYMin = 1f;

        private const string PlayerTag = "Player";

        private readonly Vector3 DetectedScaleBonus = new Vector3(0.5f, 0.5f, 0.5f);
        private readonly Vector3 RotationAnimation = new Vector3(0f, 180f, 0f);

        public event Action<BonusType, float> DetectedBonusEvent;

        [SerializeField] private Transform _powerUpTransform;

        private BonusType _bonusType;
        private Vector3 _targetLocation;
        private bool _isActive;

        public Vector3 TargetLocation { private get; set; }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!otherCollider.CompareTag(PlayerTag))
            {
                return;
            }

            BonusDetectedHide();
            DetectedBonusEvent?.Invoke(_bonusType, BonusTime);
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
            float targetPoseY = isActive ? DefaultBonusPoseY : BonusPoseYMin;

            DOTween.Kill(_powerUpTransform);

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_powerUpTransform.DOMoveY(targetPoseY, animationDuration))
                .Append(_powerUpTransform.DOScale(GetTargetScale(isActive), animationDuration))
                .OnComplete(() =>
                {
                    if (!isActive)
                    {
                        _powerUpTransform.gameObject.SetActive(false);
                    }
                    else
                    {
                        AnimateCylinder();
                    }

                    _isActive = isActive;
                })
                .SetId(_powerUpTransform);
            sequence.Play();
        }

        public void BonusDetectedHide()
        {
            DOTween.Kill(_powerUpTransform);
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_powerUpTransform.DOMoveY(2f, DetectedBonusMoveTime / 4f))
                .Append(_powerUpTransform.DOScale(DetectedScaleBonus, DetectedBonusScaleTime))
                .OnComplete(() => SetActiveImmediately(false))
                .SetId(_powerUpTransform);
            sequence.Play();
        }

        private void AnimateCylinder()
        {
            DOTween.Kill(_powerUpTransform);
            _powerUpTransform.DOLocalRotate(RotationAnimation, AnimationDuration).SetLoops(-1).SetEase(Ease.Linear).SetId(_powerUpTransform);
        }

        private Vector3 GetTargetScale(bool isActive) => isActive ? Vector3.one : Vector3.zero;
        private BonusType GetBonusType() => Random.Range(0, 2) % 2 == 0f ? BonusType.MovementBoost : BonusType.DamageBoost;
    }
}