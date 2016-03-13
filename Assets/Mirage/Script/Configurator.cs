using UnityEngine;

namespace Mirage
{
    public class Configurator : MonoBehaviour
    {
        void Start()
        {
            if (!Application.isEditor)
            {
                // Hide mouse cursor.
                Cursor.visible = false;

                // Activate all the displays.
                for (var i = 1; i < Display.displays.Length; i++)
                    Display.displays[i].Activate();
            }
        }
    }
}
