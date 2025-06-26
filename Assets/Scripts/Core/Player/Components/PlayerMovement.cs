using Core.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [Inject] private InputConfig _input;
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;
        [SerializeField] private PlayerInstance _player;

        private bool _isRunning;
        private float _speed;
        private float _horizontal;
        private float _vertical;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _vertical = Input.GetAxis("Vertical");
            _horizontal = Input.GetAxis("Horizontal");

            _isRunning = Input.GetKey(_input.Acceleration);
        }

        private void FixedUpdate()
        {
            _speed = _isRunning ? _player.Stats.RunSpeed : _player.Stats.WalkSpeed;

            var moveDirection = new Vector3(_horizontal, 0, _vertical);

            if (moveDirection != Vector3.zero)
            {
                RotateToMovement(moveDirection);
                Move(moveDirection);
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

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
        }
    }
}