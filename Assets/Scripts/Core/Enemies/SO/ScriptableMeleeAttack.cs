using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/ScriptableStates/MeleeAttack", fileName = "MeleeAttack")]
    public class ScriptableMeleeAttack : ScriptableState<EnemyInstance>
    {
        public override StateInstance<EnemyInstance> GetInstance()
        {
            return new EnemyMeleeAttackState();
        }
    }
}