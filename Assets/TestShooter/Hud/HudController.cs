using TestShooter.Player;
using TMPro;
using UnityEngine;

namespace TestShooter.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _movementState;

        public void SetState(Enums.MovementState movementState)
        {
            _movementState.text = movementState.ToString();
            _movementState.color = Enums.MovementStatesDictionary[movementState];
        }
    }
}