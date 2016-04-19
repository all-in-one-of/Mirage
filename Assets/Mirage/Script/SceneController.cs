using UnityEngine;

namespace Mirage
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] float _pointLightIntensity;
        [SerializeField, Range(0, 1)] float _frontLightIntensity;
        [SerializeField, Range(0, 1)] float _spotLightIntensity;
        [SerializeField, Range(0, 1)] float _spotLightAngle;
        [SerializeField, Range(0, 1)] float _lightEnvelope;

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

        const float _beatInterval = 60.0f / 140 * 2;

        public void ResetSwarm(int index)
        {
            if (index == 0) _swarm1.Restart();
            if (index == 1) _swarm2.Restart();
            if (index == 2) _swarm3.Restart();
        }

        void Update()
        {
            var env = 1.0f - Time.time / _beatInterval % 1.0f;
            env *= _lightEnvelope;

            _pointLight.intensity = _pointLightIntensity * 2.2f + env;
            _frontLight.intensity = _frontLightIntensity * 1.2f + env;

            foreach (var l in _spotLights)
                l.intensity = _spotLightIntensity * 2.5f + env;

            var angle = Mathf.Lerp(-5.0f, 60.0f, _spotLightAngle);
            _spotLightPivots[0].rotation = Quaternion.Euler(angle, -9, 0);
            _spotLightPivots[1].rotation = Quaternion.Euler(angle, +9, 0);

            _swarm1.throttle = _swarm1Throttle;
            _swarm2.throttle = _swarm2Throttle;
            _swarm3.throttle = _swarm3Throttle;

            _dust.throttle = _dustThrottle;
            _shards.throttle = _shardsThrottle;
            _rocks.throttle = _rocksThrottle;
        }
    }
}
