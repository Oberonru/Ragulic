using System.Factories;
using Zenject;

namespace System.Installers
{
    public abstract class MonoBehaviourFactoryInstaller<IFactory, FactoryInstance> :
        MonoInstaller where IFactory : class, IMonoBehaviorFactory where FactoryInstance : class, IFactory
    {
        public override void InstallBindings()
        {
            Container.Bind<IFactory>().To<FactoryInstance>().AsSingle();
        }
    }
}