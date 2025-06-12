using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Player
{
    [CreateAssetMenu(menuName = "Configs/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : LifeEntityConfig
    {
        [BoxGroup("PlayerStats")] [SerializeField, LabelText("Rotation speed")]
        private float _rotationSpeed;

        public float RotationSpeed => _rotationSpeed;
    }
}