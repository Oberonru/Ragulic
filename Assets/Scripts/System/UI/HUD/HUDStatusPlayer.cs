using Core.Player;
using UnityEngine;
using Zenject;

namespace System.UI.HUD
{
    public class HUDStatusPlayer : MonoBehaviour
    {
        [Inject] private IPlayerInstance _player;
        [SerializeField] private HealthBar _healthBar;
        //остальные параметры игрока


        private void Start()
        {
            _healthBar.SetData(_player.Health);
        }
    }
}