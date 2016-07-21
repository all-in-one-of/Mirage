using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Mirage
{
    public static class LucyMeshEditor
    {
        static Object[] SelectedMeshes {
            get {
                return Selection.GetFiltered(typeof(Mesh), SelectionMode.Deep);
            }
        }

        static string NewFileName(Mesh mesh, int index)
        {
            var path = AssetDatabase.GetAssetPath(mesh);

            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (Path.GetExtension(path) != "")
                path = path.Replace(Path.GetFileName(path), "");

            return AssetDatabase.GenerateUniqueAssetPath(
                path + "/Lucy Converted " + index + ".asset"
            );
        }

        [MenuItem("Assets/Mirage/Lucy/Convert Mesh", true)]
        static bool ValidateConvertMesh()
        {
            return SelectedMeshes.Length > 0;
        }

        [MenuItem("Assets/Mirage/Lucy/Convert Mesh")]
        static void ConvertMesh()
        {
            foreach (Mesh mesh in SelectedMeshes)
            {
                var newMeshes = ConvertMesh(mesh);
                for (var i = 0; i < newMeshes.Length; i++)
                {
                    var newFilePath = NewFileName(mesh, i);
                    AssetDatabase.CreateAsset(newMeshes[i], newFilePath);
                }
            }
        }

        static Mesh[] ConvertMesh(Mesh source)
        {
            var src_indices   = source.GetIndices(0);
            var src_vertices  = source.vertices;
            var src_normals   = source.normals;
            var src_tangents  = source.tangents;
            var src_texcoords = source.uv;

            var total_vcount = src_indices.Length;
            var meshes = new Mesh[(total_vcount + 65000) / 65001];

            for (var i = 0; i < meshes.Length; i++)
            {
                meshes[i] = BuildMeshPart(
                    src_indices, i * 65001,
                    src_vertices,
                    src_normals,
                    src_tangents,
                    src_texcoords
                );
            }

            return meshes;
        }

        static Mesh BuildMeshPart(
            int[] src_indices, int index_start,
            Vector3[] src_vertices,
            Vector3[] src_normals,
            Vector4[] src_tangents,
            Vector2[] src_texcoords
        )
        {
            var vcount = Mathf.Min(src_indices.Length - index_start, 65001);

            var indices   = new int[vcount];
            var vertices  = new List<Vector3>(vcount);
            var normals   = new List<Vector3>(vcount);
            var tangents  = new List<Vector4>(vcount);
            var texcoords = new List<Vector2>(vcount);
            var vrefs1    = new List<Vector3>(vcount);
            var vrefs2    = new List<Vector3>(vcount);

            for (var i = 0; i < vcount; i++)
                indices[i] = i;

            for (var i = 0; i < vcount; i++)
            {
                var vi = src_indices[index_start + i];
                vertices. Add(src_vertices [vi]);
                normals.  Add(src_normals  [vi]);
                tangents. Add(src_tangents [vi]);
                texcoords.Add(src_texcoords[vi]);
            }

            for (var i = 0; i < vcount; i += 3)
            {
                var v1 = src_vertices[src_indices[index_start + i]];
                var v2 = src_vertices[src_indices[index_start + i + 1]];
                var v3 = src_vertices[src_indices[index_start + i + 2]];

                vrefs1.Add(v2);
                vrefs2.Add(v3);

                vrefs1.Add(v3);
                vrefs2.Add(v1);

                vrefs1.Add(v1);
                vrefs2.Add(v2);
            }

            var mesh = new Mesh();
            mesh.name = "Lucy Converted";
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetTangents(tangents);
            mesh.SetUVs(0, texcoords);
            mesh.SetUVs(1, vrefs1);
            mesh.SetUVs(2, vrefs2);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
