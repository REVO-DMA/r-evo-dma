using cs2_dma_esp.CS2;
using Silk.NET.Core;
using Silk.NET.GLFW;
using Silk.NET.Input.Glfw;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;
using Thread = System.Threading.Thread;
using Window = Silk.NET.Windowing.Window;

namespace cs2_dma_esp.ESP
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
            ESP_Config.ESP_ResolutionX = vm.Resolution!.Value.X;
            ESP_Config.ESP_ResolutionY = vm.Resolution!.Value.Y;
            WindowOptions options = WindowOptions.Default;
            options.Title = "EVO DMA - CS2 ESP";
            options.Position = new(_monitor.Bounds.Origin.X, _monitor.Bounds.Origin.Y);
            options.Size = new(ESP_Config.ESP_ResolutionX, ESP_Config.ESP_ResolutionY);
            options.PreferredStencilBufferBits = 8;
            options.PreferredBitDepth = new(8, 8, 8, 8);
            options.VSync = false;
            options.FramesPerSecond = vm.RefreshRate ?? 144;
            options.WindowBorder = WindowBorder.Hidden;
            options.WindowState = WindowState.Fullscreen;
            options.TopMost = true;

            _window = _monitor.CreateWindow(options);

            _window.Load += OnLoad;
            _window.Render += OnRender;

            // Track FPS
            _fpsStopwatch.Start();
        }

        public void DoRender() => _window.Run(); // Blocking

        private void OnLoad()
        {
            _window.Monitor = _monitor;
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

            // Draw FPS
            canvas.DrawText(_fpsString, ESP_Config.ESP_ResolutionX, 20f, PaintsManager.FPSText);

            RenderESP(canvas);

            canvas.Flush();
        }

        private static void RenderESP(SKCanvas canvas)
        {
            try
            {
                if (!Game.IsInMatch)
                {
                    Thread.Sleep(16);
                    return;
                }

                ESP_Utilities.ViewMatrix = Memory.ReadValue<Matrix4x4>(Game.ViewMatrix, false);

                var players = Game.Players;
                if (players is null)
                    return;

                int[] BoneLinkIndices = Player.BoneLinkIndices;
                Player LocalPlayer = Game.LocalPlayer;
                if (LocalPlayer is null)
                    return;

                foreach (var player in players.Values)
                {
                    if (player is null || player.IsLocal || (player.TeamID == LocalPlayer.TeamID && !Game.IsFFA) || !player.IsAlive)
                        continue;

                    Vector3[] BoneWorldPositions = player.BonePositions;
                    SKPoint[] BoneScreenPositions = new SKPoint[BoneLinkIndices.Length];
                    for (int i = 0; i < BoneLinkIndices.Length; i += 2)
                    {
                        int index1 = BoneLinkIndices[i];
                        int index2 = BoneLinkIndices[i + 1];

                        // Get bone joints screen positions
                        Vector3 pos1 = BoneWorldPositions[index1];
                        if (!ESP_Utilities.WorldToScreen(pos1, out var from))
                            continue;

                        Vector3 pos2 = BoneWorldPositions[index2];
                        if (!ESP_Utilities.WorldToScreen(pos2, out var to))
                            continue;

                        BoneScreenPositions[i] = new(from.X, from.Y);
                        BoneScreenPositions[i + 1] = new(to.X, to.Y);
                    }

                    float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
                    foreach (var BoneScreenPosition in BoneScreenPositions)
                    {
                        if (BoneScreenPosition.X < minX) minX = BoneScreenPosition.X;
                        if (BoneScreenPosition.Y < minY) minY = BoneScreenPosition.Y;

                        if (BoneScreenPosition.X > maxX) maxX = BoneScreenPosition.X;
                        if (BoneScreenPosition.Y > maxY) maxY = BoneScreenPosition.Y;
                    }

                    if (player.IsVisible)
                        DrawBones(canvas, BoneScreenPositions, PaintsManager.WhiteBox);
                    else
                        DrawBones(canvas, BoneScreenPositions, PaintsManager.RedBox);

                    DrawHealth(canvas, minX - 5, minY, 3, maxY - minY, 3, player.Health);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] Realtime() -> ERROR {ex}");
            }
        }

        private static void DrawHealth(SKCanvas canvas, float x, float y, float w, float h, int thickness, int health)
        {
            SKPaint paint;
            if (health >= 70)
                paint = PaintsManager.HealthGood;
            else if (health < 70 && health >= 40)
                paint = PaintsManager.HealthWarn;
            else
                paint = PaintsManager.HealthLow;

            float height = (h) * health / 100;

            canvas.DrawRect(x - (w / 2) - 3, y + (h - height), thickness, height, paint);
        }

        private static void DrawBones(SKCanvas canvas, SKPoint[] bones, SKPaint paint)
        {
            canvas.DrawPoints(SKPointMode.Lines, bones, paint);
        }
    }
}
