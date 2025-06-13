using System.StateMachineSystem;

namespace Core.Enemies.EnemyStateMachine
{
    public class EnemyStateIdle : StateInstance<EnemyInstance>
    {
        public override void Enter()
        {
            Owner.EnemyNavMesh.Stop();
        }

        public override void Exit()
        {
        }
        
        
        
    }
}