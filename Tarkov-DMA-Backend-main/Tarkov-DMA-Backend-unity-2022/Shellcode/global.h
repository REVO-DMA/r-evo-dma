#pragma once

#include "xorstr.h"

#define IS_INVALID_ADDR(va) (va < 0x100000 || va >= 0x7FFFFFFFFFF)
#define RETURN_0_IF_INVALID(va) if (IS_INVALID_ADDR(va)) return 0x0
#define CONTINUE_IF_INVALID(va) if (IS_INVALID_ADDR(va)) continue
#define V3_IS_ZERO(v) (v.X == 0.0f && v.Y == 0.0f && v.Z == 0.0f)

extern "C" int _fltused = 1;

typedef unsigned char uint8_t;
typedef unsigned long long ulong;
typedef unsigned char* PBYTE;

struct Vector3 { float X, Y, Z; };
struct Vector2 { float X, Y; };
struct Matrix4x4 { float M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44; };