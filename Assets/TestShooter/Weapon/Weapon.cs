using System;
using TestShooter.Player;
using UnityEngine;

namespace TestShooter.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponSettings _weaponSettings;
        [SerializeField] private Bullet[] _bulletPool;

        private GameObject _currentGameObject;
        private DateTime _timeFromLastShot;
        private int _bulletCount;

        private WeaponType _weaponType;
        private float _bulletDamage;
        private float _weaponSpeed;

        private void Awake()
        {
            InitWeaponSettings();
            _currentGameObject = gameObject;

            foreach (Bullet bullet in _bulletPool)
            {
                bullet.SetActive(false);
            }
        }

        public void SetActive(bool isActive)
        {
            _currentGameObject.SetActive(isActive);
        }

        public void StopShooting()
        {
            _bulletCount = 0;
            foreach (Bullet bullet in _bulletPool)
            {
                bullet.BulletReset();
            }
        }

        public void Shooting()
        {
            if (IsShootPossible())
            {
                Shoot();
            }
        }

        private void InitWeaponSettings()
        {
            _bulletDamage = _weaponSettings.BulletDamage;
            _weaponSpeed = _weaponSettings.WeaponSpeed;

            foreach (Bullet bullet in _bulletPool)
            {
                bullet.Damage = _bulletDamage;
            }
        }

        private void Shoot()
        {
            if (_bulletCount == _bulletPool.Length)
            {
                StopShooting();
            }

            _bulletPool[_bulletCount].Shoot();
            _bulletCount++;
            _timeFromLastShot = DateTime.Now;
        }

        private bool IsShootPossible() => (DateTime.Now - _timeFromLastShot).TotalSeconds > _weaponSpeed;

#if UNITY_EDITOR
        [ContextMenu("Find all bullets")]
        private void FindAllBullets()
        {
            _bulletPool = GetComponentsInChildren<Bullet>();
        }
#endif
    }
}