using UnityEngine;
using Zenject;

namespace System.Spawners
{
    public abstract class EntitySpawner<TInterface, TImplementation> : MonoInstaller
        where TImplementation : Component, TInterface
    {
        [SerializeField] private TImplementation _prefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private bool _isSingle;

        public override void InstallBindings()
        {
            var instance = this.Container.InstantiatePrefabForComponent<TImplementation>(_prefab,
                _prefab.transform.position,
                _prefab.transform.rotation, _spawnPoint);

            if (_isSingle)
            {
                this.Container.Bind<TInterface>().FromInstance(instance).AsSingle();
            }
            else this.Container.Bind<TInterface>().FromInstance(instance);
        }
    }
}