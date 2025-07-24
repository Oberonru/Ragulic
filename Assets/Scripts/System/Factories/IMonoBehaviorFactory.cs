using JetBrains.Annotations;
using UnityEngine;

namespace System.Factories
{
    public interface IMonoBehaviorFactory
    {
    }

    public interface IMonoBehaviorFactory<TPrefab, TReturn> : IMonoBehaviorFactory where TReturn : IFactoryObject
        where TPrefab : MonoBehaviour, TReturn
    {
        public IObservable<TReturn> OnSpawn { get; }

        TReturn Create(TPrefab prefab, Vector3 position, Quaternion rotation, [CanBeNull] Transform parent = null);
    }
}