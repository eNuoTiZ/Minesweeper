using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BuildMyGame
{
    [MenuItem("File/AutoBuilder/Android")]
    public static void BuildAndroid()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("File/AutoBuilder/Windows")]
    public static void BuildWindows()
    {
        Build(BuildTarget.StandaloneWindows);
    }

    public static void Build(BuildTarget target)
    {
        string[] scenes = { "Assets/Scenes/MainScene_Loading.unity" };

        string outputPath = Environment.GetCommandLineArgs().Last();
        if (string.IsNullOrEmpty(outputPath))
        {
            outputPath = "../../Build/" + target.ToString();
        }
        BuildPipeline.BuildPlayer(scenes, outputPath, target, BuildOptions.None);
    }
}