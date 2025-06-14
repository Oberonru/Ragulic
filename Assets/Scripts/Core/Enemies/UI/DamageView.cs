using System.Collections;
using Core.BaseComponents;
using TMPro;
using UniRx;
using UnityEngine;

namespace Core.Enemies.UI
{
    public class DamageView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _damage;
        [SerializeField] private HealthComponent _health;
        private bool _isActive;
        private Coroutine _coroutine;

        private void OnEnable()
        {
            _health.OnHealthChanged.Subscribe(
                amount =>
                {
                    if (_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                    }

                    _coroutine = StartCoroutine(FadeInOut(1f, amount));
                }).AddTo(this);
        }

        private IEnumerator FadeInOut(float duration, int amount)
        {
            _damage.text = "-" + amount;
            var elapsed = 0f;
            Color color = _damage.color;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                _damage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                _damage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }
    }
}