using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/ScriptableStates/ScriptableIdle", fileName = "ScriptableIdle")]
    public class ScriptableIdle : ScriptableState<EnemyInstance>
    {
        public override StateInstance<EnemyInstance> GetInstance()
        {
            return new EnemyStateIdle();
        }
    }
}