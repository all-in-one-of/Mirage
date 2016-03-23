Shader "Hidden/Mirage/Lucy/SplitMesh"
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
    }

    CGINCLUDE

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

    float _Bend;
    float4 _Axes;
    float _Twist;
    float3 _Spike; // dist, filter, seed
    float _Voxel;

    struct Input
    {
        float2 uv_ColorMap;
    };

    // PRNG
    float UVRand(float2 uv)
    {
        return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
    }

    // Bend deformation
    float3 Bend(float3 v)
    {
        const float z0 = 1.8;

        float theta = (v.z + z0) / _Bend;
        float l1 = _Bend - dot(v.xy, _Axes.xy);
        float l2 = dot(v.xy, _Axes.zw);

        float2 xy = _Axes.xy * (_Bend - cos(theta) * l1);
        xy += _Axes.zw * l2;

        float z = sin(theta) * l1 - z0;

        return lerp(float3(xy, z), v, saturate(_Bend / 100 - 1));
    }

    // Twist deformation
    float3 Twist(float3 v)
    {
        float phi = (v.z + 1.8) * _Twist;

        float sin_phi, cos_phi;
        sincos(phi, sin_phi, cos_phi);

        float2 r1 = float2(cos_phi, -sin_phi);
        float2 r2 = float2(sin_phi,  cos_phi);

        return float3(dot(r1, v.xy), dot(r2, v.xy), v.z);
    }

    // Spiky displacement
    float3 Displace(float3 v0, float3 v, float3 n)
    {
        float d = UVRand(v0.xz + v0.zy + _Spike.z) < _Spike.y;
        return v + n * d * _Spike.x;
    }

    float3 Voxelize(float3 v)
    {
        float3 v2 = ceil(v * _Voxel) / _Voxel;
        return lerp(v2, v, saturate(_Voxel / 100 - 1));
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0

        void vert(inout appdata_full v)
        {
            float3 v1 = v.vertex.xyz;
            float3 v2 = v.texcoord1.xyz;
            float3 v3 = v.texcoord2.xyz;
            float3 n = v.normal;

            v1 = Bend(Twist(Displace(v1, Voxelize(v1), n)));
            v2 = Bend(Twist(Displace(v2, Voxelize(v2), n)));
            v3 = Bend(Twist(Displace(v3, Voxelize(v3), n)));

            v.vertex.xyz = v1;
            v.normal = normalize(cross(v2 - v1, v3 - v1));
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_ColorMap;

            fixed4 cm = tex2D(_ColorMap, uv);
            o.Albedo = lerp(_Color1, _Color2, cm);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            fixed4 nrm = tex2D(_BumpMap, uv);
            o.Normal = UnpackScaleNormal(nrm, _BumpScale) * float3(1, -1, 1);

            fixed occ = tex2D(_OcclusionMap, uv).g;
            occ = pow(1 - occ, _OcclusionContrast);
            o.Occlusion = 1 - _OcclusionStrength * occ;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
