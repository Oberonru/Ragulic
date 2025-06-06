using System.Providers.Configs;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "System/Configs/ScriptableConfig", fileName = "Key Config")]
    public class KeyConfig : ScriptableConfig
    {
        public KeyCode E => _e;
        [SerializeField] private KeyCode _e = KeyCode.E;
    }
}