using Core.Player;
using UnityEngine;
using Zenject;

namespace System.UI.HUD
{
    public class HUDStatusPlayer : MonoBehaviour
    {
        [Inject] private IPlayerInstance _player;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private HeartUI _heart;

        private void Start()
        {
            _healthBar.SetData(_player.Health);
        }

        private void OnValidate()
        {
            if (_healthBar is null) _healthBar = GetComponent<HealthBar>();
            if (_heart is null) _heart = GetComponent<HeartUI>();
        }
    }
}