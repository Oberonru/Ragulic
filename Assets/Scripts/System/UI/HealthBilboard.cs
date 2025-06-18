using Core.Camera;
using UnityEngine;
using Zenject;

namespace System.UI
{
    public class HealthBilboard : MonoBehaviour
    {
        [Inject] private IGameCamera _camera;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.Forward);
        }
    }
}