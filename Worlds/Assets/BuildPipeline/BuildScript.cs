using UnityEditor;

#if UNITY_EDITOR

public class BuildScript
{
    public static void PerformBuild(string[] scenes, string buildTarget)
    {
        BuildPipeline.BuildPlayer(scenes, buildTarget + $"/{UnityEngine.Application.productName}.exe",
            BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    public static void PerformAssetBundleBuild(string buildTarget)
    {
        BuildPipeline.BuildAssetBundles(buildTarget,
            BuildAssetBundleOptions.ChunkBasedCompression,
            BuildTarget.StandaloneWindows);
    }
}

#endif