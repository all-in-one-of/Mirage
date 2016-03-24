using UnityEngine;

namespace Mirage
{
    public class Configurator : MonoBehaviour
    {
        [SerializeField] TweakFx _tweakFx;
        [SerializeField] Camera _secondaryCamera;
        [SerializeField] GUISkin _guiSkin;

        void Start()
        {
            #if MIRAGE_TEST
            AddCamera();
            #endif

            if (!Application.isEditor)
            {
                Cursor.visible = false;

                #if MIRAGE_TEST
                TryActivateDisplay(0); // Config
                #endif

                TryActivateDisplay(1); // Primary screen
                TryActivateDisplay(2); // Secondary screen
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

        void AddCamera()
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

            GUILayout.Label("Low level: " + _tweakFx.low);
            _tweakFx.low = GUILayout.HorizontalSlider(_tweakFx.low, 0, 1);

            GUILayout.FlexibleSpace();

            GUILayout.Label("High level: " + _tweakFx.high);
            _tweakFx.high = GUILayout.HorizontalSlider(_tweakFx.high, 0, 1);

            GUILayout.FlexibleSpace();

            var cam = _secondaryCamera;
            GUILayout.Label("FOV: " + cam.fieldOfView);
            cam.fieldOfView = GUILayout.HorizontalSlider(cam.fieldOfView, 30, 60);

            GUILayout.FlexibleSpace();

            var pos = _secondaryCamera.transform.position;
            GUILayout.Label("Position (Y): " + pos.y);
            pos.y = GUILayout.HorizontalSlider(pos.y, 1, 3);
            _secondaryCamera.transform.position = pos;

            GUILayout.FlexibleSpace();

            GUILayout.EndArea();
        }

        #endif
    }
}
