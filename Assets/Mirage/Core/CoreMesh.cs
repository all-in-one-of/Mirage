using UnityEngine;
using System.Collections.Generic;

namespace Mirage
{
    public class CoreMesh : ScriptableObject
    {
        #region Public properties

        /// Subdivision level of icosphere
        public int subdivisionLevel {
            get { return _subdivisionLevel; }
        }

        [SerializeField, Range(0, 5)] int _subdivisionLevel = 1;

        /// Shared mesh asset
        public Mesh sharedMesh {
            get { return _mesh; }
        }

        [SerializeField, HideInInspector] Mesh _mesh;

        #endregion

        #region Public methods

        /// Rebuild mesh structure
        /// This function constructs a mesh in a really slow and
        /// memory-intensive way. Never call this at runtime.
        public void RebuildMesh()
        {
            if (_mesh == null) {
                Debug.LogError("Mesh asset is missing.");
                return;
            }

            _mesh.Clear();

            // Build an icosphere with the given subdivision level.
            var builder = new Emgen.IcosphereBuilder();
            for (var i = 0; i < _subdivisionLevel; i++) builder.Subdivide();

            // Make vertex arrays.
            var vc = builder.vertexCache;
            var vcount = 3 * vc.triangles.Count;
            var va1 = new List<Vector3>(vcount); // vertex position
            var va2 = new List<Vector3>(vcount); // previous vertex position
            var va3 = new List<Vector3>(vcount); // next vertex position

            foreach (var t in vc.triangles)
            {
                var v1 = vc.vertices[t.i1];
                var v2 = vc.vertices[t.i2];
                var v3 = vc.vertices[t.i3];

                va1.Add(v1);
                va2.Add(v2);
                va3.Add(v3);

                va1.Add(v2);
                va2.Add(v3);
                va3.Add(v1);

                va1.Add(v3);
                va2.Add(v1);
                va3.Add(v2);
            }

            // Index array for lines
            var lines = new List<int>(2 * vcount);

            for (var i = 0; i < vcount; i += 3)
            {
                lines.Add(i);
                lines.Add(i + 1);
                lines.Add(i + 1);
                lines.Add(i + 2);
                lines.Add(i + 2);
                lines.Add(i);
            }

            // Build a mesh asset.
            _mesh.SetVertices(va1);
            _mesh.SetUVs(0, va2);
            _mesh.SetUVs(1, va3);

            _mesh.subMeshCount = 2;
            _mesh.SetIndices(vc.MakeIndexArrayForFlatMesh(), MeshTopology.Triangles, 0);
            _mesh.SetIndices(lines.ToArray(), MeshTopology.Lines, 1);

            // We have no idea about the bounds, so use a magic number.
            _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 5);

            _mesh.Optimize();
            _mesh.UploadMeshData(true);
        }

        #endregion

        #region ScriptableObject functions

        void OnEnable()
        {
            if (_mesh == null) {
                _mesh = new Mesh();
                _mesh.name = "Core";
            }
        }

        #endregion
    }
}
