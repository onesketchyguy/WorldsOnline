using System.IO;
using UnityEngine;

public static class FileManager
{
    public static readonly string AppDir = Application.dataPath + "\\user_data";
    public const string fileExtention = ".json";

    public enum Directories
    {
        saves,
        settings
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
    public static void Save(object dataToSave, string fileName, Directories directory)
    {
        CheckDirectory(directory);
        var data = JsonUtility.ToJson(dataToSave);

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

    public static T[] LoadAllFromDirectory<T>(Directories directory)
    {
        CheckDirectory(directory);
        var direct = $"{AppDir}/{directory.ToString()}/";

        var list = new System.Collections.Generic.List<T>();

        foreach (var item in Directory.EnumerateFiles(direct))
        {
            if (File.Exists(item) == false)
                continue;

            var data = File.ReadAllText(item);
            list.Add(JsonUtility.FromJson<T>(data));
        }

        return list.ToArray();
    }
}