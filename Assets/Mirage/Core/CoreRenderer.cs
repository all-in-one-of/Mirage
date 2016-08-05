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

        [Space] // Dissolve animation

        [SerializeField, Range(0, 1)] float _dissolveLevel = 0.5f;

        public float dissolveLevel {
            get { return _dissolveLevel; }
            set { _dissolveLevel = value; }
        }

        [SerializeField] float _dissolveFrequency = 0.5f;

        public float dissolveFrequency {
            get { return _dissolveFrequency; }
            set { _dissolveFrequency = value; }
        }

        [SerializeField] float _dissolveMotion = 2;

        public float dissolveMotion {
            get { return _dissolveMotion; }
            set { _dissolveMotion = value; }
        }

        [Space] // Material properties

        [SerializeField, ColorUsage(false)]
        Color _color = Color.white;

        public Color color {
            get { return _color; }
            set { _color = value; }
        }

        [SerializeField, ColorUsage(false)]
        Color _backColor = Color.gray;

        public Color backColor {
            get { return _backColor; }
            set { _backColor = value; }
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

        #endregion

        #region Private resources

        [SerializeField, HideInInspector] CoreMesh _mesh;
        [SerializeField, HideInInspector] Shader _shader;

        #endregion

        #region Private variables

        Material _frontMaterial;
        Material _backMaterial;
        MaterialPropertyBlock _materialProps;

        Vector3 _spikeOffset;
        Vector3 _dissolveOffset;

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            var shader = Shader.Find("Hidden/Mirage/Core");

            _frontMaterial = new Material(shader);
            _frontMaterial.hideFlags = HideFlags.DontSave;
            _frontMaterial.Property("_Flip", 1.0f);

            _backMaterial = new Material(shader);
            _backMaterial.hideFlags = HideFlags.DontSave;
            _backMaterial.Property("_Flip", -1.0f);

            _materialProps = new MaterialPropertyBlock();
        }

        void OnDisable()
        {
            if (_frontMaterial != null) DestroyImmediate(_frontMaterial);
            if (_backMaterial != null) DestroyImmediate(_backMaterial);

            _frontMaterial = null;
            _backMaterial = null;
            _materialProps = null;
        }

        void Update()
        {
            // State update
            var spikeDir = new Vector3(1, 0.5f, 0.2f).normalized;
            var dissolveDir = new Vector3(-0.3f, -1, -0.1f).normalized;

            _spikeOffset += spikeDir * (_spikeMotion * Time.deltaTime);
            _dissolveOffset += dissolveDir * (_dissolveMotion * Time.deltaTime);
        }

        void LateUpdate()
        {
            // Material setup
            _materialProps.
                Property("_SpikeOffset", _spikeOffset).
                Property("_SpikeAmplitude", _spikeAmplitude).
                Property("_SpikeExponent", _spikeExponent).
                Property("_SpikeFrequency", _spikeFrequency).
                Property("_DissolveOffset", _dissolveOffset).
                Property("_DissolveLevel", _dissolveLevel).
                Property("_DissolveFrequency", _dissolveFrequency).
                Property("_Glossiness", _smoothness).
                Property("_Metallic", _metallic);

            // Draw call
            var matrix = transform.localToWorldMatrix;
            matrix *= Matrix4x4.Scale(Vector3.one * _radius);

            _backMaterial.
                Property("_CullMode", (int)CullMode.Front).
                Property("_Color", _backColor);

            Graphics.DrawMesh(
                _mesh.sharedMesh, matrix, _backMaterial, gameObject.layer,
                null, 0, _materialProps
            );

            _frontMaterial.
                Property("_CullMode", (int)CullMode.Back).
                Property("_Color", _color).
                Property("_EmissionColor", _emission);

            Graphics.DrawMesh(
                _mesh.sharedMesh, matrix, _frontMaterial, gameObject.layer,
                null, 0, _materialProps
            );
        }

        #endregion
    }
}
