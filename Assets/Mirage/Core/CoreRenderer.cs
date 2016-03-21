using UnityEngine;
using UnityEngine.Rendering;
using Klak.MaterialExtension;

namespace Mirage
{
    [ExecuteInEditMode]
    public class CoreRenderer : MonoBehaviour
    {
        #region Exposed parameters

        [SerializeField] float _radius = 1.0f;

        public float radius {
            get { return _radius; }
            set { _radius = value; }
        }

        [Space] // Spike animation

        [SerializeField] float _spikeAmplitude = 8;

        public float spikeAmplitude {
            get { return _spikeAmplitude; }
            set { _spikeAmplitude = value; }
        }

        [SerializeField] float _spikeExponent = 8;

        public float spikeExponent {
            get { return _spikeExponent; }
            set { _spikeExponent = value; }
        }

        [SerializeField] float _spikeFrequency = 2;

        public float spikeFrequency {
            get { return _spikeFrequency; }
            set { _spikeFrequency = value; }
        }

        [SerializeField] float _spikeMotion = 2;

        public float spikeMotion {
            get { return _spikeMotion; }
            set { _spikeMotion = value; }
        }

        [Space] // Mask animation

        [SerializeField] float _maskFrequency = 0.5f;

        public float maskFrequency {
            get { return _maskFrequency; }
            set { _maskFrequency = value; }
        }

        [SerializeField] float _maskMotion = 2;

        public float maskMotion {
            get { return _maskMotion; }
            set { _maskMotion = value; }
        }

        [Space] // Material properties

        [SerializeField, ColorUsage(true, true, 0, 8, 0.125f, 3)]
        Color _lineColor = Color.white;

        public Color lineColor {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        [SerializeField, ColorUsage(false)]
        Color _surfaceColor = Color.white;

        public Color surfaceColor {
            get { return _surfaceColor; }
            set { _surfaceColor = value; }
        }

        [SerializeField, ColorUsage(false)]
        Color _backSurfaceColor = Color.gray;

        public Color backSurfaceColor {
            get { return _backSurfaceColor; }
            set { _backSurfaceColor = value; }
        }

        [SerializeField, Range(0, 1)] float _smoothness = 0;

        public float smoothness {
            get { return _smoothness; }
            set { _smoothness = value; }
        }

        [SerializeField, Range(0, 1)] float _metallic = 0;

        public float metallic {
            get { return _metallic; }
            set { _metallic = value; }
        }

        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
        Color _emission = Color.black;

        public Color emission {
            get { return _emission; }
            set { _emission = value; }
        }

        [Space] // Additional shader properties

        [SerializeField, Range(0, 1)] float _cutoff = 0.5f;

        public float cutoff {
            get { return _cutoff; }
            set { _cutoff = value; }
        }

        [SerializeField, Range(0, 1)] float _shrink = 0.5f;

        public float shrink {
            get { return _shrink; }
            set { _shrink = value; }
        }

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
                Property("_Shrink", _shrink).
                Property("_SpikeOffset", _spikeOffset).
                Property("_SpikeAmplitude", _spikeAmplitude).
                Property("_SpikeExponent", _spikeExponent).
                Property("_SpikeFrequency", _spikeFrequency).
                Property("_MaskOffset", _maskOffset).
                Property("_MaskFrequency", _maskFrequency);

            _surfaceMaterial.
                Property("_Color", _surfaceColor).
                Property("_BackColor", _backSurfaceColor).
                Property("_Glossiness", _smoothness).
                Property("_Metallic", _metallic).
                Property("_EmissionColor", _emission);

            _lineMaterial.color = _lineColor;

            // Draw call
            var matrix = transform.localToWorldMatrix;
            var scale1 = Matrix4x4.Scale(Vector3.one * (_radius - 0.02f));
            var scale2 = Matrix4x4.Scale(Vector3.one * _radius);

            if (_cutoff < 0.999f)
                Graphics.DrawMesh(
                    _mesh.sharedMesh, matrix * scale1, _surfaceMaterial,
                    gameObject.layer, null, 0, _shaderArgs
                );

            if (_lineColor.a > 0.001f)
                Graphics.DrawMesh(
                    _mesh.sharedMesh, matrix * scale2, _lineMaterial,
                    gameObject.layer, null, 1, _shaderArgs
                );
        }

        #endregion
    }
}
