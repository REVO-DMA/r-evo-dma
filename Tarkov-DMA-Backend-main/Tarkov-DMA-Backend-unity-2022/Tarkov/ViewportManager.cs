using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;

namespace Tarkov_DMA_Backend.Tarkov
{
    public static class ViewportManager
    {
        public const bool SCOPE_FACTORS_DEBUG = false;

        private static ulong LastHandsVersion = ulong.MaxValue;
        private static float LastScopeFOV;

        public static void Update()
        {
            if (!CameraManager.PlayerIsInRaid) return;

            Player localPlayer = EFTDMA.LocalPlayer;
            if (localPlayer == null)
                return;

            VC_Structs.ESPData espData = VisibilityCheck.GetESPData();
            ESP_Utilities.LastESPData = espData;

            var isScoped = XBOOL.Get(espData.IsScoped);
            var lensPosition = VC_Structs.Vector3.ToSystem(espData.LensPosition);
            var cameraMatrix = VC_Structs.Matrix4x4.ToSystem(espData.CameraMatrix);
            var scopeMatrix = VC_Structs.Matrix4x4.ToSystem(espData.ScopeMatrix);

            ESP_Utilities.IsUsingScope = isScoped;
            ESP_Utilities.CameraMatrix = cameraMatrix;

            // Only update scope factors when the scope changes in some way
            if (espData.ScopeFOV != LastScopeFOV ||
                LocalHandsManager.HandsVersion != LastHandsVersion)
            {
                if (!SCOPE_FACTORS_DEBUG)
                    UpdateScopeFactors(espData.ScopeFOV, LocalHandsManager.WeaponScopeIDs);
                
                LastScopeFOV = espData.ScopeFOV;
                LastHandsVersion = LocalHandsManager.HandsVersion;
            }

            if (isScoped)
            {
                float scopeRadius = MathF.Max(MathF.Max(espData.AABB.Extent.x, espData.AABB.Extent.y), espData.AABB.Extent.z);

                float yOffset;
                if (EFTDMA.MapID == "lighthouse")
                    yOffset = 0f;
                else
                    yOffset = GetComplementaryDegree(localPlayer.Rotation.Y);

                Vector3 offsetVector = new(0f, 90f - yOffset, 0f);
                Vector3 localRotation = new(localPlayer.Rotation.X, localPlayer.Rotation.Y, 0f);
                Vector3 dirUp = EULToDIR(localRotation + offsetVector);

                Vector2 spTop = ESP_Utilities.W2S_Basic(lensPosition + dirUp * scopeRadius);
                Vector2 spBottom = ESP_Utilities.W2S_Basic(lensPosition + dirUp * -scopeRadius);

                float scopeDiameter = Vector2.Distance(spBottom, spTop);
                ESP_Utilities.ScopeDiameter = scopeDiameter;
                if (!SCOPE_FACTORS_DEBUG)
                    ESP_Utilities.ScopeDiameter += ESP_Utilities.ScopeDiameter * ESP_Utilities.ScopeDiameterFactor;

                Vector2 scopeCenter = ESP_Utilities.W2S_Basic(lensPosition);
                ESP_Utilities.ScopeCenter = scopeCenter;
                if (!SCOPE_FACTORS_DEBUG)
                {
                    ESP_Utilities.ScopeCenter.X += ESP_Utilities.ScopeDiameter * ESP_Utilities.ScopeCenterFactorX;
                    ESP_Utilities.ScopeCenter.Y += ESP_Utilities.ScopeDiameter * ESP_Utilities.ScopeCenterFactorY;
                }

                ESP_Utilities.ScopeMatrix = scopeMatrix;
            }

            if (SCOPE_FACTORS_DEBUG)
            {
                string[] fData = File.ReadAllText("fov.dat").Split("\n");
                if (float.TryParse(fData[0].Trim(), out float scale) &&
                    float.TryParse(fData[1].Trim(), out float diameter) &&
                    float.TryParse(fData[2].Trim(), out float centerX) &&
                    float.TryParse(fData[3].Trim(), out float centerY))
                {
                    ESP_Utilities.ScopeScaleFactor = scale;

                    ESP_Utilities.ScopeDiameter += ESP_Utilities.ScopeDiameter * diameter;
                    
                    ESP_Utilities.ScopeCenter.X += ESP_Utilities.ScopeDiameter * centerX;
                    ESP_Utilities.ScopeCenter.Y += ESP_Utilities.ScopeDiameter * centerY;
                }

                Logger.WriteLine(espData.ScopeFOV);
            }
        }

