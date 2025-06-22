using Core.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Player.Components
{
    public class PlayerMovement : MonoBehaviour
    {
        [Inject] InputConfig _input;
        [SerializeField, ReadOnly] private PlayerInstance _player;
        [SerializeField] private CharacterController _controller;
        private float _horizontal;
        private float _vertical;
        private bool _isRunning;
        private float _speed;

        private void FixedUpdate()
        {
            var moveDirection = new Vector3(_horizontal, 0, _vertical);

            if (moveDirection.magnitude > 0.1f)
            {
                var targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, _player.Stats.RotationSpeed * Time.deltaTime);
            }

            _isRunning = Input.GetKey(_input.Acceleration);

            _speed = _isRunning ? _player.Stats.RunSpeed : _player.Stats.WalkSpeed;

            _controller.Move(moveDirection * (Time.deltaTime * _speed));
        }

        private void Update()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        }

        private void OnValidate()
        {
            _player ??= GetComponent<PlayerInstance>();
        }
    }
}