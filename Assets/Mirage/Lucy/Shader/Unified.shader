Shader "Hidden/Mirage/Lucy/Unified"
{
    Properties
    {
        _Color1("", Color) = (1, 1, 1, 1)
        _Color2("", Color) = (0.4, 0.2, 0.1, 1)
        _ColorMap("", 2D) = "black"{}

        _Glossiness("", Range(0, 1)) = 0
        [Gamma] _Metallic("", Range(0, 1)) = 0

        _BumpMap("", 2D) = "bump"{}
        _BumpScale("", Float) = 1

        _OcclusionMap("", 2D) = "white"{}
        _OcclusionStrength("", Range(0, 1)) = 1
        _OcclusionContrast("", Range(0, 5)) = 1

        _BackColor("", Color) = (0, 0, 0)
    }

    CGINCLUDE

    #include "SimplexNoise2D.cginc"

    fixed4 _Color1;
    fixed4 _Color2;
    sampler2D _ColorMap;

    half _Glossiness;
    half _Metallic;

    sampler2D _BumpMap;
    half _BumpScale;

    sampler2D _OcclusionMap;
    half _OcclusionStrength;
    half _OcclusionContrast;

    fixed4 _BackColor;

    float _Cutout;
    float _Highlight;

    struct Input
    {
        float2 uv_ColorMap;
    };

    float ProceduralPattern(float2 uv)
    {
        float n = snoise(uv * float2(30, 0.3) + float2(0.4, _Time.y * 3));
        return (n + 1) * 0.5;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard nolightmap addshadow
        #pragma target 3.0

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_ColorMap;

            float pp = ProceduralPattern(uv);
            clip(pp - _Cutout);

            fixed4 cm = tex2D(_ColorMap, uv);
            o.Albedo = lerp(_Color1, _Color2, cm);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            fixed4 nrm = tex2D(_BumpMap, uv);
            o.Normal = UnpackScaleNormal(nrm, _BumpScale) * float3(1, -1, 1);

            fixed occ = tex2D(_OcclusionMap, uv).g;
            occ = pow(1 - occ, _OcclusionContrast);
            o.Occlusion = 1 - _OcclusionStrength * occ;

            o.Emission = pp < _Highlight;
        }

        ENDCG

        Cull Front

        CGPROGRAM

        #pragma surface surf Standard nolightmap addshadow
        #pragma target 3.0

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_ColorMap;

            float pp = ProceduralPattern(uv);
            clip(pp - _Cutout);

            o.Albedo = _BackColor.rgb;
            o.Normal *= -1;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
