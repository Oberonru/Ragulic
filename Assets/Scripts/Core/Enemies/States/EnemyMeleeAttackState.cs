using System.StateMachineSystem;
using Core.CombatSystem;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeAttackState : StateInstance<EnemyInstance>, IUpdate
    {
        public IHitBox HitBox;
        
        public override void Enter()
        {
            Debug.Log("EnemyMeleeAttackState enter");
        }

        public override void Exit()
        {
        }

        public void Update()
        {
            
        }
    }
}