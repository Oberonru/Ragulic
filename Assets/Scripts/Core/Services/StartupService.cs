using System.Interfaces;
using System.Providers.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    [CreateAssetMenu(menuName = "System/Service/StartupService", fileName = "Startup")]
    public class StartupService : ScriptableService, IInitialize
    {
        [Header("Device frame rate")] [SerializeField, MinValue(30)]
        private int _frameRate = 60;

        public void Initialize()
        {
#if UNITY_EDITOR
            SceneManager.LoadScene("Boot");
#endif

            Application.targetFrameRate = _frameRate;
        }
    }
}