using Unity.Behavior;

namespace Core.Enemies.States
{
    [BlackboardEnum]
    public enum EnemyStateType
    {
        Idle,
        SearchPlayer,
        MoveToPlayer,
        Attack,
        Die
    }
}