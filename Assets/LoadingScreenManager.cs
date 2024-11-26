using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen instance;

    public static LoadingScreen Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadingScreen>(true);
            }
            return instance;
        }
    }

    [Header("Loading Screen UI")]
    public GameObject loadingCanvas;
    public Image fadeImage;
    public TextMeshProUGUI loadingText;
    public GameObject loadingIcon;

    [Header("Settings")]
    public float fadeDuration = 1f;

    private bool isLoading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (fadeImage)
            {
                fadeImage.color = Color.black;
                fadeImage.raycastTarget = false;
            }

            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!SceneLoadingSettings.NeedsLoadingScreen(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        if (isLoading) return;

        if (!fadeImage || !loadingCanvas)
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        gameObject.SetActive(true);
        loadingCanvas.SetActive(true);
        StartCoroutine(LoadSceneRoutine(sceneName));
    }


    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        // Initial setup
        fadeImage.color = new Color(0, 0, 0, 0);
        if (loadingText) loadingText.gameObject.SetActive(false);
        if (loadingIcon) loadingIcon.SetActive(false);

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Fade to black
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = Color.black;

        if (loadingText)
        {
            loadingText.gameObject.SetActive(true);
            loadingText.color = Color.white;
        }
        if (loadingIcon) loadingIcon.SetActive(true);

        // Wait until the scene is ready
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Give a small delay to show loading screen
        yield return new WaitForSeconds(0.5f);

        // Hide loading elements but keep fade image
        if (loadingText) loadingText.gameObject.SetActive(false);
        if (loadingIcon) loadingIcon.SetActive(false);

        // Allow scene activation
        asyncLoad.allowSceneActivation = true;

        // Wait for scene to actually finish loading
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Additional small delay to ensure scene is ready
        yield return new WaitForSeconds(0.1f);

        // Fade out
        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        loadingCanvas.SetActive(false);
        gameObject.SetActive(false);
        isLoading = false;
    }
}