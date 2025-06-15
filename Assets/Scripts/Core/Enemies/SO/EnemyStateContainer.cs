using System.StateMachineSystem.SO;
using UnityEngine;

namespace Core.Enemies.SO
{
    [CreateAssetMenu(menuName = "StateMachine/Container/EnemyStateContainer", fileName = "EnemyStateContainer")]
    public class EnemyStateContainer : StateContainer<EnemyInstance>
    {
    }
}