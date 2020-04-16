public static class PreferencesManager
{
    public class Preferences
    {
        public int screenX = 1280;
        public int screenY = 720;
        public bool fullScreen = true;
        public float masterVolume = 1;
    }

    public static Preferences current_prefs;

    public static void SaveSettings()
    {
        FileManager.Save(current_prefs, "Prefs", FileManager.Directories.settings);
    }

    public static void LoadSettings()
    {
        current_prefs = FileManager.Load<Preferences>("Prefs", FileManager.Directories.settings);

        if (current_prefs == null)
        {
            current_prefs = new Preferences();
        }
    }
}