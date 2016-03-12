using UnityEngine;
using UnityEngine.Rendering;

namespace Mirage
{
    [ExecuteInEditMode]
    public class CageRenderer : MonoBehaviour
    {
        #region Exposed Parameters

        [SerializeField]
        float _radius = 1.0f;

        public float radius {
            get { return _radius; }
            set { _radius = value; }
        }

        [SerializeField]
        float _noiseAmplitude = 0.05f;

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        [SerializeField]
        float _noiseFrequency = 1.0f;

        public float noiseFrequency {
            get { return _noiseFrequency; }
            set { _noiseFrequency = value; }
        }

        [SerializeField]
        float _noiseMotion = 0.2f;

        public float noiseMotion {
            get { return _noiseMotion; }
            set { _noiseMotion = value; }
        }

        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
        Color _lineColor = Color.white;

        public Color lineColor {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        #endregion

        #region Private Resources

        [SerializeField, HideInInspector]
        CageMesh _mesh;

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
