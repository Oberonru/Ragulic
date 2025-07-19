using UnityEngine;
using UniRx;

namespace Core.Player.Components
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerController _controller;

        protected struct AnimationParams
        {
            public bool IsWalking;
            public bool IsRunning;
            public Vector3 Direction;
            public float MotionScale;
        }

        private AnimationParams _params;

        protected virtual void Start()
        {
            _controller = GetComponentInParent<PlayerController>();
            _controller.EndUpdate.Subscribe((tuple) => UpdateAnimationState(tuple)).AddTo(this);
        }

        void UpdateAnimationState(Vector3 vel)
        {
            vel.y = 0;
            var speed = vel.magnitude;

            bool isRunning = speed > _controller.Config.NormalWalkSpeed * 2 + (_params.IsRunning ? -0.15f : 0.15f);
            bool isWalking = !isRunning &&
                             speed > _controller.Config.IdleThreshold + (_params.IsWalking ? -0.05f : 0.05f);

            _params.IsWalking = isWalking;
            _params.IsRunning = isRunning;

            _params.Direction = speed > _controller.Config.IdleThreshold ? vel / speed : Vector3.zero;
            _params.MotionScale = isWalking ? speed / _controller.Config.NormalWalkSpeed : 1;

            if (isRunning)
            {
                var motionScale = speed / _controller.Config.NormalRunSpeed;
                var constraintScale = Mathf.Min(_controller.Config.MaxRunScale,
                    1 + (speed - _controller.Config.NormalRunSpeed) / (3 * _controller.Config.NormalRunSpeed));

                _params.MotionScale = (speed < _controller.Config.NormalRunSpeed)
                    ? motionScale
                    : constraintScale;
            }

            UpdateAnimation(_params);
        }

        protected virtual void UpdateAnimation(AnimationParams animationParams)
        {
            if (!TryGetComponent(out Animator animator))
            {
                Debug.LogError("PlayerAnimator: Animator component is required");
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