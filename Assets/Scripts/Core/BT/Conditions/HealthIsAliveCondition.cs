using Core.BaseComponents;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "HealthIsAliveCondition", story: "[Target] is Alive", category: "Conditions", id: "76e9e8dd55920dad7dbd091fd7b200ea")]
public partial class HealthIsAliveCondition : Condition
{
    [SerializeReference] public BlackboardVariable<HealthComponent> Target;
    
    public override bool IsTrue()
    {
        Debug.Log("Target.Value is null");
        if (Target.Value is null) return false;

        Debug.Log(Target.Value.IsAllive + " Target.Value.IsAllive");
        return Target.Value.IsAllive == false;
    }
}
