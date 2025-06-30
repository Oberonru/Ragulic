using System.StateMachineSystem;
using System.Threading;
using Core.CombatSystem;
using UniRx;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeAttackState : StateInstance<EnemyInstance>
    {
        public IHitBox HitBox;
        private CompositeDisposable _disposable;
        private CancellationTokenSource _tokenSource;

        public override void Enter()
        {
            Owner.NavMesh.Stop();
            _disposable = new CompositeDisposable();
            _tokenSource = new CancellationTokenSource();

            MeleeAttack();
        }

        public override void Exit()
        {
            _disposable?.Clear();
            _tokenSource?.Cancel();
        }

        private void MeleeAttack()
        {
            if (!HitBox.HealthComponent.IsAllive) return;

            var pushDirection = HitBox.Rigidbody.transform.forward;
            HitBox.Rigidbody.AddForce(pushDirection * Owner.Stats.PushForce, ForceMode.Impulse);
            HitBox.HealthComponent.TakeDamage(Owner.Stats.Damage);
        }
    }
}