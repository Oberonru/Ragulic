using Core.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Configs
{
    public class LifeEntityConfig : ScriptableBaseEntityData
    {
        [BoxGroup("EntityStats")]
        [SerializeField, LabelText("Walk speed")]
        private float _walkSpeed;

        [FormerlySerializedAs("_maxHealth")]
        [BoxGroup("EntityStats")] [SerializeField, LabelText("Max health")]
        private int _health;
        
        [BoxGroup("EntityStats")]
        [SerializeField, LabelText("Damage")]
        private int _damage;

        [BoxGroup("EntityStats")]
        [SerializeField, LabelText("Attack per seconds"), SuffixLabel("seconds")]
        private float _attackPerSeconds;

        [BoxGroup("EntityStats")] 
        [LabelText("Damage shift")]
        [SerializeField]
        private int _damageShift;
        
        public float WalkSpeed => _walkSpeed;
        public int Health => _health;
        public int Damage => _damage;
        public float AttackPerSeconds => _attackPerSeconds;
        public int DamageShift => _damageShift;

    }
}