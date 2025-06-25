using Core.CombatSystem;
using Core.Player.CombatSystem;
using UnityEngine;

namespace Core.Enemies.Components
{
    public interface IEnemyState
    {
        void SetIdle();
        void SetMeleeMoveToTarget(Transform target);
        void SetMeleeCycleAttack(IHitBox hitBox);

        void SetMeleeAttack(IPlayerHitBox hitBox, Collision collision);
    }
}