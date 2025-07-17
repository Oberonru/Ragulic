using UnityEngine;
using UniRx;

namespace Core.Player.Components
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Tooltip(
            "Настройте это в соответствии с анимацией в модели: ноги не должны скользить при ходьбе с такой скоростью")]
        public float NormalWalkSpeed = 1.7f;

        [Tooltip(
            "Настройте это в соответствии с анимацией в модели: ноги не должны скользить при беге с такой скоростью")]
        public float NormalSprintSpeed = 5;

        [Tooltip(
            "\nНикогда не ускоряйте анимацию спринта больше, чем это необходимо, чтобы избежать абсурдно быстрого движения")]
        public float MaxSprintScale = 1.4f;

        [SerializeField] private PlayerController _controller;

        protected struct AnimationParams
        {
            public bool IsWalking;
            public bool IsRunning;
            public Vector3 Direction;
            public float MotionScale;
        }

        AnimationParams m_AnimationParams;

        const float k_IdleThreshold = 0.2f;

        protected virtual void Start()
        {
            _controller = GetComponentInParent<PlayerController>();
            _controller.EndUpdate.Subscribe((tuple) => UpdateAnimationState(tuple)).AddTo(this);
        }

        void UpdateAnimationState(Vector3 vel)
        {
            vel.y = 0;
            var speed = vel.magnitude;

            bool isRunning = speed > NormalWalkSpeed * 2 + (m_AnimationParams.IsRunning ? -0.15f : 0.15f);
            bool isWalking = !isRunning && speed > k_IdleThreshold + (m_AnimationParams.IsWalking ? -0.05f : 0.05f);
            m_AnimationParams.IsWalking = isWalking;
            m_AnimationParams.IsRunning = isRunning;

            m_AnimationParams.Direction = speed > k_IdleThreshold ? vel / speed : Vector3.zero;
            m_AnimationParams.MotionScale = isWalking ? speed / NormalWalkSpeed : 1;

            if (isRunning)
                m_AnimationParams.MotionScale = (speed < NormalSprintSpeed)
                    ? speed / NormalSprintSpeed
                    : Mathf.Min(MaxSprintScale, 1 + (speed - NormalSprintSpeed) / (3 * NormalSprintSpeed));

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