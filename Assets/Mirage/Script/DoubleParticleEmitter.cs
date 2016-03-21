using UnityEngine;

namespace Mirage
{
    class DoubleParticleEmitter : MonoBehaviour
    {
        [Space]
        [SerializeField] Bounds _emissionRange = new Bounds(
            Vector3.zero, Vector3.one
        );

        [Space]
        [SerializeField] ParticleSystem _mainEmitter;
        [SerializeField] int _mainEmissionCount = 1;

        [Space]
        [SerializeField] ParticleSystem _subEmitter;
        [SerializeField] int _subEmissionCount = 1;

        Vector3 RandomPointInRange()
        {
            var r = new Vector3(Random.value, Random.value, Random.value);
            r = Vector3.Scale(r - Vector3.one * 0.5f, _emissionRange.size);
            return r + _emissionRange.center;
        }

        public void Emit()
        {
            var pos = transform.TransformPoint(RandomPointInRange());

            if (_mainEmitter != null) {
                _mainEmitter.transform.position = pos;
                _mainEmitter.Emit(_mainEmissionCount);
            }

            if (_subEmitter != null) {
                _subEmitter.transform.position = pos;
                _subEmitter.Emit(_subEmissionCount);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_emissionRange.center, _emissionRange.size);
        }
    }
}
