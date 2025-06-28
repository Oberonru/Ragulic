using System.Factories;
using Core.Effects;

namespace Core.Factories
{
    public interface IEffectFactory : IMonoBehaviorFactory<EffectInstance, IEffectInstance>
    {
    }
}