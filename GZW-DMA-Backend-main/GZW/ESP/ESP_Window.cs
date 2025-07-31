using Silk.NET.Core;
using Silk.NET.GLFW;
using Silk.NET.Input.Glfw;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;
using Window = Silk.NET.Windowing.Window;

namespace gzw_dma_backend.GZW.ESP
{
    public class ESP_Window
    {
        private static readonly UserConfig _config = Program.UserConfig;
        public static readonly SKTypeface FontFamilyRegular;
        public static readonly SKTypeface FontFamilyBold;

        private readonly IWindow _window;

        private readonly IMonitor _monitor;
        private GRContext _grContext;
        private GRBackendRenderTarget _grBackendRenderTarget;

        // FPS Tracking
        private readonly Stopwatch _fpsStopwatch = new();
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
            _monitor = windowPlatform.GetMonitors().ElementAt(_config.SelectedMonitor);
            var vm = _monitor.VideoMode;

            WindowOptions options = WindowOptions.Default;

            if (Program.UserConfig.FullScreen)
            {
                ESP_Config.ESP_ResolutionX = vm.Resolution!.Value.X;
                ESP_Config.ESP_ResolutionY = vm.Resolution!.Value.Y;

                options.Position = new(_monitor.Bounds.Origin.X, _monitor.Bounds.Origin.Y);
                options.WindowBorder = WindowBorder.Hidden;
                options.WindowState = WindowState.Fullscreen;
                options.TopMost = true;
            }
            else
            {
                ESP_Config.ESP_ResolutionX = 1280;
                ESP_Config.ESP_ResolutionY = 720;

                options.WindowBorder = WindowBorder.Fixed;
                options.Position = new(_monitor.Bounds.Origin.X + 50, _monitor.Bounds.Origin.Y + 50);
            }

            options.Title = "EVO DMA - GZW ESP";
            options.Size = new(ESP_Config.ESP_ResolutionX, ESP_Config.ESP_ResolutionY);
            options.PreferredStencilBufferBits = 8;
            options.PreferredBitDepth = new(8, 8, 8, 8);
            options.VSync = false;
            options.FramesPerSecond = vm.RefreshRate ?? 144;
            options.TransparentFramebuffer = Program.UserConfig.Moonlight;

            _window = _monitor.CreateWindow(options);

            _window.Load += OnLoad;
            _window.Render += OnRender;

            // Track FPS
            _fpsStopwatch.Start();
        }

        public void DoRender() => _window.Run(); // Blocking

        private void OnLoad()
        {
            if (Program.UserConfig.FullScreen) _window.Monitor = _monitor;

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
            //if (!CameraManager.PlayerIsInRaid)
            //{
            //    canvas.Flush();

            //    Thread.Sleep(100);
            //    return;
            //}

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

                int[] BoneLinkIndices = Player.BoneLinkIndices;

                var players = Manager.Players.ToArray();

                foreach (var playerRaw in players)
                {
                    try
                    {
                        var player = playerRaw.Value;

                        if (player.IsLocalPlayer)
                            continue;

                        if (!player.IsDead)
                        {
                            if (player.IsAI && player.Distance > 250)
                                continue;

                            ESP_Utilities.W2S(player.Position, out var screenPos);
                            var offsetHead = new Vector3D<double>(player.BonePositions[6].X, player.BonePositions[6].Y + 0.3d, player.BonePositions[6].Z);
                            ESP_Utilities.W2S(offsetHead, out var headScreen);
                            ESP_Utilities.W2S(player.BonePositions[6], out var normal_head);
                            ESP_Utilities.W2S(player.BonePositions[0], out var root);

                            float BoxHeight = root.Y - normal_head.Y;
                            float BoxWidth = BoxHeight / 3f;

                            DrawHealth(canvas, screenPos.X, headScreen.Y, BoxWidth, BoxHeight, 3, player.Health);

                            canvas.DrawText($"[{player.Distance} M]", headScreen.X, headScreen.Y - 5f, PaintsManager.WhiteText);

                            SKPoint[] BoneScreenPositions = new SKPoint[BoneLinkIndices.Length];
                            for (int ii = 0; ii < BoneLinkIndices.Length; ii += 2)
                            {
                                int index1 = BoneLinkIndices[ii];
                                int index2 = BoneLinkIndices[ii + 1];

                                // Get bone joints screen positions
                                Vector3D<double> pos1 = player.BonePositions[index1];
                                if (!ESP_Utilities.W2S(pos1, out var from))
                                    continue;

                                Vector3D<double> pos2 = player.BonePositions[index2];
                                if (!ESP_Utilities.W2S(pos2, out var to))
                                    continue;

                                BoneScreenPositions[ii] = new(from.X, from.Y); // From
                                BoneScreenPositions[ii + 1] = new(to.X, to.Y); // To
                            }

                            if (player.IsAI)
                                DrawBones(canvas, BoneScreenPositions, PaintsManager.BlueLine);
                            else if (Manager.LocalPlayer != null && player.TeamID == Manager.LocalPlayer.TeamID)
                                DrawBones(canvas, BoneScreenPositions, PaintsManager.GreenLine);
                            else
                                DrawBones(canvas, BoneScreenPositions, PaintsManager.RedLine);
                        }
                        else if (player.IsDead && player.Distance <= 150)
                        {
                            ESP_Utilities.W2S(player.Position, out var screenPos);

                            if (player.IsAI)
                                canvas.DrawText($"[{player.Distance} M] AI Corpse", screenPos.X, screenPos.Y, PaintsManager.WhiteText);
                            else
                                canvas.DrawText($"[{player.Distance} M] Player Corpse", screenPos.X, screenPos.Y, PaintsManager.WhiteText);
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] Realtime() -> ERROR {ex}");
            }
        }

        private static void DrawHealth(SKCanvas canvas, float x, float y, float w, float h, int thickness, Player.HealthStatus healthStatus)
        {
            SKPaint paint;
            float health;
            if (healthStatus == Player.HealthStatus.Healthy)
            {
                paint = PaintsManager.HealthGood;
                health = 100f;
            }
            else if (healthStatus == Player.HealthStatus.Injured)
            {
                paint = PaintsManager.HealthWarn;
                health = 50f;
            }
            else if (healthStatus == Player.HealthStatus.Critical)
            {
                paint = PaintsManager.HealthLow;
                health = 25f;
            }
            else
                return;


            float height = (h) * health / 100f;

            canvas.DrawRect(x - (w / 2f) - 3f, y + (h - height), thickness, height, paint);
        }

        private static void DrawBones(SKCanvas canvas, SKPoint[] bones, SKPaint paint)
        {
            canvas.DrawPoints(SKPointMode.Lines, bones, paint);
        }
    }
}
