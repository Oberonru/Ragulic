using UnityEngine;

namespace Core.Enemies.Components
{
    public class RuntimeEnemyData : MonoBehaviour
    {
        public bool IsSee
        {
            get => _isSee; 
            set => _isSee = value;
        }
        private bool _isSee;

        public Transform Waypoints => _waypoints;
        [SerializeField] private Transform _waypoints;
    }
}