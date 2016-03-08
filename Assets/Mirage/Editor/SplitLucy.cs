using UnityEngine;
using UnityEditor;

namespace Mirage
{
    public static class LucyEditor
    {
        const string _fbxPath = "Assets/StanfordLucy/Winged Victory.FBX";

        [MenuItem("Mirage/Split Lucy")]
        static void SplitLucy()
        {
            var source = AssetDatabase.LoadAssetAtPath<Mesh>(_fbxPath);

            var iarray0 = source.GetIndices(0);
            var iarray1 = new int[(iarray0.Length + 1) / 2];
            var iarray2 = new int[iarray0.Length / 2];

            System.Array.Copy(iarray0, 0, iarray1, 0, iarray1.Length);
            System.Array.Copy(iarray0, iarray1.Length, iarray2, 0, iarray2.Length);

            var mesh1 = new Mesh();
            var mesh2 = new Mesh();

            mesh1.name = "Lucy1";
            mesh2.name = "Lucy2";

            mesh1.vertices = source.vertices;
            mesh2.vertices = source.vertices;

            mesh1.normals = source.normals;
            mesh2.normals = source.normals;

            mesh1.tangents = source.tangents;
            mesh2.tangents = source.tangents;

            mesh1.uv = source.uv;
            mesh2.uv = source.uv;

            mesh1.SetIndices(iarray1, MeshTopology.Triangles, 0);
            mesh2.SetIndices(iarray2, MeshTopology.Triangles, 0);

            mesh1.bounds = source.bounds;
            mesh2.bounds = source.bounds;

            Spektr.ScatterTool.MakeScatterableInplace(mesh1);
            Spektr.ScatterTool.MakeScatterableInplace(mesh2);

            AssetDatabase.CreateAsset(mesh1, "Assets/Mirage/Model/Lucy1.asset");
            AssetDatabase.CreateAsset(mesh2, "Assets/Mirage/Model/Lucy2.asset");
        }
    }
}
