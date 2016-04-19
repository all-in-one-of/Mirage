using UnityEngine;

namespace Mirage
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] float _pointLightIntensity;
        [SerializeField, Range(0, 1)] float _frontLightIntensity;
        [SerializeField, Range(0, 1)] float _spotLightIntensity;
        [SerializeField, Range(0, 1)] float _spotLightAngle;

        [Space]

        [SerializeField, Range(0, 1)] float _swarm1Throttle;
        [SerializeField, Range(0, 1)] float _swarm2Throttle;
        [SerializeField, Range(0, 1)] float _swarm3Throttle;

        [Space]

        [SerializeField, Range(0, 1)] float _dustThrottle;
        [SerializeField, Range(0, 1)] float _shardsThrottle;
        [SerializeField, Range(0, 1)] float _rocksThrottle;

        [Space]

        [SerializeField] Light _pointLight;
        [SerializeField] Light _frontLight;
        [SerializeField] Light[] _spotLights;
        [SerializeField] Transform[] _spotLightPivots;

        [Space]

        [SerializeField] Kvant.Swarm _swarm1;
        [SerializeField] Kvant.Swarm _swarm2;
        [SerializeField] Kvant.Swarm _swarm3;

        [Space]

        [SerializeField] Kvant.Spray _dust;
        [SerializeField] Kvant.Spray _shards;
        [SerializeField] Kvant.Spray _rocks;

        public void KickLightEnv()
        {
        }

        public void RandomizeLightColor()
        {
        }

        public void ResetLightColor()
        {
        }

        void Update()
        {
        }
    }
}
