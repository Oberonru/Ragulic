using System.Providers.Configs;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "System/Configs/ScriptableConfig", fileName = "InputConfig")]
    public class InputConfig : ScriptableConfig
    {
        public KeyCode Interaction => _interaction;
        [Tooltip("Взаимодействие с предметами")]
        [SerializeField] private KeyCode _interaction = KeyCode.E;
        
        public KeyCode Acceleration => _acceleration;
        [Tooltip("Ускорение")]
        [SerializeField] private KeyCode _acceleration = KeyCode.LeftShift;
        
        public KeyCode Crouch => _crouch;
        [Tooltip("Приседение")]
        [SerializeField] private KeyCode _crouch = KeyCode.LeftControl;
    }
}