using UnityEngine;

namespace Core.Enemies.Components
{
    public interface IEnemyState
    {
        void SetIdle();
        void SetMeleeMoveToTarget(Transform target);
        //должен быть тот кто реализует интерфейс  IHitBox
        void SetMeleeAttack(Transform target);
    }
}