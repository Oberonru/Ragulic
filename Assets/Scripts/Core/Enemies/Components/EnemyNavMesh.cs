using Core.Configs.Enemies;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Core.Enemies.Components
{
    public class EnemyNavMesh : MonoBehaviour
    {
        [Inject] private AIConfig _aiConfig;
        [Inject] private NavMeshAgentConfig _navMeshAgentConfig;

        [SerializeField] private EnemyInstance _enemyInstance;
        [SerializeField] private NavMeshAgent _agent;

        private float _stoppingDistance;

        private void Awake()
        {
            SetUpAgentParams();
            _stoppingDistance = GetDistance();
        }

        private void OnValidate()
        {
            if (_enemyInstance is null) GetComponent<EnemyInstance>();
            if (_agent is null) GetComponent<NavMeshAgent>();
        }

        public void MoveToTarget(Vector3 target)
        {
            var randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            if (ValidDistance(target))
            {
                MoveTo(target + randomOffset);
            }
            else
            {
                Stop();
            }
        }

        private bool ValidDistance(Vector3 target)
        {
            return Vector3.Distance(transform.position, target) > _stoppingDistance;
        }

        private void MoveTo(Vector3 target)
        {
            _agent.isStopped = false;
            _agent.SetDestination(target);
        }

        private void SetUpAgentParams()
        {
            _agent.agentTypeID = _navMeshAgentConfig.AgentTypeId;
            _agent.baseOffset = _navMeshAgentConfig.BaseOffset;

            _agent.speed = _enemyInstance.EnemyStats.Speed;

            _agent.angularSpeed = _navMeshAgentConfig.AngularSpeed;
            _agent.acceleration = _navMeshAgentConfig.Acceleration;
            _agent.autoBraking = _navMeshAgentConfig.AutoBracking;
            _agent.radius = _navMeshAgentConfig.Radius;
            _agent.height = _navMeshAgentConfig.Height;
            _agent.obstacleAvoidanceType = _navMeshAgentConfig.Quality;
            _agent.avoidancePriority = _navMeshAgentConfig.Priority;
            _agent.autoTraverseOffMeshLink = _navMeshAgentConfig.AutoTraverseOffMeshLink;
            _agent.autoRepath = _navMeshAgentConfig.AutoRepath;
        }

        private void Stop()
        {
            if (enabled)
            {
                _agent.SetDestination(transform.position);
                _agent.isStopped = true;
            }
        }

        private float GetDistance()
        {
            return Random.Range(_aiConfig.MinStoppingDistance, _aiConfig.MaxStoppingDistance);
        }
    }
}