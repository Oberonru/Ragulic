using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class FillSlider : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        public void SetSliderValue(float currentValue, float maxValue)
        {
            _fillImage.fillAmount = currentValue / maxValue;
        }
    }
}