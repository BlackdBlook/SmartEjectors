using BepInEx.Configuration;
using NebulaAPI;

namespace SmartEjectors
{
    public static class Config
    {
        private static ConfigFile configFile;

        public static MultiplayerConfigEntry<bool> enableLockEjector;

        public static void Init(string path)
        {
            configFile = new ConfigFile(path, true);

            enableLockEjector = new MultiplayerConfigEntry<bool>(configFile.Bind("General", "enableLockEjector", true, "When set to true, EM Rail Ejectors automatically stop firing when the local Dyson Sphere has no available cell points."));

            NebulaModAPI.OnMultiplayerGameStarted += () =>
            {
                // Reset to local value (overridden by Import() for clients)
                enableLockEjector.reset();
            };

            NebulaModAPI.OnMultiplayerGameEnded += () =>
            {
                // Restore local value
                enableLockEjector.reset();
            };
        }
    }

    public class MultiplayerConfigEntry<T>
    {
        public T local;
        public T inUse;

        public MultiplayerConfigEntry(ConfigEntry<T> entry)
        {
            local = entry.Value;
            inUse = entry.Value;
        }

        public void reset()
        {
            inUse = local;
        }
    }
}
