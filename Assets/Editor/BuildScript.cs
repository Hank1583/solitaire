using UnityEditor;

public class BuildScript
{
    public static void BuildiOS()
    {
        string buildPath = "build/iOS";
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, BuildTarget.iOS, BuildOptions.None);
    }
}
