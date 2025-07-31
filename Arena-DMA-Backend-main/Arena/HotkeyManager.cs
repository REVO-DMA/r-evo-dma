using arena_dma_backend.Unity;
using DebounceThrottle;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace arena_dma_backend.Arena
{
    public static class HotkeyManager
    {
        private const int ToggleKeyDebounceTime = 100;
        private const int PressKeyDebounceTime = 16;
        private const int ReleaseKeyDebounceTime = 32;

        public static readonly FrozenDictionary<int, string> HotkeyIndexToName = new Dictionary<int, string>()
        {
            { 0, "None" },
            { 8, "Backspace" },
            { 9, "Tab" },
            { 12, "Clear" },
            { 13, "Return" },
            { 19, "Pause" },
            { 27, "Escape" },
            { 32, "Space" },
            { 33, "Exclaim" },
            { 34, "DoubleQuote" },
            { 35, "Hash" },
            { 36, "Dollar" },
            { 38, "Ampersand" },
            { 39, "Quote" },
            { 40, "LeftParen" },
            { 41, "RightParen" },
            { 42, "Asterisk" },
            { 43, "Plus" },
            { 44, "Comma" },
            { 45, "Minus" },
            { 46, "Period" },
            { 47, "Slash" },
            { 48, "Alpha0" },
            { 49, "Alpha1" },
            { 50, "Alpha2" },
            { 51, "Alpha3" },
            { 52, "Alpha4" },
            { 53, "Alpha5" },
            { 54, "Alpha6" },
            { 55, "Alpha7" },
            { 56, "Alpha8" },
            { 57, "Alpha9" },
            { 58, "Colon" },
            { 59, "Semicolon" },
            { 60, "Less" },
            { 61, "Equals" },
            { 62, "Greater" },
            { 63, "Question" },
            { 64, "At" },
            { 91, "LeftBracket" },
            { 92, "Backslash" },
            { 93, "RightBracket" },
            { 94, "Caret" },
            { 95, "Underscore" },
            { 96, "BackQuote" },
            { 97, "A" },
            { 98, "B" },
            { 99, "C" },
            { 100, "D" },
            { 101, "E" },
            { 102, "F" },
            { 103, "G" },
            { 104, "H" },
            { 105, "I" },
            { 106, "J" },
            { 107, "K" },
            { 108, "L" },
            { 109, "M" },
            { 110, "N" },
            { 111, "O" },
            { 112, "P" },
            { 113, "Q" },
            { 114, "R" },
            { 115, "S" },
            { 116, "T" },
            { 117, "U" },
            { 118, "V" },
            { 119, "W" },
            { 120, "X" },
            { 121, "Y" },
            { 122, "Z" },
            { 127, "Delete" },
            { 256, "Keypad0" },
            { 257, "Keypad1" },
            { 258, "Keypad2" },
            { 259, "Keypad3" },
            { 260, "Keypad4" },
            { 261, "Keypad5" },
            { 262, "Keypad6" },
            { 263, "Keypad7" },
            { 264, "Keypad8" },
            { 265, "Keypad9" },
            { 266, "KeypadPeriod" },
            { 267, "KeypadDivide" },
            { 268, "KeypadMultiply" },
            { 269, "KeypadMinus" },
            { 270, "KeypadPlus" },
            { 271, "KeypadEnter" },
            { 272, "KeypadEquals" },
            { 273, "UpArrow" },
            { 274, "DownArrow" },
            { 275, "RightArrow" },
            { 276, "LeftArrow" },
            { 277, "Insert" },
            { 278, "Home" },
            { 279, "End" },
            { 280, "PageUp" },
            { 281, "PageDown" },
            { 282, "F1" },
            { 283, "F2" },
            { 284, "F3" },
            { 285, "F4" },
            { 286, "F5" },
            { 287, "F6" },
            { 288, "F7" },
            { 289, "F8" },
            { 290, "F9" },
            { 291, "F10" },
            { 292, "F11" },
            { 293, "F12" },
            { 294, "F13" },
            { 295, "F14" },
            { 296, "F15" },
            { 300, "Numlock" },
            { 301, "CapsLock" },
            { 302, "ScrollLock" },
            { 303, "RightShift" },
            { 304, "LeftShift" },
            { 305, "RightControl" },
            { 306, "LeftControl" },
            { 307, "RightAlt" },
            { 308, "LeftAlt" },
            { 309, "RightCommand" },
            { 310, "LeftCommand" },
            { 311, "LeftWindows" },
            { 312, "RightWindows" },
            { 313, "AltGr" },
            { 315, "Help" },
            { 316, "Print" },
            { 317, "SysReq" },
            { 318, "Break" },
            { 319, "Menu" },
            { 323, "Left Mouse" },
            { 324, "Right Mouse" },
            { 325, "Middle Mouse" },
            { 326, "B. Side Mouse" },
            { 327, "F. Side Mouse" },
            { 328, "Mouse5" },
            { 329, "Mouse6" },
        }.ToFrozenDictionary();

        public static ConcurrentDictionary<UnityKeyCode, List<string>> BoundHotkeys = new();

        public static ConcurrentDictionary<string, Action> PressActions = new();
        public static ConcurrentDictionary<string, Action> ReleaseActions = new();

        private static ConcurrentDictionary<string, DebounceDispatcher> ToggleKeyDebouncers = new();
        private static ConcurrentDictionary<string, DebounceDispatcher> PressDebouncers = new();
        private static ConcurrentDictionary<string, DebounceDispatcher> ReleaseDebouncers = new();

        private static HashSet<UnityKeyCode> PressedKeys = new();
        private static HashSet<UnityKeyCode> ToggleKeys = new();

        public static void Start()
        {
            RegisterHotkey((UnityKeyCode)Program.UserConfig.AimHotkey, "aimbot_hotkey");

            // Aimbot engage
            PressActions["aimbot_hotkey"] = () =>
            {
                Aimbot.Engaged = true;
            };
            PressDebouncers["aimbot_hotkey"] = new(PressKeyDebounceTime);
            // Aimbot disengage
            ReleaseActions["aimbot_hotkey"] = () =>
            {
                Aimbot.Engaged = false;
            };
            ReleaseDebouncers["aimbot_hotkey"] = new(ReleaseKeyDebounceTime);
        }

        public static void RegisterHotkey(UnityKeyCode key, string actionID, bool isToggleKey = false)
        {
            // Remove this action from any other hotkeys
            foreach (var hotkey in BoundHotkeys)
            {
                if (hotkey.Value.Remove(actionID))
                {
                    if (hotkey.Value.Count == 0)
                    {
                        // Remove this key from toggleKeys since no actions remain
                        ToggleKeys.Remove(key);
                    }

                    break;
                }
            }

            // Don't associate an action if the hotkey is set to "None"
            if (key == 0) return;

            // Add this action
            BoundHotkeys.AddOrUpdate(key, (newItem) => new List<string>() { actionID }, (key, existing) =>
            {
                existing.Add(actionID);
                return existing;
            });

            if (isToggleKey)
            {
                ToggleKeys.Add(key);
            }
        }

        public static void HandleKeyPress(UnityKeyCode key)
        {
            if (BoundHotkeys.TryGetValue(key, out var actionIDs))
            {
                if (actionIDs == null) return;

                bool pressedKeys_add = false;
                bool pressedKeys_remove = false;

                foreach (var actionID in actionIDs)
                {
                    if (!PressedKeys.Contains(key))
                    {
                        pressedKeys_add = true;

                        if (!ToggleKeys.Contains(key))
                        {
                            PressActions.TryGetValue(actionID, out var action);

                            if (action == null) continue;

                            PressDebouncers[actionID].Debounce(() =>
                            {
                                action.Invoke();
                            });
                        }
                    }
                    else if (ToggleKeys.Contains(key))
                    {
                        pressedKeys_remove = true;

                        ReleaseActions.TryGetValue(actionID, out var action);

                        if (action == null) continue;

                        ToggleKeyDebouncers[actionID].Debounce(() =>
                        {
                            action.Invoke();
                        });
                    }

                    Thread.Sleep(10);
                }

                if (pressedKeys_add) PressedKeys.Add(key);
                else if (pressedKeys_remove) PressedKeys.Remove(key);
            }
            else
            {
                Logger.WriteLine("Invalid hotkey!");
            }
        }

        public static void HandleKeyRelease(UnityKeyCode key)
        {
            // If key is pressed and this is not a toggle key, do the release action
            if (PressedKeys.Contains(key) && !ToggleKeys.Contains(key) && BoundHotkeys.TryGetValue(key, out var actionIDs))
            {
                if (actionIDs == null) return;

                PressedKeys.Remove(key);

                foreach (var actionID in actionIDs)
                {
                    ReleaseActions.TryGetValue(actionID, out var action);

                    if (action == null) continue;

                    ReleaseDebouncers[actionID].Debounce(() =>
                    {
                        action.Invoke();
                    });
                }
            }
        }
    }
}
