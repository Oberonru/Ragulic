using UnityEngine;

namespace Unity.Cinemachine.Samples
{
    /// <summary>
    /// This is a behaviour whose job it is to drive animation based on the player's motion.
    /// It is a sample implementation that you can modify or replace with your own.  As shipped, it is
    /// hardcoded to work specifically with the sample `CameronSimpleController` Animation controller, which
    /// is set up with states that the SimplePlayerAnimator knows about.  You can modify
    /// this class to work with your own animation controller.
    ///
    /// SimplePlayerAnimator works with or without a SimplePlayerControllerBase alongside.
    /// Without one, it monitors the transform's position and drives the animation accordingly.
    /// You can see it used like this in some of the sample scenes, such as RunningRace or ClearShot.
    /// In this mode, is it unable to detect the player's grounded state, and so it always
    /// assumes that the player is grounded.
    ///
    /// When a SimplePlayerControllerBase is detected, the SimplePlayerAnimator installs callbacks
    /// and expects to be driven by the SimplePlayerControllerBase using the STartJump, EndJump,
    /// and PostUpdate callbacks.
    /// </summary>
    public class SimplePlayerAnimator : MonoBehaviour
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

        SimplePlayerControllerBase m_Controller;
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
            m_Controller = GetComponentInParent<SimplePlayerControllerBase>();
            if (m_Controller != null)
            {
                // Установите наши обратные вызовы для обработки прыжков и анимации в зависимости от скорости
                m_Controller.StartJump += () => m_AnimationParams.JumpTriggered = true;
                m_Controller.EndJump += () => m_AnimationParams.LandTriggered = true;
                
                //На вход из плейер контроллера передаётся текущая скорость игрока и по ней обновляется
                //состояние анимации
                m_Controller.PostUpdate += (vel, jumpAnimationScale) => UpdateAnimationState(vel, jumpAnimationScale);
            }
        }

        /// <summary>
        /// Функция LateUpdate используется для того, чтобы не беспокоиться о порядке выполнения скрипта:
        /// можно предположить, что проигрыватель уже был перемещен.
        /// </summary>
        protected virtual void LateUpdate()
        {
            // В режиме без контроллера мы отслеживаем движение игрока и создаем соответствующую анимацию.
            // Прыжки в этом режиме не поддерживаются.
            if (m_Controller == null || !m_Controller.enabled)
            {
                // Получите скорость в локальных координатах игрока
                var pos = transform.position;
                //Инверсионный кватернион, те обратный отменяет действие исходного вращения
                var vel = Quaternion.Inverse(transform.rotation) * (pos - m_PreviousPosition) / Time.deltaTime;
                m_PreviousPosition = pos;
                UpdateAnimationState(vel, 1);
            }
        }

        /// <summary>
        /// Обновите анимацию в зависимости от скорости игрока.
        /// Переопределите это для надлежащего взаимодействия с вашим контроллером анимации.
        /// </summary>
        /// <param name="vel">Player's velocity, in player-local coordinates.</param>
        /// <param name="jumpAnimationScale">Scale factor to apply to the jump animation.
        /// Его можно использовать для замедления анимации прыжка при выполнении более длинных прыжков.</param>
        void UpdateAnimationState(Vector3 vel, float jumpAnimationScale)
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
            m_AnimationParams.JumpScale = JumpAnimationScale * jumpAnimationScale;

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