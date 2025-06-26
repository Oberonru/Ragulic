using Core.Enemies.Dto;
using UniRx;
using UnityEngine;

namespace Core.CombatSystem
{
    public class CollisionHitBoxDetector : MonoBehaviour, ICollisionHitBoxDetector
    {
        public ISubject<IHitBox> OnHitBoxExit => _onHitBoxExit;
        private Subject<IHitBox> _onHitBoxExit = new();
        public ISubject<IAttackDto> OnDetected => _onAttackDto;
        private Subject<IAttackDto> _onAttackDto = new(); 

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<IHitBox>(out var hitBox))
            {
                var dto = new AttackDto();
                dto.Initialize(hitBox, other);
                
                _onAttackDto?.OnNext(dto);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.TryGetComponent<IHitBox>(out var hitBox))
            {
                _onHitBoxExit?.OnNext(hitBox);
            }
        }
    }
}