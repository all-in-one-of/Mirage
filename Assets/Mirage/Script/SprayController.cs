using UnityEngine;

namespace Mirage
{
    [RequireComponent(typeof(Kvant.Spray))]
    public class SprayController : MonoBehaviour
    {
        [SerializeField]
        Transform _target;

        [SerializeField, Range(0, 2)]
        float _applyVelocity;

        Kvant.Spray _spray;

        void Start()
        {
            _spray = GetComponent<Kvant.Spray>();
        }

        void Update()
        {
            var delta = _target.position - _spray.emitterCenter;

            _spray.emitterCenter = _target.position;

            if (_applyVelocity > 0)
                _spray.initialVelocity =
                    delta * (_applyVelocity / Time.deltaTime);
        }
    }
}

