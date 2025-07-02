using Core.Effects;
using Core.Factories;
using Core.Factories.Effects;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Player.CombatSystem
{
    public class VFXEffectHandler : MonoBehaviour
    {
        [Inject] private IEffectFactory _effectFactory;
        [SerializeField] private PlayerInstance _player;
        [SerializeField] private EffectInstance _effect;

        private void OnEnable()
        {
            _player.Health.OnHit.Subscribe(_ =>
            {
                _effectFactory.Create(
                    _effect, _player.Transform.position, _player.Transform.rotation, _player.Transform);
            }).AddTo(this);
        }

        private void OnValidate()
        {
            if (_player is null) _player = GetComponent<PlayerInstance>();
            if (_effect is null) _effect = GetComponent<EffectInstance>();
        }
    }
}