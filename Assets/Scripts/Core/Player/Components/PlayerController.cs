using System;
using System.Collections.Generic;
using Core.Camera;
using Core.Configs.Player;
using Core.Player.StateMachine.States;
using Sirenix.OdinInspector;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public abstract class SimplePlayerControllerBase : MonoBehaviour, IInputAxisOwner
    {
        public InputAxis MoveX = InputAxis.DefaultMomentary;
        public InputAxis MoveZ = InputAxis.DefaultMomentary;
        public InputAxis Sprint = InputAxis.DefaultMomentary;
        public InputAxis Crouch = InputAxis.DefaultMomentary;

        void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
        {
            axes.Add(new()
                { DrivenAxis = () => ref MoveX, Name = "Move X", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
            axes.Add(new()
                { DrivenAxis = () => ref MoveZ, Name = "Move Z", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
            axes.Add(new() { DrivenAxis = () => ref Sprint, Name = "Sprint" });
            axes.Add(new() { DrivenAxis = () => ref Crouch, Name = "Crouch" });
        }

        public virtual void SetStrafeMode(bool b)
        {
        }

        public abstract bool IsMoving { get; }
    }

    public class PlayerController : SimplePlayerControllerBase
    {
        [Inject] private IGameCamera _gameCamera;
        [Inject] private PlayerControllerConfig _config;

        [SerializeField] private PlayerInstance _player;

        public IObservable<Unit> StartUpdate => _startUpdate;
        private Subject<Unit> _startUpdate = new();
        public IObservable<Vector3> EndUpdate => _endUpdate;
        private Subject<Vector3> _endUpdate = new();
        [ShowInInspector] public float Speed { get; set; }
        public bool IsCrouch { get; set; }
        public bool IsPanic { get; set; }
        private bool _isStrafeMoving;

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

        public ForwardModes InputForward = ForwardModes.Camera;
        public UpModes UpMode = UpModes.World;
        public UnityEngine.Camera CameraOverride;

        public LayerMask GroundLayers = 1;

        private float _gravity = 10;

        private Vector3 currentVelocityXZ;
        Vector3 m_LastInput;
        float _currentVelocityY;
        bool m_IsSprinting;
        private CharacterController _characterController;

        bool m_InTopHemisphere = true;
        float m_TimeInHemisphere = 100;
        Vector3 m_LastRawInput;
        Quaternion m_Upsidedown = Quaternion.AngleAxis(180, Vector3.left);

        public override void SetStrafeMode(bool b) => _isStrafeMoving = b;
        public override bool IsMoving => m_LastInput.sqrMagnitude > 0.01f;

        public bool IsSprinting => m_IsSprinting;
        public UnityEngine.Camera Camera => CameraOverride == null ? UnityEngine.Camera.main : CameraOverride;

        public bool IsGrounded() => GetDistanceFromGround(transform.position, UpDirection, 10) < 0.01f;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();

            var trackingTarget = GetComponentInChildren<TrackingTarget>();
            if (trackingTarget is null) throw new NullReferenceException("Tracking target is not found");

            _gameCamera.SetTarget(trackingTarget.transform);

            _player.StateMachine.SetWalkSpeed(_player.Stats.WalkSpeed);
        }

        private void OnEnable()
        {
            _currentVelocityY = 0;
        }

        void Update()
        {
            _startUpdate.OnNext(Unit.Default);
            BackToGround();
            SetSpeed();
            var inputFrame = SetVelocity();
            Moving();
            Rotation(inputFrame);
            EndUpdatingSubscribe();
        }

        private void OnValidate()
        {
            _player = GetComponent<PlayerInstance>();
        }

        private void SetSpeed()
        {
            if (!IsPanic)
            {
                if (_player.StateMachine.GetActiveState() != typeof(PlayerRunState)
                    && Sprint.Value > 0.1f)
                {
                    _player.StateMachine.SetRunnSpeed(_player.Stats.RunSpeed);
                }

                else if ((_player.StateMachine.GetActiveState() != typeof(PlayerWalkState) &&
                          (Mathf.Abs(MoveX.Value) > 0.1 || Mathf.Abs(MoveZ.Value) > 0.1f)
                          && !IsCrouch))
                {
                    _player.StateMachine.SetWalkSpeed(_player.Stats.WalkSpeed);
                }

                if (_player.StateMachine.GetActiveState() != typeof(PlayerCrouchState)
                    && (Mathf.Abs(Crouch.Value) > 0.1f))
                {
                    _player.StateMachine.SetCrouchSpeed(_player.Stats.CrouchSpeed);
                }
            }
        }


        private Quaternion SetVelocity()
        {
            var rawInput = new Vector3(MoveX.Value, 0, MoveZ.Value);
            var inputFrame = GetInputFrame(Vector3.Dot(rawInput, m_LastRawInput) < 0.8f);
            m_LastRawInput = rawInput;

            m_LastInput = inputFrame * rawInput;
            if (m_LastInput.sqrMagnitude > 1)
                m_LastInput.Normalize();

            m_IsSprinting = Sprint.Value > 0.5f;
            var desiredVelocity = m_LastInput * (m_IsSprinting ? _player.Stats.RunSpeed : Speed);
            var damping = _config.Damping;
            if (Vector3.Angle(currentVelocityXZ, desiredVelocity) < 100)
                currentVelocityXZ = Vector3.Slerp(
                    currentVelocityXZ, desiredVelocity,
                    Damper.Damp(1, damping, Time.deltaTime));
            else
                currentVelocityXZ += Damper.Damp(
                    desiredVelocity - currentVelocityXZ, damping, Time.deltaTime);
            return inputFrame;
        }

        private void EndUpdatingSubscribe()
        {
            var vel = Quaternion.Inverse(transform.rotation) * currentVelocityXZ;
            vel.y = _currentVelocityY;
            _endUpdate.OnNext(vel);
        }

        private void BackToGround()
        {
            if (!IsGrounded())
            {
                _currentVelocityY -= _gravity * Time.deltaTime;
            }
        }

        private void Rotation(Quaternion inputFrame)
        {
            if (!_isStrafeMoving && currentVelocityXZ.sqrMagnitude > 0.001f)
            {
                var fwd = inputFrame * Vector3.forward;
                var qA = transform.rotation;
                var qB = Quaternion.LookRotation(
                    (InputForward == ForwardModes.Player && Vector3.Dot(fwd, currentVelocityXZ) < 0)
                        ? -currentVelocityXZ
                        : currentVelocityXZ, UpDirection);
                var damping = _config.Damping;
                transform.rotation = Quaternion.Slerp(qA, qB, Damper.Damp(1, damping, Time.deltaTime));
            }
        }

        private Vector3 UpDirection => UpMode == UpModes.World ? Vector3.up : transform.up;

        Quaternion GetInputFrame(bool inputDirectionChanged)
        {
            var frame = Quaternion.identity;
            switch (InputForward)
            {
                case ForwardModes.Camera: frame = Camera.transform.rotation; break;
                case ForwardModes.Player: return transform.rotation;
                case ForwardModes.World: break;
            }

            var playerUp = transform.up;
            var up = frame * Vector3.up;

            const float BlendTime = 2f;
            m_TimeInHemisphere += Time.deltaTime;
            bool inTopHemisphere = Vector3.Dot(up, playerUp) >= 0;
            if (inTopHemisphere != m_InTopHemisphere)
            {
                m_InTopHemisphere = inTopHemisphere;
                m_TimeInHemisphere = Mathf.Max(0, BlendTime - m_TimeInHemisphere);
            }

            var axis = Vector3.Cross(up, playerUp);
            if (axis.sqrMagnitude < 0.001f && inTopHemisphere)
                return frame;

            var angle = UnityVectorExtensions.SignedAngle(up, playerUp, axis);
            var frameA = Quaternion.AngleAxis(angle, axis) * frame;

            Quaternion frameB = frameA;
            if (!inTopHemisphere || m_TimeInHemisphere < BlendTime)
            {
                frameB = frame * m_Upsidedown;
                var axisB = Vector3.Cross(frameB * Vector3.up, playerUp);
                if (axisB.sqrMagnitude > 0.001f)
                    frameB = Quaternion.AngleAxis(180f - angle, axisB) * frameB;
            }

            if (inputDirectionChanged)
                m_TimeInHemisphere = BlendTime;

            if (m_TimeInHemisphere >= BlendTime)
                return inTopHemisphere ? frameA : frameB;

            if (inTopHemisphere)
                return Quaternion.Slerp(frameB, frameA, m_TimeInHemisphere / BlendTime);
            return Quaternion.Slerp(frameA, frameB, m_TimeInHemisphere / BlendTime);
        }

        private void Moving()
        {
            if (_characterController != null)
                _characterController.Move((_currentVelocityY * UpDirection + currentVelocityXZ) * Time.deltaTime);
        }

        float GetDistanceFromGround(Vector3 pos, Vector3 up, float max)
        {
            float kExtraHeight =
                _characterController == null ? 2 : 0;
            if (Physics.Raycast(pos + up * kExtraHeight, -up, out var hit,
                    max + kExtraHeight, GroundLayers, QueryTriggerInteraction.Ignore))
                return hit.distance - kExtraHeight;
            return max + 1;
        }
    }
}