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
            Owner.Controller.Speed = Owner.Stats.WalkSpeed;
        }

        private async UniTask Panic(float panicTime)
        {
            Owner.Controller.Speed = Speed;
            Owner.Controller.IsPanic = true;

            await UniTask.Delay(TimeSpan.FromSeconds(panicTime));

            Owner.Controller.IsPanic = false;
            if (Owner.Controller.IsCrouch) Owner.Controller.IsCrouch = false;
        }
    }
}