using SkiaSharp;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;

namespace Tarkov_DMA_Backend.ESP
{
    public static class ESP_Utilities
    {
        public static VC_Structs.ESPData LastESPData;

        public static bool IsUsingScope;
        public static Vector2 ScopeCenter;
        public static float ScopeDiameter;

        public static Matrix4x4 CameraMatrix;
        public static Matrix4x4 ScopeMatrix;

        public static float ScopeScaleFactor = 0f;
        public static float ScopeDiameterFactor = 0f;
        public static float ScopeCenterFactorX = 0f;
        public static float ScopeCenterFactorY = 0f;

        public static Vector2 W2S_Basic(Vector3 input)
        {
            Matrix4x4 m = CameraMatrix;

            Vector3 translationVector = new(m.M14, m.M24, m.M34);

            Vector3 up = new(m.M12, m.M22, m.M32);
            Vector3 right = new(m.M11, m.M21, m.M31);

            float w = Vector3.Dot(translationVector, input) + m.M44;

            float y = Vector3.Dot(up, input) + m.M42;
            float x = Vector3.Dot(right, input) + m.M41;

            float screenX = (ESP_Config.ResolutionX / 2f) * (1f + x / w);
            float screenY = (ESP_Config.ResolutionY / 2f) * (1f - y / w);

            return new(screenX, screenY);
        }

        public static bool W2S_Camera(Vector3 input, out Vector2 output, bool offscreenCheck = true, bool useTolerance = true)
        {
            Matrix4x4 m = CameraMatrix;

            Vector3 translationVector = new(m.M14, m.M24, m.M34);

            float w = Vector3.Dot(translationVector, input) + m.M44;
            if (w < 0.098f)
            {
                output = default;
                return false;
            }

            Vector3 up = new(m.M12, m.M22, m.M32);
            Vector3 right = new(m.M11, m.M21, m.M31);

            float y = Vector3.Dot(up, input) + m.M42;
            float x = Vector3.Dot(right, input) + m.M41;

            float screenX = (ESP_Config.ResolutionX / 2f) * (1f + x / w);
            float screenY = (ESP_Config.ResolutionY / 2f) * (1f - y / w);

            output = new(screenX, screenY);

            if (offscreenCheck && IsOffscreen(output, useTolerance))
                return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool W2S(Vector3 input, out SKPoint output, bool scale = true, bool offscreenCheck = true, bool useTolerance = true)
        {
            if (W2S(input, out Vector2 vectorOutput, scale, offscreenCheck, useTolerance))
            {
                output = new(vectorOutput.X, vectorOutput.Y);
                return true;
            }
            else
            {
                output = default;
                return false;
            }
        }

        public static bool W2S(Vector3 input, out Vector2 output, bool scale = true, bool offscreenCheck = true, bool useTolerance = true)
        {
            Vector2 cameraPosition;
            if (!W2S_Camera(input, out cameraPosition, offscreenCheck, useTolerance))
            {
                output = default;
                return false;
            }

            // Is this position not inside the scope?
            if (!IsUsingScope || (Vector2.Distance(cameraPosition, ScopeCenter) > ScopeDiameter))
            {
                if (scale && ESP_Config.ShouldScaleESP)
                    output = ScaleESPCoordinate(cameraPosition);
                else
                    output = cameraPosition;

                return true;
            }

            Matrix4x4 m = ScopeMatrix;

            Vector3 translationVector = new(m.M14, m.M24, m.M34);

            Vector3 up = new(m.M12, m.M22, m.M32);
            Vector3 right = new(m.M11, m.M21, m.M31);

            float w = Vector3.Dot(translationVector, input) + m.M44;

            float y = Vector3.Dot(up, input) + m.M42;
            float x = Vector3.Dot(right, input) + m.M41;

            if (ScopeScaleFactor > 0f)
            {
                float angleRadHalf = ((MathF.PI / 180f) * ScopeScaleFactor) * 0.5f;
                float angleCtg = MathF.Cos(angleRadHalf) / MathF.Sin(angleRadHalf);
                y /= angleCtg * 0.5f;
                x /= angleCtg * 0.5f;
            }

            Vector2 scopePosition = new(ScopeDiameter * (1f + x / w), ScopeDiameter * (1f - y / w));

            scopePosition.X += ScopeCenter.X - ScopeDiameter;
            scopePosition.Y += ScopeCenter.Y - ScopeDiameter;

            if (Vector2.Distance(scopePosition, ScopeCenter) > ScopeDiameter)
            {
                output = default;
                return false;
            }

            if (scale && ESP_Config.ShouldScaleESP)
                output = ScaleESPCoordinate(scopePosition);
            else
                output = scopePosition;

            return true;
        }

        public static Vector2 ScaleESPCoordinate(Vector2 coord)
        {
            float transformX = (ESP_Config.ResolutionX / 2) - (ESP_Config.ESP_ResolutionX / 2);
            float transformY = (ESP_Config.ResolutionY / 2) - (ESP_Config.ESP_ResolutionY / 2);

            return new(coord.X - transformX, coord.Y - transformY);
        }

        public static bool IsOffscreen(Vector2 vec, bool useTolerance = true)
        {
            bool isOffscreen;

            if (useTolerance)
            {
                isOffscreen = vec.X > ESP_Config.DisplayXMax ||
                    vec.X < ESP_Config.DisplayXMin ||
                    vec.Y > ESP_Config.DisplayYMax ||
                    vec.Y < ESP_Config.DisplayYMin;
            }
            else
            {
                isOffscreen = vec.X > ESP_Config.ResolutionX ||
                    vec.X < 0 ||
                    vec.Y > ESP_Config.ResolutionY ||
                    vec.Y < 0;
            }
            
            return isOffscreen;
        }
    }
}
