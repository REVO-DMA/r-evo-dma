using apex_dma_esp.Apex;
using Silk.NET.Core;
using Silk.NET.GLFW;
using Silk.NET.Input.Glfw;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;
using static apex_dma_esp.Apex.Player;
using Thread = System.Threading.Thread;
using Window = Silk.NET.Windowing.Window;

namespace apex_dma_esp.ESP
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
            options.Title = "EVO DMA - Apex ESP";
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
                var localPlayer = Manager.LocalPlayer;
                if (localPlayer is null)
                {
                    Thread.Sleep(16);
                    return;
                }

                #region Update Camera Info

                ESP_Utilities.Update();

                #endregion

                #region Render Players

                var players = Manager.Players;
                if (players is null)
                    return;

                foreach (var player in players.Values)
                {
                    if (player is null || player.IsLocal || player.Health <= 0)
                        continue;

                    try
                    {
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

                        DrawBones(canvas, BoneScreenPositions, PaintsManager.WhiteBox);
                    }
                    catch { }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP] Realtime() -> ERROR {ex}");
            }
        }

        private static void DrawBones(SKCanvas canvas, SKPoint[] bones, SKPaint paint)
        {
            canvas.DrawPoints(SKPointMode.Lines, bones, paint);
        }
    }
}
