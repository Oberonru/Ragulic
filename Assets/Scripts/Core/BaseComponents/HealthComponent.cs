using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Core.BaseComponents
{
    public class HealthComponent : MonoBehaviour, IHealthComponent
    {
        public bool IsAllive => _currentHealth > 0;

        [ShowInInspector, ReadOnly]
        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        [ShowInInspector, ReadOnly]
        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }

        public ISubject<object> OnHit => _onHit;
        private Subject<object> _onHit = new();
        public ISubject<int> OnHealthChanged => _onHealthChanged;
        private Subject<int> _onHealthChanged = new();
        public ISubject<Unit> OnDead => _onDead;
        private Subject<Unit> _onDead = new();
        private int _currentHealth { get; set; }
        private int _maxHealth;


        public void TakeDamage(int amount, [CanBeNull] object damager = null)
        {
            if (_currentHealth <= 0) return;

            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
            _onHit?.OnNext(damager);
            _onHealthChanged?.OnNext(amount);

            if (_currentHealth <= 0)
            {
                _onDead?.OnNext(Unit.Default);
                _onDead?.OnCompleted();
            }
        }

        public void Regeneration(int amount)
        {
            throw new NotImplementedException();
        }
    }
}