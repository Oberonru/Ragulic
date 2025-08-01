using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class CircleSlider : MonoBehaviour
    {
        [SerializeField] private Image _circleImage;

        public void SetValue(float currentTime, float maxTime)
        {
            _circleImage.fillAmount = Mathf.Clamp01(currentTime / maxTime);
        }
    }
}