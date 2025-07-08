using System.Providers.Configs;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Configs.Camera
{
    [CreateAssetMenu(menuName = "Configs/CinemachineCameraConfig", fileName = "CinemachineCameraConfig")]
    public class CinemachineCameraConfig : ScriptableConfig
    {
        [BoxGroup("CinemachineCameraConfig")]
        [LabelText("OutputChannels")]
        [PropertyTooltip(" \"слой\" или \"канал\", с помощью которого CinemachineCamera связывается с конкретным " +
                         "CinemachineBrain, позволяя управлять выводом камеры в мультиэкранных " +
                         "или сложных сценах с несколькими камерами")]
        [SerializeField]
        private OutputChannels _outputChannel = OutputChannels.Default;

        [BoxGroup("CinemachineCameraConfig")]
        [LabelText("OutputChannels")]
        [PropertyTooltip(" Частота обновления камеры, когда она не активна(Never - когда не активна не обновляется)")]
        [SerializeField]
        private CinemachineVirtualCameraBase.StandbyUpdateMode _standbyUpdateMode =
            CinemachineVirtualCameraBase.StandbyUpdateMode.Never; 
        
        [FormerlySerializedAs("verticalFieldOfView")]
        [FormerlySerializedAs("_verticalFOV")]
        [FoldoutGroup("Lens")]
        [LabelText("FOV")]
        [PropertyTooltip("Вертикальный угол обзора камеры в градусах. Чем больше значение — тем шире угол обзора.")]
        [SerializeField]
        private float _fieldOfView = 60;
        
        [FoldoutGroup("Lens")]
        [LabelText("Near Clip Plane")]
        [PropertyTooltip("Ближняя плоскость отсечения - расстояние ближе которого объект не отрисовывается")]
        [SerializeField]
        private float _nearClipPlane = 0.3f;
        
        [FoldoutGroup("Lens")]
        [LabelText("Far Clip Plane")]
        [PropertyTooltip("Дальняя плоскость отсечения - расстояние дальше которого объект не отрисовывается")]
        [SerializeField]
        private float _farClipPlane = 1000f;
        
        [FoldoutGroup("Lens")]
        [LabelText("Dutch")]
        [PropertyTooltip("Угол наклона камеры вокруг оси Z")]
        [SerializeField]
        private float _dutch = 0;

        [FoldoutGroup("Lens")]
        [LabelText("ModeOverride")]
        [PropertyTooltip("Позволяет принудительно задать режим камеры Unity")]
        [SerializeField]
        private LensSettings.OverrideModes _modeOverride = LensSettings.OverrideModes.None;
        
        public OutputChannels OutputChannel => _outputChannel;
        public CinemachineVirtualCameraBase.StandbyUpdateMode StandbyUpdateMode => _standbyUpdateMode;
        public float FieldOfView => _fieldOfView;
        public float NearClipPlane => _nearClipPlane;
        public float FarClipPlane => _farClipPlane;
        public float Dutch => _dutch;
        public LensSettings.OverrideModes ModeOverride => _modeOverride;
    }
}