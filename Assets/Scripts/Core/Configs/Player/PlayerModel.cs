using System;
using UnityEditor;
using UnityEngine;

namespace Core.Configs.Player
{
    public class PlayerModel
    {
        public string Id { get; }
        private PlayerConfig _playerConfig;

        public PlayerModel(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            Id = Guid.NewGuid().ToString();
        }
    }
}