using System;
using System.Collections;
using TestShooter.Hud;
using TestShooter.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestShooter.PowerUp
{
    public class BonusController : MonoBehaviour
    {
        [Serializable]
        public class BonusMap
        {
            [field: SerializeField] public Vector2 MinAxis { get; private set; }
            [field: SerializeField] public Vector2 MaxAxis { get; private set; }
        }
        
        private const float BonusAppearingTime = 2f;

        [SerializeField] private Bonus _bonus;
        [SerializeField] private BonusMap _bonusMap;
        
        private PlayerController _playerController;
        private HudController _hud;

        private void Awake()
        {
            _bonus.SetActiveImmediately(false);
        }

        private void OnEnable()
        {
            _bonus.DetectedBonusEvent += BonusDetectedEventHandler;
        }

        private void OnDisable()
        {
            _bonus.DetectedBonusEvent -= BonusDetectedEventHandler;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(GetBonusAppearingSeconds());
            ShowBonus();
        }

        public void Init(PlayerController playerController, HudController hudController)
        {
            _playerController = playerController;
            _hud = hudController;
        }

        private void ShowBonus()
        {
            _bonus.TargetLocation = GetTargetLocation();
            _bonus.SetActive(true);
        }

        private void BonusDetectedEventHandler(Bonus.BonusType bonusType, float bonusTime)
        {
            _hud.ShowBonus(bonusType.ToString());
            switch (bonusType)
            {
                case Bonus.BonusType.MovementBoost:
                    _playerController.BoostMovement(bonusTime);
                    break;
                case Bonus.BonusType.DamageBoost:
                    _playerController.BoostDamage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        private float GetBonusAppearingSeconds() => BonusAppearingTime;
        
        private Vector3 GetTargetLocation()
        {
            float x = Random.Range(_bonusMap.MinAxis.x, _bonusMap.MaxAxis.x);
            float z = Random.Range(_bonusMap.MinAxis.x, _bonusMap.MaxAxis.y);
            return new Vector3(x, 0f, z);
        }

#if UNITY_EDITOR
        [ContextMenu("Show Bonus")]
        private void BonusAppear()
        {
            _bonus.SetActive(true);
        }

        [ContextMenu("Hide Bonus")]
        private void BonusHide()
        {
            _bonus.SetActive(false);
        }

        [ContextMenu("Detect bonus")]
        private void DetectBonus()
        {
            _bonus.BonusDetectedHide();
        }

#endif
    }
}