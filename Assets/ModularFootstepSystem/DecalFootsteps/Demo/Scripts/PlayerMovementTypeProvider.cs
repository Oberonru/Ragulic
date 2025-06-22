namespace ModularFootstepSystem.Demo
{
    using UnityEngine;
    using ModularFootstepSystem.Extensions;
    using ModularFootstepSystem;

    /// <summary>
    /// Player movement type provider.
    /// </summary>
    /// <remarks>
    ///Демонстрационная реализация настройки типа движения игрока. 
    /// Используется для изменения эффекта шагов при ускорении. 
    /// Также может использоваться, например,
    /// для изменения эффекта шагов, когда игрок приземляется на разные поверхности 
    /// или для других необходимых вам типов движений
    /// </remarks>
    public class PlayerMovementTypeProvider : MonoBehaviour
    {
        [SerializeField]
        protected Animator animator = default;

        [SerializeField]
        protected string animatorStateName = string.Empty;

        [SerializeField]
        protected FootstepsStateController footstepsStateController = default;

        [SerializeField]
        protected FootstepStateType footstepStateType = default;

        protected bool inState = false;
        protected bool tempStateValue = false;

        private void Update()
        {
            inState = animator.GetCurrentAnimatorStateInfo(0).IsName(animatorStateName);

            if (inState != tempStateValue)
            {
                if (inState)
                {
                    footstepsStateController.SetState(footstepStateType);
                }

                tempStateValue = inState;
            }
        }
    }
}