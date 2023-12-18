using System.Linq;
using UnityEngine;

namespace TestShooter.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponHandler[] _weaponHandlers;
        private WeaponHandler _currentWeapon;

        private void Awake()
        {
            foreach (WeaponHandler weaponHandler in _weaponHandlers)
            {
                weaponHandler.SetActive(false);
            }
        }

        public void FastWeaponShoot()
        {
            if (!IsCurrentWeaponEqualTarget(WeaponType.Fast))
            {
                ShowWeapon(WeaponType.Fast);
            }

            _currentWeapon.Shoot();
        }

        public void HardWeaponShoot()
        {
            if (!IsCurrentWeaponEqualTarget(WeaponType.Hard))
            {
                ShowWeapon(WeaponType.Hard);
            }

            _currentWeapon.Shoot();
        }
        
        public void IncreaseDamage()
        {
            _currentWeapon.IncreaseDamage();
        }

        private void ShowWeapon(WeaponType weaponType)
        {
            _currentWeapon?.SetActive(false);
            WeaponHandler weaponHandler = GetWeaponHandler(weaponType);
            _currentWeapon = weaponHandler;
            weaponHandler.SetActive(true);
        }

        private bool IsCurrentWeaponEqualTarget(WeaponType weaponType)
        {
            if (_currentWeapon == null)
            {
                return false;
            }

            return _currentWeapon.WeaponType == weaponType;
        }

        private WeaponHandler GetWeaponHandler(WeaponType weaponType) => _weaponHandlers.First(weaponHandler => weaponHandler.WeaponType.Equals(weaponType));
    }
}