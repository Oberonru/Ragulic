using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class BaseInstanceInstaller<TInterface, TInstance> : MonoInstaller
        where TInstance : Component, TInterface
    {
        [SerializeField] private TInstance _prefab;
        [SerializeField] private bool _isSingle;

        public override void InstallBindings()
        {
            if (_prefab is null) throw new NullReferenceException($"{_prefab.GetType()} not found");

            if (_isSingle)
            {
                Container.Bind<TInterface>().To<TInstance>().FromInstance(_prefab).AsSingle();
            }
            else
            {
                Container.Bind<TInterface>().To<TInstance>().FromInstance(_prefab);
            }
        }
    }
}