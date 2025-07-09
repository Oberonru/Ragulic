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
        
        
        public float PushForce => _pushForce;

      
    }
}