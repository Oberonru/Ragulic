using System;
using Core.Configs.Player;
using Core.Player.Components;
using UnityEngine;

namespace Core.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInstance : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _playerStats;
        [SerializeField] private PlayerMovement _movement;

        public PlayerConfig Stats => _playerStats;
        
        private void OnValidate()
        {
            _movement ??= GetComponent<PlayerMovement>();
        }
    }
}