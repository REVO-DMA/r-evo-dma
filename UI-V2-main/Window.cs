using System.Runtime.CompilerServices;
using TerraFX.Interop.Windows;
using UI_V2.ResourceHelper;
using static TerraFX.Interop.Windows.Windows;

namespace UI_V2
{
    public sealed class Window
    {
        private readonly Form _form;

        public Size OriginalSize { get; private set; }
        public Point OriginalLocation { get; private set; }

        public eWindowState State { get; private set; } = eWindowState.Restored;

        private event EventHandler OnMaximize = null;
        private event EventHandler OnRestore = null;
        private event EventHandler OnFocus = null;
        private event EventHandler OnBlur = null;

        public Window(Form form, EventHandler onFocus = null, EventHandler onBlur = null, EventHandler onMaximize = null, EventHandler onRestore = null)
        {
            _form = form;

            if (onFocus != null)
                OnFocus += onFocus;

            if (onBlur != null)
                OnBlur += onBlur;

            if (onMaximize != null)
                OnMaximize += onMaximize;

            if (onRestore != null)
                OnRestore += onRestore;
        }

        #region Class Types

        public enum eWindowState
        {
            Maximized,
            Restored
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the dimensions of the monitor the majority of the window is on.
        /// </summary>
        public static unsafe Size GetMonitorSize(HWND hWND)
        {
            HMONITOR hMonitor = MonitorFromWindow(hWND, MONITOR.MONITOR_DEFAULTTONEAREST);

            MONITORINFO monitorInfo;
            monitorInfo.cbSize = (uint)Unsafe.SizeOf<MONITORINFO>();

            if (GetMonitorInfoA(hMonitor, &monitorInfo))
            {
                int x = monitorInfo.rcMonitor.right - monitorInfo.rcMonitor.left;
                int y = monitorInfo.rcMonitor.bottom - monitorInfo.rcMonitor.top;

                return new(x, y);
            }
            else
                return new(-1, -1);
        }

        /// <summary>
        /// Gets the border thickness of the window.
        /// </summary>
        public static unsafe RECT GetBorderThickness(HWND hWND)
        {
            RECT borderThickness;
            AdjustWindowRectEx(&borderThickness, (uint)(GetWindowLongPtrA(hWND, GWL.GWL_STYLE) & ~WS.WS_CAPTION), FALSE, NULL);

            borderThickness.top *= -1;
            borderThickness.left *= -1;

            return borderThickness;
        }

