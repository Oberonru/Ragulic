using System.Providers.Configs;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "System/Configs/ScriptableConfig", fileName = "InputConfig")]
    public class InputConfig : ScriptableConfig
    {
        public KeyCode Interaction => _interaction;
        [SerializeField] private KeyCode _interaction = KeyCode.E;
        
        public KeyCode Acceleration => _acceleration;
        [SerializeField] private KeyCode _acceleration = KeyCode.LeftShift;
    }
}