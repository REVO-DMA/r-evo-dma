using DebounceThrottle;
using Silk.NET.Input.Sdl;
using Silk.NET.Windowing.Sdl;
using SkiaSharp;
using Tarkov_DMA_Backend.LowLevel;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Tarkov.Toolkit.Features;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;

namespace Tarkov_DMA_Backend.ESP
{
    public static class ESP_Manager
    {
        private static XThread _windowManager;
        private static XThread _windowThread;

        private static bool ESP_Active = false;
        private static int ESP_ActiveMode = 0;
        private static int ESP_ActiveMonitor = -1;
        private static bool ESP_Ready => ESP_Config.Enabled && ESP_Active;

        public static EspWindow ESPWindow = null;
        public static FuserWindow FuserWindow = null;

        public static void Start()
        {
            SdlWindowing.RegisterPlatform();
            SdlWindowing.Use();

            SdlInput.RegisterPlatform();
            SdlInput.Use();

            _windowManager = new(WindowManager);
            _windowThread = new(WindowThread);
        }

        private static void ResetESP()
        {
            if (ESPWindow != null)
            {
                // Handle user closing the window manually
                if (!ESPWindow.IsRunning())
                {
                    ESP_Config.Enabled = false;
                    // TODO: sync enabled state with UI
                    goto cleanUp;
                }
                // Handle user disabling ESP from the UI
                else if (!ESP_Config.Enabled)
                    goto cleanUp;
                else if (ESP_Config.Mode != ESP_ActiveMode)
                    goto cleanUp;
                else if (ESP_Config.FuserMonitor != ESP_ActiveMonitor)
                    goto cleanUp;
                else
                    return;

                cleanUp:
                ESPWindow?.Dispose();
                ESPWindow = null;
                ESP_Active = false;
            }
            
            if (FuserWindow != null)
            {
                // Handle user closing the window manually
                if (!FuserWindow.IsRunning())
                {
                    ESP_Config.Enabled = false;
                    // TODO: sync enabled state with UI
                    goto cleanUp;
                }
                // Handle user disabling ESP from the UI
                else if (!ESP_Config.Enabled)
                    goto cleanUp;
                else if (ESP_Config.Mode != ESP_ActiveMode)
                    goto cleanUp;
                else if (ESP_Config.FuserMonitor != ESP_ActiveMonitor)
                    goto cleanUp;
                else
                    return;

            cleanUp:
                FuserWindow?.Dispose();
                FuserWindow = null;
                ESP_Active = false;
            }
        }

        private static void WindowManager()
        {
            // Wait for initial config from UI
            while (!ESP_Config.InitialSyncCompleted)
            {
                Thread.Sleep(300);
                continue;
            }

            while (true)
            {
                if (_windowManager.ShouldTerminate())
                    return;

                try
                {
                    if (!ESP_Active)
                    {
                        if (ESP_Config.Enabled)
                        {
                            if (ESP_Config.Mode == 1)
                            {
                                ESPWindow = new();
                                ESP_Active = true;
                                ESP_ActiveMode = ESP_Config.Mode;
                                ESP_ActiveMonitor = ESP_Config.FuserMonitor;
                            }
                            else if (ESP_Config.Mode == 2)
                            {
                                FuserWindow = new(ESP_Config.FuserMonitor);
                                ESP_Active = true;
                                ESP_ActiveMode = ESP_Config.Mode;
                                ESP_ActiveMonitor = ESP_Config.FuserMonitor;
                            }
                        }
                    }
                    else
                        ResetESP();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[ESP MANAGER] -> WindowManager(): {ex}");
                    ResetESP();
                }

                Thread.Sleep(16);
            }
        }

        private static void WindowThread()
        {
            while (true)
            {
                if (_windowThread.ShouldTerminate())
                    return;

                try
                {
                    if (ESP_Ready)
                    {
                        if (ESP_Config.Mode == 1)
                            ESPWindow?.Run();
                        if (ESP_Config.Mode == 2)
                            FuserWindow?.Run();
                    }

                    Thread.Sleep(16);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[ESP MANAGER] -> WindowThread(): {ex}");
                    ResetESP();
                }
            }
        }

        public static void OnESPResize(int width, int height)
        {
            ESP_Config.ESP_ResolutionX = width;
            ESP_Config.ESP_ResolutionY = height;
        }

