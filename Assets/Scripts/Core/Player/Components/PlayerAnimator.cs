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

        [Tooltip("Масштабный коэффициент для общей скорости анимации прыжка")]
        public float JumpAnimationScale = 0.65f;

        [SerializeField] private PlayerController _controller;
        Vector3 m_PreviousPosition; // used if m_Controller == null or disabled

        protected struct AnimationParams
        {
            public bool IsWalking;
            public bool IsRunning;
            public bool IsJumping;
            public bool LandTriggered;
            public bool JumpTriggered;
            public Vector3 Direction; // normalized direction of motion
            public float MotionScale; // scale factor for the animation speed
            public float JumpScale; // scale factor for the jump animation
        }

        AnimationParams m_AnimationParams;

        const float k_IdleThreshold = 0.2f;

        public enum States
        {
            Idle,
            Walk,
            Run,
            Jump,
            RunJump
        }

        /// <summary>Текущее состояние игрока</summary>
        public States State
        {
            get
            {
                if (m_AnimationParams.IsJumping)
                    return m_AnimationParams.IsRunning ? States.RunJump : States.Jump;
                if (m_AnimationParams.IsRunning)
                    return States.Run;
                return m_AnimationParams.IsWalking ? States.Walk : States.Idle;
            }
        }

        protected virtual void Start()
        {
            m_PreviousPosition = transform.position;
            _controller = GetComponentInParent<PlayerController>();

            //_controller.PostUpdate += (vel, jumpAnimationScale) => UpdateAnimationState(vel, jumpAnimationScale);
            _controller.EndUpdate.Subscribe((tuple) => UpdateAnimationState(tuple)).AddTo(this);
        }

        void UpdateAnimationState(Vector3 vel)
        {
            vel.y = 0; //мы не рассматриваем вертикальное перемещение
            var speed = vel.magnitude;

            // Уменьшение гистерезиса
            bool isRunning = speed > NormalWalkSpeed * 2 + (m_AnimationParams.IsRunning ? -0.15f : 0.15f);
            bool isWalking = !isRunning && speed > k_IdleThreshold + (m_AnimationParams.IsWalking ? -0.05f : 0.05f);
            m_AnimationParams.IsWalking = isWalking;
            m_AnimationParams.IsRunning = isRunning;

            // Установите нормализованное направление движения и масштабируйте скорость анимации в соответствии со скоростью движения
            m_AnimationParams.Direction = speed > k_IdleThreshold ? vel / speed : Vector3.zero;
            m_AnimationParams.MotionScale = isWalking ? speed / NormalWalkSpeed : 1;

            // Мы масштабируем скорость анимации спринта так, чтобы она примерно соответствовала реальной скорости, но мы изменяем
            // на самом высоком уровне, чтобы анимация не выглядела нелепо
            if (isRunning)
                m_AnimationParams.MotionScale = (speed < NormalSprintSpeed)
                    ? speed / NormalSprintSpeed
                    : Mathf.Min(MaxSprintScale, 1 + (speed - NormalSprintSpeed) / (3 * NormalSprintSpeed));

            UpdateAnimation(m_AnimationParams);

            if (m_AnimationParams.JumpTriggered)
                m_AnimationParams.IsJumping = true;
            if (m_AnimationParams.LandTriggered)
                m_AnimationParams.IsJumping = false;

            m_AnimationParams.JumpTriggered = false;
            m_AnimationParams.LandTriggered = false;
        }

        /// <summary>
        /// Измените анимацию в зависимости от состояния игрока.
        /// Переопределите это для надлежащего взаимодействия с вашим контроллером анимации.
        /// </summary>
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
            animator.SetFloat("JumpScale", animationParams.JumpScale);

            if (m_AnimationParams.JumpTriggered)
                animator.SetTrigger("Jump");
            if (m_AnimationParams.LandTriggered)
                animator.SetTrigger("Land");
        }
    }
}