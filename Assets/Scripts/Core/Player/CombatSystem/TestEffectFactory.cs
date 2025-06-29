using System;
using Core.Effects;
using Core.Factories;
using UnityEngine;
using Zenject;

namespace Core.Player.CombatSystem
{
    public class TestEffectFactory : MonoBehaviour
    {
        [Inject] private IEffectFactory _effectFactory;
        [SerializeField] private EffectInstance _effectInstance;

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                _effectFactory.Create(_effectInstance, Input.mousePosition, Quaternion.identity, null);
            }
        }
    }
}