using TestShooter.Enemies;
using UnityEngine;

namespace TestShooter.Weapon
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Bullet : MonoBehaviour
    {
        private const string EnemyTag = "Enemy";
        private const float Speed = 15f;

        [SerializeField] private Rigidbody _currentRigidbody;

        private Transform _currentTransform;
        private Transform _currentParent;
        
        private GameObject _currentGameObject;
        private Vector3 _defaultPosition;
        private bool _isBulletShot;

        public float Damage { get; set; }

        private GameObject CurrentGameObject => _currentGameObject ??= gameObject;
        private Transform CurrentTransform => _currentTransform ??= transform;

        private void Awake()
        {
            _defaultPosition = CurrentTransform.localPosition;
            _currentParent = CurrentTransform.parent;
        }

        private void OnDisable()
        {
            BulletReset();
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.CompareTag(EnemyTag))
            {
                EnemyDamage(otherCollider.gameObject);
            }

            _isBulletShot = false;
            BulletReset();
        }

        private void OnCollisionEnter(Collision otherCollision)
        {
            _isBulletShot = false;
            BulletReset();
        }

        public void SetActive(bool isActive)
        {
            CurrentGameObject.SetActive(isActive);
        }

        public void Shoot(Vector3 direction)
        {
            _isBulletShot = true;
            SetActive(true);
            _currentRigidbody.velocity = direction * Speed;
            CurrentTransform.parent = null;
            Invoke(nameof(KillBullet), 5f);
        }

        public void BulletReset()
        {
            if (_isBulletShot)
            {
                return;
            }
            
            KillBullet();
        }

        private void KillBullet()
        {
            _currentRigidbody.velocity = Vector3.zero;
            _currentRigidbody.angularVelocity = Vector3.zero;
            CurrentTransform.parent = _currentParent;
            CurrentTransform.localPosition = _defaultPosition;
            CurrentGameObject.SetActive(false);
        }
        
        private void EnemyDamage(GameObject enemyGameObject)
        {
            Enemy enemy =  enemyGameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(Damage);
            }
        }
    }
}