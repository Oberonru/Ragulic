using Core.Player;
using UnityEngine;

namespace Core.Observers
{
    public abstract class PlayerStatisticObserver : MonoBehaviour
    {
        protected IPlayerStatisticHandler StatisticHandler;
        protected IPlayerInstance Player;
    }
}