        public static void Realtime(SKCanvas canvas)
        {
            try
            {
                Player LocalPlayer = EFTDMA.LocalPlayer;
                Player[] Players = EFTDMA.DisplayPlayers;

                if (LocalPlayer == null) return;
                if (Players == null) return;

                if (ESP_Utilities.IsUsingScope && ViewportManager.SCOPE_FACTORS_DEBUG)
                    canvas.DrawCircle(ESP_Utilities.ScopeCenter.X, ESP_Utilities.ScopeCenter.Y, ESP_Utilities.ScopeDiameter, PaintsManager.GenericStroke_White);

                RenderExfils(canvas, LocalPlayer.Position);

                RenderLoot(canvas, LocalPlayer.Position);

                RenderHumans(canvas, Players);
                RenderAIs(canvas, Players);

                RenderGrenades(canvas, LocalPlayer.Position);

                RenderClaymores(canvas, LocalPlayer.Position);

                RenderTripwires(canvas, LocalPlayer.Position);

                RenderHeldWeaponInfo(canvas);

                Vector2 crosshairCenter = RenderCrosshair(canvas);

                RenderSilentAimTarget(canvas, crosshairCenter);

                RenderAimbotFOV(canvas);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] Realtime() -> ERROR {ex}");
            }
        }

        private static void DrawCornerBox(SKCanvas c, SKPoint s, float w, float h, SKPaint p)
        {
            // Corner width
            float cw = w / 5f;
            // Corner height
            float ch = w / 5f;

            // Top Left
            c.DrawLine(s, new(s.X, s.Y + ch), p);
            c.DrawLine(s, new(s.X + cw, s.Y), p);

            // Top Right
            c.DrawLine(new(s.X + w - cw, s.Y), new(s.X + w, s.Y), p);
            c.DrawLine(new(s.X + w, s.Y), new(s.X + w, s.Y + ch), p);

            // Bottom Left
            c.DrawLine(new(s.X, s.Y + h - ch), new(s.X, s.Y + h), p);
            c.DrawLine(new(s.X, s.Y + h), new(s.X + cw, s.Y + h), p);

            // Bottom Right
            c.DrawLine(new(s.X + w - cw, s.Y + h), new(s.X + w, s.Y + h), p);
            c.DrawLine(new(s.X + w, s.Y + h - ch), new(s.X + w, s.Y + h), p);
        }

