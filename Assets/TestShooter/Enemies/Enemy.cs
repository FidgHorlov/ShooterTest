using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestShooter.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour
    {
        private const string HealthTemplate = "{0} HP";

        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private MeshRenderer[] _bodyMeshes;

        private EnemyData _enemyData;
        private float _sliderMax;
        private float _currentHealth;

        public void SetEnemyData(EnemyData enemyData)
        {
            _enemyData = enemyData;
            SetMaterialToBody(_enemyData.NormalHealthEnemy);
            _currentHealth = _enemyData.DefaultHealth;
            _healthSlider.maxValue = _currentHealth;
            ChangeHealthStatusUi(_currentHealth);
        }

        public void Damage(float damage)
        {
            _currentHealth -= damage;
            ChangeBodyColor(_currentHealth);
            if (_currentHealth <= 0f)
            {
                gameObject.SetActive(false);
                Debug.Log($"Died");
                return;
            }

            ChangeHealthStatusUi(_currentHealth);
        }

        private void ChangeBodyColor(float health)
        {
            if (health > _enemyData.DefaultHealth / 2f)
            {
                SetMaterialToBody(_enemyData.HalfDeadHealthEnemy);
            }
            else if (health < _enemyData.DefaultHealth * 0.1f)
            {
                SetMaterialToBody(_enemyData.DeadHealthEnemy);
            }
        }

        private void SetMaterialToBody(Material targetMaterial)
        {
            foreach (MeshRenderer meshRenderer in _bodyMeshes)
            {
                meshRenderer.material = targetMaterial;
            }
        }

        private void ChangeHealthStatusUi(float health)
        {
            _healthSlider.SetValueWithoutNotify(health);
            _healthText.text = string.Format(HealthTemplate, health);
        }
    }
}