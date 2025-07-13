using Core.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "[Agent] move to [Target]", category: "Action", id: "077c8c17f8f013c29858617f383e44e2")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyInstance> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    

    protected override Status OnStart()
    {
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

