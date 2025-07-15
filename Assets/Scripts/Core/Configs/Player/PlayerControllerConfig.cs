using System.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Player
{
    [CreateAssetMenu(menuName = "System/Configs/PlayerControllerConfig", fileName = "PlayerControllerConfig")]
    public class PlayerControllerConfig : ScriptableConfig
    {
        [BoxGroup("Movement params")]
        [LabelText("Damping"), SuffixLabel("sec")]
        [PropertyTooltip("Продолжительность перехода при изменении вращения или скорости")]
        [SerializeField]
        private float _damping = 0.5f;
        
        public float Damping => _damping;
        
    }
}