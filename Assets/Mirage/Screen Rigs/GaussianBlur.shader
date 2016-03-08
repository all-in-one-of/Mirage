Shader "Hidden/Mirage/GaussianBlur"
{
    Properties
    {
        _MainTex("", 2D) = ""{}
        _Color0("", Color) = (0, 0, 0)
        _Color1("", Color) = (1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    half4 _Color0;
    half4 _Color1;

    half4 frag_down(v2f_img i) : SV_Target
    {
        float4 d = _MainTex_TexelSize.xyxy * float4(1, 1, -1, -1);
        half p0 = tex2D(_MainTex, i.uv + d.zw).r;
        half p1 = tex2D(_MainTex, i.uv + d.xw).r;
        half p2 = tex2D(_MainTex, i.uv + d.zy).r;
        half p3 = tex2D(_MainTex, i.uv + d.xy).r;
        return (p0 + p1 + p2 + p3) * 0.25;
    }

    half4 frag_up(v2f_img i) : SV_Target
    {
        float4 d = _MainTex_TexelSize.xyxy * float4(1, 1, -1, -1);
        half p0 = tex2D(_MainTex, i.uv + d.zw).r;
        half p1 = tex2D(_MainTex, i.uv + d.xw).r;
        half p2 = tex2D(_MainTex, i.uv + d.zy).r;
        half p3 = tex2D(_MainTex, i.uv + d.xy).r;
        return (p0 + p1 + p2 + p3) * 0.25;
    }

    half4 frag_final(v2f_img i) : SV_Target
    {
        float4 d = _MainTex_TexelSize.xyxy * float4(1, 1, -1, -1);
        half p0 = tex2D(_MainTex, i.uv + d.zw).r;
        half p1 = tex2D(_MainTex, i.uv + d.xw).r;
        half p2 = tex2D(_MainTex, i.uv + d.zy).r;
        half p3 = tex2D(_MainTex, i.uv + d.xy).r;
        half o = (p0 + p1 + p2 + p3) * 0.25;
        return lerp(_Color1, _Color0, pow(1 - o, 10));
    }

    ENDCG

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_down
            ENDCG
        }
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_up
            ENDCG
        }
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_final
            ENDCG
        }
    }
}
