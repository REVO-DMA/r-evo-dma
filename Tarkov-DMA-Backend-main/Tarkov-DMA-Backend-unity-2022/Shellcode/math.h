#pragma once

#include <intrin.h>

__forceinline float Sqrt(float f)
{
    return _mm_cvtss_f32(_mm_sqrt_ss(_mm_set_ss(f)));
}

__forceinline float V3_Dot(Vector3 v1, Vector3 v2)
{
    float length = (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
    return length;
}

__forceinline Vector3 V3_Add(Vector3 v1, Vector3 v2)
{
    return { (v1.X + v2.X), (v1.Y + v2.Y), (v1.Z + v2.Z) };
}

__forceinline Vector3 V3_Subtract(Vector3 v1, Vector3 v2)
{
    return { (v1.X - v2.X), (v1.Y - v2.Y), (v1.Z - v2.Z) };
}

__forceinline Vector3 V3_Multiply(Vector3 v1, Vector3 v2)
{
    return { (v1.X * v2.X), (v1.Y * v2.Y), (v1.Z * v2.Z) };
}

__forceinline Vector3 V3_Multiply(Vector3 v1, float x)
{
    return V3_Multiply(v1, { x, x, x });
}

__forceinline float V3_DistanceSquared(Vector3 v1, Vector3 v2)
{
    Vector3 difference = V3_Subtract(v1, v2);
    return V3_Dot(difference, difference);
}

__forceinline float V3_Distance(Vector3 v1, Vector3 v2)
{
    float distanceSquared = V3_DistanceSquared(v1, v2);
    return Sqrt(distanceSquared);
}

__forceinline float V3_LengthSquared(Vector3 v)
{
    return V3_Dot(v, v);
}

__forceinline float V3_Length(Vector3 v)
{
    float length = V3_LengthSquared(v);
    return Sqrt(length);
}

__forceinline Vector3 V3_Normalize(Vector3 v)
{
    float length = V3_Length(v);
    Vector3 newVec = { v.X / length, v.Y / length, v.Z / length };
    return newVec;
}