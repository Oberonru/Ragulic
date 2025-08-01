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

    protected override Status OnStart()
    {
        _targetComponent = Agent.Value.NavMesh.CurrentHealth;
        
        if (_targetComponent == null)
        {
            return Status.Failure;
        }
        
        //agent setDestination(player.transform)
        Agent.Value.NavMesh.MoveToTarget(_targetComponent.Position);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}