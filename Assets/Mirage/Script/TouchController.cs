using UnityEngine;

namespace Mirage
{
    public class TouchController : MonoBehaviour
    {
        [SerializeField] Transform _frontScreen;
        [SerializeField] Camera _primaryCamera;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mp = Input.mousePosition;

                var view = _primaryCamera.rect;
                var px =  mp.x / Screen.width;
                var py = (mp.y / Screen.height - view.yMin) / view.height;

                var scale = _frontScreen.localScale;
                px = (px - 0.5f) * scale.x;
                py *= scale.y;

                transform.position = new Vector3(px, py, 0);
            }
        }
    }
}
