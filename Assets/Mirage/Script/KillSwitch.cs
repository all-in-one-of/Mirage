using UnityEngine;
using UnityEngine.UI;

namespace Mirage
{
    public class KillSwitch : MonoBehaviour
    {
        [SerializeField] Slider[] _slidersToKill;

        public void KillAllEffects()
        {
            foreach (var slider in _slidersToKill) slider.value = 0;
        }
    }
}
