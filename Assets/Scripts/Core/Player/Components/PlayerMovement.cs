using Core.BaseComponents;
using Core.Camera;
using Core.Configs;
using Core.Player.StateMachine.States;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [Inject] private InputConfig _input;
        [Inject] private IGameCamera _camera;
        
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;
        [SerializeField] private PlayerInstance _player;
        [SerializeField] private HealthComponent _health;

        public bool IsRunning
        {
            get => _isRunning;
            set => _isRunning = value;
        }

        private bool _isRunning;

        public bool IsCrouch
        {
            get => _isCrouch;
            set => _isCrouch = value;
        }

        private bool _isCrouch;

        public bool IsPanic
        {
            get => _isPanic;
            set => _isPanic = value;
        }

        private bool _isPanic;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        [ShowInInspector] private float _speed;
        private float _horizontal;
        private float _vertical;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _player.StateMachine.SetWalking(_player.Stats.WalkSpeed);
            _camera.SetTarget(_player.transform);
        }

        private void OnEnable()
        {
            _health?.OnHit.Subscribe(_ =>
                    _player.StateMachine.SetPanic(_player.Stats.PanicSpeed))
                .AddTo(this);
        }

        private void Update()
        {
            _vertical = Input.GetAxis("Vertical");
            _horizontal = Input.GetAxis("Horizontal");

            if (!IsPanic)
            {
                if (_player.StateMachine.GetActiveState() != typeof(PlayerRunState) &&
                    Input.GetKey(_input.Acceleration))
                {
                    _player.StateMachine.SetRunning(_player.Stats.RunSpeed);
                }
                else if (_player.StateMachine.GetActiveState() != typeof(PlayerWalkState) && !IsCrouch)
                {
                    _player.StateMachine.SetWalking(_player.Stats.WalkSpeed);
                }

                if (_player.StateMachine.GetActiveState() != typeof(PlayerCrouchState) &&
                    Input.GetKey(_input.Crouch))
                {
                    _player.StateMachine.SetCrouch(_player.Stats.CrouchSpeed);
                }
            }
        }

        private void FixedUpdate()
        {
            var moveDirection = new Vector3(_horizontal, 0, _vertical);

            if (moveDirection != Vector3.zero)
            {
                RotateToMovement(moveDirection);
                Move(moveDirection);
            }
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
            if (_health is null) _health = GetComponent<HealthComponent>();
        }

        private void RotateToMovement(Vector3 moveDirection)
        {
            var rotation = Quaternion.LookRotation(moveDirection);

            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _player.Stats.RotationSpeed);
        }

        private void Move(Vector3 moveDirection)
        {
            _rigidbody.linearVelocity = moveDirection * this.Speed;
        }
    }
}