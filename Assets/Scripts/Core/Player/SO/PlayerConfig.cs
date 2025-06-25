using Core.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Player.SO
{
    [CreateAssetMenu(menuName = "Configs/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : LifeEntityConfig
    {
        [BoxGroup("PlayerStats")] [SerializeField, LabelText("Rotation speed")]
        private float _rotationSpeed;
        
        [BoxGroup("PlayerStats")]
        [SerializeField, LabelText("Run speed")]
        private float _runSpeed;
        
        [BoxGroup("PlayerStats")]
        [PropertyTooltip("Паническая скорость при получении урона")]
        [ LabelText("Panic speed")]
        [SerializeField]
        private float _panicSpeed;

        public float RotationSpeed => _rotationSpeed;
        public float RunSpeed => _runSpeed;
        public float PanicSpeed => _panicSpeed;
    }
}