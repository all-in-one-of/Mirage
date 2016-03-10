Shader "Hidden/Mirage/Cage/Line"
{
    Properties
    {
        _Color ("", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0

        #include "SimplexNoiseGrad3D.cginc"

        float _Radius;
        float _NoiseAmplitude;
        float _NoiseFrequency;
        float3 _NoiseOffset;

        float3 displace(float3 p)
        {
            float3 q = normalize(cross(p, float3(0, 1, 0)) + float3(0, 1e-5, 0));
            float3 r = cross(p, q);
            float3 n = snoise_grad(p * _NoiseFrequency + _NoiseOffset) * _NoiseAmplitude;
            return p * (_Radius + n.x) + q * n.y + r * n.z;
        }

        struct Input { float dummy; };

        fixed4 _Color;

        void vert(inout appdata_full v)
        {
            v.vertex.xyz = displace(v.vertex.xyz);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Emission = _Color.rgb;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
