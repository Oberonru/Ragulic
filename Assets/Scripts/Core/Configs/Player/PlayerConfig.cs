using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Player
{
    [CreateAssetMenu(menuName = "Configs/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : LifeEntityConfig
    {
        [BoxGroup("PlayerStats")] 
        [SerializeField, LabelText("Rotation speed")]
        private float _rotationSpeed;

        [BoxGroup("PlayerStats")]
        [SerializeField, LabelText("Run speed")]
        private float _runSpeed;

        [BoxGroup("PlayerStats")]
        [PropertyTooltip("Паническая скорость при получении урона")]
        [LabelText("Panic speed")]
        [SerializeField]
        private float _panicSpeed;

        [BoxGroup("PlayerStats")]
        [PropertyTooltip("Врямя панического бега")]
        [LabelText("Panic speed time")]
        [SerializeField]
        private float _panicTime;

        public float RotationSpeed => _rotationSpeed;
        public float RunSpeed => _runSpeed;
        public float PanicSpeed => _panicSpeed;
        public float PanicTime => _panicTime;
    }
}