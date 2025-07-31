using arena_dma_backend.Arena;
using arena_dma_backend.DMA.Collections;
using System.Buffers.Binary;

namespace arena_dma_backend.Unity
{
    public sealed class InputManager : IDisposable
    {
        private readonly ulong _unityInputManager;

        private readonly Thread _mainThread;
        private readonly CancellationTokenSource _cts;
        private readonly CancellationToken _ct;

        private byte[] _keySateBuffer;

        public InputManager(ulong unityBase)
        {
            _unityInputManager = Memory.ReadPtr(unityBase + UnityOffsets.ModuleBase.InputManager, false);

            _cts = new();
            _ct = _cts.Token;

            _mainThread = new(Worker)
            {
                IsBackground = true,
            };

            _mainThread.Start();
        }

        public static bool TryLoad(ulong unityBase, out InputManager inputManager)
        {
            try
            {
                inputManager = new InputManager(unityBase);

                return true;
            }
            catch (Exception ex)
            {
                inputManager = null;
                Logger.WriteLine($"[INPUT MANAGER] -> TryLoad(): {ex}");

                return false;
            }
        }

        private void Worker()
        {
            try
            {
                Logger.WriteLine("[INPUT MANAGER] -> Worker(): Thread starting...");

                while (true)
                {
                    _ct.ThrowIfCancellationRequested();

                    // Only process hotkeys when in raid
                    if (!CameraManager.PlayerIsInRaid)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    UpdateKeyStates();

                    ProcessHotkeys();

                    Thread.Sleep(10);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"[INPUT MANAGER] -> Worker(): CRITICAL ERROR ~ {ex}");
                throw;
            }
            finally
            {
                Logger.WriteLine("[INPUT MANAGER] -> Worker(): Thread stopping...");
            }
        }

        private void ProcessHotkeys()
        {
            foreach (var kvp in HotkeyManager.BoundHotkeys)
            {
                try
                {
                    UnityKeyCode hotkey = kvp.Key;

                    bool isKeyDown = IsKeyDown(hotkey);

                    if (isKeyDown)
                        HotkeyManager.HandleKeyPress(hotkey);
                    else
                        HotkeyManager.HandleKeyRelease(hotkey);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[INPUT MANAGER] -> ProcessHotkeys(): {ex}");
                }
            }
        }

        private void UpdateKeyStates()
        {
            ulong addr = Memory.ReadPtr(_unityInputManager + (UnityOffsets.UnityInputManager.CurrentKeyState + UnityOffsets.DynamicBitsetBase.Bits));

            using var hBuf = new SharedMemory<byte>(0x64);
            var buffer = hBuf.Span;

            Memory.ReadBuffer(addr,  buffer, false);

            _keySateBuffer = buffer.ToArray();
        }

        private bool IsKeyDown(UnityKeyCode keycode)
        {
            uint kc = (uint)keycode;

            int mask = (int)(kc & 0x1F);
            int buffOffset = (int)((kc >> 5) * 0x4);

            Span<byte> ksBuff = _keySateBuffer.AsSpan(buffOffset, 4);
            uint ks = BinaryPrimitives.ReadUInt32LittleEndian(ksBuff);

            return (ks & (1u << mask)) != 0;
        }

        #region IDisposable

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        #endregion
    }

    public enum UnityKeyCode : int
    {
        None = 0,
        Backspace = 8,
        Tab = 9,
        Clear = 12,
        Return = 13,
        Pause = 19,
        Escape = 27,
        Space = 32,
        Exclaim = 33,
        DoubleQuote = 34,
        Hash = 35,
        Dollar = 36,
        Ampersand = 38,
        Quote = 39,
        LeftParen = 40,
        RightParen = 41,
        Asterisk = 42,
        Plus = 43,
        Comma = 44,
        Minus = 45,
        Period = 46,
        Slash = 47,
        Alpha0 = 48,
        Alpha1 = 49,
        Alpha2 = 50,
        Alpha3 = 51,
        Alpha4 = 52,
        Alpha5 = 53,
        Alpha6 = 54,
        Alpha7 = 55,
        Alpha8 = 56,
        Alpha9 = 57,
        Colon = 58,
        Semicolon = 59,
        Less = 60,
        Equals = 61,
        Greater = 62,
        Question = 63,
        At = 64,
        LeftBracket = 91,
        Backslash = 92,
        RightBracket = 93,
        Caret = 94,
        Underscore = 95,
        BackQuote = 96,
        A = 97,
        B = 98,
        C = 99,
        D = 100,
        E = 101,
        F = 102,
        G = 103,
        H = 104,
        I = 105,
        J = 106,
        K = 107,
        L = 108,
        M = 109,
        N = 110,
        O = 111,
        P = 112,
        Q = 113,
        R = 114,
        S = 115,
        T = 116,
        U = 117,
        V = 118,
        W = 119,
        X = 120,
        Y = 121,
        Z = 122,
        Delete = 127,
        Keypad0 = 256,
        Keypad1 = 257,
        Keypad2 = 258,
        Keypad3 = 259,
        Keypad4 = 260,
        Keypad5 = 261,
        Keypad6 = 262,
        Keypad7 = 263,
        Keypad8 = 264,
        Keypad9 = 265,
        KeypadPeriod = 266,
        KeypadDivide = 267,
        KeypadMultiply = 268,
        KeypadMinus = 269,
        KeypadPlus = 270,
        KeypadEnter = 271,
        KeypadEquals = 272,
        UpArrow = 273,
        DownArrow = 274,
        RightArrow = 275,
        LeftArrow = 276,
        Insert = 277,
        Home = 278,
        End = 279,
        PageUp = 280,
        PageDown = 281,
        F1 = 282,
        F2 = 283,
        F3 = 284,
        F4 = 285,
        F5 = 286,
        F6 = 287,
        F7 = 288,
        F8 = 289,
        F9 = 290,
        F10 = 291,
        F11 = 292,
        F12 = 293,
        F13 = 294,
        F14 = 295,
        F15 = 296,
        Numlock = 300,
        CapsLock = 301,
        ScrollLock = 302,
        RightShift = 303,
        LeftShift = 304,
        RightControl = 305,
        LeftControl = 306,
        RightAlt = 307,
        LeftAlt = 308,
        RightApple = 309,
        RightCommand = 309,
        LeftApple = 310,
        LeftCommand = 310,
        LeftWindows = 311,
        RightWindows = 312,
        AltGr = 313,
        Help = 315,
        Print = 316,
        SysReq = 317,
        Break = 318,
        Menu = 319,
        Mouse0 = 323,
        Mouse1 = 324,
        Mouse2 = 325,
        Mouse3 = 326,
        Mouse4 = 327,
        Mouse5 = 328,
        Mouse6 = 329,
    };
}
