using System;
using System.StateMachineSystem;
using Core.CombatSystem;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeRigidbodyAttackState : StateInstance<EnemyInstance>
    {
        public IHitBox HitBox;
        
        public override void Enter()
        {
            Debug.Log("attack rigidbody");
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }
    }
}