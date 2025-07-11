using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/ScriptableStates/ScriptablePatrolState", fileName = "Patrol")]

    public class ScriptablePatrolState : ScriptableState<EnemyInstance>
    {
        public override StateInstance<EnemyInstance> GetInstance()
        {
            return new EnemyPatrolState();
        }
    }
}