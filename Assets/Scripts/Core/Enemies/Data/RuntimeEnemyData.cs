using UnityEngine;

namespace Core.Enemies.Data
{
    public class RuntimeEnemyData : MonoBehaviour
    {
        public Transform[] Waypoints = new Transform[0];
        public bool IsSee { get; set; }
    }
}