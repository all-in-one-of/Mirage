using UnityEngine;
using UnityEngine.Events;
using System;

namespace Mirage
{
    public class Impulse : MonoBehaviour
    {
        [Serializable]
        public class ValueEvent : UnityEvent<float> {}

        [SerializeField]
        float _amplitude = 1;

        public float amplitude {
            get { return _amplitude; }
            set { _amplitude = value; }
        }

        [SerializeField]
        ValueEvent _target;

        int _triggered;

        public void Trigger()
        {
            _target.Invoke(_amplitude);
            _triggered = 1;
        }

        void Update()
        {
            if (_triggered == 1)
            {
                _triggered++;
            }
            else if (_triggered == 2)
            {
                _target.Invoke(0);
                _triggered = 0;
            }
        }
    }
}
