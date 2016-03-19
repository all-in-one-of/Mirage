#include "SimplexNoise3D.cginc"

float3 _SpikeOffset;
float _SpikeAmplitude;
float _SpikeExponent;
float _SpikeFrequency;

float3 _MaskOffset;
float _MaskFrequency;

float3 SpikeDisplacement(float3 vp)
{
    float n = snoise(vp * _SpikeFrequency + _SpikeOffset);
    return vp * (1.0 + pow(abs(n), _SpikeExponent) * _SpikeAmplitude);
}

float MaskAlpha(float3 vp)
{
    vp *= _MaskFrequency;
    float n1 = snoise(vp + _MaskOffset);
    float n2 = snoise(vp * 2 - _MaskOffset) * 0.5;
    return (n1 + n2) * 0.5 + 0.5;
}
