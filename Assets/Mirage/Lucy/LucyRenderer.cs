using UnityEngine;
using UnityEngine.Rendering;
using Klak.MaterialExtension;

namespace Mirage
{
    [ExecuteInEditMode]
    public class LucyRenderer : MonoBehaviour
    {
        #region Public properties

        [SerializeField] bool _split;

        public bool split {
            get { return _split; }
            set { _split = value; }
        }

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

        [Space]
        [SerializeField, ColorUsage(false)] Color _backColor = Color.red;
        [SerializeField, Range(0, 1)] float _backSmoothness;
        [SerializeField, Range(0, 1)] float _backMetallic;

        #endregion

        #region Private properties

        bool _initialized;

        Material _unifiedMaterial;
        [SerializeField, HideInInspector] Mesh _unifiedMesh;
        [SerializeField, HideInInspector] Shader _unifiedMeshShader;

        Material _splitMaterial;
        [SerializeField, HideInInspector] Mesh _splitMesh1;
        [SerializeField, HideInInspector] Mesh _splitMesh2;
        [SerializeField, HideInInspector] Shader _splitMeshShader;

        Matrix4x4 transformMatrix {
            get {
                return Matrix4x4.TRS(
                    transform.position,
                    transform.localRotation * Quaternion.Euler(-90, -180, 0),
                    transform.localScale
                );
            }
        }

        #endregion

        #region Private methods

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
                Property("_OcclusionContrast", _occlusionContrast).
                Property("_BackColor", _backColor).
                Property("_BackGlossiness", _backSmoothness).
                Property("_BackMetallic", _backMetallic);
        }

        void InitializeResources()
        {
            _unifiedMaterial = new Material(_unifiedMeshShader);
            _unifiedMaterial.hideFlags = HideFlags.DontSave;
            ApplyParametersToMaterial(_unifiedMaterial);

            _splitMaterial = new Material(_splitMeshShader);
            _splitMaterial.hideFlags = HideFlags.DontSave;
            ApplyParametersToMaterial(_splitMaterial);

            _initialized = true;
        }

        #endregion

        #region MonoBehaviour functions

        void OnDisable()
        {
            if (_initialized)
            {
                DestroyImmediate(_unifiedMaterial);
                _unifiedMaterial = null;

                DestroyImmediate(_splitMaterial);
                _splitMaterial = null;

                _initialized = false;
            }
        }

        void Update()
        {
            if (!_initialized) InitializeResources();

            var matrix = transformMatrix;

            if (_split)
            {
                Graphics.DrawMesh(_splitMesh1, matrix, _splitMaterial, 0);
                Graphics.DrawMesh(_splitMesh2, matrix, _splitMaterial, 0);
            }
            else
            {
                Graphics.DrawMesh(_unifiedMesh, matrix, _unifiedMaterial, 0);
            }
        }

        #endregion
    }
}
