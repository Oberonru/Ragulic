using System.Collections.Generic;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;

namespace Core.Player.Components
{
    public class PlayerCameraRotation : MonoBehaviour, Unity.Cinemachine.IInputAxisOwner
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

        private SimplePlayerController _controller;
        private Transform _controllerTransform;
        private Quaternion _desiredWorldRotation;

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

        private void OnValidate()
        {
            HorizontalLook.Validate();
            VerticalLook.Range.x = Mathf.Clamp(VerticalLook.Range.x, -90, 90);
            VerticalLook.Range.y = Mathf.Clamp(VerticalLook.Range.y, 12, 20);
            VerticalLook.Validate();
        }

        private void OnEnable()
        {
            _controller = GetComponentInParent<SimplePlayerController>();
            if (_controller == null)
                Debug.LogError($"PlayerController not found on parent object");
            else
            {
                _controller.StartUpdate.Subscribe(_ => UpdatePlayerRotation()).AddTo(this);
                _controller.EndUpdate.Subscribe(PostUpdate).AddTo(this);

                _controllerTransform = _controller.transform;
            }
        }

        private void OnDisable()
        {
            if (_controller != null)
            {
                _controllerTransform = null;
            }
        }

        private void RecenterPlayer(float damping = 0)
        {
            if (_controllerTransform == null)
                return;

            var rot = transform.localRotation.eulerAngles;
            rot.y = NormalizeAngle(rot.y);
            var delta = rot.y;
            delta = Damper.Damp(delta, damping, Time.deltaTime);

            _controllerTransform.rotation = Quaternion.AngleAxis(
                delta, _controllerTransform.up) * _controllerTransform.rotation;

            HorizontalLook.Value -= delta;
            rot.y -= delta;
            transform.localRotation = Quaternion.Euler(rot);
        }

        public void SetLookDirection(Vector3 worldspaceDirection)
        {
            if (_controllerTransform == null)
                return;
            var rot = (Quaternion.Inverse(_controllerTransform.rotation)
                       * Quaternion.LookRotation(worldspaceDirection, _controllerTransform.up)).eulerAngles;
            HorizontalLook.Value = HorizontalLook.ClampValue(rot.y);
            VerticalLook.Value = VerticalLook.ClampValue(NormalizeAngle(rot.x));
        }

        private void UpdatePlayerRotation()
        {
            var t = transform;
            t.localRotation = Quaternion.Euler(VerticalLook.Value, HorizontalLook.Value, 0);
            _desiredWorldRotation = t.rotation;
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
                    _controller.SetStrafeMode(true);
                    if (_controller.IsMoving)
                        RecenterPlayer(RotationDamping);
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

        private void PostUpdate(Vector3 vel)
        {
            if (PlayerRotation == CouplingMode.Decoupled)
            {
                transform.rotation = _desiredWorldRotation;
                var delta = (Quaternion.Inverse(_controllerTransform.rotation) * _desiredWorldRotation).eulerAngles;
                VerticalLook.Value = NormalizeAngle(delta.x);
                HorizontalLook.Value = NormalizeAngle(delta.y);
            }
        }

        private float NormalizeAngle(float angle)
        {
            while (angle > 180)
                angle -= 360;
            while (angle < -180)
                angle += 360;
            return angle;
        }
    }
}