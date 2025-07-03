using Core.Enemies;
using UnityEngine;

namespace System.UI.Health
{
    public class EnemyWidgetStatus : MonoBehaviour
    {
        [SerializeField] private EnemyInstance _enemyInstance;
        [SerializeField] private HealthBar _healthBar;

        private void Start()
        {
            _healthBar.SetData(_enemyInstance.HealthComponent);
        }
    }
}