using Core.Enemies.Components;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectedCollider", story: "Update [Range] Detector and assign [Target]", category: "Action",
    id: "9e5cbecf5b5007a56c338c644fff0c6e")]
public partial class DetectedColliderAction : Action
{
    [SerializeReference] public BlackboardVariable<RangeDetector> Range;
    [SerializeReference] public BlackboardVariable<GameObject> Target;


    protected override Status OnUpdate()
    {
        Target.Value = Range.Value.UpdateDetector();
        return Range.Value.UpdateDetector() == null ? Status.Failure : Status.Success;
    }
}