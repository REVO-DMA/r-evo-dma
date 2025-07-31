using System.ComponentModel;

namespace cs2_dma_esp.Misc
{
    /// <summary>
    /// Encapsulates a timer based on the CreateWaitableTimerEx Win32 API.
    /// </summary>
    public sealed class WaitTimer : IDisposable
    {
        private const uint INFINITE = 0xFFFFFFFF;
        private readonly WaitTimerHandle _handle;
        private bool _autoReset;
        private int _interval;
        private long _dueTime;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WaitTimer() 
        { 
            _handle = new WaitTimerHandle();
        }

        /// <summary>
        /// Sets the 'Wait' interval for the timer. Must set this before calling .Wait()
        /// </summary>
        /// <param name="interval">The interval in which the timer will fire. If set to 0, the timer will only fire once.</param>
        /// <param name="dueTime">The duration of the timer in nanoseconds.</param>
        /// <param name="autoReset">Automatically reset the timer before each Wait call. Must set this to true if 
        /// setting interval to 0.</param>
        public void SetWait(int interval, long dueTime, bool autoReset = false)
        {
            if (autoReset)
            {
                _autoReset = true;
                _interval = interval;
                _dueTime = dueTime;
            }
            else if (!SetWaitableTimer(_handle, ref dueTime, interval, IntPtr.Zero, IntPtr.Zero, false))
                throw new Win32Exception($"Failed to set the wait timer. Error code: {Marshal.GetLastWin32Error()}");
        }

        /// <summary>
        /// Wait on this timer.
        /// </summary>
        /// <param name="timeout">Maximum time in milliseconds to block.</param>
        public void Wait(uint timeout = INFINITE)
        {
            if (_autoReset)
                SetWait(_interval, _dueTime);
            WaitForSingleObject(_handle, timeout);
        }

        /// <summary>
        /// Stops the Timer and cleans up Native Resources.
        /// </summary>
        public void Stop() => Dispose();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetWaitableTimer(SafeHandle hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, bool fResume);

        [DllImport("kernel32.dll")]
        private static extern uint WaitForSingleObject(SafeHandle hHandle, uint dwMilliseconds);

        #region IDisposable
        private bool _disposed = false;

        public void Dispose()
        {
            if (_disposed) 
                return;
            _handle.Dispose();
            _disposed = true;
        }
        #endregion
    }

    public sealed class WaitTimerHandle : SafeHandle
    {
        private const uint CREATE_WAITABLE_TIMER_HIGH_RESOLUTION = 0x00000002;
        private const uint TIMER_ALL_ACCESS = 0x1F0003;

        public WaitTimerHandle() : base(IntPtr.Zero, true)
        {
            this.handle = CreateTimer();
        }

        private static IntPtr CreateTimer()
        {
            // Create a waitable timer
            var hTimer = CreateWaitableTimerEx(IntPtr.Zero, IntPtr.Zero, CREATE_WAITABLE_TIMER_HIGH_RESOLUTION, TIMER_ALL_ACCESS);
            if (hTimer == IntPtr.Zero)
                throw new Win32Exception($"Failed to create the wait timer. Error code: {Marshal.GetLastWin32Error()}");
            return hTimer;
        }

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            try
            {
                CancelWaitableTimer(this.handle);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[WARNING] Unable to cleanup WaitTimer: {ex}");
                return false;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateWaitableTimerEx(IntPtr lpTimerAttributes, IntPtr lpTimerName, uint dwFlags, uint dwDesiredAccess);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CancelWaitableTimer(IntPtr hTimer);
    }
}
