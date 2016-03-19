using UnityEngine;
using Klak.MaterialExtension;

namespace Mirage
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class StripeFx : MonoBehaviour
    {
        #region Exposed properties

        public float cutoff {
            get { return _cutoff; }
            set { _cutoff = value; }
        }

        [SerializeField, Range(0, 1)] float _cutoff = 0.5f;

        public Transform origin {
            get { return _origin; }
            set { _origin = value; }
        }

        [Space, SerializeField] Transform _origin;

        public float frequency {
            get { return _frequency; }
            set { _frequency = value; }
        }

        [SerializeField] float _frequency = 0.3f;

        public float speed {
            get { return _speed; }
            set { _speed = value; }
        }

        [SerializeField] float _speed = 1;

        public Color color {
            get { return _color; }
            set { _color = value; }
        }

        [Space, SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
        Color _color = Color.white;

        public float specular {
            get { return _specular; }
            set { _specular = value; }
        }

        [SerializeField] float _specular = 2;

        #endregion

        #region Private resources

        [SerializeField, HideInInspector] Shader _shader;
        Material _material;

        #endregion

        #region Private properties and variables

        float _offset;

        #endregion

        #region MonoBehaviour functions

        void OnEnable()
        {
            _material = new Material(Shader.Find("Hidden/Mirage/Stripe"));
            _material.hideFlags = HideFlags.DontSave;
        }

        void OnDisable()
        {
            DestroyImmediate(_material);
            _material = null;
        }

        void Update()
        {
            _offset += _speed * Time.deltaTime;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var spos = _origin != null ? _origin.position : Vector3.zero;
            var sdir = _origin != null ? _origin.up : Vector3.up;

            _material.
                Property("_Color", _color).
                Property("_Specular", _specular).
                Property("_Frequency", _frequency).
                Property("_Cutoff", _cutoff).
                Property("_Origin", spos).
                Property("_Direction", sdir).
                Property("_Offset", _offset);

            Graphics.Blit(source, destination, _material, 0);
        }

        #endregion
    }
}
