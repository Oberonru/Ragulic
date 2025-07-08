using Core.Configs.Camera;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Core.Camera
{
    public class GameCamera : MonoBehaviour, IGameCamera
    {
        [Inject] private CinemachineCameraConfig _config;

        [SerializeField] private CinemachineCamera _camera;
        private Transform _target;

        private void Start()
        {
            _camera.OutputChannel = _config.OutputChannel;
            _camera.StandbyUpdate = _config.StandbyUpdateMode;
            _camera.Lens.FieldOfView = _config.FieldOfView;
            _camera.Lens.NearClipPlane = _config.NearClipPlane;
            _camera.Lens.FarClipPlane = _config.FarClipPlane;
            _camera.Lens.Dutch = _config.Dutch;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _camera.Follow = _target;
        }

        public Vector3 Forward => transform.forward;
    }
}