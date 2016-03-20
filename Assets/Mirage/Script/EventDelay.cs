using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Mirage
{
    public class EventDelay : MonoBehaviour
    {
        [SerializeField] float _delay = 0.1f;

        [SerializeField] UnityEvent _target;

        Queue<float> _timeQueue;

        public void Trigger()
        {
            _timeQueue.Enqueue(Time.time + _delay);
        }

        void Start()
        {
            _timeQueue = new Queue<float>();
        }

        void Update()
        {
            if (_timeQueue.Count > 0)
            {
                float time = _timeQueue.Peek();
                if (time < Time.time) {
                    _target.Invoke();
                    _timeQueue.Dequeue();
                }
            }
        }
    }
}
