using BepInEx.Configuration;

namespace SmartEjectors
{
    public static class Config
    {
        private static ConfigFile configFile;

        public static ConfigEntry<bool> enableLockEjector;

        public static void Init(string path)
        {
            configFile = new ConfigFile(path, true);

            enableLockEjector = configFile.Bind("General", "enableLockEjector", true, "When set to true, EM Rail Ejectors automatically stop firing when the local Dyson Sphere has no available cell points.");
        }
    }
}
