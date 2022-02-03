using BepInEx.Configuration;
using NebulaAPI;
using System.IO;

namespace SmartEjectors
{
    public static class Config
    {
        private static ConfigFile configFile;

        public static MultiplayerConfigEntry<bool> enableLockEjector;
        public static MultiplayerConfigEntry<bool> enableDefaultOrbit;

        public static void Init(string path)
        {
            configFile = new ConfigFile(path, true);

            enableLockEjector = new MultiplayerConfigEntry<bool>(configFile.Bind("General", "enableLockEjector", true, "When set to true, EM Rail Ejectors automatically stop firing when the local Dyson Sphere has no available cell points."));
            enableDefaultOrbit = new MultiplayerConfigEntry<bool>(configFile.Bind("General", "enableDefaultOrbit", true, "When set to true, EM Rail Ejectors will be set to target Orbit 1 when placed."));
        }

        public static void Export(BinaryWriter w)
        {
            w.Write(enableLockEjector.local);
            w.Write(enableDefaultOrbit.local);
        }

        public static void Import(BinaryReader r)
        {
            enableLockEjector.remote = r.ReadBoolean();
            enableDefaultOrbit.remote = r.ReadBoolean();
        }

        public static bool isActive(this MultiplayerConfigEntry<bool> entry)
        {
            if (!NebulaModAPI.IsMultiplayerActive) return entry.local;
            if (NebulaModAPI.MultiplayerSession.LocalPlayer.IsHost) return entry.local;
            if (NebulaModAPI.MultiplayerSession.LocalPlayer.IsClient) return entry.remote;

            // Unreachable
            return false;
        }
    }

    public class MultiplayerConfigEntry<T>
    {
        public T local;
        public T remote;

        public MultiplayerConfigEntry(ConfigEntry<T> entry)
        {
            local = entry.Value;
            remote = entry.Value;
        }
    }
}
