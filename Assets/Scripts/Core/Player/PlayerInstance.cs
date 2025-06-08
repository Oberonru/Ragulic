using System;
using Core.Configs.Player;
using Core.Player.Components;
using UnityEngine;
using Zenject;

namespace Core.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInstance : MonoBehaviour
    {
        [Inject] private PlayerConfig _playerStats;
        [SerializeField] private PlayerMovement _movement;

        public PlayerConfig Stats => _playerStats;
        
        private void OnValidate()
        {
            _movement ??= GetComponent<PlayerMovement>();
        }
    }
}