using UnityEngine;

namespace Mirage
{
    public class Configurator : MonoBehaviour
    {
        [SerializeField] Camera[] _cameras;

        void Start()
        {
            if (!Application.isEditor)
            {
                // Hide mouse cursor.
                Cursor.visible = false;

                #if MIRAGE_SINGLE

                // Remap displays for the single display mode.
                _cameras[0].enabled = false;
                _cameras[1].targetDisplay = 0;
                _cameras[2].enabled = false;

                #elif MIRAGE_DUAL

                // Remap displays for the dual display mode.
                _cameras[0].enabled = false;
                _cameras[1].targetDisplay = 0;
                _cameras[2].targetDisplay = 1;

                // Try activating multiple displays.
                TryActivateDisplay(0);
                TryActivateDisplay(1);

                #else

                // Try activating multiple displays.
                TryActivateDisplay(0);
                TryActivateDisplay(1);
                TryActivateDisplay(2);

                #endif
            }
        }

        void TryActivateDisplay(int index)
        {
            if (index < Display.displays.Length)
                Display.displays[index].Activate();
        }
    }
}
