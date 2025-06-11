using Core.Configs.Player;
using Core.Player.Components;
using Unity.Collections;
using UnityEngine;
using Zenject;

namespace Core.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInstance : MonoBehaviour, IPlayerInstance
    {
        [Inject] private PlayerConfig _playerStats;
        [SerializeField, ReadOnly] private PlayerMovement _movement;

        public PlayerConfig Stats => _playerStats;
        
        private void OnValidate()
        {
            _movement ??= GetComponent<PlayerMovement>();
        }
    }
}