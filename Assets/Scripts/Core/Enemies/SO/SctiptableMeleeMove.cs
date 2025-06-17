using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/ScriptableStates/ScriptableMeleeMove", fileName = "ScriptableMeleeMove")]

    public class ScriptableMeleeMove : ScriptableState<EnemyInstance>
    {
        public override StateInstance<EnemyInstance> GetInstance()
        {
            return new EnemyMeleeMoveState();
        }
    }
}