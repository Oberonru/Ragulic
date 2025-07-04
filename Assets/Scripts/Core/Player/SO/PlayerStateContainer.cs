using System.StateMachineSystem.SO;
using UnityEngine;

namespace Core.Player.SO
{
    [CreateAssetMenu(menuName = "StateMachine/Container/PlayerStateContainer", fileName = "PlayerStateContainer")]
    public class PlayerStateContainer : StateContainer<PlayerInstance>
    {
    }
}