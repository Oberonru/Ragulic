using System;
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
            get
            { 
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }

        [ShowInInspector, ReadOnly]
        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }

        public ISubject<object> OnHit { get; }
        public ISubject<int> OnHealthChanged => _onHealthChanged;
        public ISubject<Unit> OnDead { get; }
        private int _currentHealth { get; set; }
        private int _maxHealth;

        private Subject<int> _onHealthChanged = new();

        public void TakeDamage(int amount, object damager = null)
        {
            if (_currentHealth <= 0) return;
            
             _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
            OnHit?.OnNext(damager);
            OnHealthChanged?.OnNext(amount);

            if (_currentHealth <= 0)
            {
                OnDead?.OnNext(Unit.Default);
                OnDead?.OnCompleted();
            }
        }

        public void Regeneration(int amount)
        {
            throw new NotImplementedException();
        }
    }
}