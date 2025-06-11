using Core.Player;
using UnityEngine;
using Zenject;

namespace System.Spawners
{
    public class MonobehPlayerSpawner : MonoInstaller
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnPoint;
        
        public override void InstallBindings()
        {
            var instance =
                Container.InstantiatePrefabForComponent<IPlayerInstance>(_prefab, _spawnPoint.position,
                    _spawnPoint.rotation, null);
           Container.Bind<IPlayerInstance>().FromInstance(instance);
        }
    }
}