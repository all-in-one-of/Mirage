using UnityEngine;

[ExecuteInEditMode]
public class BackScreenEffect : MonoBehaviour
{
    [SerializeField]
    Color _color0 = Color.black;

    [SerializeField]
    Color _color1 = Color.white;

    [SerializeField] 
    int _blurIterations = 3;

    [SerializeField]
    Shader _blurShader;

    Material blurMaterial {
        get {
            if (_blurMaterial == null) {
                var shader = Shader.Find("Hidden/Mirage/BackScreenEffect");
                _blurMaterial = new Material(shader);
                _blurMaterial.hideFlags = HideFlags.DontSave;
            }
            return _blurMaterial;
        }
    }

    Material _blurMaterial;

    void OnDisable()
    {
        if (_blurMaterial != null) DestroyImmediate(_blurMaterial);
        _blurMaterial = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var rt = new RenderTexture[_blurIterations];
        var tw = source.width;
        var th = source.height;
        var format = RenderTextureFormat.RHalf;
        var m = blurMaterial;

        var i = 0;
        while (i < rt.Length)
        {
            tw /= 2;
            th /= 2;

            rt[i] = RenderTexture.GetTemporary(tw, th, 0, format);
            Graphics.Blit(i == 0 ? source : rt[i - 1], rt[i], m, 0);
            i++;
        }

        i--;

        while (i > 0)
        {
            Graphics.Blit(rt[i], rt[i - 1], m, 1);
            RenderTexture.ReleaseTemporary(rt[i]);
            i--;
        }

        m.SetColor("_Color0", _color0);
        m.SetColor("_Color1", _color1);
        Graphics.Blit(rt[0], destination, m, 2);
        RenderTexture.ReleaseTemporary(rt[0]);
    }
}
