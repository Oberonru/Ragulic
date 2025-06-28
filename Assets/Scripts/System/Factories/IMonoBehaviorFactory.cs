using JetBrains.Annotations;
using UnityEngine;

namespace System.Factories
{
    
    public interface IMonoBehaviorFactory {}
    
    public interface IMonoBehaviorFactory<TPrefab, TReturn> where TReturn : IFactoryObject where TPrefab : MonoBehaviour, TReturn
    {
        TReturn Create(TPrefab prefab, Vector3 position, Quaternion rotation, [CanBeNull] Transform parent = null);
    }
}