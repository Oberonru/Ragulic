using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Share.Configs;
using UnityEngine;
using Zenject;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace Share.Animations
{
    public class AlarmViewAnimation : MonoBehaviour
    {
        [Inject] private AlarmViewAnimationConfig _config;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _alarmInfo;

        private Sequence _startSequence;
        private Sequence _endSequence;

        private async void OnEnable()
        {
            _endSequence?.Kill();
            _startSequence?.Kill();
            _icon.DOFade(0, 0);
            _alarmInfo.DOFade(0, 0);

            _startSequence = DOTween.Sequence();
            _startSequence.Append(_icon.DOFade(1, _config.Speed / 2));
            _startSequence.Join(_alarmInfo.DOFade(1, _config.Speed / 2));

            _startSequence.OnComplete(() => PlayFade());
        }

        private async void PlayFade()
        {
            var token = this.GetCancellationTokenOnDestroy();
            TimeSpan span = TimeSpan.FromSeconds(_config.Duration);

            await UniTask.Delay(span, cancellationToken: token);

            _endSequence = DOTween.Sequence();
            _endSequence.Append(_icon.DOFade(0, _config.Speed));
            _endSequence.Join(_alarmInfo.DOFade(0, _config.Speed));

            _endSequence.SetEase(_config.Ease);

            _endSequence.Play();
            _endSequence.OnComplete(() => Deactivate());
        }

        private async void Deactivate()
        {
            var token = this.GetCancellationTokenOnDestroy();
            TimeSpan span = TimeSpan.FromSeconds(_config.DelayDeactivate);

            await UniTask.Delay(span, cancellationToken: token);
            gameObject.SetActive(false);
        }
    }
}