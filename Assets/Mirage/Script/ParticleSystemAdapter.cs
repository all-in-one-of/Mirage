using UnityEngine;

namespace Mirage
{
    class ParticleSystemAdapter : MonoBehaviour
    {
        [SerializeField] ParticleSystem _target;

        public float emissionRate {
            set {
                var em = _target.emission;
                if (value > 0.01f)
                {
                    em.enabled = true;
                    em.rate = new ParticleSystem.MinMaxCurve(value);
                }
                else
                {
                    em.enabled = false;
                }
            }
        }

        public Vector3 minForce {
            set {
                var fol = _target.forceOverLifetime;
                var x = fol.x;
                var y = fol.y;
                var z = fol.z;
                x.constantMin = value.x;
                y.constantMin = value.y;
                z.constantMin = value.z;
                fol.x = x;
                fol.y = y;
                fol.z = z;
            }
        }

        public Vector3 maxForce {
            set {
                var fol = _target.forceOverLifetime;
                var x = fol.x;
                var y = fol.y;
                var z = fol.z;
                x.constantMax = value.x;
                y.constantMax = value.y;
                z.constantMax = value.z;
                fol.x = x;
                fol.y = y;
                fol.z = z;
            }
        }
    }
}
