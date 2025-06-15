using System.Interfaces;
using UnityEngine;
using Zenject;

namespace System.Spawners
{
    public abstract class MonobehEntitySpawner<TInterface, TImplementation> : MonoInstaller
        where TImplementation : Component, TInterface
    {
        [SerializeField] private TImplementation _prefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private bool _isSingle;

        public override void InstallBindings()
        {
            var instance = this.Container.InstantiatePrefabForComponent<TImplementation>(_prefab,
                _prefab.transform.position,
                _prefab.transform.rotation, _spawnPoint) as TImplementation;

            if (_isSingle)
            {
                this.Container.Bind<TInterface>().To<TImplementation>().FromInstance(instance).AsSingle();
            }
            else this.Container.Bind<TInterface>().To<TImplementation>().FromInstance(instance);
        }
    }
}