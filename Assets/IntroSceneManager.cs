using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroSceneManager : MonoBehaviour
{
    public InkDialogueManager dialogueManager;

    void Start()
    {
        Invoke("StartIntroDialogue", 0.1f);
    }

    void StartIntroDialogue()
    {
        if (dialogueManager != null)
        {
            dialogueManager.InitAndStartDialogue("beginning");
        }
        else
        {
            UnityEngine.Debug.LogError("Dialogue Manager not assigned!");
        }
    }

    public void OnDialogueEnd()
    {
        StartCoroutine(LoadNextSceneRoutine());
    }

    private IEnumerator LoadNextSceneRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        var loadingScreen = LoadingScreen.Instance;
        if (loadingScreen != null)
        {
            if (!loadingScreen.gameObject.activeInHierarchy)
            {
                UnityEngine.Debug.Log("Activating LoadingScreen GameObject");
                loadingScreen.gameObject.SetActive(true);
            }
            loadingScreen.LoadScene("Kitchen");
        }
        else
        {
            UnityEngine.Debug.LogError("LoadingScreen instance not found!");
            SceneManager.LoadScene("Kitchen");
        }
    }
}