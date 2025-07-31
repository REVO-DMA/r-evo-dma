using MemoryPack;
using static MemoryPack.MemoryPackSerializer;

[MemoryPackable]
public partial struct EspWindowPreferencesData
{
    public int X;
    public int Y;
    public int Width;
    public int Height;
}

namespace Tarkov_DMA_Backend.ESP
{
    public class WindowPreferences
    {
        private const string overlayPreferencesFile = "overlayPreferences.evo";
        private static readonly object _lock = new();

        private static readonly EspWindowPreferencesData Defaults = new()
        {
            X = 30,
            Y = 30,
            Width = 600,
            Height = 350,
        };

        public EspWindowPreferencesData Preferences;

        public WindowPreferences()
        {
            Preferences = Load();
        }

        private static EspWindowPreferencesData Load()
        {
            lock (_lock)
            {
                if (!File.Exists(overlayPreferencesFile))
                    File.WriteAllBytes(overlayPreferencesFile, Serialize(Defaults));

                byte[] data = File.ReadAllBytes(overlayPreferencesFile);
                var preferences = Deserialize<EspWindowPreferencesData>(data);

                return preferences;
            }
        }

        public void Save()
        {
            lock (_lock)
            {
                byte[] data = Serialize(Preferences);

                File.WriteAllBytes(overlayPreferencesFile, data);
            }
        }
    }
}
