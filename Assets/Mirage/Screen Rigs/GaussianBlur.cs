using UnityEngine;

[ExecuteInEditMode]
public class GaussianBlur : MonoBehaviour
{
    [SerializeField] Color _color0 = Color.black;
    [SerializeField] Color _color1 = Color.white;
    [SerializeField] Shader _shader;

    Material material {
        get {
            if (_material == null) {
                var shader = Shader.Find("Hidden/Mirage/GaussianBlur");
                _material = new Material(shader);
                _material.hideFlags = HideFlags.DontSave;
            }
            return _material;
        }
    }

    Material _material;

    void OnDisable()
    {
        if (_material != null) DestroyImmediate(_material);
        _material = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var rt = new RenderTexture[1];
        var tw = source.width;
        var th = source.height;
        var format = RenderTextureFormat.RHalf;

        var i = 0;
        while (i < rt.Length)
        {
            tw /= 2;
            th /= 2;

            rt[i] = RenderTexture.GetTemporary(tw, th, 0, format);
            Graphics.Blit(i == 0 ? source : rt[i - 1], rt[i], material, 0);
            i++;
        }

        i--;

        while (i > 0)
        {
            Graphics.Blit(rt[i], rt[i - 1], material, 1);
            RenderTexture.ReleaseTemporary(rt[i]);
            i--;
        }

        material.SetColor("_Color0", _color0);
        material.SetColor("_Color1", _color1);
        Graphics.Blit(rt[0], destination, material, 2);
        RenderTexture.ReleaseTemporary(rt[0]);
    }
}
