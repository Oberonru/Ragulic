using Core.Configs.Player;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Inject] private PlayerControllerConfig _config;
        [Inject] private PlayerConfig _playerConfig;
        [SerializeField] private PlayerController _controller;
        [SerializeField] private Animator _animator;

        protected struct AnimationParams
        {
            public bool IsWalking;
            public bool IsRunning;
            public bool IsPanic;
            public Vector3 Direction;
            public float MotionScale;
        }

        private AnimationParams _animationParams;
        
        public enum States
        {
            Idle,
            Walk,
            Run,
        }

        public States State
        {
            get
            {
                if (_animationParams.IsRunning)
                    return States.Run;
                return _animationParams.IsWalking ? States.Walk : States.Idle;
            }
        }

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            
            _controller = GetComponentInParent<PlayerController>();
            if (_controller != null)
            {
                _controller.EndUpdate.Subscribe(UpdateAnimationState).AddTo(this);
            }
        }

        private void UpdateAnimationState(Vector3 velocity)
        {
            velocity.y = 0;
            var speed = velocity.magnitude;

            bool isRunning = speed > _playerConfig.WalkSpeed + (_animationParams.IsRunning ? -0.15f : 0.15f);
            bool isWalking =
                !isRunning && speed > _config.IdleThreshold + (_animationParams.IsWalking ? -0.05f : 0.05f);
            bool isPanic = speed > _playerConfig.RunSpeed + (_animationParams.IsPanic ? -0.15f : 0.15f);
            
            _animationParams.IsWalking = isWalking;
            _animationParams.IsRunning = isRunning;
            _animationParams.IsPanic = isPanic;

            _animationParams.Direction = speed > _config.IdleThreshold ? velocity / speed : Vector3.zero;
            _animationParams.MotionScale = isWalking ? speed / _config.NormalWalkSpeed : 1;

            if (isRunning)
                _animationParams.MotionScale = (speed < _config.NormalRunSpeed)
                    ? speed / _config.NormalRunSpeed
                    : Mathf.Min(_config.MaxRunScale,
                        1 + (speed - _config.NormalRunSpeed) / (3 * _config.NormalRunSpeed));

            UpdateAnimation(_animationParams);
        }

        protected virtual void UpdateAnimation(AnimationParams animationParams)
        {
            if (_animator is null)
            {
                Debug.LogError("SimplePlayerAnimator: An Animator component is required");
                return;
            }

            _animator.SetFloat("DirX", animationParams.Direction.x);
            _animator.SetFloat("DirZ", animationParams.Direction.z);
            _animator.SetFloat("MotionScale", animationParams.MotionScale);
            
            _animator.SetBool("Walking", animationParams.IsWalking);
            _animator.SetBool("Running", animationParams.IsRunning);
            _animator.SetBool("Panic", animationParams.IsPanic);
        }
    }
}