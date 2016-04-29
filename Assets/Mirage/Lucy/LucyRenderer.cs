using UnityEngine;
using UnityEngine.Rendering;
using Klak.MaterialExtension;
using Klak.Math;

namespace Mirage
{
    [ExecuteInEditMode]
    public class LucyRenderer : MonoBehaviour
    {
        #region Public properties and methods

        public float bend { get; set; }

        public void ChangeBendDirection()
        {
            _bendAngle = Random.value * Mathf.PI * 2;
        }

        public float twist { get; set; }

        public void InvertTwistDirection()
        {
            _twistDirection *= -1;
        }

        public float spike { get; set; }

        public void ChangeSpikeSeed()
        {
            _spikeSeed = Random.value;
        }

        public float voxel { get; set; }

        #endregion

        #region Editable properties

        [Space]
        [SerializeField, ColorUsage(false)] Color _color1 = Color.white;
        [SerializeField, ColorUsage(false)] Color _color2 = Color.black;
        [SerializeField] Texture _colorMap;

        [Space]
        [SerializeField, Range(0, 1)] float _smoothness;
        [SerializeField, Range(0, 1)] float _metallic;

        [Space]
        [SerializeField] Texture _normalMap;
        [SerializeField, Range(0, 2)] float _normalScale = 1;

        [Space]
        [SerializeField] Texture _occlusionMap;
        [SerializeField, Range(0, 1)] float _occlusionStrength = 1;
        [SerializeField, Range(0, 5)] float _occlusionContrast = 1;

        #endregion

        #region Private properties

        Material _defaultMaterial;
        [SerializeField, HideInInspector] Mesh _defaultMesh;
        [SerializeField, HideInInspector] Shader _defaultShader;

        Material _splitMaterial;
        [SerializeField, HideInInspector] Mesh _splitMesh1;
        [SerializeField, HideInInspector] Mesh _splitMesh2;
        [SerializeField, HideInInspector] Shader _splitMeshShader;

        MaterialPropertyBlock _mpblock;
        bool _initialized;

        float _splitTimer;
        float _bendAngle;
        float _twistDirection = 1;
        float _spikeSeed;

        Matrix4x4 transformMatrix {
            get {
                var baseRotation = Quaternion.Euler(-90, -180, 0);
                return Matrix4x4.TRS(
                    transform.position,
                    transform.rotation * baseRotation,
                    transform.lossyScale
                );
            }
        }

        #endregion

        #region Private methods

        static bool IsAlmostZero(float x)
        {
            return Mathf.Abs(x) < 0.001f;
        }

        void InitializeResources()
        {
            _defaultMaterial = new Material(_defaultShader);
            _defaultMaterial.hideFlags = HideFlags.DontSave;
            ApplyParametersToMaterial(_defaultMaterial);

            _splitMaterial = new Material(_splitMeshShader);
            _splitMaterial.hideFlags = HideFlags.DontSave;
            ApplyParametersToMaterial(_splitMaterial);

            _mpblock = new MaterialPropertyBlock();

            _initialized = true;
        }

        void ApplyParametersToMaterial(Material material)
        {
            material.
                Property("_Color1", _color1).
                Property("_Color2", _color2).
                Property("_ColorMap", _colorMap).
                Property("_Glossiness", _smoothness).
                Property("_Metallic", _metallic).
                Property("_BumpMap", _normalMap).
                Property("_BumpScale", _normalScale).
                Property("_OcclusionMap", _occlusionMap).
                Property("_OcclusionStrength", _occlusionStrength).
                Property("_OcclusionContrast", _occlusionContrast);
        }

        #endregion

        #region MonoBehaviour functions

        void OnDisable()
        {
            if (_initialized)
            {
                DestroyImmediate(_defaultMaterial);
                _defaultMaterial = null;

                DestroyImmediate(_splitMaterial);
                _splitMaterial = null;

                _initialized = false;
            }
        }

        void Update()
        {
            if (!_initialized) InitializeResources();

            if (IsAlmostZero(bend) &&
                IsAlmostZero(twist) &&
                IsAlmostZero(spike) &&
                IsAlmostZero(voxel))
            {
                _splitTimer -= Time.deltaTime;
            }
            else
            {
                _splitTimer = 4;
            }

            if (_splitTimer > 0)
            {
                var bendAxes = new Vector4(
                    Mathf.Cos(_bendAngle), -Mathf.Sin(_bendAngle),
                    Mathf.Sin(_bendAngle),  Mathf.Cos(_bendAngle)
                );
                var bendDist = 1 / (bend + 1e-6f);

                _mpblock.
                    Property("_Bend", bendDist).
                    Property("_Axes", bendAxes).
                    Property("_Twist", twist * _twistDirection * 3).
                    Property("_Spike", spike * 2, 0.003f, _spikeSeed).
                    Property("_Voxel", 4 / (voxel + 1e-6f));

                var matrix = transformMatrix;

                Graphics.DrawMesh(
                    _splitMesh1, matrix,
                    _splitMaterial, gameObject.layer, null, 0, _mpblock
                );

                Graphics.DrawMesh(
                    _splitMesh2, matrix,
                    _splitMaterial, gameObject.layer, null, 0, _mpblock
                );
            }
            else
            {
                Graphics.DrawMesh(
                    _defaultMesh, transformMatrix,
                    _defaultMaterial, gameObject.layer, null, 0, _mpblock
                );
            }
        }

        #endregion
    }
}
