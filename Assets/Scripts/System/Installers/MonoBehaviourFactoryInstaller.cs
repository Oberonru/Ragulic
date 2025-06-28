using Zenject;

namespace System.Installers
{
    public class MonoBehaviourFactoryInstaller<IFactory, FactoryInstance> :
        MonoInstaller where IFactory : class where FactoryInstance : class, IFactory 
    {
        public override void InstallBindings()
        {
            Container.Bind<IFactory>().To<FactoryInstance>().AsSingle();
        }
    }
}