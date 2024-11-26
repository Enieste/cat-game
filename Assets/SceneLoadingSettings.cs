using UnityEngine;

public static class SceneLoadingSettings
{
    public static readonly string[] ScenesWithLoadingScreen = new string[]
    {
        "StartScene",
        "IntroDialogue"
    };

    public static bool NeedsLoadingScreen(string sceneName)
    {
        return System.Array.Exists(ScenesWithLoadingScreen, scene => scene == sceneName);
    }
}