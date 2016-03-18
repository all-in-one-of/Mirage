using UnityEngine;

namespace Mirage
{
    public class LatticeController : MonoBehaviour
    {
        public float transition { get; set; }

        Kvant.Lattice _lattice;
        Color _surfaceColor;
        Color _lineColor;

        void Start()
        {
            _lattice = GetComponent<Kvant.Lattice>();
            _surfaceColor = _lattice.material.color;
            _lineColor = _lattice.lineColor;
        }

        void Update()
        {
            if (transition > 0.01f)
            {
                _lattice.enabled = true;

                _lattice.material.color = Color.Lerp(Color.white, _surfaceColor, transition);

                var lc = _lineColor;
                lc.a = transition;
                _lattice.lineColor = lc;
            }
            else
            {
                _lattice.enabled = false;
            }
        }
    }
}
