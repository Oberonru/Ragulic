using System.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Player
{
    [CreateAssetMenu(menuName ="Configs/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableConfig
    {
        [BoxGroup("PlayerStats")]
        [SerializeField, LabelText("Movement speed")]
        private float _speed;

        [BoxGroup("PlayerStats")]
        [SerializeField, LabelText("Rotation speed")] private float _rotationSpeed;
        
        public float Speed => _speed;
        public float RotationSpeed => _rotationSpeed;
    }
}