using cs2_dma_esp;
using Kokuban;
using Private_Overlay.Misc;
using Silk.NET.OpenGL;
using SkiaSharp;
using System.Diagnostics;
using TerraFX.Interop.Windows;
using GL = Silk.NET.OpenGL.GL;

namespace Private_Overlay.Rendering
{
    public static class SLOBS
    {
        public static ulong RenderDataIndex = 0;
        private static ulong LastRenderDataIndex = 0;
        public static PlayerESP_Data[] RenderData = Array.Empty<PlayerESP_Data>();

        public static DateTime LastRenderTime = DateTime.MinValue;
        public static bool ShouldClearScreen = false;

        private static readonly string PleaseClose = stackalloc char[] { '[', 'i', ']', ' ', 'P', 'l', 'e', 'a', 's', 'e', ' ', 'c', 'l', 'o', 's', 'e', ' ', 'S', 'L', 'O', 'B', 'S', '.' }.Encrypt();
        private static readonly string PleaseInstall = stackalloc char[] { '[', 'i', ']', ' ', 'P', 'l', 'e', 'a', 's', 'e', ' ', 'i', 'n', 's', 't', 'a', 'l', 'l', ' ', 'S', 'L', 'O', 'B', 'S', ' ', 't', 'o', ' ', 't', 'h', 'e', ' ', 'd', 'e', 'f', 'a', 'u', 'l', 't', ' ', 'l', 'o', 'c', 'a', 't', 'i', 'o', 'n', ' ', '(', 'C', ':', '\\', 'P', 'r', 'o', 'g', 'r', 'a', 'm', ' ', 'F', 'i', 'l', 'e', 's', '\\', 'S', 't', 'r', 'e', 'a', 'm', 'l', 'a', 'b', 's', ' ', 'O', 'B', 'S', ')', '.' }.Encrypt();

        private static readonly string BinaryPath = stackalloc char[] { 'C', ':', '\\', 'P', 'r', 'o', 'g', 'r', 'a', 'm', ' ', 'F', 'i', 'l', 'e', 's', '\\', 'S', 't', 'r', 'e', 'a', 'm', 'l', 'a', 'b', 's', ' ', 'O', 'B', 'S', '\\', 'r', 'e', 's', 'o', 'u', 'r', 'c', 'e', 's', '\\', 'a', 'p', 'p', '.', 'a', 's', 'a', 'r', '.', 'u', 'n', 'p', 'a', 'c', 'k', 'e', 'd', '\\', 'n', 'o', 'd', 'e', '_', 'm', 'o', 'd', 'u', 'l', 'e', 's', '\\', 'g', 'a', 'm', 'e', '_', 'o', 'v', 'e', 'r', 'l', 'a', 'y', '\\', 'g', 'a', 'm', 'e', '_', 'o', 'v', 'e', 'r', 'l', 'a', 'y', '.', 'n', 'o', 'd', 'e' }.Encrypt();
        private static readonly string BinaryName = stackalloc char[] { 'g', 'a', 'm', 'e', '_', 'o', 'v', 'e', 'r', 'l', 'a', 'y', '.', 'n', 'o', 'd', 'e' }.Encrypt();

        private static readonly string ProcessName = stackalloc char[] { 'S', 't', 'r', 'e', 'a', 'm', 'l', 'a', 'b', 's', ' ', 'O', 'B', 'S' }.Encrypt();
        private static readonly string WindowName = stackalloc char[] { 'o', 'v', 'e', 'r', 't', 'h', 'e', 't', 'o', 'p', '_', 'o', 'v', 'e', 'r', 'l', 'a', 'y' }.Encrypt();

        private static HWND HWND;

        public static readonly SKTypeface FontFamilyRegular;
        public static readonly SKTypeface FontFamilyBold;

        // Rendering resources
        private static GL _gl;
        private static GRGlInterface _grGlInterface;
        private static GRContext _grContext;
        private static GRBackendRenderTarget _grBackendRenderTarget;

        // Monitor info
        private static int MonitorWidth;
        private static int MonitorHeight;

        // FPS Tracking
        private static Stopwatch _fpsStopwatch = new();
        private static int _fps = 0;
        private static string _fpsString = "";

        // Custom overlay module replacement state
        private static bool _moduleReplaced = false;

        private static readonly SKPaint FPSText = new()
        {
            IsAntialias = true,
            Color = SKColors.Green,
            TextSize = 20f,
            TextAlign = SKTextAlign.Right,
            Typeface = FontFamilyRegular,
        };

        [Obsolete]
        static SLOBS()
        {
            _fpsStopwatch.Start();

            // Load fonts
            FontFamilyRegular = SKTypeface.FromTypeface(SKTypeface.FromStream(new MemoryStream(File.ReadAllBytes("Fonts\\Neo Sans Std Regular.otf"))), SKTypefaceStyle.Normal);
            FontFamilyBold = SKTypeface.FromTypeface(SKTypeface.FromStream(new MemoryStream(File.ReadAllBytes("Fonts\\Neo Sans Std Bold.otf"))), SKTypefaceStyle.Bold);
        }

