using UnityEngine;

namespace Mirage
{
    class ParticleSystemAdapter : MonoBehaviour
    {
        [SerializeField] ParticleSystem _target;

        public float emissionRate { get; set; }

        void Update()
        {
            var em = _target.emission;

            if (emissionRate > 0.01f)
            {
                em.enabled = true;
                em.rate = new ParticleSystem.MinMaxCurve(emissionRate);
            }
            else
            {
                em.enabled = false;
            }
        }
    }
}
