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
        public static MultiplayerConfigEntry<int> nodeToSailRatio;

        public static void Init(string path)
        {
            configFile = new ConfigFile(path, true);

            enableLockEjector = new MultiplayerConfigEntry<bool>(configFile.Bind("General", "enableLockEjector", true, "When set to true, EM Rail Ejectors automatically stop firing when the local Dyson Sphere has no available cell points."));
            enableDefaultOrbit = new MultiplayerConfigEntry<bool>(configFile.Bind("General", "enableDefaultOrbit", true, "When set to true, EM Rail Ejectors will be set to target Orbit 1 when placed."));
            nodeToSailRatio = new MultiplayerConfigEntry<int>(configFile.Bind("General", "nodeToSailRatio", 125, new ConfigDescription(
                "Amount of sails to allow in orbit per avaliable node. Set to 0 to disable limit.",
                new AcceptableValueRange<int>(0, 10000)
            )));
        }

        public static void Export(BinaryWriter w)
        {
            w.Write(enableLockEjector.local);
            w.Write(enableDefaultOrbit.local);
            w.Write(nodeToSailRatio.local);
        }

        public static void Import(BinaryReader r)
        {
            enableLockEjector.remote = r.ReadBoolean();
            enableDefaultOrbit.remote = r.ReadBoolean();
            nodeToSailRatio.remote = r.ReadInt32();
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

        public T ActiveValue()
        {
            if (!NebulaModAPI.IsMultiplayerActive) return local;
            if (NebulaModAPI.MultiplayerSession.LocalPlayer.IsHost) return local;
            if (NebulaModAPI.MultiplayerSession.LocalPlayer.IsClient) return remote;

            // Unreachable
            return default(T);
        }
    }
}
