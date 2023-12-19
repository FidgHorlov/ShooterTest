using TestShooter.Player;
using TMPro;
using UnityEngine;

namespace TestShooter.Hud
{
    public class HudController : MonoBehaviour
    {
        private const float TimeBeforeHideBonus = 4f;
        private const float TimeBeforeHideAlert = 4f;
        
        [SerializeField] private TextMeshProUGUI _movementState;
        [SerializeField] private BonusHud _bonusHud;
        [SerializeField] private GameObject _alertQuit;

        private void Awake()
        {
            _bonusHud.SetActiveImmediately(false);
        }

        public void SetMovementState(Enums.MovementState movementState)
        {
            _movementState.text = movementState.ToString();
            _movementState.color = Enums.MovementStatesDictionary[movementState];
        }

        public void ShowBonus(string bonusName)
        {
            _bonusHud.SetBonusName(bonusName);
            _bonusHud.SetActive(true);
            Invoke(nameof(HideBonus), TimeBeforeHideBonus);
        }

        public void ShowQuitAlert()
        {
            _alertQuit.SetActive(true);
            Invoke(nameof(HideQuitAlert), TimeBeforeHideAlert);
        }
        
        public void HideQuitAlert()
        {
            _alertQuit.SetActive(false);
        }

        private void HideBonus()
        {
            _bonusHud.SetActive(false);
        }

#if UNITY_EDITOR
        [ContextMenu("Show Bonus")]
        private void ShowBonusUi()
        {
            ShowBonus("Temp bonus");
        }
        
        [ContextMenu("Hide bonus")]
        private void HideBonusUi()
        {
            ShowBonus("Temp bonus");
        }
#endif
    }
}