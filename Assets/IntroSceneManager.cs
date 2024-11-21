using UnityEngine;
using UnityEngine.SceneManagement;

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
            Debug.LogError("Dialogue Manager not assigned!");
        }
    }

    public void OnDialogueEnd()
    {
        SceneManager.LoadScene("Kitchen");
    }
}