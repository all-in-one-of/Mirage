using UnityEngine;

namespace Mirage
{
    class Spinner
    {
        #region Static properties

        public static int columns { get; set; }
        public static int rows { get; set; }

        #endregion

        #region Public functions

        public Spinner()
        {
            _currentPosition = new Vector2(
                Random.Range(0, columns),
                Random.Range(0, rows)
            );

            _currentRotation = Quaternion.identity;

            _distortion = new Vector3(Random.value, Random.value, Random.value);
            _rollAxis = Random.onUnitSphere;

            ChooseNext();
        }

        public Vector3 GetPosition(float time01)
        {
            var p = Vector2.Lerp(_currentPosition, _nextPosition, time01);
            return new Vector3(p.x, p.y, 0);
        }

        public Quaternion GetRotation(float time01, float roll)
        {
            return
                Quaternion.AngleAxis(roll, _rollAxis) *
                Quaternion.Lerp(_currentRotation, _nextRotation, time01);
        }

        public Vector3 GetScale(float distortion)
        {
            return Vector3.one + _distortion * distortion;
        }

        public void StartNextMove()
        {
            _currentPosition = _nextPosition;
            _currentRotation = _nextRotation;

            ChooseNext();
        }

        #endregion

        #region Private members

        Vector2 _currentPosition;
        Vector2 _nextPosition;

        Quaternion _currentRotation;
        Quaternion _nextRotation;

        Vector3 _distortion;
        Vector3 _rollAxis;

        void ChooseNext()
        {
            var dir = Random.Range(0, 4);

            var x = _currentPosition.x;
            var y = _currentPosition.y;

            var dx = (dir == 0 ? -1 : (dir == 2 ? 1 : 0));
            var dy = (dir == 1 ? -1 : (dir == 3 ? 1 : 0));

            if (x + dx < 0 || x + dx >= columns) dx *= -1;
            if (y + dy < 0 || y + dy >= rows   ) dy *= -1;

            _nextPosition = _currentPosition + new Vector2(dx, dy);

            _nextRotation =
                Quaternion.AngleAxis(dx * 90, -Vector3.up) *
                Quaternion.AngleAxis(dy * 90, Vector3.right) *
                _currentRotation;
        }

        #endregion
    }
}
