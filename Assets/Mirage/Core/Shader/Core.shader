Shader "Hidden/Mirage/Core"
{
    Properties
    {
        _Color("", Color) = (1, 1, 1, 1)
        _BackColor("", Color) = (0.5, 0.5, 0.5, 1)
        _Glossiness("", Range(0, 1)) = 0.5
        [Gamma] _Metallic("", Range(0, 1)) = 0
        _EmissionColor("", Color) = (0, 0, 0)
        _Flip("", Float) = 1
        _CullMode("", Int) = 2
    }

    CGINCLUDE

    #include "SimplexNoise3D.cginc"

    fixed4 _Color;
    half _Glossiness;
    half _Metallic;
    fixed3 _EmissionColor;

    float3 _SpikeOffset;
    float _SpikeAmplitude;
    float _SpikeExponent;
    float _SpikeFrequency;

    float3 _DissolveOffset;
    float _DissolveLevel;
    float _DissolveFrequency;

    float _Flip;

    float3 SpikeDisplacement(float3 vp)
    {
        float n = snoise(vp * _SpikeFrequency + _SpikeOffset);
        return vp * (1.0 + pow(abs(n), _SpikeExponent) * _SpikeAmplitude);
    }

    void ApplyModifier(inout appdata_full v)
    {
        float3 v1 = v.vertex.xyz;
        float3 v2 = v.texcoord.xyz;
        float3 v3 = v.texcoord1.xyz;

        float3 center = (v1 + v2 + v3) * (1.0 / 3);

        float dissolve = snoise(center + _DissolveOffset);
        dissolve = saturate(dissolve - 1 + _DissolveLevel * 3);

        v1 = SpikeDisplacement(v1);
        v2 = SpikeDisplacement(v2);
        v3 = SpikeDisplacement(v3);
        center = (v1 + v2 + v3) * (1.0 / 3);

        v.vertex.xyz = SpikeDisplacement(lerp(v1, center, dissolve));
        v.normal = normalize(cross(v2 - v1, v3 - v1)) * _Flip;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Cull [_CullMode]

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0

        struct Input { float not_in_use; };

        void vert(inout appdata_full v)
        {
            ApplyModifier(v);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = _EmissionColor.rgb;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
