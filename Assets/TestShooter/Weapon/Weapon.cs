﻿using System;
using UnityEngine;

namespace TestShooter.Weapon
{
    public class Weapon : MonoBehaviour
    {
        private const float DamageIncrease = 1.2f;
        
        [SerializeField] private WeaponSettings _weaponSettings;
        [SerializeField] private Bullet[] _bulletPool;

        private GameObject _currentGameObject;
        private Transform _currentTransform;
        private DateTime _timeFromLastShot;
        private int _bulletCount;

        private WeaponType _weaponType;
        private float _bulletDamage;
        private float _weaponSpeed;

        private void Awake()
        {
            InitWeaponSettings();
            _currentTransform = transform;
            _currentGameObject = gameObject;

            foreach (Bullet bullet in _bulletPool)
            {
                bullet.SetActive(false);
            }
        }

        internal void SetActive(bool isActive)
        {
            _currentGameObject.SetActive(isActive);
        }

        internal void Shooting()
        {
            if (IsShootPossible())
            {
                Shoot();
            }
        }
        
        internal void IncreaseDamage()
        {
            _bulletDamage *= DamageIncrease;
            SetBulletDamage();
        }

        private void InitWeaponSettings()
        {
            _bulletDamage = _weaponSettings.BulletDamage;
            _weaponSpeed = _weaponSettings.WeaponSpeed;
            SetBulletDamage();
        }
        
        private void SetBulletDamage()
        {
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

            _bulletPool[_bulletCount].Shoot(_currentTransform.forward);
            _bulletCount++;
            _timeFromLastShot = DateTime.Now;
        }
        
        private void StopShooting()
        {
            _bulletCount = 0;
            foreach (Bullet bullet in _bulletPool)
            {
                bullet.BulletReset();
            }
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