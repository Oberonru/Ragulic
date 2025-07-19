using Core.Configs.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Inject] private PlayerControllerConfig _config;
        private SimplePlayerController _controller;

        protected struct AnimationParams
        {
            public bool IsWalking;
            public bool IsRunning;
            public Vector3 Direction;
            public float MotionScale;
        }

        private AnimationParams m_AnimationParams;
        
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
                if (m_AnimationParams.IsRunning)
                    return States.Run;
                return m_AnimationParams.IsWalking ? States.Walk : States.Idle;
            }
        }

        protected virtual void Start()
        {
            _controller = GetComponentInParent<SimplePlayerController>();
            if (_controller != null)
            {
                _controller.EndUpdate.Subscribe(UpdateAnimationState).AddTo(this);
            }
        }

        private void UpdateAnimationState(Vector3 vel)
        {
            vel.y = 0;
            var speed = vel.magnitude;

            bool isRunning = speed > _config.NormalWalkSpeed * 2 + (m_AnimationParams.IsRunning ? -0.15f : 0.15f);
            bool isWalking =
                !isRunning && speed > _config.IdleThreshold + (m_AnimationParams.IsWalking ? -0.05f : 0.05f);
            m_AnimationParams.IsWalking = isWalking;
            m_AnimationParams.IsRunning = isRunning;

            m_AnimationParams.Direction = speed > _config.IdleThreshold ? vel / speed : Vector3.zero;
            m_AnimationParams.MotionScale = isWalking ? speed / _config.NormalWalkSpeed : 1;

            if (isRunning)
                m_AnimationParams.MotionScale = (speed < _config.NormalRunSpeed)
                    ? speed / _config.NormalRunSpeed
                    : Mathf.Min(_config.MaxRunScale,
                        1 + (speed - _config.NormalRunSpeed) / (3 * _config.NormalRunSpeed));

            UpdateAnimation(m_AnimationParams);
        }

        protected virtual void UpdateAnimation(AnimationParams animationParams)
        {
            if (!TryGetComponent(out Animator animator))
            {
                Debug.LogError("SimplePlayerAnimator: An Animator component is required");
                return;
            }

            animator.SetFloat("DirX", animationParams.Direction.x);
            animator.SetFloat("DirZ", animationParams.Direction.z);
            animator.SetFloat("MotionScale", animationParams.MotionScale);
            animator.SetBool("Walking", animationParams.IsWalking);
            animator.SetBool("Running", animationParams.IsRunning);
        }
    }
}