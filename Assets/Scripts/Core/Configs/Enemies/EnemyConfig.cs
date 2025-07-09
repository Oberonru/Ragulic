using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Enemies
{
    [CreateAssetMenu(menuName = "Configs/EnemyConfig", fileName = "Enemy Config")]
    public class EnemyConfig : LifeEntityConfig
    {
        [BoxGroup("Enemy stats"), LabelText("Push force")]
        [PropertyTooltip("Сила удара")]
        [SerializeField]
        private float _pushForce;

        [BoxGroup("Enemy stats"), LabelText("Is see")] [PropertyTooltip("Видит ли враг игрока")] [SerializeField]
        private bool _isSee;
       
        
        public float PushForce => _pushForce;

        public bool IsSee
        {
            get => _isSee;
            set => _isSee = value;
        }
    }
}