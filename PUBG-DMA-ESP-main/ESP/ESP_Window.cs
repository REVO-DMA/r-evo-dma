using pubg_dma_esp.PUBG;
using Silk.NET.Core;
using Silk.NET.GLFW;
using Silk.NET.Input.Glfw;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;
using Thread = System.Threading.Thread;
using Window = Silk.NET.Windowing.Window;

namespace pubg_dma_esp.ESP
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
            
            options.Title = "EVO DMA - PUBG ESP";
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
            canvas.Clear(SKColors.Transparent);

            // Draw nothing when out of match, and throttle render loop
            if (!Manager.IsInMatch)
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
                if (!LocalPlayer.IsValid())
                {
                    Thread.Sleep(16);
                    return;
                }

                ESP_Utilities.UpdateW2S();

                #region Render Players

                var players = Manager.Players;
                if (players is null)
                    return;

                int[] BoneLinkIndices = Player.BoneLinkIndices;

                if (LocalPlayer.SpectatorCount > 0)
                    canvas.DrawText($"Spectators: {LocalPlayer.SpectatorCount}", 0, 20f, PaintsManager.SpectatorText);

                foreach (var player in players.Values)
                {
                    if (player is null || player.IsLocalPlayer || !player.ShouldRender)
                        continue;

                    try
                    {
                        ESP_Utilities.W2S(player.Position, out var screenPos);
                        var offsetHead = new Vector3(player.BonePositions[6].X, player.BonePositions[6].Y + 0.3f, player.BonePositions[6].Z);
                        ESP_Utilities.W2S(offsetHead, out var headScreen);
                        ESP_Utilities.W2S(player.BonePositions[6], out var normal_head);
                        ESP_Utilities.W2S(player.BonePositions[0], out var root);

                        float BoxHeight = root.Y - normal_head.Y;
                        float BoxWidth = BoxHeight / 3f;

                        DrawHealth(canvas, screenPos.X, headScreen.Y, BoxWidth, BoxHeight, 3, player.Health);

                        if (player.IsHuman)
                        {
                            if (player.Name.Length > 0)
                                canvas.DrawText($"[{player.Distance} M] {player.Name}", headScreen.X, headScreen.Y - 5f, PaintsManager.WhiteText);
                            else
                                canvas.DrawText($"[{player.Distance} M]", headScreen.X, headScreen.Y - 5f, PaintsManager.WhiteText);

                            if (player.Knocked)
                                canvas.DrawText($"T{player.Team} (Knocked)", root.X, root.Y + 20, PaintsManager.WhiteText);
                            else
                                canvas.DrawText($"T{player.Team}", root.X, root.Y + 20, PaintsManager.WhiteText);
                        }
                        else
                        {
                            if (player.Knocked)
                                canvas.DrawText($"[{player.Distance} M] BOT (Knocked)", headScreen.X, headScreen.Y - 5f, PaintsManager.WhiteText);
                            else
                                canvas.DrawText($"[{player.Distance} M] BOT", headScreen.X, headScreen.Y - 5f, PaintsManager.WhiteText);
                        }

                        Vector3[] BoneWorldPositions = player.BonePositions;
                        SKPoint[] BoneScreenPositions = new SKPoint[BoneLinkIndices.Length];
                        for (int ii = 0; ii < BoneLinkIndices.Length; ii += 2)
                        {
                            int index1 = BoneLinkIndices[ii];
                            int index2 = BoneLinkIndices[ii + 1];

                            // Get bone joints screen positions
                            Vector3 pos1 = BoneWorldPositions[index1];
                            if (!ESP_Utilities.W2S(pos1, out var from))
                                continue;

                            Vector3 pos2 = BoneWorldPositions[index2];
                            if (!ESP_Utilities.W2S(pos2, out var to))
                                continue;

                            // TODO: Draw head circle
                            BoneScreenPositions[ii] = new(from.X, from.Y); // From
                            BoneScreenPositions[ii + 1] = new(to.X, to.Y); // To
                        }

                        if (player.IsVisible)
                            DrawBones(canvas, BoneScreenPositions, PaintsManager.WhiteLine);
                        else
                            DrawBones(canvas, BoneScreenPositions, PaintsManager.RedLine);
                    }
                    catch { }
                }

                #endregion

                #region Render Vehicles

                var vehicles = Manager.Vehicles;
                if (vehicles is not null)
                    foreach (var vehicle in vehicles.Values)
                    {
                        if (vehicle is null)
                            continue;

                        if (vehicle.Distance > 600)
                            continue;

                        ESP_Utilities.W2S(vehicle.Position, out var screenPos);

                        canvas.DrawText($"[{vehicle.Distance} M] {vehicle.Name}", screenPos.X, screenPos.Y, PaintsManager.WhiteText);
                    }

                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] Realtime() -> ERROR {ex}");
            }
        }

        private static void DrawHealth(SKCanvas canvas, float x, float y, float w, float h, int thickness, float health)
        {
            SKPaint paint;
            if (health >= 70f)
                paint = PaintsManager.HealthGood;
            else if (health < 70f && health >= 40f)
                paint = PaintsManager.HealthWarn;
            else
                paint = PaintsManager.HealthLow;

            float height = (h) * health / 100f;

            canvas.DrawRect(x - (w / 2f) - 3f, y + (h - height), thickness, height, paint);
        }

        private static void DrawBones(SKCanvas canvas, SKPoint[] bones, SKPaint paint)
        {
            canvas.DrawPoints(SKPointMode.Lines, bones, paint);
        }
    }
}
