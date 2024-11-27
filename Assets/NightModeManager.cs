using UnityEngine;
using UnityEngine.UI;

public class NightModeManager : MonoBehaviour
{
    public Button nightModeButton;
    public Image darkOverlay;
    public InkDialogueManager dialogueManager;

    private void Start()
    {
        if (darkOverlay != null)
        {
            darkOverlay.gameObject.SetActive(false);
        }

        if (nightModeButton != null)
        {
            nightModeButton.onClick.AddListener(StartNightMode);
        }
    }

    public void StartNightMode()
    {
        Debug.Log("StartNightMode called");
        if (darkOverlay != null)
        {
            darkOverlay.gameObject.SetActive(true);
            Debug.Log("Darkoverlay activated");
        }
        if (dialogueManager != null)
        {
            Debug.Log("Attempting to start night dialogue");
            dialogueManager.StartNightDialogue();
        }
        else
        {
            Debug.LogError("DialogueManager is null!");
        }
    }

    public void EndNightMode()
    {
        if (darkOverlay != null)
        {
            darkOverlay.gameObject.SetActive(false);
        }
    }
}