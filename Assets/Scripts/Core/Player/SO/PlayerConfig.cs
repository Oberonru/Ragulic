using Core.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public float RotationSpeed => _rotationSpeed;
        public float RunSpeed => _runSpeed;
    }
}