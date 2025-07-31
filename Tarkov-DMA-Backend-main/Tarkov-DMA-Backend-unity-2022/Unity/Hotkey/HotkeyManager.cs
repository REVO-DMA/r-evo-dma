using DebounceThrottle;
using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Tarkov;
using Tarkov_DMA_Backend.Tarkov.Toolkit;
using Tarkov_DMA_Backend.Tarkov.Toolkit.Features;

namespace Tarkov_DMA_Backend.Unity.Hotkey
{
    public static class HotkeyManager
    {
        private const int ToggleKeyDebounceTime = 100;

        public static ConcurrentDictionary<UnityKeyCode, List<string>> BoundHotkeys = new();

        public static ConcurrentDictionary<string, Action> PressActions = new();
        public static ConcurrentDictionary<string, Action> ReleaseActions = new();

        private static ConcurrentDictionary<string, DebounceDispatcher> ToggleKeyDebouncers = new();

        private static HashSet<UnityKeyCode> PressedKeys = new();
        private static HashSet<UnityKeyCode> ToggleKeys = new();

        static HotkeyManager()
        {
            // Add feature toggle actions
            foreach (var featureState in ToolkitManager.FeatureState)
            {
                ReleaseActions[featureState.Key] = () =>
                {
                    bool newState = !ToolkitManager.FeatureState[featureState.Key];
                    ToolkitManager.FeatureState[featureState.Key] = newState;

                    Server.SendFeatureState(featureState.Key, newState);

                    // Invoke immediately
                    var feature = EFTDMA.FeaturesController?.GetFeature(featureState.Key);
                    if (feature == null) return;
                    feature.RunImmediately = true;
                };

                ToggleKeyDebouncers[featureState.Key] = new(ToggleKeyDebounceTime);
            }

            // Instant Zoom
            PressActions["instantZoom_hotkey"] = () =>
            {
                var instantZoom = EFTDMA.FeaturesController?.GetFeature("instantZoom");
                
                if (instantZoom == null || !ToolkitManager.FeatureState["instantZoom"])
                    return;

                InstantZoom.instantZoom_engaged = true;

                instantZoom.RunImmediately = true;
            };
            ReleaseActions["instantZoom_hotkey"] = () =>
            {
                var instantZoom = EFTDMA.FeaturesController?.GetFeature("instantZoom");

                if (instantZoom == null || !ToolkitManager.FeatureState["instantZoom"])
                    return;

                InstantZoom.instantZoom_engaged = false;

                instantZoom.RunImmediately = true;
            };

            // Super Speed
            PressActions["superSpeed_hotkey"] = () =>
            {
                if (SuperSpeed.superSpeedHack_state == false) return;

                // Temp disable speed hack if it's enabled
                var speedHack = EFTDMA.FeaturesController?.GetFeature("speedHack");

                if (speedHack == null || speedHack.CurrentState != true)
                {
                    SuperSpeed.superSpeedHack_engaged = true;
                    return;
                }

                SuperSpeed.superSpeedHack_engaged = true;

                speedHack.OverriddenState = false;
                speedHack.RunImmediately = true;
            };
            ReleaseActions["superSpeed_hotkey"] = () =>
            {
                if (SuperSpeed.superSpeedHack_state == false) return;

                // Re-enable speed hack if it was enabled
                var speedHack = EFTDMA.FeaturesController?.GetFeature("speedHack");

                if (speedHack == null || speedHack.OverriddenState == null)
                {
                    SuperSpeed.superSpeedHack_engaged = false;
                    return;
                }

                SuperSpeed.superSpeedHack_engaged = false;

                speedHack.OverriddenState = null;
                speedHack.RunImmediately = true;
            };

            // Character Manipulation
            PressActions["characterManipulationUp_hotkey"] = () =>
            {
                if (CharacterManipulation.characterManipulation_state == false) return;
                CharacterManipulation.characterManipulationUp_engaged = true;
            };
            ReleaseActions["characterManipulationUp_hotkey"] = () =>
            {
                if (CharacterManipulation.characterManipulation_state == false) return;
                CharacterManipulation.characterManipulationUp_engaged = false;
            };
            PressActions["characterManipulationDown_hotkey"] = () =>
            {
                if (CharacterManipulation.characterManipulation_state == false) return;
                CharacterManipulation.characterManipulationDown_engaged = true;
            };
            ReleaseActions["characterManipulationDown_hotkey"] = () =>
            {
                if (CharacterManipulation.characterManipulation_state == false) return;
                CharacterManipulation.characterManipulationDown_engaged = false;
            };

            // ADS
            PressActions["thirdPerson_adsHotkey"] = () =>
            {
                // Temp disable third person if it's enabled
                var thirdPerson = EFTDMA.FeaturesController?.GetFeature("thirdPerson");

                if (thirdPerson == null || thirdPerson.CurrentState != true) return;

                thirdPerson.OverriddenState = false;
                thirdPerson.RunImmediately = true;
            };

            ReleaseActions["thirdPerson_adsHotkey"] = () =>
            {
                // Re-enable third person if it was enabled
                var thirdPerson = EFTDMA.FeaturesController?.GetFeature("thirdPerson");

                if (thirdPerson == null || thirdPerson.OverriddenState == null) return;

                thirdPerson.OverriddenState = null;
                thirdPerson.RunImmediately = true;
            };

            // Aimbot engage
            PressActions["aimbot_hotkey"] = () =>
            {
                if (Aimbot.AlwaysOn)
                    return;

                Aimbot.Engaged = true;
                Aimbot.Dirty = false;
            };

            ReleaseActions["aimbot_hotkey"] = () =>
            {
                if (Aimbot.AlwaysOn)
                    return;

                Aimbot.Engaged = false;
                Aimbot.Dirty = true;
            };

            // Wide lean vertical above
            PressActions["wideLean_verticalAbove_hotkey"] = () =>
            {
                WideLean.VerticalAbove_hotkey = true;
            };

            ReleaseActions["wideLean_verticalAbove_hotkey"] = () =>
            {
                WideLean.VerticalAbove_hotkey = false;
            };

            // Wide Lean vertical below
            PressActions["wideLean_verticalBelow_hotkey"] = () =>
            {
                WideLean.VerticalBelow_hotkey = true;
            };

            ReleaseActions["wideLean_verticalBelow_hotkey"] = () =>
            {
                WideLean.VerticalBelow_hotkey = false;
            };

            // Silent Loot
            PressActions["silentLoot_hotkey"] = () =>
            {
                // Engage silent loot
                var silentLoot = EFTDMA.FeaturesController?.GetFeature("silentLoot");

                if (silentLoot == null) return;

                silentLoot.OverriddenState = true;
                silentLoot.RunImmediately = true;
            };
            ReleaseActions["silentLoot_hotkey"] = () =>
            {
                // Disengage silent loot
                var silentLoot = EFTDMA.FeaturesController?.GetFeature("silentLoot");

                if (silentLoot == null) return;

                silentLoot.OverriddenState = null;
                silentLoot.RunImmediately = true;
            };

            // Gather AI
            PressActions["gatherAI_hotkey"] = () =>
            {
                // Engage Gather AI
                var gatherAI = EFTDMA.FeaturesController?.GetFeature("gatherAI");

                if (gatherAI == null) return;

                gatherAI.OverriddenState = true;
                gatherAI.RunImmediately = true;
            };

            // ESP Battle Mode
            PressActions["esp_battleModeHotkey"] = () =>
            {
                bool newState = !ESP_Config.BattleModeEnabled;

                ESP_Config.BattleModeEnabled = newState;

                // Send new state to ESP UI
            };
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
            if (key == UnityKeyCode.None) return;

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

                        if (!ToggleKeys.Contains(key) && PressActions.TryGetValue(actionID, out var action))
                        {
                            action.Invoke();
                        }
                    }
                    else if (ToggleKeys.Contains(key) && ReleaseActions.TryGetValue(actionID, out var action))
                    {
                        pressedKeys_remove = true;

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
                    ReleaseActions.TryGetValue(actionID, out Action action);

                    if (action == null) continue;

                    action.Invoke();
                }
            }
        }
    }
}