        public static unsafe HGLRC? Init()
        {
            // Make sure the modified module is loaded
            if (!_moduleReplaced)
            {
                bool pleaseCloseShown = false;
                bool pleaseReinstallShown = false;

                while (true)
                {
                    var processes = Process.GetProcessesByName(ProcessName.Decrypt());
                    if (processes.Length > 0)
                    {
                        if (!pleaseCloseShown)
                        {
                            Console.Clear();
                            Console.WriteLine(Chalk.Red + PleaseClose.Decrypt());
                            pleaseCloseShown = true;
                        }
                    }
                    else
                    {
                        // Copy the modified binary to SLOBS unpacked modules directory
                        if (File.Exists(BinaryPath.Decrypt()))
                        {
                            byte[] bytes = File.ReadAllBytes(BinaryPath.Decrypt());
                            File.WriteAllBytes(BinaryPath.Decrypt(), bytes);

                            _moduleReplaced = true;
                            Program.slobsInfoShown = false;

                            break;
                        }
                        else
                        {
                            if (!pleaseReinstallShown)
                            {
                                Console.Clear();
                                Console.WriteLine(Chalk.Red + PleaseInstall.Decrypt());
                                Program.ShowSetup();
                                pleaseReinstallShown = true;
                            }
                        }
                    }

                    Thread.Sleep(100);
                }
            }

            // Get SLOBS overlay hwnd
            var overlayHost = GetOverlayHost();
            if (overlayHost != null)
                HWND = (HWND)overlayHost;
            else
                return null;

            MonitorWidth = GetSystemMetrics(0);
            MonitorHeight = GetSystemMetrics(1);

            // Fullscreen
            MoveWindow(HWND, 0, 0, MonitorWidth, MonitorHeight, 0);

            // Transparency
            MARGINS margins = new()
            {
                cxLeftWidth = -1,
                cxRightWidth = -1,
                cyBottomHeight = -1,
                cyTopHeight = -1
            };
            DwmExtendFrameIntoClientArea(HWND, &margins);

            SetOGL_PixelFormat();
            HGLRC hglrc = SetOGL_Context();

            InitOGL();

            return hglrc;
        }

        public static void Cleanup(HGLRC gctx)
        {
            wglDeleteContext(gctx);
        }

