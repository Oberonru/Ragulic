using Core.CombatSystem;
using Core.Player.CombatSystem;
using UnityEngine;

namespace Core.Enemies.Components
{
    public interface IEnemyState
    {
        void SetIdle();
        void SetMeleeMoveToTarget(Transform target);
        void SetMeleeAttack(IHitBox hitBox);

        void SetMeleeRigidbodyAttack(IPlayerHitBox hitBox, Collision collision);
    }
}