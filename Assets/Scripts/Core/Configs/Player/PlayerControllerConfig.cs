using System.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Configs.Player
{
    [CreateAssetMenu(menuName = "System/Configs/PlayerControllerConfig", fileName = "PlayerControllerConfig")]
    public class PlayerControllerConfig : ScriptableConfig
    {
        [BoxGroup("Movement params")]
        [LabelText("Damping"), SuffixLabel("sec")]
        [PropertyTooltip("Продолжительность перехода при изменении вращения или скорости")]
        [SerializeField]
        private float _damping = 0.5f;
        
        [BoxGroup("PlayerAnimatorStats")]
        [LabelText("Normal Walk Speed")]
        [PropertyTooltip(
            "Настройте это в соответствии с анимацией в модели:" +
            " ноги не должны скользить при ходьбе с такой скоростью")]
        [SerializeField]
        //можно для нала _walkSpeed / 2
        //спросить, можно ли чтобы _normalWalkSpeed была формулой и зависила от WalkSpeed ?
        private float _normalWalkSpeed = 1.7f;

        [BoxGroup("PlayerAnimatorStats")]
        [LabelText("Normal Run Speed")]
        [PropertyTooltip("Также как и Normal Walk Speed")]
        [SerializeField]
        private float _normalRunSpeed = 5f;
        
        [BoxGroup("PlayerAnimatorStats")]
        [LabelText("Max Run Scale")]
        [PropertyTooltip("Никогда не ускоряйте анимацию спринта больше," +
                         " чем это необходимо, чтобы избежать абсурдно быстрого движения")]
        [SerializeField]
        private float _maxRunScale = 1.4f;
        
        [BoxGroup("PlayerAnimatorStats")]
        [LabelText("Koefficient Idle Threshold")]
        [PropertyTooltip("Вес при котором будет будет переход из покоя в движение")]
        [SerializeField]
        private  float k_IdleThreshold = 0.2f;
        
        public float Damping => _damping;
        public float NormalWalkSpeed => _normalWalkSpeed;
        public float NormalRunSpeed => _normalRunSpeed;
        public float MaxRunScale => _maxRunScale;
        public float IdleThreshold => k_IdleThreshold;
    }
}