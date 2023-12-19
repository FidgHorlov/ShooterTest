using System;
using UnityEngine;

namespace TestShooter.Weapon
{
    [Serializable]
    public class WeaponHandler
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public GameObject BackPack { get; private set; }
        [field: SerializeField] public Weapon Weapon { get; private set; }

        public void SetActive(bool isActive)
        {
            Weapon.SetActive(isActive);
            BackPack.SetActive(!isActive);
        }

        public void Shoot()
        {
            Weapon.Shooting();
        }

        public void IncreaseDamage()
        {
            Weapon.IncreaseDamage();
        }
        
    }
}