        /// <summary>
        /// Gets the DPI scaling factor of the program.
        /// </summary>
        public static float GetDpiScale(HWND hwnd)
        {
            HDC screen = GetDC(hwnd);
            int dpi = GetDeviceCaps(screen, LOGPIXELSX);
            ReleaseDC(hwnd, screen);

            return dpi / 96f;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handle window events.
        /// </summary>
        public unsafe bool WndProc(ref Message message)
        {
            if (message.Msg == WM.WM_NCCALCSIZE && message.WParam == TRUE)
            {
                NCCALCSIZE_PARAMS* sz = (NCCALCSIZE_PARAMS*)message.LParam;

                Size windowSize = new(sz->rgrc[0].right - sz->rgrc[0].left, sz->rgrc[0].bottom - sz->rgrc[0].top);
                RECT borderThickness = GetBorderThickness((HWND)_form.Handle);
                Size monitorSize = GetMonitorSize((HWND)_form.Handle);

                if (monitorSize.Width != -1 &&
                    monitorSize.Height != -1 &&
                    windowSize.Width >= monitorSize.Width &&
                    windowSize.Height >= monitorSize.Height)
                {
                    // This fixes the top of the application being partially offscreen when the window is fullscreen
                    sz->rgrc[0].top += borderThickness.top;
                }
                else
                {
                    // Only add 1px to the top if the window is not maximized
                    if (!IsMaximizedSize(windowSize))
                        sz->rgrc[0].top += 1;
                }

                sz->rgrc[0].left += borderThickness.left;
                sz->rgrc[0].right -= borderThickness.right;
                sz->rgrc[0].bottom -= borderThickness.bottom;

                return true;
            }
            else if (message.Msg == WM.WM_SYSCOMMAND)
            {
                int command = message.WParam.ToInt32() & 0xFFF0;

                if (command == SC.SC_MAXIMIZE || command == SC.SC_RESTORE)
                {
                    // Handle showing a window after it was minimized
                    if (_form.WindowState == FormWindowState.Minimized)
                    {
                        if (State == eWindowState.Maximized)
                            Maximize(true);
                        else
                            Restore();
                    }
                    else
                    {
                        if (IsMaximized())
                            Restore();
                        else
                            Maximize();

                        return true;
                    }
                }
                else if (command == SC.SC_MOVE)
                {
                    if (State == eWindowState.Maximized)
                    {
                        Restore(true);

                        float dpiScale = GetDpiScale((HWND)_form.Handle);
                        int newX = Cursor.Position.X - _form.Size.Width / 2;
                        int newY = Cursor.Position.Y - (int)(15 * dpiScale);
                        SetWindowPos((HWND)_form.Handle, HWND.NULL, newX, newY, 0, 0, SWP.SWP_NOZORDER | SWP.SWP_NOSIZE | SWP.SWP_NOACTIVATE);
                    }
                }
            }
            else if (message.Msg == WM.WM_ACTIVATE)
            {
                if (message.WParam == FALSE)
                    OnBlur?.Invoke(this, EventArgs.Empty);
                else
                    OnFocus?.Invoke(this, EventArgs.Empty);
            }

            return false;
        }

        /// <summary>
        /// This changes the window styles to allow it to still be resizable even though it's a borderless window.
        /// </summary>
        public void FixStyle()
        {
            HWND hWND = (HWND)_form.Handle;

            int style = GetWindowLongA(hWND, GWL.GWL_STYLE);

            style &= ~WS.WS_CAPTION;
            style |= WS.WS_THICKFRAME;

            SetWindowLongA(hWND, GWL.GWL_STYLE, style);

            SetWindowPos(hWND, HWND.NULL, 0, 0, 0, 0, SWP.SWP_FRAMECHANGED | SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOZORDER);
        }

        /// <summary>
        /// This sets the icon from an embedded .ico resource.
        /// </summary>
        public void SetIcon(string icoName)
        {
            if (Path.GetExtension(icoName) != ".ico")
                throw new Exception("Only .ico files are supported!");

            byte[] resource = ResourceLoader.LoadBinary(icoName);
            using MemoryStream ms = new(resource);
            Icon icon = new(ms);

            SendMessageA((HWND)_form.Handle, WM.WM_SETICON, ICON_BIG, (LPARAM)icon.Handle);
            SendMessageA((HWND)_form.Handle, WM.WM_SETICON, ICON_SMALL, (LPARAM)icon.Handle);
        }

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        public void Maximize(bool restore = false)
        {
            RECT borderThickness = GetBorderThickness((HWND)_form.Handle);

            // If the window is being restored from a minimized state don't save the position
            if (!restore)
            {
                OriginalSize = _form.Size;
                OriginalLocation = _form.Location;
            }

            _form.Size = new(SystemInformation.WorkingArea.Width + (borderThickness.left * 2), SystemInformation.WorkingArea.Height + borderThickness.top);
            _form.Location = new(SystemInformation.WorkingArea.Left - borderThickness.top, SystemInformation.WorkingArea.Top);

            State = eWindowState.Maximized;

            OnMaximize?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Restores the window to the size it was before it was maximized.
        /// </summary>
        public void Restore(bool onMove = false)
        {
            _form.Size = OriginalSize;

            if (!onMove)
                _form.Location = OriginalLocation;

            State = eWindowState.Restored;

            OnRestore?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>
        /// Minimizes the window.
        /// </summary>
        public void Minimize()
        {
            if (State != eWindowState.Maximized)
            {
                OriginalSize = _form.Size;
                OriginalLocation = _form.Location;
            }

            _form.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Determines whether or not the window is currently maximized.
        /// </summary>
        public bool IsMaximized()
        {
            RECT borderThickness = GetBorderThickness((HWND)_form.Handle);
            Size maximizedSize = GetMaximizedSize(borderThickness);

            if (_form.Size == maximizedSize)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether or not the given Size is equal to the maximized size.
        /// </summary>
        public bool IsMaximizedSize(Size testSize)
        {
            RECT borderThickness = GetBorderThickness((HWND)_form.Handle);
            Size maximizedSize = GetMaximizedSize(borderThickness);

            if (maximizedSize.Width == testSize.Width && maximizedSize.Height == testSize.Height)
                return true;

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the size the window is when it's maximized.
        /// </summary>
        private Size GetMaximizedSize(RECT borderThickness)
        {
            return new(SystemInformation.WorkingArea.Width + (borderThickness.left * 2), SystemInformation.WorkingArea.Height + borderThickness.top);
        }

        #endregion

    }
}
