using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Mirage
{
    public class EventDelay : MonoBehaviour
    {
        public enum TimeMode { Second, Frame }

        [SerializeField]
        TimeMode _timeMode = TimeMode.Second;

        [SerializeField]
        float _delay = 1;

        [SerializeField]
        UnityEvent _target;

        Queue<float> _timeQueue;

        bool CheckTime(float time)
        {
            if (_timeMode == TimeMode.Second)
                return time < Time.time;
            else
                return time < Time.frameCount;
        }

        public void Trigger()
        {
            if (_timeMode == TimeMode.Second)
                _timeQueue.Enqueue(Time.time + _delay);
            else
                _timeQueue.Enqueue(Time.frameCount + _delay);
        }

        void Start()
        {
            _timeQueue = new Queue<float>();
        }

        void Update()
        {
            if (_timeQueue.Count > 0 && CheckTime(_timeQueue.Peek()))
            {
                _target.Invoke();
                _timeQueue.Dequeue();
            }
        }
    }
}
