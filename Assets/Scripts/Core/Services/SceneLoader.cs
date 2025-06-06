using System.Providers.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    [CreateAssetMenu(menuName = "System/Service/SceneLoader", fileName = "Scene Loader")]
    public class SceneLoader : ScriptableService
    {
        public void LoadingScene(string sceneName)
        {
            UniTask.Void(async () => await LoadSceneAsync("Boot"));
            UniTask.Void(async () => await LoadSceneAsync(sceneName));
        }

        private async UniTask LoadSceneAsync(string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName) return;

            AsyncOperation delay = SceneManager.LoadSceneAsync(sceneName);

            while (!delay.isDone)
            {
                await UniTask.Yield();

                if (delay.progress >= 90)
                {
                    delay.allowSceneActivation = true;
                }
            }
        }
    }
}