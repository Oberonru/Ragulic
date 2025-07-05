using System;
using System.StateMachineSystem;
using Cysharp.Threading.Tasks;

namespace Core.Player.StateMachine.States
{
    public class PlayerPanicState : StateInstance<PlayerInstance>
    {
        public float Speed { get; set; }
        
        public override void Enter()
        {
            Panic(Speed);
        }

        public override void Exit()
        {
            Owner.Movement.Speed = Owner.Stats.WalkSpeed;
        }
        
        private async UniTask Panic(float panicTime)
        {
            Owner.Movement.Speed = Speed;
            Owner.Movement.IsPanic = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(panicTime));
            
            Owner.Movement.IsPanic = false;
            if (Owner.Movement.IsCrouch) Owner.Movement.IsCrouch = false;
        }
    }
}