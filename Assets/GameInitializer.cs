using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private static bool hasInitialized = false;

    void Awake()
    {
        if (!hasInitialized)
        {
            UnityEngine.Debug.Log("Initializing LoadingScreen");
            GameObject loadingScreenPrefab = Resources.Load<GameObject>("LoadingScreen");
            if (loadingScreenPrefab != null)
            {
                GameObject loadingScreen = Instantiate(loadingScreenPrefab);
                // Don't activate it - let it handle its own activation
                DontDestroyOnLoad(loadingScreen);
                hasInitialized = true;
                UnityEngine.Debug.Log("LoadingScreen initialized successfully");
            }
            else
            {
                UnityEngine.Debug.LogError("LoadingScreen prefab not found in Resources folder!");
            }
        }
    }
}