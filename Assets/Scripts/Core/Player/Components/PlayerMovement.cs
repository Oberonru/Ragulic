using UnityEngine;

namespace Core.Player.Components
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInstance _player;
        [SerializeField] private CharacterController _controller;
        private float _horizontal;
        private float _vertical;

        private void FixedUpdate()
        {
            var moveDirection = new Vector3(-_vertical, 0, _horizontal);

            if (moveDirection.magnitude > 0.1f)
            {
                var targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, _player.Stats.RotationSpeed * Time.deltaTime);
            }

            _controller.Move(moveDirection * (Time.deltaTime * _player.Stats.Speed));
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