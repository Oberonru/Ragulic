using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DebugAction", story: "[Agent] debug [Target]", category: "Action",
    id: "d1b6b31d17b5e634b5292a0c4951e1c3")]
public partial class DebugAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        Debug.Log(Agent.Value + " OnStart");
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Debug.Log(Agent.Value + " OnUpdate");
        return Status.Success;
    }

    protected override void OnEnd()
    {
        Debug.Log(Agent.Value + " OnEnd");
    }
}