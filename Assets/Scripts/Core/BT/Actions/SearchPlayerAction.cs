using System;
using Core.BaseComponents;
using Core.Configs.Enemies;
using Core.Enemies;
using Core.Player.CombatSystem;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Zenject;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SearchPlayer", story: "Check player in [Radius] for [Self]", category: "Action",
    id: "f334d6c784da0f8f87af89d63c4fa1f0")]
public partial class SearchPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyInstance> Self;
    [SerializeReference] public BlackboardVariable<float> Radius;

    protected override Status OnUpdate()
    {
        var colliders = Physics.OverlapSphere(Self.Value.Transform.position, Radius.Value);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IPlayerHitBox>(out var playerHitBox))
            {
                if (Self.Value.NavMesh.CurrentHealth != playerHitBox.HealthComponent && playerHitBox.HealthComponent.IsAllive)
                {
                    Self.Value.NavMesh.SetTarget(playerHitBox.HealthComponent);
                    Debug.Log("find player");
                }

                return Status.Success;
            }
        }

        return Status.Running;
    }
}