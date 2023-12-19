using UnityEngine;

namespace TestShooter.Enemies
{
    public class EnemiesController : MonoBehaviour
    {
        [SerializeField] private Enemy[] _enemiesArray;
        [SerializeField] private EnemyData[] _enemiesData;

        public void Awake()
        {
            for (int index = 0; index < _enemiesArray.Length; index++)
            {
                int enemyType = index % 2 == 0 ? 0 : 1;
                _enemiesArray[index].SetEnemyData(_enemiesData[enemyType]);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Find all enemies")]
        private void FindAllEnemies()
        {
            _enemiesArray = GetComponentsInChildren<Enemy>();
        }
#endif
    }
}