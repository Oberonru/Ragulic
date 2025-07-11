using Core.CombatSystem;
using UnityEngine;

namespace Core.Enemies.Components
{
    public interface IEnemyState
    {
        void SetSearchPlayer();
        void SetMeleeMoveToTarget(Transform target);
        void SetMeleeCycleAttack(IHitBox hitBox);

        void SetMeleeAttack(IHitBox hitBox);

        void SetPatrol();
    }
}