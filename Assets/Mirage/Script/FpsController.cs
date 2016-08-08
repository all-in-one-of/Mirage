using UnityEngine;
using System.Collections;

namespace Mirage
{
    // Does some weird things to circumvent the multi-display v-sync problem.
    public class FpsController : MonoBehaviour
    {
        void Start()
        {
            if (QualitySettings.vSyncCount < 1)
            {
                var fps = 60;

                // Try to limit the frame rate when vsync is disabled.
                // Not sure if it's actually effective though.
                Application.targetFrameRate = fps;

                // Change the history length of Swarms.
                foreach (var swarm in FindObjectsOfType<Kvant.Swarm>())
                    swarm.historyLength = swarm.historyLength * fps / 60;
            }
        }
    }
}
