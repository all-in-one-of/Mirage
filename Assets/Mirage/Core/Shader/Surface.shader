Shader "Hidden/Mirage/Core/Surface"
{
    Properties
    {
        _Color("", Color) = (1,1,1,1)
        _BackColor("", Color) = (0.5, 0.5, 0.5, 1)

        _Glossiness("", Range(0.0, 1.0)) = 0.5
        [Gamma] _Metallic("", Range(0.0, 1.0)) = 0.0

        _EmissionColor("", Color) = (0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }

        Cull back

        CGPROGRAM

        #pragma surface surf Standard vertex:vert alphatest:_Cutoff nolightmap addshadow
        #pragma target 3.0

        #define RECALC_NORMAL 1

        #include "Common.cginc"

        struct Input {
            float3 localPos;
        };

        fixed4 _Color;
        half _Glossiness;
        half _Metallic;
        fixed3 _EmissionColor;

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            data.localPos = v.vertex.xyz;
            ApplyModifier(v);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = _EmissionColor.rgb;
            o.Alpha = MaskAlpha(IN.localPos);
        }

        ENDCG

        Cull front

        CGPROGRAM

        #pragma surface surf Standard vertex:vert alphatest:_Cutoff nolightmap addshadow
        #pragma target 3.0

        #include "Common.cginc"

        struct Input {
            float3 localPos;
        };

        fixed4 _BackColor;
        half _Glossiness;
        half _Metallic;

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            data.localPos = v.vertex.xyz;
            ApplyModifier(v);
            v.normal *= -1;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _BackColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = MaskAlpha(IN.localPos);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
