using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Share.Configs;
using UnityEngine;
using Zenject;

namespace Share.Animations
{
    public class AlarmViewAnimation : MonoBehaviour
    {
        [Inject] private AlarmViewAnimationConfig _config;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Sequence _startSequence;
        private Sequence _endSequence;

        private async void OnEnable()
        {
            _endSequence?.Kill();
            _startSequence?.Kill();
            _canvasGroup.alpha = 0;

            _startSequence = DOTween.Sequence();
            _startSequence.Append(_canvasGroup.DOFade(1, _config.Speed / 2));
            _startSequence.OnComplete(() => PlayFade());
        }

        private async void PlayFade()
        {
            var token = this.GetCancellationTokenOnDestroy();
            TimeSpan span = TimeSpan.FromSeconds(_config.Duration);
            await UniTask.Delay(span, cancellationToken: token);

            _endSequence = DOTween.Sequence();
            _endSequence.Append(_canvasGroup.DOFade(0, _config.Speed));
            _endSequence.SetEase(_config.Ease);
            _endSequence.Play();
            _endSequence.OnComplete(() => Deactivate());
        }

        private async void Deactivate()
        {
            try
            {
                var token = this.GetCancellationTokenOnDestroy();
                TimeSpan span = TimeSpan.FromSeconds(_config.DelayDeactivate);

                await UniTask.Delay(span, cancellationToken: token);
                gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                throw new Exception("Deactivate failed", e);
            }
        }
    }
}