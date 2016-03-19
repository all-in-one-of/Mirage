Shader "Hidden/Mirage/Stripe"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _Color("", Color) = (1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;

    sampler2D_float _CameraDepthTexture;
    sampler2D _CameraGBufferTexture2;
    float4x4 _WorldToCamera;

    fixed3 _Color;
    float _Specular;

    float _Frequency;
    float _Cutoff;
    float3 _Origin;
    float3 _Direction;
    float _Offset;

    half4 frag(v2f_img i) : SV_Target
    {
        // Sample a linear depth on the depth buffer.
        float depth_s = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
        float depth_o = LinearEyeDepth(depth_s);

        // Sample a view-space normal vector on the g-buffer.
        float3 norm_o = tex2D(_CameraGBufferTexture2, i.uv).xyz * 2 - 1;
        norm_o = mul((float3x3)_WorldToCamera, norm_o);

        // Reconstruct the view-space position.
        float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
        float2 p13_31 = float2(unity_CameraProjection._13, unity_CameraProjection._23);
        float3 pos_o = float3((i.uv * 2 - float2(1, -1) - p13_31) / p11_22, 1) * depth_o;

        // Rimlight
        float rim = (depth_s < 1);
        rim *= pow(1 - saturate(-dot(normalize(pos_o), norm_o)), _Specular);

        // Stripe
        float stripe = dot(pos_o - _Origin, _Direction);
        stripe = frac(stripe * _Frequency - _Offset) < _Cutoff;

        return lerp(0, fixed4(_Color, 1), rim * stripe);
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
            #pragma target 3.0
            ENDCG
        }
    }
}
