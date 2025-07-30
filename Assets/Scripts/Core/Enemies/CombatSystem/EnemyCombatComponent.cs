using System;
using Core.CombatSystem;
using Core.Player;
using Core.Player.CombatSystem;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Enemies.CombatSystem
{
    public class EnemyCombatComponent : CombatComponent
    {
        [Inject] private IPlayerInstance _player;
        [SerializeField, ReadOnly] private EnemyInstance _enemyInstance;
        [SerializeField] private EnemyTriggerHitBoxDetector _detector;

        private bool _isAttacking;

        private void OnEnable()
        {
            _detector.OnDetected.Subscribe(hitBox =>
            {
                if (!(hitBox is IPlayerHitBox)) return;
                if (_isAttacking) return;

                Attack(hitBox);
            });
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) _enemyInstance = GetComponent<EnemyInstance>();
            if (_detector is null) _detector = GetComponent<EnemyTriggerHitBoxDetector>();
        }

        private async UniTask Attack(IHitBox hitBox)
        {
            if (!(hitBox is IPlayerHitBox) || !hitBox.HealthComponent.IsAllive) return;
            
            _isAttacking = true;
        }
    }
}