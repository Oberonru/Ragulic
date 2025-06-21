using Core.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Configs
{
    public class LifeEntityConfig : SrcriptableBaseEntityData
    {
        [BoxGroup("EntityStats")]
        [SerializeField, LabelText("Movement speed")]
        private float _speed;

        [FormerlySerializedAs("_maxHealth")]
        [BoxGroup("EntityStats")] [SerializeField, LabelText("Max health")]
        private int _health;
        
        [BoxGroup("EntityStats")]
        [SerializeField, LabelText("Damage")]
        private int _damage;

        [BoxGroup("EntityStats")]
        [SerializeField, LabelText("Attack per seconds"), SuffixLabel("APS")]
        private float _attackPerSeconds;

        [BoxGroup("EntityStats")] 
        [LabelText("Damage shift")]
        [SerializeField]
        private int _damageShift;
        
        public float Speed => _speed;
        public int Health => _health;
        public int Damage => _damage;
        public float AttackPerSeconds => _attackPerSeconds;
        public int DamageShift => _damageShift;

    }
}