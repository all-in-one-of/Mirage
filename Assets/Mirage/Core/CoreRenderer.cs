using UnityEngine;
using UnityEngine.Rendering;
using Klak.MaterialExtension;

namespace Mirage
{
    [ExecuteInEditMode]
    public class CoreRenderer : MonoBehaviour
    {

        #region Exposed parameters

        /// Radius of sphere
        public float radius {
            get { return _radius; }
            set { _radius = value; }
        }

        [SerializeField] float _radius = 1.0f;

        /// Spike amplitude
        public float spikeAmplitude {
            get { return _spikeAmplitude; }
            set { _spikeAmplitude = value; }
        }

        [Space, SerializeField] float _spikeAmplitude = 8;

        /// Exponent coefficient of spike amplitude
        public float spikeExponent {
            get { return _spikeExponent; }
            set { _spikeExponent = value; }
        }

        [SerializeField] float _spikeExponent = 8;

        /// Frequency of spikes
        public float spikeFrequency {
            get { return _spikeFrequency; }
            set { _spikeFrequency = value; }
        }

        [SerializeField] float _spikeFrequency = 2;

        /// Animation speed of spikes
        public float spikeMotion {
            get { return _spikeMotion; }
            set { _spikeMotion = value; }
        }

        [SerializeField] float _spikeMotion = 2;

        /// Frequency of noise mask
        public float maskFrequency {
            get { return _maskFrequency; }
            set { _maskFrequency = value; }
        }

        [Space, SerializeField] float _maskFrequency = 0.5f;

        /// Animation speed of noise mask
        public float maskMotion {
            get { return _maskMotion; }
            set { _maskMotion = value; }
        }

        [SerializeField] float _maskMotion = 2;

        /// Line color
        public Color lineColor {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        [Space, SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
        Color _lineColor = Color.white;

        /// Surface color
        public Color surfaceColor {
            get { return _surfaceColor; }
            set { _surfaceColor = value; }
        }

        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
        Color _surfaceColor = Color.white;

        /// Alpha cutoff level
        public float cutoff {
            get { return _cutoff; }
            set { _cutoff = value; }
        }

        [SerializeField, Range(0, 1)] float _cutoff = 0.5f;

        #endregion

        #region Private resources

        [SerializeField, HideInInspector] CoreMesh _mesh;
        [SerializeField, HideInInspector] Shader _lineShader;
        [SerializeField, HideInInspector] Shader _surfaceShader;

        #endregion

        #region Private variables

        Material _lineMaterial;
        Material _surfaceMaterial;
        MaterialPropertyBlock _shaderArgs;

        Vector3 _spikeOffset;
        Vector3 _maskOffset;

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            var lineShader = Shader.Find("Hidden/Mirage/Core/Line");
            _lineMaterial = new Material(lineShader);
            _lineMaterial.hideFlags = HideFlags.DontSave;

            var surfaceShader = Shader.Find("Hidden/Mirage/Core/Surface");
            _surfaceMaterial = new Material(surfaceShader);
            _surfaceMaterial.hideFlags = HideFlags.DontSave;

            _shaderArgs = new MaterialPropertyBlock();
        }

        void OnDisable()
        {
            if (_lineMaterial != null)
                DestroyImmediate(_lineMaterial);

            if (_surfaceMaterial != null)
                DestroyImmediate(_surfaceMaterial);

            _lineMaterial = null;
            _surfaceMaterial = null;
            _shaderArgs = null;
        }

        void Update()
        {
            // State update
            var spikeDir = new Vector3(1, 0.5f, 0.2f).normalized;
            var maskDir = new Vector3(-0.3f, -1, -0.1f).normalized;

            _spikeOffset += spikeDir * (_spikeMotion * Time.deltaTime);
            _maskOffset += maskDir * (_maskMotion * Time.deltaTime);

            // Material setup
            _shaderArgs.
                Property("_Cutoff", _cutoff).
                Property("_SpikeOffset", _spikeOffset).
                Property("_SpikeAmplitude", _spikeAmplitude).
                Property("_SpikeExponent", _spikeExponent).
                Property("_SpikeFrequency", _spikeFrequency).
                Property("_MaskOffset", _maskOffset).
                Property("_MaskFrequency", _maskFrequency);

            _surfaceMaterial.color = _surfaceColor;
            _lineMaterial.color = _lineColor;

            // Draw call
            var matrix = transform.localToWorldMatrix;
            var scale1 = Matrix4x4.Scale(Vector3.one * (_radius - 0.01f));
            var scale2 = Matrix4x4.Scale(Vector3.one * _radius);

            Graphics.DrawMesh(
                _mesh.sharedMesh, matrix * scale1, _surfaceMaterial,
                gameObject.layer, null, 0, _shaderArgs
            );

            Graphics.DrawMesh(
                _mesh.sharedMesh, matrix * scale2, _lineMaterial,
                gameObject.layer, null, 1, _shaderArgs
            );
        }

        #endregion
    }
}
