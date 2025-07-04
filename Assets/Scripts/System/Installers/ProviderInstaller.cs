using System.Interfaces;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public abstract class ProviderInstaller<T> : MonoInstaller where T : UnityEngine.Object 
    {
        [SerializeField] private T[] _elements = new T [0];

        public override void InstallBindings()
        {
           for (var i = 0; i < _elements.Length; i++)
            {
                Container.Bind(_elements[i].GetType()).FromInstance(_elements[i]).AsSingle();
                
                if (_elements[i] is IInitialize initialize)
                {
                    initialize.Initialize();
                }
            }
        }
    }
}