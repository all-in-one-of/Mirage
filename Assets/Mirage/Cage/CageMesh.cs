using UnityEngine;
using System.Collections.Generic;

namespace Mirage
{
    public class CageMesh : ScriptableObject
    {
        #region Public Properties

        [SerializeField, Range(0, 5)]
        int _subdivisionLevel = 1;

        public int subdivisionLevel {
            get { return _subdivisionLevel; }
        }

        [SerializeField, HideInInspector]
        Mesh _mesh;

        public Mesh sharedMesh {
            get { return _mesh; }
        }

        #endregion

        #region Public Methods

        public void RebuildMesh()
        {
            if (_mesh == null)
            {
                Debug.LogError("Mesh asset is missing.");
                return;
            }

            _mesh.Clear();

            var builder = new Emgen.IcosphereBuilder();
            for (var i = 0; i < _subdivisionLevel; i++)
                builder.Subdivide();

            var vcache = builder.vertexCache;

            _mesh.vertices = vcache.MakeVertexArrayForLineMesh();

            _mesh.SetIndices(
                vcache.MakeIndexArrayForLineMesh(), MeshTopology.Lines, 0
            );

            _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10);

            _mesh.Optimize();
            _mesh.UploadMeshData(true);
        }

        #endregion

        #region ScriptableObject Functions

        void OnEnable()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "Cage";
            }
        }

        #endregion
    }
}
