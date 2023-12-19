using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TestShooter.Hud
{
    public class BonusHud : MonoBehaviour
    {
        private const float ShowBonusHeight = -125f;
        private const float ShowBonusAnimationDuration = 1.5f;
        private const float HideBonusAnimationDuration = 0.5f;

        [SerializeField] private GameObject _bonusGameObject;
        [SerializeField] private TextMeshProUGUI _bonusText;

        private Transform _bonusTransform;
        private Vector3 _bonusDefaultPosition;
        private float _targetBonusHeight;

        private Transform BonusTransform => _bonusTransform ??= _bonusGameObject.transform;

        private void Awake()
        {
            _bonusDefaultPosition = BonusTransform.localPosition;
            _targetBonusHeight = _bonusDefaultPosition.y + ShowBonusHeight;
        }

        public void SetBonusName(string bonusName)
        {
            _bonusText.text = bonusName;
        }

        public void SetActiveImmediately(bool isActive)
        {
            _bonusGameObject.SetActive(isActive);
            Vector3 targetPosition = _bonusDefaultPosition;
            if (isActive)
            {
                targetPosition.y = _targetBonusHeight;
            }

            BonusTransform.localPosition = targetPosition;
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                _bonusGameObject.SetActive(true);
                BonusTransform.localPosition = _bonusDefaultPosition;
            }

            float targetPositionY = isActive ? _targetBonusHeight : _bonusDefaultPosition.y;
            BonusTransform.DOLocalMoveY(targetPositionY, isActive ? ShowBonusAnimationDuration : HideBonusAnimationDuration)
                .OnComplete(() =>
                {
                    if (!isActive)
                    {
                        _bonusGameObject.SetActive(false);
                    }
                });
        }
    }
}