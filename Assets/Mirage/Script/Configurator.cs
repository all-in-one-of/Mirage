using UnityEngine;

namespace Mirage
{
    public class Configurator : MonoBehaviour
    {
        [SerializeField] Camera[] _sceneCameras;
        [SerializeField] Camera[] _clearCameras;
        [SerializeField] MeshRenderer _background;

        [Space]
        [SerializeField] GUISkin _guiSkin;

        Material _backgroundMaterial;

        void Start()
        {
            // Add an empty camera if the 1st display is
            // going to be used for GUI.
            #if MIRAGE_TEST && MIRAGE_TRIPLE
            AddNullCamera();
            #endif

            if (!Application.isEditor)
            {
                // Hide mouse cursor.
                Cursor.visible = false;

                #if MIRAGE_TRIPLE
                // Remap displays for the triple display mode.
                for (var i = 0; i < 2; i++)
                {
                    _sceneCameras[i].targetDisplay = i + 1;
                    _clearCameras[i].targetDisplay = i + 1;
                }
                #endif

                // Try activating multiple displays.
                #if !MIRAGE_TRIPLE || MIRAGE_TEST
                TryActivateDisplay(0);
                #endif
                TryActivateDisplay(1);
                #if MIRAGE_TRIPLE
                TryActivateDisplay(2);
                #endif
            }
        }

        void TryActivateDisplay(int index)
        {
            if (index < Display.displays.Length)
                Display.displays[index].Activate();
        }

        #if MIRAGE_TEST

        Rect guiRect {
            get {
                var w = Screen.width;
                var h = Screen.height;
                return new Rect(w / 3, h / 4, w / 3, h / 2);
            }
        }

        GUIStyle labelStyle {
            get {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle();
                    _labelStyle.normal.textColor = Color.white;
                    _labelStyle.fontSize = 20;
                    _labelStyle.padding = new RectOffset(10, 10, 10, 10);
                }
                return _labelStyle;
            }
        }

        GUIStyle _labelStyle;

        void AddNullCamera()
        {
            var cam = gameObject.AddComponent<Camera>();
            cam.backgroundColor = Color.gray;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.cullingMask = 0;
            cam.depth = 1000;
        }

        void OnGUI()
        {
            GUI.skin = _guiSkin;

            GUILayout.BeginArea(guiRect);

            GUILayout.Label("Back screen configuration");

            GUILayout.FlexibleSpace();

            var cam = _sceneCameras[1];
            GUILayout.Label("FOV: " + cam.fieldOfView);
            cam.fieldOfView = GUILayout.HorizontalSlider(cam.fieldOfView, 30, 60);

            GUILayout.FlexibleSpace();

            var pos = cam.transform.position;
            GUILayout.Label("Position (Y): " + pos.y);
            pos.y = GUILayout.HorizontalSlider(pos.y, 1, 3);
            cam.transform.position = pos;

            GUILayout.FlexibleSpace();

            var material = _background.material;
            var color = material.color;
            GUILayout.Label("Albedo brightness: " + (color.r * 255));
            var br = GUILayout.HorizontalSlider(color.r, 0, 1);
            material.color = Color.white * br;

            GUILayout.EndArea();
        }

        #endif
    }
}
