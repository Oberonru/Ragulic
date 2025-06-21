using System;
using System.StateMachineSystem;
using Core.CombatSystem;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Core.Enemies.States
{
    public class EnemyMeleeAttackState : StateInstance<EnemyInstance>
    {
        public IHitBox HitBox;
        private CompositeDisposable _disposable;
        private TimeSpan _spanTimeAttack => TimeSpan.FromSeconds(Owner.EnemyStats.AttackPerSeconds);

        public override void Enter()
        {
            Owner.NavMesh.Stop();
            MeleeAttack().Forget();
        }

        public override void Exit()
        {
        }

        private async UniTask MeleeAttack()
        {
            try
            {
                while (HitBox != null)
                {
                    if (!HitBox.HealthComponent.IsAllive) break;
                    //задержка должна быть по времени, но нет UniTask.Delay почемму то
                    //И нижний код корявый, сначала устанавливается перед атакое значение рандомного урона
                    //а затем уже урон и так каждый раз
                    Owner.EnemyCombat.SetRandomDamage(Owner.EnemyCombat.Damage);
                    HitBox.HealthComponent.TakeDamage(Owner.EnemyCombat.Damage);
                }
            }
            catch(OperationCanceledException)
            {
                Owner.StateMachine.SetIdle();
            }
        }
    }
}