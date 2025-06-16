using System.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Configs.Enemies
{
    [CreateAssetMenu(menuName = "System/Configs/NavMeshAgentConfig", fileName = "NavMeshAgent")]
    public class NavMeshAgentConfig : ScriptableConfig
    {
        [BoxGroup("BaseInfo")]
        [SerializeField]
        private int _agentTypeId = 0;

        [BoxGroup("BaseInfo")]
        [SerializeField] 
        private float _baseOffset = 0.5f;

        
        [BoxGroup("Steering")] 
        [SerializeField]
        private float _angularSpeed = 120f;
        
        [BoxGroup("Steering")]
        [SerializeField]
        private float _acceleration = 8f;

        [BoxGroup("Steering")]
        [SerializeField]
        private bool _autoBracking = true;

        
        [BoxGroup("Obstacle Avoidance")]
        [SerializeField]
        private float _radius = 0.5f;
        
        [BoxGroup("Obstacle Avoidance")]
        [SerializeField]
        private float _height = 1f;

        [BoxGroup("Obstacle Avoidance")]
        [SerializeField]
        private ObstacleAvoidanceType _quality = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        [BoxGroup("Obstacle Avoidance")]
        [SerializeField]
        private int _priority = 50;


        [BoxGroup("Path Finding")] 
        [SerializeField]
        private bool _autoTraverseOffMeshLink = true;
        
        [BoxGroup("Path Finding")] 
        [SerializeField]
        private bool _autoRepath = true;

        public int AgentTypeId => _agentTypeId;
        public float BaseOffset => _baseOffset;
        public float AngularSpeed => _angularSpeed;
        public float Acceleration => _acceleration;
        public bool AutoBracking => _autoBracking;
        public float Radius => _radius;
        public float Height => _height;
        public ObstacleAvoidanceType Quality => _quality;
        public int Priority => _priority;
        public bool AutoTraverseOffMeshLink => _autoTraverseOffMeshLink;
        public bool AutoRepath => _autoRepath;

    }
}