using UnityEngine;
using System.Collections.Generic;
using Klak.Motion;

namespace Mirage
{
    public class TouchController : MonoBehaviour
    {
        #region External object references

        [SerializeField] Transform[] _targets;
        [SerializeField] Transform _frontScreen;
        [SerializeField] Camera _primaryCamera;

        #endregion

        #region Coordinate transformer

        class Transformer
        {
            Rect _viewRect;
            Vector2 _dimensions;

            public Transformer(Camera camera, Transform screen)
            {
                _viewRect = camera.rect;

                var scale = screen.localScale;
                _dimensions = new Vector2(scale.x, scale.y);
            }

            public Vector3 ScreenToWorld(Vector2 sp)
            {
                var x = sp.x / Screen.width;
                var y = sp.y / Screen.height;

                y = (y - _viewRect.yMin) / _viewRect.height;

                x = (x - 0.5f) * _dimensions.x;
                y *= _dimensions.y;

                return new Vector3(x, y, 0);
            }
        }

        Transformer _transformer;

        #endregion

        #region Touch point handler

        class TouchPoint
        {
            public int FingerID { get; set; }

            public bool Captured {
                get { return FingerID >= 0; }
            }

            Transform _target;
            Vector2 _lastPosition;

            public TouchPoint(Transform target)
            {
                _target = target;
                FingerID = -1;
            }

            public void Capture(int fingerID)
            {
                FingerID = fingerID;
            }

            public void Release()
            {
                FingerID = -1;
            }

            public float CalculateDistance(Vector2 position)
            {
                return (position - _lastPosition).magnitude;
            }

            public void Update(Vector2 position, Transformer trans)
            {
                _target.localPosition = trans.ScreenToWorld(position);
                _lastPosition = position;
            }
        }

        TouchPoint FindCapturedTouchPoint(int fingerID)
        {
            foreach (var tp in _touchPoints)
                if (tp.Captured && tp.FingerID == fingerID) return tp;
            return null;
        }

        TouchPoint GetClosestFreeTouchPoint(Vector2 position)
        {
            var minDist = 1e6f;
            TouchPoint closest = null;

            foreach (var tp in _touchPoints)
            {
                if (tp.Captured) continue;

                var dist = tp.CalculateDistance(position);
                if (dist < minDist) {
                    minDist = dist;
                    closest = tp;
                }
            }

            return closest;
        }

        List<TouchPoint> _touchPoints;

        #endregion

        #region MonoBehaviour functions

        void Start()
        {
            _transformer = new Transformer(_primaryCamera, _frontScreen);

            _touchPoints = new List<TouchPoint>(_targets.Length);
            foreach (var t in _targets) _touchPoints.Add(new TouchPoint(t));
        }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                // Multi touch mode
                foreach (var t in Input.touches)
                {
                    var pos = t.position;

                    if (t.phase == TouchPhase.Began)
                    {
                        // Touch began: try to capture the closest point.
                        var tp = GetClosestFreeTouchPoint(pos);
                        if (tp != null) {
                            tp.Capture(t.fingerId);
                            tp.Update(pos, _transformer);
                        }
                    }
                    else if (t.phase == TouchPhase.Moved)
                    {
                        // Touch moved: update it if it's captured one.
                        var tp = FindCapturedTouchPoint(t.fingerId);
                        if (tp != null) tp.Update(pos, _transformer);
                    }
                    else if (t.phase == TouchPhase.Ended ||
                             t.phase == TouchPhase.Canceled)
                    {
                        // Touch ended: relase it if it's captured one.
                        var tp = FindCapturedTouchPoint(t.fingerId);
                        if (tp != null) tp.Release();
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                // Touch emulation mode:
                // update the targets with the mouse position.
                foreach (var tp in _touchPoints)
                    tp.Update(Input.mousePosition, _transformer);
            }
        }

        #endregion
    }
}
