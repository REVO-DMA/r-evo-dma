namespace cs2_dma_esp.CS2.Features
{
    public static class RCS
    {
        private static Thread _t1;

        private static readonly Stopwatch SensitivitySW = new();
        private static float Sensitivity;

        private static Vector2 LastPunch = Vector2.Zero;
        private static uint LastShotIndex = 0;

        public static void Start()
        {
            SensitivitySW.Start();

            _t1 = new Thread(Run)
            {
                IsBackground = true
            };

            _t1.Start();
        }

        private static void Run()
        {
            while (true)
            {
                try
                {
                    if (Game.LocalPlayer == null)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    else if (SensitivitySW.ElapsedMilliseconds >= 1000)
                    {
                        UpdateSensitivity();
                        SensitivitySW.Restart();
                    }

                    uint shotIndex = Memory.ReadValue<uint>(Game.LocalPlayer.Pawn + SDK.Client.C_CSPlayerPawnBase.m_iShotsFired, false);

                    if (shotIndex > 1)
                    {
                        if (shotIndex > LastShotIndex)
                        {
                            Vector2 currPunch = GetVecPunch(Game.LocalPlayer.Pawn);

                            Vector2 newPunch;
                            newPunch.X = (currPunch.X - LastPunch.Y) * 2f / (0.022f * Sensitivity);
                            newPunch.Y = -(currPunch.X - LastPunch.X) * 2f / (0.022f * Sensitivity);

                            Input.MouseMove(new(newPunch.X, newPunch.Y));

                            LastPunch = currPunch;
                            LastShotIndex = shotIndex;
                        }
                    }
                    else
                    {
                        LastPunch = Vector2.Zero;
                        LastShotIndex = 0;
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[RCS] Error: {ex}");
                }

                Thread.Sleep(2);
            }
        }

        private struct CUtlVector
        {
            public ulong Count;
            public ulong Data;
        }

        private static Vector2 GetVecPunch(ulong pawn)
        {
            Vector2 data = default;

            var aimPunchCache = Memory.ReadValue<CUtlVector>(pawn + SDK.Client.C_CSPlayerPawn.m_aimPunchCache, false);

            if (aimPunchCache.Count > 0 && aimPunchCache.Count < 0xFFFF)
                return data;

            data = Memory.ReadValue<Vector2>(aimPunchCache.Data + ((aimPunchCache.Count - 1) * 12), false);

            return data;
        }

        private static float GetFOV_Multiplier(ulong pawn)
        {
            return Memory.ReadValue<float>(pawn + SDK.Client.C_BasePlayerPawn.m_flFOVSensitivityAdjust, false);
        }

        private static void UpdateSensitivity()
        {
            if (Game.LocalPlayer == null)
                return;

            Task.Run(() =>
            {
                Sensitivity = Input.GetSensitivity() * GetFOV_Multiplier(Game.LocalPlayer.Pawn);
            });
        }
    }
}
