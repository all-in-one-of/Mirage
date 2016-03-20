using UnityEngine;
using UnityEngine.Events;
using System;
using Klak.Math;

namespace Mirage
{
    public class NoiseInjector : MonoBehaviour
    {
        [Serializable]
        public class ValueEvent : UnityEvent<float> {}

        [SerializeField]
        float _frequency = 1.0f;

        [SerializeField, Range(1, 5)]
        int _fractalLevel = 2;

        [SerializeField, Range(0, 1)]
        float _noiseRate = 0.1f;

        [SerializeField]
        ValueEvent _target;

        NoiseGenerator _noise;

        public float inputValue {
            set {
                SendWithNoise(value);
            }
        }

        void SendWithNoise(float input)
        {
            var n = _noise.Value01(0) * input * _noiseRate;
            _target.Invoke(input - n);
        }

        void Start()
        {
            _noise = new NoiseGenerator(_frequency);
            _noise.FractalLevel = _fractalLevel;
        }

        void Update()
        {
            _noise.Frequency = _frequency;
            _noise.FractalLevel = _fractalLevel;
            _noise.Step();
        }
    }
}

