using Share.Configs;
using UnityEngine;
using Zenject;

namespace Share.Animations
{
    public class AlarmViewAnimation : MonoBehaviour
    {
        [Inject] private AlarmViewAnimationConfig _config;

        private void OnEnable()
        {
            
        }
    }
}