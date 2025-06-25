using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/ScriptableStates/CycleMeleeAttackState", fileName = "CycleMeleeAttackState")]
    public class ScriptableCycleMeleeAttack : ScriptableState<EnemyInstance>
    {
        public override StateInstance<EnemyInstance> GetInstance()
        {
            return new EnemyCycleMeleeAttackState();
        }
    }
}