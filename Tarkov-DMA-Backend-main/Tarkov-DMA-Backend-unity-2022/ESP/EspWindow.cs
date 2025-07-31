using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using SkiaSharp;
using Tarkov_DMA_Backend.Tarkov;
using TerraFX.Interop.Windows;
using Thread = System.Threading.Thread;
using Window = Silk.NET.Windowing.Window;
using static TerraFX.Interop.Windows.Windows;
using Tarkov_DMA_Backend.LowLevel;

namespace Tarkov_DMA_Backend.ESP
{
    public unsafe sealed class EspWindow : IDisposable
    {
        private readonly IWindow _window;

        private readonly WindowPreferences _windowPreferences;

        private GRGlInterface grGlInterface;
        private GRContext grContext;
        private GRBackendRenderTarget renderTarget;
        private SKSurface surface;

        private volatile bool closed = false;
        private volatile bool shouldClose = false;

        public EspWindow()
        {
            SdlProvider.SDL.Value.SetHint(Sdl.HintVideoMinimizeOnFocusLoss, "0");
            SdlProvider.SDL.Value.SetHint(Sdl.HintFramebufferAcceleration, "opengl");
            SdlProvider.SDL.Value.SetHint(Sdl.HintRenderDriver, "opengl");
            SdlProvider.SDL.Value.SetHint(Sdl.HintRenderScaleQuality, "best");
            SdlProvider.SDL.Value.SetHint(Sdl.HintVideoForeignWindowOpengl, "1");
            SdlProvider.SDL.Value.SetHint(Sdl.HintWindowsDpiAwareness, "permonitorv2");

            _windowPreferences = new();

            // Init window
            IWindowPlatform windowPlatform = Window.GetWindowPlatform(false);
            if (windowPlatform == null)
                throw new Exception("[ESP WINDOW] Unable to acquire a window platform.");

            WindowOptions options = WindowOptions.Default;

            options.API = GraphicsAPI.Default;

            ESP_Config.ESP_ResolutionX = _windowPreferences.Preferences.Width;
            ESP_Config.ESP_ResolutionY = _windowPreferences.Preferences.Height;

            options.Title = "ESP Window";
            options.Size = new(ESP_Config.ESP_ResolutionX, ESP_Config.ESP_ResolutionY);

            options.PreferredStencilBufferBits = 8;
            options.PreferredBitDepth = new(8, 8, 8, 8);

            options.Position = new(_windowPreferences.Preferences.X, _windowPreferences.Preferences.Y);
            options.WindowBorder = WindowBorder.Resizable;
            options.WindowState = WindowState.Normal;
            options.TopMost = false;

            options.VSync = false;
            options.FramesPerSecond = 0;

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Render += OnRender;
            _window.Closing += OnClose;
            _window.Resize += OnResize;
            _window.Move += OnMove;
        }

        /// <summary>
        /// Blocks until window is closed.
        /// </summary>
        public void Run() => _window?.Run();

        public bool IsRunning() => !closed;

        public void Close() => _window?.Close();

        public void SetWindowProperties()
        {
            SetBorderless(ESP_Config.Borderless);
            SetTopmost(ESP_Config.Topmost);
            SetClickthrough(ESP_Config.Clickthrough);
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
                throw new Exception("[ESP WINDOW] Failed to validate GL interface.");

            grContext = GRContext.CreateGl(grGlInterface);
            CreateSurface();

            SetWindowProperties();
        }

        private void CreateSurface()
        {
            surface?.Dispose();
            renderTarget?.Dispose();

            renderTarget = new(_windowPreferences.Preferences.Width, _windowPreferences.Preferences.Height, 0, 8, new GRGlFramebufferInfo(0, 0x8058));
            surface = SKSurface.Create((GRRecordingContext)grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888, null, null);
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

            Thread.Sleep(1000 / ESP_Config.ESP_UpdateSpeed);
        }

        private void OnClose()
        {
            closed = true;
        }

        private void OnResize(Vector2D<int> newSize)
        {
            if (!CanApplyWindowState())
                return;

            _windowPreferences.Preferences.Width = newSize.X;
            _windowPreferences.Preferences.Height = newSize.Y;
            _windowPreferences.Save();

            CreateSurface();
            ESP_Manager.OnESPResize(newSize.X, newSize.Y);
        }

        private void OnMove(Vector2D<int> newPosition)
        {
            if (!CanApplyWindowState())
                return;

            _windowPreferences.Preferences.X = newPosition.X;
            _windowPreferences.Preferences.Y = newPosition.Y;
            _windowPreferences.Save();
        }

        private bool CanApplyWindowState()
        {
            if (_window.WindowState == WindowState.Normal ||
                _window.WindowState == WindowState.Maximized)
            {
                return true;
            }

            return false;
        }

        private void SetBorderless(bool borderless)
        {
            if (borderless)
                _window.WindowBorder = WindowBorder.Hidden;
            else
                _window.WindowBorder = WindowBorder.Resizable;
        }

        private void SetTopmost(bool topMost)
        {
            _window.TopMost = topMost;
        }

        private void SetClickthrough(bool clickthrough)
        {
            HWND hwnd = GetHWND(_window.Title);

            nint style = GetWindowLongPtrA(hwnd, GWL.GWL_EXSTYLE);

            if (clickthrough)
                SetWindowLongPtrA(hwnd, GWL.GWL_EXSTYLE, style | WS.WS_EX_LAYERED | WS.WS_EX_TRANSPARENT);
            else
                SetWindowLongPtrA(hwnd, GWL.GWL_EXSTYLE, style & ~WS.WS_EX_TRANSPARENT);
        }

        private HWND GetHWND(string windowName)
        {
            PChar pName = new(windowName);
            return FindWindowW(null, pName);
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
