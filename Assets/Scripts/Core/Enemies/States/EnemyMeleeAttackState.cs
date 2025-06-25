using System.StateMachineSystem;
using Core.Player.CombatSystem;
using UniRx;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeAttackState : StateInstance<EnemyInstance>
    {
        public IPlayerHitBox HitBox;
        public Collision Collision;
        
        private CompositeDisposable _disposable;
        
        public override void Enter()
        {
            Owner.NavMesh.Stop();
            _disposable = new CompositeDisposable();
            
            MeleeAttack();
        }

        public override void Exit()
        {
            _disposable?.Clear();
        }

        private void MeleeAttack()
        {
            var pushDirection = Collision.relativeVelocity.normalized;
            
            HitBox.Rigidbody.AddForce(pushDirection * Owner.EnemyStats.PushForce, ForceMode.Impulse);
        }
    }
}