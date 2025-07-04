using System;
using Core.BaseComponents;
using Core.Configs;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [Inject] private InputConfig _input;
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
        }

        private void OnEnable()
        {
            
            _health?.OnHit.Subscribe((damager) => Panic(_player.Stats.PanicTime))
                .AddTo(this);
        }

        private void Update()
        {
            _vertical = Input.GetAxis("Vertical");
            _horizontal = Input.GetAxis("Horizontal");

            if (Input.GetKey(_input.Acceleration))
            {
                _player.StateMachine.SetRunning(_player.Stats.RunSpeed);
            }

            if (Input.GetKey(_input.Crouch))
            {
                _player.StateMachine.SetCrouch(_player.Stats.CrouchSpeed);
            }

            //_isRunning = Input.GetKey(_input.Acceleration);
            //
            // if (_isRunning && _isCrouch)
            // {
            //     _isCrouch = false;
            // }
            //
            // if (Input.GetKeyDown(_input.Crouch))
            // {
            //     _isCrouch = !_isCrouch;
            // }
            //
            // Debug.Log(_isCrouch + " crouch");
        }

        private void FixedUpdate()
        {
            // SetSpeed();
            //
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

        private async UniTask Panic(float panicTime)
        {
            _isPanic = true;
            await UniTask.Delay(TimeSpan.FromSeconds(panicTime));
            _isPanic = false;

            if (_isCrouch) _isCrouch = false;
        }

        private void SetSpeed()
        {
            if (_isPanic)
            {
                _speed = _player.Stats.PanicSpeed;
            }
            else if (_isRunning)
            {
                _speed = _player.Stats.RunSpeed;
            }
            else if (_isCrouch)
            {
                _speed = _player.Stats.CrouchSpeed;
            }


            else
            {
                _speed = _player.Stats.WalkSpeed;
            }
        }

        private void RotateToMovement(Vector3 moveDirection)
        {
            var rotation = Quaternion.LookRotation(moveDirection);

            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _player.Stats.RotationSpeed);
        }

        private void Move(Vector3 moveDirection)
        {
            _rigidbody.linearVelocity = moveDirection * _speed;
        }
    }
}