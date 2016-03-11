using UnityEngine;
using UnityEngine.Rendering;
using Klak.MaterialExtension;

namespace Mirage
{
    [ExecuteInEditMode]
    public class LucyRenderer : MonoBehaviour
    {
        #region Exposed properties

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

        Material material {
            get {
                if (_material == null) {
                    var shader = Shader.Find("Hidden/Mirage/Lucy/Disintegration");
                    _material = new Material(shader);
                    _material.hideFlags = HideFlags.DontSave;
                }
                return _material;
            }
        }

        Material _material;

        [SerializeField, HideInInspector] Shader _shader;
        [SerializeField, HideInInspector] Mesh _mesh1;
        [SerializeField, HideInInspector] Mesh _mesh2;

        #endregion

        #region MonoBehaviour Functions

        void OnDisable()
        {
            if (_material != null) DestroyImmediate(_material);
            _material = null;
        }

        void Update()
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

            var rotation =
                transform.localRotation * Quaternion.Euler(-90, -180, 0);

            var matrix = Matrix4x4.TRS(
                transform.position, rotation, transform.localScale
            );

            Graphics.DrawMesh(_mesh1, matrix, material, 0);
            Graphics.DrawMesh(_mesh2, matrix, material, 0);
        }

        #endregion
    }
}
