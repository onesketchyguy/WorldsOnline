using System.IO;
using System.Linq;
using UnityEngine;

public static class FileManager
{
    public static readonly string AppDir = Application.dataPath + "\\user_data";
    public const string fileExtention = ".json";

    public enum Directories
    {
        saves,
        prefrences
    }

    private static void CheckDirectory(Directories directory)
    {
        string dir = $"{AppDir}/{directory.ToString()}";

        if (Directory.Exists(dir)) return; // The directory exists, so we don't need to make one.

        Directory.CreateDirectory(dir);
    }

    private static void CreateFile(string content, string path)
    {
        File.WriteAllText($"{path}{fileExtention}", content);
    }

    /// <summary>
    /// Will save an abstract item to a specified directory.
    /// </summary>
    /// <param name="dataToSave"></param>
    /// <param name="directory"></param>
    /// <param name="prettyFormat">If true, format the output for readability. If false, format the output for minimum size.</param>
    public static void Save(object dataToSave, string fileName, Directories directory, bool prettyFormat = true)
    {
        CheckDirectory(directory);
        var data = JsonUtility.ToJson(dataToSave, prettyFormat);

        string dir = $"{AppDir}/{directory.ToString()}/{fileName}";
        CreateFile(data, dir);
    }

    public static T Load<T>(string fileName, Directories directory)
    {
        CheckDirectory(directory);
        string dir = $"{AppDir}/{directory.ToString()}/{fileName}{fileExtention}";

        if (File.Exists(dir) == false)
            return default;

        var data = File.ReadAllText(dir);
        return JsonUtility.FromJson<T>(data);
    }

    /// <summary>
    /// Get all the items from a directory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="directory">The directory to use.</param>
    /// <param name="filters">Item extentions to filter out.</param>
    /// <returns></returns>
    public static T[] LoadAllFromDirectory<T>(Directories directory, params string[] filters)
    {
        CheckDirectory(directory);
        var direct = $"{AppDir}/{directory.ToString()}/";

        T[] items = new T[] { };

        void Add(T item)
        {
            var old = items;
            items = new T[old.Length + 1];

            for (int i = 0; i < old.Length; i++)
            {
                items[i] = old[i];
            }

            items[old.Length] = item;
        }

        // get all the files from the directory
        var files = Directory.EnumerateFiles(direct);

        foreach (var file in files)
        {
            var fileName = file.Split('/').LastOrDefault();

            // Check our filters
            bool canRead = true;
            for (int i = 0; i < filters.Length; i++)
                if (fileName.Contains(filters[i])) canRead = false;

            if (canRead == false) continue;

            // Remove the file extention from the name
            fileName = fileName.Split('.').FirstOrDefault();

            // Get the item from our load functionality
            var item = Load<T>(fileName, directory);

            // Add it to our array to return
            Add(item);
        }

        return items;
    }
}