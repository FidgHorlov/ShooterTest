using UnityEngine;

namespace TestShooter.Enemies
{
    [CreateAssetMenu(menuName = "Create enemy Data", fileName = "EnemyData", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public float DefaultHealth { get; private set; }
        [field: SerializeField] public Material NormalHealthEnemy { get; private set; }
        [field: SerializeField] public Material HalfDeadHealthEnemy { get; private set; }
        [field: SerializeField] public Material DeadHealthEnemy { get; private set; }
    }
}