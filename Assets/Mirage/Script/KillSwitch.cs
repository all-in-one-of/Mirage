using UnityEngine;
using UnityEngine.UI;

namespace Mirage
{
    public class KillSwitch : MonoBehaviour
    {
        [SerializeField] Slider[] _slidersToKill;
        [SerializeField] Button[] _buttonsToClick;

        public void KillAllEffects()
        {
            foreach (var slider in _slidersToKill) slider.value = 0;
            foreach (var button in _buttonsToClick) button.onClick.Invoke();
        }
    }
}
