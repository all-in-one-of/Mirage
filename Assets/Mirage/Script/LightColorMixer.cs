using UnityEngine;

namespace Mirage
{
    public class LightColorMixer : MonoBehaviour
    {
        [SerializeField]
        Light[] _targets;

        [SerializeField, Range(0, 1)]
        float _saturation = 0.3f;

        [SerializeField]
        float _highlight = 2;

        [SerializeField]
        float _speed = 8;

        Color[] _colors;

        Color RandomColor()
        {
            return Color.HSVToRGB(Random.value, _saturation, 1.0f);
        }

        public void Reset()
        {
            for (var i = 0; i < _colors.Length; i++)
                _colors[i] = Color.white;
        }

        public void Shuffle()
        {
            for (var i = 0; i < _colors.Length; i++)
            {
                _colors[i] = RandomColor();
                if (_highlight > 0)
                    _targets[i].color = Color.white * _highlight;
            }
        }

        void Start()
        {
            _colors = new Color[_targets.Length];
            Reset();
        }

        void Update()
        {
            var coeff = Mathf.Exp(Time.deltaTime * -_speed);
            for (var i = 0; i < _colors.Length; i++)
                _targets[i].color = Color.Lerp(
                    _colors[i], _targets[i].color, coeff
                );
        }
    }
}

