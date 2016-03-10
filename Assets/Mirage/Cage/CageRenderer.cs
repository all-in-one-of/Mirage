using UnityEngine;
using UnityEngine.Rendering;

namespace Mirage
{
    [ExecuteInEditMode]
    public class CageRenderer : MonoBehaviour
    {
        #region Exposed Parameters

        [SerializeField]
        CageMesh _mesh;

        [Space]
        [SerializeField]
        float _radius = 1.0f;

        [SerializeField]
        float _noiseAmplitude = 0.05f;

        [SerializeField]
        float _noiseFrequency = 1.0f;

        [SerializeField]
        float _noiseMotion = 0.2f;

        [Space]
        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
        Color _lineColor = Color.white;

        #endregion

        #region Private Resources

        [SerializeField, HideInInspector]
        Shader _lineShader;

        #endregion

        #region Private Variables

        Material _lineMaterial;
        Vector3 _noiseOffset;

        #endregion

        #region MonoBehaviour Functions

        void Update()
        {
            if (_lineMaterial == null)
            {
                _lineMaterial = new Material(_lineShader);
                _lineMaterial.hideFlags = HideFlags.DontSave;
            }

            _noiseOffset += new Vector3(0.13f, 0.82f, 0.11f) * _noiseMotion * Time.deltaTime;

            _lineMaterial.color = _lineColor;
            _lineMaterial.SetFloat("_Radius", _radius * 1.05f);

            _lineMaterial.SetFloat("_NoiseAmplitude", _noiseAmplitude);
            _lineMaterial.SetFloat("_NoiseFrequency", _noiseFrequency);
            _lineMaterial.SetVector("_NoiseOffset", _noiseOffset);

            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _lineMaterial, 0
            );
        }

        #endregion
    }
}
