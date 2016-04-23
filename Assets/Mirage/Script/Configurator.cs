using UnityEngine;

namespace Mirage
{
    public class Configurator : MonoBehaviour
    {
        [SerializeField] Camera[] _cameras;

        void Start()
        {
            if (Application.isEditor)
            {
                _cameras[0].targetDisplay = 0;
                _cameras[1].targetDisplay = 1;
            }
            else
            {
                // Hide mouse cursor.
                Cursor.visible = false;

                #if MIRAGE_SINGLE

                // Single mode: disable the secondary camera.
                _cameras[1].enabled = false;
                TryActivateDisplay(0);

                #elif MIRAGE_DUAL

                // Dual mode: remap cameras.
                _cameras[0].targetDisplay = 0;
                _cameras[1].targetDisplay = 1;
                TryActivateDisplay(0);
                TryActivateDisplay(1);

                #else

                // Triple mode: remap cameras.
                // The 1st display is not in use.
                _cameras[0].targetDisplay = 1;
                _cameras[1].targetDisplay = 2;
                TryActivateDisplay(1);
                TryActivateDisplay(2);

                #endif
            }
        }

        void TryActivateDisplay(int index)
        {
            if (index < Display.displays.Length)
            {
                Display.displays[index].Activate();
                CreateClearOnlyCamera(index);
            }
        }

        void CreateClearOnlyCamera(int index)
        {
            var go = new GameObject("Clear Camera " + index);
            var cam = go.AddComponent<Camera>();
            cam.depth = -1000;
            cam.cullingMask = 0;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.targetDisplay = index;
        }
    }
}
