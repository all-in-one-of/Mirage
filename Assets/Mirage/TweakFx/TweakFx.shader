Shader "Hidden/Mirage/Tweak"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    float _Bias;
    float _Amp;

    half4 frag(v2f_img i) : SV_Target
    {
        float4 c = tex2D(_MainTex, i.uv);
        c.rgb = (c.rgb + _Bias) * _Amp;
        return c;
    }

    ENDCG

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}
