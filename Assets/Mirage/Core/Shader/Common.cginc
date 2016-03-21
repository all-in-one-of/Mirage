#include "SimplexNoise3D.cginc"

float3 _SpikeOffset;
float _SpikeAmplitude;
float _SpikeExponent;
float _SpikeFrequency;

float3 _MaskOffset;
float _MaskFrequency;

float _Shrink;

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

    float shrink = _Shrink * 2 + snoise(center + _MaskOffset) - 1;
    shrink = saturate(shrink * 2);

    v1 = SpikeDisplacement(v1);
    v2 = SpikeDisplacement(v2);
    v3 = SpikeDisplacement(v3);
    center = (v1 + v2 + v3) * (1.0 / 3);

    v.vertex.xyz = SpikeDisplacement(lerp(v1, center, shrink));

#if RECALC_NORMAL
    v.normal = normalize(cross(v2 - v1, v3 - v1));
#endif
}

float MaskAlpha(float3 vp)
{
    vp *= _MaskFrequency;
    float n1 = snoise(vp + _MaskOffset);
    float n2 = snoise(vp * 2 - _MaskOffset) * 0.5;
    return (n1 + n2) * 0.5 + 0.5;
}
