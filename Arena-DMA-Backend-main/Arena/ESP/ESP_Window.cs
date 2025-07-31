using arena_dma_backend.Arena;
using Silk.NET.Core;
using Silk.NET.GLFW;
using Silk.NET.Input.Glfw;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;
using Thread = System.Threading.Thread;
using Window = Silk.NET.Windowing.Window;

namespace arena_dma_backend.ESP
{
    public class ESP_Window
    {
        public static readonly SKTypeface FontFamilyRegular;
        public static readonly SKTypeface FontFamilyBold;

        private readonly IWindow _window;

        private readonly IMonitor _monitor;
        private GRContext _grContext;
        private GRBackendRenderTarget _grBackendRenderTarget;

        // FPS Tracking
        private readonly Stopwatch _fpsStopwatch = new();
        private readonly Stopwatch _espCacheRefreshSW = new();
        private int _fps = 0;
        private string _fpsString = "";

        [Obsolete]
        static ESP_Window()
        {
            GlfwWindowing.RegisterPlatform();
            GlfwWindowing.Use();

            GlfwInput.RegisterPlatform();

            // Prevent created fullscreen windows from iconifying on focus loss
            GlfwProvider.GLFW.Value.WindowHint(WindowHintBool.AutoIconify, false);

            // Load fonts
            FontFamilyRegular = SKTypeface.FromTypeface(SKTypeface.FromStream(new MemoryStream(File.ReadAllBytes("Fonts\\Neo Sans Std Regular.otf"))), SKTypefaceStyle.Normal);
            FontFamilyBold = SKTypeface.FromTypeface(SKTypeface.FromStream(new MemoryStream(File.ReadAllBytes("Fonts\\Neo Sans Std Bold.otf"))), SKTypefaceStyle.Bold);
        }

        public ESP_Window()
        {
            // Init window

            var windowPlatform = Window.GetWindowPlatform(false) ?? throw new Exception("[ESP Window] Unable to acquire a window platform.");

            if (ESP_Config.ESP_FuserMode)
                _monitor = windowPlatform.GetMonitors().ElementAt(ESP_Config.ESP_MonitorIndex);
            else
                _monitor = windowPlatform.GetMainMonitor();

            var vm = _monitor.VideoMode;

            if (ESP_Config.ESP_FuserMode)
            {
                ESP_Config.ESP_ResolutionX = vm.Resolution!.Value.X;
                ESP_Config.ESP_ResolutionY = vm.Resolution!.Value.Y;
            }
            else
            {
                ESP_Config.ESP_ResolutionX = 1280;
                ESP_Config.ESP_ResolutionY = 720;
            }

            WindowOptions options = WindowOptions.Default;
            if (ESP_Config.ESP_FuserMode)
            {
                options.Position = new(_monitor.Bounds.Origin.X, _monitor.Bounds.Origin.Y);
                options.WindowBorder = WindowBorder.Hidden;
                options.WindowState = WindowState.Fullscreen;
                options.TopMost = true;
            }
            else
            {
                options.WindowBorder = WindowBorder.Fixed;
                options.Position = new(_monitor.Bounds.Origin.X + 50, _monitor.Bounds.Origin.Y + 50);
            }

            options.Title = "EVO DMA - Arena ESP";
            options.Size = new(ESP_Config.ESP_ResolutionX, ESP_Config.ESP_ResolutionY);
            options.PreferredStencilBufferBits = 8;
            options.PreferredBitDepth = new(8, 8, 8, 8);
            options.VSync = false;
            options.FramesPerSecond = vm.RefreshRate ?? 144;

            _window = _monitor.CreateWindow(options);

            _window.Load += OnLoad;
            _window.Render += OnRender;

            // Track FPS
            _fpsStopwatch.Start();
            _espCacheRefreshSW.Start();
        }

        public void DoRender() => _window.Run(); // Blocking

        private void OnLoad()
        {
            if (ESP_Config.ESP_FuserMode) _window.Monitor = _monitor;

            _grContext = GRContext.CreateGl();
            _grBackendRenderTarget = new GRBackendRenderTarget(_window.Size.X, _window.Size.Y, 0, 8, new GRGlFramebufferInfo(0, 0x8058));

            SetWindowIcon();
        }

