using System.Interfaces;
using UnityEngine;
using Zenject;

namespace Settings.System.Spawners
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
                _spawnPoint.transform.position,
                _spawnPoint.transform.rotation, _spawnPoint) as TImplementation;

            if (instance is IDrawGizmos gizmos)
            {
                gizmos.DrawGizmos();
            }

            if (_isSingle)
            {
                this.Container.Bind<TInterface>().To<TImplementation>().FromInstance(instance).AsSingle();
            }
            else this.Container.Bind<TInterface>().To<TImplementation>().FromInstance(instance);
        }
    }
}