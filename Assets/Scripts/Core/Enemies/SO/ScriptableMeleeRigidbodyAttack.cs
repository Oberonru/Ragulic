using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/ScriptableStates/ScriptableMeleeRigidbodyAttack", fileName = "ScriptableMeleeRigidbodyAttack")]
    public class ScriptableMeleeRigidbodyAttack : ScriptableState<EnemyInstance>
    {
        public override StateInstance<EnemyInstance> GetInstance()
        {
            return new EnemyMeleeRigidbodyAttackState();
        }
    }
}