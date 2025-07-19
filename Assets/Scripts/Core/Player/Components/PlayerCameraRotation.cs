using System;
using System.Collections.Generic;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;

namespace Core.Player.Components
{
    public class PlayerCameraRotation : MonoBehaviour, IInputAxisOwner
    {
        public enum CouplingMode
        {
            Coupled,
            CoupledWhenMoving,
            Decoupled
        }

        public CouplingMode PlayerRotation;

        public float RotationDamping = 0.2f;

        public InputAxis HorizontalLook = new()
            { Range = new Vector2(-180, 180), Wrap = true, Recentering = InputAxis.RecenteringSettings.Default };

        public InputAxis VerticalLook = new()
            { Range = new Vector2(-70, 70), Recentering = InputAxis.RecenteringSettings.Default };

        private PlayerController _controller;
        private Transform _controllerTransform;
        private Quaternion _desiredWorldRotation;

        private IDisposable _endUpdateSubscription;

        void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
        {
            axes.Add(new()
            {
                DrivenAxis = () => ref HorizontalLook,
                Name = "Horizontal Look",
                Hint = IInputAxisOwner.AxisDescriptor.Hints.X
            });
            axes.Add(new()
            {
                DrivenAxis = () => ref VerticalLook,
                Name = "Vertical Look",
                Hint = IInputAxisOwner.AxisDescriptor.Hints.Y
            });
        }

        private void OnEnable()
        {
            _controller = GetComponentInParent<PlayerController>();

            if (_controller == null)
            {
                enabled = false;
                return;
            }

            _controller.StartUpdate.Subscribe(_ => UpdatePlayerRotation()).AddTo(this);

            _endUpdateSubscription?.Dispose();
            _endUpdateSubscription = _controller.EndUpdate.Subscribe(PostUpdate).AddTo(this);

            _controllerTransform = _controller.transform;
        }

        private void OnDisable()
        {
            _endUpdateSubscription?.Dispose();
            _controllerTransform = null;
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

        private void UpdatePlayerRotation()
        {
            var t = transform;
            t.localRotation = Quaternion.Euler(VerticalLook.Value, HorizontalLook.Value, 0);
            _desiredWorldRotation = t.rotation;

            switch (PlayerRotation)
            {
                case CouplingMode.Coupled:
                    _controller.SetStrafeMode(true);
                    RecenterPlayer();
                    break;

                case CouplingMode.CoupledWhenMoving:
                    _controller.SetStrafeMode(true);
                    if (_controller.IsMoving())
                        RecenterPlayer(RotationDamping);
                    break;

                case CouplingMode.Decoupled:
                    _controller.SetStrafeMode(false);
                    break;
            }

            VerticalLook.UpdateRecentering(Time.deltaTime, VerticalLook.TrackValueChange());
            HorizontalLook.UpdateRecentering(Time.deltaTime, HorizontalLook.TrackValueChange());
        }

        private void PostUpdate(Vector3 velocity)
        {
            if (PlayerRotation == CouplingMode.Decoupled && _controllerTransform != null)
            {
                // Камера вращается как нужно — по входу мыши, игрок не поворачивается.
                transform.rotation = _desiredWorldRotation;

                // Обновляем углы внутри InputAxis для корректного поведения UI/логики.
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