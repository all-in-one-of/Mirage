using UnityEngine;
using Klak.MaterialExtension;

namespace Mirage
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class TweakFx : MonoBehaviour
    {
        #region Exposed properties

        public float low {
            get { return _low; }
            set { _low = value; }
        }

        [SerializeField, Range(0, 1)] float _low = 0;

        public float high {
            get { return _high; }
            set { _high = value; }
        }

        [SerializeField, Range(0, 1)] float _high = 1;

        #endregion

        #region Private resources

        [SerializeField, HideInInspector] Shader _shader;
        Material _material;

        #endregion

        #region MonoBehaviour functions

        void OnEnable()
        {
            _material = new Material(Shader.Find("Hidden/Mirage/Tweak"));
            _material.hideFlags = HideFlags.DontSave;
        }

        void OnDisable()
        {
            DestroyImmediate(_material);
            _material = null;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _material.
                Property("_Bias", -_low).
                Property("_Amp", 1 / (_high - _low));

            Graphics.Blit(source, destination, _material, 0);
        }

        #endregion
    }
}
