using Unity.Cinemachine;
using UnityEngine;

namespace Core.Camera
{
    public class GameCamera : MonoBehaviour, IGameCamera
    {
        [SerializeField] private CinemachineCamera _camera;
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
            _camera.Follow = _target;
        }

        public Vector3 Forward => transform.forward;
    }
}