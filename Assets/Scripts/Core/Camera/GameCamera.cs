using Core.Configs.Camera;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Core.Camera
{
    public class GameCamera : MonoBehaviour, IGameCamera
    {
        [Inject] private CinemachineCameraConfig _config;
        public CinemachineCamera MainCamera => _camera;
        [SerializeField] private CinemachineCamera _camera;
        public Transform Transform => transform;

        private void Start()
        {
            _camera.OutputChannel = _config.OutputChannel;
            _camera.StandbyUpdate = _config.StandbyUpdateMode;
            _camera.Lens.FieldOfView = _config.FieldOfView;
            _camera.Lens.NearClipPlane = _config.NearClipPlane;
            _camera.Lens.FarClipPlane = _config.FarClipPlane;
            _camera.Lens.Dutch = _config.Dutch;
        }

        private void OnValidate()
        {
            if (_camera is null) _camera = GetComponent<CinemachineCamera>();
        }

        public void SetTarget(Transform target)
        {
            _camera.Follow = target;
            _camera.LookAt = null;
        }

        public Vector3 Forward => transform.forward;
    }
}