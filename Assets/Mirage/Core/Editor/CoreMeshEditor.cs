using UnityEngine;
using UnityEditor;
using System.IO;

namespace Mirage
{
    [CustomEditor(typeof(CoreMesh))]
    public class CoreMeshEditor : Editor
    {
        SerializedProperty _subdivisionLevel;

        void OnEnable()
        {
            _subdivisionLevel = serializedObject.FindProperty("_subdivisionLevel");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_subdivisionLevel);
            var rebuild = EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            if (rebuild)
                foreach (var t in targets)
                    ((CoreMesh)t).RebuildMesh();
        }

        [MenuItem("Assets/Create/Mirage/CoreMesh")]
        public static void CreateCoreMeshAsset()
        {
            // Make a proper path from the current selection.
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (Path.GetExtension(path) != "")
                path = path.Replace(Path.GetFileName(path), "");
            var assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/CoreMesh.asset");

            // Create an CoreMesh asset.
            var asset = ScriptableObject.CreateInstance<CoreMesh>();
            AssetDatabase.CreateAsset(asset, assetPathName);
            AssetDatabase.AddObjectToAsset(asset.sharedMesh, asset);

            // Build an initial mesh for the asset.
            asset.RebuildMesh();

            // Save the generated mesh asset.
            AssetDatabase.SaveAssets();

            // Tweak the selection.
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
