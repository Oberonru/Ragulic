using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Core.Camera
{
    public interface IGameCamera
    {
        void SetTarget(Transform target);
        Vector3 Forward { get; }
    }
}