using UnityEngine;

namespace TestShooter.Weapon
{
    [CreateAssetMenu(fileName = "Weapon Settings", menuName = "WeaponSettings", order = 1)]
    public class WeaponSettings : ScriptableObject
    {
        [field: SerializeField] public float BulletDamage { get; private set; }
        [field: SerializeField] public float WeaponSpeed { get; private set; }
    }
}