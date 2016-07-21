using UnityEngine;

namespace Mirage
{
    public class SpinnersRenderer : MonoBehaviour
    {
        #region Exposed properties

        [SerializeField]
        int _columns = 10;

        [SerializeField]
        int _rows = 5;

        [Space]

        [SerializeField]
        float _scale = 0.25f;

        [SerializeField]
        float _interval = 0.3f;

        [Space]

        [SerializeField]
        float _moveDuration = 0.5f;

        [SerializeField]
        int _spinnerCount = 10;

        [Space]

        [SerializeField] Mesh _mesh;
        [SerializeField] Material _material;

        #endregion

        #region Public properties and methods

        public float distortion { get; set; }
        public float roll { get; set; }

        public float scale {
            get { return _scale; }
            set { _scale = value; }
        }

        public void StartNextMove()
        {
            if (enabled && _time >= _moveDuration)
            {
                foreach (var s in _spinners) s.StartNextMove();
                _time = 0;
            }
        }

        #endregion

        #region Private members

        Spinner[] _spinners;
        float _time;

        #endregion

        #region MonoBehaviour functions

        void OnEnable()
        {
            Spinner.columns = _columns;
            Spinner.rows = _rows;

            _spinners = new Spinner[_spinnerCount];

            for (var i = 0; i < _spinnerCount; i++)
                _spinners[i] = new Spinner();

            _time = 1;
        }

        void OnDisable()
        {
            _spinners = null;
        }

        void Update()
        {
            _time += Time.deltaTime;

            var time01 = Mathf.Clamp01(_time / _moveDuration);

            for (var i = 0; i < _spinnerCount; i++)
            {
                var s = _spinners[i];

                var s_t = s.GetPosition(time01) * _interval;
                var s_r = s.GetRotation(time01, roll);
                var s_s = s.GetScale(distortion);

                var matrix = Matrix4x4.TRS(
                    transform.position + transform.rotation * s_t,
                    transform.rotation * s_r,
                    s_s * _scale
                );

                Graphics.DrawMesh(_mesh, matrix, _material, gameObject.layer);
            }
        }

        #endregion
    }
}