        private void SetWindowIcon()
        {
            if (_window is null)
                return;

            var fileBytes = File.ReadAllBytes("evoIcon.png");
            var bitmap = SKBitmap.Decode(fileBytes);

            byte[] bytes = new byte[bitmap.Pixels.Length * 4];

            int index = 0;
            foreach (var pixel in bitmap.Pixels)
            {
                bytes[index++] = pixel.Red;
                bytes[index++] = pixel.Green;
                bytes[index++] = pixel.Blue;
                bytes[index++] = pixel.Alpha;
            }

            RawImage icon = new(128, 128, new Memory<byte>(bytes));

            _window.SetWindowIcon(ref icon);
        }

        private void OnRender(double dt)
        {
            // Get FPS and cleanup every second
            if (_fpsStopwatch.ElapsedMilliseconds >= 1000)
            {
                _fpsString = _fps.ToString();
                _fps = 0;

                //_grContext?.ResetContext();
                _grContext?.PurgeResources();

                _fpsStopwatch.Restart();
            }

            _fps++;

            using var surface = SKSurface.Create(_grContext, _grBackendRenderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            var canvas = surface.Canvas;
            canvas.Clear();

            // Draw nothing when out of match, and throttle render loop
            if (!CameraManager.PlayerIsInRaid)
            {
                canvas.Flush();

                Thread.Sleep(100);
                return;
            }

            // Draw FPS
            canvas.DrawText(_fpsString, ESP_Config.ESP_ResolutionX, 20f, PaintsManager.FPSText);

            RenderESP(canvas);

            canvas.Flush();
        }

        private static void RenderESP(SKCanvas canvas)
        {
            try
            {
                ESP_Utilities.UpdateW2S();

                var players = Game.Players.Values.AsEnumerable();
                if (!players.Any())
                    return;

                int[] BoneLinkIndices = Player.BoneLinkIndices;

                foreach (Player player in players)
                {
                    if (!player.CanRender())
                        continue;

                    try
                    {
                        SKPoint[] BoneScreenPositions = new SKPoint[BoneLinkIndices.Length];

                        for (int ii = 0; ii < BoneLinkIndices.Length; ii += 2)
                        {
                            int index1 = BoneLinkIndices[ii];
                            int index2 = BoneLinkIndices[ii + 1];

                            // Get bone joints screen positions
                            Vector3 pos1 = player.BonePositions[index1];
                            if (!ESP_Utilities.W2S(pos1, true, out var from)) continue;

                            Vector3 pos2 = player.BonePositions[index2];
                            if (!ESP_Utilities.W2S(pos2, true, out var to)) continue;

                            // Draw head circle if this is the first iteration
                            if (ii == 0)
                            {
                                if (player.IsTeammate)
                                    canvas.DrawCircle(from.X, from.Y, to.Y - from.Y, PaintsManager.PlayerBone_Teammate);
                                else
                                    canvas.DrawCircle(from.X, from.Y, to.Y - from.Y, PaintsManager.PlayerBone_Enemy);
                            }
                            else
                            {
                                BoneScreenPositions[ii] = new(from.X, from.Y); // From
                                BoneScreenPositions[ii + 1] = new(to.X, to.Y); // To
                            }
                        }

                        if (player.IsTeammate)
                            canvas.DrawPoints(SKPointMode.Lines, BoneScreenPositions, PaintsManager.PlayerBone_Teammate);
                        else
                            canvas.DrawPoints(SKPointMode.Lines, BoneScreenPositions, PaintsManager.PlayerBone_Enemy);

                        if (!ESP_Utilities.W2S(player.Position, true, out var rootPosition)) continue;

                        canvas.DrawText(player.ESP_NameString, rootPosition.X, rootPosition.Y + 18f, PaintsManager.WhiteText);
                        canvas.DrawText(player.ESP_DistanceString, rootPosition.X, rootPosition.Y + 32f, PaintsManager.WhiteText);
                    }
                    catch (Exception ex) { Logger.WriteLine($"[RENDER] {ex}"); }
                }

                canvas.DrawCircle(ESP_Config.ESP_ResolutionX / 2f, ESP_Config.ESP_ResolutionY / 2f, Program.UserConfig.AimFOV, PaintsManager.WhiteLine);

                if (Program.UserConfig.SilentAim && Aimbot.SilentAimPosition != Vector3.Zero)
                {
                    ESP_Utilities.W2S2(Aimbot.SilentAimPosition, true, out var aimBonePos);

                    canvas.DrawLine(ESP_Config.ESP_ResolutionX / 2f, ESP_Config.ESP_ResolutionY / 2f, aimBonePos.X, aimBonePos.Y, PaintsManager.WhiteLine);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] Realtime() -> ERROR {ex}");
            }
        }
    }
}
