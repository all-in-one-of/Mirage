Shader "Hidden/Mirage/Core/Surface"
{
    Properties
    {
        _Color("", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }

        Cull back

        CGPROGRAM

        #pragma surface surf Standard vertex:vert alphatest:_Cutoff nolightmap addshadow
        #pragma target 3.0

        #include "Common.cginc"

        struct Input {
            float3 localPos;
        };

        fixed4 _Color;

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);

            data.localPos = v.vertex.xyz;

            float3 v1 = SpikeDisplacement(v.vertex.xyz);
            float3 v2 = SpikeDisplacement(v.texcoord.xyz);
            float3 v3 = SpikeDisplacement(v.texcoord1.xyz);

            v.vertex.xyz = v1;
            v.normal = normalize(cross(v2 - v1, v3 - v1));
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
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

        fixed4 _Color;

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);

            data.localPos = v.vertex.xyz;

            float3 v1 = SpikeDisplacement(v.vertex.xyz);
            float3 v2 = SpikeDisplacement(v.texcoord.xyz);
            float3 v3 = SpikeDisplacement(v.texcoord1.xyz);

            v.vertex.xyz = v1;
            v.normal = -normalize(cross(v2 - v1, v3 - v1));
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Alpha = MaskAlpha(IN.localPos);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
