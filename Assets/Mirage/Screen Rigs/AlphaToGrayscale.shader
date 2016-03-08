Shader "Mirage/Alpha To Grayscale"
{
    Properties
    {
        _MainTex("", 2D) = ""{}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;

    half4 frag(v2f_img i) : SV_Target
    {
        return tex2D(_MainTex, i.uv).a;
    }

    ENDCG

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}
