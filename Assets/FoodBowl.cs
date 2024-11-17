using UnityEngine;

public class FoodBowl : MonoBehaviour
{
    private InkDialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<InkDialogueManager>();
        Debug.Log("Food Bowl initialized, DialogueManager found: " + (dialogueManager != null));
    }

    void OnMouseDown()
    {
        Debug.Log("Food Bowl clicked!");
        if (dialogueManager != null)
        {
            if (!dialogueManager.dialoguePanel.activeSelf)
            {
                Debug.Log("Starting food bowl dialogue");
                dialogueManager.StartFoodBowlDialogue();
            }
            else
            {
                Debug.Log("Dialogue panel is already active");
            }
        }
        else
        {
            Debug.LogError("No DialogueManager found!");
        }
    }
}