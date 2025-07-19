using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Player.Components
{
    /// <summary>
    /// Это базовый класс для SimplePlayerController и SimplePlayerController2D.
    /// Вы также можете использовать его в качестве базового класса для своих пользовательских контроллеров.
    /// Он предоставляет следующие сервисы:
    ///
    /// **:**
    ///
    /// - 2D оси движения (moveX и MoveZ)
    /// - Кнопка перехода
    /// - Кнопка спринта
    /// - API для режима strafe
    ///
    /// **Действия:**
    ///
    /// - Предварительное обновление - вызывается в начале `Update()`
    /// - PostUpdate - вызывается в конце `Update()`
    /// - StartJump - вызывается, когда игрок начинает прыгать
    /// - EndJump - вызывается, когда игрок перестает прыгать
    ///
    /// **События:**
    ///
    /// - Приземлился - вызывается, когда игрок приземляется на землю
    /// </summary>
    public abstract class SimplePlayerControllerBase : MonoBehaviour, Unity.Cinemachine.IInputAxisOwner
    {
        [Tooltip("Ground speed when walking")] public float Speed = 1f;

        [Tooltip("Ground speed when sprinting")]
        public float SprintSpeed = 4;

        [Tooltip("Initial vertical speed when jumping")]
        public float JumpSpeed = 4;

        [Tooltip("Initial vertical speed when sprint-jumping")]
        public float SprintJumpSpeed = 6;

        public Action PreUpdate;
        public Action<Vector3, float> PostUpdate;
        public Action StartJump;
        public Action EndJump;

        [Header("Input Axes")] [Tooltip("X Axis movement.  Value is -1..1.  Управляет боковым перемещением")]
        public InputAxis MoveX = InputAxis.DefaultMomentary;

        [Tooltip("Z Axis movement.  Value is -1..1. Controls the forward movement")]
        public InputAxis MoveZ = InputAxis.DefaultMomentary;

        [Tooltip("Jump movement.  Value is 0 or 1. Controls the vertical movement")]
        public InputAxis Jump = InputAxis.DefaultMomentary;

        [Tooltip("Sprint movement.  Value is 0 or 1. If 1, затем начинается бег")]
        public InputAxis Sprint = InputAxis.DefaultMomentary;

        [Header("Events")] [Tooltip("This event is sent when the player lands after a jump.")]
        public UnityEvent Landed = new();

        ///Сообщите о доступных входных осях контроллеру входных осей.
        /// Мы используем контроллер оси ввода, потому что он работает как с пакетом ввода, так и с другим пакетом ввода.
        /// и устаревшая система ввода. Это пример кода, и мы хотим, чтобы это работало везде.
        void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
        {
            axes.Add(new()
                { DrivenAxis = () => ref MoveX, Name = "Move X", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
            axes.Add(new()
                { DrivenAxis = () => ref MoveZ, Name = "Move Z", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
            axes.Add(new() { DrivenAxis = () => ref Jump, Name = "Jump" });
            axes.Add(new() { DrivenAxis = () => ref Sprint, Name = "Sprint" });
        }

        public virtual void SetStrafeMode(bool b)
        {
        }

        public abstract bool IsMoving { get; }
    }

    /// <summary>
    /// Это 3D-контроллер персонажей, созданный на основе SimplePlayerControllerBase.
    /// Он предоставляет следующие сервисы и настройки:
    ///
    /// - Демпфирование (применяется к скорости игрока и его вращению)
    /// - Режим "Стрейф"
    /// - Гравитация.
    /// - Входные кадры (какой опорный кадр используется для интерпретации входных данных: камера, окружающий мир или игрок)
    /// - Определение местности (с помощью радиопередач или делегирование контроллеру персонажа)
    /// - Переопределение камеры (камера используется только для определения входного кадра)
    ///
    /// Это поведение должно быть привязано к корню игрового объекта player. Оно перемещает игровой объект
    /// transform. Если у игрового объекта также есть компонент контроллера персонажа Unity, то простой игрок
    /// Контроллер делегирует ему заземленное состояние и перемещение. Если у игрового объекта нет
    /// Контроллера персонажа, простой игровой контроллер сам управляет движением и запускает лучи
    /// для проверки заземленного состояния.
    /// </summary>
    public class SimplePlayerController : SimplePlayerControllerBase
    {
        [Tooltip("Продолжительность перехода (в секундах), когда игрок меняет скорость или вращение.")]
        public float Damping = 0.5f;

        [Tooltip(
            "Заставляет игрока отклоняться в сторону при движении вбок, в противном случае он поворачивается лицом к направлению движения.")]
        public bool Strafe = false;

        public enum ForwardModes
        {
            Camera,
            Player,
            World
        };

        public enum UpModes
        {
            Player,
            World
        };

        [Tooltip("Reference frame for the input controls:\n"
                 + "<b>Camera</b>: Input forward is camera forward direction.\n"
                 + "<b>Player</b>: Input forward is Player's forward direction.\n"
                 + "<b>World</b>: Input forward is World forward direction.")]
        public ForwardModes InputForward = ForwardModes.Camera;

        [Tooltip("Up direction for computing motion:\n"
                 + "<b>Player</b>: Move in the Player's local XZ plane.\n"
                 + "<b>World</b>: Move in global XZ plane.")]
        public UpModes UpMode = UpModes.World;

        [Tooltip(
            "If non-null, take the input frame from this camera instead of Camera.main. Useful for split-screen games.")]
        public UnityEngine.Camera CameraOverride;

        [Tooltip("Layers to include in ground detection via Raycasts.")]
        public LayerMask GroundLayers = 1;

        [Tooltip("Force of gravity in the down direction (m/s^2)")]
        public float Gravity = 10;

        const float kDelayBeforeInferringJump = 0.3f;
        float m_TimeLastGrounded = 0;

        Vector3 m_CurrentVelocityXZ;
        Vector3 m_LastInput;
        float m_CurrentVelocityY;
        bool m_IsSprinting;
        bool m_IsJumping;
        UnityEngine.CharacterController m_Controller; // optional

        // Это часть стратегии борьбы с блокировкой входного шарнира при управлении плеером
        // который может свободно перемещаться по поверхностям, перевернутым относительно камеры.
        // Это используется только в конкретной ситуации, когда персонаж перевернут относительно входного кадра,
        // и входные инструкции становятся неоднозначными.
        // Если камера и входной кадр перемещаются вместе с игроком, то они не используются.
        bool m_InTopHemisphere = true;
        float m_TimeInHemisphere = 100;
        Vector3 m_LastRawInput;
        Quaternion m_Upsidedown = Quaternion.AngleAxis(180, Vector3.left);

        public override void SetStrafeMode(bool b) => Strafe = b;
        public override bool IsMoving => m_LastInput.sqrMagnitude > 0.01f;

        public bool IsSprinting => m_IsSprinting;
        public bool IsJumping => m_IsJumping;
        public UnityEngine.Camera Camera => CameraOverride == null ? UnityEngine.Camera.main : CameraOverride;

        public bool IsGrounded() => GetDistanceFromGround(transform.position, UpDirection, 10) < 0.01f;

        // Обратите внимание, что m_Controller - это необязательный компонент: мы будем использовать его, если он там есть.
        void Start() => TryGetComponent(out m_Controller);

        private void OnEnable()
        {
            m_CurrentVelocityY = 0;
            m_IsSprinting = false;
            m_IsJumping = false;
            m_TimeLastGrounded = Time.time;
        }

        void Update()
        {
            PreUpdate?.Invoke();

            // Process Jump and gravity
            bool justLanded = ProcessJump();

            // Get the reference frame for the input
            var rawInput = new Vector3(MoveX.Value, 0, MoveZ.Value);
            var inputFrame = GetInputFrame(Vector3.Dot(rawInput, m_LastRawInput) < 0.8f);
            m_LastRawInput = rawInput;

            // Считайте вводимые данные от пользователя и помещайте их во фрейм ввода
            m_LastInput = inputFrame * rawInput;
            if (m_LastInput.sqrMagnitude > 1)
                m_LastInput.Normalize();

            // Вычислите новую скорость и переместите игрока, но только если он не находится в середине прыжка
            if (!m_IsJumping)
            {
                m_IsSprinting = Sprint.Value > 0.5f;
                var desiredVelocity = m_LastInput * (m_IsSprinting ? SprintSpeed : Speed);
                var damping = justLanded ? 0 : Damping;
                if (Vector3.Angle(m_CurrentVelocityXZ, desiredVelocity) < 100)
                    m_CurrentVelocityXZ = Vector3.Slerp(
                        m_CurrentVelocityXZ, desiredVelocity,
                        Damper.Damp(1, damping, Time.deltaTime));
                else
                    m_CurrentVelocityXZ += Damper.Damp(
                        desiredVelocity - m_CurrentVelocityXZ, damping, Time.deltaTime);
            }

            // Apply the position change
            ApplyMotion();

            // Если не выполняется обстрел, поверните проигрыватель лицом к направлению движения
            if (!Strafe && m_CurrentVelocityXZ.sqrMagnitude > 0.001f)
            {
                var fwd = inputFrame * Vector3.forward;
                var qA = transform.rotation;
                var qB = Quaternion.LookRotation(
                    (InputForward == ForwardModes.Player && Vector3.Dot(fwd, m_CurrentVelocityXZ) < 0)
                        ? -m_CurrentVelocityXZ
                        : m_CurrentVelocityXZ, UpDirection);
                var damping = justLanded ? 0 : Damping;
                transform.rotation = Quaternion.Slerp(qA, qB, Damper.Damp(1, damping, Time.deltaTime));
            }

            if (PostUpdate != null)
            {
                // Get local-space velocity
                var vel = Quaternion.Inverse(transform.rotation) * m_CurrentVelocityXZ;
                vel.y = m_CurrentVelocityY;
                PostUpdate(vel, m_IsSprinting ? JumpSpeed / SprintJumpSpeed : 1);
            }
        }

        Vector3 UpDirection => UpMode == UpModes.World ? Vector3.up : transform.up;

        // Получаем исходную рамку для входных данных. Идея заключается в том, чтобы привязать камеру fwd/вправо
        // к плоскости XZ игрока. Здесь есть некоторая сложность, связанная с тем, чтобы избежать
        // блокировки кардана, когда игрок наклонен на 180 градусов относительно входной рамки.
        Quaternion GetInputFrame(bool inputDirectionChanged)
        {
            // Get the raw input frame, depending of forward mode setting
            var frame = Quaternion.identity;
            switch (InputForward)
            {
                case ForwardModes.Camera: frame = Camera.transform.rotation; break;
                case ForwardModes.Player: return transform.rotation;
                case ForwardModes.World: break;
            }

            // Map the raw input frame to something that makes sense as a direction for the player
            var playerUp = transform.up;
            var up = frame * Vector3.up;

            // Is the player in the top or bottom hemisphere?  This is needed to avoid gimbal lock,
            // but only when the player is upside-down relative to the input frame.
            const float BlendTime = 2f;
            m_TimeInHemisphere += Time.deltaTime;
            bool inTopHemisphere = Vector3.Dot(up, playerUp) >= 0;
            if (inTopHemisphere != m_InTopHemisphere)
            {
                m_InTopHemisphere = inTopHemisphere;
                m_TimeInHemisphere = Mathf.Max(0, BlendTime - m_TimeInHemisphere);
            }

            // Если игрок находится в положении "untilted" относительно входных данных,
            // то выполняется ранний выход с помощью простого поворота
            var axis = Vector3.Cross(up, playerUp);
            if (axis.sqrMagnitude < 0.001f && inTopHemisphere)
                return frame;

            // Player is tilted relative to input frame: tilt the input frame to match
            var angle = UnityVectorExtensions.SignedAngle(up, playerUp, axis);
            var frameA = Quaternion.AngleAxis(angle, axis) * frame;

            //Если проигрыватель наклонен, то нам нужно проявить хитрость, чтобы избежать блокировки кардана
            // когда проигрыватель наклонен на 180 градусов. Идеального решения для этого не существует,
            // нам нужно его обмануть :/
            Quaternion frameB = frameA;
            if (!inTopHemisphere || m_TimeInHemisphere < BlendTime)
            {
                // Вычислите альтернативную систему отсчета для нижней полусферы.
                // Две системы отсчета несовместимы там, где они пересекаются, особенно
                // когда игрок направлен вверх вдоль оси X кадра камеры.
                // Не существует единой системы отсчета, которая работала бы для всех направлений игрока.
                frameB = frame * m_Upsidedown;
                var axisB = Vector3.Cross(frameB * Vector3.up, playerUp);
                if (axisB.sqrMagnitude > 0.001f)
                    frameB = Quaternion.AngleAxis(180f - angle, axisB) * frameB;
            }

            // Blend timer force-expires when user changes input direction
            if (inputDirectionChanged)
                m_TimeInHemisphere = BlendTime;

            // If we have been long enough in one hemisphere, then we can just use its reference frame
            if (m_TimeInHemisphere >= BlendTime)
                return inTopHemisphere ? frameA : frameB;

            // Поскольку FrameA и FrameB не соединяются плавно, когда игрок находится вдоль оси X,
            // мы смешиваем их с течением времени, чтобы избежать неправильного вращения.
            // Иногда это приводит к странным движениям, но это меньшее из зол.
            if (inTopHemisphere)
                return Quaternion.Slerp(frameB, frameA, m_TimeInHemisphere / BlendTime);
            return Quaternion.Slerp(frameA, frameB, m_TimeInHemisphere / BlendTime);
        }

        bool ProcessJump()
        {
            bool justLanded = false;
            var now = Time.time;
            bool grounded = IsGrounded();

            m_CurrentVelocityY -= Gravity * Time.deltaTime;

            if (!m_IsJumping)
            {
                // Process jump command
                if (grounded && Jump.Value > 0.01f)
                {
                    m_IsJumping = true;
                    m_CurrentVelocityY = m_IsSprinting ? SprintJumpSpeed : JumpSpeed;
                }

                // If we are falling, assume the jump pose
                if (!grounded && now - m_TimeLastGrounded > kDelayBeforeInferringJump)
                    m_IsJumping = true;

                if (m_IsJumping)
                {
                    StartJump?.Invoke();
                    grounded = false;
                }
            }

            if (grounded)
            {
                m_TimeLastGrounded = Time.time;
                m_CurrentVelocityY = 0;

                // If we were jumping, complete the jump
                if (m_IsJumping)
                {
                    EndJump?.Invoke();
                    m_IsJumping = false;
                    justLanded = true;
                    Landed.Invoke();
                }
            }

            return justLanded;
        }

        void ApplyMotion()
        {
            if (m_Controller != null)
                m_Controller.Move((m_CurrentVelocityY * UpDirection + m_CurrentVelocityXZ) * Time.deltaTime);
            else
            {
                var pos = transform.position + m_CurrentVelocityXZ * Time.deltaTime;

                // Не проваливайся под землю
                var up = UpDirection;
                var altitude = GetDistanceFromGround(pos, up, 10);
                if (altitude < 0 && m_CurrentVelocityY <= 0)
                {
                    pos -= altitude * up;
                    m_CurrentVelocityY = 0;
                }
                else if (m_CurrentVelocityY < 0)
                {
                    var dy = -m_CurrentVelocityY * Time.deltaTime;
                    if (dy > altitude)
                    {
                        pos -= altitude * up;
                        m_CurrentVelocityY = 0;
                    }
                }

                transform.position = pos + m_CurrentVelocityY * up * Time.deltaTime;
            }
        }

        float GetDistanceFromGround(Vector3 pos, Vector3 up, float max)
        {
            float kExtraHeight =
                m_Controller == null ? 2 : 0; // start a little above the player in case it's moving down fast
            if (UnityEngine.Physics.Raycast(pos + up * kExtraHeight, -up, out var hit,
                    max + kExtraHeight, GroundLayers, QueryTriggerInteraction.Ignore))
                return hit.distance - kExtraHeight;
            return max + 1;
        }
    }
}