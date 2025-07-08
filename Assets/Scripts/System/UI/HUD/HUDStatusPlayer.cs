using System.UI.Health;
using System.UI.Heart;
using Core.Player;
using UnityEngine;
using Zenject;

namespace System.UI.HUD
{
    public class HUDStatusPlayer : MonoBehaviour
    {
        [Inject] private IPlayerInstance _player;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private HeartBeat _heartBeat;

        private void Start()
        {
            _healthBar.SetData(_player.Health);
            _heartBeat.SetData(_player.Transform, _heartBeat.Heart.DetectedRadius);
        }

        private void OnValidate()
        {
            if (_healthBar is null) _healthBar = GetComponent<HealthBar>();
            if (_heartBeat is null) _heartBeat = GetComponent<HeartBeat>();
        }

        public void OnDrawGizmos()
        {
            if (_player is null) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_player.Transform.position, _heartBeat.Heart.DetectedRadius);
        }
    }
}