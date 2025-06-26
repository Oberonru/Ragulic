using System;
using System.StateMachineSystem;
using System.Threading;
using Core.CombatSystem;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Core.Enemies.States
{
    public class EnemyMeleeAttackState : StateInstance<EnemyInstance>
    {
        public IHitBox HitBox;
        public Collision Collision;
        
        private CompositeDisposable _disposable;
        private CancellationTokenSource _tokenSource;
        
        public override void Enter()
        {
            Owner.NavMesh.Stop();
            _disposable = new CompositeDisposable();
            _tokenSource = new CancellationTokenSource();
            
            MeleeAttack().Forget();
        }

        public override void Exit()
        {
            _disposable?.Clear();
        }

        private async UniTask MeleeAttack()
        {
            var pushDirection = HitBox.Rigidbody.transform.forward;
            HitBox.Rigidbody.AddForce(pushDirection * Owner.EnemyStats.PushForce, ForceMode.Impulse);
            HitBox.HealthComponent.TakeDamage(Owner.EnemyStats.Damage);

            await UniTask.Delay(TimeSpan.FromSeconds(Owner.EnemyStats.AttackPerSeconds), cancellationToken: _tokenSource.Token);
        }
    }
}