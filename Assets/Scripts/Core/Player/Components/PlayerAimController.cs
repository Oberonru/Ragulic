using System.Collections.Generic;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;

namespace Core.Player.Components
{
    /// <summary>
    /// Это дополнение для SimplePlayerController, которое управляет ядром прицеливания игрока.
    ///
    /// Предполагается, что этот компонент будет находиться в дочернем объекте игрока,
    /// у которого есть SimplePlayerController
    /// behavior. Он тесно связан с этим компонентом.
    //
    /// Цель ядра прицеливания - отделить вращение камеры от вращения игрока.
    /// Вращение камеры определяется вращением игрового объекта player core, и это поведение
    /// предоставляет входные оси для управления им. Когда ядро игрока используется в качестве мишени для
    /// в кинокамере CinemachineCamera с компонентом ThirdPersonFollow камера будет смотреть вдоль оси ядра
    /// вперед и поворачиваться вокруг начала координат ядра.
    ///
    /// Ядро прицеливания также используется для определения начала и направления стрельбы игрока, если игрок
    /// обладает такой способностью.
    ///
    /// Чтобы реализовать стрельбу игрока, добавьте к этому игровому объекту поведение SimplePlayerShoot.
    /// </summary>
    public class PlayerAimController : MonoBehaviour, Unity.Cinemachine.IInputAxisOwner
    {
        public enum CouplingMode
        {
            Coupled,
            CoupledWhenMoving,
            Decoupled
        }

        [Tooltip("Как вращение игрока связано с вращением камеры. Доступны три режима:\n"
                 + "<b>Сопряженный</b>: Игрок вращается вместе с камерой. Движение в сторону приведет к обстрелу.\n"
                 + "<b>Подключается при перемещении</b>: камера может свободно вращаться вокруг игрока, когда он неподвижен " +
                 "но когда игрок начнет двигаться, он повернется лицом к камере.\n"
                 + "<b>Отключено</b>: Вращение проигрывателя не зависит от вращения камеры.")]
        public CouplingMode PlayerRotation;

        [Tooltip("Как быстро игрок поворачивается лицом к камере, когда начинает движение.  " +
                 "Используется только в том случае, если вращение игрока связано с перемещением.")]
        public float RotationDamping = 0.2f;

        [Tooltip("Horizontal Rotation.  Value is in degrees, with 0 being centered.")]
        public InputAxis HorizontalLook = new()
            { Range = new Vector2(-180, 180), Wrap = true, Recentering = InputAxis.RecenteringSettings.Default };

        [Tooltip("Vertical Rotation.  Value is in degrees, with 0 being centered.")]
        public InputAxis VerticalLook = new()
            { Range = new Vector2(-70, 70), Recentering = InputAxis.RecenteringSettings.Default };

        [SerializeField] private PlayerController _controller;
        Transform m_ControllerTransform; // cached for efficiency
        Quaternion m_DesiredWorldRotation;
        
        void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
        {
            axes.Add(new()
            {
                DrivenAxis = () => ref HorizontalLook, Name = "Horizontal Look",
                Hint = IInputAxisOwner.AxisDescriptor.Hints.X
            });
            axes.Add(new()
            {
                DrivenAxis = () => ref VerticalLook, Name = "Vertical Look",
                Hint = IInputAxisOwner.AxisDescriptor.Hints.Y
            });
        }

        void OnValidate()
        {
            HorizontalLook.Validate();
            VerticalLook.Range.x = Mathf.Clamp(VerticalLook.Range.x, -90, 90);
            VerticalLook.Range.y = Mathf.Clamp(VerticalLook.Range.y, 12, 20);
            VerticalLook.Validate();
        }

        void OnEnable()
        {
            _controller = GetComponentInParent<PlayerController>();

            _controller.StartUpdate.Subscribe(_ => UpdatePlayerRotation()).AddTo(this);
            _controller.EndUpdate.Subscribe(_ => UpdatePlayerRotation()).AddTo(this);
            
            m_ControllerTransform = _controller.transform;
        }