        private static void DrawBox(SKCanvas canvas, SKPoint topLeft, float width, float height, SKPaint paint)
        {
            SKRect rect = new(topLeft.X, topLeft.Y, topLeft.X + width, topLeft.Y + height);
            canvas.DrawRect(rect, paint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CanRenderHuman(Player player)
        {
            if (player == null ||
                !player.IsHuman ||
                player.Distance > ESP_Config.PlayerDrawDistance ||
                (ESP_Config.PlayerRenderStyle == 2 && !player.IsVisible))
            {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CanRenderAI(Player player)
        {
            if (player == null ||
                player.IsHuman ||
                player.Distance > ESP_Config.AIDrawDistance ||
                (ESP_Config.AIRenderStyle == 2 && !player.IsVisible))
            {
                return false;
            }

            return true;
        }

        private static void RenderHumanBones(SKCanvas canvas, Player player, ESP_Style.PlayerTypeStyle espStyle)
        {
            if (!ESP_Config.ShowPlayerBones)
                return;

            Span<int> boneLinkIndices = Player.BoneLinkIndices;
            Span<int> boneIndices = Player.BoneIndices;

            Span<Vector3> boneWorldPositions = player.BonePositions;
            Span<bool> visibilityInfo = player.VisibilityInfo;

            for (int ii = 0; ii < boneLinkIndices.Length; ii += 2)
            {
                int index1 = boneLinkIndices[ii];
                int index2 = boneLinkIndices[ii + 1];

                Vector3 pos1 = boneWorldPositions[index1];
                Vector3 pos2 = boneWorldPositions[index2];
                if (!ESP_Utilities.W2S(pos1, out SKPoint from) ||
                    !ESP_Utilities.W2S(pos2, out SKPoint to))
                {
                    continue;
                }

                if (ESP_Config.PlayerVisibilityCheckStyle == 1) // Per-Limb
                {
                    int visCheckIndex = Player.BoneIndexToVisCheckIndex(boneIndices[index2]);
                    if (visCheckIndex == -1)
                        continue;

                    bool isLimbVisible = visibilityInfo[visCheckIndex];

                    if (isLimbVisible)
                        canvas.DrawLine(from, to, espStyle.BoneVisible);
                    else
                        canvas.DrawLine(from, to, espStyle.BoneInvisible);
                }
                else if (ESP_Config.PlayerVisibilityCheckStyle == 2) // Whole Body
                {
                    if (player.IsVisible)
                        canvas.DrawLine(from, to, espStyle.BoneVisible);
                    else
                        canvas.DrawLine(from, to, espStyle.BoneInvisible);
                }
                else if (ESP_Config.PlayerVisibilityCheckStyle == 3) // Box Only
                {
                    canvas.DrawLine(from, to, espStyle.BoneVisible);
                }
            }
        }

        private static void RenderAIBones(SKCanvas canvas, Player player, ESP_Style.PlayerTypeStyle espStyle)
        {
            if (!ESP_Config.ShowAIBones)
                return;

            Span<int> boneLinkIndices = Player.BoneLinkIndices;
            Span<int> boneIndices = Player.BoneIndices;

            Span<Vector3> boneWorldPositions = player.BonePositions;
            Span<bool> visibilityInfo = player.VisibilityInfo;

            for (int ii = 0; ii < boneLinkIndices.Length; ii += 2)
            {
                int index1 = boneLinkIndices[ii];
                int index2 = boneLinkIndices[ii + 1];

                Vector3 pos1 = boneWorldPositions[index1];
                Vector3 pos2 = boneWorldPositions[index2];
                if (!ESP_Utilities.W2S(pos1, out SKPoint from) ||
                    !ESP_Utilities.W2S(pos2, out SKPoint to))
                {
                    continue;
                }

                if (ESP_Config.AIVisibilityCheckStyle == 2) // Whole Body
                {
                    if (player.IsVisible)
                        canvas.DrawLine(from, to, espStyle.BoneVisible);
                    else
                        canvas.DrawLine(from, to, espStyle.BoneInvisible);
                }
                else if (ESP_Config.AIVisibilityCheckStyle == 3) // Box Only
                {
                    canvas.DrawLine(from, to, espStyle.BoneVisible);
                }
            }
        }

        private readonly struct PlayerExtents(Vector2 head, Vector2 root, float width, float height)
        {
            public readonly Vector2 Head = head;
            public readonly Vector2 Root = root;
            public readonly float Width = width;
            public readonly float Height = height;
        }

        private static Result<PlayerExtents> GetPlayerExtents(Player player)
        {
            if (!ESP_Utilities.W2S(player.BonePositions[18], out Vector2 head, offscreenCheck: false) ||
                !ESP_Utilities.W2S(player.Position, out Vector2 root, offscreenCheck: false))
            {
                return Result<PlayerExtents>.Fail;
            }

            float adjHeight = Vector2.Distance(root, head);
            head.Y -= adjHeight * 0.16f;
            root.Y += adjHeight * 0.08f;

            float height = Vector2.Distance(root, head);
            float width = height / 1.85f;

            return new(true, new(head, root, width, height));
        }

        private static SKPoint RenderHumanBox(SKCanvas canvas, Player player, PlayerExtents extents, ESP_Style.PlayerTypeStyle espStyle)
        {
            SKPoint boxTopLeft = new(extents.Root.X - (extents.Width / 2f), extents.Head.Y);

            if (player.IsVisible)
            {
                if (ESP_Config.PlayerBoxStyle == 2)
                    DrawCornerBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxVisible);
                else if (ESP_Config.PlayerBoxStyle == 3)
                    DrawBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxVisible);
            }
            else
            {
                if (ESP_Config.PlayerBoxStyle == 2)
                    DrawCornerBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxInvisible);
                else if (ESP_Config.PlayerBoxStyle == 3)
                    DrawBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxInvisible);
            }

            return boxTopLeft;
        }

