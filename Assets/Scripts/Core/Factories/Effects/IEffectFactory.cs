using System.Factories;
using Core.Effects;

namespace Core.Factories.Effects
{
    public interface IEffectFactory : IMonoBehaviorFactory<EffectInstance, IEffectInstance>
    {
    }
}