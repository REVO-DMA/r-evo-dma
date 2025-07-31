using Silk.NET.SDL;
using Silk.NET.Windowing;
using SkiaSharp;
using Tarkov_DMA_Backend.Tarkov;
using Thread = System.Threading.Thread;
using Window = Silk.NET.Windowing.Window;

namespace Tarkov_DMA_Backend.ESP
{
    public unsafe sealed class FuserWindow : IDisposable
    {
        private readonly IMonitor _monitor;
        private readonly IWindow _window;

        private GRGlInterface grGlInterface;
        private GRContext grContext;
        private GRBackendRenderTarget renderTarget;
        private SKSurface surface;

        private volatile bool closed = false;
        private volatile bool shouldClose = false;

        public FuserWindow(int monitor)
        {
            SdlProvider.SDL.Value.SetHint(Sdl.HintVideoMinimizeOnFocusLoss, "0");
            SdlProvider.SDL.Value.SetHint(Sdl.HintFramebufferAcceleration, "opengl");
            SdlProvider.SDL.Value.SetHint(Sdl.HintRenderDriver, "opengl");
            SdlProvider.SDL.Value.SetHint(Sdl.HintRenderScaleQuality, "best");
            SdlProvider.SDL.Value.SetHint(Sdl.HintVideoForeignWindowOpengl, "1");
            SdlProvider.SDL.Value.SetHint(Sdl.HintWindowsDpiAwareness, "permonitorv2");

            // Init window
            IWindowPlatform windowPlatform = Window.GetWindowPlatform(false);
            if (windowPlatform == null)
                throw new Exception("[FUSER WINDOW] Unable to acquire a window platform.");

            _monitor = windowPlatform.GetMonitors().ElementAt(monitor);
            var vm = _monitor.VideoMode;

            WindowOptions options = WindowOptions.Default;

            options.API = GraphicsAPI.Default;

            ESP_Config.ESP_ResolutionX = vm.Resolution.Value.X;
            ESP_Config.ESP_ResolutionY = vm.Resolution.Value.Y;

            options.Title = "Fuser ESP";
            options.Size = new(ESP_Config.ESP_ResolutionX, ESP_Config.ESP_ResolutionY);

            options.PreferredStencilBufferBits = 8;
            options.PreferredBitDepth = new(8, 8, 8, 8);

            options.Position = new(_monitor.Bounds.Origin.X, _monitor.Bounds.Origin.Y);
            options.WindowBorder = WindowBorder.Hidden;
            options.WindowState = WindowState.Fullscreen;
            options.TopMost = true;

            options.VSync = false;

            _window = _monitor.CreateWindow(options);

            _window.Load += OnLoad;
            _window.Render += OnRender;
            _window.Closing += OnClose;
        }

        /// <summary>
        /// Blocks until window is closed.
        /// </summary>
        public void Run() => _window?.Run();

        public bool IsRunning() => !closed;

        public void Close() => _window?.Close();

        public void SetWindowFramerate()
        {
            int windowFPS = 0;
            VideoMode vm = _monitor.VideoMode;
            if (!vm.RefreshRate.HasValue)
                goto setFPS;

            if (ESP_Config.LowLatencyMode == 1)
                windowFPS = 0;
            else if (ESP_Config.LowLatencyMode == 2)
                windowFPS = vm.RefreshRate.Value;
            else if (ESP_Config.LowLatencyMode == 3)
                windowFPS = (int)(vm.RefreshRate + (vm.RefreshRate * 0.10f));

        setFPS:
            _window.FramesPerSecond = windowFPS;
        }

        public void Dispose()
        {
            shouldClose = true;

            while (IsRunning())
                Thread.Sleep(1);

            surface?.Dispose();
            renderTarget?.Dispose();
            grContext?.Dispose();
            grGlInterface?.Dispose();
            _window?.Dispose();
        }

        private void OnLoad()
        {
            grGlInterface = GRGlInterface.Create(GetProcedureAddress);
            if (!grGlInterface.Validate())
                throw new Exception("[FUSER WINDOW] Failed to validate GL interface.");

            grContext = GRContext.CreateGl(grGlInterface);
            renderTarget = new(ESP_Config.ESP_ResolutionX, ESP_Config.ESP_ResolutionY, 0, 8, new GRGlFramebufferInfo(0, 0x8058));
            surface = SKSurface.Create((GRRecordingContext)grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888, null, null);

            SetWindowFramerate();
        }

        private void OnRender(double delta)
        {
            if (shouldClose)
            {
                Close();
                return;
            }

            SKCanvas canvas = surface.Canvas;

            if (!CameraManager.PlayerIsInRaid)
            {
                canvas.Clear(SKColors.Black);
                canvas.Flush();
                Thread.Sleep(100);
                return;
            }

            ViewportManager.Update();

            canvas.Clear(SKColors.Black);

            ESP_Manager.Realtime(canvas);

            canvas.Flush();

            if (_window.FramesPerSecond == 0)
                Thread.Sleep(1000 / ESP_Config.ESP_UpdateSpeed);
        }

        private void OnClose()
        {
            closed = true;
        }

        private IntPtr GetProcedureAddress(string name)
        {
            if (_window.GLContext == null)
                return IntPtr.Zero;

            if (_window.GLContext.TryGetProcAddress(name, out var addr))
                return addr;

            return IntPtr.Zero;
        }
    }
}
