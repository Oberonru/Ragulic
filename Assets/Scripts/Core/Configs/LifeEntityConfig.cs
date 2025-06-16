using Core.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Configs
{
    public class LifeEntityConfig : SrcriptableBaseEntityData
    {
        [BoxGroup("EntityStats")] [SerializeField, LabelText("Movement speed")]
        private float _speed;

        [FormerlySerializedAs("_maxHealth")] [BoxGroup("EntityStats")] [SerializeField, LabelText("Max health")]
        private int _health;

        public float Speed => _speed;
        public int Health => _health;
    }
}