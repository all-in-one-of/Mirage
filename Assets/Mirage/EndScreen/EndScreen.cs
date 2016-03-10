using UnityEngine;

[ExecuteInEditMode]
public class EndScreen : MonoBehaviour
{
    [SerializeField] float _blinkInterval = 0.3f;
    [SerializeField, HideInInspector] Texture _texture0;
    [SerializeField, HideInInspector] Texture _texture1;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var t = Time.time / _blinkInterval % 2 > 1 ? _texture0 : _texture1;
        Graphics.Blit(t, destination);
    }
}
