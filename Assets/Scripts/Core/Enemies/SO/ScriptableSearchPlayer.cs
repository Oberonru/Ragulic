using System.StateMachineSystem;
using System.StateMachineSystem.SO;
using Core.Enemies.States;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/ScriptableStates/ScriptableSearchPlayer", fileName = "Search Player")]
    public class ScriptableSearchPlayer : ScriptableState<EnemyInstance>
    {
        public override StateInstance<EnemyInstance> GetInstance()
        {
            return new EnemySearchPlayer();
        }
    }
}