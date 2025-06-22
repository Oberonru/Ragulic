using System;
using System.StateMachineSystem;
using System.Threading;
using Core.CombatSystem;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Core.Enemies.States
{
    public class EnemyMeleeAttackState : StateInstance<EnemyInstance>
    {
        public IHitBox HitBox;
        private CompositeDisposable _disposable;
        private CancellationTokenSource _tokenSource;
        private TimeSpan _spanTimeAttack => TimeSpan.FromSeconds(Owner.EnemyStats.AttackPerSeconds);

        public override void Enter()
        {
            Owner.NavMesh.Stop();
            _disposable = new CompositeDisposable();
            _tokenSource = new CancellationTokenSource();
            MeleeAttack().Forget();
        }

        public override void Exit()
        {
            _tokenSource?.Cancel();
            _disposable?.Clear();
        }

        private async UniTask MeleeAttack()
        {
            try
            {
                while (HitBox != null)
                {
                    if (!HitBox.HealthComponent.IsAllive) break;
                    //И нижний код корявый, сначала устанавливается перед атакое значение рандомного урона
                    //а затем уже урон и так каждый раз
                    Owner.EnemyCombat.SetRandomDamage(Owner.EnemyCombat.Damage);
                    HitBox.HealthComponent.TakeDamage(Owner.EnemyCombat.Damage);
                    await UniTask.Delay(_spanTimeAttack, cancellationToken: _tokenSource.Token);
                }

                Owner.StateMachine.SetIdle();
            }
            catch (OperationCanceledException)
            {
                Owner.StateMachine.SetIdle();
            }
        }
    }
}