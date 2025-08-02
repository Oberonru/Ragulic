using System;
using Core.BaseComponents;
using Core.Enemies;
using Core.Player;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToPlayer", story: "[Agent] move to player", category: "Action",
    id: "74feed042dafd0add69a17c1cd99586e")]
public partial class MoveToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyInstance> Agent;
    private IHealthComponent _targetComponent;
    private Vector3 _lastTargetPosition;

    protected override Status OnStart()
    {
        _targetComponent = Agent.Value.NavMesh.CurrentHealth;

        if (_targetComponent == null)
        {
            return Status.Failure;
        }
        
        _lastTargetPosition = _targetComponent.Position;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value.NavMesh.Agent is null || _targetComponent is null)
        {
            return Status.Failure;
        }

        if (Vector3.Distance(Agent.Value.Position, _lastTargetPosition) > 0.5f)
            UpdateTargetPosition();

        if (Agent.Value.NavMesh.Agent.velocity.sqrMagnitude == 0)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    private void UpdateTargetPosition()
    {
        Agent.Value.NavMesh.MoveToTarget(_targetComponent.Position);
        _lastTargetPosition = _targetComponent.Position;
    }
}