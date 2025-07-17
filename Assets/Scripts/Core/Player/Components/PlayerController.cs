using System;
using System.Collections.Generic;
using Core.Camera;
using Core.Configs.Player;
using Core.Player.StateMachine.States;
using Sirenix.OdinInspector;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerController : MonoBehaviour, IInputAxisOwner
    {
        [Inject] private PlayerControllerConfig _config;
        [Inject] private IGameCamera _gameCamera;
        [SerializeField, ReadOnly] private PlayerInstance _player;
        [SerializeField] private UnityEngine.Camera _camera;

        private CharacterController _characterController;
        public IObservable<Unit> StartUpdate => _startUpdate;
        private Subject<Unit> _startUpdate = new();

        public IObservable<Vector3> EndUpdate => _endUpdate;
        private Subject<Vector3> _endUpdate = new();

        private InputAxis Horizontal = InputAxis.DefaultMomentary;
        private InputAxis Vertical = InputAxis.DefaultMomentary;
        private InputAxis Acceleration = InputAxis.DefaultMomentary;
        private InputAxis Crouch = InputAxis.DefaultMomentary;

        private float _gravity = 10f;
        private float _timeInHemisphere = 100;
        private Vector3 _currentVelocityXZ;
        private Vector3 _upVelocity;
        private Vector3 _lastInput;
        private Vector3 _lastRawInput;
        private Quaternion _upsidedown = Quaternion.AngleAxis(180, Vector3.left);
        private Quaternion _inputFrame;
        private bool _inTopHemisphere = true;
        private bool _isRunning;
        private bool _isStrafeMoving;
        
        [ShowInInspector] public float Speed { get; set; }
        public bool IsCrouch { get; set; }
        public bool IsPanic { get; set; }

        private enum ForwardType
        {
            Player,
            Camera,
            World
        }

        public enum UpType
        {
            Player,
            World,
        }

        public UnityEngine.Camera ProjectCamera => _camera == null ? UnityEngine.Camera.main : _camera;

        public UpType UpDirection = UpType.World;
        private Vector3 _upDirection => UpDirection == UpType.World ? Vector3.up : transform.up;

        private ForwardType InputForward = ForwardType.Camera;
        public bool IsMoving() => _lastInput.sqrMagnitude > 0.01f;
        
        private void OnEnable()
        {
            _player.Health.OnHit.Subscribe
                (_ => _player.StateMachine.SetPanicSpeed(_player.Stats.PanicSpeed)).AddTo(this);

            var trackingTarget = _player.GetComponentInChildren<PlayerAim>();

            if (trackingTarget == null) throw new NullReferenceException("Tracking target is not found");

            _gameCamera.SetTarget(trackingTarget.transform);
        }

        private void Start()
        {
            _player.StateMachine.SetWalkSpeed(_player.Stats.WalkSpeed);
            _characterController = GetComponent<CharacterController>();
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
            if (_characterController is null) _characterController = GetComponent<CharacterController>();
        }

        void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
        {
            axes.Add(new IInputAxisOwner.AxisDescriptor()
            {
                DrivenAxis = () => ref Horizontal,
                Name = "Horizontal X",
                Hint = IInputAxisOwner.AxisDescriptor.Hints.X
            });

            axes.Add(new IInputAxisOwner.AxisDescriptor()
            {
                DrivenAxis = () => ref Vertical,
                Name = "Vertical Z",
                Hint = IInputAxisOwner.AxisDescriptor.Hints.Y
            });

            axes.Add(new IInputAxisOwner.AxisDescriptor()
            {
                DrivenAxis = () => ref Acceleration,
                Name = "Acceleration",
            });

            axes.Add(new IInputAxisOwner.AxisDescriptor()
            {
                DrivenAxis = () => ref Crouch,
                Name = "Crouch",
            });
        }

        public virtual void SetStrafeMode(bool b)
        {
        }
        
        private void Update()
        {
            _startUpdate.OnNext(Unit.Default);

            BackToGround();
           

            if (!IsPanic)
            {
                if (_player.StateMachine.GetActiveState() != typeof(PlayerRunState)
                    && Acceleration.Value > 0.1f)
                {
                    _player.StateMachine.SetRunnSpeed(_player.Stats.RunSpeed);
                }

                else if ((_player.StateMachine.GetActiveState() != typeof(PlayerWalkState) &&
                          (Mathf.Abs(Horizontal.Value) > 0.1 || Mathf.Abs(Vertical.Value) > 0.1f)
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

            SetVelocity();
            Move();

            var velocity = Quaternion.Inverse(transform.rotation) * _currentVelocityXZ;
            _endUpdate.OnNext(velocity);
        }

        private void BackToGround()
        {
            _upVelocity.y = _characterController.isGrounded ? 0 : _upVelocity.y -= Time.deltaTime * _gravity;
        }

        private void FixedUpdate()
        {
            Rotate();
        }

        private void SetVelocity()
        {
            if (_characterController is null) return;

            var rawInput = new Vector3(Horizontal.Value, 0, Vertical.Value);

            _inputFrame = GetInputFrame(Vector3.Dot(rawInput, _lastRawInput) < 0.8f);
            _lastRawInput = rawInput;

            _lastInput = _inputFrame * rawInput;

            if (_lastInput.sqrMagnitude > 1)
                _lastInput.Normalize();

            var desiredVelocity = _lastInput * Speed;

            if (Vector3.Angle(_currentVelocityXZ, desiredVelocity) < 100)
            {
                _currentVelocityXZ = Vector3.Slerp(
                    _currentVelocityXZ, desiredVelocity,
                    Damper.Damp(1, _config.Damping, Time.deltaTime));
            }

            else
                _currentVelocityXZ += Damper.Damp(
                    desiredVelocity - _currentVelocityXZ, _config.Damping, Time.deltaTime);
        }

        private void Move()
        {
            _characterController.Move((_upVelocity +_currentVelocityXZ) * Time.deltaTime);
        }

        private void Rotate()
        {
            if (!_isStrafeMoving && _currentVelocityXZ.sqrMagnitude > 0.001f)
            {
                var fwd = _inputFrame * Vector3.forward;
                var qA = transform.rotation;

                var moveDirection = _currentVelocityXZ.normalized;
                if (moveDirection.sqrMagnitude > 0.001f)
                {
                    var qB = Quaternion.LookRotation(moveDirection, _upDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qB,
                        Damper.Damp(1, _config.Damping, Time.deltaTime));
                    transform.rotation = Quaternion.Slerp(qA, qB, Damper.Damp(1, _config.Damping, Time.deltaTime));
                }
            }
        }

        private Quaternion GetInputFrame(bool inputDirectionChanged)
        {
            var frame = Quaternion.identity;
            switch (InputForward)
            {
                case ForwardType.Camera:
                    frame = ProjectCamera.transform.rotation;
                    break;
                case ForwardType.Player:
                    return transform.rotation;
                case ForwardType.World:
                    break;
            }

            var playerUp = transform.up;
            var up = frame * Vector3.up;

            const float BlendTime = 2f;
            _timeInHemisphere += Time.deltaTime;
            bool inTopHemisphere = Vector3.Dot(up, playerUp) >= 0;
            if (inTopHemisphere != _inTopHemisphere)
            {
                _inTopHemisphere = inTopHemisphere;
                _timeInHemisphere = Mathf.Max(0, BlendTime - _timeInHemisphere);
            }

            var axis = Vector3.Cross(up, playerUp);
            if (axis.sqrMagnitude < 0.001f && inTopHemisphere)
                return frame;

            var angle = UnityVectorExtensions.SignedAngle(up, playerUp, axis);
            var frameA = Quaternion.AngleAxis(angle, axis) * frame;


            Quaternion frameB = frameA;
            if (!inTopHemisphere || _timeInHemisphere < BlendTime)
            {
                frameB = frame * _upsidedown;
                var axisB = Vector3.Cross(frameB * Vector3.up, playerUp);
                if (axisB.sqrMagnitude > 0.001f)
                    frameB = Quaternion.AngleAxis(180f - angle, axisB) * frameB;
            }

            if (inputDirectionChanged)
                _timeInHemisphere = BlendTime;

            if (_timeInHemisphere >= BlendTime)
                return inTopHemisphere ? frameA : frameB;

            if (inTopHemisphere)
                return Quaternion.Slerp(frameB, frameA, _timeInHemisphere / BlendTime);
            return Quaternion.Slerp(frameA, frameB, _timeInHemisphere / BlendTime);
        }
    }
}