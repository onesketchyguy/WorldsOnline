namespace Worlds.Saves
{
    public static class PreferencesManager
    {
        public class Preferences
        {
            public int screenX = 1280;
            public int screenY = 720;
            public bool fullScreen = true;
            public bool usingAMPM = true;
            public float masterVolume = 1;
            public int quality = 2;
        }

        public static Preferences current_prefs;

        public static void SaveSettings()
        {
            FileManager.Save(current_prefs, "Prefs", FileManager.Directories.prefrences);
        }

        public static void LoadSettings()
        {
            current_prefs = FileManager.Load<Preferences>("Prefs", FileManager.Directories.prefrences);

            if (current_prefs == null)
            {
                current_prefs = new Preferences();
            }
        }
    }
}