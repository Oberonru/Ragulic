using Core.CombatSystem;
using UniRx;
using UnityEngine;

namespace Core.Player.CombatSystem
{
    public class PlayerCombatComponent : MonoBehaviour
    {
        [SerializeField] private HitBoxDetector _detector;

        private void OnEnable()
        {
            _detector.OnDetected.Subscribe((hitBox) =>
            {
                hitBox.HealthComponent.TakeDamage(5);
                Debug.Log("Combat player system");
            });
        }
    }
}