        void OnDisable()
        {
            if (_controller != null)
            {
                m_ControllerTransform = null;
            }
        }

        /// <summary>Перенастраивает игрока в соответствии с моей ротацией</summary>
        /// <param name="демпфирование">Сколько времени должно занять повторное центрирование</param>
        public void RecenterPlayer(float damping = 0)
        {
            if (m_ControllerTransform == null)
                return;

            // Get my rotation relative to parent
            var rot = transform.localRotation.eulerAngles;
            rot.y = NormalizeAngle(rot.y);
            var delta = rot.y;
            delta = Damper.Damp(delta, damping, Time.deltaTime);

            // Rotate the parent towards me
            m_ControllerTransform.rotation = Quaternion.AngleAxis(
                delta, m_ControllerTransform.up) * m_ControllerTransform.rotation;

            // Rotate me in the opposite direction
            HorizontalLook.Value -= delta;
            rot.y -= delta;
            transform.localRotation = Quaternion.Euler(rot);
        }

        /// <summary>
        /// Set my rotation to look in this direction, without changing player rotation.
        /// Here we only set the axis values, we let the player controller do the actual rotation.
        /// </summary>
        /// <param name="worldspaceDirection">Direction to look in, in worldspace</param>
        public void SetLookDirection(Vector3 worldspaceDirection)
        {
            if (m_ControllerTransform == null)
                return;
            var rot = (Quaternion.Inverse(m_ControllerTransform.rotation)
                       * Quaternion.LookRotation(worldspaceDirection, m_ControllerTransform.up)).eulerAngles;
            HorizontalLook.Value = HorizontalLook.ClampValue(rot.y);
            VerticalLook.Value = VerticalLook.ClampValue(NormalizeAngle(rot.x));
        }

        // This is called by the player controller before it updates its own rotation.
        void UpdatePlayerRotation()
        {
            var t = transform;
            t.localRotation = Quaternion.Euler(VerticalLook.Value, HorizontalLook.Value, 0);
            m_DesiredWorldRotation = t.rotation;
            switch (PlayerRotation)
            {
                case CouplingMode.Coupled:
                {
                    _controller.SetStrafeMode(true);
                    RecenterPlayer();
                    break;
                }
                case CouplingMode.CoupledWhenMoving:
                {
                    // Если проигрыватель движется, поверните его в соответствии с направлением камеры,
                    // В противном случае пусть камера вращается по орбите
                    _controller.SetStrafeMode(true);
                    // if (m_Controller.IsMoving)
                    //     RecenterPlayer(RotationDamping);
                    break;
                }
                case CouplingMode.Decoupled:
                {
                    _controller.SetStrafeMode(false);
                    break;
                }
            }

            VerticalLook.UpdateRecentering(Time.deltaTime, VerticalLook.TrackValueChange());
            HorizontalLook.UpdateRecentering(Time.deltaTime, HorizontalLook.TrackValueChange());
        }

        // Обратный вызов для контроллера игрока, чтобы обновить нашу ротацию после того,
        // как он обновит свою собственную.
        void PostUpdate(Vector3 vel)
        {
            if (PlayerRotation == CouplingMode.Decoupled)
            {
                // После того, как игрок был повернут, мы вычитаем любое изменение вращения
                // из нашего собственного преобразования, чтобы сохранить вращение нашего мира
                transform.rotation = m_DesiredWorldRotation;
                var delta = (Quaternion.Inverse(m_ControllerTransform.rotation) * m_DesiredWorldRotation).eulerAngles;
                VerticalLook.Value = NormalizeAngle(delta.x);
                HorizontalLook.Value = NormalizeAngle(delta.y);
            }
        }

        float NormalizeAngle(float angle)
        {
            while (angle > 180)
                angle -= 360;
            while (angle < -180)
                angle += 360;
            return angle;
        }
    }
}