        public static void UpdateScopeFactors(float scopeFOV, HashSet<string> weaponScopeIDs)
        {
            if (weaponScopeIDs == null)
                return;

            float scaleFactor = 0f;
            float diameterFactor = 0f;
            float centerFactorX = 0f;
            float centerFactorY = 0f;

            if (weaponScopeIDs.Contains("618ba27d9008e4636a67f61d")) // Vortex Razor HD Gen.2 1-6x24 30mm riflescope
            {
                scaleFactor = 62.75f;

                if (scopeFOV == 3.2f)
                    diameterFactor = -0.205f;
            }
            else if (weaponScopeIDs.Contains("544a3a774bdc2d3a388b4567")) // Leupold Mark 4 HAMR 4x24 DeltaPoint hybrid assault scope
            {
                scaleFactor = 57.1f;
                diameterFactor = -0.2f;
            }
            else if (weaponScopeIDs.Contains("57adff4f24597737f373b6e6")) // SIG Sauer BRAVO4 4x30 scope
            {
                scaleFactor = 53.9f;
                diameterFactor = -0.29f;
            }
            else if (weaponScopeIDs.Contains("5d2dc3e548f035404a1a4798")) // Monstrum Tactical Compact Prism Scope 2x32
            {
                scaleFactor = 66.8f;
                diameterFactor = -0.455f;
            }
            else if (weaponScopeIDs.Contains("5c0517910db83400232ffee5")) // Valday PS-320 1/6x scope
            {
                scaleFactor = 57.9f;
                diameterFactor = -0.19f;
                centerFactorY = 0.084f;
            }
            else if (weaponScopeIDs.Contains("6567e7681265c8a131069b0f")) // SIG TANGO6T 1-6x24 30mm riflescope
            {
                scaleFactor = 66.75f;

                if (scopeFOV == 3.2f)
                    diameterFactor = -0.174f;
            }
            else if (weaponScopeIDs.Contains("617151c1d92c473c770214ab")) // Schmidt & Bender PM II 1-8x24 30mm riflescope
            {
                scaleFactor = 64.9f;
                diameterFactor = 0.15f;

                if (scopeFOV == 3f)
                    diameterFactor = 0.1f;
            }
            else if (weaponScopeIDs.Contains("5dfe6104585a0c3e995c7b82")) // NcSTAR ADO P4 Sniper 3-9x42 riflescope
            {
                scaleFactor = 56.7f;
                diameterFactor = -0.1f;
            }
            else if (weaponScopeIDs.Contains("5c0a2cec0db834001b7ce47d") ||
                weaponScopeIDs.Contains("5c07dd120db834001c39092d")) // EOTech HHS-1 hybrid sight
            {
                scaleFactor = 61.7f;
                diameterFactor = -0.62f;
            }
            else if (weaponScopeIDs.Contains("626bb8532c923541184624b4")) // SwampFox Trihawk Prism Scope 3x30
            {
                scaleFactor = 64.1f;
                diameterFactor = -0.38f;
                centerFactorY = 0.021f;
            }
            else if (weaponScopeIDs.Contains("5c052a900db834001a66acbd") ||
                weaponScopeIDs.Contains("5c05293e0db83400232fff80")) // Trijicon ACOG TA01NSN 4x32 scope
            {
                scaleFactor = 56f;
                diameterFactor = -0.27f;
            }
            else if (weaponScopeIDs.Contains("59db7e1086f77448be30ddf3")) // Trijicon ACOG TA11D 3.5x35 scope
            {
                scaleFactor = 58.3f;
            }
            else if (weaponScopeIDs.Contains("57ac965c24597706be5f975c") ||
                weaponScopeIDs.Contains("57aca93d2459771f2c7e26db")) // ELCAN SpecterDR 1x/4x scope
            {
                scaleFactor = 56.5f;
                diameterFactor = -0.185f;
                centerFactorY = 0.024f;

                if (scopeFOV == 6f)
                    diameterFactor = -0.4f;
            }
            else if (weaponScopeIDs.Contains("5c1cdd512e22161b267d91ae")) // Kiba Arms Short Prism 2.5x scope
            {
                scaleFactor = 57.5f;
                diameterFactor = -0.23f;
                centerFactorY = 0.023f;
            }
            else if (weaponScopeIDs.Contains("5b3b99475acfc432ff4dcbee")) // EOTech Vudu 1-6x24 30mm riflescope
            {
                scaleFactor = 63.3f;
                diameterFactor = 0.07f;

                if (scopeFOV == 3.2f)
                    diameterFactor = -0.138f;
            }
            else if (weaponScopeIDs.Contains("5b2388675acfc4771e1be0be")) // Burris FullField TAC30 1-4x24 30mm riflescope
            {
                scaleFactor = 55.2f;
                diameterFactor = 0.32f;
            }
            else if (weaponScopeIDs.Contains("56ea70acd2720b844b8b4594")) // Hensoldt FF 4-16x56 34mm riflescope
            {
                scaleFactor = 61.9f;
                
                if (scopeFOV == 1.2f)
                    diameterFactor = -0.149f;
            }
            else if (weaponScopeIDs.Contains("61714eec290d254f5e6b2ffc")) // Schmidt & Bender PM II 3-12x50 34mm riflescope
            {
                scaleFactor = 63.6f;
                diameterFactor = 0.023f;
            }
            else if (weaponScopeIDs.Contains("62850c28da09541f43158cca")) // Schmidt & Bender PM II 5-25x56 34mm riflescope
            {
                scaleFactor = 55.3f;
                diameterFactor = 0.12f;
            }
            else if (weaponScopeIDs.Contains("5aa66be6e5b5b0214e506e97")) // Nightforce ATACR 7-35x56 34mm riflescope
            {
                scaleFactor = 64.5f;
                diameterFactor = -0.02f;
            }
            else if (weaponScopeIDs.Contains("5a37cb10c4a282329a73b4e7")) // Leupold Mark 4 LR 6.5-20x50 30mm riflescope
            {
                scaleFactor = 61.2f;
                diameterFactor = -0.015f;
            }
            else if (weaponScopeIDs.Contains("57c5ac0824597754771e88a9")) // March Tactical 3-24x42 FFP 30mm riflescope
            {
                scaleFactor = 58.5f;
                diameterFactor = 0.02f;
            }
            else if (weaponScopeIDs.Contains("5b3b6e495acfc4330140bd88")) // Armasight Vulcan MG 3.5x Bravo night vision scope
            {
                scaleFactor = 60.5f;
                diameterFactor = -0.1f;
                centerFactorY = 0.012f;
            }
            else if (weaponScopeIDs.Contains("5dff772da3651922b360bf91")) // VOMZ Pilad 4x32 25.4mm riflescope
            {
                scaleFactor = 68.6f;
                diameterFactor = -0.22f;
            }
            else if (weaponScopeIDs.Contains("5d0a3e8cd7ad1a6f6a3d35bd") ||
                weaponScopeIDs.Contains("5d0a3a58d7ad1a669c15ca14")) // KMZ 1P69 3-10x riflescope || KMZ 1P59 3-10x riflescope
            {
                scaleFactor = 57.45f;
                diameterFactor = 0.19f;
            }
            else if (weaponScopeIDs.Contains("5b3f7c1c5acfc40dc5296b1d")) // PU 3.5x riflescope
            {
                scaleFactor = 64.1f;
                diameterFactor = -0.18f;
                centerFactorX = -0.045f;
                centerFactorY = -0.09f;
            }
            else if (weaponScopeIDs.Contains("5c82342f2e221644f31c060e") ||
                weaponScopeIDs.Contains("5c82343a2e221644f31c0611") ||
                weaponScopeIDs.Contains("576fd4ec2459777f0b518431")) // BelOMO PSO-1 4x24 scope || BelOMO PSO-1M2 4x24 scope || BelOMO PSO-1M2-1 4x24 scope
            {
                scaleFactor = 57.3f;
            }
            else if (weaponScopeIDs.Contains("5a7c74b3e899ef0014332c29")) // NSPU-M 3.5x dovetail night vision scope
            {
                scaleFactor = 70.5f;
                diameterFactor = 0.36f;
                centerFactorY = 0.02f;
            }
            else if (weaponScopeIDs.Contains("5cf638cbd7f00c06595bc936")) // NPZ USP-1 "Tyulpan" 4x scope
            {
                scaleFactor = 62f;
                diameterFactor = -0.4f;
                centerFactorX = 1.47f;
                centerFactorY = 0.1f;
            }
            else if (weaponScopeIDs.Contains("5d1b5e94d7ad1a2b865a96b0")) // FLIR RS-32 2.25-9x 35mm 60Hz thermal riflescope
            {
                scaleFactor = 52f;
                diameterFactor = -0.32f;
                centerFactorY = 0.03f;
            }
            else if (weaponScopeIDs.Contains("5a1eaa87fcdbcb001865f75e")) // Trijicon REAP-IR thermal scope
            {
                scaleFactor = 53.2f;
                diameterFactor = -0.359f;
            }
            else if (weaponScopeIDs.Contains("6478641c19d732620e045e17")) // SIG Sauer ECHO1 1-2x30mm 30Hz thermal reflex scope
            {
                scaleFactor = 56.3f;
                diameterFactor = -0.38f;
                centerFactorY = 0.15f;
            }
            else if (weaponScopeIDs.Contains("63fc44e2429a8a166c7f61e6")) // Armasight Zeus-Pro 640 2-8x50 30Hz thermal scope
            {
                scaleFactor = 50f;
                centerFactorY = -0.01f;
            }
            else if (weaponScopeIDs.Contains("618a75f0bd321d49084cd399")) // NPZ 1P78-1 2.8x scope
            {
                scaleFactor = 51.9f;
                diameterFactor = -0.3f;
            }
            else if (weaponScopeIDs.Contains("62ea7c793043d74a0306e19f")) // Steyr AUG A1 STG77 1.5x optic sight
            {
                scaleFactor = 60.6f;
                diameterFactor = 0.1f;
            }
            else if (weaponScopeIDs.Contains("62ebd290c427473eff0baafb")) // Steyr AUG A3 M1 1.5x optic sight
            {
                scaleFactor = 56.6f;
                diameterFactor = 0.1f;
            }

            ESP_Utilities.ScopeScaleFactor = scaleFactor;
            ESP_Utilities.ScopeDiameterFactor = diameterFactor;
            ESP_Utilities.ScopeCenterFactorX = centerFactorX;
            ESP_Utilities.ScopeCenterFactorY = centerFactorY;
        }

        #region Utility Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float DEG2RAD(float degrees)
        {
            return degrees * (MathF.PI / 180f);
        }

        private static Vector3 EULToDIR(Vector3 eulerAngles)
        {
            eulerAngles.X = DEG2RAD(eulerAngles.X);
            eulerAngles.Y = DEG2RAD(eulerAngles.Y);

            float sinYaw = MathF.Sin(eulerAngles.X);
            float cosYaw = MathF.Cos(eulerAngles.X);

            float sinPitch = MathF.Sin(eulerAngles.Y);
            float cosPitch = MathF.Cos(eulerAngles.Y);
            cosPitch *= -1f;

            float x = sinYaw * cosPitch;
            float y = sinPitch;
            float z = cosYaw * cosPitch;

            Vector3 rotatedDirection = new(x, y, z);

            return Vector3.Normalize(rotatedDirection);
        }

        private static float GetComplementaryDegree(float degree)
        {
            degree %= 360f;

            if (degree <= 90f)
                return 90f + degree;
            else
                return degree - 270f;
        }

        #endregion
    }
}