        public static unsafe void RenderESP()
        {
            Thread slobsOverlayWatcher;
            CancellationTokenSource cts = new();
            CancellationToken ct = cts.Token;

            bool slobsOverlayAvailable = true;

            HDC DeviceContext = GetDC(HWND);

            try
            {
                slobsOverlayWatcher = new(() =>
                {
                    try
                    {
                        while (true)
                        {
                            ct.ThrowIfCancellationRequested();

                            var overlayHost = GetOverlayHost();
                            if (overlayHost == null)
                            {
                                slobsOverlayAvailable = false;
                                break;
                            }

                            Thread.Sleep(100);
                        }
                    }
                    catch (OperationCanceledException) { return; }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[SLOBS] -> slobsOverlayWatcher(): Error {ex}");
                    }
                });

                slobsOverlayWatcher.IsBackground = true;
                slobsOverlayWatcher.Priority = ThreadPriority.BelowNormal;

                slobsOverlayWatcher.Start();

                while (slobsOverlayAvailable)
                {
                    // If the cheat is not sending data, clear the screen.
                    if (ShouldClearScreen)
                    {
                        _gl.ClearColor(0, 0, 0, 0);
                        _gl.Clear(ClearBufferMask.ColorBufferBit);
                        _gl.Flush();
                    }

                    // Only proceed if there is new render data
                    if (RenderDataIndex == LastRenderDataIndex)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    if (_fpsStopwatch.ElapsedMilliseconds >= 1000)
                    {
                        _fpsString = _fps.ToString();
                        _fps = 0;

                        _fpsStopwatch.Restart();
                    }

                    _fps++;

                    _gl.ClearColor(0, 0, 0, 0);
                    _gl.Clear(ClearBufferMask.ColorBufferBit);

                    using var surface = SKSurface.Create(_grContext, _grBackendRenderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
                    var canvas = surface.Canvas;

                    foreach (var player in RenderData)
                    {
                        float BoxHeight = player.BoneBounds.MaxY - player.BoneBounds.MinY;
                        float BoxWidth = BoxHeight / 1.6f;

                        DrawHealth(ref canvas, player.HeadPosition.X, player.HeadPosition.Y, BoxWidth, BoxHeight, 3, player.Health);

                        if (player.Visible)
                            DrawBones(ref canvas, player.BoneScreenPositions, PaintsManager.WhiteBone);
                        else
                            DrawBones(ref canvas, player.BoneScreenPositions, PaintsManager.RedBone);

                        DrawString(ref canvas, player.HeadPosition.X, player.HeadPosition.Y - 8f, player.Name, FPSText, new()
                        {
                            Size = 14
                        });
                    }

                    // Draw FPS
                    DrawString(ref canvas, MonitorWidth, 24f, _fpsString, FPSText, new()
                    {
                        Alignment = SKTextAlign.Right,
                        Size = 24f,
                        Color = SKColors.Green
                    });

                    canvas.Flush();

                    _gl.Flush();

                    LastRenderDataIndex = RenderDataIndex;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[SLOBS] -> RenderESP(): Error while rendering ESP {ex}");
            }
            finally
            {
                cts.Cancel();
                cts.Dispose();

                ReleaseDC(HWND, DeviceContext);
            }
        }

        private static void DrawHealth(ref SKCanvas canvas, float x, float y, float boxWidth, float boxHeight, int thickness, float health)
        {
            SKPaint paint;
            if (health >= 70f)
                paint = PaintsManager.HealthGood;
            else if (health < 70f && health >= 40f)
                paint = PaintsManager.HealthWarn;
            else
                paint = PaintsManager.HealthLow;

            float barHeight = boxHeight * health / 100f;

            canvas.DrawRect(x - (boxWidth / 2f) - thickness, y, thickness, boxHeight, PaintsManager.HealthBar);
            canvas.DrawRect(x - (boxWidth / 2f) - thickness, y + (boxHeight - barHeight), thickness, barHeight, paint);
        }

        private static void DrawBones(ref SKCanvas canvas, SKPoint[] bones, SKPaint paint)
        {
            canvas.DrawPoints(SKPointMode.Lines, bones, paint);
        }

        private static readonly SKPaint StringPaint = new()
        {
            IsAntialias = true,
            Color = SKColors.Black,
            TextSize = 20f,
            TextAlign = SKTextAlign.Right,
            IsStroke = true,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            Typeface = FontFamilyRegular,
        };

        private struct StringDesign
        {
            public float Size = 12f;
            public float Outline = 2f;
            public SKTextAlign Alignment = SKTextAlign.Center;
            public SKColor Color = SKColors.White;

            public StringDesign() { }
        }

        private static void DrawString(ref SKCanvas canvas, float x, float y, string text, SKPaint paint, StringDesign? designP = null)
        {
            StringDesign design;

            if (designP == null)
                design = new();
            else
                design = (StringDesign)designP;

            // Draw shadow
            if (design.Outline > 0f)
            {
                StringPaint.TextSize = design.Size;
                StringPaint.TextAlign = design.Alignment;
                StringPaint.StrokeWidth = design.Outline;
                canvas.DrawText(text, x, y, StringPaint);
            }

            // Draw main text
            paint.TextSize = design.Size;
            paint.TextAlign = design.Alignment;
            paint.Color = design.Color;
            canvas.DrawText(text, x, y, paint);
        }

        private static unsafe HWND? GetOverlayHost()
        {
            fixed (char* windowNamePtr = WindowName.Decrypt())
            {
                var hwnd = FindWindowW((ushort*)windowNamePtr, null);

                if (hwnd != nint.Zero)
                    return hwnd;
            }

            return null;
        }

        private static void InitOGL()
        {
            _gl = new(new GLContext());
            _grGlInterface = GRGlInterface.Create((name) => _gl.Context.TryGetProcAddress(name, out var addr) ? addr : 0);
            _grContext = GRContext.CreateGl(_grGlInterface);
            _grBackendRenderTarget = new(MonitorWidth, MonitorHeight, 0, 8, new(0, 0x8058));
        }

        private static unsafe void SetOGL_PixelFormat()
        {
            PIXELFORMATDESCRIPTOR pfd = new()
            {
                nSize = (ushort)sizeof(PIXELFORMATDESCRIPTOR),
                nVersion = 1,
                dwFlags = PFD.PFD_DRAW_TO_WINDOW | PFD.PFD_SUPPORT_OPENGL,
                iPixelType = PFD.PFD_TYPE_RGBA,
                cColorBits = 24,
                cAlphaBits = 8,
                cStencilBits = 0,
                cDepthBits = 0,
            };

            HDC deviceContext = GetDC(HWND);

            int iPixelFormat = ChoosePixelFormat(deviceContext, &pfd);

            SetPixelFormat(deviceContext, iPixelFormat, &pfd);

            ReleaseDC(HWND, deviceContext);
        }

        private static HGLRC SetOGL_Context()
        {
            var deviceContext = GetDC(HWND);
            var oglCtx = wglCreateContext(deviceContext);
            wglMakeCurrent(deviceContext, oglCtx);
            ReleaseDC(HWND, deviceContext);

            return oglCtx;
        }
    }
}
