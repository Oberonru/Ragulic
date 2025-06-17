using UnityEngine;

namespace Core.Enemies.Components
{
    public interface IEnemyState
    {
        void SetIdle();
        void SetMeleeMoveToTarget(Transform target);
    }
}