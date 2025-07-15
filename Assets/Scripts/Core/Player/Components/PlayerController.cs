using System.Collections.Generic;
using Core.Configs.Player;
using Core.Player.StateMachine.States;
using Sirenix.OdinInspector;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerController : MonoBehaviour, IInputAxisOwner
    {
        [Inject] private PlayerControllerConfig _config;
        //[Inject] private IGameCamera _camera;

        public ISubject<Unit> StartUpdate;
        private Subject<Unit> _startUpdate = new();

        [SerializeField] private PlayerInstance _player;
        [SerializeField] private Rigidbody _rigidbody;

        [ShowInInspector] private InputAxis Horizontal = InputAxis.DefaultMomentary;
        [ShowInInspector] private InputAxis Vertical = InputAxis.DefaultMomentary;
        [ShowInInspector] private InputAxis Acceleration = InputAxis.DefaultMomentary;
        [ShowInInspector] private InputAxis Crouch = InputAxis.DefaultMomentary;

        [SerializeField] private UnityEngine.Camera _camera;
        public UnityEngine.Camera ProjectCamera => _camera == null ? UnityEngine.Camera.main : _camera;

        private bool _inTopHemisphere = true;
        private float _timeInHemisphere = 100;
        private bool _isStrafeMoving;
        private Vector3 _currentVelocityXY;
        private Vector3 _lastInput;
        Vector3 _lastRawInput;
        Quaternion _upsidedown = Quaternion.AngleAxis(180, Vector3.left);
        private bool _isRunning;

        [ShowInInspector] public float Speed { get; set; }
        public bool IsCrouch { get; set; }
        public bool IsPanic { get; set; }

        private enum ForwardType
        {
            Player,
            Camera,
            World
        }

        private ForwardType InputForward = ForwardType.Camera;

        private void OnEnable()
        {
            _player.Health.OnHit.Subscribe
                (_ => _player.StateMachine.SetPanicSpeed(_player.Stats.PanicSpeed)).
                AddTo(this);
        }

        private void Start()
        {
            _player.StateMachine.SetWalkSpeed(_player.Stats.WalkSpeed);
            _rigidbody = GetComponent<Rigidbody>();
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
        
        public bool IsMoving() => _lastInput.sqrMagnitude > 0.01f;

        private void Update()
        {
            _startUpdate.OnNext(Unit.Default);
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
        }

        private void SetVelocity()
        {
            if (_rigidbody is null) return;

            var rawInput = new Vector3(Horizontal.Value, 0, Vertical.Value);

            var inputFrame = GetInputFrame(Vector3.Dot(rawInput, _lastRawInput) < 0.8f);
            _lastRawInput = rawInput;

            _lastInput = inputFrame * rawInput;

            if (_lastInput.sqrMagnitude > 1)
                _lastInput.Normalize();

            var desiredVelocity = _lastInput * Speed;

            if (Vector3.Angle(_currentVelocityXY, desiredVelocity) < 100)
            {
                _currentVelocityXY = Vector3.Slerp(
                    _currentVelocityXY, desiredVelocity,
                    Damper.Damp(1, _config.Damping, Time.deltaTime));
            }

            else
                _currentVelocityXY += Damper.Damp(
                    desiredVelocity - _currentVelocityXY, _config.Damping, Time.deltaTime);
        }

        private void Move()
        {
            _rigidbody.linearVelocity = _currentVelocityXY;
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