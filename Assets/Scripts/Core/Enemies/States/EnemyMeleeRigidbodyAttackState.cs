using System.Interfaces;
using System.StateMachineSystem;
using Core.CombatSystem;
using Core.Player.CombatSystem;
using UniRx;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeRigidbodyAttackState : StateInstance<EnemyInstance>
    {
        public IPlayerHitBox HitBox;
        public Collision Collision;
        
        private CompositeDisposable _disposable;
        
        public override void Enter()
        {
            Owner.NavMesh.Stop();
            Debug.Log("RigidbodyAttackState Collision " + Collision);
            _disposable = new CompositeDisposable();
            
            RigidbodyAttack();
        }

        public override void Exit()
        {
            _disposable?.Clear();
        }

        private void RigidbodyAttack()
        {
            var pushDirection = Collision.relativeVelocity.normalized;
            
            Debug.DrawRay(Collision.transform.position, Collision.relativeVelocity, Color.red);

            HitBox.Rigidbody.AddForce(pushDirection * 100, ForceMode.Impulse);
        }
    }
}