        private static SKPoint RenderAIBox(SKCanvas canvas, Player player, PlayerExtents extents, ESP_Style.PlayerTypeStyle espStyle)
        {
            SKPoint boxTopLeft = new(extents.Root.X - (extents.Width / 2f), extents.Head.Y);

            if (player.IsVisible)
            {
                if (ESP_Config.AIBoxStyle == 2)
                    DrawCornerBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxVisible);
                else if (ESP_Config.AIBoxStyle == 3)
                    DrawBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxVisible);
            }
            else
            {
                if (ESP_Config.AIBoxStyle == 2)
                    DrawCornerBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxInvisible);
                else if (ESP_Config.AIBoxStyle == 3)
                    DrawBox(canvas, boxTopLeft, extents.Width, extents.Height, espStyle.BoxInvisible);
            }

            return boxTopLeft;
        }

        private static void RenderHumanText(SKCanvas canvas, Player player, PlayerExtents extents, SKPoint boxTopLeft, ESP_Style.PlayerTypeStyle espStyle)
        {
            if (ESP_Config.BattleModeEnabled)
                return;

            int leftSideItemCount = 0;
            int maxVerticalItems = 999;
            if (ESP_Config.SmartDeclutterEnabled)
            {
                if (ESP_Config.PlayerShowHealth)
                    leftSideItemCount++;
                if (ESP_Config.PlayerShowLevel)
                    leftSideItemCount++;
                if (ESP_Config.PlayerShowKD)
                    leftSideItemCount++;

                if (extents.Height < ESP_Config.PlayerFontSize + (ESP_Config.PlayerFontSize * 0.3))
                    return;

                if (extents.Height < ESP_Config.PlayerFontSize * leftSideItemCount)
                {
                    maxVerticalItems = 0;
                    for (int i = 0; i < leftSideItemCount; i++)
                    {
                        if (extents.Height >= ESP_Config.PlayerFontSize * (i + 1))
                            maxVerticalItems++;
                        else
                            break;
                    }
                }
            }

            const float textSpacing = 8f;
            const float verticalTextSpacing = 2f;

            // Name & Team Number
            if (ESP_Config.PlayerShowName)
            {
                string playerName = player.Name;
                if (playerName != null)
                {
                    float xPos = extents.Root.X;
                    float yPos = extents.Root.Y - extents.Height - textSpacing;

                    if (ESP_Config.PlayerShowTeamNumber && player.TeamNumber != -1)
                        canvas.DrawText($"{player.TeamNumber} {playerName}", xPos, yPos, espStyle.TextCenter);
                    else
                        canvas.DrawText(playerName, xPos, yPos, espStyle.TextCenter);
                }
            }

            // Hands
            if (ESP_Config.PlayerShowHandsContents)
            {
                string Hands = player.ItemInHands;
                if (Hands != null)
                {
                    float xPos = extents.Root.X;
                    float yPos = extents.Root.Y + textSpacing + ESP_Config.PlayerFontSize;
                    canvas.DrawText(Hands, xPos, yPos, espStyle.TextCenter);

                }
            }

            // Top right side items
            {
                float yPosOffset = 0f;

                // Distance
                if (ESP_Config.PlayerShowDistance)
                {
                    float xPos = boxTopLeft.X + extents.Width + textSpacing;
                    float yPos = boxTopLeft.Y + ESP_Config.PlayerFontSize + yPosOffset;
                    yPosOffset += verticalTextSpacing + ESP_Config.PlayerFontSize;
                    canvas.DrawText($"{player.Distance:F0}m", xPos, yPos, espStyle.TextLeft);
                }
            }

            // Top left side items
            {
                float yPosOffset = 0f;
                int renderedVerticalItems = 0;

                // Health
                if (ESP_Config.PlayerShowHealth &&
                    renderedVerticalItems < maxVerticalItems)
                {
                    float xPos = boxTopLeft.X - textSpacing;
                    float yPos = boxTopLeft.Y + ESP_Config.PlayerFontSize + yPosOffset;
                    yPosOffset += verticalTextSpacing + ESP_Config.PlayerFontSize;
                    renderedVerticalItems++;
                    canvas.DrawText($"{player.HealthPercent} HP", xPos, yPos, espStyle.TextRight);
                }

                // Level
                if (ESP_Config.PlayerShowLevel &&
                    renderedVerticalItems < maxVerticalItems)
                {
                    float xPos = boxTopLeft.X - textSpacing;
                    float yPos = boxTopLeft.Y + ESP_Config.PlayerFontSize + yPosOffset;
                    yPosOffset += verticalTextSpacing + ESP_Config.PlayerFontSize;
                    renderedVerticalItems++;
                    canvas.DrawText($"{player.Level} Lv.", xPos, yPos, espStyle.TextRight);
                }

                // K/D
                if (ESP_Config.PlayerShowKD &&
                    renderedVerticalItems < maxVerticalItems)
                {
                    float xPos = boxTopLeft.X - textSpacing;
                    float yPos = boxTopLeft.Y + ESP_Config.PlayerFontSize + yPosOffset;
                    yPosOffset += verticalTextSpacing + ESP_Config.PlayerFontSize;
                    renderedVerticalItems++;
                    canvas.DrawText($"{player.KillDeathRatio} K/D", xPos, yPos, espStyle.TextRight);
                }
            }
        }

        private static void RenderAIText(SKCanvas canvas, Player player, PlayerExtents extents, SKPoint boxTopLeft, ESP_Style.PlayerTypeStyle espStyle)
        {
            if (ESP_Config.BattleModeEnabled)
                return;

            int leftSideItemCount = 0;
            int maxVerticalItems = 999;
            if (ESP_Config.SmartDeclutterEnabled)
            {
                if (ESP_Config.AIShowHealth)
                    leftSideItemCount++;

                if (extents.Height < ESP_Config.AIFontSize + (ESP_Config.AIFontSize * 0.3))
                    return;

                if (extents.Height < ESP_Config.AIFontSize * leftSideItemCount)
                {
                    maxVerticalItems = 0;
                    for (int i = 0; i < leftSideItemCount; i++)
                    {
                        if (extents.Height >= ESP_Config.AIFontSize * (i + 1))
                            maxVerticalItems++;
                        else
                            break;
                    }
                }
            }

            const float textSpacing = 8f;
            const float verticalTextSpacing = 2f;

            // Name & Team Number
            if (ESP_Config.AIShowName)
            {
                string playerName = player.Name;
                if (playerName != null)
                {
                    float xPos = extents.Root.X;
                    float yPos = extents.Root.Y - extents.Height - textSpacing;
                    canvas.DrawText(playerName, xPos, yPos, espStyle.TextCenter);
                }
            }

            // Hands
            if (ESP_Config.AIShowHandsContents)
            {
                string Hands = player.ItemInHands;
                if (Hands != null)
                {
                    float xPos = extents.Root.X;
                    float yPos = extents.Root.Y + textSpacing + ESP_Config.AIFontSize;
                    canvas.DrawText(Hands, xPos, yPos, espStyle.TextCenter);
                }
            }

            // Top right side items
            {
                float yPosOffset = 0f;

                // Distance
                if (ESP_Config.AIShowDistance)
                {
                    float xPos = boxTopLeft.X + extents.Width + textSpacing;
                    float yPos = boxTopLeft.Y + ESP_Config.AIFontSize + yPosOffset;
                    yPosOffset += verticalTextSpacing + ESP_Config.AIFontSize;
                    canvas.DrawText($"{player.Distance:F0}m", xPos, yPos, espStyle.TextLeft);
                }
            }

            // Top left side items
            {
                float yPosOffset = 0f;
                int renderedVerticalItems = 0;

                // Health
                if (ESP_Config.AIShowHealth &&
                    renderedVerticalItems < maxVerticalItems)
                {
                    float xPos = boxTopLeft.X - textSpacing;
                    float yPos = boxTopLeft.Y + ESP_Config.AIFontSize + yPosOffset;
                    yPosOffset += verticalTextSpacing + ESP_Config.AIFontSize;
                    renderedVerticalItems++;
                    canvas.DrawText($"{player.HealthPercent} HP", xPos, yPos, espStyle.TextRight);
                }
            }
        }

        private static void RenderHumans(SKCanvas canvas, Player[] players)
        {
            int playersLength = players.Length;
            if (playersLength == 0) return;

            for (int i = 0; i < playersLength; i++)
            {
                Player player = players[i];

                if (!CanRenderHuman(player))
                    continue;

                if (!ESP_Style.Styles.TryGetValue(player.Type, out var espStyle))
                    continue;

                Result<PlayerExtents> extents = GetPlayerExtents(player);
                if (!extents)
                    continue;

                RenderHumanBones(canvas, player, espStyle);
                SKPoint boxTopLeft = RenderHumanBox(canvas, player, extents, espStyle);
                RenderHumanText(canvas, player, extents, boxTopLeft, espStyle);
            }
        }

        private static void RenderAIs(SKCanvas canvas, Player[] players)
        {
            int playersLength = players.Length;
            if (playersLength == 0) return;

            for (int i = 0; i < playersLength; i++)
            {
                Player player = players[i];

                if (!CanRenderAI(player))
                    continue;

                if (!ESP_Style.Styles.TryGetValue(player.Type, out var espStyle))
                    continue;

                Result<PlayerExtents> extents = GetPlayerExtents(player);
                if (!extents)
                    continue;

                RenderAIBones(canvas, player, espStyle);
                SKPoint boxTopLeft = RenderAIBox(canvas, player, extents, espStyle);
                RenderAIText(canvas, player, extents, boxTopLeft, espStyle);
            }
        }

        private static void RenderGrenades(SKCanvas canvas, Vector3 LocalPlayerPosition)
        {
            IReadOnlyDictionary<ulong, Grenade> Grenades = EFTDMA.Grenades;

            if (!ESP_Config.ShowGrenades) return;
            if (Grenades == null) return;
            if (Grenades.Count == 0) return;

            float grenadeDrawDistance = ESP_Config.GrenadeDrawDistance;

            foreach (var grenade in Grenades)
            {
                if (!grenade.Value.IsActive)
                    continue;

                float distance = Vector3.Distance(LocalPlayerPosition, grenade.Value.Position);

                if (distance > grenadeDrawDistance)
                    continue;

                if (!ESP_Utilities.W2S(grenade.Value.Position, out Vector2 espPosition)) continue;

                // Draw grenade
                canvas.DrawCircle(espPosition.X, espPosition.Y, (10f / distance) * 8f, PaintsManager.Grenade_Circle);
            }
        }

        private static void RenderClaymores(SKCanvas canvas, Vector3 LocalPlayerPosition)
        {
            IReadOnlyList<Claymore> Claymores = EFTDMA.Claymores;

            if (!ESP_Config.ShowClaymores) return;
            if (Claymores == null) return;
            if (Claymores.Count == 0) return;

            float claymoreDrawDistance = ESP_Config.ClaymoreDrawDistance;
            float claymoreWarningDistance = ESP_Config.ClaymoreWarningDistance;

            foreach (var claymore in Claymores)
            {
                float distance = Vector3.Distance(LocalPlayerPosition, claymore.Position);

                if (distance > claymoreDrawDistance)
                    continue;

                if (!ESP_Utilities.W2S(claymore.Position, out Vector2 espPosition)) continue;

                if (distance <= claymoreWarningDistance)
                    canvas.DrawCircle(espPosition.X, espPosition.Y, 6f, PaintsManager.Claymore_WarningCircle);

                // Draw claymore text
                canvas.DrawText($"[{distance:F0}m] Claymore", espPosition.X, espPosition.Y + 18f, PaintsManager.Claymore_Text);
            }
        }

        private static void RenderTripwires(SKCanvas canvas, Vector3 LocalPlayerPosition)
        {
            var tripwires = EFTDMA.Tripwires;
            if (tripwires == null) return;

            foreach (var tripwire in tripwires.Values)
            {
                Vector3 between = Vector3.Lerp(tripwire.FromPosition, tripwire.ToPosition, 0.5f) + new Vector3(0f, 0.15f, 0f);
                float distance = Vector3.Distance(LocalPlayerPosition, between);

                if (distance > ESP_Config.TripwiresDrawDistance)
                    continue;

                if (!ESP_Utilities.W2S(tripwire.FromPosition, out SKPoint first) ||
                    !ESP_Utilities.W2S(tripwire.ToPosition, out SKPoint second))
                {
                    continue;
                }

                if (!ESP_Utilities.W2S(between, out SKPoint textPos))
                    continue;

                canvas.DrawLine(first, second, PaintsManager.GenericStroke_Red);
                canvas.DrawText($"[{distance:F0}m] Tripwire ({tripwire.ShortName})", textPos, PaintsManager.Tripwires_Text);
            }
        }

        private static void RenderLoot(SKCanvas canvas, Vector3 LocalPlayerPosition)
        {
            if (ESP_Config.BattleModeEnabled) return;
            if (!ESP_Config.ShowLoot) return;
            if (EFTDMA.Loot == null) return;
            if (EFTDMA.Loot.DisplayLoot == null) return;

            float LootDrawDistance = ESP_Config.LootDrawDistance;

            try
            {
                // Draw Quest locations
                var questLocations = QuestManager.ActiveQuests;
                if (ItemManager.ShowQuestItems && questLocations != null)
                {
                    foreach (var location in questLocations)
                    {
                        float distance = Vector3.Distance(LocalPlayerPosition, location.Position);

                        if (distance > LootDrawDistance)
                            continue;

                        if (!ESP_Utilities.W2S(location.Position, out Vector2 espPosition)) continue;

                        string distanceString = distance.ToString("F2");
                        SKPaint paint = default;
                        // TODO: make a UI option to control the distance
                        if (distance <= 8f)
                        {
                            if (distance > 3.5f)
                                paint = PaintsManager.LootItem_Text_TooFar;
                            else if (distance <= 3.5f)
                                paint = PaintsManager.LootItem_Text_WithinReach;
                        }
                        else
                        {
                            distanceString = distance.ToString("F0");
                            paint = PaintsManager.LootItem_Text_Default;
                        }

                        // Only draw the precise position circle if the user is closer than 10m
                        if (distance <= 10f)
                            canvas.DrawCircle(espPosition.X, espPosition.Y, 6f, PaintsManager.LootItem_Circle);

                        // Draw player text
                        float textYOffset = 18f;

                        // Draw distance
                        canvas.DrawText($"{distanceString}m", espPosition.X, espPosition.Y + textYOffset, paint);
                        textYOffset += ESP_Config.LootFontSize;
                        // Draw item name
                        canvas.DrawText($"(Quest) {location.Name}", espPosition.X, espPosition.Y + textYOffset, PaintsManager.LootItem_Text_Default);
                    }
                }

                var LootItems = EFTDMA.Loot.DisplayLoot;
                foreach (var lootItemRaw in LootItems)
                {
                    var lootItem = lootItemRaw.Value;

                    // Get the first item in the container
                    if (lootItem is LootContainer container && container.FilteredDisplayItems.Count > 0)
                    {
                        lootItem = container.FilteredDisplayItems[0];
                    }

                    float distance = Vector3.Distance(LocalPlayerPosition, lootItem.Position);

                    if (distance > LootDrawDistance)
                        continue;

                    if (!ESP_Utilities.W2S(lootItem.Position, out Vector2 espPosition)) continue;

                    string distanceString = distance.ToString("F2");
                    SKPaint paint = default;
                    if (distance <= 8f)
                    {
                        if (distance > LootThroughWalls.newLootRaycastDistance)
                            paint = PaintsManager.LootItem_Text_TooFar;
                        else if (distance <= LootThroughWalls.newLootRaycastDistance)
                            paint = PaintsManager.LootItem_Text_WithinReach;
                    }
                    else
                    {
                        distanceString = distance.ToString("F0");
                        paint = PaintsManager.LootItem_Text_Default;
                    }

                    const float preciseLocationDiameter = 6f;

                    // Only draw the precise position circle if the user is closer than 10m
                    if (distance <= 10f)
                    {
                        var circleColor = lootItem?._item?.ActiveLootFilter?.ColorPaint;
                        if (circleColor != null)
                            canvas.DrawCircle(espPosition.X, espPosition.Y, preciseLocationDiameter, circleColor);
                        else
                            canvas.DrawCircle(espPosition.X, espPosition.Y, preciseLocationDiameter, PaintsManager.LootItem_Circle);
                    }

                    // Draw player text
                    float textYOffset = preciseLocationDiameter + ESP_Config.LootFontSize;

                    // Draw distance
                    canvas.DrawText($"{distanceString}m", espPosition.X, espPosition.Y + textYOffset, paint);
                    textYOffset += ESP_Config.LootFontSize;
                    // Draw item name
                    var textColor = lootItem?._item?.ActiveLootFilter?.TextColorPaint;
                    if (textColor != null)
                        canvas.DrawText(lootItem.GetLabel(), espPosition.X, espPosition.Y + textYOffset, textColor);
                    else
                        canvas.DrawText(lootItem.GetLabel(), espPosition.X, espPosition.Y + textYOffset, PaintsManager.LootItem_Text_Default);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] RenderLoot() -> ERROR: {ex}");
            }
        }

        private static void RenderExfils(SKCanvas canvas, Vector3 LocalPlayerPosition)
        {
            if (ESP_Config.BattleModeEnabled) return;
            if (!ESP_Config.ShowExtractions) return;
            if (UICache.MapConfig == null) return;
            if (EFTDMA.Exfils == null) return;
            if (EFTDMA.Exfils.Count == 0) return;

            float ExtractionDrawDistance = ESP_Config.ExtractionDrawDistance;
            byte ExtractionVisibility = ESP_Config.ExtractionVisibility;

            try
            {
                foreach (Exfil exfil in EFTDMA.Exfils)
                {
                    if (exfil == null || exfil.Name.Length == 0) continue;

                    float distance = Vector3.Distance(LocalPlayerPosition, exfil.Position);

                    if (distance > ExtractionDrawDistance)
                        continue;

                    if (ExtractionVisibility != 1)
                        if (exfil.Status == ExfilStatus.Closed) continue;

                    if (!ESP_Utilities.W2S(exfil.Position, out Vector2 espPosition)) continue;

                    string statusText = "Status: ";
                    SKPaint paint;
                    if (exfil.Status == ExfilStatus.Open)
                    {
                        statusText += "Open";
                        paint = PaintsManager.ExfilText_Open;
                    }
                    else if (exfil.Status == ExfilStatus.Pending)
                    {
                        statusText += "Pending";
                        paint = PaintsManager.ExfilText_Pending;
                    }
                    else if (exfil.Status == ExfilStatus.Closed)
                    {
                        statusText += "Closed";
                        paint = PaintsManager.ExfilText_Closed;
                    }
                    else
                        continue;

                    float textYOffset = 0f;

                    // Draw exfil name
                    canvas.DrawText(exfil.Name, espPosition.X, espPosition.Y + textYOffset, paint);
                    textYOffset += ESP_Config.ExfilFontSize;
                    // Draw exfil status
                    canvas.DrawText(statusText, espPosition.X, espPosition.Y + textYOffset, paint);
                    textYOffset += ESP_Config.ExfilFontSize;
                    // Draw exfil distance
                    canvas.DrawText($"{distance:F0}m", espPosition.X, espPosition.Y + textYOffset, paint);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] RenderExfils() -> ERROR: {ex}");
            }
        }

        private static void RenderHeldWeaponInfo(SKCanvas canvas)
        {
            if (!ESP_Config.ShowHeldWeaponInfo)
                return;

            Player localPlayer = EFTDMA.LocalPlayer;
            if (localPlayer == null)
                return;

            var ammoData = LocalHandsManager.WeaponAmmoData;
            var fireModeData = LocalHandsManager.WeaponFireModeData;
            var ammoLevel = LocalHandsManager.WeaponAmmoLevel;

            float ResolutionX = ESP_Config.ESP_ResolutionX;
            float ResolutionY = ESP_Config.ESP_ResolutionY;

            float ammoLevelWidth = PaintsManager.AmmoCount_BulletCountText.MeasureText(ammoLevel);

            const float xOffset = 200f;
            const float lowerYOffset = 35f;
            const float upperYOffset = lowerYOffset + 45f;

            canvas.DrawText(ammoData, ResolutionX - xOffset, ResolutionY - upperYOffset, PaintsManager.HeldWeaponInfoText_Left);
            canvas.DrawText(fireModeData, ResolutionX - xOffset + ammoLevelWidth, ResolutionY - upperYOffset, PaintsManager.HeldWeaponInfoText_Right);
            canvas.DrawText(ammoLevel, ResolutionX - xOffset, ResolutionY - lowerYOffset, PaintsManager.AmmoCount_BulletCountText);
        }

        private static readonly DebounceDispatcher DynamicCrosshairDebouncer = new(120);
        private static bool CanShowDynamicCrosshair = false;
        private static bool CanShowDynamicCrosshairNewState = false;

        private static Vector2 RenderCrosshair(SKCanvas canvas)
        {
            float CenterX = ESP_Config.ESP_ResolutionX / 2f;
            float CenterY = ESP_Config.ESP_ResolutionY / 2f;

            if (!ESP_Config.ShowCrosshair)
                return new(CenterX, CenterY);

            float crosshairScale = ESP_Config.CrosshairScale;

            if (ESP_Config.DynamicCrosshair)
            {
                Vector3 firePortIntersectionPoint = VC_Structs.Vector3.ToSystem(ESP_Utilities.LastESPData.firePortIntersectionPoint);
                Vector3 firePortPosition3D = VC_Structs.Vector3.ToSystem(ESP_Utilities.LastESPData.firePortPos);
                if (firePortIntersectionPoint != Vector3.Zero &&
                    firePortPosition3D != Vector3.Zero)
                {
                    if (ESP_Utilities.W2S(firePortIntersectionPoint, out SKPoint firePortDistant, offscreenCheck: true, useTolerance: false) &&
                        ESP_Utilities.W2S(firePortPosition3D, out SKPoint firePortPosition, offscreenCheck: true, useTolerance: false))
                    {
                        if (CanShowDynamicCrosshairNewState != true)
                        {
                            CanShowDynamicCrosshairNewState = true;
                            DynamicCrosshairDebouncer.Debounce(() =>
                            {
                                CanShowDynamicCrosshair = true;
                            });
                        }
                        

                        if (CanShowDynamicCrosshair)
                        {
                            CenterX = firePortDistant.X;
                            CenterY = firePortDistant.Y;

                            if (ESP_Config.FakeLaser)
                                canvas.DrawLine(firePortPosition, firePortDistant, PaintsManager.GenericStroke_White);
                        }
                    }
                    else
                    {
                        if (CanShowDynamicCrosshairNewState != false)
                        {
                            CanShowDynamicCrosshairNewState = false;
                            DynamicCrosshairDebouncer.Debounce(() =>
                            {
                                CanShowDynamicCrosshair = false;
                            });
                        }
                    }
                }
                else
                {
                    if (CanShowDynamicCrosshairNewState != false)
                    {
                        CanShowDynamicCrosshairNewState = false;
                        DynamicCrosshairDebouncer.Debounce(() =>
                        {
                            CanShowDynamicCrosshair = false;
                        });
                    }
                }
            }

            if (ESP_Config.CrosshairStyle == 1)
            {
                // Dot Style
                canvas.DrawCircle(CenterX, CenterY, 8f * crosshairScale, PaintsManager.GenericFill_White);
            }
            else if (ESP_Config.CrosshairStyle == 2)
            {
                // Normal (+) Style

                // Horizontal line
                float crosshairStartX = CenterX - (15f * crosshairScale);
                float crosshairEndX = CenterX + (15f * crosshairScale);
                canvas.DrawLine(crosshairStartX, CenterY, crosshairEndX, CenterY, PaintsManager.GenericStroke_White);
                // Vertical line
                float crosshairStartY = CenterY - (15f * crosshairScale);
                float crosshairEndY = CenterY + (15f * crosshairScale);
                canvas.DrawLine(CenterX, crosshairStartY, CenterX, crosshairEndY, PaintsManager.GenericStroke_White);
            }

            return new(CenterX, CenterY);
        }

        private static void RenderSilentAimTarget(SKCanvas canvas, Vector2 center)
        {
            if (!ESP_Config.ShowShowSilentAimTarget ||
                Aimbot.LastTargetPosition == Vector3.Zero)
            {
                return;
            }

            if (ESP_Utilities.W2S(Aimbot.LastTargetPosition, out Vector2 espBonePosition, offscreenCheck: false))
                canvas.DrawLine(center.X, center.Y, espBonePosition.X, espBonePosition.Y, PaintsManager.GenericStroke_White);
        }

        private static void RenderAimbotFOV(SKCanvas canvas)
        {
            if (!Aimbot.Enabled || !ESP_Config.ShowAimbotFOV) return;

            float ResolutionX = ESP_Config.ESP_ResolutionX;
            float ResolutionY = ESP_Config.ESP_ResolutionY;

            float CenterX = ResolutionX / 2f;
            float CenterY = ResolutionY / 2f;

            canvas.DrawCircle(CenterX, CenterY, Aimbot.FOV, PaintsManager.GenericStroke_White);
        }
    }
}
